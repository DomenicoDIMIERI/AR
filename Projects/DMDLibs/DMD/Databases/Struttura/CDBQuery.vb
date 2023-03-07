Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CDBQuery
        Inherits CDBEntity

        Public Sub New()
        End Sub

        Protected Overrides Sub CreateInternal1()
            Me.Connection.CreateView(Me)
        End Sub

        Protected Overrides Sub DropInternal1()
            Me.Connection.DropView(Me)
            Me.Connection.Queries.RemoveByKey(Me.Name)
        End Sub

        Protected Overrides Sub UpdateInternal1()
            Me.Connection.UpdateView(Me)
        End Sub

        Protected Overrides Sub RenameItnernal(newName As String)
            Throw New NotImplementedException
        End Sub

    End Class

End Class


