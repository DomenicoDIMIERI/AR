Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella delle spedizioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SpedizioniCursor
        Inherits DBObjectCursorPO(Of Spedizione)

        Private m_AspettoBeni As New CCursorFieldObj(Of String)("AspettoBeni")
        Private m_IDMittente As New CCursorField(Of Integer)("IDMittente")
        Private m_NomeMittente As New CCursorFieldObj(Of String)("NomeMittente")
        Private m_IndirizzoMittente_Nome As New CCursorFieldObj(Of String)("IndirizzoMittente_Nome")
        Private m_IndirizzoMittente_ToponimoViaECivico As New CCursorFieldObj(Of String)("IndirizzoMittente_Via")
        Private m_IndirizzoMittente_CAP As New CCursorFieldObj(Of String)("IndirizzoMittente_CAP")
        Private m_IndirizzoMittente_Citta As New CCursorFieldObj(Of String)("IndirizzoMittente_Citta")
        Private m_IndirizzoMittente_Provincia As New CCursorFieldObj(Of String)("IndirizzoMittente_Provincia")
                
        Private m_IDDestinatario As New CCursorField(Of Integer)("IDDestinatario")
        Private m_NomeDestinatario As New CCursorFieldObj(Of String)("NomeDestinatario")
        Private m_IndirizzoDestinatario_Nome As New CCursorFieldObj(Of String)("IndirizzoDest_Nome")
        Private m_IndirizzoDestinatario_ToponimoViaECivico As New CCursorFieldObj(Of String)("IndirizzoDest_Via")
        Private m_IndirizzoDestinatario_CAP As New CCursorFieldObj(Of String)("IndirizzoDest_CAP")
        Private m_IndirizzoDestinatario_Citta As New CCursorFieldObj(Of String)("IndirizzoDest_Citta")
        Private m_IndirizzoDestinatario_Provincia As New CCursorFieldObj(Of String)("IndirizzoDest_Provincia")
                
        Private m_NumeroColli As New CCursorField(Of Integer)("NumeroColli")
        Private m_Peso As New CCursorField(Of Double)("Peso")
        Private m_Altezza As New CCursorField(Of Double)("Altezza")
        Private m_Larghezza As New CCursorField(Of Double)("Larghezza")
        Private m_Profondita As New CCursorField(Of Double)("Profondita")

        Private m_IDSpeditoDa As New CCursorField(Of Integer)("IDSpeditoDa")
        Private m_NomeSpeditoDa As New CCursorFieldObj(Of String)("NomeSpeditoDa")

        Private m_IDRicevutoDa As New CCursorField(Of Integer)("IDRicevutoDa")
        Private m_NomeRicevutoDa As New CCursorFieldObj(Of String)("NomeRicevutoDa")

        Private m_DataInizioSpedizione As New CCursorField(Of Date)("DataInizioSpedizione")

        Private m_NotePerIlCorriere As New CCursorFieldObj(Of String)("NotePerIlCorriere")

        Private m_NotePerIlDestinatario As New CCursorFieldObj(Of String)("NotePerIlDestinatario")

        Private m_StatoSpedizione As New CCursorField(Of StatoSpedizione)("StatoSpedizione")
        Private m_StatoConsegna As New CCursorField(Of StatoConsegna)("StatoConsegna")

        Private m_DataConsegna As New CCursorField(Of Date)("DataConsegna")

        Private m_Flags As New CCursorField(Of SpedizioneFlags)("Flags")

        Private m_NomeCorriere As New CCursorFieldObj(Of String)("NomeCorriere")
        Private m_IDCorriere As New CCursorField(Of Integer)("IDCorriere")
        Private m_NumeroSpedizione As New CCursorFieldObj(Of String)("NumeroSpedizione")

        Public Sub New()
        End Sub

        Public ReadOnly Property AspettoBeni As CCursorFieldObj(Of String)
            Get
                Return Me.m_AspettoBeni
            End Get
        End Property

        Public ReadOnly Property IDMittente As CCursorField(Of Integer)
            Get
                Return Me.m_IDMittente
            End Get
        End Property

        Public ReadOnly Property NomeMittente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeMittente
            End Get
        End Property

        Public ReadOnly Property IndirizzoMittente_Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDestinatario_Nome
            End Get
        End Property

        Public ReadOnly Property IndirizzoMittente_ToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDestinatario_ToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property IndirizzoMittente_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoMittente_CAP
            End Get
        End Property

        Public ReadOnly Property IndirizzoMittente_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoMittente_Citta
            End Get
        End Property

        Public ReadOnly Property IndirizzoMittente_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoMittente_Provincia
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

        Public ReadOnly Property IndirizzoDestinatario_Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDestinatario_Nome
            End Get
        End Property

        Public ReadOnly Property IndirizzoDestinatario_ToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDestinatario_ToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property IndirizzoDestinatario_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDestinatario_CAP
            End Get
        End Property

        Public ReadOnly Property IndirizzoDestinatario_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDestinatario_Citta
            End Get
        End Property

        Public ReadOnly Property IndirizzoDestinatario_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDestinatario_Provincia
            End Get
        End Property

        Public ReadOnly Property NumeroColli As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroColli
            End Get
        End Property

        Public ReadOnly Property Peso As CCursorField(Of Double)
            Get
                Return Me.m_Peso
            End Get
        End Property

        Public ReadOnly Property Altezza As CCursorField(Of Double)
            Get
                Return Me.m_Altezza
            End Get
        End Property

        Public ReadOnly Property Larghezza As CCursorField(Of Double)
            Get
                Return Me.m_Larghezza
            End Get
        End Property

        Public ReadOnly Property Profondita As CCursorField(Of Double)
            Get
                Return Me.m_Profondita
            End Get
        End Property

        Public ReadOnly Property IDSpeditoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDSpeditoDa
            End Get
        End Property

        Public ReadOnly Property NomeSpeditoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeSpeditoDa
            End Get
        End Property

        Public ReadOnly Property IDRicevutoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDRicevutoDa
            End Get
        End Property

        Public ReadOnly Property NomeRicevutoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRicevutoDa
            End Get
        End Property

        Public ReadOnly Property DataInizioSpedizione As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioSpedizione
            End Get
        End Property

        Public ReadOnly Property NotePerIlCorriere As CCursorFieldObj(Of String)
            Get
                Return Me.m_NotePerIlCorriere
            End Get
        End Property

        Public ReadOnly Property NotePerIlDestinatario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NotePerIlDestinatario
            End Get
        End Property

        Public ReadOnly Property StatoSpedizione As CCursorField(Of StatoSpedizione)
            Get
                Return Me.m_StatoSpedizione
            End Get
        End Property

        Public ReadOnly Property StatoConsegna As CCursorField(Of StatoConsegna)
            Get
                Return Me.m_StatoConsegna
            End Get
        End Property

        Public ReadOnly Property DataConsegna As CCursorField(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of SpedizioneFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property NomeCorriere As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCorriere
            End Get
        End Property

        Public ReadOnly Property IDCorriere As CCursorField(Of Integer)
            Get
                Return Me.m_IDCorriere
            End Get
        End Property

        Public ReadOnly Property NumeroSpedizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroSpedizione
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.Spedizioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeSpedizioni"
        End Function



        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class