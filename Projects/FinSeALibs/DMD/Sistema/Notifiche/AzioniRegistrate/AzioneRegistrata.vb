Imports DMD.Databases

Partial Public Class Sistema

     


    ''' <summary>
    ''' Azione definita per il tipo di oggetto associato alla notifica (campo SourceName)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class AzioneRegistrata
        Inherits DBObject
        Implements IComparable

        Private m_Priorita As Integer
        Private m_Description As String
        Private m_Categoria As String
        Private m_SourceName As String
        Private m_ActionType As String
        Private m_IconURL As String
        Private m_Attivo As Boolean

        Public Sub New()
            Me.m_Priorita = 0
            Me.m_Description = ""
            Me.m_SourceName = ""
            Me.m_ActionType = ""
            Me.m_Categoria = ""
            Me.m_IconURL = ""
            Me.m_Attivo = True
        End Sub

        Public Function NewAzione() As AzioneEseguibile
            Return Types.CreateInstance(Me.ActionType)
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il gestore è abilitato o meno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return Me.m_Attivo
            End Get
            Set(value As Boolean)
                If (Me.m_Attivo = value) Then Exit Property
                Me.m_Attivo = value
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore intero che viene utilizzato per ordinare in senso crescente le azioni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priorita As Integer
            Get
                Return Me.m_Priorita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Priorita
                If (oldValue = value) Then Exit Property
                Me.m_Priorita = value
                Me.DoChanged("Priorita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'immagine associata all'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IconURL
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una descrizione per l'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Description As String
            Get
                Return Me.m_Description
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Description
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Description = value
                Me.DoChanged("Description", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la categoria della notifica a cui si applica l'azione.
        ''' Se questo campo è vuoto l'azione si applica a tutte le categorie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Categoria
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo su cui è definita l'azione
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
                Dim oldValue As String = Me.m_SourceName
                If (oldValue = value) Then Exit Property
                Me.m_SourceName = value
                Me.DoChanged("SourceName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del tipo che definisce l'azione sugli oggetti SourceName
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ActionType As String
            Get
                Return Me.m_ActionType
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ActionType
                If (oldValue = value) Then Exit Property
                Me.m_ActionType = value
                Me.DoChanged("ActionType", value, oldValue)
            End Set
        End Property



        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Priorita = reader.Read("Priorita", Me.m_Priorita)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_Description = reader.Read("Description", Me.m_Description)
            Me.m_SourceName = reader.Read("SourceType", Me.m_SourceName)
            Me.m_ActionType = reader.Read("ActionType", Me.m_ActionType)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_Attivo = reader.Read("Attivo", Me.m_Attivo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Priorita", Me.m_Priorita)
            writer.Write("Description", Me.m_Description)
            writer.Write("SourceType", Me.m_SourceName)
            writer.Write("ActionType", Me.m_ActionType)
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("Attivo", Me.m_Attivo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Notifiche.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SYSNotifyAct"
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Priorita", Me.m_Priorita)
            writer.WriteAttribute("Description", Me.m_Description)
            writer.WriteAttribute("SourceName", Me.m_SourceName)
            writer.WriteAttribute("ActionType", Me.m_ActionType)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Priorita" : Me.m_Priorita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Description" : Me.m_Description = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceName" : Me.m_SourceName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ActionType" : Me.m_ActionType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            SyncLock Sistema.Notifiche
                Dim item As AzioneRegistrata = Sistema.Notifiche.RegisteredActions.GetItemById(GetID(Me))
                If item IsNot Nothing Then Sistema.Notifiche.RegisteredActions.Remove(item)
                If Me.Stato = ObjectStatus.OBJECT_VALID Then
                    Sistema.Notifiche.RegisteredActions.Add(Me)
                    Sistema.Notifiche.RegisteredActions.Sort()
                End If
            End SyncLock
        End Sub

        Public Function CompareTo(ByVal other As AzioneRegistrata) As Integer
            Dim ret As Integer = Me.m_Priorita - other.m_Priorita
            If (ret = 0) Then ret = Strings.Compare(Me.m_Description, other.m_Description, CompareMethod.Text)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class

       


 

End Class