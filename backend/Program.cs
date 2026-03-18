using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartLeaf.Application.Interfaces;
using SmartLeaf.Infrastructure.Repositories;
using SmartLeaf.Infrastructure.ExternalServices;
using SmartLeaf.Services;

var builder = WebApplication.CreateBuilder(args);
var config  = builder.Configuration;
var jwtKey  = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key missing");

// Autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = false,
        ValidateAudience         = false,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey))
    };
});

// ─── Repositorios ───────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGardenRepository, GardenRepository>();
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<ISpeciesCatalogRepository, SpeciesCatalogRepository>();
builder.Services.AddScoped<ICareTaskRepository, CareTaskRepository>();
builder.Services.AddScoped<ISocialRepository, SocialRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();

// ─── Servicios de aplicación ─────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGardenService, GardenService>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<ICareTaskService, CareTaskService>();
builder.Services.AddScoped<ISocialService, SocialService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

// ─── Servicios externos ───────────────────────────────────────────────────────
builder.Services.AddHttpClient<ISupabaseAuthProvider, SupabaseAuthProvider>();
builder.Services.AddHttpClient<IPlantIdentificationService, PlantIdService>();
builder.Services.AddHttpClient<IChatbotService, ChatbotService>();
builder.Services.AddHttpClient<IPlantSearchService, PlantSearchService>();

// ─── Infraestructura general ──────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5174", "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();