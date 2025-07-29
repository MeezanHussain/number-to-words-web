using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace NumberToWordsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRouting();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // API endpoint to convert amounts to words
            app.MapGet("/api/convert", (HttpRequest request) =>
            {
                string? amountParam = request.Query["amount"];
                if (string.IsNullOrWhiteSpace(amountParam))
                {
                    return Results.BadRequest("Query parameter 'amount' is required.");
                }
                if (!decimal.TryParse(amountParam, out decimal amount))
                {
                    return Results.BadRequest("Invalid numeric amount.");
                }
                var converter = new NumberToWordsConverter();
                string words = converter.ConvertAmountToWords(amount);
                return Results.Ok(words);
            });

            app.MapFallback(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/index.html");
            });

            app.Run();
        }
    }
}