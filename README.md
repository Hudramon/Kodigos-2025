# Sistema de Ordem de Serviço (Protótipo Full-Stack)

Um protótipo completo de um sistema para registro de Ordens de Serviço (OS), construído com uma interface moderna em Vue.js, uma API RESTful em .NET 8 e totalmente containerizável com Docker para garantir portabilidade e facilidade de execução.

---

### ✨ Demonstração

*Tela de Login com design "Glassmorphism" e ícones de suporte.*

*Tela principal para registro de OS, com componentes customizados e layout responsivo.*

---

### 🚀 Funcionalidades

* **Interface Moderna:** Design responsivo com efeito "Glassmorphism", tipografia profissional (Inter) e componentes de formulário totalmente customizados.
* **Autenticação Persistente:** O estado de login é mantido entre recarregamentos de página utilizando `sessionStorage`, melhorando a experiência do usuário.
* **Criação de OS:** Formulário completo e intuitivo para registro de serviços, incluindo:
    * Campo de descrição detalhada.
    * Checklist de verificação com interações visuais.
    * Upload de imagem para comprovação com pré-visualização.
* **Backend Robusto:** API RESTful construída com a simplicidade e performance do .NET 8 Minimal API.
* **Banco de Dados:** Utiliza SQLite gerenciado pelo Entity Framework Core com a abordagem Code-First.
* **Containerização Completa:** Inclui um `Dockerfile` multi-estágio otimizado que empacota a API em uma imagem leve, garantindo que o projeto rode em qualquer ambiente com Docker.

---

### 🛠️ Tecnologias Utilizadas

* **Frontend:** HTML5, CSS3, JavaScript, **Vue.js 3** (via CDN)
* **Backend:** **C#** com **.NET 8** (Minimal API)
* **Banco de Dados:** **SQLite** com **Entity Framework Core 8** (Code-First)
* **Containerização:** **Docker**

---

### 🐳 Como Executar (com Docker)

Este é o método recomendado. Garanta que você tenha o [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução.

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/SEU_USUARIO/NOME_DO_SEU_REPOSITORIO.git](https://github.com/SEU_USUARIO/NOME_DO_SEU_REPOSITORIO.git)
    cd NOME_DO_SEU_REPOSITORIO
    ```

2.  **Construa a imagem Docker:** Este comando lê o `Dockerfile` e empacota a aplicação.
    ```bash
    docker build -t os-service-api .
    ```

3.  **Execute o container:** Este comando inicia a aplicação e conecta a porta local `5006` à porta do container, além de persistir os dados do banco.
    ```bash
    # Para Windows (usando Command Prompt ou PowerShell)
    docker run -d -p 5006:8080 --name os-service-container -v "%cd%/data:/app/data" os-service-api
    
    # Para MacOS ou Linux
    docker run -d -p 5006:8080 --name os-service-container -v "$(pwd)/data:/app/data" os-service-api
    ```

4.  **Acesse a aplicação** no seu navegador em: `http://localhost:5006`. O frontend (`index.html`) pode ser aberto diretamente no navegador.

---

### 👨‍💻 Execução Manual (Ambiente de Desenvolvimento)

Para executar o projeto sem Docker, você precisará ter o **.NET 8 SDK** instalado.

1.  **Clone o repositório** (se ainda não o fez).

2.  **Inicie o Backend (API):**
    * Abra um terminal e navegue até a pasta da API: `cd ApiOS`
    * Execute o comando para criar o banco de dados e iniciar a API:
        ```bash
        dotnet ef database update
        dotnet run
        ```
    * *Deixe este terminal rodando.* A API estará disponível, provavelmente em uma porta como `5006`. Verifique a saída do terminal.

3.  **Inicie o Frontend:**
    * Abra o arquivo `index.html` em seu navegador. A forma mais fácil é usar a extensão **Live Server** no VS Code.

---

### 📂 Estrutura do Projeto

```
/
├── ApiOS/                # Contém todo o projeto do backend em .NET
│   ├── Migrations/       # Arquivos de migração do Entity Framework
│   ├── Properties/       # Configurações de lançamento
│   ├── Program.cs        # O coração da API (configuração e endpoints)
│   ├── ApiOS.csproj      # Arquivo de definição do projeto .NET
│   └── ...
├── .dockerignore         # Especifica arquivos a serem ignorados pelo Docker
├── .gitignore            # Especifica arquivos a serem ignorados pelo Git
├── Dockerfile            # Instruções para construir a imagem Docker da API
├── index.html            # A estrutura da interface do usuário (View)
├── script.js             # A lógica do frontend (ViewModel)
├── style.css             # O design e a aparência da interface (Style)
└── README.md             # Esta documentação
```
