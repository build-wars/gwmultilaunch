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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GWMultiLaunch
{
    public class FileCopier
    {
        #region Native Method Signatures

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);

        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public FILE_OP_TYPE wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            public FILE_OP_FLAGS fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszProgressTitle;
        }

        #endregion

        #region Enumerations

        private enum FILE_OP_TYPE : uint
        {
            FO_MOVE     = 0x0001,
            FO_COPY     = 0x0002,
            FO_DELETE   = 0x0003,
            FO_RENAME   = 0x0004,
        }

        [Flags]
        private enum FILE_OP_FLAGS : ushort
        {
            FOF_MULTIDESTFILES          = 0x0001,
            FOF_CONFIRMMOUSE            = 0x0002,
            FOF_SILENT                  = 0x0004,  
            FOF_RENAMEONCOLLISION       = 0x0008,
            FOF_NOCONFIRMATION          = 0x0010,  
            FOF_WANTMAPPINGHANDLE       = 0x0020,  
            FOF_ALLOWUNDO               = 0x0040,
            FOF_FILESONLY               = 0x0080,  
            FOF_SIMPLEPROGRESS          = 0x0100,  
            FOF_NOCONFIRMMKDIR          = 0x0200, 
            FOF_NOERRORUI               = 0x0400, 
            FOF_NOCOPYSECURITYATTRIBS   = 0x0800, 
            FOF_NORECURSION             = 0x1000, 
            FOF_NO_CONNECTED_ELEMENTS   = 0x2000,
            FOF_WANTNUKEWARNING         = 0x4000,
            FOF_NORECURSEREPARSE        = 0x8000,
        }

        #endregion

        #region Functions

        public static bool CopyFiles(List<string> from, List<string> to)
        {
            return CopyFiles(ConstructFilenamesString(from), ConstructFilenamesString(to));
        }

        private static bool CopyFiles(string from, string to)
        {
            bool success = false;

            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT();
            lpFileOp.hwnd = IntPtr.Zero;
            lpFileOp.wFunc = FILE_OP_TYPE.FO_COPY;
            lpFileOp.pFrom = from;
            lpFileOp.pTo = to;
            lpFileOp.fFlags = FILE_OP_FLAGS.FOF_NORECURSION | FILE_OP_FLAGS.FOF_NOCONFIRMMKDIR | FILE_OP_FLAGS.FOF_MULTIDESTFILES;
            lpFileOp.fAnyOperationsAborted = false;
            lpFileOp.hNameMappings = IntPtr.Zero;
            lpFileOp.lpszProgressTitle = string.Empty;

            //do copy operation and store result
            int result = SHFileOperation(ref lpFileOp);

            //we also need to check for user aborted operations
            if (result == 0)
            {
                success = !lpFileOp.fAnyOperationsAborted;
            }

            return success;
        }

        private static string ConstructFilenamesString(List<string> filenames)
        {
            //Filename limit for Windows XP is 255 chars
            //255 * number of files should give more than enough initial size
            System.Text.StringBuilder result = new System.Text.StringBuilder(255 * filenames.Count);

            foreach (string file in filenames)
            {
                result.Append(file);

                //each filename must be separated by a null
                result.Append("\0");
            }

            result.Append("\0");     //needs to be doubly null terminated

            return result.ToString();
        }

        #endregion
    }

}
