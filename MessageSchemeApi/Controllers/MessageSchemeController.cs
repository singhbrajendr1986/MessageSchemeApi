using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;


namespace MessageSchemeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageSchemeController : ControllerBase
    {

        public IBinaryMessageDecoder binaryMessageDecoder;
        public IBinaryMessageEncoder binaryMessageEncoder;

        public MessageSchemeController(IBinaryMessageDecoder _binaryMessageDecoder, IBinaryMessageEncoder _binaryMessageEncoder)
        {
            this.binaryMessageDecoder = _binaryMessageDecoder;
            this.binaryMessageEncoder = _binaryMessageEncoder;

        }

        // GET: api/<MessageSchemeController>
        [Route("sms")]
        [HttpPost]
        public string GetMessage(byte[] data)
        {
            Message result = binaryMessageDecoder.DecodeMessage(data);
            return System.Text.Encoding.UTF8.GetString(result.Payload); ;
        }

        // POST api/<MessageSchemeController>
        [HttpPost]
        [Route("encoded/sms")]
        public byte[] GetEncodedMessage(string value)
        {
            Message request;
            Dictionary<string, string> My_dict1 =
                       new Dictionary<string, string>();
            My_dict1.Add("Type", "Text");
            
            request = new Message
            {
                Headers = My_dict1,
                Payload = System.Text.Encoding.UTF8.GetBytes(value)
            };
            byte[] result = binaryMessageEncoder.EncodeMessage(request);
            return result;
        }

    }
}
