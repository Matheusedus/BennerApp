using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public PessoasViewModel Pessoas { get; private set; }
        public ProdutosViewModel Produtos { get; private set; }
        public PedidosViewModel Pedidos { get; private set; }

        public MainViewModel(PessoasViewModel pvm, ProdutosViewModel prodvm, PedidosViewModel pedvm)
        {
            Pessoas = pvm; Produtos = prodvm; Pedidos = pedvm;
        }
    }
}
