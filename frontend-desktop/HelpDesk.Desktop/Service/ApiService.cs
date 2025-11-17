using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HelpDesk.Desktop.Models;

namespace HelpDesk.Desktop.Services
{
    /// <summary>
    /// Serviço para comunicação com a API REST do HelpDesk
    /// </summary>
    public class ApiService
    {
        private static ApiService? _instance;
        private readonly HttpClient _httpClient;

        private ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Utils.AppConfig.ApiBaseUrl),
                Timeout = TimeSpan.FromSeconds(Utils.AppConfig.ApiTimeout)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static ApiService Instance => _instance ??= new ApiService();

        #region Auth

        public async Task<LoginResponse?> LoginAsync(string email, string senha)
        {
            try
            {
                var loginData = new { email, senha };
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/Login/Authenticate", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        SetAuthToken(loginResponse.Token);
                    }
                    return loginResponse;
                }

                throw new Exception($"Erro ao fazer login: {responseContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro de conexão: {ex.Message}", ex);
            }
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearAuthToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        #endregion

        #region Tickets

        public async Task<List<Ticket>> GetTicketsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/Tickets");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Ticket>>(content) ?? new List<Ticket>();
                }

                throw new Exception($"Erro ao buscar tickets: {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar tickets: {ex.Message}", ex);
            }
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/Tickets/{id}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Ticket>(content);
                }

                throw new Exception($"Erro ao buscar ticket: {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar ticket: {ex.Message}", ex);
            }
        }

        public async Task<Ticket?> CreateTicketAsync(Ticket ticket)
        {
            try
            {
                var json = JsonConvert.SerializeObject(ticket);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/Tickets", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Ticket>(responseContent);
                }

                throw new Exception($"Erro ao criar ticket: {responseContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar ticket: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateTicketAsync(int id, Ticket ticket)
        {
            try
            {
                var json = JsonConvert.SerializeObject(ticket);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/Tickets/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao atualizar ticket: {errorContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar ticket: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/Tickets/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao excluir ticket: {errorContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir ticket: {ex.Message}", ex);
            }
        }

        #endregion

        #region Usuarios

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/Usuarios");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Usuario>>(content) ?? new List<Usuario>();
                }

                throw new Exception($"Erro ao buscar usuários: {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuários: {ex.Message}", ex);
            }
        }

        #endregion

        #region Categorias

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/Categorias");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Categoria>>(content) ?? new List<Categoria>();
                }

                throw new Exception($"Erro ao buscar categorias: {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar categorias: {ex.Message}", ex);
            }
        }

        public async Task<Categoria?> CreateCategoriaAsync(Categoria categoria)
        {
            try
            {
                var json = JsonConvert.SerializeObject(categoria);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/Categorias", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Categoria>(responseContent);
                }

                throw new Exception($"Erro ao criar categoria: {responseContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar categoria: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateCategoriaAsync(int id, Categoria categoria)
        {
            try
            {
                var json = JsonConvert.SerializeObject(categoria);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/Categorias/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao atualizar categoria: {errorContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar categoria: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/Categorias/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao excluir categoria: {errorContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir categoria: {ex.Message}", ex);
            }
        }

        #endregion

        #region Setores

        public async Task<List<Setor>> GetSetoresAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/Setores");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Setor>>(content) ?? new List<Setor>();
                }

                throw new Exception($"Erro ao buscar setores: {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar setores: {ex.Message}", ex);
            }
        }

        #endregion

        #region Historico

        public async Task<List<TicketHistorico>> GetHistoricoTicketAsync(int ticketId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/Tickets/{ticketId}/historico");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<TicketHistorico>>(content) ?? new List<TicketHistorico>();
                }

                throw new Exception($"Erro ao buscar histórico: {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar histórico: {ex.Message}", ex);
            }
        }

        #endregion

        #region Health Check

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}

