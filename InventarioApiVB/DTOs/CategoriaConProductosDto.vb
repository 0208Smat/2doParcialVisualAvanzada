Imports System.Collections.Generic

Namespace DTOs
    Public Class CategoriaConProductosDto
        Public Property Id As Integer
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property Productos As List(Of ProductoSimpleDto) = New List(Of ProductoSimpleDto)()
    End Class
End Namespace
