using KO.Core.Constants;
using KO.Core.Enums.Memory;
using KO.Core.Structs.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KO.Core.Helpers.Memory
{
    public class MemoryHelper : WinApi
    {
        public static IntPtr GetWindowHandleByTitle(string title)
        {
            return Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == title).MainWindowHandle;
        }

        public static Process[] GetProcesses(string name)
        {
            return Process.GetProcesses()
                .Where(x => x.ProcessName.Contains(name))
                .ToArray();
        }

        public static IntPtr GetHandle(string title)
        {
            GetWindowThreadProcessId(FindWindow(null, title), out int pid);
            return OpenProcess(ProcessAccessFlag.All, false, pid);
        }

        public static void Wait(int miliseconds)
        {
            var now = DateTime.Now.AddMilliseconds(miliseconds);

            while (now > DateTime.Now)
                Application.DoEvents();
        }

        public static void Kill(string title, bool contains = true)
        {
            Process.GetProcesses()
                .Where(x => contains ? x.MainWindowTitle.Contains(title) : x.MainWindowTitle == title)
                .ToList().ForEach(item =>
                {
                    item.Kill();
                });
        }

        public static bool CheckProcessByName(string name)
        {
            return Process.GetProcesses().Any(x => x.ProcessName == name);
        }

        public static bool CheckProcessByTitle(string title)
        {
            return Process.GetProcesses().Any(x => x.MainWindowTitle == title);
        }

        public static async Task<bool> OpenParentProcess(string path, string name, string arguments)
        {
            var processor = Path.Combine(Environment.CurrentDirectory, "KO.Processor.exe");
            var fileInfo = new FileInfo(processor);
            if (!fileInfo.Exists) return await Task.FromResult(false);

            using (var managementClass = new ManagementClass("Win32_Process"))
            {
                var processInfo = new ManagementClass("Win32_ProcessStartup");

                var parameters = managementClass.GetMethodParameters("Create");

                parameters["CommandLine"] = $"{processor} --p \"{path}\" --n \"{name}\" --a \"{arguments}\"";
                parameters["CurrentDirectory"] = fileInfo.Directory.FullName;
                parameters["ProcessStartupInformation"] = processInfo;

                managementClass.InvokeMethod("Create", parameters, null);

                return await Task.FromResult(true);
            }
        }

        public static void CloseMutant(Process process, string processName)
        {
            try
            {
                var handles = GetHandles(process, "Mutant", processName);

                if (handles.Any())
                    foreach (var handle in handles)
                    {
                        IntPtr ipHandle = IntPtr.Zero;
                        DuplicateHandle(Process.GetProcessById(handle.ProcessID).Handle, handle.Handle, GetCurrentProcess(), out ipHandle, 0, false, 0x1);
                        CloseHandle(ipHandle);
                    }
            }
            catch (Exception) { }
        }

        #region [Close Mutant]
        // https://stackoverflow.com/q/6808831/3024129
        private static bool Is64Bits()
        {
            return Marshal.SizeOf(typeof(IntPtr)) == 8;
        }

        private static SystemHandleInformation[] GetHandles(Process process = null, string IN_strObjectTypeName = null, string IN_strObjectName = null)
        {
            int nHandleInfoSize = 0x10000;
            IntPtr ipHandlePointer = Marshal.AllocHGlobal(nHandleInfoSize);
            int nLength = 0;
            while ((_ = NtQuerySystemInformation(16, ipHandlePointer, nHandleInfoSize, ref nLength)) == 0xC0000004)
            {
                nHandleInfoSize = nLength;
                Marshal.FreeHGlobal(ipHandlePointer);
                ipHandlePointer = Marshal.AllocHGlobal(nLength);
            }

            byte[] baTemp = new byte[nLength];
            Marshal.Copy(ipHandlePointer, baTemp, 0, nLength);
            IntPtr ipHandle;

            long lHandleCount;
            if (Is64Bits())
            {
                lHandleCount = Marshal.ReadInt64(ipHandlePointer);
                ipHandle = new IntPtr(ipHandlePointer.ToInt64() + 8);
            }
            else
            {
                lHandleCount = Marshal.ReadInt32(ipHandlePointer);
                ipHandle = new IntPtr(ipHandlePointer.ToInt32() + 4);
            }

            SystemHandleInformation shHandle;
            List<SystemHandleInformation> lstHandles = new List<SystemHandleInformation>();

            for (long lIndex = 0; lIndex < lHandleCount; lIndex++)
            {
                shHandle = new SystemHandleInformation();
                if (Is64Bits())
                {
                    shHandle = (SystemHandleInformation)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                    ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle) + 8);
                }
                else
                {
                    ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle));
                    shHandle = (SystemHandleInformation)Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                }

                if (process != null)
                {
                    if (shHandle.ProcessID != process.Id) continue;
                }

                if (IN_strObjectTypeName != null)
                {
                    var strObjectTypeName = GetObjectTypeName(shHandle, Process.GetProcessById(shHandle.ProcessID));
                    if (strObjectTypeName != IN_strObjectTypeName) continue;
                }

                if (IN_strObjectName != null)
                {
                    var strObjectName = GetObjectName(shHandle, Process.GetProcessById(shHandle.ProcessID));
                    if (string.IsNullOrEmpty(strObjectName) || !strObjectName.Contains(IN_strObjectName)) continue;
                }

                lstHandles.Add(shHandle);
            }
            return lstHandles.ToArray();
        }

        private static string GetObjectName(SystemHandleInformation shHandle, Process process)
        {
            var m_ipProcessHwnd = OpenProcess(ProcessAccessFlag.All, false, process.Id);
            var objBasic = new ObjectBasicInformation();
            var objObjectName = new ObjectNameInformation();
            int nLength = 0;
            if (!DuplicateHandle(m_ipProcessHwnd, shHandle.Handle, GetCurrentProcess(),
                                          out IntPtr ipHandle, 0, false, 0x2))
                return null;

            var ipBasic = Marshal.AllocHGlobal(Marshal.SizeOf(objBasic));
            NtQueryObject(ipHandle, (int)ObjectInformationClass.ObjectBasicInformation,
                                   ipBasic, Marshal.SizeOf(objBasic), ref nLength);
            objBasic = (ObjectBasicInformation)Marshal.PtrToStructure(ipBasic, objBasic.GetType());
            Marshal.FreeHGlobal(ipBasic);


            nLength = objBasic.NameInformationLength;

            IntPtr ipObjectName = Marshal.AllocHGlobal(nLength);
            while ((uint)(_ = NtQueryObject(
                     ipHandle, (int)ObjectInformationClass.ObjectNameInformation,
                     ipObjectName, nLength, ref nLength))
                   == 0xC0000004)
            {
                Marshal.FreeHGlobal(ipObjectName);
                ipObjectName = Marshal.AllocHGlobal(nLength);
            }
            objObjectName = (ObjectNameInformation)Marshal.PtrToStructure(ipObjectName, objObjectName.GetType());

            IntPtr ipTemp;
            if (Is64Bits())
            {
                ipTemp = new IntPtr(Convert.ToInt64(objObjectName.Name.Buffer.ToString(), 10) >> 32);
            }
            else
            {
                ipTemp = objObjectName.Name.Buffer;
            }

            if (ipTemp != IntPtr.Zero)
            {

                byte[] baTemp2 = new byte[nLength];
                try
                {
                    Marshal.Copy(ipTemp, baTemp2, 0, nLength);

                    string strObjectName = Marshal.PtrToStringUni(Is64Bits() ?
                                               new IntPtr(ipTemp.ToInt64()) :
                                               new IntPtr(ipTemp.ToInt32()));
                    return strObjectName;
                }
                catch (AccessViolationException)
                {
                    return null;
                }
                finally
                {
                    Marshal.FreeHGlobal(ipObjectName);
                    CloseHandle(ipHandle);
                }
            }
            return null;
        }

        private static string GetObjectTypeName(SystemHandleInformation shHandle, Process process)
        {
            IntPtr m_ipProcessHwnd = OpenProcess(ProcessAccessFlag.All, false, process.Id);
            var objBasic = new ObjectBasicInformation();
            var objObjectType = new ObjectTypeInformation();
            int nLength = 0;
            if (!DuplicateHandle(m_ipProcessHwnd, shHandle.Handle,
                                          GetCurrentProcess(), out IntPtr ipHandle,
                                          0, false, 0x2))
                return null;

            IntPtr ipBasic = Marshal.AllocHGlobal(Marshal.SizeOf(objBasic));
            NtQueryObject(ipHandle, (int)ObjectInformationClass.ObjectBasicInformation,
                                   ipBasic, Marshal.SizeOf(objBasic), ref nLength);
            objBasic = (ObjectBasicInformation)Marshal.PtrToStructure(ipBasic, objBasic.GetType());
            Marshal.FreeHGlobal(ipBasic);

            IntPtr ipObjectType = Marshal.AllocHGlobal(objBasic.TypeInformationLength);
            nLength = objBasic.TypeInformationLength;
            while ((uint)(_ = NtQueryObject(
                ipHandle, (int)ObjectInformationClass.ObjectTypeInformation, ipObjectType,
                  nLength, ref nLength)) ==
                0xC0000004)
            {
                Marshal.FreeHGlobal(ipObjectType);
                ipObjectType = Marshal.AllocHGlobal(nLength);
            }

            objObjectType = (ObjectTypeInformation)Marshal.PtrToStructure(ipObjectType, objObjectType.GetType());
            IntPtr ipTemp;
            if (Is64Bits())
            {
                ipTemp = new IntPtr(Convert.ToInt64(objObjectType.Name.Buffer.ToString(), 10) >> 32);
            }
            else
            {
                ipTemp = objObjectType.Name.Buffer;
            }

            string strObjectTypeName = Marshal.PtrToStringUni(ipTemp, objObjectType.Name.Length >> 1);
            Marshal.FreeHGlobal(ipObjectType);
            return strObjectTypeName;
        }
        #endregion
    }
}
