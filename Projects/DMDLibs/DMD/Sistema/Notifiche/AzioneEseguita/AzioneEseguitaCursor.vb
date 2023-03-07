Imports DMD.Databases

Partial Public Class Sistema



    ''' <summary>
    ''' Cursore sulla tabella delle azione eseguite
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AzioneEseguitaCursor
        Inherits DBObjectCursor

        Private m_IDNotifica As New CCursorField(Of Integer)("Notifica")
        Private m_AzioneType As New CCursorFieldObj(Of String)("Azione")
        Private m_DataEsecuzione As New CCursorField(Of Date)("DataEsecuzione")
        Private m_Parameters As New CCursorFieldObj(Of String)("Parameters")
        Private m_Results As New CCursorFieldObj(Of String)("Results")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDNotifica As CCursorField(Of Integer)
            Get
                Return Me.m_IDNotifica
            End Get
        End Property

        Public ReadOnly Property AzioneType As CCursorFieldObj(Of String)
            Get
                Return Me.m_AzioneType
            End Get
        End Property

        Public ReadOnly Property DataEsecuzione As CCursorField(Of Date)
            Get
                Return Me.m_DataEsecuzione
            End Get
        End Property

        Public ReadOnly Property Parameters As CCursorFieldObj(Of String)
            Get
                Return Me.m_Parameters
            End Get
        End Property

        Public ReadOnly Property Results As CCursorFieldObj(Of String)
            Get
                Return Me.m_Results
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Notifiche.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SYSNotifyRes"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New AzioneEseguita
        End Function

    End Class


End Class