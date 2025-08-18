
using Umap.Api.Services;
using Umap.Api.Services.impl;

namespace Umap.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IPaddleOcr, PaddleOcr>();
            builder.Services.AddSingleton<IScreenCaptureService, ScreenCaptureService>();
            builder.Services.AddSingleton<IScreenCropService, ScreenCropService>();
            builder.Services.AddSingleton<IUmaMusumeWindowBringToFrontService, UmaMusumeWindowBringToFrontService>();
            builder.Services.AddSingleton<IWindowBoundsService, WindowBoundsService>();
            builder.Services.AddSingleton<IWindowService, WindowService>();


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

           
            app.MapOpenApi();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
