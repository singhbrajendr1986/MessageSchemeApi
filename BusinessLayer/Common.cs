using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BusinessLayer
{
    public static class Common
    {

        public static void WriteStringLimited(BinaryWriter writer, string value)
        {
            Validate.ValidateHeaderLength(value);

            byte[] bytes = Encoding.ASCII.GetBytes(value);
            int length = Math.Min(bytes.Length, 1023);
            writer.Write((byte)length);
            writer.Write(bytes, 0, length);
        }

        public static string ReadStringLimited(BinaryReader reader)
        {
            byte length = reader.ReadByte();
            Validate.ValidateHeaderLength(length);

            byte[] bytes = reader.ReadBytes(length);
            return Encoding.ASCII.GetString(bytes);
        }

        public enum MessageType
        {
            Text,
            Image
        }
    }
}
