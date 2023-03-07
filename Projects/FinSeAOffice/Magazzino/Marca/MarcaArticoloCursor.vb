Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
 
    Public Class MarcaArticoloCursor
        Inherits DBObjectCursor(Of MarcaArticolo)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        

        Public Sub New()
        End Sub


        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property
         

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.MarcheArticoli.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeMarcheArticoli"
        End Function


    End Class

End Class


