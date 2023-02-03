using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Service.Business;
using Service.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContactDbContext>(
    options =>
    {
        if (builder.Environment.IsProduction())
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
        }
        else
        {
            options.UseInMemoryDatabase("Contact");
        }
    });

builder.Services.AddScoped<IContactDbContext, ContactDbContext>();
builder.Services.AddScoped<IContactBusiness, ContactBusiness>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy",
        builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DefaultCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
