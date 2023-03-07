Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Rappresenta una visita effettuata o ricevuta
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CTelegramma
        Inherits CContattoUtente

        Private m_Luogo As New CIndirizzo

        Public Sub New()
        End Sub

        Public Overrides Function GetNomeTipoOggetto() As String
            Return "Telegramma"
        End Function

        Public Property Luogo As CIndirizzo
            Get
                Return Me.m_Luogo
            End Get
            Set(value As CIndirizzo)
                Dim oldValue As CIndirizzo = Me.m_Luogo
                If (oldValue = value) Then Exit Property

                If (value Is Nothing) Then
                    Me.m_Luogo = New CIndirizzo
                Else
                    Types.Copy(value, Me.m_Luogo)
                End If

                Me.DoChanged("Luogo", value, oldValue)
            End Set
        End Property

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() Or Me.m_Luogo.IsChanged
        End Function

        Public Overrides Function GetModule() As CModule
            Return Telefonate.Module
        End Function

        Public Overrides ReadOnly Property DescrizioneAttivita As String
            Get
                Dim ret As String
                ret = "Telegramma " & CStr(IIf(Me.Ricevuta, "ricevuto da", "inviato a ")) & " " & Me.NomePersona
                ret &= ", presso: "
                Select Case Me.Luogo.Nome
                    Case "Residenza", "Domicilio", "Sede di impiego" : ret &= " " & Me.Luogo.Nome
                    Case Else
                        If Left(Me.Luogo.Nome, Len("Ufficio di ")) = "Ufficio di " Then
                            ret &= " " & Me.Luogo.Nome
                        Else
                            ret &= Me.Luogo.ToString
                        End If
                End Select
                Return ret
            End Get
        End Property


        Public Overrides ReadOnly Property AzioniProposte As CCollection(Of CAzioneProposta)
            Get
                Return New CCollection(Of CAzioneProposta)
                'Dim azione As CAzioneProposta
                'ret = MyBase.AzioniProposte
                'If (Me.Ricontattare) And (Me.DataRicontatto <= Now()) Then
                '    azione = New CAzioneRicontatto
                '    Call azione.Initialize(Me)
                '    ret.Add(azione)
                'Else
                '    azione = New CAzioneChiama
                '    Call azione.Initialize(Me)
                '    ret.Add(azione)
                'End If
                'azione = New CAzioneCreaPratica
                'Call azione.Initialize(Me)
                'ret.Add(azione)
                'AzioniProposte = ret
            End Get
        End Property


        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Luogo", Me.Luogo)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Luogo" : Me.Luogo = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Telefonate"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Dim ret As Boolean = MyBase.LoadFromRecordset(reader)
            With Me.m_Luogo
                .Nome = Me.NomeIndirizzo
                .ToponimoViaECivico = Me.Options.GetValueString("Indirizzo_Via", "")
                .CAP = Me.Options.GetValueString("Indirizzo_CAP", "")
                .NomeComune = Me.Options.GetValueString("Indirizzo_Citta", "")
            End With
            Me.m_Luogo.SetChanged(False)
            Return ret
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            Me.NumeroOIndirizzo = Me.m_Luogo.ToString
            Me.NomeIndirizzo = Me.m_Luogo.Nome
            With Me.m_Luogo
                Me.Options.SetValueString("Indirizzo_Via", .ToponimoViaECivico)
                Me.Options.SetValueString("Indirizzo_CAP", .CAP)
                Me.Options.SetValueString("Indirizzo_Citta", .NomeComune)
            End With
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Me.m_Luogo.SetChanged(False)
            Return ret
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            If (Me.Stato = ObjectStatus.OBJECT_VALID) AndAlso ((Me.StatoConversazione = CustomerCalls.StatoConversazione.INATTESA) OrElse (Me.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO)) Then
                Telegrammi.SetInAttesa(Me)
            Else
                Telegrammi.SetFineAttesa(Me)
            End If
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Telegrammi.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Telegrammi.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            Telegrammi.doItemModified(New ItemEventArgs(Me))
        End Sub
    End Class




End Class

