Imports System.ComponentModel.DataAnnotations

Namespace DTOs
    Public Class CategoriaUpdateDto
        <Required(ErrorMessage:="El nombre de la categoría es obligatorio.")>
        <StringLength(50, ErrorMessage:="El nombre de la categoría no puede superar los 50 caracteres.")>
        Public Property Nombre As String

        <StringLength(200, ErrorMessage:="La descripción no puede superar los 200 caracteres.")>
        Public Property Descripcion As String
    End Class
End Namespace
