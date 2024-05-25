using api_receita.DAL.Database;
using api_receita.DAL.Interfaces;
using api_receita.DAL.Repositorys;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Carregar as variáveis de ambiente do arquivo .env
DotNetEnv.Env.Load();

// Obter a string de conexão do ambiente
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

// Adicionar serviços ao contêiner
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext com a string de conexão carregada
builder.Services.AddDbContext<AppReceitasDbContext>(options =>
    options.UseNpgsql(connectionString));

// Registrar repositórios
builder.Services.AddTransient<IUser, UserRepository>();

var app = builder.Build();

// Configurar o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
