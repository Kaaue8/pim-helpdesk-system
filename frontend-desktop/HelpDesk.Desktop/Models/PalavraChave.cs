using System;

namespace HelpDesk.Desktop.Models
{
    public class PalavraChave
    {
        public int ID_PalavraChave { get; set; }
        public string Palavra { get; set; } = string.Empty;
        public string? CategoriaSugerida { get; set; }
        public string? PrioridadeSugerida { get; set; }
        public int Peso { get; set; } = 1;
        public int Ocorrencias { get; set; } = 0;
        public bool Ativa { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public bool ContemEm(string texto)
        {
            return !string.IsNullOrEmpty(texto) &&
                   texto.Contains(Palavra, StringComparison.OrdinalIgnoreCase);
        }

        public void IncrementarOcorrencia()
        {
            Ocorrencias++;
        }
    }
}