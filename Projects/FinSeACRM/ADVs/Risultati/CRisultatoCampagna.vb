Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV

    ''' <summary>
    ''' Valori che indicano lo stato di un messaggio inviato in una campagna pubblicitaria
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum StatoMessaggioCampagna As Integer
        ''' <summary>
        ''' Il messaggio non è ancora pronto per essere spedito
        ''' </summary>
        ''' <remarks></remarks>
        InPreparazione = 0

        ''' <summary>
        ''' Il messaggio è pronto per essere spedito
        ''' </summary>
        ''' <remarks></remarks>
        ProntoPerLaSpedizione = 1

        ''' <summary>
        ''' Il messaggio è stato inviato al destinatario
        ''' </summary>
        ''' <remarks></remarks>
        Inviato = 2

        ''' <summary>
        ''' Il messaggio è stato rifiutato dal vettore di spedizione
        ''' </summary>
        ''' <remarks></remarks>
        RifiutatoDalVettore = 3

        ''' <summary>
        ''' Il messaggio è stato rifiutato dal destinatario
        ''' </summary>
        ''' <remarks></remarks>
        RifiutatoDalDestinatario = 4

        ''' <summary>
        ''' Il messaggio è stato confermato 
        ''' </summary>
        ''' <remarks></remarks>
        Letto = 5
    End Enum

    <Serializable> _
    Public Class CRisultatoCampagna
        Inherits DBObject

        Private m_IDCampagna As Integer
        <NonSerialized> _
        Private m_Campagna As CCampagnaPubblicitaria
        Private m_NomeCampagna As String
        <NonSerialized> _
        Private m_Destinatario As CPersona
        Private m_IDDestinatario As Integer
        Private m_NomeDestinatario As String
        Private m_StatoMessaggio As StatoMessaggioCampagna
        Private m_DataSpedizione As Nullable(Of Date)
        Private m_NomeMezzoSpedizione As String
        Private m_StatoSpedizione As String
        Private m_DataConsegna As Nullable(Of Date)
        Private m_DataLettura As Nullable(Of Date)
        Private m_TipoCampagna As TipoCampagnaPubblicitaria
        Private m_IndirizzoDestinatario As String
        Private m_DataEsecuzuine As Nullable(Of Date)
        Private m_MessageID As String                   'ID del messaggio (usato dal sistema esterno)

        Public Sub New()
            Me.m_IDCampagna = 0
            Me.m_Campagna = Nothing
            Me.m_NomeCampagna = ""
            Me.m_Destinatario = Nothing
            Me.m_IDDestinatario = 0
            Me.m_NomeDestinatario = ""
            Me.m_StatoMessaggio = StatoMessaggioCampagna.InPreparazione
            Me.m_DataSpedizione = Nothing
            Me.m_NomeMezzoSpedizione = ""
            Me.m_StatoSpedizione = ""
            Me.m_DataConsegna = Nothing
            Me.m_DataLettura = Nothing
            Me.m_TipoCampagna = TipoCampagnaPubblicitaria.NonImpostato
            Me.m_IndirizzoDestinatario = ""
            Me.m_DataEsecuzuine = Nothing
            Me.m_MessageID = vbNullString
        End Sub

        ''' <summary>
        ''' Restituisce o imposta una stringa utilizzabile dal sistema di invio/ricezione per identificare univocamente il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MessageID As String
            Get
                Return Me.m_MessageID
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MessageID
                If (oldValue = value) Then Exit Property
                Me.m_MessageID = value
                Me.DoChanged("MessageID", value, oldValue)
            End Set
        End Property

        Public Property IDCampagna As Integer
            Get
                Return GetID(Me.m_Campagna, Me.m_IDCampagna)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCampagna
                If (oldValue = value) Then Exit Property
                Me.m_IDCampagna = value
                Me.m_Campagna = Nothing
                Me.DoChanged("IDCampagna", value, oldValue)
            End Set
        End Property

        Public Property Campagna As CCampagnaPubblicitaria
            Get
                If Me.m_Campagna Is Nothing Then Me.m_Campagna = ADV.Campagne.GetItemById(Me.m_IDCampagna)
                Return Me.m_Campagna
            End Get
            Set(value As CCampagnaPubblicitaria)
                Me.m_Campagna = value
                Me.m_IDCampagna = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCampagna = value.NomeCampagna
                Me.DoChanged("Campagna", value)
            End Set
        End Property

        Public Property NomeCampagna As String
            Get
                Return Me.m_NomeCampagna
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCampagna
                If (oldValue = value) Then Exit Property
                Me.m_NomeCampagna = value
                Me.DoChanged("NomeCampagna", value, oldValue)
            End Set
        End Property

        Public Property Destinatario As CPersona
            Get
                If Me.m_Destinatario Is Nothing Then Me.m_Destinatario = Anagrafica.Persone.GetItemById(Me.m_IDDestinatario)
                Return Me.m_Destinatario
            End Get
            Set(value As CPersona)
                Me.m_Destinatario = value
                Me.m_IDDestinatario = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeDestinatario = value.Nominativo
                Me.DoChanged("Destinatario", value)
            End Set
        End Property

        Public Property IDDestinatario As Integer
            Get
                Return GetID(Me.m_Destinatario, Me.m_IDDestinatario)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDestinatario
                If (oldValue = value) Then Exit Property
                Me.m_IDDestinatario = value
                Me.m_Destinatario = Nothing
                Me.DoChanged("IDDestinatario", value, oldValue)
            End Set
        End Property

        Public Property NomeDestinatario As String
            Get
                Return Me.m_NomeDestinatario
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeDestinatario
                If (oldValue = value) Then Exit Property
                Me.m_NomeDestinatario = value
                Me.DoChanged("NomeDestinatario", value, oldValue)
            End Set
        End Property

        Public Property StatoMessaggio As StatoMessaggioCampagna
            Get
                Return Me.m_StatoMessaggio
            End Get
            Set(value As StatoMessaggioCampagna)
                Dim oldValue As StatoMessaggioCampagna = Me.m_StatoMessaggio
                If (oldValue = value) Then Exit Property
                Me.m_StatoMessaggio = value
                Me.DoChanged("StatoMessaggio", value, oldValue)
            End Set
        End Property

        Public Property DataSpedizione As Nullable(Of Date)
            Get
                Return Me.m_DataSpedizione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_DataSpedizione = value
                Me.DoChanged("DataSpedizione", value, oldValue)
            End Set
        End Property

        Public Property NomeMezzoSpedizione As String
            Get
                Return Me.m_NomeMezzoSpedizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeMezzoSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_NomeMezzoSpedizione = value
                Me.DoChanged("NomeMezzoSpedizione", value, oldValue)
            End Set
        End Property

        Public Property StatoSpedizioneEx As String
            Get
                Return Me.m_StatoSpedizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_StatoSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_StatoSpedizione = value
                Me.DoChanged("StatoSpedizioneEx", value, oldValue)
            End Set
        End Property

        Public Property DataConsegna As Nullable(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataConsegna
                If (oldValue = value) Then Exit Property
                Me.m_DataConsegna = value
                Me.DoChanged("DataConsegna", value, oldValue)
            End Set
        End Property

        Public Property DataLettura As Nullable(Of Date)
            Get
                Return Me.m_DataLettura
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataLettura
                If (oldValue = value) Then Exit Property
                Me.m_DataLettura = value
                Me.DoChanged("DataLettura", value, oldValue)
            End Set
        End Property

        Public Property TipoCampagna As TipoCampagnaPubblicitaria
            Get
                Return Me.m_TipoCampagna
            End Get
            Set(value As TipoCampagnaPubblicitaria)
                Dim oldValue As TipoCampagnaPubblicitaria = Me.m_TipoCampagna
                If (oldValue = value) Then Exit Property
                Me.m_TipoCampagna = value
                Me.DoChanged("TipoCampagna", value, oldValue)
            End Set
        End Property

        Public Property IndirizzoDestinatario As String
            Get
                Return Me.m_IndirizzoDestinatario
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IndirizzoDestinatario
                If (oldValue = value) Then Exit Property
                Me.m_IndirizzoDestinatario = value
                Me.DoChanged("IndirizzoDestinatario", value, oldValue)
            End Set
        End Property

        Public Property DataEsecuzione As Nullable(Of Date)
            Get
                Return Me.m_DataEsecuzuine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataEsecuzuine
                If (oldValue = value) Then Exit Property
                Me.m_DataEsecuzuine = value
                Me.DoChanged("DataEsecuzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il testo generato dal modello
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ParseTemplate(ByVal value As String) As String
            Dim ret As String = value
            If (Me.Destinatario IsNot Nothing) Then
                If TypeOf (Me.Destinatario) Is CPersonaFisica Then
                    With DirectCast(Me.Destinatario, CPersonaFisica)
                        ret = Replace(ret, "%%NOME%%", .Nome)
                        ret = Replace(ret, "%%COGNOME%%", .Cognome)
                        If .ImpiegoPrincipale IsNot Nothing Then
                            ret = Replace(ret, "%%NOMEAZIENDA%%", .ImpiegoPrincipale.NomeAzienda)
                        Else
                            ret = Replace(ret, "%%NOMEAZIENDA%%", "")
                        End If
                    End With
                Else
                    ret = Replace(ret, "%%NOME%%", "")
                    ret = Replace(ret, "%%COGNOME%%", "")
                    ret = Replace(ret, "%%NOMEAZIENDA%%", "")
                End If
                ret = Replace(ret, "%%NOMINATIVO%%", Me.Destinatario.Nominativo)
                ret = Replace(ret, "%%DATANASCITA%%", Formats.FormatUserDate(Me.Destinatario.DataNascita))
                ret = Replace(ret, "%%DATAMORTE%%", Formats.FormatUserDate(Me.Destinatario.DataMorte))
                ret = Replace(ret, "%%TITOLO%%", Me.Destinatario.Titolo)
                ret = Replace(ret, "%%PROFESSIONE%%", Me.Destinatario.Professione)
                ret = Replace(ret, "%%CITTADINANZA%%", Me.Destinatario.Cittadinanza)
                ret = Replace(ret, "%%NATOA%%", Me.Destinatario.NatoA.ToString)
                ret = Replace(ret, "%%MORTOA%%", Me.Destinatario.MortoA.ToString)
                ret = Replace(ret, "%%RESIDENTEA%%", Me.Destinatario.ResidenteA.ToString)
                ret = Replace(ret, "%%DOMICILIATOA%%", Me.Destinatario.DomiciliatoA.ToString)

            Else
                ret = Replace(ret, "%%NOME%%", "")
                ret = Replace(ret, "%%COGNOME%%", "")
                ret = Replace(ret, "%%NOMINATIVO%%", Me.NomeDestinatario)
                ret = Replace(ret, "%%DATANASCITA%%", "")
                ret = Replace(ret, "%%DATAMORTE%%", "")
                ret = Replace(ret, "%%TITOLO%%", "")
                ret = Replace(ret, "%%PROFESSIONE%%", "")
                ret = Replace(ret, "%%CITTADINANZA%%", "")
                ret = Replace(ret, "%%NATOA%%", "")
                ret = Replace(ret, "%%MORTOA%%", "")
                ret = Replace(ret, "%%RESIDENTEA%%", "")
                ret = Replace(ret, "%%DOMICILIATOA%%", "")
            End If
            ret = Replace(ret, "%%DATAINVIO%%", Formats.FormatUserDateTimeOggi(Me.DataSpedizione))
            ret = Replace(ret, "%%DATAESECUZIONE%%", Formats.FormatUserDateTimeOggi(Me.DataEsecuzione))
            ret = Replace(ret, "%%DATACONSEGNA%%", Formats.FormatUserDateTimeOggi(Me.DataConsegna))
            ret = Replace(ret, "%%DATALETTURA%%", Formats.FormatUserDateTimeOggi(Me.DataLettura))
            ret = Replace(ret, "%%INDIRIZZOSPEDIZIONE%%", Me.IndirizzoDestinatario)
            ret = Replace(ret, "%%NOMECAMPAGNA%%", Me.NomeCampagna)
            ret = Replace(ret, "%%NOMEDESTINATARIO%%", Me.NomeDestinatario)
            ret = Replace(ret, "%%MEZZOSPEDIZIONE%%", Me.NomeMezzoSpedizione)
            ret = Replace(ret, "%%IDCAMPAGNA%%", Me.IDCampagna)
            Dim po As CUfficio = Nothing
            Dim nomePO As String = Anagrafica.Aziende.AziendaPrincipale.Nominativo
            Dim indirizzoPO As String = Anagrafica.Aziende.AziendaPrincipale.ResidenteA.ToString
            Dim telefonoPO As String = Formats.FormatPhoneNumber(Anagrafica.Aziende.AziendaPrincipale.Telefono)
            Dim faxPO As String = Formats.FormatPhoneNumber(Anagrafica.Aziende.AziendaPrincipale.Fax)
            Dim emailPO As String = Anagrafica.Aziende.AziendaPrincipale.eMail

            If (Me.Destinatario IsNot Nothing) Then po = Me.Destinatario.PuntoOperativo
            If (po IsNot Nothing) Then
                nomePO = po.Nome
                indirizzoPO = po.Indirizzo.ToString
                telefonoPO = Formats.FormatPhoneNumber(po.Telefono)
                faxPO = Formats.FormatPhoneNumber(po.Fax)
                emailPO = po.eMail
            End If

            ret = Replace(ret, "%%PUNTOOPERATIVO_NOME%%", nomePO)
            ret = Replace(ret, "%%PUNTOOPERATIVO_INDIRIZZO%%", indirizzoPO)
            ret = Replace(ret, "%%PUNTOOPERATIVO_TELEFONO%%", telefonoPO)
            ret = Replace(ret, "%%PUNTOOPERATIVO_FAX%%", faxPO)
            ret = Replace(ret, "%%PUNTOOPERATIVO_EMAIL%%", emailPO)

            Return ret
        End Function


        Public Sub Invia()
            Select Me.m_StatoMessaggio
                Case StatoMessaggioCampagna.ProntoPerLaSpedizione, _
                     StatoMessaggioCampagna.RifiutatoDalDestinatario, _
                     StatoMessaggioCampagna.RifiutatoDalVettore
                    Dim handler As HandlerTipoCampagna = ADV.Campagne.GetHandler(Me.TipoCampagna)
                    'If Me.Campagna.RichiediConfermaDiLettura AndAlso handler.SupportaConfermaLettura Then handler.RichiediConfermaLettura = True
                    'If Me.Campagna.RichiediConfermaDiRecapito AndAlso handler.SupportaConfermaRecapito Then handler.RichiediConfermaRecapito = True
                    Me.m_DataSpedizione = Now()
                    Me.m_StatoSpedizione = "Sto inviando"
                    Me.m_NomeMezzoSpedizione = handler.GetNomeMezzoSpedizione
                    Me.m_StatoMessaggio = StatoMessaggioCampagna.RifiutatoDalVettore
                    Try
                        If (handler.IsBanned(Me)) Then Throw New ArgumentException("Indirizzo bloccato nell'elenco principale")
                        If (handler.IsBlocked(Me)) Then Throw New ArgumentException("Indirizzo bloccato nel CRM")
                        handler.Send(Me)
                        'Me.m_StatoSpedizione = "Inviato"
                        'Me.m_StatoMessaggio = StatoMessaggioCampagna.Inviato
                    Catch ex As Exception
                        Me.m_StatoSpedizione = ex.Message
                        Sistema.Events.NotifyUnhandledException(ex)
                    End Try
                    Me.Save(True)
                Case Else
                    Throw New InvalidOperationException("Stato di esecuzione non valido (" & Me.m_StatoMessaggio & ")")
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return ADV.RisultatiCampagna.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ADVResults"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDCampagna = reader.Read("IDCampagna", Me.m_IDCampagna)
            Me.m_NomeCampagna = reader.Read("NomeCampagna", Me.m_NomeCampagna)
            Me.m_IDDestinatario = reader.Read("IDDestinatario", Me.m_IDDestinatario)
            Me.m_NomeDestinatario = reader.Read("NomeDestinatario", Me.m_NomeDestinatario)
            Me.m_IndirizzoDestinatario = reader.Read("IndirizzoDestinatario", Me.m_IndirizzoDestinatario)
            Me.m_StatoMessaggio = reader.Read("StatoMessaggio", Me.m_StatoMessaggio)
            Me.m_DataSpedizione = reader.Read("DataSpedizione", Me.m_DataSpedizione)
            Me.m_NomeMezzoSpedizione = reader.Read("NomeMezzoSpedizione", Me.m_NomeMezzoSpedizione)
            Me.m_StatoSpedizione = reader.Read("StatoSpedizione", Me.m_StatoSpedizione)
            Me.m_DataConsegna = reader.Read("DataConsegna", Me.m_DataConsegna)
            Me.m_DataLettura = reader.Read("DataLettura", Me.m_DataLettura)
            Me.m_TipoCampagna = reader.Read("TipoCampagna", Me.m_TipoCampagna)
            Me.m_DataEsecuzuine = reader.Read("DataEsecuzione", Me.m_DataEsecuzuine)
            Me.m_MessageID = reader.Read("MessageID", Me.m_MessageID)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDCampagna", Me.IDCampagna)
            writer.Write("NomeCampagna", Me.m_NomeCampagna)
            writer.Write("IDDestinatario", Me.IDDestinatario)
            writer.Write("NomeDestinatario", Me.m_NomeDestinatario)
            writer.Write("IndirizzoDestinatario", Me.m_IndirizzoDestinatario)
            writer.Write("StatoMessaggio", Me.m_StatoMessaggio)
            writer.Write("DataSpedizione", Me.m_DataSpedizione)
            writer.Write("NomeMezzoSpedizione", Me.m_NomeMezzoSpedizione)
            writer.Write("StatoSpedizione", Me.m_StatoSpedizione)
            writer.Write("DataConsegna", Me.m_DataConsegna)
            writer.Write("DataLettura", Me.m_DataLettura)
            writer.Write("TipoCampagna", Me.m_TipoCampagna)
            writer.Write("DataEsecuzione", Me.m_DataEsecuzuine)
            writer.Write("MessageID", Me.m_MessageID)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDCampagna", Me.IDCampagna)
            writer.WriteAttribute("NomeCampagna", Me.m_NomeCampagna)
            writer.WriteAttribute("IDDestinatario", Me.IDDestinatario)
            writer.WriteAttribute("NomeDestinatario", Me.m_NomeDestinatario)
            writer.WriteAttribute("IndirizzoDestinatario", Me.m_IndirizzoDestinatario)
            writer.WriteAttribute("StatoMessaggio", Me.m_StatoMessaggio)
            writer.WriteAttribute("DataSpedizione", Me.m_DataSpedizione)
            writer.WriteAttribute("NomeMezzoSpedizione", Me.m_NomeMezzoSpedizione)
            writer.WriteAttribute("StatoSpedizione", Me.m_StatoSpedizione)
            writer.WriteAttribute("DataConsegna", Me.m_DataConsegna)
            writer.WriteAttribute("DataLettura", Me.m_DataLettura)
            writer.WriteAttribute("TipoCampagna", Me.m_TipoCampagna)
            writer.WriteAttribute("DataEsecuzione", Me.m_DataEsecuzuine)
            writer.WriteAttribute("MessageID", Me.m_MessageID)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDCampagna" : Me.m_IDCampagna = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCampagna" : Me.m_NomeCampagna = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDDestinatario" : Me.IDDestinatario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeDestinatario" : Me.m_NomeDestinatario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IndirizzoDestinatario" : Me.m_IndirizzoDestinatario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoMessaggio" : Me.m_StatoMessaggio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataSpedizione" : Me.m_DataSpedizione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NomeMezzoSpedizione" : Me.m_NomeMezzoSpedizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoSpedizione" : Me.m_StatoSpedizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataConsegna" : Me.m_DataConsegna = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataLettura" : Me.m_DataLettura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoCampagna" : Me.m_TipoCampagna = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataEsecuzione" : Me.m_DataEsecuzuine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "MessageID" : Me.m_MessageID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        ''' <summary>
        ''' Aggiorna lo stato del messaggio
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Update()
            If (Me.m_MessageID = vbNullString) Then Return

            Dim handler As HandlerTipoCampagna = ADV.Campagne.GetHandler(Me.TipoCampagna)
            handler.UpdateStatus(Me)

            Me.Save()
        End Sub

 
    End Class


End Class