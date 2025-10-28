using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.Models
{
    public class ItemPedido
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal ValorUnit { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal { get { return ValorUnit * Quantidade; } } // calculado
    }
}
