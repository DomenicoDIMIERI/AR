Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office

   
    Public NotInheritable Class DocumentiCaricati
        Private Shared m_Module As CModule

        Private Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Shared ReadOnly Property [Module] As CModule
            Get
                Return Nothing
            End Get
        End Property

        Public Shared Function GetDocumentiCaricatiPerPersona(ByVal persona As CPersona) As CCollection(Of CDocCarPerPersona)
            Dim cursor As New CDocCarPerPersonaCursor
            Dim ret As New CCollection(Of CDocCarPerPersona)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPersona.Value = GetID(persona)
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Public Shared Function MatchDocumentiAttachmentsPersona(ByVal persona As CPersona) As CCollection(Of CDocCarPerPersona)
            Dim noncaricati As CCollection(Of CDocCarPerPersona) = GetDocumentiNonCaricatiPerPersona(persona)
            Dim attachments As New CAttachments(persona)
            Dim ret As CCollection(Of CDocCarPerPersona) = GetDocumentiCaricatiPerPersona(persona)
            For Each doc As CDocCarPerPersona In noncaricati
                For Each att As CAttachment In attachments
                    If (att.Tipo = doc.Documento.Nome) Then
                        doc.Attachment = att
                        Exit For
                    End If
                Next
                ret.Add(doc)
            Next
            Return ret
        End Function

        Public Shared Function GetDocumentiNonCaricatiPerPersona(ByVal persona As CPersona) As CCollection(Of CDocCarPerPersona)
            Dim caricati As CCollection(Of CDocCarPerPersona)
            Dim ret As New CCollection(Of CDocCarPerPersona)
            Dim cursor As New CDocumentiCursor
            Dim t As Boolean
            caricati = GetDocumentiCaricatiPerPersona(persona)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Nome.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                t = False
                For Each item As CDocCarPerPersona In caricati
                    t = item.IDDocumento = GetID(cursor.Item)
                    If (t) Then Exit For
                Next
                If (Not t) Then
                    Dim doc As New CDocCarPerPersona
                    doc.Documento = cursor.Item
                    doc.Persona = persona
                End If
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        '------------------------------
        ''' <summary>
        ''' Rappresenta un documento caricato per una persona
        ''' </summary>
        ''' <remarks></remarks>
        Public Class CDocCarPerPersona
            Inherits DBObject

            Private m_IDDocumento As Integer
            Private m_Documento As CDocumento
            Private m_IDPersona As Integer
            Private m_Persona As CPersona
            Private m_IDAttachment As Integer
            Private m_Attachment As CAttachment
            Private m_DataCaricamento As Date
            Private m_IDOperatoreCaricamento As Integer
            Private m_OperatoreCaricamento As CUser
            Private m_NomeOperatoreCaricamento As String
            Private m_Annotazioni As String
            Private m_DataRilascio As Nullable(Of Date)
            Private m_DataScadenza As Nullable(Of Date)
            Private m_IDRilasciatoDa As Integer
            Private m_RilasciatoDa As CAzienda
            Private m_NomeRilasciatoDa As String

            Public Sub New()
            End Sub

            Public Overrides Function GetModule() As CModule
                Return DocumentiCaricati.Module
            End Function

            ''' <summary>
            ''' Restituisce 
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property IDDocumento As Integer
                Get
                    Return GetID(Me.m_Documento, Me.m_IDDocumento)
                End Get
                Set(value As Integer)
                    Dim oldValue As Integer = Me.IDDocumento
                    If oldValue = value Then Exit Property
                    Me.m_IDDocumento = value
                    Me.m_Documento = Nothing
                    Me.DoChanged("IDDocumento", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce la descrizione del documento caricato
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Documento As CDocumento
                Get
                    If Me.m_Documento Is Nothing Then Me.m_Documento = GDE.GetItemById(Me.m_IDDocumento)
                    Return Me.m_Documento
                End Get
                Set(value As CDocumento)
                    Dim oldValue As CDocumento = Me.Documento
                    If (oldValue = value) Then Exit Property
                    Me.m_Documento = value
                    Me.m_IDDocumento = GetID(value)
                    Me.DoChanged("Documento", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta l'ID della persona a cui è associato il documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property IDPersona As Integer
                Get
                    Return GetID(Me.m_Persona, Me.m_IDPersona)
                End Get
                Set(value As Integer)
                    Dim oldValue As Integer = Me.IDPersona
                    If oldValue = value Then Exit Property
                    Me.m_IDPersona = value
                    Me.m_Persona = Nothing
                    Me.DoChanged("IDPersona", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta la persona a cui è associato il documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Persona As CPersona
                Get
                    If Me.m_Persona Is Nothing Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                    Return Me.m_Persona
                End Get
                Set(value As CPersona)
                    Dim oldValue As CPersona = Me.Persona
                    If (oldValue = value) Then Exit Property
                    Me.m_Persona = value
                    Me.m_IDPersona = GetID(value)
                    Me.DoChanged("Persona", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta la data di caricamento del documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property DataCaricamento As Date
                Get
                    Return Me.m_DataCaricamento
                End Get
                Set(value As Date)
                    Dim oldValue As Date = Me.m_DataCaricamento
                    If (oldValue = value) Then Exit Property
                    Me.m_DataCaricamento = value
                    Me.DoChanged("DataCaricamento", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta l'ID dell'operatore che ha caricato il documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property IDOperatoreCaricamento As Integer
                Get
                    Return GetID(Me.m_OperatoreCaricamento, Me.m_IDOperatoreCaricamento)
                End Get
                Set(value As Integer)
                    Dim oldValue As Integer = Me.IDOperatoreCaricamento
                    If oldValue = value Then Exit Property
                    Me.m_IDOperatoreCaricamento = value
                    Me.m_OperatoreCaricamento = Nothing
                    Me.DoChanged("IDOperatoreCaricamento", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce l'operatore che ha caricato il documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property OperatoreCaricamento As CUser
                Get
                    If Me.m_OperatoreCaricamento Is Nothing Then Me.m_OperatoreCaricamento = Sistema.Users.GetItemById(Me.m_IDOperatoreCaricamento)
                    Return Me.m_OperatoreCaricamento
                End Get
                Set(value As CUser)
                    Dim oldValue As CUser = Me.OperatoreCaricamento
                    If (oldValue = value) Then Exit Property
                    Me.m_OperatoreCaricamento = value
                    Me.m_IDOperatoreCaricamento = GetID(value)
                    If (value IsNot Nothing) Then Me.m_NomeOperatoreCaricamento = value.Nominativo
                    Me.DoChanged("OperatoreCaricamento", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta il nome dell'operatore che ha caricato il documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property NomeOperatoreCaricamento As String
                Get
                    Return Me.m_NomeOperatoreCaricamento
                End Get
                Set(value As String)
                    value = Trim(value)
                    Dim oldValue As String = Me.m_NomeOperatoreCaricamento
                    If (oldValue = value) Then Exit Property
                    Me.m_NomeOperatoreCaricamento = value
                    Me.DoChanged("NomeOperatoreCaricamento", value, oldValue)
                End Set
            End Property

            Public Property Annotazioni As String
                Get
                    Return Me.m_Annotazioni
                End Get
                Set(value As String)
                    Dim oldValue As String = Me.m_Annotazioni
                    If (oldValue = value) Then Exit Property
                    Me.m_Annotazioni = value
                    Me.DoChanged("Annotazioni", value, oldValue)
                End Set
            End Property

            Public Property DataRilascio As Nullable(Of Date)
                Get
                    Return Me.m_DataRilascio
                End Get
                Set(value As Nullable(Of Date))
                    Dim oldValue As Nullable(Of Date) = Me.m_DataRilascio
                    If (oldValue = value) Then Exit Property
                    Me.m_DataRilascio = value
                    Me.DoChanged("DataRilascio", value, oldValue)
                End Set
            End Property

            Public Property DataScadenza As Nullable(Of Date)
                Get
                    Return Me.m_DataScadenza
                End Get
                Set(value As Nullable(Of Date))
                    Dim oldValue As Nullable(Of Date) = Me.m_DataScadenza
                    If (oldValue = value) Then Exit Property
                    Me.m_DataScadenza = value
                    Me.DoChanged("DataScadenza", value, oldValue)
                End Set
            End Property

            Public Property IDRilasciatoDa As Integer
                Get
                    Return GetID(Me.m_RilasciatoDa, Me.m_IDRilasciatoDa)
                End Get
                Set(value As Integer)
                    Dim oldValue As Integer = Me.IDRilasciatoDa
                    If oldValue = value Then Exit Property
                    Me.m_IDRilasciatoDa = value
                    Me.m_RilasciatoDa = Nothing
                    Me.DoChanged("IDRilasciatoDa", value, oldValue)
                End Set
            End Property

            Public Property RilasciatoDa As CAzienda
                Get
                    If Me.m_RilasciatoDa Is Nothing Then Me.m_RilasciatoDa = Anagrafica.Aziende.GetItemById(Me.m_IDRilasciatoDa)
                    Return Me.m_RilasciatoDa
                End Get
                Set(value As CAzienda)
                    Dim oldValue As CAzienda = Me.RilasciatoDa
                    If (oldValue = value) Then Exit Property
                    Me.m_RilasciatoDa = value
                    Me.m_IDRilasciatoDa = GetID(value)
                    If (value IsNot Nothing) Then Me.m_NomeRilasciatoDa = value.Nominativo
                    Me.DoChanged("RilasciatoDa", value, oldValue)
                End Set
            End Property

            Public Property NomeRilasciatoDa As String
                Get
                    Return Me.m_NomeRilasciatoDa
                End Get
                Set(value As String)
                    value = Trim(value)
                    Dim oldValue As String = Me.m_NomeRilasciatoDa
                    If (oldValue = value) Then Exit Property
                    Me.m_NomeRilasciatoDa = value
                    Me.DoChanged("NomeRilasciatoDa", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta l'ID del file caricato sul server
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property IDAttachment As Integer
                Get
                    Return GetID(Me.m_Attachment, Me.m_IDAttachment)
                End Get
                Set(value As Integer)
                    Dim oldValue As Integer = Me.IDAttachment
                    If oldValue = value Then Exit Property
                    Me.m_IDAttachment = value
                    Me.m_Attachment = Nothing
                    Me.DoChanged("IDAttachment", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta l'ID dell'oggetto caricato sul server
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Attachment As CAttachment
                Get
                    Return Me.m_Attachment
                End Get
                Set(value As CAttachment)
                    Dim oldValue As CAttachment = Me.Attachment
                    If (oldValue = value) Then Exit Property
                    Me.m_Attachment = value
                    Me.m_IDAttachment = GetID(value)
                    Me.DoChanged("Attachment", value, oldValue)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Dim ret As New System.Text.StringBuilder(1024)
                If Me.Documento IsNot Nothing Then
                    ret.Append(Me.Documento.Nome)
                Else
                    ret.Append("ID Documento: " & Me.IDDocumento)
                End If
                If Me.Persona IsNot Nothing Then
                    ret.Append(", " & Me.Persona.Nominativo)
                Else
                    ret.Append(", ID persona: " & Me.IDPersona)
                End If
                ret.Append(", Rilasciato da: " & Me.NomeRilasciatoDa)
                If Not Types.IsNull(Me.DataRilascio) Then ret.Append(" il: " & Formats.FormatUserDate(Me.DataRilascio))
                Return ret.ToString
            End Function

            Public Overrides Function GetTableName() As String
                Return "tbl_DocCarPerPersona"
            End Function

            Protected Overrides Function GetConnection() As CDBConnection
                Return Office.Database
            End Function

            Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
                reader.Read("IDDocumento", Me.m_IDDocumento)
                reader.Read("IDPersona", Me.m_IDPersona)
                reader.Read("IDAttachment", Me.m_IDAttachment)
                reader.Read("DataCaricamento", Me.m_DataCaricamento)
                reader.Read("IDOperatoreCaricamento", Me.m_IDOperatoreCaricamento)
                reader.Read("NomeOperatoreCaricamento", Me.m_NomeOperatoreCaricamento)
                reader.Read("Annotazioni", Me.m_Annotazioni)
                reader.Read("DataRilascio", Me.m_DataRilascio)
                reader.Read("DataScadenza", Me.m_DataScadenza)
                reader.Read("IDRilasciatoDa", Me.m_IDRilasciatoDa)
                reader.Read("NomeRilasciatoDa", Me.m_NomeRilasciatoDa)
                Return MyBase.LoadFromRecordset(reader)
            End Function

            Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
                writer.Write("IDDocumento", Me.IDDocumento)
                writer.Write("IDPersona", Me.IDPersona)
                writer.Write("IDAttachment", Me.IDAttachment)
                writer.Write("DataCaricamento", Me.m_DataCaricamento)
                writer.Write("IDOperatoreCaricamento", Me.IDOperatoreCaricamento)
                writer.Write("NomeOperatoreCaricamento", Me.m_NomeOperatoreCaricamento)
                writer.Write("Annotazioni", Me.m_Annotazioni)
                writer.Write("DataRilascio", Me.m_DataRilascio)
                writer.Write("DataScadenza", Me.m_DataScadenza)
                writer.Write("IDRilasciatoDa", Me.IDRilasciatoDa)
                writer.Write("NomeRilasciatoDa", Me.m_NomeRilasciatoDa)
                Return MyBase.SaveToRecordset(writer)
            End Function

            Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
                writer.WriteAttribute("IDDocumento", Me.IDDocumento)
                writer.WriteAttribute("IDPersona", Me.IDPersona)
                writer.WriteAttribute("IDAttachment", Me.IDAttachment)
                writer.WriteAttribute("DataCaricamento", Me.m_DataCaricamento)
                writer.WriteAttribute("IDOperatoreCaricamento", Me.IDOperatoreCaricamento)
                writer.WriteAttribute("NomeOperatoreCaricamento", Me.m_NomeOperatoreCaricamento)
                writer.WriteAttribute("DataRilascio", Me.m_DataRilascio)
                writer.WriteAttribute("DataScadenza", Me.m_DataScadenza)
                writer.WriteAttribute("IDRilasciatoDa", Me.IDRilasciatoDa)
                writer.WriteAttribute("NomeRilasciatoDa", Me.m_NomeRilasciatoDa)
                MyBase.XMLSerialize(writer)
                writer.WriteTag("Annotazioni", Me.m_Annotazioni)
            End Sub

            Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
                Select Case fieldName
                    Case "IDDocumento" : Me.m_IDDocumento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    Case "IDAttachment" : Me.m_IDAttachment = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    Case "DataCaricamento" : Me.m_DataCaricamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                    Case "IDOperatoreCaricamento" : Me.m_IDOperatoreCaricamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    Case "NomeOperatoreCaricamento" : Me.m_NomeOperatoreCaricamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                    Case "Annotazioni" : Me.m_Annotazioni = XML.Utils.Serializer.DeserializeString(fieldValue)
                    Case "DataRilascio" : Me.m_DataRilascio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                    Case "DataScadenza", Me.m_DataScadenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                    Case "IDRilasciatoDa" : Me.m_IDRilasciatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    Case "NomeRilasciatoDa" : Me.m_NomeRilasciatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                    Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
                End Select
            End Sub

        End Class

        Public Class CDocCarPerPersonaCursor
            Inherits DBObjectCursor(Of CDocCarPerPersona)

            Private m_IDDocumento As New CCursorField(Of Integer)("IDDocumento")
            Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
            Private m_IDAttachment As New CCursorField(Of Integer)("IDAttachment")
            Private m_DataCaricamento As New CCursorField(Of Date)("DataCaricamento")
            Private m_IDOperatoreCaricamento As New CCursorField(Of Integer)("IDOperatoreCaricamento")
            Private m_NomeOperatoreCaricamento As New CCursorFieldObj(Of String)("NomeOperatoreCaricamento")
            Private m_Annotazioni As New CCursorFieldObj(Of String)("Annotazioni")
            Private m_DataRilascio As New CCursorField(Of Date)("DataRilascio")
            Private m_DataScadenza As New CCursorField(Of Date)("DataScadenza")
            Private m_IDRilasciatoDa As New CCursorField(Of Integer)("IDRilasciatoDa")
            Private m_NomeRilasciatoDa As New CCursorFieldObj(Of String)("NomeRilasciatoDa")

            Public Sub New()
            End Sub

            Public ReadOnly Property IDDocumento As CCursorField(Of Integer)
                Get
                    Return Me.m_IDDocumento
                End Get
            End Property

            Public ReadOnly Property IDPersona As CCursorField(Of Integer)
                Get
                    Return Me.m_IDPersona
                End Get
            End Property

            Public ReadOnly Property IDAttachment As CCursorField(Of Integer)
                Get
                    Return Me.m_IDAttachment
                End Get
            End Property

            Public ReadOnly Property DataCaricamento As CCursorField(Of Date)
                Get
                    Return Me.m_DataCaricamento
                End Get
            End Property

            Public ReadOnly Property IDOperatoreCaricamento As CCursorField(Of Integer)
                Get
                    Return Me.m_IDOperatoreCaricamento
                End Get
            End Property

            Public ReadOnly Property NomeOperatoreCaricamento As CCursorFieldObj(Of String)
                Get
                    Return Me.m_NomeOperatoreCaricamento
                End Get
            End Property

            Public ReadOnly Property Annotazioni As CCursorFieldObj(Of String)
                Get
                    Return Me.m_Annotazioni
                End Get
            End Property

            Public ReadOnly Property DataRilascio As CCursorField(Of Date)
                Get
                    Return Me.m_DataRilascio
                End Get
            End Property

            Public ReadOnly Property DataScadenza As CCursorField(Of Date)
                Get
                    Return Me.m_DataScadenza
                End Get
            End Property

            Public ReadOnly Property IDRilasciatoDa As CCursorField(Of Integer)
                Get
                    Return Me.m_IDRilasciatoDa
                End Get
            End Property

            Public ReadOnly Property NomeRilasciatoDa As CCursorFieldObj(Of String)
                Get
                    Return Me.m_NomeRilasciatoDa
                End Get
            End Property


            Protected Overrides Function GetConnection() As CDBConnection
                Return Office.Database
            End Function

            Protected Overrides Function GetModule() As CModule
                Return DocumentiCaricati.Module
            End Function

            Public Overrides Function GetTableName() As String
                Return "tbl_DocCarPerPersona"
            End Function
        End Class
    End Class


End Class