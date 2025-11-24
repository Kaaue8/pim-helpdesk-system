using System;

namespace HelpDesk.Desktop.Models
{
    public class SolucaoComum
    {
        public int ID_Solucao { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Solucao { get; set; } = string.Empty;
        public string? PalavrasChave { get; set; }
        public string? Categoria { get; set; }
        public int VezesUtilizada { get; set; } = 0;
        public int FeedbackUtil { get; set; } = 0;
        public int FeedbackNaoUtil { get; set; } = 0;
        public bool Ativa { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public double TaxaSucesso()
        {
            int total = FeedbackUtil + FeedbackNaoUtil;
            return total > 0 ? (double)FeedbackUtil / total * 100 : 0;
        }

        public void RegistrarUso()
        {
            VezesUtilizada++;
            DataAtualizacao = DateTime.Now;
        }

        public void RegistrarFeedbackUtil()
        {
            FeedbackUtil++;
            DataAtualizacao = DateTime.Now;
        }

        public void RegistrarFeedbackNaoUtil()
        {
            FeedbackNaoUtil++;
            DataAtualizacao = DateTime.Now;
        }

        public int CalcularRelevancia(string texto)
        {
            if (string.IsNullOrEmpty(PalavrasChave) || string.IsNullOrEmpty(texto))
                return 0;

            var palavras = PalavrasChave.Split(',');
            int relevancia = 0;

            foreach (var palavra in palavras)
            {
                if (texto.Contains(palavra.Trim(), StringComparison.OrdinalIgnoreCase))
                    relevancia++;
            }

            return relevancia;
        }
    }
}