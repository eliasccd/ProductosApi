Imports ProductosAPI.Data
Imports ProductosAPI.Models
Imports Microsoft.EntityFrameworkCore
Namespace Repositories
    Public Class CategoriaRepository
        Implements ICategoriaRepository
        Private ReadOnly _context As ApplicationDbContext
        Public Sub New(context As ApplicationDbContext)
            _context = context
        End Sub
        Public Async Function GetAllAsync() As Task(Of List(Of Categoria)) Implements ICategoriaRepository.GetAllAsync
            Return Await _context.Categorias.AsNoTracking().OrderBy(Function(c) c.Id).ToListAsync()
        End Function
        Public Async Function GetByIdAsync(id As Integer) As Task(Of Categoria) Implements ICategoriaRepository.GetByIdAsync
            Return Await _context.Categorias.Include(Function(c) c.Productos).ThenInclude(Function(p) p.Categoria).FirstOrDefaultAsync(Function(c) c.Id = id)
        End Function
        Public Async Function NombreExisteAsync(nombre As String, Optional excluirId As Integer = 0) As Task(Of Boolean) Implements ICategoriaRepository.NombreExisteAsync
            Dim normalizado = nombre.Trim().ToUpper()
            Return Await _context.Categorias.AnyAsync(Function(c) c.Id <> excluirId AndAlso c.Nombre.ToUpper() = normalizado)
        End Function
        Public Async Function TieneProductosAsync(id As Integer) As Task(Of Boolean) Implements ICategoriaRepository.TieneProductosAsync
            Return Await _context.Productos.AnyAsync(Function(p) p.CategoriaId = id)
        End Function
        Public Async Function AddAsync(categoria As Categoria) As Task Implements ICategoriaRepository.AddAsync
            Await _context.Categorias.AddAsync(categoria)
        End Function
        Public Sub Delete(categoria As Categoria) Implements ICategoriaRepository.Delete
            _context.Categorias.Remove(categoria)
        End Sub
        Public Async Function SaveChangesAsync() As Task Implements ICategoriaRepository.SaveChangesAsync
            Await _context.SaveChangesAsync()
        End Function
    End Class
End Namespace
