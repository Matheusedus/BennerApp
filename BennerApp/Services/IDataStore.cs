using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BennerApp.Models;

namespace BennerApp.Services
{
    public interface IDataStore
    {
        List<Pessoa> LoadPessoas(); void SavePessoas(List<Pessoa> pessoas);
        List<Produto> LoadProdutos(); void SaveProdutos(List<Produto> produtos);
        List<Pedido> LoadPedidos(); void SavePedidos(List<Pedido> pedidos);
        Sequences LoadSequences(); void SaveSequences(Sequences seq);
    }
}
