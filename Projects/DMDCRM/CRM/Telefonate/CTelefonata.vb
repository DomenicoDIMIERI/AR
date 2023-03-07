Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    <Serializable> _
    Public Class CTelefonata
        Inherits CContattoUtente


        Public Sub New()
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CustomerCalls.CRM.Module
        End Function


        Public Overrides ReadOnly Property DescrizioneAttivita As String
            Get
                Dim ret As String
                ret = "Telefonata " & CStr(IIf(Me.Ricevuta, "ricevuta da", "a")) & " " & Me.NomePersona
                If Me.NumeroOIndirizzo <> "" Then ret &= ", tel: " & Formats.FormatPhoneNumber(Me.NumeroOIndirizzo)
                Return ret
            End Get
        End Property

        Public Overrides ReadOnly Property AzioniProposte As CCollection(Of CAzioneProposta)
            Get
                Return New CCollection(Of CAzioneProposta)
            End Get
        End Property

        Public Property Numero As String
            Get
                Return Me.NumeroOIndirizzo
            End Get
            Set(value As String)
                Me.NumeroOIndirizzo = value
            End Set
        End Property

        Public Overrides Property NumeroOIndirizzo As String
            Get
                Return MyBase.NumeroOIndirizzo
            End Get
            Set(value As String)
                MyBase.NumeroOIndirizzo = Formats.ParsePhoneNumber(value)
            End Set
        End Property

        'Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
        '    MyBase.XMLSerialize(writer)
        '    'writer.WriteTag("Numero", Me.m_Numero)
        'End Sub

        'Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
        '    Select Case fieldName
        '        Case "Numero" : m_Numero = XML.Utils.Serializer.DeserializeString(fieldValue)
        '        Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        '    End Select
        'End Sub

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
            Return "Telefonata"
        End Function


        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            If (Me.Stato = ObjectStatus.OBJECT_VALID) AndAlso ((Me.StatoConversazione = CustomerCalls.StatoConversazione.INATTESA) OrElse (Me.StatoConversazione = CustomerCalls.StatoConversazione.INCORSO)) Then
                Telefonate.SetInAttesa(Me)
            Else
                Telefonate.SetFineAttesa(Me)
            End If
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            CustomerCalls.Telefonate.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            CustomerCalls.Telefonate.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CustomerCalls.Telefonate.doItemModified(New ItemEventArgs(Me))
        End Sub


    End Class



End Class