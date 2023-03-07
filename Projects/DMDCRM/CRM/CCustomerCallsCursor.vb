Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    ''' <summary>
    ''' Cursore sulla tabella delle telefonate
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCustomerCallsCursor
        Inherits DBObjectCursorPO(Of CContattoUtente)

        Private m_IDAzienda As New CCursorField(Of Integer)("IDAzienda")
        Private m_ClassName As New CCursorFieldObj(Of String)("ClassName")  '[TEXT] Nome del tipo di contatto ricercato
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")  '[int] ID della persona associata
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona") '[Text] Nome della persona
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto") '[int]  ID del contesto
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto") '[Text] Tipo del contesto
        Private m_Ricevuta As New CCursorField(Of Boolean)("Ricevuta") '[Bool] Se vero indica che si tratta di una chiamata ricevuta. Se falso si tratta di una chiamata effettuata
        Private m_Scopo As New CCursorFieldObj(Of String)("Scopo") '[Text] Scopo della chiamata
        'Private m_Data As New CCursorField(Of Date)("Data") '[Date] Data e ora della chiamata
        Private m_Data As New CCursorFieldDBDate("DataStr") '[Date] Data e ora della chiamata
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore") '[Int]  ID dell'operatore che ha effettuato la chiamata o che ha risposto
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore") '[text] Nome dell'operatore
        Private m_Note As New CCursorFieldObj(Of String)("Note") '[text] Campo generico
        Private m_Esito As New CCursorField(Of EsitoChiamata)("Esito")
        Private m_DettaglioEsito As New CCursorFieldObj(Of String)("DettaglioEsito")
        Private m_MessageID As New CCursorFieldObj(Of String)("MessageID")
        Private m_NumeroOIndirizzo As New CCursorFieldObj(Of String)("Numero")
        Private m_NomeIndirizzo As New CCursorFieldObj(Of String)("Indirizzo_Nome")
        Private m_Durata As New CCursorField(Of Single)("Durata")
        Private m_Attesa As New CCursorField(Of Single)("Attesa")
        Private m_StatoConversazione As New CCursorField(Of StatoConversazione)("StatoConversazione")
        Private m_IDAccoltoDa As New CCursorField(Of Integer)("IDAccoltoDa")
        Private m_NomeAccoltoDa As New CCursorFieldObj(Of String)("NomeAccoltoDa")
        Private m_DataRicezione As New CCursorField(Of Date)("DataRicezione")
        Private m_AttachmentID As New CCursorField(Of Integer)("IDAttachment")
        Private m_IDPerContoDi As New CCursorField(Of Integer)("IDPerContoDi")
        Private m_NomePerContoDi As New CCursorFieldObj(Of String)("NomePerContoDi")
        Private m_Costo As New CCursorField(Of Decimal)("Costo")

        Public Sub New()
        
        End Sub

        Public ReadOnly Property Costo As CCursorField(Of Decimal)
            Get
                Return Me.m_Costo
            End Get
        End Property

        Public ReadOnly Property IDPerContoDi As CCursorField(Of Integer)
            Get
                Return Me.m_IDPerContoDi
            End Get
        End Property

        Public ReadOnly Property NomePerContoDi As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePerContoDi
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            If (dbRis Is Nothing) Then Return New CTelefonata
            Return Types.CreateInstance(Formats.ToString(dbRis.GetValue("ClassName", vbNullString)))
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Telefonate"
        End Function

        Public ReadOnly Property AttachmentID As CCursorField(Of Integer)
            Get
                Return Me.m_AttachmentID
            End Get
        End Property

        Public ReadOnly Property ClassName As CCursorFieldObj(Of String)
            Get
                Return Me.m_ClassName
            End Get
        End Property

        Public ReadOnly Property IDAccoltoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDAccoltoDa
            End Get
        End Property

        Public ReadOnly Property NomeAccoltoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAccoltoDa
            End Get
        End Property

        Public ReadOnly Property DataRicezione As CCursorField(Of Date)
            Get
                Return Me.m_DataRicezione
            End Get
        End Property

        Public ReadOnly Property StatoConversazione As CCursorField(Of StatoConversazione)
            Get
                Return Me.m_StatoConversazione
            End Get
        End Property


        Public ReadOnly Property IDAzienda As CCursorField(Of Integer)
            Get
                Return Me.m_IDAzienda
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

        Public ReadOnly Property IDContesto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContesto
            End Get
        End Property

        Public ReadOnly Property Contesto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContesto
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Single)
            Get
                Return Me.m_Durata
            End Get
        End Property

        Public ReadOnly Property Attesa As CCursorField(Of Single)
            Get
                Return Me.m_Attesa
            End Get
        End Property

        Public ReadOnly Property Ricevuta As CCursorField(Of Boolean)
            Get
                Return Me.m_Ricevuta
            End Get
        End Property

        Public ReadOnly Property Scopo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Scopo
            End Get
        End Property

        Public ReadOnly Property Data As CCursorFieldDBDate
            Get
                Return Me.m_Data
            End Get
        End Property

        'Public ReadOnly Property Data As CCursorField(Of Date)
        '    Get
        '        Return Me.m_Data
        '    End Get
        'End Property

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property Esito As CCursorField(Of EsitoChiamata)
            Get
                Return Me.m_Esito
            End Get
        End Property

        Public ReadOnly Property DettaglioEsito As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioEsito
            End Get
        End Property

        Public ReadOnly Property MessageID As CCursorFieldObj(Of String)
            Get
                Return Me.m_MessageID
            End Get
        End Property

        Public ReadOnly Property NumeroOIndirizzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroOIndirizzo
            End Get
        End Property

        Public ReadOnly Property NomeIndirizzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeIndirizzo
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return CustomerCalls.CRM.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.TelDB
        End Function

        'Protected Overrides Function GetWhereFields() As CKeyCollection(Of CCursorField)
        '    Dim ret As CKeyCollection(Of CCursorField) = MyBase.GetWhereFields()
        '    ret.RemoveByKey("Note")
        '    Return ret
        'End Function

        'Public Overrides Function GetWherePart() As String
        '    Dim ret As String = MyBase.GetWherePart
        '    If (ret = "(0<>0)") OrElse (ret = "0<>0") Then Return ret

        '    If (Me.m_Note.IsSet) Then
        '        Dim res As CCollection(Of Internals.CIndexingService.CResult) = CustomerCalls.CRM.Index.Find(Me.m_Note.Value, -1)
        '        Dim ids() As Integer = {}
        '        For Each r As Internals.CIndexingService.CResult In res
        '            ids = Arrays.InsertSorted(ids, r.OwnerID)
        '        Next
        '        If (ids.Length = 0) Then Return "(0<>0)"
        '        Dim buffer As New System.Text.StringBuilder
        '        For Each id As Integer In ids
        '            If (buffer.Length > 0) Then buffer.Append(",")
        '            buffer.Append(id)
        '        Next

        '        Return Strings.Combine(ret, "[ID] In (" & buffer.ToString & ")", " AND ")
        '    Else
        '        Return ret
        '    End If
        'End Function


    End Class



End Class