using BosApi.Responses;
using System.Net.Http.Json;

namespace BosApi
{
    public class BosClient
    {
        private HttpClient _http = null;

        /// <summary>
        /// Создание клиента для работы с секретами BOS
        /// </summary>
        /// <param name="apiKey">Api ключ для сейфа из личного кабинета</param>
        public BosClient(string apiKey) 
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            _http = new HttpClient() { BaseAddress = new Uri("http://localhost:5062/") };
            _http.DefaultRequestHeaders.Add("Api-Key", apiKey);
        }

        /// <summary>
        /// Получить секрет по уникальному идентификатору
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public async Task<CoreResponse<ReadRecordResponse>> ReadSecret(string recordId)
        {
                 var request = new HttpRequestMessage(HttpMethod.Get, $"api/Record/ReadWithKey?recId={recordId}");
            
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ReadRecordResponse>();
                return new CoreResponse<ReadRecordResponse>(res);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var res = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                return new CoreResponse<ReadRecordResponse>(res);
            }
            else
            {
                throw new Exception("Ошибка");
            }

        }


        public void SetBaseAddress(string address)
        {
            _http.BaseAddress = new Uri(address);
        }

    }
}