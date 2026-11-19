Imports System
Imports System.Net
Imports System.Text.Json
Imports System.Threading.Tasks
Imports InventarioApiVB.DTOs
Imports InventarioApiVB.Exceptions
Imports Microsoft.AspNetCore.Http
Imports Microsoft.Extensions.Logging

Namespace Middleware
    Public Class ExceptionHandlingMiddleware
        Private ReadOnly _next As RequestDelegate
        Private ReadOnly _logger As ILogger(Of ExceptionHandlingMiddleware)

        Public Sub New([next] As RequestDelegate, logger As ILogger(Of ExceptionHandlingMiddleware))
            _next = [next]
            _logger = logger
        End Sub

        Public Async Function InvokeAsync(context As HttpContext) As Task
            Try
                Await _next(context)
            Catch ex As Exception
                HandleExceptionAsync(context, ex)
            End Try
        End Function

        Private Async Function HandleExceptionAsync(context As HttpContext, ex As Exception) As Task
            Dim statusCode As Integer
            Dim mensaje As String
            Dim detalle As String

            If TypeOf ex Is NotFoundException Then
                statusCode = CInt(HttpStatusCode.NotFound) ' 404
                mensaje = "Recurso no encontrado."
                detalle = ex.Message
                _logger.LogWarning("Recurso no encontrado: {Mensaje}", ex.Message)

            ElseIf TypeOf ex Is BusinessRuleException Then
                statusCode = CInt(HttpStatusCode.Conflict) ' 409
                mensaje = "Conflicto con una regla de negocio."
                detalle = ex.Message
                _logger.LogWarning("Conflicto de regla de negocio: {Mensaje}", ex.Message)

            ElseIf TypeOf ex Is ArgumentException Then
                statusCode = CInt(HttpStatusCode.BadRequest) ' 400
                mensaje = "Datos de solicitud inválidos."
                detalle = ex.Message
                _logger.LogWarning("Solicitud inválida: {Mensaje}", ex.Message)

            Else
                ' 500 Internal Server Error: nunca exponer detalles internos ni stack trace al consumidor
                statusCode = CInt(HttpStatusCode.InternalServerError)
                mensaje = "Ocurrió un error interno inesperado en el servidor."
                detalle = "Por favor, intente nuevamente más tarde o contacte al soporte técnico."
                _logger.LogError(ex, "Error inesperado procesando la solicitud {Method} {Path}", context.Request.Method, context.Request.Path)
            End If

            context.Response.ContentType = "application/json"
            context.Response.StatusCode = statusCode

            Dim errorResponse = New ErrorResponseDto With {
                .StatusCode = statusCode,
                .Mensaje = mensaje,
                .Detalle = detalle,
                .FechaUtc = DateTime.UtcNow
            }

            Dim jsonOptions = New JsonSerializerOptions With {
                .PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }

            Dim json = JsonSerializer.Serialize(errorResponse, jsonOptions)
            Await context.Response.WriteAsync(json)
        End Function
    End Class
End Namespace
