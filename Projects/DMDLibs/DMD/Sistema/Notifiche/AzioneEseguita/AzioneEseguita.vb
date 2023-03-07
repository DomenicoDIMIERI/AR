Imports DMD.Databases

Partial Public Class Sistema



    ''' <summary>
    ''' Rappresenta il risultato di un'azione eseguita su una notifica
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class AzioneEseguita
        Inherits DBObject

        <NonSerialized> _
        Private m_Notifica As Notifica          'Notifica su cui è stata eseguita l'azione
        Private m_IDNotifica As Integer         'ID della notifica su cui è stata eseguita l'azione
        <NonSerialized> _
        Private m_Azione As AzioneEseguibile      'Risultato dell'operazione
        Private m_AzioneType As String          'Tipo dell'azione
        Private m_DataEsecuzione As Date        'Data di esecuzione
        Private m_Parameters As String          'Parametri di esecuzione (in form
        Private m_Results As String

        Public Sub New()
            Me.m_Notifica = Nothing
            Me.m_IDNotifica = 0
            Me.m_Azione = Nothing
            Me.m_AzioneType = vbNullString
            Me.m_DataEsecuzione = Nothing
            Me.m_Parameters = vbNullString
            Me.m_Results = vbNullString
        End Sub

        ''' <summary>
        ''' Restituisce l'ID della notifica associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDNotifica As Integer
            Get
                Return GetID(Me.m_Notifica, Me.m_IDNotifica)
            End Get
            Friend Set(value As Integer)
                Dim oldValue As Integer = Me.IDNotifica
                If (oldValue = value) Then Exit Property
                Me.m_IDNotifica = value
                Me.m_Notifica = Nothing
                Me.DoChanged("IDNotifica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la notifica associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Notifica As Notifica
            Get
                If (Me.m_Notifica Is Nothing) Then Me.m_Notifica = Notifiche.GetItemById(Me.m_IDNotifica)
                Return Me.m_Notifica
            End Get
            Friend Set(value As Notifica)
                Dim oldValue As Notifica = Me.m_Notifica
                If (oldValue Is value) Then Exit Property
                Me.m_Notifica = value
                Me.m_IDNotifica = GetID(value)
                Me.DoChanged("Notifica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'azione eseguita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Azione As AzioneEseguibile
            Get
                If (Me.m_Azione Is Nothing) AndAlso (Me.m_AzioneType <> vbNullString) Then Me.m_Azione = Sistema.Types.CreateInstance(Me.m_AzioneType)
                Return Me.m_Azione
            End Get
            Friend Set(value As AzioneEseguibile)
                Dim oldValue As AzioneEseguibile = Me.Azione
                If (TypeName(value) = TypeName(oldValue)) Then Exit Property
                Me.m_Azione = value
                Me.m_AzioneType = TypeName(value)
                Me.DoChanged("Azione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di esecuzione dell'handler
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEsecuzione As Date
            Get
                Return Me.m_DataEsecuzione
            End Get
            Friend Set(value As Date)
                Dim oldValue As Date = Me.m_DataEsecuzione
                If (oldValue = value) Then Exit Property
                Me.m_DataEsecuzione = value
                Me.DoChanged("DataEsecuzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce una stringa descrittiva dei parametri di esecuzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Parameters As String
            Get
                Return Me.m_Parameters
            End Get
            Friend Set(value As String)
                Dim oldValue As String = Me.m_Parameters
                If (oldValue = value) Then Exit Property
                Me.m_Parameters = value
                Me.DoChanged("Parameters", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa descrittiva dei risultati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Results As String
            Get
                Return Me.m_Results
            End Get
            Friend Set(value As String)
                'value = Trim(value)
                Dim oldValue As String = Me.m_Results
                If (oldValue = value) Then Exit Property
                Me.m_Results = value
                Me.DoChanged("Results", value, oldValue)
            End Set
        End Property

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDNotifica = reader.Read("Notifica", Me.m_IDNotifica)
            Me.m_AzioneType = reader.Read("Azione", Me.m_AzioneType)
            Me.m_DataEsecuzione = reader.Read("DataEsecuzione", Me.m_DataEsecuzione)
            Me.m_Parameters = reader.Read("Parameters", Me.m_Parameters)
            Me.m_Results = reader.Read("Results", Me.m_Results)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Notifica", Me.IDNotifica)
            writer.Write("Azione", Me.m_AzioneType)
            writer.Write("DataEsecuzione", Me.m_DataEsecuzione)
            writer.Write("Parameters", Me.m_Parameters)
            writer.Write("Results", Me.m_Results)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Notifica", Me.IDNotifica)
            writer.WriteAttribute("Azione", Me.m_AzioneType)
            writer.WriteAttribute("DataEsecuzione", Me.m_DataEsecuzione)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parameters", Me.m_Parameters)
            writer.WriteTag("Results", Me.m_Results)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Notifica" : Me.m_IDNotifica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Azione" : Me.m_AzioneType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataEsecuzione" : Me.m_DataEsecuzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Parameters" : Me.m_Parameters = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Results" : Me.m_Results = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Notifiche.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SYSNotifyRes"
        End Function



    End Class


End Class