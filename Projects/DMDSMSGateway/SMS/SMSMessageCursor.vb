Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

'Namespace DMD

Partial Class SMSGateway


    Public Class SMSMessageCursor
        Inherits DBObjectCursorBase(Of SMSMessage)

        Private m_Driver As New CCursorFieldObj(Of String)("Driver")
        Private m_Modem As New CCursorFieldObj(Of String)("Modem")
        Private m_DataInvio As New CCursorField(Of Date)("DataInvio")
        Private m_DataRicezioneServer As New CCursorField(Of Date)("DataRicezioneServer")
        Private m_DataInvioServer As New CCursorField(Of Date)("DataInvioServer")
        Private m_NumeroTentativiInvio As New CCursorField(Of Integer)("NumeroTentativiInvio")
        Private m_DataRicezione As New CCursorField(Of Date)("DataRicezione")
        Private m_NumeroMittente As New CCursorFieldObj(Of String)("NumeroMittente")
        Private m_NomeMittente As New CCursorFieldObj(Of String)("NomeMittente")
        Private m_NumeroDestinatario As New CCursorFieldObj(Of String)("NumeroDestinatario")
        Private m_NomeDestinatario As New CCursorFieldObj(Of String)("NomeDestinatario")
        Private m_Flags As New CCursorField(Of SMSFlags)("Flags")
        Private m_StatoInvio As New CCursorField(Of SMSSendStatus)("StatoInvio")
        Private m_DettaglioStatoInvio As New CCursorFieldObj(Of String)("DettaglioStatoInvio")
        Private m_StatoRicezione As New CCursorField(Of SMSReceiveStatus)("StatoRicezione")
        Private m_DettaglioStatoRicezione As New CCursorFieldObj(Of String)("DettaglioStatoRicezione")
        Private m_DataLettura As New CCursorField(Of Date)("DataLettura")
        Private m_MessageID As New CCursorFieldObj(Of String)("MessageID")
        Private m_Cartella As New CCursorFieldObj(Of String)("Cartella")

        Public Sub New()

        End Sub

        Public ReadOnly Property Driver As CCursorFieldObj(Of String)
            Get
                Return Me.m_Driver
            End Get
        End Property

        Public ReadOnly Property Modem As CCursorFieldObj(Of String)
            Get
                Return Me.m_Modem
            End Get
        End Property

        Public ReadOnly Property DataInvio As CCursorField(Of Date)
            Get
                Return Me.m_DataInvio
            End Get
        End Property

        Public ReadOnly Property DataRicezioneServer As CCursorField(Of Date)
            Get
                Return Me.m_DataRicezioneServer
            End Get
        End Property

        Public ReadOnly Property DataInvioServer As CCursorField(Of Date)
            Get
                Return Me.m_DataInvioServer
            End Get
        End Property

        Public ReadOnly Property NumeroTentativiInvio As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroTentativiInvio
            End Get
        End Property

        Public ReadOnly Property DataRicezione As CCursorField(Of Date)
            Get
                Return Me.m_DataRicezione
            End Get
        End Property

        Public ReadOnly Property NumeroMittente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroMittente
            End Get
        End Property

        Public ReadOnly Property NomeMittente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeMittente
            End Get
        End Property

        Public ReadOnly Property NumeroDestinatario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroDestinatario
            End Get
        End Property

        Public ReadOnly Property NomeDestinatario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeDestinatario
            End Get
        End Property

        Public ReadOnly Property Cartella As CCursorFieldObj(Of String)
            Get
                Return Me.m_Cartella
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of SMSFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property StatoInvio As CCursorField(Of SMSSendStatus)
            Get
                Return Me.m_StatoInvio
            End Get
        End Property

        Public ReadOnly Property DettaglioStatoInvio As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStatoInvio
            End Get
        End Property

        Public ReadOnly Property StatoRicezione As CCursorField(Of SMSReceiveStatus)
            Get
                Return Me.m_StatoRicezione
            End Get
        End Property

        Public ReadOnly Property DettaglioStatoRicezione As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStatoRicezione
            End Get
        End Property

        Public ReadOnly Property DataLettura As CCursorField(Of Date)
            Get
                Return Me.m_DataLettura
            End Get
        End Property

        Public ReadOnly Property MessageID As CCursorFieldObj(Of String)
            Get
                Return Me.m_MessageID
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return SMSGateway.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Messaggi"
        End Function
    End Class

End Class

'End Namespace