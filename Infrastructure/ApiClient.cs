using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using System.Net.Http.Json;
using UserInformationApp.Contracts;
using UserInformationApp.Domain;

namespace UserInformationApp.Infrastructure
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreaker;

        public ApiClient(HttpClient httpClient, IOptions<ApiOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

            _circuitBreaker = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int page)
        {
            var response = await _circuitBreaker.ExecuteAsync(() =>
                _httpClient.GetAsync($"/api/users?page={page}"));

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<UsersResponse>();
            return result?.Data ?? [];
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var response = await _circuitBreaker.ExecuteAsync(() =>
                _httpClient.GetAsync($"/api/users/{userId}"));

            if (!response.IsSuccessStatusCode) return null;
            var result = await response.Content.ReadFromJsonAsync<UserResponse>();
            return result?.Data;
        }
    }

}
