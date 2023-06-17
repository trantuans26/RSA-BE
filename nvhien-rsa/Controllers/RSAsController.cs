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
        private RSAModel _rsaModel;

        public RSAsController()
        {
            rsa = new RSACryptoServiceProvider(1024);
            _publicKey = rsa.ExportParameters(false);
            _privateKey = rsa.ExportParameters(true);
            _rsaModel = new RSAModel();
        }

        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromBody] EncryptInput input)
        {
            if (input.value == null)
            {
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                RSAKey khoa = _rsaModel.taoKhoa();

                KetQuaMaHoa results = new KetQuaMaHoa(_rsaModel.MaHoa(input.value, khoa), khoa);

                return StatusCode(StatusCodes.Status200OK, results);
            }
        }


        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] DecryptInput input)
        {
            if (input.textToDecrypt == null || input.khoa == null)
            {
                return StatusCode(StatusCodes.Status200OK);
            }
            else
            {
                string ketQua = _rsaModel.GiaiMa(input.textToDecrypt, input.khoa);

                return StatusCode(StatusCodes.Status200OK, ketQua);
            }

        }

        [HttpGet("taoKhoa")]
        public IActionResult taoKhoa()
        {
            RSAKey rsaKey = _rsaModel.taoKhoa();

            return StatusCode(StatusCodes.Status200OK, rsaKey);
        }
    }
}
