Imports System

Namespace Exceptions
    Public Class NotFoundException
        Inherits Exception

        Public Sub New(mensaje As String)
            MyBase.New(mensaje)
        End Sub
    End Class
End Namespace
