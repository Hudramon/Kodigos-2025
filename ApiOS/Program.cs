// using: Estamos dizendo ao C# quais "caixas de ferramentas" (bibliotecas) vamos usar.
using Microsoft.EntityFrameworkCore;

// --- BLOCO 1: CONFIGURAÇÃO DA API ---
// Aqui nós preparamos e configuramos nosso aplicativo.
var builder = WebApplication.CreateBuilder(args);

// Linha crucial: Estamos dizendo para a API: "Você vai usar um banco de dados.
// O tipo é SQLite e o arquivo físico se chamará 'ordens.db'".
builder.Services.AddDbContext<OrdemServicoDb>(opt => opt.UseSqlite("Data Source=data/ordens.db"));
// Habilitando o CORS. É uma configuração de segurança que permite
// que nosso futuro arquivo HTML (que rodará em um endereço)
// possa conversar com nossa API (que rodará em outro).
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => 
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// ...
var app = builder.Build();
app.UseCors("AllowAll"); // APLICA A PERMISSÃO TOTAL


// --- BLOCO 2: OS ENDPOINTS (AS URLS DA API) ---
// Endpoints são as URLs que nossa API vai entender e responder.

// Um endpoint de teste. Se você acessar a URL principal, ele responde isso.
app.MapGet("/", () => "API de Ordem de Serviço está funcionando!");

// O endpoint principal! Ele escuta na URL "/ordensdeservico".
// "MapPost" significa que ele só aceita requisições do tipo POST (usadas para CRIAR dados).
app.MapPost("/ordensdeservico", async (OrdemServico os, OrdemServicoDb db) =>
{
    // Quando o frontend enviar os dados, o .NET vai automaticamente:
    // 1. Transformar os dados em um objeto C# do tipo 'OrdemServico' (a variável 'os').
    // 2. Nos dar acesso ao banco de dados (a variável 'db').

    db.OrdensServico.Add(os);        // Prepara para adicionar a nova ordem de serviço.
    await db.SaveChangesAsync();       // Efetivamente salva no banco de dados.
    return Results.Created($"/ordensdeservico/{os.Id}", os); // Retorna uma resposta de sucesso.
});

// Manda a aplicação rodar e ficar escutando por requisições.
app.Run();


// --- BLOCO 3: DEFINIÇÃO DOS DADOS (MODELOS E CONTEXTO) ---
// Aqui nós descrevemos a ESTRUTURA dos nossos dados.

// 1. O Modelo (A "Planta" da Tabela):
// Esta classe define como uma Ordem de Serviço se parece.
// O EF Core vai usar isso para criar uma tabela chamada "OrdensServico" no banco.
// Cada "prop" (Id, Descricao) vira uma coluna na tabela.
public class OrdemServico
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public string Checklist { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}

// 2. O Contexto (A "Conexão" com o Banco):
// Esta classe representa o nosso banco de dados no código.
class OrdemServicoDb : DbContext
{
    public OrdemServicoDb(DbContextOptions<OrdemServicoDb> options) : base(options) { }

    // Esta linha diz: "Dentro deste banco, haverá uma tabela de Ordens de Serviço".
    public DbSet<OrdemServico> OrdensServico { get; set; }
}