Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports InventarioApiVB.DTOs
Imports InventarioApiVB.Exceptions
Imports InventarioApiVB.Models
Imports InventarioApiVB.Repositories

Namespace Services
    Public Class CategoriaService
        Implements ICategoriaService

        Private ReadOnly _categoriaRepository As ICategoriaRepository

        Public Sub New(categoriaRepository As ICategoriaRepository)
            _categoriaRepository = categoriaRepository
        End Sub

        Public Async Function GetAllAsync() As Task(Of IEnumerable(Of CategoriaResponseDto)) Implements ICategoriaService.GetAllAsync
            Dim categorias = Await _categoriaRepository.GetAllAsync()
            Return categorias.Select(Function(c) MapToResponseDto(c)).ToList()
        End Function

        Public Async Function GetByIdAsync(id As Integer) As Task(Of CategoriaResponseDto) Implements ICategoriaService.GetByIdAsync
            Dim categoria = Await _categoriaRepository.GetByIdAsync(id)
            If categoria Is Nothing Then
                Throw New NotFoundException($"No se encontró la categoría con identificador {id}.")
            End If

            Return MapToResponseDto(categoria)
        End Function

        Public Async Function GetByIdWithProductosAsync(id As Integer) As Task(Of CategoriaConProductosDto) Implements ICategoriaService.GetByIdWithProductosAsync
            Dim categoria = Await _categoriaRepository.GetByIdWithProductosAsync(id)
            If categoria Is Nothing Then
                Throw New NotFoundException($"No se encontró la categoría con identificador {id}.")
            End If

            Return New CategoriaConProductosDto With {
                .Id = categoria.Id,
                .Nombre = categoria.Nombre,
                .Descripcion = categoria.Descripcion,
                .Productos = categoria.Productos.Select(
                    Function(p) New ProductoSimpleDto With {
                        .Id = p.Id,
                        .Nombre = p.Nombre,
                        .Descripcion = p.Descripcion,
                        .Precio = p.Precio,
                        .Stock = p.Stock,
                        .FechaRegistro = p.FechaRegistro
                    }).ToList()
            }
        End Function

        Public Async Function CreateAsync(dto As CategoriaCreateDto) As Task(Of CategoriaResponseDto) Implements ICategoriaService.CreateAsync
            Dim nombreLimpio = dto.Nombre.Trim()

            ' Regla 2: No se permite registrar dos categorías con el mismo nombre (ignorando mayúsculas/minúsculas)
            Dim existeNombre = Await _categoriaRepository.ExistsByNombreAsync(nombreLimpio)
            If existeNombre Then
                Throw New BusinessRuleException($"Ya existe una categoría registrada con el nombre '{nombreLimpio}'.")
            End If

            Dim nuevaCategoria = New Categoria With {
                .Nombre = nombreLimpio,
                .Descripcion = If(dto.Descripcion IsNot Nothing, dto.Descripcion.Trim(), Nothing)
            }

            Dim creada = Await _categoriaRepository.AddAsync(nuevaCategoria)
            Return MapToResponseDto(creada)
        End Function

        Public Async Function UpdateAsync(id As Integer, dto As CategoriaUpdateDto) As Task(Of CategoriaResponseDto) Implements ICategoriaService.UpdateAsync
            Dim categoriaExistente = Await _categoriaRepository.GetByIdAsync(id)
            If categoriaExistente Is Nothing Then
                Throw New NotFoundException($"No se encontró la categoría con identificador {id}.")
            End If

            Dim nombreLimpio = dto.Nombre.Trim()

            ' Regla 2: Evitar duplicados de nombre al modificar
            Dim existeOtroNombre = Await _categoriaRepository.ExistsByNombreAsync(nombreLimpio, id)
            If existeOtroNombre Then
                Throw New BusinessRuleException($"Ya existe otra categoría registrada con el nombre '{nombreLimpio}'.")
            End If

            categoriaExistente.Nombre = nombreLimpio
            categoriaExistente.Descripcion = If(dto.Descripcion IsNot Nothing, dto.Descripcion.Trim(), Nothing)

            Dim actualizada = Await _categoriaRepository.UpdateAsync(categoriaExistente)
            Return MapToResponseDto(actualizada)
        End Function

        Public Async Function DeleteAsync(id As Integer) As Task Implements ICategoriaService.DeleteAsync
            Dim categoria = Await _categoriaRepository.GetByIdAsync(id)
            If categoria Is Nothing Then
                Throw New NotFoundException($"No se encontró la categoría con identificador {id}.")
            End If

            ' Regla 4: No se permite eliminar una categoría que tenga productos asociados
            Dim tieneProductos = Await _categoriaRepository.HasProductosAsync(id)
            If tieneProductos Then
                Throw New BusinessRuleException($"No se puede eliminar la categoría '{categoria.Nombre}' porque tiene productos asociados.")
            End If

            Await _categoriaRepository.DeleteAsync(categoria)
        End Function

        Private Shared Function MapToResponseDto(categoria As Categoria) As CategoriaResponseDto
            Return New CategoriaResponseDto With {
                .Id = categoria.Id,
                .Nombre = categoria.Nombre,
                .Descripcion = categoria.Descripcion
            }
        End Function
    End Class
End Namespace
