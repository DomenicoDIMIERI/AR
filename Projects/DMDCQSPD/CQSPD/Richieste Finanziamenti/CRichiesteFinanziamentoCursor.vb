Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Class CRichiesteFinanziamentoCursor
        Inherits DBObjectCursorPO(Of CRichiestaFinanziamento)

        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_IDFonte As New CCursorField(Of Integer)("IDFonte")
        Private m_NomeFonte As New CCursorFieldObj(Of String)("NomeFonte")
        Private m_IDCanale As New CCursorField(Of Integer)("IDCanale")
        Private m_NomeCanale As New CCursorFieldObj(Of String)("NomeCanale")
        Private m_IDCanale1 As New CCursorField(Of Integer)("IDCanale1")
        Private m_NomeCanale1 As New CCursorFieldObj(Of String)("NomeCanale1")
        Private m_Referrer As New CCursorFieldObj(Of String)("Referrer")
        Private m_ImportoRichiesto As New CCursorField(Of Decimal)("ImportoRichiesto")
        Private m_RataMassima As New CCursorField(Of Decimal)("RataMassima")
        Private m_DurataMassima As New CCursorField(Of Integer)("DurataMassima")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_IDAssegnatoA As New CCursorField(Of Integer)("IDAssegnatoA")
        Private m_NomeAssegnatoA As New CCursorFieldObj(Of String)("NomeAssegnatoA")
        Private m_IDPresaInCaricoDa As New CCursorField(Of Integer)("IDPresaInCaricoDa")
        Private m_NomePresaInCarocoDa As New CCursorFieldObj(Of String)("NomePresaInCaricoDa")
        Private m_IDFonteStr As New CCursorFieldObj(Of String)("IDFonteStr")
        Private m_IDCampagnaStr As New CCursorFieldObj(Of String)("IDCampagnaStr")
        Private m_IDAnnuncioStr As New CCursorFieldObj(Of String)("IDAnnuncioStr")
        Private m_IDKeyWordStr As New CCursorFieldObj(Of String)("IDKeyWordStr")
        Private m_TipoFonte As New CCursorFieldObj(Of String)("TipoFonte")
        Private m_StatoRichiesta As New CCursorField(Of StatoRichiestaFinanziamento)("StatoRichiesta")
        Private m_TipoRichiesta As New CCursorField(Of TipoRichiestaFinanziamento)("TipoRichiesta")
        Private m_ImportoRichiesto1 As New CCursorField(Of Decimal)("ImportoRichiesto1")
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")
        Private m_Durata As New CCursorField(Of Double)("Durata")
        Private m_Flags As New CCursorField(Of RichiestaFinanziamentoFlags)("Flags")
        Private m_IDFinestraLavorazione As New CCursorField(Of Integer)("IDFinestraLavorazione")
        Private m_Scopo As New CCursorFieldObj(Of String)("Scopo")
        Private m_IDCollaboratore As New CCursorField(Of Integer)("IDCollaboratore")
        Private m_NomeCollaboratore As New CCursorFieldObj(Of String)("NomeCollaboratore")


        Public Sub New()
        End Sub

        Public ReadOnly Property IDCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDCollaboratore
            End Get
        End Property

        Public ReadOnly Property NomeCollaboratore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCollaboratore
            End Get
        End Property

        Public ReadOnly Property Scopo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Scopo
            End Get
        End Property

        Public ReadOnly Property IDFinestraLavorazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDFinestraLavorazione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of RichiestaFinanziamentoFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Double)
            Get
                Return Me.m_Durata
            End Get
        End Property

        Public ReadOnly Property IDContesto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContesto
            End Get
        End Property

        Public ReadOnly Property TipoContesto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContesto
            End Get
        End Property


        Public ReadOnly Property TipoRichiesta As CCursorField(Of TipoRichiestaFinanziamento)
            Get
                Return Me.m_TipoRichiesta
            End Get
        End Property

        Public ReadOnly Property StatoRichiesta As CCursorField(Of StatoRichiestaFinanziamento)
            Get
                Return Me.m_StatoRichiesta
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
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

        Public ReadOnly Property IDCanale As CCursorField(Of Integer)
            Get
                Return Me.m_IDCanale
            End Get
        End Property

        Public ReadOnly Property IDCanale1 As CCursorField(Of Integer)
            Get
                Return Me.m_IDCanale1
            End Get
        End Property

        Public ReadOnly Property NomeCanale As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCanale
            End Get
        End Property

        Public ReadOnly Property NomeCanale1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCanale
            End Get
        End Property

        Public ReadOnly Property Referrer As CCursorFieldObj(Of String)
            Get
                Return Me.m_Referrer
            End Get
        End Property

        Public ReadOnly Property ImportoRichiesto As CCursorField(Of Decimal)
            Get
                Return Me.m_ImportoRichiesto
            End Get
        End Property

        Public ReadOnly Property ImportoRichiesto1 As CCursorField(Of Decimal)
            Get
                Return Me.m_ImportoRichiesto1
            End Get
        End Property

        Public ReadOnly Property RataMassima As CCursorField(Of Decimal)
            Get
                Return Me.m_RataMassima
            End Get
        End Property

        Public ReadOnly Property DurataMassima As CCursorField(Of Integer)
            Get
                Return Me.m_DurataMassima
            End Get
        End Property

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

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property IDAssegnatoA As CCursorField(Of Integer)
            Get
                Return Me.m_IDAssegnatoA
            End Get
        End Property

        Public ReadOnly Property NomeAssegnatoA As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAssegnatoA
            End Get
        End Property

        Public ReadOnly Property IDPresaInCaricoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDPresaInCaricoDa
            End Get
        End Property

        Public ReadOnly Property NomePresaInCarocoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePresaInCarocoDa
            End Get
        End Property

        Public ReadOnly Property IDFonteStr As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDFonteStr
            End Get
        End Property

        Public ReadOnly Property IDCampagnaStr As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDCampagnaStr
            End Get
        End Property

        Public ReadOnly Property IDAnnuncioStr As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDAnnuncioStr
            End Get
        End Property

        Public ReadOnly Property IDKeyWordStr As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDKeyWordStr
            End Get
        End Property

        Public ReadOnly Property TipoFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonte
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return RichiesteFinanziamento.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiesteFinanziamenti"
        End Function
    End Class


End Class
