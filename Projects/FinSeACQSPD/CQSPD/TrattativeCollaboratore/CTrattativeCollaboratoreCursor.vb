Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Class CTrattativeCollaboratoreCursor
        Inherits DBObjectCursor(Of CTrattativaCollaboratore)

        Private m_IDCollaboratore As New CCursorField(Of Integer)("Collaboratore")

        Public Sub New()
        End Sub

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.TrattativeCollaboratore.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CollaboratoriTrattative"
        End Function

        Public ReadOnly Property IDCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDCollaboratore
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class

   
End Class
