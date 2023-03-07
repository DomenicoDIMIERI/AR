Partial Public Class Office

    <Serializable>
    Public Class TemplateItemsCollection
        Inherits CCollection(Of TemplateItem)

        Private m_Document As DocumentTemplate

        Public Sub New()
            Me.m_Document = Nothing
        End Sub

        Public Sub New(ByVal doc As DocumentTemplate)
            Me.New
            Me.SetDocument(doc)
        End Sub

        Protected Friend Overridable Sub SetDocument(ByVal doc As DocumentTemplate)
            Me.m_Document = doc
        End Sub

        Public Overloads Function Add(ByVal type As TemplateItemTypes, ByVal text As String, ByVal bounds As CRectangle) As TemplateItem
            Dim ret As New TemplateItem(type, text, bounds)
            Me.Add(ret)
            Return ret
        End Function

        Public Overloads Function Add(ByVal type As TemplateItemTypes, ByVal text As String, ByVal bounds As CRectangle, ByVal color As String) As TemplateItem
            Dim ret As New TemplateItem(type, text, bounds, color)
            Me.Add(ret)
            Return ret
        End Function

    End Class

End Class