Imports ProductosAPI.DTOs
Imports ProductosAPI.Services
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Namespace Controllers
    <ApiController>
    <Route("api/productos")>
    Public Class ProductoController
        Inherits ControllerBase
        Private ReadOnly _service As IProductoService
        Public Sub New(service As IProductoService)
            _service = service
        End Sub
        ''' <summary>Obtiene todos los productos junto con el nombre de su categoría.</summary>
        <HttpGet>
        <ProducesResponseType(GetType(List(Of ProductoResponseDto)), StatusCodes.Status200OK)>
        Public Async Function GetAll() As Task(Of ActionResult(Of List(Of ProductoResponseDto)))
            Return Ok(Await _service.GetAllAsync())
        End Function
        ''' <summary>Obtiene un producto por su identificador.</summary>
        ''' <param name="id">Identificador del producto.</param>
        <HttpGet("{id:int}")>
        Public Async Function GetById(id As Integer) As Task(Of ActionResult(Of ProductoResponseDto))
            Return Ok(Await _service.GetByIdAsync(id))
        End Function
        ''' <summary>Obtiene los productos de una categoría existente.</summary>
        ''' <param name="categoriaId">Identificador de la categoría.</param>
        <HttpGet("categoria/{categoriaId:int}")>
        Public Async Function GetByCategory(categoriaId As Integer) As Task(Of ActionResult(Of List(Of ProductoResponseDto)))
            Return Ok(Await _service.GetByCategoriaIdAsync(categoriaId))
        End Function
        ''' <summary>Registra un producto asociado a una categoría existente.</summary>
        ''' <remarks>El nombre debe ser único. El precio debe ser mayor que cero y el stock no puede ser negativo.</remarks>
        <HttpPost>
        <ProducesResponseType(GetType(ProductoResponseDto), StatusCodes.Status201Created)>
        Public Async Function Create(<FromBody> dto As ProductoCreateDto) As Task(Of ActionResult(Of ProductoResponseDto))
            Dim result = Await _service.CreateAsync(dto)
            Return CreatedAtAction(NameOf(GetById), New With {.id = result.Id}, result)
        End Function
        ''' <summary>Modifica los datos de un producto existente.</summary>
        ''' <param name="id">Identificador del producto.</param>
        <HttpPut("{id:int}")>
        Public Async Function Update(id As Integer, <FromBody> dto As ProductoUpdateDto) As Task(Of ActionResult(Of ProductoResponseDto))
            Return Ok(Await _service.UpdateAsync(id, dto))
        End Function
        ''' <summary>Elimina un producto.</summary>
        ''' <param name="id">Identificador del producto.</param>
        <HttpDelete("{id:int}")>
        <ProducesResponseType(StatusCodes.Status204NoContent)>
        Public Async Function Delete(id As Integer) As Task(Of IActionResult)
            Await _service.DeleteAsync(id)
            Return NoContent()
        End Function
    End Class
End Namespace
