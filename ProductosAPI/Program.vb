Imports ProductosAPI.Data
Imports ProductosAPI.Middleware
Imports ProductosAPI.Repositories
Imports ProductosAPI.Services
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Configuration
Imports Microsoft.OpenApi.Models
Imports System.Reflection
Imports System.IO

Public Class Program
    Public Shared Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)
        builder.Services.AddControllers()
        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen(Sub(options)
            options.SwaggerDoc("v1", New OpenApiInfo With {
                .Title = "API de Inventario",
                .Version = "v1",
                .Description = "API REST para administrar categorías y productos del inventario. Permite registrar, consultar, modificar y eliminar información, con validaciones y reglas de negocio."
            })
            Dim xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
            Dim xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile)
            If File.Exists(xmlPath) Then options.IncludeXmlComments(xmlPath)
        End Sub)
        Dim connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        If String.IsNullOrWhiteSpace(connectionString) Then Throw New InvalidOperationException("Falta ConnectionStrings:DefaultConnection en la configuración.")
        builder.Services.AddDbContext(Of ApplicationDbContext)(Sub(options) options.UseSqlServer(connectionString))
        builder.Services.AddScoped(Of IProductoRepository, ProductoRepository)()
        builder.Services.AddScoped(Of ICategoriaRepository, CategoriaRepository)()
        builder.Services.AddScoped(Of IProductoService, ProductoService)()
        builder.Services.AddScoped(Of ICategoriaService, CategoriaService)()
        Dim app = builder.Build()
        app.UseMiddleware(Of ExceptionHandlingMiddleware)()
        app.UseSwagger()
        app.UseSwaggerUI(Sub(options)
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Inventario v1")
            options.DocumentTitle = "Documentación | API de Inventario"
        End Sub)
        app.MapControllers()
        app.Run()
    End Sub
End Class
