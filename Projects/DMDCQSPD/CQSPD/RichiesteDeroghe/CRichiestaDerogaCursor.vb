Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Class CRichiestaDerogaCursor
        Inherits DBObjectCursorPO(Of CRichiestaDeroga)

        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_StatoRichiesta As New CCursorField(Of StatoRichiestaDeroga)("StatoRichiesta")
        Private m_DataRichiesta As New CCursorField(Of Date)("DataRichiesta")
        Private m_IDRichiedente As New CCursorField(Of Integer)("IDRichiedente")
        Private m_NomeRichiedente As New CCursorFieldObj(Of String)("NomeRichiedente")
        Private m_MotivoRichiesta As New CCursorFieldObj(Of String)("MotivoRichiesta")
        Private m_IDAgenziaConcorrente As New CCursorField(Of Integer)("IDAgenziaConcorrente")
        Private m_NomeAgenziaConcorrente As New CCursorFieldObj(Of String)("NomeAgenziaConcorrente")
        Private m_NomeProdottoConcorrente As New CCursorFieldObj(Of String)("NomeProdottoConcorrente")
        Private m_NumeroPreventivoConcorrente As New CCursorFieldObj(Of String)("NumeroPreventivoConcorrente")
        Private m_RataConcorrente As New CCursorField(Of Decimal)("RataConcorrente")
        Private m_DurataConcorrente As New CCursorField(Of Integer)("DurataConcorrente")
        Private m_NettoRicavoConcorrente As New CCursorField(Of Decimal)("NettoRicavoConcorrente")
        Private m_TANConcorrente As New CCursorField(Of Double)("TANConcorrente")
        Private m_TAEGConcorrente As New CCursorField(Of Double)("TAEGConcorrente")
        Private m_IDOffertaIniziale As New CCursorField(Of Integer)("IDOffertaIniziale")
        Private m_InviatoA As New CCursorFieldObj(Of String)("InviatoA")
        Private m_InviatoACC As New CCursorFieldObj(Of String)("InviatoACC")
        Private m_MezzoDiInvio As New CCursorFieldObj(Of String)("MezzoDiInvio")
        Private m_SendSubject As New CCursorFieldObj(Of String)("SendSubject")
        Private m_SendMessange As New CCursorFieldObj(Of String)("SendMessange")
        Private m_SendDate As New CCursorField(Of Date)("SendDate")
        Private m_RicevutoIl As New CCursorField(Of Date)("RicevutoIl")
        Private m_RispostoIl As New CCursorField(Of Date)("RispostoIl")
        Private m_RispostoDa As New CCursorFieldObj(Of String)("RispostoDa")
        Private m_RispostoAMezzo As New CCursorFieldObj(Of String)("RispostoAMezzo")
        Private m_RispostoSubject As New CCursorFieldObj(Of String)("RispostoSubject")
        Private m_RispostoMessage As New CCursorFieldObj(Of String)("RispostoMessage")
        Private m_IDOffertaCorrente As New CCursorField(Of Integer)("IDOffertaCorrente")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDFinestraLavorazione As New CCursorField(Of Integer)("IDFinestraLavorazione")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property StatoRichiesta As CCursorField(Of StatoRichiestaDeroga)
            Get
                Return Me.m_StatoRichiesta
            End Get
        End Property


        Public ReadOnly Property DataRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
        End Property

        Public ReadOnly Property IDRichiedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiedente
            End Get
        End Property

        Public ReadOnly Property NomeRichiedente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRichiedente
            End Get
        End Property

        Public ReadOnly Property MotivoRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoRichiesta
            End Get
        End Property

        Public ReadOnly Property IDAgenziaConcorrente As CCursorField(Of Integer)
            Get
                Return Me.m_IDAgenziaConcorrente
            End Get
        End Property

        Public ReadOnly Property NomeAgenziaConcorrente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAgenziaConcorrente
            End Get
        End Property

        Public ReadOnly Property NomeProdottoConcorrente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProdottoConcorrente
            End Get
        End Property

        Public ReadOnly Property NumeroPreventivoConcorrente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroPreventivoConcorrente
            End Get
        End Property

        Public ReadOnly Property RataConcorrente As CCursorField(Of Decimal)
            Get
                Return Me.m_RataConcorrente
            End Get
        End Property

        Public ReadOnly Property DurataConcorrente As CCursorField(Of Integer)
            Get
                Return Me.m_DurataConcorrente
            End Get
        End Property

        Public ReadOnly Property NettoRicavoConcorrente As CCursorField(Of Decimal)
            Get
                Return Me.m_NettoRicavoConcorrente
            End Get
        End Property

        Public ReadOnly Property TANConcorrente As CCursorField(Of Double)
            Get
                Return Me.m_TANConcorrente
            End Get
        End Property

        Public ReadOnly Property TAEGConcorrente As CCursorField(Of Double)
            Get
                Return Me.m_TAEGConcorrente
            End Get
        End Property

        Public ReadOnly Property IDOffertaIniziale As CCursorField(Of Integer)
            Get
                Return Me.m_IDOffertaIniziale
            End Get
        End Property

        Public ReadOnly Property InviatoA As CCursorFieldObj(Of String)
            Get
                Return Me.m_InviatoA
            End Get
        End Property

        Public ReadOnly Property InviatoACC As CCursorFieldObj(Of String)
            Get
                Return Me.m_InviatoACC
            End Get
        End Property

        Public ReadOnly Property MezzoDiInvio As CCursorFieldObj(Of String)
            Get
                Return Me.m_MezzoDiInvio
            End Get
        End Property

        Public ReadOnly Property SendSubject As CCursorFieldObj(Of String)
            Get
                Return Me.m_SendSubject
            End Get
        End Property

        Public ReadOnly Property SendMessage As CCursorFieldObj(Of String)
            Get
                Return Me.m_SendMessange
            End Get
        End Property

        Public ReadOnly Property SendDate As CCursorField(Of Date)
            Get
                Return Me.m_SendDate
            End Get
        End Property

        Public ReadOnly Property RicevutoIl As CCursorField(Of Date)
            Get
                Return Me.m_RicevutoIl
            End Get
        End Property

        Public ReadOnly Property RispostoIl As CCursorField(Of Date)
            Get
                Return Me.m_RispostoIl
            End Get
        End Property

        Public ReadOnly Property RispostoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_RispostoDa
            End Get
        End Property

        Public ReadOnly Property RispostoAMezzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_RispostoAMezzo
            End Get
        End Property

        Public ReadOnly Property RispostoSubject As CCursorFieldObj(Of String)
            Get
                Return Me.m_RispostoSubject
            End Get
        End Property

        Public ReadOnly Property RispostoMessage As CCursorFieldObj(Of String)
            Get
                Return Me.m_RispostoMessage
            End Get
        End Property

        Public ReadOnly Property IDOffertaCorrente As CCursorField(Of Integer)
            Get
                Return Me.m_IDOffertaCorrente
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDFinestraLavorazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDFinestraLavorazione
            End Get
        End Property



        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.RichiesteDeroghe.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiesteDeroghe"
        End Function
    End Class


End Class
