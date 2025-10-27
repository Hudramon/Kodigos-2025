using Microsoft.EntityFrameworkCore;

// --- BLOCO 1: CONFIGURAÇÃO DA API ---
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrdemServicoDb>(opt => opt.UseSqlite("Data Source=data/ordens.db"));
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => 
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// ...
var app = builder.Build();
app.UseCors("AllowAll"); 
// Um endpoint de teste. Se você acessar a URL principal, ele responde isso.
app.MapGet("/", () => "API de Ordem de Serviço está funcionando!");

app.MapPost("/ordensdeservico", async (OrdemServico os, OrdemServicoDb db) =>
{

    db.OrdensServico.Add(os);        // Prepara para adicionar a nova ordem de serviço.
    await db.SaveChangesAsync();       // salva no banco de dados.
    return Results.Created($"/ordensdeservico/{os.Id}", os); // Retorna uma resposta de sucesso.
});

app.Run();
public class OrdemServico
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public string Checklist { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
class OrdemServicoDb : DbContext
{
    public OrdemServicoDb(DbContextOptions<OrdemServicoDb> options) : base(options) { }

    public DbSet<OrdemServico> OrdensServico { get; set; }
}
