using System.Runtime.InteropServices;
using System;

namespace Nphies.Core.Documents.VIDA3
{
    public class LogUser
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(
       string lpszUsername,
       string lpszDomain,
       string lpszPassword,
       int dwLogonType,
       int dwLogonProvider,
       out IntPtr phToken
       );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

    }
}
