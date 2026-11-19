Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports InventarioApiVB.DTOs
Imports InventarioApiVB.Services
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc

Namespace Controllers
    <ApiController>
    <Route("api/[controller]")>
    Public Class ProductosController
        Inherits ControllerBase

        Private ReadOnly _productoService As IProductoService

        Public Sub New(productoService As IProductoService)
            _productoService = productoService
        End Sub

        ''' <summary>
        ''' Consulta todos los productos incluyendo los datos de su categoría.
        ''' </summary>
        <HttpGet>
        <ProducesResponseType(GetType(IEnumerable(Of ProductoResponseDto)), StatusCodes.Status200OK)>
        Public Async Function GetAll() As Task(Of ActionResult(Of IEnumerable(Of ProductoResponseDto)))
            Dim productos = Await _productoService.GetAllAsync()
            Return Ok(productos)
        End Function

        ''' <summary>
        ''' Consulta un producto por su identificador incluyendo los datos de su categoría.
        ''' </summary>
        <HttpGet("{id:int}")>
        <ProducesResponseType(GetType(ProductoResponseDto), StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status404NotFound)>
        Public Async Function GetById(id As Integer) As Task(Of ActionResult(Of ProductoResponseDto))
            Dim producto = Await _productoService.GetByIdAsync(id)
            Return Ok(producto)
        End Function

        ''' <summary>
        ''' Consulta todos los productos pertenecientes a una categoría específica.
        ''' </summary>
        <HttpGet("categoria/{categoriaId:int}")>
        <ProducesResponseType(GetType(IEnumerable(Of ProductoResponseDto)), StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status404NotFound)>
        Public Async Function GetByCategoria(categoriaId As Integer) As Task(Of ActionResult(Of IEnumerable(Of ProductoResponseDto)))
            Dim productos = Await _productoService.GetByCategoriaIdAsync(categoriaId)
            Return Ok(productos)
        End Function

        ''' <summary>
        ''' Registra un nuevo producto asociado a una categoría existente.
        ''' </summary>
        <HttpPost>
        <ProducesResponseType(GetType(ProductoResponseDto), StatusCodes.Status201Created)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status400BadRequest)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status409Conflict)>
        Public Async Function Create(<FromBody> dto As ProductoCreateDto) As Task(Of ActionResult(Of ProductoResponseDto))
            Dim creado = Await _productoService.CreateAsync(dto)
            Return CreatedAtAction(NameOf(GetById), New With {.id = creado.Id}, creado)
        End Function

        ''' <summary>
        ''' Modifica un producto existente.
        ''' </summary>
        <HttpPut("{id:int}")>
        <ProducesResponseType(GetType(ProductoResponseDto), StatusCodes.Status200OK)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status400BadRequest)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status404NotFound)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status409Conflict)>
        Public Async Function Update(id As Integer, <FromBody> dto As ProductoUpdateDto) As Task(Of ActionResult(Of ProductoResponseDto))
            Dim actualizado = Await _productoService.UpdateAsync(id, dto)
            Return Ok(actualizado)
        End Function

        ''' <summary>
        ''' Elimina un producto por su identificador.
        ''' </summary>
        <HttpDelete("{id:int}")>
        <ProducesResponseType(StatusCodes.Status204NoContent)>
        <ProducesResponseType(GetType(ErrorResponseDto), StatusCodes.Status404NotFound)>
        Public Async Function Delete(id As Integer) As Task(Of IActionResult)
            Await _productoService.DeleteAsync(id)
            Return NoContent()
        End Function
    End Class
End Namespace
