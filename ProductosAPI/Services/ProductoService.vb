Imports ProductosAPI.DTOs
Imports ProductosAPI.Models
Imports ProductosAPI.Repositories
Imports ProductosAPI.Middleware
Namespace Services
    Public Class ProductoService
        Implements IProductoService
        Private ReadOnly _repository As IProductoRepository
        Public Sub New(repository As IProductoRepository)
            _repository = repository
        End Sub
        Private Shared Function Map(p As Producto) As ProductoResponseDto
            Return New ProductoResponseDto With {.Id = p.Id, .Nombre = p.Nombre, .Descripcion = p.Descripcion, .Precio = p.Precio, .Stock = p.Stock, .FechaRegistro = p.FechaRegistro, .CategoriaId = p.CategoriaId, .CategoriaNombre = p.Categoria.Nombre}
        End Function
        Public Async Function GetAllAsync() As Task(Of List(Of ProductoResponseDto)) Implements IProductoService.GetAllAsync
            Return (Await _repository.GetAllAsync()).Select(Function(p) Map(p)).ToList()
        End Function
        Public Async Function GetByIdAsync(id As Integer) As Task(Of ProductoResponseDto) Implements IProductoService.GetByIdAsync
            Dim p = Await _repository.GetByIdAsync(id)
            If p Is Nothing Then Throw New KeyNotFoundException($"No existe el producto con Id {id}.")
            Return Map(p)
        End Function
        Public Async Function GetByCategoriaIdAsync(categoriaId As Integer) As Task(Of List(Of ProductoResponseDto)) Implements IProductoService.GetByCategoriaIdAsync
            If Not Await _repository.CategoriaExisteAsync(categoriaId) Then Throw New KeyNotFoundException($"No existe la categoría con Id {categoriaId}.")
            Return (Await _repository.GetByCategoriaIdAsync(categoriaId)).Select(Function(p) Map(p)).ToList()
        End Function
        Public Async Function CreateAsync(dto As ProductoCreateDto) As Task(Of ProductoResponseDto) Implements IProductoService.CreateAsync
            If Await _repository.NombreExisteAsync(dto.Nombre) Then Throw New BusinessConflictException("Ya existe un producto con ese nombre.")
            If Not Await _repository.CategoriaExisteAsync(dto.CategoriaId) Then Throw New BusinessConflictException("La categoría indicada no existe.")
            Dim p = New Producto With {.Nombre = dto.Nombre.Trim(), .Descripcion = dto.Descripcion, .Precio = dto.Precio, .Stock = dto.Stock, .CategoriaId = dto.CategoriaId, .FechaRegistro = DateTime.UtcNow}
            Await _repository.AddAsync(p)
            Await _repository.SaveChangesAsync()
            Return Await GetByIdAsync(p.Id)
        End Function
        Public Async Function UpdateAsync(id As Integer, dto As ProductoUpdateDto) As Task(Of ProductoResponseDto) Implements IProductoService.UpdateAsync
            Dim p = Await _repository.GetByIdAsync(id)
            If p Is Nothing Then Throw New KeyNotFoundException($"No existe el producto con Id {id}.")
            If Await _repository.NombreExisteAsync(dto.Nombre, id) Then Throw New BusinessConflictException("Ya existe un producto con ese nombre.")
            If Not Await _repository.CategoriaExisteAsync(dto.CategoriaId) Then Throw New BusinessConflictException("La categoría indicada no existe.")
            p.Nombre = dto.Nombre.Trim() : p.Descripcion = dto.Descripcion : p.Precio = dto.Precio : p.Stock = dto.Stock : p.CategoriaId = dto.CategoriaId
            Await _repository.SaveChangesAsync()
            Return Await GetByIdAsync(id)
        End Function
        Public Async Function DeleteAsync(id As Integer) As Task Implements IProductoService.DeleteAsync
            Dim p = Await _repository.GetByIdAsync(id)
            If p Is Nothing Then Throw New KeyNotFoundException($"No existe el producto con Id {id}.")
            _repository.Delete(p)
            Await _repository.SaveChangesAsync()
        End Function
    End Class
End Namespace
