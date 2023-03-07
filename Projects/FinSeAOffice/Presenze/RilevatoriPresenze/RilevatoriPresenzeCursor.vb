Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office


    Public Class RilevatoriPresenzeCursor
        Inherits DBObjectCursorPO(Of RilevatorePresenze)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Modello As New CCursorFieldObj(Of String)("Modello")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property Modello As CCursorFieldObj(Of String)
            Get
                Return Me.m_Modello
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeRilevP"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.RilevatoriPresenze.Module
        End Function

    End Class


End Class