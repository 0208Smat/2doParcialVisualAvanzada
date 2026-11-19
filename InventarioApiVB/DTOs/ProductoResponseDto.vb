Imports System

Namespace DTOs
    Public Class ProductoResponseDto
        Public Property Id As Integer
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property Precio As Decimal
        Public Property Stock As Integer
        Public Property FechaRegistro As DateTime
        Public Property CategoriaId As Integer
        Public Property Categoria As CategoriaResponseDto
    End Class
End Namespace
