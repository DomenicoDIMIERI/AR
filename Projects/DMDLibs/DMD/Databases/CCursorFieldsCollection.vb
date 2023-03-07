Public partial class Databases

    Public NotInheritable Class CCursorFieldsCollection
        Inherits CKeyCollection(Of CCursorField)

        Public Sub New()
        End Sub

        Public Shadows Sub Add(ByVal f As CCursorField)
            MyBase.Add(f.FieldName, f)
        End Sub

        Public Shadows Sub Clear()
            For Each f As CCursorField In Me
                f.Clear()
            Next
        End Sub

    End Class

End Class
