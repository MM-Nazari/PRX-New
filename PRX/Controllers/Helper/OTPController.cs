using DotNet.RateLimiter.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PRX.Data;
using PRX.Dto.Helper;
using PRX.Dto.User;
using PRX.Utils;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly PRXDbContext _context;
        private readonly IConfiguration _configuration;

        public OTPController(IHttpClientFactory httpClientFactory, PRXDbContext context, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _configuration = configuration;
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

        [HttpPost("OTP-Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OTPLoginAsync([FromBody] OtpDto otpVerificationDto)
        {
            try
            {
                var isOtpVerified = await VerifyOtp(otpVerificationDto.Mobile, otpVerificationDto.Otp);
                if (!isOtpVerified)
                {
                    return BadRequest(new { message = ResponseMessages.OTPVerificationFailed });
                }

                var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == otpVerificationDto.Mobile);
                if (user == null)
                {
                    //Response.Headers.Add("message", "User does not exist");
                    return Unauthorized(new { message = ResponseMessages.UserNotExists });
                }

                var token = GenerateJwtToken(user);

                return Ok(new { Authorization = token });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        private string GenerateJwtToken(PRX.Models.User.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            if (securityKey.KeySize < 256)
            {
                throw new ArgumentException("JWT security key size must be at least 256 bits (32 bytes).");
            }

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define custom claim types
            const string IdClaimType = "id";
            const string PhoneClaimType = "phone_number";
            const string RoleClaimType = "role";
            const string UsernameClaimType = "username";

            // Create claims for user ID and phone number using custom claim types
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(IdClaimType, user.Id.ToString()),
                //new System.Security.Claims.Claim(PhoneClaimType, user.PhoneNumber),
                new System.Security.Claims.Claim(RoleClaimType, user.Role),
            };

            // Add phone number claim only for users
            if (user.Role == "User")
            {
                claims.Add(new System.Security.Claims.Claim(PhoneClaimType, user.PhoneNumber));
            }

            // Add username claim only for admins
            if (user.Role == "Admin")
            {
                claims.Add(new System.Security.Claims.Claim(UsernameClaimType, user.Username));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
