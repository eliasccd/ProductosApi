Imports System.ComponentModel.DataAnnotations

Namespace DTOs
    Public Class CategoriaCreateDto
        <Required, StringLength(50)>
        Public Property Nombre As String = String.Empty
        <StringLength(200)>
        Public Property Descripcion As String = String.Empty
    End Class

    Public Class CategoriaUpdateDto
        Inherits CategoriaCreateDto
    End Class

    Public Class CategoriaResponseDto
        Public Property Id As Integer
        Public Property Nombre As String = String.Empty
        Public Property Descripcion As String = String.Empty
        Public Property Productos As List(Of ProductoResponseDto) = New List(Of ProductoResponseDto)()
    End Class
End Namespace
