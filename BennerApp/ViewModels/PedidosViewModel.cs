using BennerApp.Models;
using BennerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BennerApp.ViewModels
{
    public class PedidosViewModel : BaseViewModel
    {
        private readonly IDataStore _store;
        private readonly IdGenerator _ids;
        private readonly PedidoService _service;

        public ObservableCollection<Pessoa> Pessoas { get; }
        public ObservableCollection<Produto> Produtos { get; }
        public ObservableCollection<ItemPedido> Itens { get; }

        // --- Seleção de Pessoa por Id (corrige SelectedItem por referência) ---
        private int? _pessoaSelecionadaId;
        public int? PessoaSelecionadaId
        {
            get { return _pessoaSelecionadaId; }
            set
            {
                Set(ref _pessoaSelecionadaId, value);
                PessoaSelecionada = Pessoas.FirstOrDefault(p => p.Id == _pessoaSelecionadaId);
            }
        }

        private Pessoa _pessoaSelecionada;
        public Pessoa PessoaSelecionada
        {
            get { return _pessoaSelecionada; }
            set { Set(ref _pessoaSelecionada, value); }
        }

        // (Opcional) Selecionar produto por Id no ComboBox
        private int? _produtoSelecionadoId;
        public int? ProdutoSelecionadoId
        {
            get { return _produtoSelecionadoId; }
            set { Set(ref _produtoSelecionadoId, value); }
        }

        private FormaPagamento _forma;
        public FormaPagamento Forma
        {
            get { return _forma; }
            set { Set(ref _forma, value); }
        }

        private decimal _total;
        public decimal Total
        {
            get { return _total; }
            set { Set(ref _total, value); }
        }

        private bool _finalizado;
        public bool Finalizado
        {
            get { return _finalizado; }
            set { Set(ref _finalizado, value); }
        }

        public PedidosViewModel(IDataStore store, IdGenerator ids)
        {
            _store = store;
            _ids = ids;
            _service = new PedidoService(_store, _ids);

            Pessoas = new ObservableCollection<Pessoa>(_store.LoadPessoas());
            Produtos = new ObservableCollection<Produto>(_store.LoadProdutos());
            Itens = new ObservableCollection<ItemPedido>();
        }

        public void AddItem(Produto prod, int qtd)
        {
            if (prod == null || qtd <= 0) return;

            var existente = Itens.FirstOrDefault(i => i.ProdutoId == prod.Id);
            if (existente != null)
            {
                existente.Quantidade += qtd;
            }
            else
            {
                Itens.Add(new ItemPedido
                {
                    ProdutoId = prod.Id,
                    NomeProduto = prod.Nome,
                    ValorUnit = prod.Valor,
                    Quantidade = qtd
                });
            }
            Recalcular();
        }

        public void RemoverItem(ItemPedido item)
        {
            if (item == null) return;
            Itens.Remove(item);
            Recalcular();
        }

        private void Recalcular()
        {
            Total = Itens.Sum(i => i.Subtotal);
        }

        public Pedido Finalizar()
        {
            if (PessoaSelecionada == null) throw new Exception("Selecione a pessoa.");
            if (Itens.Count == 0) throw new Exception("Adicione pelo menos um item.");

            var pedido = _service.FinalizarPedido(PessoaSelecionada.Id, Forma, Itens.ToList());
            Finalizado = true; // trava edição na UI
            return pedido;
        }
    }
}