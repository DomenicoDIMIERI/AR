Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


 
    ''' <summary>
    ''' Cursore sulla tabella degli uffici
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CUtentiXUfficioCursor
        Inherits DBObjectCursorBase(Of CUtenteXUfficio)

        Private m_IDUtente As CCursorField(Of Integer)
        Private m_IDUfficio As CCursorField(Of Integer)

        Public Sub New()
            Me.m_IDUtente = New CCursorField(Of Integer)("Utente")
            Me.m_IDUfficio = New CCursorField(Of Integer)("Ufficio")
        End Sub

        Public ReadOnly Property IDUtente As CCursorField(Of Integer)
            Get
                Return Me.m_IDUtente
            End Get
        End Property

        Public ReadOnly Property IDUfficio As CCursorField(Of Integer)
            Get
                Return Me.m_IDUfficio
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CUtenteXUfficio
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UtentiXUfficio"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class

 
End Class