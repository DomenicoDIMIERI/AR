Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica



    <Serializable> _
    Public Class ConsensoInformato
        Inherits DBObjectBase

        Private m_IDPersona As Integer
        Private m_Persona As CPersona
        Private m_NomePersona As String
        Private m_DataConsenso As Nullable(Of Date)
        Private m_Consenso As Boolean
        Private m_Richiesto As Boolean
        Private m_NomeDocumento As String
        Private m_DescrizioneDocumento As String
        Private m_LinkDocumentoVisionato As String
        Private m_LinkDocumentoFirmato As String

        Public Sub New()
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_DataConsenso = Nothing
            Me.m_Consenso = False
            Me.m_Richiesto = False
            Me.m_NomeDocumento = ""
            Me.m_DescrizioneDocumento = ""
            Me.m_LinkDocumentoVisionato = ""
            Me.m_LinkDocumentoFirmato = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona che ha dato o rifiutato il consenso
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
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona che ha dato o rifiutato il consenso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona
                If (oldValue Is value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Friend Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            Me.m_IDPersona = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della persona che ha dato o rifiutato il consenso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersona
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta un valore booleano vero se la persona ha dato il consenso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Consenso As Boolean
            Get
                Return Me.m_Consenso
            End Get
            Set(value As Boolean)
                If (Me.m_Consenso = value) Then Exit Property
                Me.m_Consenso = value
                Me.DoChanged("Consenso", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica che il consenso è necessario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Richiesto As Boolean
            Get
                Return Me.m_Richiesto
            End Get
            Set(value As Boolean)
                If (Me.m_Richiesto = value) Then Exit Property
                Me.m_Richiesto = value
                Me.DoChanged("Richiesto", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data del consenso o della negazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataConsenso As Nullable(Of Date)
            Get
                Return Me.m_DataConsenso
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataConsenso
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataConsenso = value
                Me.DoChanged("DataConsenso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del doucmento che descrive l'informativa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeDocumento As String
            Get
                Return Me.m_NomeDocumento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeDocumento
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeDocumento = value
                Me.DoChanged("NomeDocumento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DescrizioneDocumento As String
            Get
                Return Me.m_DescrizioneDocumento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DescrizioneDocumento
                If (oldValue = value) Then Exit Property
                Me.m_DescrizioneDocumento = value
                Me.DoChanged("DescrizioneDocumento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il link del documento che ha visualizzato l'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LinkDocumentoVisionato As String
            Get
                Return Me.m_LinkDocumentoVisionato
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_LinkDocumentoVisionato
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_LinkDocumentoVisionato = value
                Me.DoChanged("LinkDocumentoVisionato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il link al documento firmato inviato dal cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LinkDocumentoFirmato As String
            Get
                Return Me.m_LinkDocumentoFirmato
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_LinkDocumentoFirmato
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_LinkDocumentoFirmato = value
                Me.DoChanged("LinkDocumentoFirmato", value, oldValue)
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.ConsensiInformati.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PersoneConsensi"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_DataConsenso = reader.Read("DataConsenso", Me.m_DataConsenso)
            Me.m_Consenso = reader.Read("Consenso", Me.m_Consenso)
            Me.m_Richiesto = reader.Read("Richiesto", Me.m_Richiesto)
            Me.m_NomeDocumento = reader.Read("NomeDocumento", Me.m_NomeDocumento)
            Me.m_DescrizioneDocumento = reader.Read("DescrizioneDocumento", Me.m_DescrizioneDocumento)
            Me.m_LinkDocumentoVisionato = reader.Read("LinkVisionato", Me.m_LinkDocumentoVisionato)
            Me.m_LinkDocumentoFirmato = reader.Read("LinkFirmato", Me.m_LinkDocumentoFirmato)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("DataConsenso", Me.m_DataConsenso)
            writer.Write("Consenso", Me.m_Consenso)
            writer.Write("Richiesto", Me.m_Richiesto)
            writer.Write("NomeDocumento", Me.m_NomeDocumento)
            writer.Write("DescrizioneDocumento", Me.m_DescrizioneDocumento)
            writer.Write("LinkVisionato", Me.m_LinkDocumentoVisionato)
            writer.Write("LinkFirmato", Me.m_LinkDocumentoFirmato)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("DataConsenso", Me.m_DataConsenso)
            writer.WriteAttribute("Consenso", Me.m_Consenso)
            writer.WriteAttribute("Richiesto", Me.m_Richiesto)
            writer.WriteAttribute("NomeDocumento", Me.m_NomeDocumento)
            writer.WriteAttribute("LinkVisionato", Me.m_LinkDocumentoVisionato)
            writer.WriteAttribute("LinkFirmato", Me.m_LinkDocumentoFirmato)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("DescrizioneDocumento", Me.m_DescrizioneDocumento)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataConsenso" : Me.m_DataConsenso = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Consenso" : Me.m_Consenso = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Richiesto" : Me.m_Richiesto = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "NomeDocumento" : Me.m_NomeDocumento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "LinkVisionato" : Me.m_LinkDocumentoVisionato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "LinkFirmato" : Me.m_LinkDocumentoFirmato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DescrizioneDocumento" : Me.m_DescrizioneDocumento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

    End Class




End Class