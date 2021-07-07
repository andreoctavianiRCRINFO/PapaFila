using System;
using System.Collections.Generic;
using System.Text;

namespace PapaFilaRcr
{
    public class ItensNovaVenda
    {
        public int item { get; set; }
        public string codigo { get; set; }
        public int Id { get; set; }
        public string descricao { get; set; }
        public string marca { get; set; }
        public string departamento { get; set; }
        public decimal preco { get; set; }
        public decimal quantidade { get; set; }
        public decimal total { get; set; }
    }
}
