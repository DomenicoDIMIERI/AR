Namespace java
    Public Class LinkedList(Of T)
        Inherits ArrayList(Of T)

        Public Sub New()
        End Sub

        Public Sub New(ByVal capacity As Integer)
            MyBase.New(capacity)
        End Sub

        Public Sub New(ByVal col As Global.System.Collections.IEnumerable)
            MyBase.New(col)
        End Sub

        Function RemoveFirst() As T
            Me.remove(0)
        End Function

        Function addFirst(p1 As T) As Integer
            Throw New NotImplementedException
        End Function

        Function poll() As T
            Throw New NotImplementedException
        End Function


    End Class

End Namespace