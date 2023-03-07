Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    Public Class FaxDocumentsCursor
        Inherits CCustomerCallsCursor

        'Private m_Numero As New CCursorFieldObj(Of String)("Numero")

        Public Sub New()
            MyBase.ClassName.Value = "FAXDocument"
        End Sub

        'Public ReadOnly Property Numero As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Numero
        '    End Get
        'End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New FaxDocument
        End Function

        Public Shadows Property Item As FaxDocument
            Get
                Return MyBase.Item
            End Get
            Set(value As FaxDocument)
                MyBase.Item = value
            End Set
        End Property

    End Class



End Class