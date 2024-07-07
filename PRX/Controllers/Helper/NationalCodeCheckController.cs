using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRX.Dto.Helper;
using PRX.Models.Helper;
using PRX.Utils;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PRX.Controllers.Helper
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "NationalCodeCheck")]
    public class NationalCodeCheckController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NationalCodeCheckController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckCredentials([FromBody] NationalCodeCheckDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.NationalCode))
                {
                    return BadRequest(new { message = ResponseMessages.NationalCodeNotFound });
                }

                if (string.IsNullOrWhiteSpace(request.Mobile))
                {
                    return BadRequest(new { message = ResponseMessages.MobileNotFound });
                }

                var (isSuccess, responseContent) = await SendNationalCodeRequest(request.NationalCode, request.Mobile);

                if (isSuccess)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                    return Ok(apiResponse);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Failed to check credentials.", detail = responseContent });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        private async Task<(bool isSuccess, string responseContent)> SendNationalCodeRequest(string nationalCode, string mobile)
        {
            try
            {
                var requestBody = new
                {
                    national_code = nationalCode,
                    mobile = mobile
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add("accept", "application/json");
                    httpClient.DefaultRequestHeaders.Add("Key", "9ac6bc9a-cca3-4779-8640-6836b6ab1daa");
                    httpClient.DefaultRequestHeaders.Add("X-CSRFTOKEN", "Rl5wtFdx2T32MyyQjbj1DK0kgV8R4jcwUx1RYimqJ9bfnfStN2tMYtG7FC2SrydL");

                    var response = await httpClient.PostAsync("http://auth.nibdev.com/api/v1/credential_check/mobile/national_code/", httpContent);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return (response.IsSuccessStatusCode, responseContent);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                return (false, ex.Message);
            }
        }

    }
}
