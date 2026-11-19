Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports InventarioApiVB.Models

Namespace Repositories
    Public Interface ICategoriaRepository
        Function GetAllAsync() As Task(Of IEnumerable(Of Categoria))
        Function GetByIdAsync(id As Integer) As Task(Of Categoria)
        Function GetByIdWithProductosAsync(id As Integer) As Task(Of Categoria)
        Function ExistsAsync(id As Integer) As Task(Of Boolean)
        Function ExistsByNombreAsync(nombre As String, Optional excludeId As Integer? = Nothing) As Task(Of Boolean)
        Function HasProductosAsync(id As Integer) As Task(Of Boolean)
        Function AddAsync(categoria As Categoria) As Task(Of Categoria)
        Function UpdateAsync(categoria As Categoria) As Task(Of Categoria)
        Function DeleteAsync(categoria As Categoria) As Task
    End Interface
End Namespace
