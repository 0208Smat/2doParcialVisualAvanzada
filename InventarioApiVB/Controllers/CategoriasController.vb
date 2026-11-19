Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports InventarioApiVB.DTOs
Imports InventarioApiVB.Services
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc

Namespace Controllers
    <ApiController>
    <Route("api/[controller]")>
    Public Class CategoriasController
        Inherits ControllerBase

        Private ReadOnly _categoriaService As ICategoriaService

        Public Sub New(categoriaService As ICategoriaService)
            _categoriaService = categoriaService
        End Sub

        ''' <summary>
        ''' Consulta todas las categorías registradas.
        ''' </summary>
        <HttpGet>
        <ProducesResponseType(GetType(IEnumerable(Of CategoriaResponseDto)), StatusCodes.Status200OK)>
        Public Async Function GetAll() As Task(Of ActionResult(Of IEnumerable(Of CategoriaResponseDto)))
            Dim categorias = Await _categoriaService.GetAllAsync()
            Return Ok(categorias)
        End Function

        ''' <summary>
        ''' Consulta una categoría específica por su identificador.
        ''' </summary>
        <HttpGet("{id:int}")>
        <ProducesResponseType(GetType(CategoriaResponseDto), StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status404NotFound)>
        Public Async Function GetById(id As Integer) As Task(Of ActionResult(Of CategoriaResponseDto))
            Dim categoria = Await _categoriaService.GetByIdAsync(id)
            Return Ok(categoria)
        End Function

        ''' <summary>
        ''' Consulta una categoría incluyendo la lista de todos sus productos asociados.
        ''' </summary>
        <HttpGet("{id:int}/productos")>
        <ProducesResponseType(GetType(CategoriaConProductosDto), StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status404NotFound)>
        Public Async Function GetByIdWithProductos(id As Integer) As Task(Of ActionResult(Of CategoriaConProductosDto))
            Dim categoriaConProductos = Await _categoriaService.GetByIdWithProductosAsync(id)
            Return Ok(categoriaConProductos)
        End Function

        ''' <summary>
        ''' Registra una nueva categoría.
        ''' </summary>
        <HttpPost>
        <ProducesResponseType(GetType(CategoriaResponseDto), StatusCodes.Status201Created)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status400BadRequest)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status409Conflict)>
        Public Async Function Create(<FromBody> dto As CategoriaCreateDto) As Task(Of ActionResult(Of CategoriaResponseDto))
            Dim creada = Await _categoriaService.CreateAsync(dto)
            Return CreatedAtAction(NameOf(GetById), New With {.id = creada.Id}, creada)
        End Function

        ''' <summary>
        ''' Modifica una categoría existente.
        ''' </summary>
        <HttpPut("{id:int}")>
        <ProducesResponseType(GetType(CategoriaResponseDto), StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status400BadRequest)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status404NotFound)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status409Conflict)>
        Public Async Function Update(id As Integer, <FromBody> dto As CategoriaUpdateDto) As Task(Of ActionResult(Of CategoriaResponseDto))
            Dim actualizada = Await _categoriaService.UpdateAsync(id, dto)
            Return Ok(actualizada)
        End Function

        ''' <summary>
        ''' Elimina una categoría siempre que no tenga productos asociados.
        ''' </summary>
        <HttpDelete("{id:int}")>
        <ProducesResponseType(StatusCodes.Status204NoContent)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status404NotFound)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status409Conflict)>
        Public Async Function Delete(id As Integer) As Task(Of IActionResult)
            Await _categoriaService.DeleteAsync(id)
            Return NoContent()
        End Function
    End Class
End Namespace
