using System.Runtime.InteropServices;

namespace KO.Core.Structs.Memory
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectTypeInformation
    {
        public UnicodeString Name;
        public int ObjectCount;
        public int HandleCount;
        public int Reserved1;
        public int Reserved2;
        public int Reserved3;
        public int Reserved4;
        public int PeakObjectCount;
        public int PeakHandleCount;
        public int Reserved5;
        public int Reserved6;
        public int Reserved7;
        public int Reserved8;
        public int InvalidAttributes;
        public GenericMapping GenericMapping;
        public int ValidAccess;
        public byte Unknown;
        public byte MaintainHandleDatabase;
        public int PoolType;
        public int PagedPoolUsage;
        public int NonPagedPoolUsage;
    }
}
