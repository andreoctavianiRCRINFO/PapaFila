using SQLite;

namespace PapaFilaRcr.Models
{
    [Table("historicod")]

    public class HistoricoD
    {
        public int idcomanda { get; set; }
        public string codigo { get; set; }
        public int item { get; set; }
        public int Id { get; set; }
        public string status { get; set; }
        public string descricao { get; set; }
        public string marca { get; set; }
        public string departamento { get; set; }
        public decimal preco { get; set; }
        public decimal quantidade { get; set; }
        public decimal total { get; set; }
    }
}
