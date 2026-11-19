Imports ProductosAPI.Models
Namespace Repositories
    Public Interface ICategoriaRepository
        Function GetAllAsync() As Task(Of List(Of Categoria))
        Function GetByIdAsync(id As Integer) As Task(Of Categoria)
        Function NombreExisteAsync(nombre As String, Optional excluirId As Integer = 0) As Task(Of Boolean)
        Function TieneProductosAsync(id As Integer) As Task(Of Boolean)
        Function AddAsync(categoria As Categoria) As Task
        Sub Delete(categoria As Categoria)
        Function SaveChangesAsync() As Task
    End Interface
End Namespace
