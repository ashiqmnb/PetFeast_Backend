
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetFeast_Backend2.Mappings;
using PetFeast_Backend2.Services.Auth;
using PetFeast_Backend2.Services.CategoryService;
using PetFeast_Backend2;
using System.Text;
using Microsoft.OpenApi.Models;
using PetFeast_Backend2.Services.ProductService;
using PetFeast_Backend2.Services.CartService;
using PetFeast_Backend2.Middleware;
using PetFeast_Backend2.Services.WishListService;
using PetFeast_Backend2.Services.OrderService;
using PetFeast_Backend2.Services.UserService;
using PetFeast_Backend2.Services.CloudinaryService;
using PetFeast_Backend2.Services.AddressService;

namespace PetFeast_Backend2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Swagger configuration with JWT support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PetFeast API", Version = "v1" });

                // Add JWT Authentication in Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Adding ApplicationDbContext as a service
            builder.Services.AddScoped<AppDbContext>();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(AppMapper));

            // Inject AuthService
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Inject CategoryService
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            // Inject ProductService
            builder.Services.AddScoped<IProductService, ProductService>();

            // Inject CartService
            builder.Services.AddScoped<ICartService, CartService>();

            // Inject WishListService
            builder.Services.AddScoped<IWishListService, WishListService>();

            // Inject OrderService
            builder.Services.AddScoped<IOrderService, OrderService>();

            // Inject UserService
            builder.Services.AddScoped<IUserService, UserService>();

            // Inject CloudinaryService
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

            // Inject AddressService
            builder.Services.AddScoped<IAddressService, AddressService>();
            



            //Adding Authentication service for JWT

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("ReactPolicy");

            app.UseStaticFiles();
            
            app.UseHttpsRedirection();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<UserIdMiddlware>();

            
            app.MapControllers();

            app.Run();
        }
    }
}
