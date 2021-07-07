using SQLite;

namespace PapaFilaRcr.Models
{
    [Table("confetiqbal")]
    public class ConfEtiqBal
    {
        public int NumeroDigitos { get; set; }
        public string ImprimeDigito { get; set; }
        public string ImprimePesoPreco { get; set; }
    }
}
