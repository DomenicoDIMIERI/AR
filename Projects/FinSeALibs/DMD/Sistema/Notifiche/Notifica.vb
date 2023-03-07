Imports DMD.Databases

Partial Public Class Sistema

    ''' <summary>
    ''' Stato delle notifiche di sistema
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum StatoNotifica As Integer
        ''' <summary>
        ''' La notifica non è stata consegnata
        ''' </summary>
        ''' <remarks></remarks>
        NON_CONSEGNATA = 0

        ''' <summary>
        ''' L'utente ha ricevuto la notifica ma non l'ha ancora letta
        ''' </summary>
        ''' <remarks></remarks>
        CONSEGNATA = 1

        ''' <summary>
        ''' La notifica è stata letta
        ''' </summary>
        ''' <remarks></remarks>
        LETTA = 2

        ''' <summary>
        ''' La notifica è stata annullata
        ''' </summary>
        ''' <remarks></remarks>
        ANNULLATA = 3
    End Enum

    ''' <summary>
    ''' Rappresenta una notifica del sistema
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class Notifica
        Inherits DBObjectPO

        Private m_Data As Date                          'Data a partire dalla quale deve essere visualizzata la nofica
        Private m_Context As String                     'Contesto in cui è stata generata la nofica
        Private m_SourceName As String                  'Nome dell'oggetto a cui è associata la notifica
        Private m_SourceID As Integer                   'ID dell'oggetto a cui è associata la notifica
        Private m_TargetID As Integer                   'ID dell'utente a cui è destinata la notifica
        Private m_Target As CUser                       'Utente a cui è destinata la notifica
        Private m_TargetName As String                  'Nome dell'utente a cui è destinata la notifica
        Private m_Descrizione As String                 'Descrizione della notifica
        Private m_DataConsegna As Nullable(Of Date)      'Data di prima visualizzazione da parte dell'utente
        Private m_DataLettura As Nullable(Of Date)      'Data di lettura della notifica
        Private m_StatoNotifica As StatoNotifica                'Stato della notifica
        Private m_Categoria As String                   'Specifica una categoria

        Public Sub New()
            Me.m_Data = Nothing
            Me.m_Context = vbNullString
            Me.m_SourceName = vbNullString
            Me.m_SourceID = 0
            Me.m_TargetID = 0
            Me.m_Target = Nothing
            Me.m_TargetName = vbNullString
            Me.m_Descrizione = vbNullString
            Me.m_DataConsegna = Nothing
            Me.m_DataLettura = Nothing
            Me.m_StatoNotifica = StatoNotifica.NON_CONSEGNATA
            Me.m_Categoria = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la categoria della notifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Categoria
                If (value = oldValue) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data a partire dalla quale deve essere visualizzata la nofifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (oldValue = value) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive il contesto di validità dell'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Context As String
            Get
                Return Me.m_Context
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Context
                If (oldValue = value) Then Exit Property
                Me.m_Context = value
                Me.DoChanged("Context", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'oggetto che ha generato il promemoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceName As String
            Get
                Return Me.m_SourceName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValeu As String = Me.m_SourceName
                If (oldValeu = value) Then Exit Property
                Me.m_SourceName = value
                Me.DoChanged("SourceName", value, oldValeu)
            End Set
        End Property

        ''' <summary>
        ''' Retituisce o imposta l'ID dell'oggetto che ha generato questa notifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceID As Integer
            Get
                Return Me.m_SourceID
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SourceID
                If (oldValue = value) Then Exit Property
                Me.m_SourceID = value
                Me.DoChanged("SourceID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente a cui è destinata la notifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetID As Integer
            Get
                Return GetID(Me.m_Target, Me.m_TargetID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.TargetID
                If (oldValue = value) Then Exit Property
                Me.m_TargetID = value
                Me.m_Target = Nothing
                Me.DoChanged("TargetID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente a cui è destinata la notifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Target As CUser
            Get
                If (Me.m_Target Is Nothing) Then Me.m_Target = Users.GetItemById(Me.m_TargetID)
                Return Me.m_Target
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Target
                If (oldValue = value) Then Exit Property
                Me.m_Target = value
                Me.m_TargetID = GetID(value)
                If (value IsNot Nothing) Then Me.m_TargetName = value.Nominativo
                Me.DoChanged("Target", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente a cui è destinata la notifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetName As String
            Get
                Return Me.m_TargetName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TargetName
                If (oldValue = value) Then Exit Property
                Me.m_TargetName = value
                Me.DoChanged("TargetName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione della notifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora in cui la notifica è stata visualizzata sul PC dell'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta la data di lettura della notifica cioè la data in cui l'utente ha eseguito un'azioen sulla notifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta lo stato della notifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoNotifica As StatoNotifica
            Get
                Return Me.m_StatoNotifica
            End Get
            Set(value As StatoNotifica)
                Dim oldValue As StatoNotifica = Me.m_StatoNotifica
                If (oldValue = value) Then Exit Property
                Me.m_StatoNotifica = value
                Me.DoChanged("StatoNofica", value, oldValue)
            End Set
        End Property

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_Context = reader.Read("Context", Me.m_Context)
            Me.m_SourceName = reader.Read("SourceName", Me.m_SourceName)
            Me.m_SourceID = reader.Read("SourceID", Me.m_SourceID)
            Me.m_TargetID = reader.Read("TargetID", Me.m_TargetID)
            Me.m_TargetName = reader.Read("TargetName", Me.m_TargetName)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_DataConsegna = reader.Read("DataConsegna", Me.m_DataConsegna)
            Me.m_DataLettura = reader.Read("DataLettura", Me.m_DataLettura)
            Me.m_StatoNotifica = reader.Read("StatoNotifica", Me.m_StatoNotifica)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Data", Me.m_Data)
            writer.Write("DataStr", DBUtils.ToDBDateStr(Me.m_Data))
            writer.Write("Context", Me.m_Context)
            writer.Write("SourceName", Me.m_SourceName)
            writer.Write("SourceID", Me.SourceID)
            writer.Write("TargetID", Me.TargetID)
            writer.Write("TargetName", Me.m_TargetName)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("DataConsegna", Me.m_DataConsegna)
            writer.Write("DataLettura", Me.m_DataLettura)
            writer.Write("StatoNotifica", Me.m_StatoNotifica)
            writer.Write("Categoria", Me.m_Categoria)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("Context", Me.m_Context)
            writer.WriteAttribute("SourceName", Me.m_SourceName)
            writer.WriteAttribute("SourceID", Me.SourceID)
            writer.WriteAttribute("TargetID", Me.TargetID)
            writer.WriteAttribute("TargetName", Me.m_TargetName)
            writer.WriteAttribute("DataConsegna", Me.m_DataConsegna)
            writer.WriteAttribute("DataLettura", Me.m_DataLettura)
            writer.WriteAttribute("StatoNotifica", Me.m_StatoNotifica)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Context" : Me.m_Context = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceName" : Me.m_SourceName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TargetID" : Me.m_TargetID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TargetName" : Me.m_TargetName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataConsegna" : Me.m_DataConsegna = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataLettura" : Me.m_DataLettura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoNotifica" : Me.m_StatoNotifica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
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
            Return "tbl_SYSNotify"
        End Function



    End Class


End Class