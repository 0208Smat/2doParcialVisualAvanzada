Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports InventarioApiVB.DTOs

Namespace Services
    Public Interface ICategoriaService
        Function GetAllAsync() As Task(Of IEnumerable(Of CategoriaResponseDto))
        Function GetByIdAsync(id As Integer) As Task(Of CategoriaResponseDto)
        Function GetByIdWithProductosAsync(id As Integer) As Task(Of CategoriaConProductosDto)
        Function CreateAsync(dto As CategoriaCreateDto) As Task(Of CategoriaResponseDto)
        Function UpdateAsync(id As Integer, dto As CategoriaUpdateDto) As Task(Of CategoriaResponseDto)
        Function DeleteAsync(id As Integer) As Task
    End Interface
End Namespace
