using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KO.Infrastructure.Services.Tables
{
    public class TableService
    {
        public List<List<string>> GetList(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(x => x.ItemArray?.Select(y => y?.ToString())?.ToList()).ToList();
        }

        public DataTable GetTable(string path)
        {
            var file = new FileInfo(path);
            if (!file.Exists)
                return null;

            var buffer = File.ReadAllBytes(file.FullName);
            Decode(ref buffer);
            return GetTableFromDecodedBytes(buffer, file.Name);
        }

        public List<string> GetTables(string path)
        {
            return Directory
                .GetFiles(path)
                .Where(x => x.EndsWith(".tbl"))
                .Select(x => new FileInfo(x).Name)
                .ToList();
        }

        public bool SaveTable(DataTable data, string path)
        {
            return Save(data, path);
        }

        #region [Encryption & Decryption]
        /* Encryption & decryption codes are taken from this repository; 
         * https://github.com/mustafakemalgilor/ko-table-editor
         */
        private static class ColumnTypes
        {
            public const int SignedByte = 1;
            public const int UnsignedByte = 2;
            public const int SignedShort = 3;
            public const int UnsignedShort = 4;
            public const int SignedInteger = 5;
            public const int UnsignedInteger = 6;
            public const int String = 7;
            public const int Float = 8;
            public const int Double = 9;
            public const int SignedLong = 10;
            public const int UnsignedLong = 11;

            public static string GetColumnTypeNameFromFullName(string name)
            {
                switch (name)
                {
                    case "System.UInt64":
                        return "(unsigned long)";
                    case "System.Int64":
                        return "(signed long)";
                    case "System.Double":
                        return "(double)";
                    case "System.Single":
                        return "(float)";
                    case "System.UInt32":
                        return "(unsigned integer)";
                    case "System.Int32":
                        return "(signed integer)";
                    case "System.UInt16":
                        return "(unsigned short)";
                    case "System.Int16":
                        return "(signed short)";
                    case "System.SByte":
                        return "(signed byte)";
                    case "System.Byte":
                        return "(unsigned byte)";
                    case "System.String":
                        return "(string)";
                    default:
                        return "(unknown)";
                }
            }

            public static int GetColumnTypeFromFullName(string name)
            {
                switch (name)
                {
                    case "System.UInt64":
                        return UnsignedLong;
                    case "System.Int64":
                        return SignedLong;
                    case "System.Double":
                        return Double;
                    case "System.Single":
                        return Float;
                    case "System.UInt32":
                        return UnsignedInteger;
                    case "System.Int32":
                        return SignedInteger;
                    case "System.UInt16":
                        return UnsignedShort;
                    case "System.Int16":
                        return SignedShort;
                    case "System.SByte":
                        return SignedByte;
                    case "System.Byte":
                        return UnsignedByte;
                    case "System.String":
                        return String;
                    default:
                        Trace.Assert(false, "GetColumnTypeFromFullName() - Unknown type.");
                        return -1;
                }
            }
        }
        private static ushort _volatileKey = 0x0418;
        private const ushort CipherKey1 = 0x8041;
        private const ushort CipherKey2 = 0x1804;
        private static readonly byte[] _key =
           {
            0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 1,
            1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1,
            1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1,
            1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
            1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0,
            1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1,
            1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 1, 1, 1, 0, 1,
            0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0, 0,
            0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0,
            1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0, 1,
            0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1,
            0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
            1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 1,
            1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0,
            1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0,
            1, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 0,
            1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1,
            0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1,
            0, 1, 0, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1,
            1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0,
            1, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0,
            1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0,
            0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1,
            0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0,
            1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0,
            0, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1,
            1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0,
            1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1,
            1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1,
            0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1,
            1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 1, 1, 1,
            1, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1,
            1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1,
            1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1,
            0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0,
            0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0,
            1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0,
            1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1,
            0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1,
            0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 1, 0,
            1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0,
            0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1,
            0, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0,
            1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1,
            1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0
        };
        private static readonly byte[] ExpansionOperationMatrix =
        {
                32, 1,  2,  3,  4,  5,
                4,  5,  6,  7,  8,  9,
                8,  9,  10, 11, 12, 13,
                12, 13, 14, 15, 16, 17,
                16, 17, 18, 19, 20, 21,
                20, 21, 22, 23, 24, 25,
                24, 25, 26, 27, 28, 29,
                28, 29, 30, 31, 32, 1
       };
        private static readonly byte[] Permutation =
        {
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
        };
        private static int UnknownInteger { get; set; }
        private static byte UnknownByte { get; set; }
        private static bool Save(DataTable data, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (var bw = new BinaryWriter(fs))
                {
                    if (NewTableStructure())
                    {
                        bw.Write(UnknownInteger);
                        bw.Write(UnknownByte);
                    }

                    bw.Write(BitConverter.GetBytes((int)data.Columns.Count));
                    foreach (DataColumn column in data.Columns)
                    {
                        bw.Write(ColumnTypes.GetColumnTypeFromFullName(column.DataType.FullName));
                    }

                    bw.Write(BitConverter.GetBytes((int)data.Rows.Count));
                    foreach (DataRow row in data.Rows)
                    {
                        int columnIndex = 0;
                        foreach (var cell in row.ItemArray)
                        {
                            var col = data.Columns[columnIndex++];

                            switch (col.DataType.FullName)
                            {
                                case "System.UInt64":
                                    bw.Write(BitConverter.GetBytes((ulong)cell));
                                    break;
                                case "System.Int64":
                                    bw.Write(BitConverter.GetBytes((long)cell));
                                    break;
                                case "System.Double":
                                    bw.Write(BitConverter.GetBytes((double)cell));
                                    break;
                                case "System.Single":
                                    bw.Write(BitConverter.GetBytes((float)cell));
                                    break;
                                case "System.UInt32":
                                    bw.Write(BitConverter.GetBytes((uint)cell));
                                    break;
                                case "System.Int32":
                                    bw.Write(BitConverter.GetBytes((int)cell));
                                    break;
                                case "System.UInt16":
                                    bw.Write(BitConverter.GetBytes((ushort)cell));
                                    break;
                                case "System.Int16":
                                    bw.Write(BitConverter.GetBytes((short)cell));
                                    break;
                                case "System.SByte":
                                    bw.Write((sbyte)cell);
                                    break;
                                case "System.Byte":
                                    bw.Write((byte)cell);
                                    break;
                                case "System.String":
                                    if (!(cell is string val))
                                        bw.Write(0);
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            var chArray = Encoding.GetEncoding("EUC-KR").GetBytes(val);
                                            bw.Write((int)chArray.Length);
                                            foreach (var c in chArray)
                                                bw.Write(c);
                                        }
                                        else
                                            bw.Write((int)0);


                                    }
                                    break;
                                default:
                                    Trace.Assert(false, "KOTableFile::Save() - Unknown type.");
                                    break;
                            }
                        }
                    }

                    Encode(fs);

                    return true;
                }
            }
        }
        private static void EncodeStandart(FileStream plainStream)
        {
            plainStream.Seek(0, SeekOrigin.Begin);
            var plainByte = plainStream.ReadByte();

            _volatileKey = 0x0418;

            while (plainByte != -1)
            {
                plainStream.Seek(-1L, SeekOrigin.Current);
                byte rawByte = (byte)(plainByte & 0xFF);
                byte temporaryKey = (byte)((_volatileKey & 0xff00) >> 8);
                byte encryptedByte = (byte)(temporaryKey ^ rawByte);
                _volatileKey = (ushort)((encryptedByte + _volatileKey) * CipherKey1 + CipherKey2);
                plainStream.WriteByte(encryptedByte);
                plainByte = plainStream.ReadByte();
            }
        }
        private static bool NewTableStructure()
        {
            return true;
        }
        private static void Encode(FileStream plainStream)
        {
            /* plainStream'de sadece sütun ve satır verisi var.*/
            var fileHeader = new byte[]
            {
                0x4C, 0x26, 0x43, 0x7F, 0x80, 0xF1, 0x57, 0x98, 0x79, 0xFC, 0xAF, 0x26, 0x86, 0xD6, 0x20, 0x8E
            };

            var plainLength = plainStream.Length;

            var encodedFileSize = new byte[4];
            encodedFileSize[3] = (byte)(plainLength & 0x000000ff);
            encodedFileSize[2] = (byte)((plainLength & 0x0000ff00) >> 8);
            encodedFileSize[1] = (byte)((plainLength & 0x00ff0000) >> 16);
            encodedFileSize[0] = (byte)((plainLength & 0xff000000) >> 24);
            _ = GetRealLength(encodedFileSize, 0);

            /* Encrypt with layer 2 schema */

            EncodeStandart(plainStream);
            /* Encrypt with layer 1 schema */
            plainStream.Seek(0, SeekOrigin.Begin);

            var plain_len = plainStream.Length;
            // int length = (int) decodeBufferLen;
            var encodedbuffer = new byte[plain_len];
            plainStream.Read(encodedbuffer, 0, (int)plain_len);
            byte[] buffer = new byte[plain_len + (8 - (plain_len % 8))];
            EncodeLayer1(_key, encodedbuffer, (int)plain_len, ref buffer);


            /* Erase stream content */
            plainStream.Flush();
            plainStream.Seek(0, SeekOrigin.Begin);


            foreach (byte t in fileHeader)
                plainStream.WriteByte(t);

            foreach (byte t in encodedFileSize)
                plainStream.WriteByte(t);

            /* Write encoded buffer */

            plainStream.Write(buffer, 0, buffer.Length);
        }
        private static void Decode(ref byte[] data)
        {

            int decodeBufferLen = data.Length;
            var decodeBuffer = new byte[decodeBufferLen];

            using (Stream stream = new MemoryStream(data))
            {
                stream.Read(decodeBuffer, 0, (int)data.Length);
            }

            _ = BitConverter.ToInt32(decodeBuffer, 16);
            int length = GetRealLength(decodeBuffer, 16);

            byte[] buffer = new byte[decodeBufferLen - 20];
            // decodebuffer ilk 20 byte pas geçiyor
            DecodeLayer1(_key, decodeBuffer, decodeBufferLen, ref buffer);

            /*
             * Debug-dump decoded file 
             */
            using (FileStream fs = File.Create("DEBUG_decrypted_l1.bin"))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        bw.Write(buffer[i]);
                    }
                }
            }

            _volatileKey = 0x0418;
            for (int i = 0; i < buffer.Length; i++)
            {
                byte rawByte = buffer[i];
                byte temporaryKey = (byte)((_volatileKey & 0xff00) >> 8);
                byte encryptedByte = (byte)(temporaryKey ^ rawByte);
                _volatileKey = (ushort)((rawByte + _volatileKey) * CipherKey1 + CipherKey2);
                buffer[i] = encryptedByte;
            }

            /*
            * Debug-dump decoded file 
            */
            using (FileStream fs = File.Create("DEBUG_decrypted_full.bin"))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        bw.Write(buffer[i]);
                    }
                }
            }

            data = new byte[length];
            Array.Copy(buffer, 0, data, 0, length);
            if (length + 20 != decodeBufferLen)
            {

                //Padding = new byte[decodeBufferLen - length];
                //  Array.Copy(buffer, length, Padding, 0, decodeBufferLen - length);
            }
            //  File.WriteAllBytes("wtf.ddump", buffer);
        }
        private static int GetRealLength(byte[] buffer, int p1)
        {
            return ((((buffer[p1] << 24) + (buffer[p1 + 1] << 16)) + (buffer[p1 + 2] << 8)) + buffer[p1 + 3]);
        }
        private static void DecodeLayer1(byte[] key, byte[] inputBuffer, int bufferLen, ref byte[] outputBuffer)
        {
            var length = bufferLen - 20;
            /* bütün dosyayı 8 ayrı parça olarak işliyor sanırım */
            var mainCounter = (length + 7) >> 3;
            var num3 = 0;
            var outputIndex = 0;
            var plainByteBlock = new byte[64];

            /* ilk 20 byteyi pas geçiyor */
            Array.Copy(inputBuffer, 20, outputBuffer, 0, length);

            do
            {
                Array.Clear(plainByteBlock, 0, 64);
                int index = 0;
                int counter = 8;

                /* Seperate current byte to bits. */
                do
                {
                    byte num8 = outputBuffer[num3++];
                    plainByteBlock[index] = (byte)((num8 >> 7) & 1);
                    int num9 = index + 1;
                    plainByteBlock[num9++] = (byte)((num8 >> 6) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 5) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 4) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 3) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 2) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 1) & 1);
                    plainByteBlock[num9] = (byte)(num8 & 1);
                    index = num9 + 1;
                    counter--;
                }
                while (counter > 0);
                /* Process byte block */
                InitialDecode_Sub(ref key, plainByteBlock, 0);
                int counter2 = 0;
                /* Merge processed bits to byte.*/
                do
                {
                    byte processedByte = (byte)(plainByteBlock[counter2 + 7] | (2 * (plainByteBlock[counter2 + 6] | (2 * (plainByteBlock[counter2 + 5] | (2 * (plainByteBlock[counter2 + 4] | (2 * (plainByteBlock[counter2 + 3] | (2 * (plainByteBlock[counter2 + 2] | (2 * (plainByteBlock[counter2 + 1] | (2 * plainByteBlock[counter2]))))))))))))));
                    counter2 += 8;
                    outputBuffer[outputIndex++] = processedByte;
                }
                while (counter2 < 64);
            }
            while (mainCounter-- != 1);
        }
        private static void InitialDecode_Sub(ref byte[] p0, byte[] p1, int p2)
        {
            int num = 0;
            int num2 = 15;
            var numArray = new uint[8];
            var buffer = new byte[48];




            var numArray2 = new uint[] {
                0x10101, 0x100, 0x1000101, 0x1000000, 0x10000, 0x1010101, 0x1010001, 1, 0x1010000, 0x10001, 0x10100, 0x101, 0x1000100, 0x1000001, 0, 0x1010100,
                0, 0x1010101, 0x1010100, 0x100, 0x10101, 0x10000, 0x1000101, 0x1000000, 0x10001, 0x10100, 0x101, 0x1010001, 0x1000001, 0x1000100, 0x1010000, 1,
                0x100, 0x1000000, 0x10101, 1, 0x1000101, 0x10100, 0x10000, 0x1010001, 0x1010101, 0x101, 0x1000001, 0x1010100, 0x1010000, 0x10001, 0x1000100, 0,
                0x1010101, 0x101, 1, 0x10000, 0x100, 0x1000001, 0x1000000, 0x1010100, 0x1000100, 0x1010001, 0x1010000, 0x10101, 0x10001, 0, 0x10100, 0x1000101
             };
            var numArray3 = new uint[] {
                0x1010101, 0x1000000, 1, 0x10101, 0x10100, 0x1010001, 0x1010000, 0x100, 0x1000001, 0x1010100, 0x10000, 0x1000101, 0x101, 0, 0x1000100, 0x10001,
                0x1010000, 0x1000101, 0x100, 0x1010100, 0x1010101, 0x10000, 1, 0x10101, 0x101, 0, 0x1000000, 0x10001, 0x10100, 0x1000001, 0x1010001, 0x1000100,
                0, 0x10101, 0x1010100, 0x1010001, 0x10001, 0x100, 0x1000101, 0x1000000, 0x1000100, 1, 0x101, 0x10100, 0x1000001, 0x1010000, 0x10000, 0x1010101,
                0x1000101, 1, 0x10001, 0x1000000, 0x1010000, 0x1010101, 0x100, 0x10000, 0x1010001, 0x10100, 0x1010100, 0x101, 0, 0x1000100, 0x10101, 0x1000001
             };
            var numArray4 = new uint[] {
                0x10001, 0, 0x1000001, 0x10101, 0x10100, 0x1010000, 0x1010101, 0x1000100, 0x1000000, 0x1000101, 0x101, 0x1010100, 0x1010001, 0x100, 0x10000, 1,
                0x1000101, 0x1010100, 0, 0x1000001, 0x1010000, 0x100, 0x10100, 0x10001, 0x10000, 1, 0x1000100, 0x10101, 0x101, 0x1010001, 0x1010101, 0x1000000,
                0x1000101, 0x10100, 0x100, 0x1000001, 1, 0x1010101, 0x1010000, 0, 0x1010001, 0x1000000, 0x10000, 0x101, 0x1000100, 0x10001, 0x10101, 0x1010100,
                0x1000000, 0x10001, 0x1000101, 0, 0x10100, 0x1000001, 1, 0x1010100, 0x100, 0x1010101, 0x10101, 0x1010000, 0x1010001, 0x1000100, 0x10000, 0x101
             };
            var numArray5 = new uint[] {
                0x1010100, 0x1000101, 0x10101, 0x1010000, 0, 0x10100, 0x1000001, 0x10001, 0x1000000, 0x10000, 1, 0x1000100, 0x1010001, 0x101, 0x100, 0x1010101,
                0x1000101, 1, 0x1010001, 0x1000100, 0x10100, 0x1010101, 0, 0x1010000, 0x100, 0x1010100, 0x10000, 0x101, 0x1000000, 0x10001, 0x10101, 0x1000001,
                0x10001, 0x10100, 0x1000001, 0, 0x101, 0x1010001, 0x1010100, 0x1000101, 0x1010101, 0x1000000, 0x1010000, 0x10101, 0x1000100, 0x10000, 1, 0x100,
                0x1010000, 0x1010101, 0, 0x10100, 0x10001, 0x1000000, 0x1000101, 1, 0x1000001, 0x100, 0x1000100, 0x1010001, 0x101, 0x1010100, 0x10000, 0x10101
             };
            var numArray6 = new uint[] {
                0x10000, 0x101, 0x100, 0x1000000, 0x1010100, 0x10001, 0x1010001, 0x10100, 1, 0x1000100, 0x1010000, 0x1010101, 0x1000101, 0, 0x10101, 0x1000001,
                0x10101, 0x1010001, 0x10000, 0x101, 0x100, 0x1010100, 0x1000101, 0x1000000, 0x1000100, 0, 0x1010101, 0x10001, 0x1010000, 0x1000001, 1, 0x10100,
                0x100, 0x10000, 0x1000000, 0x1010001, 0x10001, 0x1000101, 0x1010100, 1, 0x1010101, 0x1000001, 0x101, 0x1000100, 0x10100, 0x1010000, 0, 0x10101,
                0x1010001, 1, 0x101, 0x1010100, 0x1000000, 0x10101, 0x10000, 0x1000101, 0x10100, 0x1010101, 0, 0x1000001, 0x10001, 0x100, 0x1000100, 0x1010000
             };
            var numArray7 = new uint[] {
                0x101, 0x1000000, 0x10001, 0x1010101, 0x1000001, 0x10000, 0x10100, 1, 0, 0x1000101, 0x1010000, 0x100, 0x10101, 0x1010100, 0x1000100, 0x1010001,
                0x10001, 0x1010101, 0x100, 0x10000, 0x1010100, 0x101, 0x1000001, 0x1000100, 0x10100, 0x1000000, 0x1000101, 0x10101, 0, 0x1010001, 0x1010000, 1,
                0x1000001, 0x10101, 0x1010101, 0x1000100, 0x10000, 1, 0x101, 0x1010000, 0x1010100, 0, 0x100, 0x10001, 0x1000000, 0x1000101, 0x1010001, 0x10100,
                0x100, 0x1010000, 0x10000, 0x101, 0x1000001, 0x1000100, 0x1010101, 0x10001, 0x1010001, 0x10101, 0x1000000, 0x1010100, 0x10100, 0, 1, 0x1000101
             };
            var numArray8 = new uint[] {
                0x100, 0x1010001, 0x10000, 0x10101, 0x1010101, 0, 1, 0x1000101, 0x1010000, 0x101, 0x1000001, 0x1010100, 0x1000100, 0x10001, 0x10100, 0x1000000,
                0x1000101, 0, 0x1010001, 0x1010100, 0x100, 0x1000001, 0x1000000, 0x10001, 0x10101, 0x1010000, 0x1000100, 0x101, 0x10000, 0x1010101, 1, 0x10100,
                0x1000000, 0x100, 0x1010001, 0x1000101, 0x101, 0x1010000, 0x1010100, 0x10101, 0x10001, 0x1010101, 0x10100, 1, 0, 0x1000100, 0x1000001, 0x10000,
                0x10100, 0x1010001, 0x1000101, 1, 0x1000000, 0x100, 0x10001, 0x1010100, 0x1000001, 0x1000100, 0, 0x1010101, 0x10101, 0x10000, 0x1010000, 0x101
             };
            var numArray9 = new uint[] {
                0x1000101, 0x10000, 1, 0x100, 0x10100, 0x1010101, 0x1010001, 0x1000000, 0x10001, 0x1000001, 0x1010000, 0x10101, 0x1000100, 0, 0x101, 0x1010100,
                0x1000000, 0x1010101, 0x1000101, 1, 0x10001, 0x1010000, 0x1010100, 0x100, 0x101, 0x1000100, 0x10100, 0x1010001, 0, 0x10101, 0x1000001, 0x10000,
                0x1010100, 0x1010001, 0x100, 0x1000000, 0x1000001, 0x101, 0x10101, 0x10000, 0, 0x10100, 0x10001, 0x1000101, 0x1010101, 0x1010000, 0x1000100, 1,
                0x10000, 0x1000000, 0x10101, 0x1010100, 0x100, 0x10001, 1, 0x1000101, 0x1010101, 0x101, 0x1000001, 0, 0x1010000, 0x1000100, 0x10100, 0x1010001
             };
            /* 32-bit permutation function P used on the output of the S-boxes */

            do
            {
                /* KEY EXPANSION */
                int index = 0;
                do
                {
                    int num4 = num;
                    if (p2 == 0)
                    {
                        num4 = num2;
                    }
                    byte num5 = (byte)(p0[(48 * num4) + index] ^ p1[ExpansionOperationMatrix[index] + 31]);
                    index++;
                    buffer[index - 1] = num5;
                }
                while (index < 48);



                numArray[0] = numArray2[buffer[4] | (2 * (buffer[3] | (2 * (buffer[2] | (2 * (buffer[1] | (2 * (buffer[5] | (2 * buffer[0])))))))))];
                numArray[1] = numArray3[buffer[10] | (2 * (buffer[9] | (2 * (buffer[8] | (2 * (buffer[7] | (2 * (buffer[11] | (2 * buffer[6])))))))))];
                numArray[2] = numArray4[buffer[16] | (2 * (buffer[15] | (2 * (buffer[14] | (2 * (buffer[13] | (2 * (buffer[17] | (2 * buffer[12])))))))))];
                numArray[3] = numArray5[buffer[22] | (2 * (buffer[21] | (2 * (buffer[20] | (2 * (buffer[19] | (2 * (buffer[23] | (2 * buffer[18])))))))))];
                numArray[4] = numArray6[buffer[28] | (2 * (buffer[27] | (2 * (buffer[26] | (2 * (buffer[25] | (2 * (buffer[29] | (2 * buffer[24])))))))))];
                numArray[5] = numArray7[buffer[34] | (2 * (buffer[0x21] | (2 * (buffer[32] | (2 * (buffer[31] | (2 * (buffer[0x23] | (2 * buffer[30])))))))))];
                numArray[6] = numArray8[buffer[40] | (2 * (buffer[39] | (2 * (buffer[38] | (2 * (buffer[37] | (2 * (buffer[41] | (2 * buffer[36])))))))))];
                numArray[7] = numArray9[buffer[46] | (2 * (buffer[45] | (2 * (buffer[44] | (2 * (buffer[43] | (2 * (buffer[0x2f] | (2 * buffer[42])))))))))];
                var destinationArray = new byte[32];
                for (var i = 0; i < 8; i++)
                {
                    Array.Copy(BitConverter.GetBytes(numArray[i]), 0, destinationArray, i * 4, 4);
                }
                var num7 = 0;
                if (num2 <= 0)
                {
                    int num8 = 32;
                    do
                    {
                        p1[num7] = (byte)(p1[num7] ^ destinationArray[Permutation[num7] - 1]);
                        num7++;
                        num8--;
                    }
                    while (num8 > 0);
                }
                else
                {
                    var num9 = 32;
                    do
                    {
                        byte num10 = p1[num7 + 32];
                        byte num11 = (byte)(p1[num7] ^ destinationArray[Permutation[num7] - 1]);
                        num7++;
                        num9--;
                        p1[num7 + 31] = num11;
                        p1[num7 - 1] = num10;
                    }
                    while (num9 > 0);
                }
                num2--;
                num++;
            }
            while (num2 > -1);
        }
        private static void EncodeLayer1(byte[] key, byte[] inputBuffer, int bufferLen, ref byte[] outputBuffer)
        {
            //var length = bufferLen - 20;
            /* bütün dosyayı 8 ayrı parça olarak işliyor sanırım */
            var mainCounter = (bufferLen + 7) >> 3;
            var num3 = 0;
            var outputIndex = 0;
            var plainByteBlock = new byte[64];

            /* ilk 20 byteyi pas geçiyor */
            Array.Copy(inputBuffer, 0, outputBuffer, 0, bufferLen);
            do
            {
                Array.Clear(plainByteBlock, 0, 64);
                int index = 0;
                int counter = 8;

                /* Seperate current byte to bits. */
                do
                {
                    byte num8 = outputBuffer[num3++];
                    plainByteBlock[index] = (byte)((num8 >> 7) & 1);
                    int num9 = index + 1;
                    plainByteBlock[num9++] = (byte)((num8 >> 6) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 5) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 4) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 3) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 2) & 1);
                    plainByteBlock[num9++] = (byte)((num8 >> 1) & 1);
                    plainByteBlock[num9] = (byte)(num8 & 1);
                    index = num9 + 1;
                    counter--;
                }
                while (counter > 0);
                /* Process byte block */
                InitialDecode_Sub(ref key, plainByteBlock, 1);
                int counter2 = 0;
                /* Merge processed bits to byte.*/
                do
                {
                    byte processedByte = (byte)(plainByteBlock[counter2 + 7] | (2 * (plainByteBlock[counter2 + 6] | (2 * (plainByteBlock[counter2 + 5] | (2 * (plainByteBlock[counter2 + 4] | (2 * (plainByteBlock[counter2 + 3] | (2 * (plainByteBlock[counter2 + 2] | (2 * (plainByteBlock[counter2 + 1] | (2 * plainByteBlock[counter2]))))))))))))));
                    counter2 += 8;
                    outputBuffer[outputIndex++] = processedByte;
                    //c++;
                }
                while (counter2 < 64);
            }
            while (mainCounter-- != 1);
        }
        private static DataTable GetTableFromDecodedBytes(byte[] fileData, string tableName)
        {
            using (var ms = new MemoryStream(fileData))
            {
                using (var br = new BinaryReader(ms))
                {
                    #region Read column information

                    if (NewTableStructure())
                    {
                        UnknownInteger = br.ReadInt32();
                        UnknownByte = br.ReadByte();
                    }

                    var columnCount = br.ReadInt32();
                    if (columnCount <= 0)
                        throw new Exception("Invalid column count value. Probably the specified table is corrupt.");
                    var columnData = new int[columnCount];
                    var _temporaryTable = new DataTable(tableName);

                    for (var i = 0; i < columnCount; i++)
                    {
                        var columnType = br.ReadInt32();
                        columnData[i] = columnType;
                        string columnHeader = "";
                        Type _columnType = typeof(void);

                        #region Determine column type
                        switch (columnType)
                        {
                            case ColumnTypes.SignedByte:
                                _columnType = typeof(sbyte);
                                columnHeader += "(signed byte)";
                                break;
                            case ColumnTypes.UnsignedByte:
                                _columnType = typeof(byte);
                                columnHeader += "(unsigned byte)";
                                break;
                            case ColumnTypes.SignedShort:
                                _columnType = typeof(short);
                                columnHeader += "(signed short)";
                                break;
                            case ColumnTypes.UnsignedShort:
                                _columnType = typeof(ushort);
                                columnHeader += "(unsigned short)";
                                break;
                            case ColumnTypes.SignedInteger:
                                _columnType = typeof(int);
                                columnHeader += "(signed integer)";
                                break;
                            case ColumnTypes.UnsignedInteger:
                                _columnType = typeof(uint);
                                columnHeader += "(unsigned integer)";
                                break;
                            case ColumnTypes.String:
                                _columnType = typeof(string);
                                columnHeader += "(string)";
                                break;
                            case ColumnTypes.Float:
                                _columnType = typeof(float);
                                columnHeader += "(float)";
                                break;
                            case ColumnTypes.Double:
                                _columnType = typeof(double);
                                columnHeader += "(double)";
                                break;
                            case ColumnTypes.SignedLong:
                                _columnType = typeof(Int64);
                                columnHeader += "(signed long)";
                                break;
                            case ColumnTypes.UnsignedLong:
                                _columnType = typeof(UInt64);
                                columnHeader += "(unsigned long)";
                                break;
                            default:
                                throw new Exception($"Invalid column type ({columnType})");
                        }
                        #endregion

                        var columnTitle = $"[{i}]\n{columnHeader}";
                        var temporaryColumn = new DataColumn(columnTitle, _columnType)
                        {
                            DefaultValue = GetDefault(_columnType),
                            Caption = columnTitle,
                        };

                        _temporaryTable.Columns.Add(temporaryColumn);

                    }

                    #endregion

                    #region Read value information

                    var rowCount = br.ReadInt32();
                    if (rowCount <= 0)
                        throw new Exception("Invalid row count value. Probably the specified table is corrupt.");

                    for (var i = 0; i < rowCount; i++)
                    {
                        DataRow _temporaryRow = _temporaryTable.NewRow();
                        for (var j = 0; j < columnCount; j++)
                        {
                            switch (columnData[j])
                            {
                                case ColumnTypes.SignedByte:
                                    _temporaryRow[j] = br.ReadSByte();
                                    break;
                                case ColumnTypes.UnsignedByte:
                                    _temporaryRow[j] = br.ReadByte();
                                    break;
                                case ColumnTypes.SignedShort:
                                    _temporaryRow[j] = br.ReadInt16();
                                    break;
                                case ColumnTypes.UnsignedShort:
                                    _temporaryRow[j] = br.ReadUInt16();
                                    break;
                                case ColumnTypes.SignedInteger:
                                    _temporaryRow[j] = br.ReadInt32();
                                    break;
                                case ColumnTypes.UnsignedInteger:
                                    _temporaryRow[j] = br.ReadUInt32();
                                    break;
                                case ColumnTypes.String:
                                    /* string */
                                    var strlen = br.ReadInt32();
                                    if (strlen > 0)
                                    {
                                        var chArray = Encoding.GetEncoding("EUC-KR").GetChars(br.ReadBytes(strlen));
                                        _temporaryRow[j] = new string(chArray);
                                    }

                                    break;
                                case ColumnTypes.Float:
                                    _temporaryRow[j] = br.ReadSingle();
                                    break;
                                case ColumnTypes.Double:
                                    _temporaryRow[j] = br.ReadDouble();
                                    break;
                                case ColumnTypes.SignedLong:
                                    _temporaryRow[j] = br.ReadInt64();
                                    break;
                                case ColumnTypes.UnsignedLong:
                                    _temporaryRow[j] = br.ReadUInt64();
                                    break;
                                default:
                                    throw new Exception(string.Format("Invalid column type ({0})", columnData[j]));
                            }
                        }
                        _temporaryTable.Rows.Add(_temporaryRow);
                    }

                    #endregion
                    // Trace.Assert(ms.Length - 1 == ms.Position, "Did not read all the data!");

                    return _temporaryTable;
                }
            }
        }
        private static object GetDefault(Type type)
        {
            // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
            if (type == null || !type.IsValueType || type == typeof(void))
                return null;

            // If the supplied Type has generic parameters, its default value cannot be determined
            if (type.ContainsGenericParameters)
                throw new ArgumentException(
                    "{" + MethodBase.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> contains generic parameters, so the default value cannot be retrieved");

            // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct/enum), return a 
            //  default instance of the value type
            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodBase.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            // Fail with exception
            throw new ArgumentException("{" + MethodBase.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                "> is not a publicly-visible type, so the default value cannot be retrieved");
        }
        #endregion
    }
}
