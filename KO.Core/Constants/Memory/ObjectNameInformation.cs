using System.Runtime.InteropServices;

namespace KO.Core.Structs.Memory
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectNameInformation
    {
        public UnicodeString Name;
    }
}
