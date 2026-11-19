Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports InventarioApiVB.Data
Imports InventarioApiVB.Models
Imports Microsoft.EntityFrameworkCore

Namespace Repositories
    Public Class CategoriaRepository
        Implements ICategoriaRepository

        Private ReadOnly _context As InventarioDbContext

        Public Sub New(context As InventarioDbContext)
            _context = context
        End Sub

        Public Async Function GetAllAsync() As Task(Of IEnumerable(Of Categoria)) Implements ICategoriaRepository.GetAllAsync
            Return Await _context.Categorias _
                .AsNoTracking() _
                .OrderBy(Function(c) c.Id) _
                .ToListAsync()
        End Function

        Public Async Function GetByIdAsync(id As Integer) As Task(Of Categoria) Implements ICategoriaRepository.GetByIdAsync
            Return Await _context.Categorias _
                .FirstOrDefaultAsync(Function(c) c.Id = id)
        End Function

        Public Async Function GetByIdWithProductosAsync(id As Integer) As Task(Of Categoria) Implements ICategoriaRepository.GetByIdWithProductosAsync
            Return Await _context.Categorias _
                .Include(Function(c) c.Productos) _
                .AsNoTracking() _
                .FirstOrDefaultAsync(Function(c) c.Id = id)
        End Function

        Public Async Function ExistsAsync(id As Integer) As Task(Of Boolean) Implements ICategoriaRepository.ExistsAsync
            Return Await _context.Categorias.AnyAsync(Function(c) c.Id = id)
        End Function

        Public Async Function ExistsByNombreAsync(nombre As String, Optional excludeId As Integer? = Nothing) As Task(Of Boolean) Implements ICategoriaRepository.ExistsByNombreAsync
            Dim nombreNormalizado = nombre.Trim().ToLower()
            Dim query = _context.Categorias.AsQueryable()

            If excludeId.HasValue Then
                query = query.Where(Function(c) c.Id <> excludeId.Value)
            End If

            Return Await query.AnyAsync(Function(c) c.Nombre.ToLower() = nombreNormalizado)
        End Function

        Public Async Function HasProductosAsync(id As Integer) As Task(Of Boolean) Implements ICategoriaRepository.HasProductosAsync
            Return Await _context.Productos.AnyAsync(Function(p) p.CategoriaId = id)
        End Function

        Public Async Function AddAsync(categoria As Categoria) As Task(Of Categoria) Implements ICategoriaRepository.AddAsync
            Await _context.Categorias.AddAsync(categoria)
            Await _context.SaveChangesAsync()
            Return categoria
        End Function

        Public Async Function UpdateAsync(categoria As Categoria) As Task(Of Categoria) Implements ICategoriaRepository.UpdateAsync
            _context.Categorias.Update(categoria)
            Await _context.SaveChangesAsync()
            Return categoria
        End Function

        Public Async Function DeleteAsync(categoria As Categoria) As Task Implements ICategoriaRepository.DeleteAsync
            _context.Categorias.Remove(categoria)
            Await _context.SaveChangesAsync()
        End Function
    End Class
End Namespace
