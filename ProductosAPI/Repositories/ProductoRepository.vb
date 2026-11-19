Imports ProductosAPI.Data
Imports ProductosAPI.Models
Imports Microsoft.EntityFrameworkCore
Namespace Repositories
    Public Class ProductoRepository
        Implements IProductoRepository
        Private ReadOnly _context As ApplicationDbContext
        Public Sub New(context As ApplicationDbContext)
            _context = context
        End Sub
        Public Async Function GetAllAsync() As Task(Of List(Of Producto)) Implements IProductoRepository.GetAllAsync
            Return Await _context.Productos.AsNoTracking().Include(Function(p) p.Categoria).OrderBy(Function(p) p.Id).ToListAsync()
        End Function
        Public Async Function GetByIdAsync(id As Integer) As Task(Of Producto) Implements IProductoRepository.GetByIdAsync
            Return Await _context.Productos.Include(Function(p) p.Categoria).FirstOrDefaultAsync(Function(p) p.Id = id)
        End Function
        Public Async Function GetByCategoriaIdAsync(categoriaId As Integer) As Task(Of List(Of Producto)) Implements IProductoRepository.GetByCategoriaIdAsync
            Return Await _context.Productos.AsNoTracking().Include(Function(p) p.Categoria).Where(Function(p) p.CategoriaId = categoriaId).OrderBy(Function(p) p.Id).ToListAsync()
        End Function
        Public Async Function CategoriaExisteAsync(categoriaId As Integer) As Task(Of Boolean) Implements IProductoRepository.CategoriaExisteAsync
            Return Await _context.Categorias.AnyAsync(Function(c) c.Id = categoriaId)
        End Function
        Public Async Function NombreExisteAsync(nombre As String, Optional excluirId As Integer = 0) As Task(Of Boolean) Implements IProductoRepository.NombreExisteAsync
            Dim normalizado = nombre.Trim().ToUpper()
            Return Await _context.Productos.AnyAsync(Function(p) p.Id <> excluirId AndAlso p.Nombre.ToUpper() = normalizado)
        End Function
        Public Async Function AddAsync(producto As Producto) As Task Implements IProductoRepository.AddAsync
            Await _context.Productos.AddAsync(producto)
        End Function
        Public Sub Update(producto As Producto) Implements IProductoRepository.Update
            _context.Productos.Update(producto)
        End Sub
        Public Sub Delete(producto As Producto) Implements IProductoRepository.Delete
            _context.Productos.Remove(producto)
        End Sub
        Public Async Function SaveChangesAsync() As Task Implements IProductoRepository.SaveChangesAsync
            Await _context.SaveChangesAsync()
        End Function
    End Class
End Namespace
