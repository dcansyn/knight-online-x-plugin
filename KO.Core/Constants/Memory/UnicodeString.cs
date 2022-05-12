using System;
using System.Runtime.InteropServices;

namespace KO.Core.Structs.Memory
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct UnicodeString
    {
        public ushort Length;
        public ushort MaximumLength;
        public IntPtr Buffer;
    }
}
