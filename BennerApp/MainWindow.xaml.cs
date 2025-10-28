using BennerApp.Models;
using BennerApp.Services;
using BennerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BennerApp
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel VM { get; private set; }

        private readonly PessoasViewModel _pessoasVM;
        private readonly ProdutosViewModel _produtosVM;
        private readonly PedidosViewModel _pedidosVM;

        public MainWindow()
        {
            InitializeComponent();

            // serviços de infraestrutura
            IDataStore store = new JsonDataStore();
            var ids = new IdGenerator(store);

            // viewmodels
            _pessoasVM = new PessoasViewModel(store, ids);
            _produtosVM = new ProdutosViewModel(store, ids);
            _pedidosVM = new PedidosViewModel(store, ids);

            // vm raiz para tabs
            VM = new MainViewModel(_pessoasVM, _produtosVM, _pedidosVM);
            DataContext = VM;
        }

        // ====== PESSOAS ======
        private void AddPessoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nome = TxtNomePessoa.Text.Trim();
                string cpf = TxtCpfPessoa.Text.Trim();
                string endereco = TxtEnderecoPessoa.Text.Trim();

                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf))
                {
                    MessageBox.Show("Nome e CPF são obrigatórios.", "Atenção");
                    return;
                }

                if (!CpfValidator.IsValid(cpf))
                {
                    MessageBox.Show("CPF inválido.", "Atenção");
                    return;
                }

                // Verifica duplicado
                if (_pessoasVM.Pessoas.Any(p => p.Cpf == cpf))
                {
                    MessageBox.Show("Já existe uma pessoa com esse CPF.", "Atenção");
                    return;
                }

                _pessoasVM.Incluir(nome, cpf, endereco);
                MessageBox.Show("Pessoa incluída com sucesso!", "OK");

                // limpa campos
                TxtNomePessoa.Text = "";
                TxtCpfPessoa.Text = "";
                TxtEnderecoPessoa.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao incluir pessoa: {ex.Message}");
            }
        }
        private void LimparFiltrosPessoas_Click(object sender, RoutedEventArgs e)
        {
            VM.Pessoas.FiltroNome = string.Empty;
            VM.Pessoas.FiltroCpf = string.Empty;
        }

        private void SalvarPessoas_Click(object sender, RoutedEventArgs e)
        {
            _pessoasVM.SalvarMudancas();
            MessageBox.Show("Pessoas salvas.", "OK");
        }

        private void ExcluirPessoa_Click(object sender, RoutedEventArgs e)
        {
            var p = PessoasGrid.SelectedItem as Pessoa;
            if (p == null) return;
            if (MessageBox.Show($"Excluir '{p.Nome}'?", "Confirmar", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            _pessoasVM.ExcluirSelecionado(p);
        }

        // Quando seleciona uma pessoa, carrega os pedidos dela
        private void PessoasGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var pessoa = PessoasGrid.SelectedItem as Pessoa;
            VM.Pessoas.SelecionarPessoa(pessoa);
        }

        // Botões de ação na linha do pedido
        private void PedidoPago_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((FrameworkElement)sender).Tag);
            VM.Pessoas.MarcarPago(id);
        }

        private void PedidoEnviado_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((FrameworkElement)sender).Tag);
            VM.Pessoas.MarcarEnviado(id);
        }

        private void PedidoRecebido_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((FrameworkElement)sender).Tag);
            VM.Pessoas.MarcarRecebido(id);
        }

        // ====== PRODUTOS ======
        private void AddProduto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nome = TxtNomeProduto.Text.Trim();
                string codigo = TxtCodigoProduto.Text.Trim();
                string valorStr = TxtValorProduto.Text.Trim();

                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(valorStr))
                {
                    MessageBox.Show("Nome, código e valor são obrigatórios.", "Atenção");
                    return;
                }

                if (!decimal.TryParse(valorStr, out decimal valor) || valor <= 0)
                {
                    MessageBox.Show("Informe um valor válido maior que zero.", "Atenção");
                    return;
                }

                if (_produtosVM.Produtos.Any(p => p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Já existe um produto com esse código.", "Atenção");
                    return;
                }

                _produtosVM.Incluir(nome, codigo, valor);
                MessageBox.Show("Produto incluído com sucesso!", "OK");

                TxtNomeProduto.Text = "";
                TxtCodigoProduto.Text = "";
                TxtValorProduto.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao incluir produto: {ex.Message}");
            }
        }
        private void LimparFiltrosProdutos_Click(object sender, RoutedEventArgs e)
        {
            VM.Produtos.FiltroNome = string.Empty;
            VM.Produtos.FiltroCodigo = string.Empty;
            VM.Produtos.ValorMin = null;
            VM.Produtos.ValorMax = null;
        }
        private void SalvarProdutos_Click(object sender, RoutedEventArgs e)
        {
            _produtosVM.SalvarMudancas();
            MessageBox.Show("Produtos salvos.", "OK");
        }

        private void ExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            var prod = ProdutosGrid.SelectedItem as Produto;
            if (prod == null) return;
            if (MessageBox.Show($"Excluir '{prod.Nome}'?", "Confirmar", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            _produtosVM.ExcluirSelecionado(prod);
        }

        // ====== PEDIDOS ======
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var prod = CbProduto.SelectedItem as Produto;
            int qtd;
            if (prod != null && int.TryParse(TxtQtd.Text, out qtd) && qtd > 0)
                VM.Pedidos.AddItem(prod, qtd);
        }

        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VM.Pedidos.Finalizar();
                MessageBox.Show("Pedido finalizado e salvo!", "OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
        }

        private void IncluirPedido_Click(object sender, RoutedEventArgs e)
        {
            var pessoa = PessoasGrid.SelectedItem as Pessoa;
            if (pessoa == null)
            {
                MessageBox.Show("Selecione uma pessoa antes de incluir o pedido.", "Atenção");
                return;
            }

            // 1) Seleciona pessoa na VM de pedidos e limpa itens
            VM.Pedidos.PessoaSelecionada = pessoa;
            // garante novo pedido "zerado"
            while (VM.Pedidos.Itens.Count > 0) VM.Pedidos.RemoverItem(VM.Pedidos.Itens[0]);
            VM.Pedidos.Forma = BennerApp.Models.FormaPagamento.Dinheiro;
            VM.Pedidos.Finalizado = false;
            VM.Pedidos.Total = 0m;

            // 2) Vai para a aba "Pedidos" (índice 2 se as abas estiverem na mesma ordem)
            Tabs.SelectedIndex = 2;
        }

    }
}
