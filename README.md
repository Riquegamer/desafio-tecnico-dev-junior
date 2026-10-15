$readmeContent = @"
# 📚 Desafio Técnico: Encontre um Professor

Solução completa desenvolvida para o desafio técnico de recomendação de professores de idiomas. O projeto foi desenhado sob o rigor da **Clean Architecture** em **C# .NET 8** no backend, testado com **xUnit e FluentAssertions**, e apresenta uma interface construída em **React + TypeScript + Tailwind CSS (v4)** com estilo **Neumorphism**.

---

## 🛠️ Tecnologias Utilizadas

### Backend (.NET 8)
* **C# .NET 8** (API RESTful)
* **Clean Architecture** (Separação estrita em pastas unificadas: Domain, Application, Infrastructure, Api)
* **xUnit & FluentAssertions** (Testes unitários automatizados com stubs em memória)
* **System.Text.Json** (Desserialização otimizada via \`JsonDocument\`)

### Frontend
* **Vite + React + TypeScript**
* **Tailwind CSS v4** (com design customizado Neumorphism)
* **Plus Jakarta Sans** (tipografia principal)

---

## 📂 Arquitetura do Projeto

O código está isolado em pastas lógicas dentro de um único \`.csproj\` (para simplicidade de execução), garantindo separação total de responsabilidades:

\`\`\`text
api-desafio-tecnico-dev-junior/
├── src/
│   ├── Domain/              # Entidades (Professor, Disponibilidade) e Contratos (IProfessorRepository)
│   ├── Application/         # Regras de Negócio e DTOs (BuscaProfessorService)
│   ├── Infrastructure/      # Leitura de dados estáticos e persistência local (professores.json)
│   └── Api/                 # Controllers e Exposição de Endpoints REST
├── tests/                   # Testes unitários da regra de negócio (xUnit)
└── README.md
\`\`\`

\`\`\`text
desafio-tecnico-dev-junior-pagina-react
├── src/
│   ├── types/              # Tipos pesonalizados (AlunoBuscaRequest, ProfessorElegivel, ProfessorNaoElegivel, BuscaProfessorResponse)
│   ├── components/         # Componentes do projeto (FormBusca, ListaElegiveis, ListaNaoElegiveis)
│   ├── App.tsx             # Arquivo principal do aplicativo
│   ├── App.css             # Arquivo de estilo do aplicativo
│   ├── main.tsx            # Arquivo principal de renderização do projeto
│   └── Index.css           # Arquivo de estilo principal do projeto

\`\`\`

*(Nota: O aviso \`CS7022\` reportado nos logs do xUnit ocorre apenas pela coexistência da API web e do pacote de testes no mesmo \`.csproj\` e não afeta a cobertura de 100% de sucesso da bateria de testes).*

---

## ⚙️ Regras de Negócio Implementadas

O serviço de busca atende estritamente a todas as regras do desafio:

1. **Filtro de Idioma e Agenda:** Professores são filtrados pelo idioma e desqualificados imediatamente caso a flag \`agendaBloqueada\` seja \`true\`.
2. **Validação Estrita de Horários Fixo:** A verificação garante que a mesma janela de tempo esteja livre e seja válida simultaneamente em todos os dias solicitados.
3. **Limites e "Encostamento":** Foi garantido que a aula caberá integralmente na disponibilidade e que pode encostar no limite exato do início/término de aulas pré-existentes.
4. **Ordenação Inteligente:** A lista de elegíveis sempre prioriza professores com menor carga atual de aulas marcadas na semana e, em caso de empate, alfabeticamente.
5. **Classificação Rigorosa dos Não Elegíveis:** Geração de um array paralelo informando a recusa do professor estritamente na ordem exigida pelo sistema (ex: *Não ensina o idioma*, *Agenda bloqueada*, *Sem horário em comum*).

---

## 🚀 Diferenciais e Decisões de Engenharia ("Edge Cases")

Além do solicitado pelo escopo, as seguintes abordagens foram aplicadas pensando na escalabilidade real do produto:

* **Endpoint Dinâmico de Idiomas (Fim do Hardcode):**
  Foi implementado o endpoint extra \`GET /api/professores/idiomas\` que extrai diretamente do \`professores.json\` os idiomas ensinados. O Frontend (*React*) consome esse endpoint dinamicamente ao renderizar o select. Isso garante que a escola pode adicionar um professor de *Alemão* no repositório de dados sem a necessidade de uma única linha de deploy no código frontend.
* **Validação Unificada de Minutos e Parsing de Horários:**
  Tratamento inteligente de arredondamento fracionado (para ignorar horários quebrados como \`18:15\` em blocos válidos para início).
* **Isolamento Completo nos Testes:**
  Os testes do \`BuscaProfessorService\` rodam instantaneamente (\`~1.5s\`) através da injeção de Repositórios em Memória (\`Stubs\`), sem acessar o \`professores.json\` real ou depender do startup do Host da API.

---

## ⚙️ Como Executar o Projeto

### Pré-requisitos
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Node.js](https://nodejs.org/)

### 1. Rodar a Bateria de Testes Unitários
Na pasta api-desafio-tecnico-dev-junior, execute:
\`\`\`
dotnet test
\`\`\`

### 2. Rodar a API (.NET 8)
Na pasta api-desafio-tecnico-dev-junior, inicie o backend:
\`\`\`bash
dotnet run
\`\`\`
*(A API ficará disponível em portas como \`http://localhost:5080\` ou \`https://localhost:7051\`)*.

### 3. Executar o Frontend
Em um terminal separado, navegue até a pasta desafio-tecnico-dev-junior-pagina-react, instale as dependências e rode o ambiente Vite:
\`\`\`bash
npm install
npm run dev
\`\`\`

---

## 📋 Exemplo de Comunicação (POST /api/professores/busca)

**Request:**
\`\`\`json
{
  "idioma": "Espanhol",
  "dias": ["segunda"],
  "inicioFaixa": "18:00",
  "fimFaixa": "20:00",
  "duracaoMinutos": 60
}
\`\`\`

**Response:**
\`\`\`json
{
  "professores": [
    { "id": 1, "nome": "Ana", "aulasNaSemana": 1, "horarios": ["18:00", "20:00"] }
  ],
  "naoElegiveis": [
    { "id": 2, "nome": "Bruno", "motivo": "Não ensina o idioma" }
  ]
}
\`\`\`
"@