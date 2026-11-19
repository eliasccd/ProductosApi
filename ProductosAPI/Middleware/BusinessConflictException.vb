Namespace Middleware
    Public Class BusinessConflictException
        Inherits Exception
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
    End Class
End Namespace
