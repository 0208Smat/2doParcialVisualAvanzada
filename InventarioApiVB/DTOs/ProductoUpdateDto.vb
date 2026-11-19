Imports System
Imports System.ComponentModel.DataAnnotations

Namespace DTOs
    Public Class ProductoUpdateDto
        <Required(ErrorMessage:="El nombre del producto es obligatorio.")>
        <StringLength(50, ErrorMessage:="El nombre del producto no puede superar los 50 caracteres.")>
        Public Property Nombre As String

        <StringLength(200, ErrorMessage:="La descripción no puede superar los 200 caracteres.")>
        Public Property Descripcion As String

        <Required(ErrorMessage:="El precio del producto es obligatorio.")>
        <Range(0.01, Double.MaxValue, ErrorMessage:="El precio debe ser mayor que cero.")>
        Public Property Precio As Decimal?

        <Required(ErrorMessage:="El stock del producto es obligatorio.")>
        <Range(0, Integer.MaxValue, ErrorMessage:="El stock no puede ser negativo.")>
        Public Property Stock As Integer?

        <Required(ErrorMessage:="La categoría es obligatoria.")>
        <Range(1, Integer.MaxValue, ErrorMessage:="Debe indicar un identificador de categoría válido.")>
        Public Property CategoriaId As Integer?
    End Class
End Namespace
