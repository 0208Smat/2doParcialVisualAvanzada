Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports InventarioApiVB.DTOs

Namespace Services
    Public Interface IProductoService
        Function GetAllAsync() As Task(Of IEnumerable(Of ProductoResponseDto))
        Function GetByIdAsync(id As Integer) As Task(Of ProductoResponseDto)
        Function GetByCategoriaIdAsync(categoriaId As Integer) As Task(Of IEnumerable(Of ProductoResponseDto))
        Function CreateAsync(dto As ProductoCreateDto) As Task(Of ProductoResponseDto)
        Function UpdateAsync(id As Integer, dto As ProductoUpdateDto) As Task(Of ProductoResponseDto)
        Function DeleteAsync(id As Integer) As Task
    End Interface
End Namespace
