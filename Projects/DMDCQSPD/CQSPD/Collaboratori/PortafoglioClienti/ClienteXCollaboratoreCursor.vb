Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD



    <Serializable>
    Public Class ClienteXCollaboratoreCursor
        Inherits DBObjectCursor(Of ClienteXCollaboratore)

        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Cognome As New CCursorFieldObj(Of String)("Cognome")
        Private m_CodiceFiscale As New CCursorFieldObj(Of String)("CodiceFiscale")
        Private m_IDCollaboratore As New CCursorField(Of Integer)("IDCollaboratore")
        Private m_DataNascita As New CCursorField(Of DateTime)("DataNascita")
        Private m_Indirizzo_Provincia As New CCursorFieldObj(Of String)("Indirizzo_Provincia")
        Private m_Indirizzo_Citta As New CCursorFieldObj(Of String)("Indirizzo_Citta")
        Private m_Indirizzo_CAP As New CCursorFieldObj(Of String)("Indirizzo_CAP")
        Private m_Indirizzo_Via As New CCursorFieldObj(Of String)("Indirizzo_Via")
        Private m_DataAcquisizione As New CCursorField(Of DateTime)("DataAcquisizione")
        Private m_TipoFonte As New CCursorFieldObj(Of String)("TipoFonte")
        Private m_IDFonte As New CCursorField(Of Integer)("IDFonte")
        Private m_NomeFonte As New CCursorFieldObj(Of String)("NomeFonte")
        Private m_Flags As New CCursorField(Of ClienteCollaboratoreFlags)("Flags")
        Private m_StatoLavorazione As New CCursorField(Of StatoClienteCollaboratore)("StatoLavorazione")
        Private m_DettaglioStatoLavorazione As New CCursorFieldObj(Of String)("DettaglioStatoLavorazione")
        Private m_NomeAmministrazione As New CCursorFieldObj(Of String)("NomeAmministrazione")
        Private m_TelefonoCasa As New CCursorFieldObj(Of String)("TelefonoCasa")
        Private m_TelefonoUfficio As New CCursorFieldObj(Of String)("TelefonoUfficio")
        Private m_TelefonoCellulare As New CCursorFieldObj(Of String)("TelefonoCellulare")
        Private m_Fax As New CCursorFieldObj(Of String)("Fax")
        Private m_AltroTelefono As New CCursorFieldObj(Of String)("AltroTelefono")
        Private m_eMailPersonale As New CCursorFieldObj(Of String)("eMailPersonale")
        Private m_eMailUfficio As New CCursorFieldObj(Of String)("eMailUfficio")
        Private m_PEC As New CCursorFieldObj(Of String)("PEC")
        Private m_DataRinnovoCQS As New CCursorField(Of Date)("DataRinnovoCQS")
        Private m_MotivoRicontatto As New CCursorFieldObj(Of String)("MotivoRicontatto")
        Private m_DataRinnovoPD As New CCursorField(Of Date)("DataRinnovoPD")

        Private m_ImportoRichiesto As New CCursorFieldObj(Of String)("ImportoRichiesto")
        Private m_MotivoRichiesta As New CCursorFieldObj(Of String)("MotivoRichiesta")
        Private m_DataRichiesta As New CCursorField(Of Date)("DataRichiesta")
        Private m_IDConsulente As New CCursorField(Of Integer)("IDConsulente")
        Private m_DataAssegnazione As New CCursorField(Of Date)("DataAssegnazione")
        Private m_MotivoAssegnazione As New CCursorFieldObj(Of String)("MotivoAssegnazione")
        Private m_IDAssegnatoDa As New CCursorField(Of Integer)("IDAssegnatoDa")


        Public Sub New()

        End Sub

        Public ReadOnly Property IDConsulente As CCursorField(Of Integer)
            Get
                Return Me.m_IDConsulente
            End Get
        End Property

        Public ReadOnly Property DataAssegnazione As CCursorField(Of Date)
            Get
                Return Me.m_DataAssegnazione
            End Get
        End Property

        Public ReadOnly Property MotivoAssegnazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoAssegnazione
            End Get
        End Property

        Public ReadOnly Property IDAssegnatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDAssegnatoDa
            End Get
        End Property

        Public ReadOnly Property MotivoRicontatto As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoRicontatto
            End Get
        End Property

        Public ReadOnly Property ImportoRichiesto As CCursorFieldObj(Of String)
            Get
                Return Me.m_ImportoRichiesto
            End Get
        End Property

        Public ReadOnly Property MotivoRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoRichiesta
            End Get
        End Property

        Public ReadOnly Property DataRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
        End Property


        Public ReadOnly Property DataRinnovoCQS As CCursorField(Of Date)
            Get
                Return Me.m_DataRinnovoCQS
            End Get
        End Property

        Public ReadOnly Property DataRinnovoPD As CCursorField(Of Date)
            Get
                Return Me.m_DataRinnovoPD
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Cognome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Cognome
            End Get
        End Property

        Public ReadOnly Property CodiceFiscale As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceFiscale
            End Get
        End Property

        Public ReadOnly Property IDCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDCollaboratore
            End Get
        End Property

        Public ReadOnly Property DataNascita As CCursorField(Of DateTime)
            Get
                Return Me.m_DataNascita
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Provincia
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Citta
            End Get
        End Property

        Public ReadOnly Property Indirizzo_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_CAP
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Via
            End Get
        End Property

        Public ReadOnly Property DataAcquisizione As CCursorField(Of DateTime)
            Get
                Return Me.m_DataAcquisizione
            End Get
        End Property

        Public ReadOnly Property TipoFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonte
            End Get
        End Property

        Public ReadOnly Property IDFonte As CCursorField(Of Integer)
            Get
                Return Me.m_IDFonte
            End Get
        End Property

        Public ReadOnly Property NomeFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeFonte
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ClienteCollaboratoreFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property StatoLavorazione As CCursorField(Of StatoClienteCollaboratore)
            Get
                Return Me.m_StatoLavorazione
            End Get
        End Property

        Public ReadOnly Property DettaglioStatoLavorazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStatoLavorazione
            End Get
        End Property

        Public ReadOnly Property NomeAmministrazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAmministrazione
            End Get
        End Property

        Public ReadOnly Property TelefonoCasa As CCursorFieldObj(Of String)
            Get
                Return Me.m_TelefonoCasa
            End Get
        End Property

        Public ReadOnly Property TelefonoUfficio As CCursorFieldObj(Of String)
            Get
                Return Me.m_TelefonoUfficio
            End Get
        End Property

        Public ReadOnly Property TelefonoCellulare As CCursorFieldObj(Of String)
            Get
                Return Me.m_TelefonoCellulare
            End Get
        End Property

        Public ReadOnly Property Fax As CCursorFieldObj(Of String)
            Get
                Return Me.m_Fax
            End Get
        End Property

        Public ReadOnly Property AltroTelefono As CCursorFieldObj(Of String)
            Get
                Return Me.m_AltroTelefono
            End Get
        End Property

        Public ReadOnly Property eMailPersonale As CCursorFieldObj(Of String)
            Get
                Return Me.m_eMailPersonale
            End Get
        End Property

        Public ReadOnly Property eMailUfficio As CCursorFieldObj(Of String)
            Get
                Return Me.m_eMailUfficio
            End Get
        End Property

        Public ReadOnly Property PEC As CCursorFieldObj(Of String)
            Get
                Return Me.m_PEC
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.Collaboratori.ClientiXCollaboratori.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDCliXCollab"
        End Function




    End Class

End Class