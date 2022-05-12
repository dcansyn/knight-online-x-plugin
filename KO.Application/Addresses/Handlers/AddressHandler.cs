using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using System;
using System.Threading.Tasks;

namespace KO.Application.Addresses.Handlers
{
    public static class AddressHandler
    {
        public static async Task WriteByte(this Character character, int address, int val)
        {
            try
            {
                var byteVal = BitConverter.GetBytes(val);
                WinApi.VirtualProtectEx(character.Handle, (IntPtr)address, (UIntPtr)byteVal.Length, 0x40, out uint oldProtection);
                WinApi.WriteProcessMemory(character.Handle, (IntPtr)address, byteVal, 1, out int ret);
                WinApi.VirtualProtectEx(character.Handle, (IntPtr)address, (UIntPtr)byteVal.Length, oldProtection, out oldProtection);
            }
            catch (Exception) { }

            await Task.CompletedTask;
        }

        public static async Task WriteByteArray(this Character character, IntPtr address, byte[] bValue, int length)
        {
            try
            {
                WinApi.VirtualProtectEx(character.Handle, address, (UIntPtr)length, 0x40, out uint oldProtection);
                WinApi.WriteProcessMemory(character.Handle, address, bValue, length, out int ret);
                WinApi.VirtualProtectEx(character.Handle, address, (UIntPtr)length, oldProtection, out oldProtection);
            }
            catch (Exception) { }

            await Task.CompletedTask;
        }

        public static async Task WriteByteArray(this Character character, int address, IntPtr bValue, int length)
        {
            try
            {
                WinApi.VirtualProtectEx(character.Handle, (IntPtr)address, (UIntPtr)length, 0x40, out uint oldProtection);
                WinApi.WriteProcessMemory(character.Handle, (IntPtr)address, bValue, length, out IntPtr ret);
                WinApi.VirtualProtectEx(character.Handle, (IntPtr)address, (UIntPtr)length, oldProtection, out oldProtection);
            }
            catch (Exception) { }

            await Task.CompletedTask;
        }

        public static async Task WriteLong(this Character character, int address, int val)
        {
            try
            {
                var byteVal = BitConverter.GetBytes(val);
                WinApi.VirtualProtectEx(character.Handle, (IntPtr)address, (UIntPtr)byteVal.Length, 0x40, out uint oldProtection);
                WinApi.WriteProcessMemory(character.Handle, (IntPtr)address, byteVal, 4, out int ret);
                WinApi.VirtualProtectEx(character.Handle, (IntPtr)address, (UIntPtr)byteVal.Length, oldProtection, out oldProtection);
            }
            catch (Exception) { }

            await Task.CompletedTask;
        }

        public static async Task WriteFloat(this Character character, int address, float val)
        {
            try
            {
                var byteVal = BitConverter.GetBytes(val);
                WinApi.VirtualProtectEx(character.Handle, (IntPtr)address, (UIntPtr)byteVal.Length, 0x40, out uint oldProtection);
                WinApi.WriteProcessMemory(character.Handle, (IntPtr)address, byteVal, 4, out int ret);
                WinApi.VirtualProtectEx(character.Handle, (IntPtr)address, (UIntPtr)byteVal.Length, oldProtection, out oldProtection);
            }
            catch (Exception) { }

            await Task.CompletedTask;
        }

        public static async Task WriteCode(this Character character, int address, string code)
        {
            var exByteCode = code.ConvertStringToByteArray();
            await character.WriteByteArray((IntPtr)address, exByteCode, exByteCode.Length);
        }

        public static async Task ExecuteCode(this Character character, IntPtr address, params string[] codes)
        {
            var code = string.Join("", codes);

            var exByteCode = code.ConvertStringToByteArray();
            await character.WriteByteArray(address, exByteCode, exByteCode.Length);
            var hThread = WinApi.CreateRemoteThread(character.Handle, IntPtr.Zero, IntPtr.Zero, address, IntPtr.Zero, IntPtr.Zero, out _);
            WinApi.WaitForSingleObject(hThread, 0xFFFF);
            WinApi.CloseHandle(hThread);

            await Task.CompletedTask;
        }

        public static async Task ExecuteCode(this Character character, params string[] codes)
        {
            var code = string.Join("", codes);

            var address = WinApi.VirtualAllocEx(character.Handle, IntPtr.Zero, 0x1, 0x1000, 0x40);
            var exByteCode = code.ConvertStringToByteArray();
            WinApi.WriteProcessMemory(character.Handle, address, exByteCode, exByteCode.Length, out _);
            var hThread = WinApi.CreateRemoteThread(character.Handle, IntPtr.Zero, IntPtr.Zero, address, IntPtr.Zero, IntPtr.Zero, out _);
            WinApi.WaitForSingleObject(hThread, 0xFFFF);
            WinApi.CloseHandle(hThread);
            WinApi.VirtualFreeEx(character.Handle, address, 0x0, 0x8000);

            await Task.CompletedTask;
        }

        public static async Task Send(this Character character, params string[] codes)
        {
            var packet = string.Join("", codes);

            if (packet.Length % 2 != 0)
                return;

            var bPacket = packet.ConvertStringToByteArray();
            await character.WriteByteArray(character.SendAddress, bPacket, bPacket.Length);

            await character.ExecuteCode(character.RemoteAddress, "608B0D", Settings.KO_PTR_PKT.ConvertToDword(), "68" + bPacket.Length.ConvertToDword(), "68", ((int)character.SendAddress).ConvertToDword(), "BF", Settings.KO_PTR_SND.ConvertToDword(), "FFD761C3");
        }
    }
}
