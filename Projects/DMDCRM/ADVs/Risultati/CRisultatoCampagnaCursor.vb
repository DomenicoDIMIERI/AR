Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV

    Public Class CRisultatoCampagnaCursor
        Inherits DBObjectCursor(Of CRisultatoCampagna)

        Private m_IDCampagna As New CCursorField(Of Integer)("IDCampagna")
        Private m_NomeCampagna As New CCursorFieldObj(Of String)("NomeCampagna")
        Private m_IDDestinatario As New CCursorField(Of Integer)("IDDestinatario")
        Private m_NomeDestinatario As New CCursorFieldObj(Of String)("NomeDestinatario")
        Private m_StatoMessaggio As New CCursorField(Of StatoMessaggioCampagna)("StatoMessaggio")
        Private m_DataSpedizione As New CCursorField(Of Date)("DataSpedizione")
        Private m_NomeMezzoSpedizione As New CCursorFieldObj(Of String)("NomeMezzoSpedizione")
        Private m_StatoSpedizione As New CCursorFieldObj(Of String)("StatoSpedizione")
        Private m_DataConsegna As New CCursorField(Of Date)("DataConsegna")
        Private m_DataLettura As New CCursorField(Of Date)("DataLettura")
        Private m_TipoCampagna As New CCursorField(Of TipoCampagnaPubblicitaria)("TipoCampagna")
        Private m_DataEsecuzione As New CCursorField(Of Date)("DataEsecuzione")
        Private m_MessageID As New CCursorFieldObj(Of String)("MessageID")
        Private m_IndirizzoDestinatario As New CCursorFieldObj(Of String)("IndirizzoDestinatario")


        Public Sub New()
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return ADV.RisultatiCampagna.Module
        End Function

        Public ReadOnly Property IndirizzoDestinatario As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDestinatario
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_ADVResults"
        End Function

        Public ReadOnly Property MessageID As CCursorFieldObj(Of String)
            Get
                Return Me.m_MessageID
            End Get
        End Property

        Public ReadOnly Property IDCampagna As CCursorField(Of Integer)
            Get
                Return Me.m_IDCampagna
            End Get
        End Property

        Public ReadOnly Property NomeCampagna As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCampagna
            End Get
        End Property

        Public ReadOnly Property IDDestinatario As CCursorField(Of Integer)
            Get
                Return Me.m_IDDestinatario
            End Get
        End Property

        Public ReadOnly Property NomeDestinatario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeDestinatario
            End Get
        End Property

        Public ReadOnly Property StatoMessaggio As CCursorField(Of StatoMessaggioCampagna)
            Get
                Return Me.m_StatoMessaggio
            End Get
        End Property

        Public ReadOnly Property DataSpedizione As CCursorField(Of Date)
            Get
                Return Me.m_DataSpedizione
            End Get
        End Property

        Public ReadOnly Property NomeMezzoSpedizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeMezzoSpedizione
            End Get
        End Property

        Public ReadOnly Property StatoSpedizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_StatoSpedizione
            End Get
        End Property

        Public ReadOnly Property DataConsegna As CCursorField(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
        End Property

        Public ReadOnly Property DataLettura As CCursorField(Of Date)
            Get
                Return Me.m_DataLettura
            End Get
        End Property

        Public ReadOnly Property TipoCampagna As CCursorField(Of TipoCampagnaPubblicitaria)
            Get
                Return Me.m_TipoCampagna
            End Get
        End Property

        Public ReadOnly Property DataEsecuzione As CCursorField(Of Date)
            Get
                Return Me.m_DataEsecuzione
            End Get
        End Property

    End Class

End Class