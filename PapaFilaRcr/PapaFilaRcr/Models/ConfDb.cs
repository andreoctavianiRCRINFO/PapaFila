using SQLite;

namespace PapaFilaRcr.Models
{
    [Table("confdb")]

    public class ConfDb
    {
        public string enderecoservidor { get; set; }
        public string porta { get; set; }
        public string nomebanco { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
    }
}
