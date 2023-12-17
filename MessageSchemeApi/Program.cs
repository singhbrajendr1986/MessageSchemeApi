using BusinessLayer;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBinaryMessageDecoder, BinaryMessageDecoder>().
AddScoped<IBinaryMessageEncoder, BinaryMessageEncoder>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Exception handling under pipeline
app.UseExceptionHandler(
    option =>
    {
        option.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var ex = context.Features.Get<IExceptionHandlerFeature>();
                if (ex !=null)
                {
                    await context.Response.WriteAsync(ex.Error.Message);
                }
            });
    });

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
