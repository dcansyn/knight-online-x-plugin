using KO.Application.Addresses.Handlers;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using System;
using System.Threading.Tasks;

namespace KO.Application.Addresses.Extensions
{
    public static class AddressExtensions
    {
        public static int ReadLong(this Character character, int address)
        {
            uint ret;
            try
            {
                WinApi.ReadProcessMemory(character.Handle, new IntPtr(address), out ret, 4, new IntPtr());
            }
            catch (Exception)
            {
                ret = 0;
            }
            return (int)ret;
        }

        public static float ReadFloat(this Character character, int address)
        {
            float ret;
            try
            {
                WinApi.ReadProcessMemory(character.Handle, new IntPtr(address), out ret, 4, new IntPtr());
            }
            catch (Exception)
            {
                ret = 0;
            }
            return (float)ret;
        }

        public static short ReadShort(this Character character, int address)
        {
            uint ret;
            try
            {
                WinApi.ReadProcessMemory(character.Handle, new IntPtr(address), out ret, 2, new IntPtr());
            }
            catch (Exception)
            {
                ret = 0;
            }
            return (short)ret;
        }

        public static byte ReadByte(this Character character, int address)
        {
            uint ret;
            try
            {
                WinApi.ReadProcessMemory(character.Handle, new IntPtr(address), out ret, 1, new IntPtr());
            }
            catch (Exception)
            {
                ret = 0;
            }
            return (byte)ret;
        }

        public static byte[] ReadByteArray(this Character character, int address, int bufferLen)
        {
            if (bufferLen < 0) bufferLen = 0;
            var buffer = new byte[bufferLen];
            try
            {
                WinApi.ReadProcessMemory(character.Handle, new IntPtr(address), buffer, (uint)bufferLen, out _);
                return buffer;
            }
            catch (Exception) { }
            return buffer;
        }

        public static string ReadString(this Character character, int address, int bufferLen)
        {
            var result = "";
            try
            {
                var outBytes = new byte[bufferLen];
                WinApi.ReadProcessMemory(character.Handle, new IntPtr(address), outBytes, (uint)bufferLen, out _);
                for (int i = 0; i < outBytes.Length; i++)
                    result += Convert.ToChar(outBytes[i]).ToString();
            }
            catch (Exception) { }
            return result;
        }

        public static async Task<IntPtr> GetSendHandle(this Character character)
        {
            var slot = @"\\.\mailslot\usknightonlines" + ((int)WinApi.GetTickCount()).ConvertToDword();
            var handle = WinApi.CreateMailslot(slot, 0, 50, IntPtr.Zero);

            var createFileAddress = (int)WinApi.GetProcAddress(WinApi.GetModuleHandle("kernel32.dll"), "CreateFileA");
            var writeFileAddress = (int)WinApi.GetProcAddress(WinApi.GetModuleHandle("kernel32.dll"), "WriteFile");
            var closeHandleAddress = (int)WinApi.GetProcAddress(WinApi.GetModuleHandle("kernel32.dll"), "CloseHandle");

            var slotHex = slot.ConvertStringToHex();

            var slotArray = slotHex.ConvertStringToByteArray();
            await character.WriteByteArray(character.SendMailAddress + 0x400, slotArray, slotHex.Length / 2);

            var codes = new[]
            {
                "608B4424248905",
                ((long)character.SendMailAddress + 0x100).ConvertToDword(),
                "8B4424288905",
                ((long)character.SendMailAddress + 0x104).ConvertToDword(),
                "3D004000007D3D6A0068800000006A036A006A01680000004068",
                ((long)character.SendMailAddress + 0x400).ConvertToDword(),
                "E8",
                ((long)character.SendMailAddress + 0x33).GetAddressDestination(createFileAddress).ConvertToDword(),
                "83F8FF741C6A005490FF35",
                ((long)character.SendMailAddress + 0x104).ConvertToDword(),
                "FF35",
                ((long)character.SendMailAddress + 0x100).ConvertToDword(),
                "50E8",
                ((long)character.SendMailAddress + 0x4E).GetAddressDestination(writeFileAddress).ConvertToDword(),
                "50E8",
                ((long)character.SendMailAddress + 0x54).GetAddressDestination(closeHandleAddress).ConvertToDword(),
                "616AFF68",
                0.ConvertToDword(),
                "E9",
                ((long)character.SendMailAddress + 0x61).GetAddressDestination(Settings.KO_PTR_SND + 0x7).ConvertToDword()
            };

            var hex = string.Join("", codes);
            var hexArray = hex.ConvertStringToByteArray();
            await character.WriteByteArray(character.SendMailAddress, hexArray, hex.Length / 2);

            hex = "E9" + ((long)Settings.KO_PTR_SND).GetAddressDestination((long)character.SendMailAddress).ConvertToDword();
            hexArray = hex.ConvertStringToByteArray();
            await character.WriteByteArray((IntPtr)Settings.KO_PTR_SND, hexArray, hex.Length / 2);

            return handle;
        }

