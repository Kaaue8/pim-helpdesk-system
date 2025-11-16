namespace HelpDesk.Api.Services
{
    public class HoustonService
    {
        private readonly string _apiKey;

        public HoustonService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GerarSolucaoAsync(string titulo, string descricao)
        {
            // Implementação simplificada - pode ser expandida com Semantic Kernel
            await Task.Delay(100); // Simular processamento

            return $"Solução sugerida para: {titulo}\n\n" +
                   $"Baseado na descrição fornecida, recomendamos:\n" +
                   $"1. Verificar as configurações do sistema\n" +
                   $"2. Reiniciar o serviço/aplicação\n" +
                   $"3. Consultar a documentação técnica\n" +
                   $"4. Se o problema persistir, escalar para o nível 2";
        }
    }
}

