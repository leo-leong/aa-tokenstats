//Copyright(c) Microsoft Corporation.

//MIT License

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
//documentation files (the "Software"), to deal in the Software without restriction,
//including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
//sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR
//THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace aa_tokenstats
{
    using DWORD = System.UInt32;
    using LONG = System.Int32;

    internal enum TOKEN_INFORMATION_CLASS
    {
        TokenUser = 1,
        TokenGroups,
        TokenPrivileges,
        TokenOwner,
        TokenPrimaryGroup,
        TokenDefaultDacl,
        TokenSource,
        TokenType,
        TokenImpersonationLevel,
        TokenStatistics,
        TokenRestrictedSids,
        TokenSessionId,
        TokenGroupsAndPrivileges,
        TokenSessionReference,
        TokenSandBoxInert,
        TokenAuditPolicy,
        TokenOrigin
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID
    {
        internal DWORD LowPart;
        internal LONG HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TOKEN_STATISTICS
    {
        internal LUID TokenId;
        internal LUID AuthenticationId;
    }

    internal class NativeAPI
    {


        [DllImport("Kernel32.DLL", EntryPoint = "GetCurrentThread", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 GetCurrentThread();

        [DllImport("Advapi32.DLL", EntryPoint = "OpenThreadToken", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool OpenThreadToken(UInt32 ThreadHandle, Int32 DesiredAccess, bool OpenAsSelf, ref Int32 TokenHandle);

        [DllImport("Kernel32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 GetCurrentProcess();

        [DllImport("Advapi32.DLL", EntryPoint = "OpenProcessToken", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool OpenProcessToken(UInt32 ProcessHandle, Int32 DesiredAccess, ref Int32 TokenHandle);

        [DllImport("Advapi32.DLL", EntryPoint = "GetTokenInformation", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool GetTokenInformation(Int32 TokenHandle, Int32 TokenInformationClass, IntPtr TokenInformation, Int32 TokenInformationLength, ref Int32 ReturnLength);

    }
}
