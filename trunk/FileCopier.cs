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
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct SHFILEOPSTRUCT
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

        public enum FILE_OP_TYPE : uint
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

        public static bool CopyFiles(string from, string to)
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

            int result = SHFileOperation(ref lpFileOp);
            if (result == 0)
            {
                success = !lpFileOp.fAnyOperationsAborted;
            }

            return success;
        }

        public static string TranslateStringList(List<string> filenames)
        {
            string result = string.Empty;

            foreach (string filename in filenames)
            {
                result = result + filename + "\0";
            }

            result = result + "\0";     //needs to be doubly null terminated

            return result;
        }

    }

}
