using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.External
{
    using Shared.DTOs;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;

    namespace ApiClientLibrary
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
                    _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var jsonValores = await response.Content.ReadAsStringAsync();
                    var descuentos = JsonSerializer.Deserialize<DescuentoDto>(jsonValores);
                    return descuentos.PorcentajeDeDescuento.ToString();
                }
                catch (HttpRequestException e)
                {
                    File.AppendAllText("error.txt", $"{e.Message}");
                    throw;
                }
            }
        }
    }

}
