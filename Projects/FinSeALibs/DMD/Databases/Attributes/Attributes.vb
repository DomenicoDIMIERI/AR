Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica

Public partial class Databases

    Public NotInheritable Class Attributes
        Private Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce il cursore degli attributi per l'oggetto
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetAttributeCursor(ByVal obj As Object) As CObjectAttributeCursor
            Dim cursor As New CObjectAttributeCursor
            cursor.ObjectID.Value = GetID(obj)
            cursor.ObjectType.Value = TypeName(obj)
            Return cursor
        End Function

        Public Shared Function GetAttributeObject(ByVal obj As Object, ByVal attributeName As String) As CObjectAttribute
            Dim cursor As CObjectAttributeCursor = GetAttributeCursor(obj)
            Dim ret As CObjectAttribute
            cursor.AttributeName.Value = attributeName
            ret = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function


        Public Shared Function GetAttributeValue(ByVal obj As Object, ByVal attributeName As String) As Object
            Dim a As CObjectAttribute = GetAttributeObject(obj, attributeName)
            If a Is Nothing Then Return Nothing
            Return a.AttributeValue
        End Function


        Public Shared Function SetAttributeValue(ByVal obj As Object, ByVal attributeName As String, ByVal attributeValue As Object) As CObjectAttribute
            Dim a As CObjectAttribute = GetAttributeObject(obj, attributeName)
            If a Is Nothing Then
                a = New CObjectAttribute
                a.Object = obj
                a.AttributeName = attributeName
            End If
            a.AttributeValue = attributeValue
            Return a
        End Function

    End Class
    

End Class

