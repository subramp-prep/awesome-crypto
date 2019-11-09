using System;
using System.Net;
using Crypto.Core;
using Microsoft.AspNetCore.Mvc;

namespace CryptoService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CryptoController : Controller
    {
        private readonly IMath _math;
        private readonly ISecure _secure;
        static readonly char[] padding = { '=' };

        public CryptoController(IMath math, ISecure secure)
        {
            _math = math;
            _secure = secure;
        }

        /// <summary>
        /// Welcome this instance. Just to test if we are all set!
        /// </summary>
        /// <returns>The welcome.</returns>
        [HttpGet]
        //[ActionName("Welcome")]
        public IActionResult Welcome()
        {
            return new ObjectResult("Welcome to Awesome Crypto!");
        }

        /// <summary>
        /// Input a number to calculate running Average and Standard Deviation
        /// </summary>
        /// <returns> MathDto object with Average and Standard-deviation </returns>
        /// <param name="num">Key in a number</param>
        /// <response code="200">Returns success</response>
        [HttpPost]
        [ActionName("PushAndRecalculate")]
        public IActionResult PushAndRecalculate([FromQuery]long num)
        {
            return new ObjectResult(_math.Calculate(num));
        }

        /// <summary>
        /// Input a number to calculate running Average and Standard Deviation with encryption
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     MathSecureDto
        ///     {
        ///         "httpEncodedMean": "Bx2d-4ZQnLSBddLOaXJ4sg",
        ///         "httpEncodedStandardDeviation": "IEN4GuyqsIPev4hxNTo2TA",
        ///          "mean": "Bx2d+4ZQnLSBddLOaXJ4sg==",
        ///         "standardDeviation": "IEN4GuyqsIPev4hxNTo2TA=="
        ///     }
        ///
        /// </remarks>
        /// <returns>The recalculate and encrypt.</returns>
        /// <param name="num">Number.</param>
        [HttpPost]
        [ActionName("PushRecalculateAndEncrypt")]
        public IActionResult PushRecalculateAndEncrypt([FromQuery]long num)
        {
            IActionResult result;

            try
            {
                var mathDto = _math.Calculate(num);
                var m = _secure.Encrypt(mathDto.Average.ToString());
                var sd = _secure.Encrypt(mathDto.StandardDeviation.ToString());

                MathSecureDto secureDto = new MathSecureDto()
                {
                    HTTPEncodedAverage = EncodeBase64ToURL(m),
                    HTTPEncodedStandardDeviation = EncodeBase64ToURL(sd),
                    Average = m,
                    StandardDeviation = sd
                };

                result = new ObjectResult(secureDto);
            }
            catch (Exception ex)
            {
                result = new ObjectResult(ex)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
           
            return result;
        }

        /// <summary>
        /// Decrypt the content from PushRecalculateAndEncrypt for
        /// human-readable Average and Standard-Deviation
        /// </summary>
        /// <returns>Decrypted data</returns>
        /// <param name="text">Ecrypted text</param>
        [HttpPost]
        [ActionName("Decrypt")]
        public IActionResult Decrypt([FromQuery]string text)
        {
            IActionResult result;
            try
            {
                string data = _secure.Decrypt(DecodeURLToBase64(text));
                result = new ObjectResult(data);
            }
            catch (Exception ex)
            {
                result = new ObjectResult(ex)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            return result;
        }

        private string EncodeBase64ToURL(string data)
        {
            return data.TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }

        private string DecodeURLToBase64(string data)
        {
            string decode = data.Replace('_', '/').Replace('-', '+');
            switch (data.Length % 4)
            {
                case 2: decode += "=="; break;
                case 3: decode += "="; break;
            }

            return decode;
        }
    }
}
