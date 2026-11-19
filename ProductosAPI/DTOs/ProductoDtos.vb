Imports System.ComponentModel.DataAnnotations

Namespace DTOs
    Public Class ProductoCreateDto
        <Required, StringLength(50)>
        Public Property Nombre As String = String.Empty
        <StringLength(200)>
        Public Property Descripcion As String = String.Empty
        <Range(GetType(Decimal), "0.01", "9999999999999999")>
        Public Property Precio As Decimal
        <Range(0, Integer.MaxValue)>
        Public Property Stock As Integer
        <Range(1, Integer.MaxValue)>
        Public Property CategoriaId As Integer
    End Class

    Public Class ProductoUpdateDto
        Inherits ProductoCreateDto
    End Class

    Public Class ProductoResponseDto
        Public Property Id As Integer
        Public Property Nombre As String = String.Empty
        Public Property Descripcion As String = String.Empty
        Public Property Precio As Decimal
        Public Property Stock As Integer
        Public Property FechaRegistro As DateTime
        Public Property CategoriaId As Integer
        Public Property CategoriaNombre As String = String.Empty
    End Class
End Namespace
