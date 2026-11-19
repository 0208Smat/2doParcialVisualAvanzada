Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports InventarioApiVB.DTOs
Imports InventarioApiVB.Exceptions
Imports InventarioApiVB.Models
Imports InventarioApiVB.Repositories

Namespace Services
    Public Class ProductoService
        Implements IProductoService

        Private ReadOnly _productoRepository As IProductoRepository
        Private ReadOnly _categoriaRepository As ICategoriaRepository

        Public Sub New(productoRepository As IProductoRepository, categoriaRepository As ICategoriaRepository)
            _productoRepository = productoRepository
            _categoriaRepository = categoriaRepository
        End Sub

        Public Async Function GetAllAsync() As Task(Of IEnumerable(Of ProductoResponseDto)) Implements IProductoService.GetAllAsync
            Dim productos = Await _productoRepository.GetAllWithCategoriaAsync()
            Return productos.Select(Function(p) MapToResponseDto(p)).ToList()
        End Function

        Public Async Function GetByIdAsync(id As Integer) As Task(Of ProductoResponseDto) Implements IProductoService.GetByIdAsync
            Dim producto = Await _productoRepository.GetByIdWithCategoriaAsync(id)
            If producto Is Nothing Then
                Throw New NotFoundException($"No se encontró el producto con identificador {id}.")
            End If

            Return MapToResponseDto(producto)
        End Function

        Public Async Function GetByCategoriaIdAsync(categoriaId As Integer) As Task(Of IEnumerable(Of ProductoResponseDto)) Implements IProductoService.GetByCategoriaIdAsync
            Dim categoriaExiste = Await _categoriaRepository.ExistsAsync(categoriaId)
            If Not categoriaExiste Then
                Throw New NotFoundException($"No se encontró la categoría con identificador {categoriaId}.")
            End If

            Dim productos = Await _productoRepository.GetByCategoriaIdAsync(categoriaId)
            Return productos.Select(Function(p) MapToResponseDto(p)).ToList()
        End Function

        Public Async Function CreateAsync(dto As ProductoCreateDto) As Task(Of ProductoResponseDto) Implements IProductoService.CreateAsync
            Dim nombreLimpio = dto.Nombre.Trim()

            ' Regla 5: No se permite registrar un producto con stock negativo
            If dto.Stock.Value < 0 Then
                Throw New BusinessRuleException("El stock del producto no puede ser negativo.")
            End If

            ' Regla 1: No se permite registrar dos productos con el mismo nombre (ignorando mayúsculas y minúsculas)
            Dim existeProducto = Await _productoRepository.ExistsByNombreAsync(nombreLimpio)
            If existeProducto Then
                Throw New BusinessRuleException($"Ya existe un producto registrado con el nombre '{nombreLimpio}'.")
            End If

            ' Regla 3: No se permite registrar un producto utilizando una categoría que no exista
            Dim existeCategoria = Await _categoriaRepository.ExistsAsync(dto.CategoriaId.Value)
            If Not existeCategoria Then
                Throw New BusinessRuleException($"La categoría con identificador {dto.CategoriaId.Value} no existe.")
            End If

            Dim nuevoProducto = New Producto With {
                .Nombre = nombreLimpio,
                .Descripcion = If(dto.Descripcion IsNot Nothing, dto.Descripcion.Trim(), Nothing),
                .Precio = dto.Precio.Value,
                .Stock = dto.Stock.Value,
                .FechaRegistro = DateTime.UtcNow,
                .CategoriaId = dto.CategoriaId.Value
            }

            Dim creado = Await _productoRepository.AddAsync(nuevoProducto)
            Return MapToResponseDto(creado)
        End Function

        Public Async Function UpdateAsync(id As Integer, dto As ProductoUpdateDto) As Task(Of ProductoResponseDto) Implements IProductoService.UpdateAsync
            Dim productoExistente = Await _productoRepository.GetByIdAsync(id)
            If productoExistente Is Nothing Then
                Throw New NotFoundException($"No se encontró el producto con identificador {id}.")
            End If

            Dim nombreLimpio = dto.Nombre.Trim()

            ' Regla 5: No se permite modificar un producto con stock negativo
            If dto.Stock.Value < 0 Then
                Throw New BusinessRuleException("El stock del producto no puede ser negativo.")
            End If

            ' Regla 1: Evitar duplicados de nombre ignorando mayúsculas/minúsculas
            Dim existeOtroProducto = Await _productoRepository.ExistsByNombreAsync(nombreLimpio, id)
            If existeOtroProducto Then
                Throw New BusinessRuleException($"Ya existe otro producto registrado con el nombre '{nombreLimpio}'.")
            End If

            ' Regla 3: No se permite modificar un producto utilizando una categoría que no exista
            Dim existeCategoria = Await _categoriaRepository.ExistsAsync(dto.CategoriaId.Value)
            If Not existeCategoria Then
                Throw New BusinessRuleException($"La categoría con identificador {dto.CategoriaId.Value} no existe.")
            End If

            productoExistente.Nombre = nombreLimpio
            productoExistente.Descripcion = If(dto.Descripcion IsNot Nothing, dto.Descripcion.Trim(), Nothing)
            productoExistente.Precio = dto.Precio.Value
            productoExistente.Stock = dto.Stock.Value
            productoExistente.CategoriaId = dto.CategoriaId.Value

            Dim actualizado = Await _productoRepository.UpdateAsync(productoExistente)
            Return MapToResponseDto(actualizado)
        End Function

        Public Async Function DeleteAsync(id As Integer) As Task Implements IProductoService.DeleteAsync
            Dim producto = Await _productoRepository.GetByIdAsync(id)
            If producto Is Nothing Then
                Throw New NotFoundException($"No se encontró el producto con identificador {id}.")
            End If

            Await _productoRepository.DeleteAsync(producto)
        End Function

        Private Shared Function MapToResponseDto(producto As Producto) As ProductoResponseDto
            Return New ProductoResponseDto With {
                .Id = producto.Id,
                .Nombre = producto.Nombre,
                .Descripcion = producto.Descripcion,
                .Precio = producto.Precio,
                .Stock = producto.Stock,
                .FechaRegistro = producto.FechaRegistro,
                .CategoriaId = producto.CategoriaId,
                .Categoria = If(producto.Categoria IsNot Nothing,
                    New CategoriaResponseDto With {
                        .Id = producto.Categoria.Id,
                        .Nombre = producto.Categoria.Nombre,
                        .Descripcion = producto.Categoria.Descripcion
                    },
                    Nothing)
            }
        End Function
    End Class
End Namespace
