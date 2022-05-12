using System.Runtime.InteropServices;

namespace KO.Core.Structs.Memory
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GenericMapping
    {
        public int GenericRead;
        public int GenericWrite;
        public int GenericExecute;
        public int GenericAll;
    }
}
