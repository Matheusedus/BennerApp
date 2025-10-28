using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.Models
{
    public class Pedido
    {
        public int Id { get; set; }                       // auto
        public int PessoaId { get; set; }                 // relacionamento
        public DateTime DataVenda { get; set; }           // auto (agora)
        public FormaPagamento FormaPagamento { get; set; }// obrigatório
        public StatusPedido Status { get; set; }          // default = Pendente
        public List<ItemPedido> Itens { get; set; }       // >= 1
        public decimal ValorTotal { get; set; }           // calculado

        public Pedido()
        {
            Status = StatusPedido.Pendente;
            Itens = new List<ItemPedido>();
        }
    }
}
