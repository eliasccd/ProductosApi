Imports ProductosAPI.DTOs
Imports ProductosAPI.Services
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Namespace Controllers
    <ApiController>
    <Route("api/categorias")>
    Public Class CategoriaController
        Inherits ControllerBase
        Private ReadOnly _service As ICategoriaService
        Public Sub New(service As ICategoriaService)
            _service = service
        End Sub
        ''' <summary>Obtiene todas las categorías.</summary>
        <HttpGet>
        Public Async Function GetAll() As Task(Of ActionResult(Of List(Of CategoriaResponseDto)))
            Return Ok(Await _service.GetAllAsync())
        End Function
        ''' <summary>Obtiene una categoría y los productos que contiene.</summary>
        ''' <param name="id">Identificador de la categoría.</param>
        <HttpGet("{id:int}")>
        Public Async Function GetById(id As Integer) As Task(Of ActionResult(Of CategoriaResponseDto))
            Return Ok(Await _service.GetByIdAsync(id))
        End Function
        ''' <summary>Registra una categoría con nombre único.</summary>
        <HttpPost>
        <ProducesResponseType(GetType(CategoriaResponseDto), StatusCodes.Status201Created)>
        Public Async Function Create(<FromBody> dto As CategoriaCreateDto) As Task(Of ActionResult(Of CategoriaResponseDto))
            Dim result = Await _service.CreateAsync(dto)
            Return CreatedAtAction(NameOf(GetById), New With {.id = result.Id}, result)
        End Function
        ''' <summary>Modifica una categoría existente.</summary>
        ''' <param name="id">Identificador de la categoría.</param>
        <HttpPut("{id:int}")>
        Public Async Function Update(id As Integer, <FromBody> dto As CategoriaUpdateDto) As Task(Of ActionResult(Of CategoriaResponseDto))
            Return Ok(Await _service.UpdateAsync(id, dto))
        End Function
        ''' <summary>Elimina una categoría únicamente si no tiene productos asociados.</summary>
        ''' <param name="id">Identificador de la categoría.</param>
        <HttpDelete("{id:int}")>
        Public Async Function Delete(id As Integer) As Task(Of IActionResult)
            Await _service.DeleteAsync(id)
            Return NoContent()
        End Function
    End Class
End Namespace
