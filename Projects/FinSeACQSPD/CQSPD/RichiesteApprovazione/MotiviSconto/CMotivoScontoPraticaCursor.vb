Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

     

    ''' <summary>
    ''' Cursore sulla tabella degli sconti
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CMotivoScontoPraticaCursor
        Inherits DBObjectCursor(Of CMotivoScontoPratica)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property
 
        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.MotiviSconto.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDMotiviSconti"
        End Function
    End Class




End Class
