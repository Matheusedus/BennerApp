using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BennerApp.Models;

namespace BennerApp.Services
{
    public class PedidoService
    {
        private readonly IDataStore _store;
        private readonly IdGenerator _ids;

        public PedidoService(IDataStore store, IdGenerator ids)
        {
            _store = store;
            _ids = ids;
        }

        public Pedido FinalizarPedido(int pessoaId, FormaPagamento forma, List<ItemPedido> itens)
        {
            if (pessoaId <= 0) throw new Exception("Pessoa obrigatória.");
            if (itens == null || itens.Count == 0) throw new Exception("Pedido sem itens.");

            var pedidos = _store.LoadPedidos();
            var p = new Pedido();
            p.Id = _ids.NextPedido();
            p.PessoaId = pessoaId;
            p.DataVenda = DateTime.Now;
            p.FormaPagamento = forma;
            p.Status = StatusPedido.Pendente;
            p.Itens = itens;
            p.ValorTotal = itens.Sum(i => i.Subtotal);

            pedidos.Add(p);
            _store.SavePedidos(pedidos);
            return p; 
        }

        public void AtualizarStatus(int pedidoId, StatusPedido novo)
        {
            var pedidos = _store.LoadPedidos();
            var p = pedidos.FirstOrDefault(x => x.Id == pedidoId);
            if (p == null) throw new Exception("Pedido não encontrado.");
            p.Status = novo;
            _store.SavePedidos(pedidos);
        }
    }
}
