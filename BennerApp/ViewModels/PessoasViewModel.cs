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
    public class PessoasViewModel : BaseViewModel
    {
        private readonly IDataStore _store;
        private readonly IdGenerator _ids;
        private readonly PedidoService _pedidoService;

        private List<Pessoa> _all;
        public ObservableCollection<Pessoa> Pessoas { get; private set; }

        private string _filtroNome;
        public string FiltroNome { get { return _filtroNome; } set { Set(ref _filtroNome, value); Filtrar(); } }

        private string _filtroCpf;
        public string FiltroCpf { get { return _filtroCpf; } set { Set(ref _filtroCpf, value); Filtrar(); } }

        // ===== Pedidos da pessoa selecionada =====
        private Pessoa _pessoaAtual;
        private List<Pedido> _allPedidosDaPessoa;
        public ObservableCollection<Pedido> PedidosDaPessoa { get; private set; }

        // Filtros extras
        private bool _apenasEntregues, _apenasPagos, _apenasPendentes;
        public bool ApenasEntregues { get { return _apenasEntregues; } set { Set(ref _apenasEntregues, value); FiltrarPedidos(); } }
        public bool ApenasPagos { get { return _apenasPagos; } set { Set(ref _apenasPagos, value); FiltrarPedidos(); } }
        public bool ApenasPendentes { get { return _apenasPendentes; } set { Set(ref _apenasPendentes, value); FiltrarPedidos(); } }

        public PessoasViewModel(IDataStore store, IdGenerator ids)
        {
            _store = store;
            _ids = ids;
            _pedidoService = new PedidoService(_store, _ids);

            _all = _store.LoadPessoas();
            Pessoas = new ObservableCollection<Pessoa>(_all);

            PedidosDaPessoa = new ObservableCollection<Pedido>();
            _allPedidosDaPessoa = new List<Pedido>();
        }

        // ===== CRUD básico de Pessoas =====
        public void Incluir(string nome, string cpf, string endereco)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf)) return;
            if (!CpfValidator.IsValid(cpf)) return;

            if (_all.Any(x => (x.Cpf ?? "") == cpf))
                throw new Exception("CPF já cadastrado.");

            var p = new Pessoa { Id = _ids.NextPessoa(), Nome = nome, Cpf = cpf, Endereco = endereco };
            _all.Add(p);
            _store.SavePessoas(_all);
            Filtrar();
        }

        public void SalvarMudancas()
        {
            _all = Pessoas.ToList();
            _store.SavePessoas(_all);
            Filtrar();
        }

        public void ExcluirSelecionado(Pessoa selecionada)
        {
            if (selecionada == null) return;
            _all.RemoveAll(x => x.Id == selecionada.Id);
            _store.SavePessoas(_all);
            Filtrar();
            if (_pessoaAtual != null && _pessoaAtual.Id == selecionada.Id)
                SelecionarPessoa(null); // limpa grid de pedidos
        }

        public void Filtrar()
        {
            IEnumerable<Pessoa> q = _all;
            if (!string.IsNullOrWhiteSpace(FiltroNome))
                q = q.Where(p => (p.Nome ?? "").ToLower().Contains(FiltroNome.ToLower()));
            if (!string.IsNullOrWhiteSpace(FiltroCpf))
                q = q.Where(p => (p.Cpf ?? "").Contains(FiltroCpf));

            Pessoas.Clear();
            foreach (var p in q.OrderBy(x => x.Nome)) Pessoas.Add(p);
        }

        // ===== Seleção e filtros de pedidos =====
        public void SelecionarPessoa(Pessoa p)
        {
            _pessoaAtual = p;
            if (p == null)
            {
                _allPedidosDaPessoa = new List<Pedido>();
                PedidosDaPessoa.Clear();
                return;
            }

            var todos = _store.LoadPedidos();
            _allPedidosDaPessoa = todos.Where(x => x.PessoaId == p.Id)
                                       .OrderByDescending(x => x.DataVenda)
                                       .ToList();
            FiltrarPedidos();
        }

        public void FiltrarPedidos()
        {
            IEnumerable<Pedido> q = _allPedidosDaPessoa;

            if (ApenasEntregues) q = q.Where(x => x.Status == StatusPedido.Recebido);
            if (ApenasPagos) q = q.Where(x => x.Status == StatusPedido.Pago);
            if (ApenasPendentes) q = q.Where(x => x.Status == StatusPedido.Pendente);

            PedidosDaPessoa.Clear();
            foreach (var ped in q) PedidosDaPessoa.Add(ped);
        }

        // ===== Ações por linha =====
        public void MarcarPago(int pedidoId)
        {
            _pedidoService.AtualizarStatus(pedidoId, StatusPedido.Pago);
            // recarrega mantendo filtros
            SelecionarPessoa(_pessoaAtual);
        }
        public void MarcarEnviado(int pedidoId)
        {
            _pedidoService.AtualizarStatus(pedidoId, StatusPedido.Enviado);
            SelecionarPessoa(_pessoaAtual);
        }
        public void MarcarRecebido(int pedidoId)
        {
            _pedidoService.AtualizarStatus(pedidoId, StatusPedido.Recebido);
            SelecionarPessoa(_pessoaAtual);
        }
    }
}
