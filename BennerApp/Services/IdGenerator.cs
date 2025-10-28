using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.Services
{
    public class IdGenerator
    {
        private readonly IDataStore _store;
        private Sequences _seq;

        public IdGenerator(IDataStore store)
        {
            _store = store;
            _seq = _store.LoadSequences() ?? new Sequences();
        }

        public int NextPessoa() { _seq.Pessoa++; _store.SaveSequences(_seq); return _seq.Pessoa; }
        public int NextProduto() { _seq.Produto++; _store.SaveSequences(_seq); return _seq.Produto; }
        public int NextPedido() { _seq.Pedido++; _store.SaveSequences(_seq); return _seq.Pedido; }
    }
}
