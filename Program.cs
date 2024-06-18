using api_receita.DAL.Database;
using api_receita.DAL.Interfaces;
using api_receita.DAL.Repositorys;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using app_receitas_api.Settings;
using Microsoft.OpenApi.Models;
using app_receitas_api.DAL.Interfaces;
using app_receitas_api.DAL.Repositorys;


var builder = WebApplication.CreateBuilder(args);

// Carregar as variáveis de ambiente do arquivo .env
DotNetEnv.Env.Load();

// Obter a string de conexão do ambiente
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_LOCAL");

//var connectionString = "Host= sneeringly-large-fisher.data-1.use1.tembo.io; Username= postgres;Port= 5432 ;Password = PmfkKzAv6Ss8ck7H; Database = db_recipes";

// Adicionar serviços ao contêiner
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


// Configurar DbContext com a string de conexão carregada
builder.Services.AddDbContext<ReceitasDbContext>(options =>
    options.UseNpgsql(connectionString));

// Registrar repositórios
builder.Services.AddTransient<IUser, UserRepository>();
builder.Services.AddTransient<IRecipe, RecipeRepository>();
builder.Services.AddTransient<ICategory, CategoryRepository>();
builder.Services.AddTransient<IRating, RatingRepository>();
builder.Services.AddTransient<IFavorite, FavoroteRepository>();

var key = Encoding.ASCII.GetBytes(Config.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configurar o pipeline de requisição HTTP
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// app.Run("http://192.168.42.35:3000");

app.Run("http://192.168.88.93:3000");

// app.Run("http://192.168.1.8:3000");



app.Run();

