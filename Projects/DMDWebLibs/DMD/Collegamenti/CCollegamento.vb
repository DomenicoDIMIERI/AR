Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class WebSite

    <Serializable> _
    Public Class CCollegamento
        Inherits DBObject
        Implements IComparable(Of CCollegamento), IComparable

        Private m_Nome As String
        Private m_Descrizione As String
        Private m_Link As String
        Private m_Gruppo As String
        Private m_Target As String
        Private m_IconURL As String
        Private m_EncriptURL As Boolean
        Private m_PostedData As String
        Private m_IDParent As Integer
        <NonSerialized> Private m_Parent As CCollegamento
        Private m_Childs As CSubCollegamenti
        Private m_CallModule As String
        Private m_CallAction As String
        Private m_UsersAuth As LinkUserAllowNegateCollection
        Private m_GroupAuth As LinkGroupAllowNegateCollection
        Private m_Posizione As Integer
        Private m_Flags As Integer
        Private m_Attivo As Boolean

        Public Sub New()
            Me.m_Nome = "Nuovo collegamento"
            Me.m_Descrizione = ""
            Me.m_Link = ""
            Me.m_Gruppo = ""
            Me.m_Target = ""
            Me.m_IconURL = ""
            Me.m_EncriptURL = False
            Me.m_IDParent = 0
            Me.m_Childs = Nothing
            Me.m_Parent = Nothing
            Me.m_CallAction = vbNullString
            Me.m_CallModule = vbNullString
            Me.m_Posizione = 0
            Me.m_Flags = 0
            Me.m_PostedData = ""
            Me.m_Attivo = True
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Collegamenti.Module
        End Function

        Public Property PostedData As String
            Get
                Return Me.m_PostedData
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_PostedData
                If (oldValue = value) Then Return
                Me.m_PostedData = value
                Me.DoChanged("PostedData", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags
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
        ''' Restituisce o imposta un valore crescente che forza l'ordinamento (ordinamento per gruppo, posizione, nome)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Posizione As Integer
            Get
                Return Me.m_Posizione
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Posizione
                If (oldValue = value) Then Exit Property
                Me.m_Posizione = value
                Me.DoChanged("Posizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il link è attivo
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
        ''' Restituisce o imposta il nome del link (deve essere univoco)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del modulo a cui fa riferimento il link
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CallModule As String
            Get
                Return Me.m_CallModule
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CallModule
                If (oldValue = value) Then Exit Property
                Me.m_CallModule = value
                Me.DoChanged("CallModule", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'azione da eseguire sul modulo a cui fa riferimento il link
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CallAction As String
            Get
                Return Me.m_CallAction
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CallAction
                If (oldValue = value) Then Exit Property
                Me.m_CallAction = value
                Me.DoChanged("CallModule", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica che la url deve essere nascosta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EncriptURL As Boolean
            Get
                Return Me.m_EncriptURL
            End Get
            Set(value As Boolean)
                If (Me.m_EncriptURL = value) Then Exit Property
                Me.m_EncriptURL = value
                Me.DoChanged("EncriptURL", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il testo visualizzato per il link
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
        ''' Restituisce o imposta la url a cui fa riferimento il link
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Link As String
            Get
                Return Me.m_Link
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Link
                If (oldValue = value) Then Exit Property
                Me.m_Link = value
                Me.DoChanged("Link", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del gruppo a cui fa riferimento il link
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Gruppo As String
            Get
                Return Me.m_Gruppo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Gruppo
                If (oldValue = value) Then Exit Property
                Me.m_Gruppo = value
                Me.DoChanged("Gruppo", value, oldValue)
            End Set
        End Property

        Public Property Target As String
            Get
                Return Me.m_Target
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Target
                If (oldValue = value) Then Exit Property
                Me.m_Target = value
                Me.DoChanged("Target", value, oldValue)
            End Set
        End Property

        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IconURL
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        Public Function GetURL() As String
            Dim ret As String
            If Me.m_EncriptURL Then
                'ret = "/download.asp?tk=" & ASPSecurity.FindTokenOrCreate(Me.m_Link, Me.m_Link) & "&dn=" & m_Descrizione
                ret = Me.m_Link
            Else
                ret = Me.m_Link
            End If
            Return ret
        End Function

        Public Property IDParent As Integer
            Get
                Return GetID(Me.m_Parent, Me.m_IDParent)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDParent
                If oldValue = value Then Exit Property
                Me.m_IDParent = value
                Me.m_Parent = Nothing
                Me.DoChanged("IDParent", value, oldValue)
            End Set
        End Property

        Public Property Parent As CCollegamento
            Get
                If Me.m_Parent Is Nothing Then Me.m_Parent = Collegamenti.GetItemById(Me.m_IDParent)
                Return Me.m_Parent
            End Get
            Set(value As CCollegamento)
                Dim oldValue As CCollegamento = Me.Parent
                If (oldValue = value) Then Exit Property
                Me.m_Parent = value
                Me.m_IDParent = GetID(value)
                Me.DoChanged("Parent", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetParent(ByVal p As CCollegamento)
            Me.m_Parent = p
            Me.m_IDParent = GetID(p)
        End Sub

        Public ReadOnly Property Childs As CSubCollegamenti
            Get
                SyncLock Me
                    If Me.m_Childs Is Nothing Then Me.m_Childs = New CSubCollegamenti(Me)
                    Return Me.m_Childs
                End SyncLock
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Collegamenti"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Link = reader.Read("Link", Me.m_Link)
            Me.m_Gruppo = reader.Read("Gruppo", Me.m_Gruppo)
            Me.m_Target = reader.Read("Target", Me.m_Target)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_EncriptURL = reader.Read("EncriptURL", Me.m_EncriptURL)
            Me.m_IDParent = reader.Read("IDParent", Me.m_IDParent)
            Me.m_CallModule = reader.Read("CallModule", Me.m_CallModule)
            Me.m_CallAction = reader.Read("CallAction", Me.m_CallAction)
            Me.m_Attivo = reader.Read("Attivo", Me.m_Attivo)
            Me.m_Posizione = reader.Read("Posizione", Me.m_Posizione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_PostedData = reader.Read("PostedData", Me.m_PostedData)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Link", Me.m_Link)
            writer.Write("Gruppo", Me.m_Gruppo)
            writer.Write("Target", Me.m_Target)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("EncriptURL", Me.m_EncriptURL)
            writer.Write("IDParent", Me.IDParent)
            writer.Write("CallModule", Me.m_CallModule)
            writer.Write("CallAction", Me.m_CallAction)
            writer.Write("Attivo", Me.m_Attivo)
            writer.Write("Posizione", Me.m_Posizione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("PostedData", Me.m_PostedData)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Descrizione
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Link", Me.m_Link)
            writer.WriteAttribute("Gruppo", Me.m_Gruppo)
            writer.WriteAttribute("Target", Me.m_Target)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("EncriptURL", Me.m_EncriptURL)
            writer.WriteAttribute("IDParent", Me.IDParent)
            writer.WriteAttribute("CallModule", Me.m_CallModule)
            writer.WriteAttribute("CallAction", Me.m_CallAction)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            writer.WriteAttribute("Posizione", Me.m_Posizione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("PostedData", Me.m_PostedData)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Link" : Me.m_Link = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Gruppo" : Me.m_Gruppo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Target" : Me.m_Target = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "EncriptURL" : Me.m_EncriptURL = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IDParent" : Me.m_IDParent = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CallModule" : Me.m_CallModule = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CallAction" : Me.m_CallAction = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Posizione" : Me.m_Posizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PostedData" : Me.m_PostedData = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function SetUserAllowNegate(ByVal user As CUser, ByVal allow As Boolean, ByVal negate As Boolean) As LinkUserAllowNegate
            Dim item As LinkUserAllowNegate = Nothing
            Dim i As Integer = 0
            While (i < Me.UserAuth.Count) AndAlso (item Is Nothing)
                If Me.UserAuth(i).UserID = GetID(user) Then
                    item = Me.UserAuth(i)
                Else
                    i += 1
                End If
            End While
            If (allow = negate) Then
                If (item IsNot Nothing) Then
                    item.Delete()
                    Me.UserAuth.Remove(item)
                End If
            Else
                If (item Is Nothing) Then
                    item = New LinkUserAllowNegate
                    Me.UserAuth.Add(item)
                End If
                item.Item = Me
                item.User = user
                item.Allow = allow
                item.Negate = negate
                item.Save()
            End If

            Return item
        End Function

        Public Function SetGroupAllowNegate(ByVal group As CGroup, ByVal allow As Boolean, ByVal negate As Boolean) As LinkGroupAllowNegate
            Dim item As LinkGroupAllowNegate = Nothing
            Dim i As Integer = 0
            While (i < Me.GroupAuth.Count) AndAlso (item Is Nothing)
                If Me.GroupAuth(i).GroupID = GetID(group) Then
                    item = Me.GroupAuth(i)
                Else
                    i += 1
                End If
            End While
            If (allow = negate) Then
                If (item IsNot Nothing) Then
                    item.Delete()
                    Me.GroupAuth.Remove(item)
                End If
            Else
                If (item Is Nothing) Then
                    item = New LinkGroupAllowNegate
                    Me.GroupAuth.Add(item)
                End If
                item.Item = Me
                item.Group = group
                item.Allow = allow
                item.Negate = negate
                item.Save()
            End If

            Return item
        End Function
 

        ''' <summary>
        ''' Restituisce vero se l'utente è abilitato all'azione corrente anche considerando i gruppi a cui appartiene l'utente stesso
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsVisibleToUser(ByVal user As CUser) As Boolean
            Dim a = False, n As Boolean = False
            Dim i As Integer = 0
            Dim ua As LinkUserAllowNegate
            Dim ga As LinkGroupAllowNegate

            While (Not n AndAlso i < Me.UserAuth.Count)
                ua = Me.UserAuth(i)
                If (ua.UserID = GetID(user)) Then
                    a = ua.Allow
                    n = ua.Negate
                End If
                i += 1
            End While

            i = 0
            While (Not n AndAlso i < Me.GroupAuth.Count)
                ga = Me.GroupAuth(i)
                For Each gp As CGroup In user.Groups
                    If (ga.GroupID = GetID(gp)) Then
                        a = ga.Allow
                        n = ga.Negate
                        Exit For
                    End If
                Next
                i += 1
            End While

            Return a And Not n
        End Function

        Public Function IsVisibleToUser(ByVal userID As Integer) As Boolean
            Return Me.IsVisibleToUser(Sistema.Users.GetItemById(userID))
        End Function

        Public ReadOnly Property UserAuth As LinkUserAllowNegateCollection
            Get
                SyncLock Me
                    If (Me.m_UsersAuth Is Nothing) Then Me.m_UsersAuth = New LinkUserAllowNegateCollection(Me)
                    Return Me.m_UsersAuth
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property GroupAuth As LinkGroupAllowNegateCollection
            Get
                SyncLock Me
                    If (Me.m_GroupAuth Is Nothing) Then Me.m_GroupAuth = New LinkGroupAllowNegateCollection(Me)
                    Return Me.m_GroupAuth
                End SyncLock
            End Get
        End Property

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_Childs = Nothing
            Me.m_GroupAuth = Nothing
            Me.m_UsersAuth = Nothing
        End Sub
        

        Public Overridable Function CompareTo(other As CCollegamento) As Integer Implements IComparable(Of CCollegamento).CompareTo
            Dim ret As Integer
            ret = Strings.Compare(Me.m_Gruppo, other.m_Gruppo, CompareMethod.Text)
            If (ret = 0) Then ret = Arrays.Compare(Me.m_Posizione, other.m_Posizione)
            If (ret = 0) Then ret = Strings.Compare(Me.m_Descrizione, other.m_Descrizione, CompareMethod.Text)
            Return ret
        End Function


        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Collegamenti.UpdateCached(Me)
        End Sub

    End Class

End Class

