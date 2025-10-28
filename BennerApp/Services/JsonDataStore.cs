using BennerApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using JsonFormatting = Newtonsoft.Json.Formatting;

namespace BennerApp.Services
{
    public class JsonDataStore : IDataStore
    {
        private readonly string _basePath;

        public JsonDataStore()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        }

        private T Read<T>(string file, T fallback)
        {
            var path = Path.Combine(_basePath, file);
            if (!File.Exists(path)) return fallback;
            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return fallback;
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj == null ? fallback : obj;
        }

        private void Write<T>(string file, T data)
        {
            Directory.CreateDirectory(_basePath);
            var path = Path.Combine(_basePath, file);
            var json = JsonConvert.SerializeObject(data, JsonFormatting.Indented);
            File.WriteAllText(path, json);
        }

        public List<Pessoa> LoadPessoas() { return Read("pessoas.json", new List<Pessoa>()); }
        public void SavePessoas(List<Pessoa> v) { Write("pessoas.json", v); }

        public List<Produto> LoadProdutos() { return Read("produtos.json", new List<Produto>()); }
        public void SaveProdutos(List<Produto> v) { Write("produtos.json", v); }

        public List<Pedido> LoadPedidos() { return Read("pedidos.json", new List<Pedido>()); }
        public void SavePedidos(List<Pedido> v) { Write("pedidos.json", v); }

        public Sequences LoadSequences() { return Read("sequences.json", new Sequences()); }
        public void SaveSequences(Sequences seq) { Write("sequences.json", seq); }
    }
}
