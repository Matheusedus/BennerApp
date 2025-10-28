using BennerApp.Models;
using BennerApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.ViewModels
{
    public class ProdutosViewModel : BaseViewModel
    {
        private readonly IDataStore _store;
        private readonly IdGenerator _ids;

        private List<Produto> _all;
        public ObservableCollection<Produto> Produtos { get; private set; }

        private string _fNome, _fCodigo; private decimal? _min, _max;
        public string FiltroNome { get { return _fNome; } set { Set(ref _fNome, value); Filtrar(); } }
        public string FiltroCodigo { get { return _fCodigo; } set { Set(ref _fCodigo, value); Filtrar(); } }
        public decimal? ValorMin { get { return _min; } set { Set(ref _min, value); Filtrar(); } }
        public decimal? ValorMax { get { return _max; } set { Set(ref _max, value); Filtrar(); } }

        public ProdutosViewModel(IDataStore store, IdGenerator ids)
        {
            _store = store; _ids = ids;
            _all = _store.LoadProdutos();
            Produtos = new ObservableCollection<Produto>(_all);
        }

        public void Incluir(string nome, string codigo, decimal valor)
        {
            var p = new Produto { Id = _ids.NextProduto(), Nome = nome, Codigo = codigo, Valor = valor };
            _all.Add(p); _store.SaveProdutos(_all); Filtrar();
        }

        public void Excluir(Produto p)
        {
            if (p == null) return;
            _all.RemoveAll(x => x.Id == p.Id); _store.SaveProdutos(_all); Filtrar();
        }

        public void SalvarMudancas()
        {
            _all = Produtos.ToList();
            _store.SaveProdutos(_all);
            Filtrar();
        }

        public void ExcluirSelecionado(Produto sel)
        {
            if (sel == null) return;
            _all.RemoveAll(x => x.Id == sel.Id);
            _store.SaveProdutos(_all);
            Filtrar();
        }

        public void Filtrar()
        {
            IEnumerable<Produto> q = _all;
            if (!string.IsNullOrWhiteSpace(FiltroNome))
                q = q.Where(x => (x.Nome ?? "").ToLower().Contains(FiltroNome.ToLower()));
            if (!string.IsNullOrWhiteSpace(FiltroCodigo))
                q = q.Where(x => (x.Codigo ?? "").ToLower().Contains(FiltroCodigo.ToLower()));
            if (ValorMin.HasValue) q = q.Where(x => x.Valor >= ValorMin.Value);
            if (ValorMax.HasValue) q = q.Where(x => x.Valor <= ValorMax.Value);

            Produtos.Clear();
            foreach (var p in q.OrderBy(x => x.Nome)) Produtos.Add(p);
        }
    }
}
