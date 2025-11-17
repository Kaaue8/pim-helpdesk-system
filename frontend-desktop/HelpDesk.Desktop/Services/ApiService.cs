using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // ATENÇÃO: Você precisará instalar o pacote Newtonsoft.Json
using HelpDesk.Desktop.Models;

namespace HelpDesk.Desktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5079/api/"; // endereço da nossa api note juh
        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        // Método para Autenticação (Login)
        public async Task<string> Authenticate(string email, string password)
        {
            var loginData = new { Email = email, Senha = password };
            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var authResponse = JsonConvert.DeserializeObject<AuthResponseModel>(responseContent);

                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.Token);

                    return authResponse.Token;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return $"Erro ao autenticar: {response.StatusCode} - {error}";
                }
            }
            catch (Exception ex)
            {
                return $"Erro de conexão: {ex.Message}";
            }
        }

        // Método de exemplo para buscar dados protegidos
        public async Task<string> GetUserData(string userId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}usuarios/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return $"Erro ao buscar dados: {response.StatusCode}";
        }
    }
}
