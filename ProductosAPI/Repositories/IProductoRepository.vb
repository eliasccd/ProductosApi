Imports ProductosAPI.Models
Namespace Repositories
    Public Interface IProductoRepository
        Function GetAllAsync() As Task(Of List(Of Producto))
        Function GetByIdAsync(id As Integer) As Task(Of Producto)
        Function GetByCategoriaIdAsync(categoriaId As Integer) As Task(Of List(Of Producto))
        Function CategoriaExisteAsync(categoriaId As Integer) As Task(Of Boolean)
        Function NombreExisteAsync(nombre As String, Optional excluirId As Integer = 0) As Task(Of Boolean)
        Function AddAsync(producto As Producto) As Task
        Sub Update(producto As Producto)
        Sub Delete(producto As Producto)
        Function SaveChangesAsync() As Task
    End Interface
End Namespace
