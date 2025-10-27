// using: Estamos dizendo ao C# quais "caixas de ferramentas" (bibliotecas) vamos usar.
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

// --- BLOCO 1: CONFIGURAÇÃO DA API ---
var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados SQLite
builder.Services.AddDbContext<OrdemServicoDb>(opt =>
    opt.UseSqlite("Data Source=data/ordens.db"));

// Habilitando o CORS para permitir acesso externo
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// --- BLOCO JWT: AUTENTICAÇÃO E AUTORIZAÇÃO ---
var key = "chave-super-secreta-kodigos-2025-a";// 
var keyBytes = Encoding.UTF8.GetBytes(key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; //
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
    
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// --- BLOCO 2: ENDPOINTS ---

app.MapGet("/", () => "✅ API de Ordem de Serviço está funcionando!");

app.MapGet("/seguro", (ClaimsPrincipal user) =>
{
    var nome = user.Identity?.Name ?? "Usuário desconhecido";
    return $"🔐 Acesso autorizado com JWT! Bem-vindo, {nome}.";
}).RequireAuthorization();

app.MapPost("/login", (LoginRequest login) =>
{
    if (login.Usuario == "admin" && login.Senha == "123")
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.UTF8.GetBytes(key);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login.Usuario)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Results.Ok(new { token = tokenString });
    }

    return Results.Unauthorized();
});

app.MapPost("/ordensdeservico", async (OrdemServico os, OrdemServicoDb db) =>
{
    db.OrdensServico.Add(os);
    await db.SaveChangesAsync();
    return Results.Created($"/ordensdeservico/{os.Id}", os);
}).RequireAuthorization(); 
app.Run();

// --- BLOCO 3: MODELOS E CONTEXTO ---
public class OrdemServico
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Checklist { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}

class OrdemServicoDb : DbContext
{
    public OrdemServicoDb(DbContextOptions<OrdemServicoDb> options) : base(options) { }
    public DbSet<OrdemServico> OrdensServico { get; set; }
}

public record LoginRequest(string Usuario, string Senha);
