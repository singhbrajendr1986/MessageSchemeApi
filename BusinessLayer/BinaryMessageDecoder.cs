using System.ComponentModel.DataAnnotations;
using static BusinessLayer.Common;

namespace BusinessLayer
{
    public interface IBinaryMessageDecoder
    {
        public Message DecodeMessage(byte[] data);
    }

    public class BinaryMessageDecoder : IBinaryMessageDecoder
    {
        public Message DecodeMessage(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Input data is null or empty.");
            }

            using (MemoryStream stream = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                Message message = new Message();
                message.Headers = new Dictionary<string, string>();

                // Read the number of headers
                byte headerCount = reader.ReadByte();

                // Validate header count
                if (headerCount > 63)
                {
                    throw new InvalidOperationException("Exceeded maximum allowed number of headers.");
                }

                // Read headers
                for (int i = 0; i < headerCount; i++)
                {
                    string headerName = Common.ReadStringLimited(reader);
                    string headerValue = Common.ReadStringLimited(reader);

                    // Validate header name and value lengths
                    Validate.ValidateHeaderLength(headerName);
                    Validate.ValidateHeaderLength(headerValue);

                    message.Headers.Add(headerName, headerValue);
                }


                // Read payload length
                int payloadLength = reader.ReadInt32();

                // Validate payload length
                Validate.ValidatePayloadLength(payloadLength);

                // Read payload
                message.Payload =  reader.ReadBytes(payloadLength);

                return message;
            }
        }

    }
}