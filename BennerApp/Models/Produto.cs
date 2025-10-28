using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.Models
{
    public class Produto
    {
        public int Id { get; set; }              // auto
        public string Nome { get; set; }         // obrigatório
        public string Codigo { get; set; }       // obrigatório
        public decimal Valor { get; set; }       // obrigatório
    }
}
