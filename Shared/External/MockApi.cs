using System.Net.Http.Headers;
using Shared.DTOs;
using System.Text.Json;

namespace Shared.External
{
    public class MockApi : IApiExterna
    {
        private readonly HttpClient _httpClient;

        public MockApi()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetApiDataAsync(string url)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var jsonValores = await response.Content.ReadAsStringAsync();
                var descuentos = JsonSerializer.Deserialize<DescuentoDto>(jsonValores);
                return descuentos.PorcentajeDeDescuento;
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}
