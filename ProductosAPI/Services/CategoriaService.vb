Imports ProductosAPI.DTOs
Imports ProductosAPI.Models
Imports ProductosAPI.Repositories
Imports ProductosAPI.Middleware
Namespace Services
    Public Class CategoriaService
        Implements ICategoriaService
        Private ReadOnly _repository As ICategoriaRepository
        Public Sub New(repository As ICategoriaRepository)
            _repository = repository
        End Sub
        Private Shared Function Map(c As Categoria) As CategoriaResponseDto
            Return New CategoriaResponseDto With {.Id = c.Id, .Nombre = c.Nombre, .Descripcion = c.Descripcion, .Productos = c.Productos.Select(Function(p) New ProductoResponseDto With {.Id = p.Id, .Nombre = p.Nombre, .Descripcion = p.Descripcion, .Precio = p.Precio, .Stock = p.Stock, .FechaRegistro = p.FechaRegistro, .CategoriaId = p.CategoriaId, .CategoriaNombre = c.Nombre}).ToList()}
        End Function
        Public Async Function GetAllAsync() As Task(Of List(Of CategoriaResponseDto)) Implements ICategoriaService.GetAllAsync
            Return (Await _repository.GetAllAsync()).Select(Function(c) Map(c)).ToList()
        End Function
        Public Async Function GetByIdAsync(id As Integer) As Task(Of CategoriaResponseDto) Implements ICategoriaService.GetByIdAsync
            Dim c = Await _repository.GetByIdAsync(id)
            If c Is Nothing Then Throw New KeyNotFoundException($"No existe la categoría con Id {id}.")
            Return Map(c)
        End Function
        Public Async Function CreateAsync(dto As CategoriaCreateDto) As Task(Of CategoriaResponseDto) Implements ICategoriaService.CreateAsync
            If Await _repository.NombreExisteAsync(dto.Nombre) Then Throw New BusinessConflictException("Ya existe una categoría con ese nombre.")
            Dim c = New Categoria With {.Nombre = dto.Nombre.Trim(), .Descripcion = dto.Descripcion}
            Await _repository.AddAsync(c) : Await _repository.SaveChangesAsync()
            Return Await GetByIdAsync(c.Id)
        End Function
        Public Async Function UpdateAsync(id As Integer, dto As CategoriaUpdateDto) As Task(Of CategoriaResponseDto) Implements ICategoriaService.UpdateAsync
            Dim c = Await _repository.GetByIdAsync(id)
            If c Is Nothing Then Throw New KeyNotFoundException($"No existe la categoría con Id {id}.")
            If Await _repository.NombreExisteAsync(dto.Nombre, id) Then Throw New BusinessConflictException("Ya existe una categoría con ese nombre.")
            c.Nombre = dto.Nombre.Trim() : c.Descripcion = dto.Descripcion
            Await _repository.SaveChangesAsync()
            Return Await GetByIdAsync(id)
        End Function
        Public Async Function DeleteAsync(id As Integer) As Task Implements ICategoriaService.DeleteAsync
            Dim c = Await _repository.GetByIdAsync(id)
            If c Is Nothing Then Throw New KeyNotFoundException($"No existe la categoría con Id {id}.")
            If Await _repository.TieneProductosAsync(id) Then Throw New BusinessConflictException("No se puede eliminar una categoría que tiene productos asociados.")
            _repository.Delete(c) : Await _repository.SaveChangesAsync()
        End Function
    End Class
End Namespace
