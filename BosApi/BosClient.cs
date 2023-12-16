using BosApi.Responses;
using BosApi.Services;
using System.Net.Http.Json;
using System.Web;

namespace BosApi
{
    public class BosClient
    {
        private HttpClient _http = null;
        private AsymmetricCryptoService cryptoService = null;

        /// <summary>
        /// Создание клиента для работы с секретами BOS
        /// </summary>
        /// <param name="apiKey">Api ключ для сейфа из личного кабинета</param>
        public BosClient(string apiKey) 
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            _http = new HttpClient() { BaseAddress = new Uri("http://localhost:5062/") };
            _http.DefaultRequestHeaders.Add("Api-Key", apiKey);

            cryptoService = new AsymmetricCryptoService();
        }

        /// <summary>
        /// Получить секрет по уникальному идентификатору
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public async Task<CoreResponse<ReadRecordResponse>> ReadSecret(string recordId, bool isDecrypt = true)
        {
            var keys = cryptoService.GenerateKeys();

            // Prepare query parameters
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["recId"] = recordId;
            queryString["pubKey"] = keys.publicKeyPem;

            var requestUri = $"api/Record/ReadWithKey?{queryString}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                ReadRecordResponse result = await response.Content.ReadFromJsonAsync<ReadRecordResponse>();

                if (isDecrypt)
                {
                    result.ELogin = cryptoService.DecryptFromClientData(result.ELogin, keys.privateKeyPem);
                    result.EPw = cryptoService.DecryptFromClientData(result.EPw, keys.privateKeyPem);
                    result.ESecret = cryptoService.DecryptFromClientData(result.ESecret, keys.privateKeyPem);
                }

                return new CoreResponse<ReadRecordResponse>(result);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                IEnumerable<string> errorList = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                return new CoreResponse<ReadRecordResponse>(errorList);
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}.");
            }
        }


        public async Task<CoreResponse<bool>> TryUpdateSecret(string recordId, PatchRecordDTO command)
        {
            await Task.CompletedTask;
            return new CoreResponse<bool>(true);

        }


        public void SetBaseAddress(string address)
        {
            _http.BaseAddress = new Uri(address);
        }

    }
}