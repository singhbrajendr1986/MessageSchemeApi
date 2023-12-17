using BusinessLayer;

public static class Validate
{

    public static void ValidateHeaderLength(byte length)
    {
        if (length > 1023)
        {
            throw new ArgumentException("Header name or value length exceeds the maximum allowed length.");
        }
    }

    public static void ValidateHeaderLength(string value)
    {
        if (value.Length > 1023)
        {
            throw new ArgumentException("Header name or value exceeds the maximum allowed length.");
        }
    }

    public static void ValidateMessage(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message), "Message is null.");
        }

        if (message.Headers == null)
        {
            throw new ArgumentNullException(nameof(message.Headers), "Headers dictionary is null.");
        }

        if (message.Headers.Count > 63)
        {
            throw new ArgumentException("Exceeded maximum allowed number of headers.");
        }

        foreach (var header in message.Headers)
        {
            ValidateHeaderLength(header.Key);
            ValidateHeaderLength(header.Value);
        }

        ValidatePayloadLength(message.Payload.Length);
    }

    public static void ValidatePayloadLength(int length)
    {
        if (length > 256 * 1024)
        {
            throw new ArgumentException("Payload length exceeds the maximum allowed length.");
        }
    }
}