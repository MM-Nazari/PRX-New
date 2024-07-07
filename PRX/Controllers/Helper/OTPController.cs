using DotNet.RateLimiter.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRX.Dto.Helper;
using PRX.Utils;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PRX.Controllers.NewFolder
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "OTP")]
    public class OTPController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OTPController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("send-otp")]
        [RateLimit(PeriodInSec = 60, Limit = 3)]
        public async Task<IActionResult> SendOtpAsync([FromBody] OtpDto phoneNumberRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumberRequest.Mobile))
                {
                    return BadRequest(new { message = ResponseMessages.OTPMobileNotFound });
                }

                var isOtpSent = await SendOtp(phoneNumberRequest.Mobile);
                if (!isOtpSent)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.OTPCouldntBeSent } );
                }

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyOtpAsync([FromBody] OtpDto otpVerificationDto)
        {
            try
            {
                var isOtpVerified = await VerifyOtp(otpVerificationDto.Mobile, otpVerificationDto.Otp);
                if (!isOtpVerified)
                {
                    return BadRequest(new { message = ResponseMessages.OTPVerificationFailed });
                }

                return Ok(new { message = ResponseMessages.OTPVerificationSucceded });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        private async Task<bool> SendOtp(string mobile)
        {
            try
            {
                var requestBody = new
                {
                    mobile,
                    user = 0,
                    template = "prx"
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Key", "9ac6bc9a-cca3-4779-8640-6836b6ab1daa");
                    httpClient.DefaultRequestHeaders.Add("X-CSRFTOKEN", "Rl5wtFdx2T32MyyQjbj1DK0kgV8R4jcwUx1RYimqJ9bfnfStN2tMYtG7FC2SrydL");

                    var response = await httpClient.PostAsync("http://auth.nibdev.com/api/v1/sms/send_code/", httpContent);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                return false;
            }
        }

        private async Task<bool> VerifyOtp(string mobile, int code)
        {
            try
            {
                var requestBody = new
                {
                    mobile = mobile,
                    user = 0,
                    template = "prx",
                    code = code
                };


                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Key", "9ac6bc9a-cca3-4779-8640-6836b6ab1daa");
                    httpClient.DefaultRequestHeaders.Add("X-CSRFTOKEN", "Rl5wtFdx2T32MyyQjbj1DK0kgV8R4jcwUx1RYimqJ9bfnfStN2tMYtG7FC2SrydL");

                    var response = await httpClient.PostAsync("http://auth.nibdev.com/api/v1/sms/verify_code/", httpContent);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                return false;
            }
        }
    }
}
