# 🧩 BennerApp — Teste Técnico C# (.NET Framework 4.6 / WPF)

Aplicação desktop desenvolvida em **C# com WPF (.NET Framework 4.6)** para cadastro e manipulação de dados de **Pessoas**, **Produtos** e **Pedidos**, conforme o teste técnico da Benner.

O objetivo é demonstrar:
- Estruturação de aplicação **MVVM**
- Persistência de dados em **JSON**
- Manipulação com **LINQ**
- Interface desktop funcional e organizada

---

## 🚀 Tecnologias Utilizadas

| Categoria | Ferramenta |
|------------|-------------|
| Linguagem | C# (.NET Framework 4.6) |
| Interface | WPF (Windows Presentation Foundation) |
| Padrão | MVVM (Model - View - ViewModel) |
| Persistência | Arquivos JSON (via `JsonDataStore`) |
| Consultas | LINQ |
| IDE | Visual Studio 2022 (Community) |

---

## 📂 Estrutura do Projeto

```
BennerApp/
├── Models/           # Classes de domínio (Pessoa, Produto, Pedido, enums)
├── ViewModels/       # Lógica de apresentação (MVVM)
├── Views/            # Telas XAML (MainWindow + abas)
├── Services/         # Regras de negócio e persistência (JSON, validações)
├── Resources/        # Conversores e estilos (formatadores WPF)
├── Data/             # Arquivos JSON gerados automaticamente
└── README.md         # Este arquivo
```

---

## 🧠 Entidades e Regras de Negócio

### 🧍 Pessoa
| Campo | Regra |
|--------|--------|
| Id | Automático (somente leitura) |
| Nome | Obrigatório |
| CPF | Obrigatório, válido e único |
| Endereço | Opcional |

### 📦 Produto
| Campo | Regra |
|--------|--------|
| Id | Automático (somente leitura) |
| Nome | Obrigatório |
| Código | Obrigatório e único |
| Valor | Obrigatório (> 0) |
| Pesquisa | Filtros por Nome, Código e faixa de Valor |

### 🧾 Pedido
| Campo | Regra |
|--------|--------|
| Id | Automático (somente leitura) |
| Pessoa | Obrigatório (relacionamento) |
| Produtos | Obrigatório (com quantidade) |
| Valor Total | Calculado automaticamente |
| Data da Venda | Data atual no momento da finalização |
| Forma de Pagamento | Dinheiro, Cartão ou Boleto |
| Status | Começa como “Pendente”, pode ser alterado para “Pago”, “Enviado” ou “Recebido” |

---

## 💡 Funcionalidades por Tela

### 👤 Aba **Pessoas**
- Filtros: por **Nome** e **CPF**
- Formulário para inclusão rápida
- Ações: **Salvar mudanças** e **Excluir selecionado**
- Botão **Incluir Pedido**:
  - Abre a aba de Pedidos já vinculada à pessoa selecionada
- Grid de **Pedidos da Pessoa**:
  - Mostra Data, Valor Total, Pagamento e Status
  - Botões por linha:
    - ✅ Pago
    - 📦 Enviado
    - 📬 Recebido
  - Filtros adicionais:
    - Mostrar apenas **Entregues**, **Pagos** ou **Pendentes**

### 📦 Aba **Produtos**
- Filtros: Nome, Código e faixa de valor (mín./máx.)
- Formulário para inclusão de produto
- Ações: **Salvar mudanças**, **Excluir selecionado**
- IDs automáticos e valores formatados em moeda (R$)

### 🧾 Aba **Pedidos**
- Seleção de Pessoa e Forma de Pagamento
- Adição de múltiplos produtos + quantidade
- Cálculo automático do valor total
- Botão **Finalizar Pedido**:
  - Calcula total
  - Define Status = “Pendente”
  - Define Data atual
  - Salva no arquivo JSON
  - Bloqueia edição após finalização

---

## 💾 Persistência de Dados

Os dados são armazenados no diretório:

```
/BennerApp/bin/Debug/Data/
```

Arquivos:
- `pessoas.json`
- `produtos.json`
- `pedidos.json`

> Criados automaticamente ao rodar o sistema.

A manipulação é feita via **LINQ**, com operações de filtro, busca e projeção.

---

## ⚙️ Requisitos para Executar

- **Windows 10 ou 11**
- **Visual Studio 2022**
- Workload: “Desenvolvimento para desktop com .NET”
- SDK/Targeting Pack do **.NET Framework 4.6**

---

## ▶️ Como Executar

1. Clone o repositório  
   ```bash
   git clone https://github.com/seuusuario/BennerApp.git
   ```
2. Abra o arquivo `BennerApp.sln` no **Visual Studio**
3. Garanta que o projeto usa **.NET Framework 4.6**
4. Pressione **F5** para compilar e executar

---

## 🧮 Exemplo de Fluxo de Uso

1. Vá em **Pessoas** → cadastre algumas pessoas  
2. Vá em **Produtos** → cadastre alguns produtos  
3. Selecione uma pessoa → clique em **Incluir Pedido**  
4. Adicione produtos e finalize o pedido  
5. Volte à aba **Pessoas** → veja o pedido no grid inferior  
6. Use os botões “Pago”, “Enviado” e “Recebido” para mudar status  
7. Filtre por “Apenas pagos” ou “Apenas entregues”

---

## 🧩 Conceitos Demonstrados

✔️ Padrão **MVVM** (separação de camadas)  
✔️ Manipulação de coleções com **LINQ**  
✔️ Persistência em **JSON**  
✔️ Validação de CPF e regras de negócio  
✔️ Interface **WPF** organizada, com **bindings** e **conversores**  
✔️ Controle de fluxo de pedidos e atualização automática de status  

---

## 👨‍💻 Autor

**Matheus Teixeira**  
📧 matheuslega@hotmail.com.br  

---

> Desenvolvido como parte do **Teste Técnico – Desenvolvedor C# / Benner**.
