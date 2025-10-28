using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.Models
{
    public class Pessoa
    {
        public int Id { get; set; }              // auto
        public string Nome { get; set; }         // obrigatório
        public string Cpf { get; set; }          // obrigatório + validação
        public string Endereco { get; set; }     // opcional
    }
}
