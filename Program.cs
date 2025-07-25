using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PokemonReviewApp;
using PokemonReviewApp.Data;
using PokemonReviewApp.Middleware;
using PokemonReviewApp.Repositories.Implementations;
using PokemonReviewApp.Repositories.Interfaces;
using PokemonReviewApp.Services.Implementations;
using PokemonReviewApp.Services.Interfaces;
using PokemonReviewApp.Services.Repository;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Authentication and Authorization
        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<TokenService>();
        //Authorization for Swagger to access endpoints
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
            });
        });
        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddTransient<Seed>();
        // Register Repositories
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICountryRepository, CountryRepository>();
        //builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
        //builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
        //builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        //builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        // Register Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IPokemonService, PokemonService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICountryService, CountryService>();
        builder.Services.AddScoped<IOwnerService, OwnerService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<IReviewerService, ReviewerService>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        if (args.Length == 1 && args[0].ToLower() == "seeddata")
            SeedData(app);

        void SeedData(IHost app)
        {
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopedFactory!.CreateScope())
            {
                var seed = scope.ServiceProvider.GetService<Seed>();
                seed.SeedDataContext();
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}