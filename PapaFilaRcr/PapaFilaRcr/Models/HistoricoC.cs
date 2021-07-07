using SQLite;

namespace PapaFilaRcr.Models
{
    [Table("historicoc")]

    public class HistoricoC
    {
        public int id { get; set; }
        public string numero { get; set; }
        public string datahora { get; set; }
        public decimal total { get; set; }
    }
}
