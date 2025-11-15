// ApiService.cs - Serviço para comunicação com a API
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HelpDesk.Desktop.Models;

namespace HelpDesk.Desktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string? _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Configurar opções de serialização JSON
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Autenticação
        public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
        {
            var json = JsonSerializer.Serialize(loginRequest, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoginResponse>(responseContent, _jsonOptions);
            }

            return null;
        }

        // Tickets
        public async Task<List<Ticket>?> GetTicketsAsync()
        {
            var response = await _httpClient.GetAsync("/api/tickets");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Ticket>>(content, _jsonOptions);
            }

            return null;
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/tickets/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Ticket>(content, _jsonOptions);
            }

            return null;
        }

        public async Task<Ticket?> CreateTicketAsync(Ticket ticket)
        {
            var json = JsonSerializer.Serialize(ticket, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/tickets", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Ticket>(responseContent, _jsonOptions);
            }

            return null;
        }

        public async Task<bool> UpdateTicketAsync(int id, Ticket ticket)
        {
            var json = JsonSerializer.Serialize(ticket, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/tickets/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/tickets/{id}");
            return response.IsSuccessStatusCode;
        }

        // Usuários
        public async Task<List<Usuario>?> GetUsuariosAsync()
        {
            var response = await _httpClient.GetAsync("/api/usuarios");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Usuario>>(content, _jsonOptions);
            }

            return null;
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/usuarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Usuario>(content, _jsonOptions);
            }

            return null;
        }

        public async Task<Usuario?> CreateUsuarioAsync(Usuario usuario)
        {
            var json = JsonSerializer.Serialize(usuario, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/usuarios", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Usuario>(responseContent, _jsonOptions);
            }

            return null;
        }

        public async Task<bool> UpdateUsuarioAsync(int id, Usuario usuario)
        {
            var json = JsonSerializer.Serialize(usuario, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/usuarios/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/usuarios/{id}");
            return response.IsSuccessStatusCode;
        }

        // Setores
        public async Task<List<Setor>?> GetSetoresAsync()
        {
            var response = await _httpClient.GetAsync("/api/setores");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Setor>>(content, _jsonOptions);
            }

            return null;
        }

        public async Task<Setor?> GetSetorByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/setores/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Setor>(content, _jsonOptions);
            }

            return null;
        }

        public async Task<Setor?> CreateSetorAsync(Setor setor)
        {
            var json = JsonSerializer.Serialize(setor, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/setores", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Setor>(responseContent, _jsonOptions);
            }

            return null;
        }

        public async Task<bool> UpdateSetorAsync(int id, Setor setor)
        {
            var json = JsonSerializer.Serialize(setor, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/setores/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSetorAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/setores/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}