        public static async Task<IntPtr> GetReceiveHandle(this Character character)
        {
            var slot = @"\\.\mailslot\usknightonliner" + ((int)WinApi.GetTickCount()).ConvertToDword();
            var handle = WinApi.CreateMailslot(slot, 0, 50, IntPtr.Zero);

            var createFileAddress = (int)WinApi.GetProcAddress(WinApi.GetModuleHandle("kernel32.dll"), "CreateFileA");
            var writeFileAddress = (int)WinApi.GetProcAddress(WinApi.GetModuleHandle("kernel32.dll"), "WriteFile");
            var closeHandleAddress = (int)WinApi.GetProcAddress(WinApi.GetModuleHandle("kernel32.dll"), "CloseHandle");

            var slotHex = slot.ConvertStringToHex();

            var slotArray = slotHex.ConvertStringToByteArray();
            await character.WriteByteArray(character.RecvMailAddress + 0x400, slotArray, slotHex.Length / 2);

            var codes = new[]
            {
                "558BEC83C4F433C08945FC33D28955F86A0068800000006A036A006A01680000004068",
                ((long)character.RecvMailAddress + 0x400).ConvertToDword(),
                "E8",
                ((long)character.RecvMailAddress + 0x27).GetAddressDestination(createFileAddress).ConvertToDword(),
                "8945F86A008D4DFC51FF750CFF7508FF75F8E8",
                ((long)character.RecvMailAddress + 0x3E).GetAddressDestination(writeFileAddress).ConvertToDword(),
                "8945F4FF75F8E8",
                ((long)character.RecvMailAddress + 0x49).GetAddressDestination(closeHandleAddress).ConvertToDword(),
                "8BE55DC3"
            };

            var hex = string.Join("", codes);
            var hexArray = hex.ConvertStringToByteArray();
            await character.WriteByteArray(character.RecvMailAddress, hexArray, hex.Length / 2);

            var pointer = character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG - 0x14)) + 0x8;

            codes = new[]
            {
                "558BEC83C4F8538B450883C0048B108955FC8B4D0883C1088B018945F8FF75FCFF75F8",
                "E8",
                ((long)character.RecvHookAddress + 0x23).GetAddressDestination((long)character.RecvMailAddress).ConvertToDword(),
                "83C4088B0D",
                (Settings.KO_PTR_DLG - 0x14).ConvertToDword(),
                "FF750CFF7508B8",
                character.ReadLong(pointer).ConvertToDword(),
                "FFD05B59595DC20800"
            };

            hex = string.Join("", codes);
            hexArray = hex.ConvertStringToByteArray();
            await character.WriteByteArray(character.RecvHookAddress, hexArray, hex.Length / 2);

            var addressHex = ((long)character.RecvHookAddress).ConvertToDword();
            await character.WriteByteArray((IntPtr)pointer, addressHex.ConvertStringToByteArray(), addressHex.Length / 2);

            return handle;
        }
    }
}
