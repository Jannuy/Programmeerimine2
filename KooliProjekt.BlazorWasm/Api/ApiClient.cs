using System.Net.Http.Json;
using Newtonsoft.Json;

namespace KooliProjekt.BlazorWasm
{
    public class ApiClient<T> : IApiClient<T> where T : class
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        // resource on kontrolleri nimi, nt "FoodItems"
        public ApiClient(string resource)
        {
            _baseUrl = "http://localhost:5086/api/" + resource + "/";
            _client = new HttpClient();
        }

        public async Task<OperationResult<T>> Get(int id)
        {
            var url = _baseUrl + "Get?id=" + id;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            return await Send<OperationResult<T>>(request);
        }

        public async Task<OperationResult<PagedResult<T>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            return await Send<OperationResult<PagedResult<T>>>(request);
        }

        public async Task<OperationResult> Save(T item)
        {
            var url = _baseUrl + "Save";
            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(item)
            };

            return await Send<OperationResult>(request);
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + "Delete";
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = id })
            };

            return await Send<OperationResult>(request);
        }

        // Saadab päringu ja teeb vastusest OperationResulti.
        // Kui API ei vasta või vastus on tühi (nt 404), tagastatakse viga.
        private async Task<TResult> Send<TResult>(HttpRequestMessage request) where TResult : OperationResult, new()
        {
            try
            {
                using var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(body))
                {
                    var empty = new TResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        empty.AddError("API returned " + (int)response.StatusCode + " " + response.ReasonPhrase);
                    }

                    return empty;
                }

                return JsonConvert.DeserializeObject<TResult>(body) ?? new TResult();
            }
            catch (Exception ex)
            {
                var result = new TResult();
                result.AddError(ex.Message);
                return result;
            }
        }
    }
}
