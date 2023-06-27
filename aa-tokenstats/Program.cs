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
    internal class Program
    {
        static void Main(string[] args)
        {
            Int32 hToken;
            hToken = GetCurrentToken();
            PrintTokenStats(hToken);
        }

        static Int32 GetCurrentToken()
        {
            const Int32 TOKEN_QUERY = 8;
            const Int32 ERROR_NO_TOKEN = 1008;

            Int32 hToken;
            hToken = 0;
            if (!((NativeAPI.OpenThreadToken(NativeAPI.GetCurrentThread(), TOKEN_QUERY, true, out hToken))))
            {
                if ((Marshal.GetLastWin32Error() == ERROR_NO_TOKEN))
                {
                    Console.WriteLine("No thread token found, proceeding to open process token.");
                    if (!((NativeAPI.OpenProcessToken(NativeAPI.GetCurrentProcess(), TOKEN_QUERY, out hToken))))
                    {
                        Console.WriteLine("OpenProcessToken failed " + Marshal.GetLastWin32Error().ToString());
                    }
                }
                else
                {
                    Console.WriteLine("OpenThreadToken failed " + Marshal.GetLastWin32Error().ToString());
                }
            }
            return hToken;
        }

        static void PrintTokenStats(Int32 hToken)
        {
            Int32 length;
            IntPtr ptr;
            TOKEN_STATISTICS tokenstats = new TOKEN_STATISTICS();

            NativeAPI.GetTokenInformation(hToken, (int)TOKEN_INFORMATION_CLASS.TokenStatistics, IntPtr.Zero, 0, out length);
            ptr = Marshal.AllocHGlobal(length);
            if (!((NativeAPI.GetTokenInformation(hToken, (int)TOKEN_INFORMATION_CLASS.TokenStatistics, ptr, length, out length))))
            {
                Marshal.FreeHGlobal(ptr);
                return;
            }
            tokenstats = ((TOKEN_STATISTICS)(Marshal.PtrToStructure(ptr, tokenstats.GetType())));
            Console.WriteLine("TokenId: " + tokenstats.TokenId.LowPart + "\tTokenId: " + tokenstats.AuthenticationId.LowPart);
        }
    }
}
