//Guild Wars MultiLaunch - Safe and efficient way to launch multiple GWs.
//The Guild Wars executable is never modified, keeping you inline with the tos.
//
//Copyright (C) 2009  IMKey@GuildWarsGuru

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace GWMultiLaunch
{
    public class HandleManager
    {
        #region Native Method Signatures

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(int hObject);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DuplicateHandle(int hSourceProcessHandle,
            int hSourceHandle, int hTargetProcessHandle, out int lpTargetHandle,
            uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, DuplicateOptions dwOptions);

        [DllImport("kernel32.dll")]
        private static extern int GetCurrentProcess();

        [DllImport("kernel32.dll")]
        private static extern int OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, UInt32 dwProcessID);

        [DllImport("ntdll.dll")]
        private static extern NTSTATUS NtQueryObject(int ObjectHandle, OBJECT_INFORMATION_CLASS ObjectInformationClass, 
            IntPtr ObjectInformation, int ObjectInformationLength, out int ReturnLength);

        [DllImport("ntdll.dll")]
        private static extern NTSTATUS NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, 
            IntPtr SystemInformation, int SystemInformationLength, out int ReturnLength);

        #endregion

        #region Structures
        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct SYSTEM_HANDLE_INFORMATION
        {
            public UInt32 OwnerPID;
            public Byte ObjectType;
            public Byte HandleFlags;
            public UInt16 HandleValue;
            public UInt32 ObjectPointer;
            public UInt32 AccessMask;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct OBJECT_BASIC_INFORMATION
        {
            public UInt32 Attributes;
            public UInt32 GrantedAccess;
            public UInt32 HandleCount;
            public UInt32 PointerCount;
            public UInt32 PagedPoolUsage;
            public UInt32 NonPagedPoolUsage;
            public UInt32 Reserved1;
            public UInt32 Reserved2;
            public UInt32 Reserved3;
            public UInt32 NameInformationLength;
            public UInt32 TypeInformationLength;
            public UInt32 SecurityDescriptorLength;
            public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
        }

        ////Not required, for reference
        //[StructLayout(LayoutKind.Sequential)]
        //private struct OBJECT_NAME_INFORMATION
        //{
        //    public UNICODE_STRING   Name;
        //    public Char NameBuffer;
        //}

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct UNICODE_STRING
        {
            public UInt16 Length;           //2 bytes
            public UInt16 MaxLength;        //2 bytes
            public IntPtr Buffer;           //4 bytes
        }

        #endregion

        #region Enumerations

        //DuplicateHandle
        [Flags]
        private enum DuplicateOptions : uint
        {
            DUPLICATE_CLOSE_SOURCE = (0x00000001),
            DUPLICATE_SAME_ACCESS = (0x00000002)
        }

        //OpenProcess
        [Flags]
        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        //NtQueryObject and NtQuerySystemInformation
        [Flags]
        private enum NTSTATUS : uint
        {
            STATUS_SUCCESS              = 0x00000000,
            STATUS_INFO_LENGTH_MISMATCH = 0xC0000004
        } //partial enum, actual set is huge, google ntstatus.h

        //NtQueryObject
        [Flags]
        private enum OBJECT_INFORMATION_CLASS : uint
        {
            ObjectBasicInformation      = 0,
            ObjectNameInformation       = 1,
            ObjectTypeInformation       = 2,
            ObjectAllTypesInformation   = 3,
            ObjectHandleInformation     = 4
        }

        //NtQuerySystemInformation
        [Flags]
        private enum SYSTEM_INFORMATION_CLASS : uint
        {
            SystemHandleInformation = 16
        } //partial enum, actual set is huge, google SYSTEM_INFORMATION_CLASS

        #endregion

        #region Functions

        /// <summary>
        /// Kills the handle whose name contains the nameFragment.
        /// </summary>
        /// <param name="targetProcess"></param>
        /// <param name="handleNamePattern"></param>
        public static bool KillHandle(Process targetProcess, string nameFragment)
        {
            bool success = false;

            //pSysInfoBuffer is a pointer to unmanaged memory
            IntPtr pSysInfoBuffer = GetAllHandles();
            if (pSysInfoBuffer == IntPtr.Zero) return success;

            //Assemble list of SYSTEM_HANDLE_INFORMATION for the specified process
            List<SYSTEM_HANDLE_INFORMATION> processHandles = GetHandles(targetProcess, pSysInfoBuffer);

            //time to free the unmanaged memory
            Marshal.FreeHGlobal(pSysInfoBuffer);

            //Iterate through handles which belong to target process and kill
            int hProcess = OpenProcess(ProcessAccessFlags.DupHandle, false, (UInt32)targetProcess.Id);
            foreach (SYSTEM_HANDLE_INFORMATION handleInfo in processHandles)
            {
                string name = GetHandleName(handleInfo, hProcess);

                if (name.Contains(nameFragment))
                {
                    if (CloseHandleEx(handleInfo.OwnerPID, handleInfo.HandleValue))
                    {
                        success = true;
                    }
                }
            }
            CloseHandle(hProcess);

            return success;
        }

        /// <summary>
        /// Closes a handle that is owned by another process.
        /// </summary>
        /// <param name="processID"></param>
        /// <param name="handleToClose"></param>
        private static bool CloseHandleEx(UInt32 processID, int handleToClose)
        {
            int hProcess = OpenProcess(ProcessAccessFlags.All, false, processID);

            //Kills handle by DUPLICATE_CLOSE_SOURCE option, source is killed while destinationHandle goes to null
            int x;
            bool success = DuplicateHandle(hProcess, handleToClose, 0, out x, 0, false, DuplicateOptions.DUPLICATE_CLOSE_SOURCE);

            CloseHandle(hProcess);

            return success;
        }

        /// <summary>
        /// Convert UNICODE_STRING located at pStringBuffer to a managed string.
        /// </summary>
        /// <param name="pStringBuffer">Pointer to start of UNICODE_STRING struct.</param>
        /// <returns>Managed string.</returns>
        private static string ConvertToString(IntPtr pStringBuffer)
        {
            string handleName;

            // (UNICODE_STRING.Length) + (UNICODE_STRING.MaxLength) + (IntPtr.Size) = offset
            // 2 + 2 + 4 = 8,
            int offsetToUniString = 8;
            handleName = Marshal.PtrToStringUni(new IntPtr(pStringBuffer.ToInt32() + offsetToUniString));

            return handleName;
        }

        /// <summary>
        /// Retrieves all currently active handles for all system processes.
        /// There currently isn't a way to only get it for a specific process.
        /// This relies on NtQuerySystemInformation which exists in ntdll.dll.
        /// </summary>
        /// <returns>Unmanaged IntPtr to the handles (raw data, must be processed)</returns>
        private static IntPtr GetAllHandles()
        {
            int bufferSize = 0x1000;    //initial buffer size of 4096 bytes
            int actualSize;             //will store size of data written to buffer

            IntPtr pSysInfoBuffer = Marshal.AllocHGlobal(bufferSize);

            // Keep trying until buffer is large enough to fit all handles
            NTSTATUS queryResult = NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemHandleInformation, 
                pSysInfoBuffer, bufferSize, out actualSize);
            while (queryResult == NTSTATUS.STATUS_INFO_LENGTH_MISMATCH)
            {
                bufferSize = bufferSize * 2;                //double buffer size
                Marshal.FreeHGlobal(pSysInfoBuffer);
                pSysInfoBuffer = Marshal.AllocHGlobal(bufferSize);
                queryResult = NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemHandleInformation, 
                    pSysInfoBuffer, bufferSize, out actualSize);
            }

            if (queryResult == NTSTATUS.STATUS_SUCCESS)
            {
                return pSysInfoBuffer; //pSysteInfoBuffer will be freed later
            }
            else
            {
                //something went majorly wrong
                Marshal.FreeHGlobal(pSysInfoBuffer);
                return IntPtr.Zero; 
            }
        }

        /// <summary>
        /// Filter out handles which belong to targetProcess.
        /// </summary>
        /// <param name="targetProcess">The process whose handles you want.</param>
        /// <param name="pAllHandles">Pointer to all the system handles.</param>
        /// <returns>List of handles owned by the targetProcess</returns>
        private static List<SYSTEM_HANDLE_INFORMATION> GetHandles(Process targetProcess, IntPtr pAllHandles)
        {
            List<SYSTEM_HANDLE_INFORMATION> processHandles = new List<SYSTEM_HANDLE_INFORMATION>();

            int offset;         //offset from beginning of pAllHandles
            IntPtr pLocation;   //start address of current system handle info block

            int nHandles = Marshal.ReadInt32(pAllHandles);
            SYSTEM_HANDLE_INFORMATION currentHandle = new SYSTEM_HANDLE_INFORMATION();

            // Iterate through all system handles
            for (int i = 0; i < nHandles; i++)
            {
                //first 4 bytes stores number of handles
                //data follows, each set is 16 bytes wide
                offset = 4 + i * 16;
                pLocation = new IntPtr(pAllHandles.ToInt32() + offset);

                // Create structure out of the memory block
                currentHandle = (SYSTEM_HANDLE_INFORMATION)
                    Marshal.PtrToStructure(pLocation, currentHandle.GetType());

                // Add only handles which match the target process id
                if (currentHandle.OwnerPID == (UInt32)targetProcess.Id)
                {
                    processHandles.Add(currentHandle);
                }
            }

            return processHandles;
        }

        /// <summary>
        /// Queries for name of handle.
        /// </summary>
        /// <param name="targetHandleInfo">The handle info.</param>
        /// <param name="hProcess">Open handle to the process which owns that handle.</param>
        /// <returns></returns>
        private static string GetHandleName(SYSTEM_HANDLE_INFORMATION targetHandleInfo, int hProcess)
        {
            //skip special NamedPipe handle (this causes hang up with NtQueryObject function)
            //if (targetHandleInfo.AccessMask == 0x0012019F)
            //{
            //    return String.Empty;
            //}

            int thisProcess = GetCurrentProcess();
            int handle;

            // Need to duplicate handle in this process to be able to access name
            DuplicateHandle(hProcess, targetHandleInfo.HandleValue, thisProcess, out handle, 0, false, DuplicateOptions.DUPLICATE_SAME_ACCESS);

            // Setup buffer to store unicode string
            int bufferSize = GetHandleNameLength(handle);

            // Allocate unmanaged memory to store name
            IntPtr pStringBuffer = Marshal.AllocHGlobal(bufferSize);

            // Query to fill string buffer with name 
            NtQueryObject(handle, OBJECT_INFORMATION_CLASS.ObjectNameInformation, pStringBuffer, bufferSize, out bufferSize);

            // Close this handle
            CloseHandle(handle);    //super important... almost missed this

            // Do the conversion to managed type
            string handleName = ConvertToString(pStringBuffer);

            // Release
            Marshal.FreeHGlobal(pStringBuffer);

            return handleName;
        }

        /// <summary>
        /// Get size of the name info block for that handle.
        /// </summary>
        /// <param name="handle">Handle to process.</param>
        /// <returns></returns>
        private static int GetHandleNameLength(int handle)
        {
            int infoBufferSize = 56;    //56 bytes = size of OBJECT_BASIC_INFORMATION struct
            IntPtr pInfoBuffer = Marshal.AllocHGlobal(infoBufferSize);  //allocate

            // Query for handle's OBJECT_BASIC_INFORMATION
            NtQueryObject(handle, OBJECT_INFORMATION_CLASS.ObjectBasicInformation, pInfoBuffer, infoBufferSize, out infoBufferSize);

            // Map memory to structure
            OBJECT_BASIC_INFORMATION objInfo = new OBJECT_BASIC_INFORMATION();
            objInfo = (OBJECT_BASIC_INFORMATION)Marshal.PtrToStructure(pInfoBuffer, objInfo.GetType());

            Marshal.FreeHGlobal(pInfoBuffer);   //release

            // If the handle has an empty name, we still need to give the buffer a size to map the UNICODE_STRING struct to.
            if (objInfo.NameInformationLength == 0)
            {
                return 0x100;    //reserve 256 bytes, since nameinfolength = 0 for filenames
            }
            else
            {
                return (int)objInfo.NameInformationLength;
            }
        }

        #endregion
    }
}
