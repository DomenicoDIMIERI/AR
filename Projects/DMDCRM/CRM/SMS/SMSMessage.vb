Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    ''' <summary>
    ''' Rappresenta un messaggio SMS
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class SMSMessage
        Inherits CContattoUtente

        Public Sub New()
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CustomerCalls.CRM.Module
        End Function

        'Public Property Numero As String
        '    Get
        '        Return Me.m_Numero
        '    End Get
        '    Set(value As String)
        '        value = Formats.ParsePhoneNumber(value)
        '        Dim oldValue As String = Me.m_Numero
        '        If (oldValue = value) Then Exit Property
        '        Me.m_Numero = value
        '        Me.DoChanged("Numero", value, oldValue)
        '    End Set
        'End Property

        Public Overrides ReadOnly Property DescrizioneAttivita As String
            Get
                Dim ret As String
                ret = "SMS " & CStr(IIf(Me.Ricevuta, "ricevuto da", "inviato a")) & " " & Me.NomePersona
                If Me.NumeroOIndirizzo <> "" Then ret &= ", tel: " & Formats.FormatPhoneNumber(Me.NumeroOIndirizzo)
                Return ret
            End Get
        End Property

        Public Overrides ReadOnly Property AzioniProposte As CCollection(Of CAzioneProposta)
            Get
                Return New CCollection(Of CAzioneProposta)
            End Get
        End Property


        'Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
        '    MyBase.XMLSerialize(writer)
        '    writer.WriteTag("Numero", Me.m_Numero)
        'End Sub

        'Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
        '    Select Case fieldName
        '        Case "Numero" : Me.m_Numero = XML.Utils.Serializer.DeserializeString(fieldValue)
        '        Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        '    End Select
        'End Sub

        'Public Overrides Function GetTableName() As String
        '    Return "tbl_Telefonate"
        'End Function

        'Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
        '    reader.Read("Numero", Me.m_Numero)
        '    Return MyBase.LoadFromRecordset(reader)
        'End Function

        'Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
        '    writer.Write("Numero", Me.m_Numero)
        '    Return MyBase.SaveToRecordset(writer)
        'End Function

        Public Overrides Function GetNomeTipoOggetto() As String
            Return "SMS"
        End Function

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

    End Class



End Class