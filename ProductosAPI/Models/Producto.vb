Namespace Models
    Public Class Producto
        Public Property Id As Integer
        Public Property Nombre As String = String.Empty
        Public Property Descripcion As String = String.Empty
        Public Property Precio As Decimal
        Public Property Stock As Integer
        Public Property FechaRegistro As DateTime = DateTime.UtcNow
        Public Property CategoriaId As Integer
        Public Property Categoria As Categoria
    End Class
End Namespace
