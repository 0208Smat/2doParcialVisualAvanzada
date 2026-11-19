Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models
    <Table("Productos")>
    Public Class Producto
        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Id As Integer

        <Required>
        <MaxLength(50)>
        Public Property Nombre As String

        <MaxLength(200)>
        Public Property Descripcion As String

        <Required>
        <Column(TypeName:="decimal(18,2)")>
        Public Property Precio As Decimal

        <Required>
        Public Property Stock As Integer

        <Required>
        Public Property FechaRegistro As DateTime = DateTime.UtcNow

        <Required>
        Public Property CategoriaId As Integer

        ' Propiedad de navegación: cada producto pertenece a una categoría
        <ForeignKey(NameOf(CategoriaId))>
        Public Overridable Property Categoria As Categoria
    End Class
End Namespace
