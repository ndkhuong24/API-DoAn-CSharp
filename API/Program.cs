using API.Data;
using API.Repository;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Net;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      }); // add the allowed origins  
});
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
builder.Services.AddControllers();


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
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

//Setup local IP\
string localIP = LocalIPAddress();

app.Urls.Add("https://" + localIP + ":8080");

app.Run();


static string LocalIPAddress()
{
    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
    {
        socket.Connect("8.8.8.8", 65530);
        IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
        if (endPoint != null)
        {
            return endPoint.Address.ToString();
        }
        else
        {
            return "127.0.0.1";
        }
    }
}



