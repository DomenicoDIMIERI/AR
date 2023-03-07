Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    <Serializable>
    Public Class CEstinzioniCursor
        Inherits DBObjectCursorPO(Of CEstinzione)

        Private m_Tipo As New CCursorField(Of TipoEstinzione)("Tipo")  '[INT]      Un valore intero che indica la tipologia di estinzione
        Private m_IDIstituto As New CCursorField(Of Integer)("IDIstituto")   '[INT]      ID dell'istituto con cui il cliente ha stipulato il contratto da estinguere
        Private m_NomeIstituto As New CCursorFieldObj(Of String)("NomeIstituto") '[TEXT]     Nome dell'istituto
        Private m_NomeFiliale As New CCursorFieldObj(Of String)("NomeFiliale") '[TEXT]     Nome dell'istituto
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio") '[Date]     Data di inizio del prestito
        Private m_DataEstinzione As New CCursorField(Of Date)("DataEstinzione") '[Date]     Data di scadenza del prestito
        Private m_Scadenza As New CCursorField(Of Date)("Scadenza") '[Date]     Data di scadenza del prestito
        Private m_Rata As New CCursorField(Of Decimal)("Rata") '[Double]   
        Private m_Durata As New CCursorField(Of Integer)("Durata") '[INT]      Durata in numero di rate
        Private m_TAN As New CCursorField(Of Double)("TAN") '[Double]    
        Private m_TAEG As New CCursorField(Of Double)("TAEG") '[Double]    
        Private m_Estinta As New CCursorField(Of Boolean)("Estingue") '[Boolean]  Se vero indica che estingue questo prestito
        Private m_IDPratica As New CCursorField(Of Integer)("IDPratica")
        Private m_DecorrenzaPratica As New CCursorField(Of Date)("DecorrenzaPratica")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_IDEstintoDa As New CCursorField(Of Integer)("IDEstintoDa")
        Private m_DettaglioStato As New CCursorFieldObj(Of String)("DettaglioStato")
        Private m_SourceType As New CCursorFieldObj(Of String)("SourceType")
        Private m_SourceID As New CCursorField(Of Integer)("SourceID")
        Private m_Numero As New CCursorFieldObj(Of String)("Numero")
        Private m_NomeAgenzia As New CCursorFieldObj(Of String)("NomeAgenzia")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_DataRinnovo As New CCursorField(Of Date)("DataRinnovo")
        Private m_DataRicontatto As New CCursorField(Of Date)("DataRicontatto")
        Private m_Validato As New CCursorField(Of Boolean)("Validato")
        Private m_ValidatoIl As New CCursorField(Of Date)("ValidatoIl")
        Private m_IDValidatoDa As New CCursorField(Of Integer)("IDValidatoDa")
        Private m_NomeValidatoDa As New CCursorFieldObj(Of String)("NomeValidatoDa")
        Private m_NomeSorgenteValidazione As New CCursorFieldObj(Of String)("NomeSorgenteValidazione")
        Private m_TipoSorgenteValidazione As New CCursorFieldObj(Of String)("TipoSorgenteValidazione")
        Private m_IDSorgenteValidazione As New CCursorField(Of Integer)("IDSorgenteValidazione")
        Private m_IDClienteXCollaboratore As New CCursorField(Of Integer)("IDClienteXCollaboratore")

        Public Sub New()

        End Sub

        Public ReadOnly Property IDClienteXCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDClienteXCollaboratore
            End Get
        End Property

        Public ReadOnly Property Validato As CCursorField(Of Boolean)
            Get
                Return Me.m_Validato
            End Get
        End Property

        Public ReadOnly Property ValidatoIl As CCursorField(Of Date)
            Get
                Return Me.m_ValidatoIl
            End Get
        End Property

        Public ReadOnly Property IDValidatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDValidatoDa
            End Get
        End Property

        Public ReadOnly Property NomeValidatoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeValidatoDa
            End Get
        End Property

        Public ReadOnly Property NomeSorgenteValidazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeSorgenteValidazione
            End Get
        End Property

        Public ReadOnly Property TipoSorgenteValidazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoSorgenteValidazione
            End Get
        End Property

        Public ReadOnly Property IDSorgenteValidazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDSorgenteValidazione
            End Get
        End Property

        Public ReadOnly Property DataRinnovo As CCursorField(Of Date)
            Get
                Return Me.m_DataRinnovo
            End Get
        End Property

        Public ReadOnly Property DataRicontatto As CCursorField(Of Date)
            Get
                Return Me.m_DataRicontatto
            End Get
        End Property

        Public ReadOnly Property DataEstinzione As CCursorField(Of Date)
            Get
                Return Me.m_DataEstinzione
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property


        Public ReadOnly Property Numero As CCursorFieldObj(Of String)
            Get
                Return Me.m_Numero
            End Get
        End Property

        Public ReadOnly Property NomeAgenzia As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAgenzia
            End Get
        End Property

        Public ReadOnly Property SourceType As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceType
            End Get
        End Property

        Public ReadOnly Property SourceID As CCursorField(Of Integer)
            Get
                Return Me.m_SourceID
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property DettaglioStato As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato
            End Get
        End Property

        Public ReadOnly Property IDEstintoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDEstintoDa
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public ReadOnly Property DecorrenzaPratica As CCursorField(Of Date)
            Get
                Return Me.m_DecorrenzaPratica
            End Get
        End Property

        Public ReadOnly Property IDPratica As CCursorField(Of Integer)
            Get
                Return Me.m_IDPratica
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorField(Of TipoEstinzione)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property IDIstituto As CCursorField(Of Integer)
            Get
                Return Me.m_IDIstituto
            End Get
        End Property

        Public ReadOnly Property NomeIstituto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeIstituto
            End Get
        End Property

        Public ReadOnly Property NomeFiliale As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeFiliale
            End Get
        End Property


        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property Scadenza As CCursorField(Of Date)
            Get
                Return Me.m_Scadenza
            End Get
        End Property

        Public ReadOnly Property Rata As CCursorField(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Integer)
            Get
                Return Me.m_Durata
            End Get
        End Property

        Public ReadOnly Property TAN As CCursorField(Of Double)
            Get
                Return Me.m_TAN
            End Get
        End Property

        Public ReadOnly Property TAEG As CCursorField(Of Double)
            Get
                Return Me.m_TAEG
            End Get
        End Property

        Public ReadOnly Property Estinta As CCursorField(Of Boolean)
            Get
                Return Me.m_Estinta
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CEstinzione
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Estinzioni"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Estinzioni.Module
        End Function
    End Class


End Class