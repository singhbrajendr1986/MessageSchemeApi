using System;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Text;


namespace BusinessLayer
{

    public interface IBinaryMessageEncoder
    {
        public byte[] EncodeMessage(Message data);
    }

    public class BinaryMessageEncoder: IBinaryMessageEncoder
    {

        public byte[] EncodeMessage(Message message)
        {
            // Validate input
            Validate.ValidateMessage(message!);

            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Write the number of headers
                writer.Write((byte)message.Headers.Count);

                // Write headers
                foreach (var header in message.Headers)
                {
                   Common.WriteStringLimited(writer, header.Key);
                    Common.WriteStringLimited(writer, header.Value);
                }

                // Write payload length
                writer.Write(message.Payload.Length);

                // Write payload
                writer.Write(message.Payload);

                return stream.ToArray();
            }
        }

    }
}

