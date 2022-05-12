using System.Runtime.InteropServices;

namespace KO.Core.Structs.Memory
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SystemHandleInformation
    {
        public int ProcessID;
        public byte ObjectTypeNumber;
        public byte Flags;
        public ushort Handle;
        public int Object_Pointer;
        public uint GrantedAccess;
    }
}
