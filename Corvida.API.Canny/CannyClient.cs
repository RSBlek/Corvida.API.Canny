using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Corvida.API.Canny
{
    public class CannyClient
    {
        private readonly Guid apiKey;
        private static readonly Lazy<HttpClient> httpClientLazy = new Lazy<HttpClient>();

        public String Endpoint { get; set; } = "https://corvida.eu/api";

        public CannyClient(String apiKey)
        {
            if (!Guid.TryParse(apiKey, out Guid guidApiKey))
                throw new ArgumentException("Invalid API key. The Key must be a valid GUID");
            this.apiKey = guidApiKey;
        }

        public CannyClient(Guid apiKey)
        {
            this.apiKey = apiKey;
        }
        private string ImageToBase64(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        public async Task<Image> CannyRequest(Image image)
        {
            return await CannyRequest(image, null, null, null, null);
        }

        public async Task<Image> CannyRequest(Image image, double threshold1, double threshold2)
        {
            return await CannyRequest(image, threshold1, threshold2, null, null);
        }

        public async Task<Image> CannyRequest(Image image, double threshold1, double threshold2, int apertureSize, bool l2Gradient)
        {
            return await CannyRequest(image, threshold1, threshold2, apertureSize, l2Gradient);
        }

        public async Task<Image> CannyRequest(Uri imageUrl)
        {
            return await CannyRequest(imageUrl, null, null, null, null);
        }

        public async Task<Image> CannyRequest(Uri imageUrl, double threshold1, double threshold2)
        {
            return await CannyRequest(imageUrl, threshold1, threshold2, null, null);
        }

        public async Task<Image> CannyRequest(Uri imageUrl, double threshold1, double threshold2, int apertureSize, bool l2Gradient)
        {
            return await CannyRequest(imageUrl, threshold1, threshold2, apertureSize, l2Gradient);
        }

        private async Task<Image> CannyRequest(Image image, double? threshold1 = null, double? threshold2 = null, int? apertureSize = null, bool? l2Gradient = null)
        {
            CannyRequest request = new CannyRequest(ImageToBase64(image), threshold1, threshold2, apertureSize, l2Gradient);
            return await CannyRequest(request);
        }

        private async Task<Image> CannyRequest(Uri imageUrl, double? threshold1 = null, double? threshold2 = null, int? apertureSize = null, bool? l2Gradient = null)
        {
            CannyRequest request = new CannyRequest(imageUrl, threshold1, threshold2, apertureSize, l2Gradient);
            return await CannyRequest(request);
        }

        private async Task<Image> CannyRequest(CannyRequest request)
        {
            if (request.Threshold1.HasValue && request.Threshold1.Value <= 0 || request.Threshold2.HasValue && request.Threshold2.Value <= 0)
                throw new ArgumentException("Threshold has to be greater than 0");
            if (request.ApertureSize.HasValue && request.ApertureSize.Value <= 0)
                throw new ArgumentException("ApertureSize has to be greater than 0");
            HttpClient httpClient = httpClientLazy.Value;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("ApiKey", apiKey.ToString());

            String json = JsonSerializer.Serialize(request);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(this.Endpoint + "/canny", jsonContent);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                String stringContent = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                BadRequestResult badRequestResult = JsonSerializer.Deserialize<BadRequestResult>(stringContent, serializerOptions);
                throw new BadRequestException(badRequestResult.Code, badRequestResult.Description);
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not execute request");
            }

            byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();
            MemoryStream memoryStream = new MemoryStream(responseBytes);
            return Image.FromStream(memoryStream);
        }
    }
}
