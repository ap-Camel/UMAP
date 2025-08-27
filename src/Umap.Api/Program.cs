using System;
using Umap.Api.Services;
using Umap.Api.Services.impl;

namespace Umap.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                // Add services to the container.
                builder.Services.AddSingleton<IPaddleOcr, PaddleOcr>();
                builder.Services.AddSingleton<IScreenCaptureService, ScreenCaptureService>();
                builder.Services.AddSingleton<IScreenCropService, ScreenCropService>();
                builder.Services.AddSingleton<IUmaMusumeWindowBringToFrontService, UmaMusumeWindowBringToFrontService>();
                builder.Services.AddSingleton<IWindowBoundsService, WindowBoundsService>();
                builder.Services.AddSingleton<IWindowService, WindowService>();
                builder.Services.AddSingleton<ICareerService, CareerService>();


                builder.Services.AddControllers();
                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                //builder.Services.AddOpenApi();

                var app = builder.Build();

                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseRouting();

                app.MapDefaultControllerRoute();

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
