using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Shared.External
{
    using Microsoft.Extensions.Options;
    using Shared.Configs;
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
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var jsonValores = await response.Content.ReadAsStringAsync();
                    var descuentos = JsonSerializer.Deserialize<DescuentoDto>(jsonValores);
                    return descuentos.PorcentajeDeDescuento.ToString();
                }
                catch (HttpRequestException e)
                {
                    throw;
                }
            }
        }
    }

}
