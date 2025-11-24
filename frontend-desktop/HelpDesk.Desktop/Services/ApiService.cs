using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Utils;

namespace HelpDesk.Desktop.Services
{
    public class ApiService
    {
        private static ApiService _instance;
        private readonly HttpClient _httpClient;

        public static ApiService Instance => _instance ?? (_instance = new ApiService());

        private ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.ApiBaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetAuthToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        /// <summary>
        /// Realiza login na API
        /// </summary>
        public async Task<AuthResponse> LoginAsync(string email, string senha)
        {
            try
            {
                var loginData = new
                {
                    email = email,
                    senha = senha
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Erro na API: {response.StatusCode}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<AuthResponse>(responseContent, options);
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Tempo de conexão esgotado. Verifique se a API está rodando.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao fazer login: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Solicita recuperação de senha
        /// </summary>
        public async Task RecuperarSenhaAsync(string email)
        {
            try
            {
                var data = new
                {
                    email = email
                };

                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/auth/recuperar-senha", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // Se o erro for 404, significa que o endpoint não existe ainda
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("Endpoint de recuperação de senha não implementado na API.\n\n" +
                            "Adicione o endpoint POST /auth/recuperar-senha no backend.");
                    }

                    throw new HttpRequestException($"Erro na API: {response.StatusCode} - {responseContent}");
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Tempo de conexão esgotado. Verifique se a API está rodando.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao solicitar recuperação de senha: {ex.Message}", ex);
            }
        }

        // =================================================================================================
        // MÉTODOS ADICIONADOS PARA CORRIGIR OS ERROS CS1061
        // =================================================================================================

        // Métodos para Categoria (Corrigem erros em FormCategoria.cs)
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            return await GetAsync<List<Categoria>>("/categorias");
        }

        public async Task<Categoria> CreateCategoriaAsync(Categoria categoria)
        {
            return await PostAsync<Categoria>("/categorias", categoria);
        }

        public async Task<Categoria> UpdateCategoriaAsync(Categoria categoria)
        {
            // Usa ID_Categoria conforme seu modelo
            return await PutAsync<Categoria>($"/categorias/{categoria.ID_Categoria}", categoria);
        }

        // Métodos para Ticket (Corrigem erros em FormNovoTicket.cs e FormHistorico.cs)
        public async Task<List<Ticket>> GetTicketsAsync()
        {
            return await GetAsync<List<Ticket>>("/tickets");
        }

        public async Task<List<TicketHistorico>> GetHistoricoTicketAsync(int ticketId)
        {
            return await GetAsync<List<TicketHistorico>>($"/tickets/{ticketId}/historico");
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            return await PostAsync<Ticket>("/tickets", ticket);
        }

        public async Task<Ticket> UpdateTicketAsync(Ticket ticket)
        {
            // Usa ID_Ticket (inferido) para resolver o erro de propriedade
            return await PutAsync<Ticket>($"/tickets/{ticket.ID_Ticket}", ticket);
        }

        // Métodos para Usuário (Corrigem erros em FormUsuarios.cs)
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await GetAsync<List<Usuario>>("/usuarios");
        }

        // Método para limpar o token de autenticação (Corrigem erro em AuthService.cs)
        public void ClearAuthToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        // =================================================================================================
        // MÉTODOS GENÉRICOS (EXISTENTES NO CÓDIGO ORIGINAL )
        // =================================================================================================

        /// <summary>
        /// Realiza requisição GET genérica
        /// </summary>
        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Erro na API: {response.StatusCode}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<T>(responseContent, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição GET: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza requisição POST genérica
        /// </summary>
        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Erro na API: {response.StatusCode}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<T>(responseContent, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição POST: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza requisição PUT genérica
        /// </summary>
        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Erro na API: {response.StatusCode}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<T>(responseContent, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição PUT: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza requisição DELETE genérica
        /// </summary>
        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Erro na API: {response.StatusCode}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na requisição DELETE: {ex.Message}", ex);
            }
        }
    }
}
