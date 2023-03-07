Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    <Serializable>
    Public Class CEMailMessage
        Inherits CContattoUtente

        Private m_Attachments As CCollection(Of CAttachment)

        Public Sub New()
            Me.m_Attachments = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CustomerCalls.CRM.Module
        End Function


        Public Overrides ReadOnly Property DescrizioneAttivita As String
            Get
                Dim ret As String
                ret = "eMail " & CStr(IIf(Me.Ricevuta, "ricevuta da", "a")) & " " & Me.NomePersona
                If Me.NumeroOIndirizzo <> "" Then ret &= ", tel: " & Formats.FormatPhoneNumber(Me.NumeroOIndirizzo)
                Return ret
            End Get
        End Property

        Public Overrides ReadOnly Property AzioniProposte As CCollection(Of CAzioneProposta)
            Get
                Return New CCollection(Of CAzioneProposta)
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_Telefonate"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            'Me.m_Numero = reader.Read("Numero", Me.m_Numero)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            ' writer.Write("Numero", Me.m_Numero)
            Return MyBase.SaveToRecordset(writer)
        End Function



        Public Overrides Function GetNomeTipoOggetto() As String
            Return "e-Mail"
        End Function


        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            If (Me.Stato = ObjectStatus.OBJECT_VALID) AndAlso ((Me.StatoConversazione = CustomerCalls.StatoConversazione.INATTESA) OrElse (Me.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO)) Then
                EMailMessages.SetInAttesa(Me)
            Else
                EMailMessages.SetFineAttesa(Me)
            End If
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            CustomerCalls.EMailMessages.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            CustomerCalls.EMailMessages.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CustomerCalls.EMailMessages.doItemModified(New ItemEventArgs(Me))
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo del mittente
        ''' </summary>
        ''' <returns></returns>
        Public Property From As String
            Get
                Return Me.Options.GetValueString("email_from", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.From
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.Options.SetValueString("email_from", value)
                Me.DoChanged("From", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco degli indirizzi dei destinatari separato dal ;
        ''' </summary>
        ''' <returns></returns>
        Public Property [To] As String
            Get
                Return Me.Options.GetValueString("email_to", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.To
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.Options.SetValueString("email_to", value)
                Me.DoChanged("To", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco degli indirizzi dei destinatari in copia carbone separato dal ;
        ''' </summary>
        ''' <returns></returns>
        Public Property CC As String
            Get
                Return Me.Options.GetValueString("email_cc", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.CC
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.Options.SetValueString("email_cc", value)
                Me.DoChanged("CC", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco degli indirizzi dei destinatari in copia carbone nascota separato dal ;
        ''' </summary>
        ''' <returns></returns>
        Public Property CCn As String
            Get
                Return Me.Options.GetValueString("email_ccn", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.CCn
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.Options.SetValueString("email_ccn", value)
                Me.DoChanged("CCn", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto della mail
        ''' </summary>
        ''' <returns></returns>
        Public Property Subject As String
            Get
                Return Me.Options.GetValueString("email_subject", "")
            End Get
            Set(value As String)
                Dim oldValue As String = Me.Subject
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.Options.SetValueString("email_subject", value)
                Me.DoChanged("Subject", value, oldValue)
            End Set
        End Property


        Public ReadOnly Property Attachments As CCollection(Of CAttachment)
            Get
                If (Me.m_Attachments Is Nothing) Then
                    Try
                        Me.m_Attachments = XML.Utils.Serializer.Deserialize(Me.Options.GetValueString("email_attachments"))
                    Catch ex As Exception
                        Me.m_Attachments = New CCollection(Of CAttachment)
                    End Try
                End If
                Return Me.m_Attachments
            End Get
        End Property

        Public Overrides Sub Save(Optional force As Boolean = False)
            If (Me.m_Attachments IsNot Nothing) Then
                Me.Options.SetValueString("email_attachments", XML.Utils.Serializer.Serialize(Me.m_Attachments))
            End If
            MyBase.Save(force)
        End Sub

    End Class



End Class