using API.Data;
using API.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddDbContext<StyleDbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer-Connection"));
});
builder.Services.AddDbContext<ProductDbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer-Connection"));
});

// Life cycle DI: AddSingleton(), AddTransient(), AddScoped()
builder.Services.AddScoped<IStyleRepository, StyleRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


