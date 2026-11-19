Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports InventarioApiVB.Data
Imports InventarioApiVB.Models
Imports Microsoft.EntityFrameworkCore

Namespace Repositories
    Public Class ProductoRepository
        Implements IProductoRepository

        Private ReadOnly _context As InventarioDbContext

        Public Sub New(context As InventarioDbContext)
            _context = context
        End Sub

        Public Async Function GetAllWithCategoriaAsync() As Task(Of IEnumerable(Of Producto)) Implements IProductoRepository.GetAllWithCategoriaAsync
            Return Await _context.Productos _
                .Include(Function(p) p.Categoria) _
                .AsNoTracking() _
                .OrderBy(Function(p) p.Id) _
                .ToListAsync()
        End Function

        Public Async Function GetByIdWithCategoriaAsync(id As Integer) As Task(Of Producto) Implements IProductoRepository.GetByIdWithCategoriaAsync
            Return Await _context.Productos _
                .Include(Function(p) p.Categoria) _
                .AsNoTracking() _
                .FirstOrDefaultAsync(Function(p) p.Id = id)
        End Function

        Public Async Function GetByIdAsync(id As Integer) As Task(Of Producto) Implements IProductoRepository.GetByIdAsync
            Return Await _context.Productos _
                .FirstOrDefaultAsync(Function(p) p.Id = id)
        End Function

        Public Async Function GetByCategoriaIdAsync(categoriaId As Integer) As Task(Of IEnumerable(Of Producto)) Implements IProductoRepository.GetByCategoriaIdAsync
            Return Await _context.Productos _
                .Include(Function(p) p.Categoria) _
                .AsNoTracking() _
                .Where(Function(p) p.CategoriaId = categoriaId) _
                .OrderBy(Function(p) p.Id) _
                .ToListAsync()
        End Function

        Public Async Function ExistsByNombreAsync(nombre As String, Optional excludeId As Integer? = Nothing) As Task(Of Boolean) Implements IProductoRepository.ExistsByNombreAsync
            Dim nombreNormalizado = nombre.Trim().ToLower()
            Dim query = _context.Productos.AsQueryable()

            If excludeId.HasValue Then
                query = query.Where(Function(p) p.Id <> excludeId.Value)
            End If

            Return Await query.AnyAsync(Function(p) p.Nombre.ToLower() = nombreNormalizado)
        End Function

        Public Async Function AddAsync(producto As Producto) As Task(Of Producto) Implements IProductoRepository.AddAsync
            Await _context.Productos.AddAsync(producto)
            Await _context.SaveChangesAsync()
            Return Await GetByIdWithCategoriaAsync(producto.Id)
        End Function

        Public Async Function UpdateAsync(producto As Producto) As Task(Of Producto) Implements IProductoRepository.UpdateAsync
            _context.Productos.Update(producto)
            Await _context.SaveChangesAsync()
            Return Await GetByIdWithCategoriaAsync(producto.Id)
        End Function

        Public Async Function DeleteAsync(producto As Producto) As Task Implements IProductoRepository.DeleteAsync
            _context.Productos.Remove(producto)
            Await _context.SaveChangesAsync()
        End Function
    End Class
End Namespace
