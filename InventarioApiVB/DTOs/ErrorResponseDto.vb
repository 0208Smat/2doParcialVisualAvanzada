Imports System
Imports System.Collections.Generic

Namespace DTOs
    Public Class ErrorResponseDto
        Public Property StatusCode As Integer
        Public Property Mensaje As String
        Public Property Detalle As String
        Public Property FechaUtc As DateTime = DateTime.UtcNow
        Public Property Errores As Dictionary(Of String, String())
    End Class
End Namespace
