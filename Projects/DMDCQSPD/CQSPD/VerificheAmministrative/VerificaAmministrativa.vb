Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    Public Enum StatoVerificaAmministrativa As Integer
        DaVerificare = 0
        ValutazioneInCorso = 1
        ValutazioneConclusa = 2
        Rimandata = 3
    End Enum

    Public Enum EsitoVerificaAmministrativa As Integer
        Sconosciuto = 0
        Confermato = 1
        NonConfermato = 2
    End Enum
     

    <Serializable> _
    Public Class VerificaAmministrativa
        Inherits DBObject

        Private m_IDOperatore As Integer
        Private m_Operatore As CUser
        Private m_NomeOperatore As String
        Private m_StatoVerifica As StatoVerificaAmministrativa
        Private m_EsitoVerifica As EsitoVerificaAmministrativa
        Private m_DettaglioEsitoVerifica As String
        Private m_DataInizio As Date?
        Private m_DataFine As Date?
        Private m_Flags As Integer
        Private m_IDOggettoVerificato As Integer
        Private m_TipoOggettoVerificato As String
        Private m_OggettoVerificato As Object

        Public Sub New()
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""
            Me.m_StatoVerifica = StatoVerificaAmministrativa.DaVerificare
            Me.m_EsitoVerifica = EsitoVerificaAmministrativa.Sconosciuto
            Me.m_DettaglioEsitoVerifica = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Flags = Nothing
            Me.m_IDOggettoVerificato = 0
            Me.m_TipoOggettoVerificato = ""
            Me.m_OggettoVerificato = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha effettuato la verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha effettuato la verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue Is value) Then Exit Property
                Me.m_IDOperatore = GetID(value)
                Me.m_Operatore = value
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoVerifica As StatoVerificaAmministrativa
            Get
                Return Me.m_StatoVerifica
            End Get
            Set(value As StatoVerificaAmministrativa)
                Dim oldValue As StatoVerificaAmministrativa = Me.m_StatoVerifica
                If (oldValue = value) Then Exit Property
                Me.m_StatoVerifica = value
                Me.DoChanged("StatoVerifica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'esito della verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EsitoVerifica As EsitoVerificaAmministrativa
            Get
                Return Me.m_EsitoVerifica
            End Get
            Set(value As EsitoVerificaAmministrativa)
                Dim oldValue As EsitoVerificaAmministrativa = Me.m_EsitoVerifica
                If (oldValue = value) Then Exit Property
                Me.m_EsitoVerifica = value
                Me.DoChanged("EsitoVerifica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il dettaglio dell'esito della verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioEsitoVerifica As String
            Get
                Return Me.m_DettaglioEsitoVerifica
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioEsitoVerifica
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioEsitoVerifica = value
                Me.DoChanged("DettaglioEsitoVerifica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di inizio della verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Date?
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizio
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di conclusione della verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Date?
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFine
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'oggetto verificato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOggettoVerificato As Integer
            Get
                Return GetID(Me.m_OggettoVerificato, Me.m_IDOggettoVerificato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOggettoVerificato
                If (oldValue = value) Then Exit Property
                Me.m_IDOggettoVerificato = value
                Me.m_OggettoVerificato = Nothing
                Me.DoChanged("IDOggettoVerificato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'oggetto verificato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoOggettoVerificato As String
            Get
                Return Me.m_TipoOggettoVerificato
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoOggettoVerificato
                If (oldValue = value) Then Exit Property
                Me.m_TipoOggettoVerificato = value
                Me.m_OggettoVerificato = Nothing
                Me.DoChanged("TipoOggettoVerificato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto verificato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OggettoVerificato As Object
            Get
                If (Me.m_OggettoVerificato Is Nothing) Then Me.m_OggettoVerificato = Sistema.Types.GetItemByTypeAndId(Me.m_TipoOggettoVerificato, Me.m_IDOggettoVerificato)
                Return Me.m_OggettoVerificato
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.OggettoVerificato
                If (oldValue Is value) Then Exit Property
                Me.m_IDOggettoVerificato = GetID(value)
                If (value Is Nothing) Then
                    Me.m_TipoOggettoVerificato = ""
                Else
                    Me.m_TipoOggettoVerificato = TypeName(value)
                End If
                Me.DoChanged("OggettoVerificato", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetOggettoVerificato(ByVal value As Object)
            Me.m_OggettoVerificato = value
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.VerificheAmministrative.Module
        End Function
      

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDVerificheAmministrative"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_StatoVerifica = reader.Read("StatoVerifica", Me.m_StatoVerifica)
            Me.m_EsitoVerifica = reader.Read("EsitoVerifica", Me.m_EsitoVerifica)
            Me.m_DettaglioEsitoVerifica = reader.Read("DettaglioEsitoVerifica", Me.m_DettaglioEsitoVerifica)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDOggettoVerificato = reader.Read("IDOggettoVerificato", Me.m_IDOggettoVerificato)
            Me.m_TipoOggettoVerificato = reader.Read("TipoOggettoVerificato", Me.m_TipoOggettoVerificato)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("StatoVerifica", Me.m_StatoVerifica)
            writer.Write("EsitoVerifica", Me.m_EsitoVerifica)
            writer.Write("DettaglioEsitoVerifica", Me.m_DettaglioEsitoVerifica)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDOggettoVerificato", Me.IDOggettoVerificato)
            writer.Write("TipoOggettoVerificato", Me.m_TipoOggettoVerificato)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("StatoVerifica", Me.m_StatoVerifica)
            writer.WriteAttribute("EsitoVerifica", Me.m_EsitoVerifica)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDOggettoVerificato", Me.IDOggettoVerificato)
            writer.WriteAttribute("TipoOggettoVerificato", Me.m_TipoOggettoVerificato)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("DettaglioEsitoVerifica", Me.m_DettaglioEsitoVerifica)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoVerifica" : Me.m_StatoVerifica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "EsitoVerifica" : Me.m_EsitoVerifica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioEsitoVerifica" : Me.m_DettaglioEsitoVerifica = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOggettoVerificato" : Me.m_IDOggettoVerificato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoOggettoVerificato" : Me.m_TipoOggettoVerificato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class
