Imports System.Text.Json
Imports Microsoft.AspNetCore.Http
Imports Microsoft.Extensions.Logging
Namespace Middleware
    Public Class ExceptionHandlingMiddleware
        Private ReadOnly _next As RequestDelegate
        Private ReadOnly _logger As ILogger(Of ExceptionHandlingMiddleware)
        Public Sub New(nextStep As RequestDelegate, logger As ILogger(Of ExceptionHandlingMiddleware))
            _next = nextStep : _logger = logger
        End Sub
        Public Async Function InvokeAsync(context As HttpContext) As Task
            Dim captured As Exception = Nothing
            Try
                Await _next(context)
            Catch ex As Exception
                captured = ex
            End Try
            If captured IsNot Nothing Then
                Dim ex = captured
                _logger.LogError(ex, "Error no controlado al procesar la solicitud")
                Dim status = If(TypeOf ex Is KeyNotFoundException, 404, If(TypeOf ex Is BusinessConflictException, 409, 500))
                Dim message = If(status = 404 OrElse status = 409, ex.Message, "Ocurrió un error interno.")
                context.Response.StatusCode = status
                context.Response.ContentType = "application/json"
                Await context.Response.WriteAsync(JsonSerializer.Serialize(New With {.status = status, .message = message}))
            End If
        End Function
    End Class
End Namespace
