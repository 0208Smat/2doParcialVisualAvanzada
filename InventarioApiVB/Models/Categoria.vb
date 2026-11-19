Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models
    <Table("Categorias")>
    Public Class Categoria
        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Id As Integer

        <Required>
        <MaxLength(50)>
        Public Property Nombre As String

        <MaxLength(200)>
        Public Property Descripcion As String

        ' Propiedad de navegación: una categoría puede tener varios productos (1:N)
        Public Overridable Property Productos As ICollection(Of Producto) = New List(Of Producto)()
    End Class
End Namespace
