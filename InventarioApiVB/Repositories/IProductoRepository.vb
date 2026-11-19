Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports InventarioApiVB.Models

Namespace Repositories
    Public Interface IProductoRepository
        Function GetAllWithCategoriaAsync() As Task(Of IEnumerable(Of Producto))
        Function GetByIdWithCategoriaAsync(id As Integer) As Task(Of Producto)
        Function GetByIdAsync(id As Integer) As Task(Of Producto)
        Function GetByCategoriaIdAsync(categoriaId As Integer) As Task(Of IEnumerable(Of Producto))
        Function ExistsByNombreAsync(nombre As String, Optional excludeId As Integer? = Nothing) As Task(Of Boolean)
        Function AddAsync(producto As Producto) As Task(Of Producto)
        Function UpdateAsync(producto As Producto) As Task(Of Producto)
        Function DeleteAsync(producto As Producto) As Task
    End Interface
End Namespace
