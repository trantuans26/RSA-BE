using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using RSA.Common;
using System.Security.Cryptography.Xml;

namespace nvhien_rsa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSAsController : ControllerBase
    {
        private static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public RSAsController()
        {
            rsa = new RSACryptoServiceProvider(1024);
            _publicKey = rsa.ExportParameters(false);
            _privateKey = rsa.ExportParameters(true);
        }

        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromBody] EncryptInput input)
        {   
            if(input.value == null)
            {
                return StatusCode(StatusCodes.Status200OK);
            } else
            {
                var bytesToEncrypt = Encoding.Unicode.GetBytes(input.value);

                // Lấy khóa bí mật
                var sw = new StringWriter();
                var xs = new XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, _privateKey);

                using (rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(_publicKey);

                    var cypher = rsa.Encrypt(bytesToEncrypt, false);

                    rsa.Dispose();

                    EncryptedResult results = new EncryptedResult(Convert.ToBase64String(cypher), sw.ToString());

                    return StatusCode(StatusCodes.Status200OK, results);
                    //return StatusCode(StatusCodes.Status200OK, Convert.ToBase64String(cypher));
                }
            }

            //var encrypted = Convert.ToBase64String(cypher);

            //var dataBytes = Convert.FromBase64String(encrypted);
            //rsa.Clear();
            //rsa = new RSACryptoServiceProvider();
            //rsa.ImportParameters(_privateKey);
            //var plainText = rsa.Decrypt(dataBytes, false);
            //return StatusCode(StatusCodes.Status200OK, Encoding.Unicode.GetString(plainText));
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] DecryptInput input)
        {
            if (input.textToDecrypt == null || input.privateKeyString == null)
            {
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                var bytesToDescrypt = Encoding.UTF8.GetBytes(input.textToDecrypt);


                using (rsa = new RSACryptoServiceProvider())
                {
                    // Add khóa bí mật
                    rsa.FromXmlString(input.privateKeyString);

                    // convert string sang byte
                    var resultBytes = Convert.FromBase64String(input.textToDecrypt);

                    var decryptedBytes = rsa.Decrypt(resultBytes, false);

                    var decryptedData = Encoding.Unicode.GetString(decryptedBytes);

                    return StatusCode(StatusCodes.Status200OK, decryptedData.ToString());
                }
            }

        }

        [HttpGet("publicKey")]
        public IActionResult getPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);

            return StatusCode(StatusCodes.Status200OK, sw.ToString());
        }

        [HttpGet("privateKey")]
        public IActionResult getPrivateKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _privateKey);

            return StatusCode(StatusCodes.Status200OK, sw.ToString());
        }
    }
}
