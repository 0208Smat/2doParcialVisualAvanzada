Imports System
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports InventarioApiVB.Data
Imports InventarioApiVB.DTOs
Imports InventarioApiVB.Middleware
Imports InventarioApiVB.Repositories
Imports InventarioApiVB.Services
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.OpenApi.Models

Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers() _
            .ConfigureApiBehaviorOptions(
                Sub(options)
                    options.InvalidModelStateResponseFactory =
                        Function(context)
                            Dim errores = context.ModelState _
                                .Where(Function(e) e.Value.Errors.Count > 0) _
                                .ToDictionary(
                                    Function(kvp) kvp.Key,
                                    Function(kvp) kvp.Value.Errors.Select(Function(err) err.ErrorMessage).ToArray())

                            Dim respuesta = New ErrorResponseDto With {
                                .StatusCode = StatusCodes.Status400BadRequest,
                                .Mensaje = "Error de validación en los datos enviados.",
                                .Detalle = "Verifique los campos requeridos y las restricciones de formato.",
                                .FechaUtc = DateTime.UtcNow,
                                .Errores = errores
                            }

                            Return New BadRequestObjectResult(respuesta)
                        End Function
                End Sub)

        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen(
            Sub(options)
                options.SwaggerDoc("v1", New OpenApiInfo With {
                    .Title = "API REST de Inventario - Visual Basic .NET 8",
                    .Version = "v1",
                    .Description = "Sistema de administración de categorías y productos desarrollado con VB.NET 8, ASP.NET Core Web API y Entity Framework Core 8."
                })

                Dim xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
                Dim xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile)
                If File.Exists(xmlPath) Then
                    options.IncludeXmlComments(xmlPath)
                End If
            End Sub)

        builder.Services.AddDbContext(Of InventarioDbContext)(
            Sub(options)
                options.UseSqlServer(builder.Configuration.GetConnectionString("InventarioDB"))
            End Sub)

        builder.Services.AddScoped(Of ICategoriaRepository, CategoriaRepository)()
        builder.Services.AddScoped(Of IProductoRepository, ProductoRepository)()
        builder.Services.AddScoped(Of ICategoriaService, CategoriaService)()
        builder.Services.AddScoped(Of IProductoService, ProductoService)()

        Dim app = builder.Build()

        ' Middleware centralizado para el manejo global de excepciones
        app.UseMiddleware(Of ExceptionHandlingMiddleware)()

        If app.Environment.IsDevelopment() Then
            app.UseSwagger()
            app.UseSwaggerUI(
                Sub(c)
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventario API v1")
                End Sub)
        End If

        app.UseHttpsRedirection()
        app.MapControllers()

        app.Run()
    End Sub
End Module
