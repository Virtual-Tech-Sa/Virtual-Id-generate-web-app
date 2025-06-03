using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VID.Data;
using VID.Repositories;
using VID.Models;
using VID.Services;

        
        //delete at the top.
    
        var builder = WebApplication.CreateBuilder(args);

        // Register the generic repository
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Add services to the container
        builder.Services.AddControllers();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<PasswordResetService>();

       // builder.Services.AddScoped(typeof(GenericRepository<>));  // or your custom interface if used

        
       

        //builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();


        // Add PostgreSQL database connection
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("VID_DB"))
        );

        // builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseNpgsql(builder.Configuration.GetConnectionString("Home_VD"))
        // );

        // Configure JWT authentication
        var unused = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
#pragma warning disable CS8604 // Possible null reference argument.
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
#pragma warning restore CS8604 // Possible null reference argument.
            });

        // Add CORS support
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp",
                builder => builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        
        //
        // services.AddCors(options =>
        // {
        //     options.AddDefaultPolicy(builder =>
        //     {
        //         builder.AllowAnyOrigin()
        //             .AllowAnyHeader()
        //             .AllowAnyMethod();
        //     });
        // });


        var app = builder.Build();
        app.UseCors("AllowReactApp");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    

