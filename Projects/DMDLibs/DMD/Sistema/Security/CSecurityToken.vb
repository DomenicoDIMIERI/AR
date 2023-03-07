Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases

Partial Public Class Sistema

    <Serializable> _
    Public NotInheritable Class CSecurityToken
        Inherits DBObjectBase

        Friend m_TokenID As String
        Friend m_TokenClass As String
        Friend m_TokenSourceName As String
        Friend m_TokenSourceID As Integer
        Friend m_TokenName As String
        Friend m_Valore As String
        Friend m_Session As String
        Friend m_CreatoDaID As Integer
        Friend m_CreatoDa As CUser
        Friend m_CreatoIl As Date
        Friend m_UsatoDaID As Integer
        Friend m_UsatoDa As CUser
        Friend m_UsatoIl As Nullable(Of Date)
        Friend m_Dettaglio As String
        Friend m_ExpireTime As Nullable(Of Date)
        Friend m_ExpireCount As Integer
        Friend m_UseCount As Integer

        Public Sub New()
            Me.m_TokenID = ""
            Me.m_TokenName = ""
            Me.m_Valore = ""
            Me.m_Session = ""
            Me.m_CreatoDaID = 0
            Me.m_CreatoDa = Nothing
            Me.m_CreatoIl = Nothing
            Me.m_UsatoDaID = 0
            Me.m_UsatoDa = Nothing
            Me.m_UsatoIl = Nothing
            Me.m_Dettaglio = ""
            Me.m_ExpireTime = Nothing
            Me.m_ExpireCount = 1
            Me.m_UseCount = 0
            Me.m_TokenSourceName = ""
            Me.m_TokenSourceID = 0
            Me.m_TokenClass = ""
        End Sub

        Public Property TokenClass As String
            Get
                Return Me.m_TokenClass
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TokenClass
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TokenClass = value
                Me.DoChanged("TokenClass", value, oldValue)
            End Set
        End Property

        Public Property TokenSourceName As String
            Get
                Return Me.m_TokenSourceName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TokenSourceName
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TokenSourceName = value
                Me.DoChanged("TokenSourceName", value, oldValue)
            End Set
        End Property


        Public Property TokenSourceID As Integer
            Get
                Return Me.m_TokenSourceID
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TokenSourceID
                If (oldValue = value) Then Exit Property
                Me.m_TokenSourceID = value
                Me.DoChanged("TokenSourceID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la stringa che identifica questo oggetto univoco
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TokenID As String
            Get
                Return Me.m_TokenID
            End Get
        End Property

        Friend Sub SetToken(ByVal value As String)
            Me.m_TokenID = value
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TokenName As String
            Get
                Return Me.m_TokenName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TokenName
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TokenName = value
                Me.DoChanged("TokenName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore del token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TokenValue As String
            Get
                Return Me.m_Valore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Valore
                If (oldValue = value) Then Exit Property
                Me.m_Valore = value
                Me.DoChanged("TokenValue", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della sessione utente in cui è stato creato il token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SessionID As String
            Get
                Return Me.m_Session
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Session
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Session = value
                Me.DoChanged("SessionID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'ID dell'utente che ha creato il token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CreatoDaID As Integer
            Get
                Return GetID(Me.m_CreatoDa, Me.m_CreatoDaID)
            End Get
        End Property


        ''' <summary>
        ''' Restituisce l'utente che ha creato il token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CreatoDa As CUser
            Get
                If (Me.m_CreatoDa Is Nothing) Then Me.m_CreatoDa = Sistema.Users.GetItemById(Me.m_CreatoDaID)
                Return Me.m_CreatoDa
            End Get
        End Property

        Friend Sub SetCreatoDa(ByVal value As CUser)
            Me.m_CreatoDa = value
            Me.m_CreatoDaID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce la data di creazione del token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CreatoIl As Date
            Get
                Return Me.m_CreatoIl
            End Get
        End Property

        Friend Sub SetCreatoIl(ByVal value As Date)
            Me.m_CreatoIl = value
        End Sub

        ''' <summary>
        ''' Restituisce l'ID dell'utente che ha usato il token per la prima volta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UsatoDaID As Integer
            Get
                Return GetID(Me.m_UsatoDa, Me.m_UsatoDaID)
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'utente che ha usato il token per la prima volta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UsatoDa As CUser
            Get
                If (Me.m_UsatoDa Is Nothing) Then Me.m_UsatoDa = Sistema.Users.GetItemById(Me.m_UsatoDaID)
                Return Me.m_UsatoDa
            End Get
        End Property

        Friend Sub SetUsatoDa(ByVal user As CUser)
            Me.m_UsatoDa = user
            Me.m_UsatoDaID = GetID(user)
        End Sub

        ''' <summary>
        ''' Restitusice la data della prima volta in cui è stato usato il token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UsatoIl As Nullable(Of Date)
            Get
                Return Me.m_UsatoIl
            End Get
        End Property

        Friend Sub SetUsatoIl(ByVal value As Nullable(Of Date))
            Me.m_UsatoIl = value
        End Sub

        ''' <summary>
        ''' Restituisce o imposta una stringa descrittiva dell'utilizzo del token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Dettaglio As String
            Get
                Return Me.m_Dettaglio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Dettaglio
                If (oldValue = value) Then Exit Property
                Me.m_Dettaglio = value
                Me.DoChanged("Dettaglio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la data oltra la quale il token non sarà più utilizzabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ExpireTime As Nullable(Of Date)
            Get
                Return Me.m_ExpireTime
            End Get
        End Property

        Friend Sub SetExpireTime(ByVal value As Nullable(Of Date))
            Me.m_ExpireTime = value
        End Sub

        ''' <summary>
        ''' Restituisce il numero di volte in cui il token può essere utilizzato.
        ''' Se 0 il token può essere utilizzato infinite volte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ExpireCount As Integer
            Get
                Return Me.m_ExpireCount
            End Get
        End Property

        Friend Sub SetExpireCount(ByVal value As Integer)
            Me.m_ExpireCount = value
        End Sub

        ''' <summary>
        ''' Restituisce il numero di volte in cui il token è stato utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UseCount As Integer
            Get
                Return Me.m_UseCount
            End Get
        End Property

        Friend Sub SetUseCount(ByVal value As Integer)
            Me.m_UseCount = value
        End Sub

        ''' <summary>
        ''' Utilizza il token ed aggiunge il dettaglio.
        ''' Se il token è scaduto o non è più utilizzabile viene generato un errore
        ''' </summary>
        ''' <param name="dettaglio"></param>
        ''' <remarks></remarks>
        Public Sub Usa(ByVal dettaglio As String)
            SyncLock ASPSecurity.Lock
                If (Me.m_ExpireTime.HasValue AndAlso Me.m_ExpireTime < DateUtils.Now) Then
                    Throw New InvalidOperationException("Il token è scaduto")
                ElseIf (Me.m_ExpireCount > 0 AndAlso Me.m_UseCount >= Me.m_ExpireCount) Then
                    Throw New InvalidOperationException("Superato il numero di utilizzi del token")
                End If
                If (Me.m_UsatoDaID = 0) Then
                    Me.m_UsatoDa = Sistema.Users.CurrentUser
                    Me.m_UsatoDaID = GetID(Me.m_UsatoDa)
                    Me.m_UsatoIl = Now
                End If
                Me.m_UseCount += 1
                Me.m_Dettaglio = Strings.Combine(Me.m_Dettaglio, dettaglio, vbNewLine)
                Me.Save(True)
            End SyncLock
        End Sub



        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SecurityTokens"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_TokenID = reader.Read("Token", Me.m_TokenID)
            Me.m_TokenName = reader.Read("TokenName", Me.m_TokenName)
            Me.m_Valore = reader.Read("Valore", Me.m_Valore)
            Me.m_Session = reader.Read("Session", Me.m_Session)
            Me.m_CreatoDaID = reader.Read("CreatoDa", Me.m_CreatoDaID)
            Me.m_CreatoIl = reader.Read("CreatoIl", Me.m_CreatoIl)
            Me.m_UsatoDaID = reader.Read("UsatoDa", Me.m_UsatoDaID)
            Me.m_UsatoIl = reader.Read("UsatoIl", Me.m_UsatoIl)
            Me.m_Dettaglio = reader.Read("Dettaglio", Me.m_Dettaglio)
            Me.m_ExpireTime = reader.Read("ExpireTime", Me.m_ExpireTime)
            Me.m_ExpireCount = reader.Read("ExpireCount", Me.m_ExpireCount)
            Me.m_UseCount = reader.Read("UseCount", Me.m_UseCount)
            Me.m_TokenSourceName = reader.Read("TokenSourceName", Me.m_TokenSourceName)
            Me.m_TokenSourceID = reader.Read("TokenSourceID", Me.m_TokenSourceID)
            Me.m_TokenClass = reader.Read("TokenClass", Me.m_TokenClass)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Token", Me.m_TokenID)
            writer.Write("TokenName", Me.m_TokenName)
            writer.Write("Valore", Me.m_Valore)
            writer.Write("Session", Me.m_Session)
            writer.Write("CreatoDa", Me.m_CreatoDaID)
            writer.Write("CreatoIl", Me.m_CreatoIl)
            writer.Write("UsatoDa", Me.m_UsatoDaID)
            writer.Write("UsatoIl", Me.m_UsatoIl)
            writer.Write("Dettaglio", Me.m_Dettaglio)
            writer.Write("ExpireTime", Me.m_ExpireTime)
            writer.Write("ExpireCount", Me.m_ExpireCount)
            writer.Write("UseCount", Me.m_UseCount)
            writer.Write("TokenSourceName", Me.m_TokenSourceName)
            writer.Write("TokenSourceID", Me.m_TokenSourceID)
            writer.Write("TokenClass", Me.m_TokenClass)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("TokenID", Me.m_TokenID)
            writer.WriteAttribute("TokenName", Me.m_TokenName)
            writer.WriteAttribute("Valore", Me.m_Valore)
            writer.WriteAttribute("Session", Me.m_Session)
            writer.WriteAttribute("CreatoDa", Me.m_CreatoDaID)
            writer.WriteAttribute("CreatoIl", Me.m_CreatoIl)
            writer.WriteAttribute("UsatoDa", Me.m_UsatoDaID)
            writer.WriteAttribute("UsatoIl", Me.m_UsatoIl)
            writer.WriteAttribute("ExpireTime", Me.m_ExpireTime)
            writer.WriteAttribute("ExpireCount", Me.m_ExpireCount)
            writer.WriteAttribute("UseCount", Me.m_UseCount)
            writer.WriteAttribute("TokenSourceName", Me.m_TokenSourceName)
            writer.WriteAttribute("TokenSourceID", Me.m_TokenSourceID)
            writer.WriteAttribute("TokenClass", Me.m_TokenClass)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Dettaglio", Me.m_Dettaglio)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "TokenID" : Me.m_TokenID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TokenName" : Me.m_TokenName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Valore" : Me.m_Valore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Session" : Me.m_Session = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CreatoDa" : Me.m_CreatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CreatoIl" : Me.m_CreatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "UsatoDa" : Me.m_UsatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UsatoIl" : Me.m_UsatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Dettaglio" : Me.m_Dettaglio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ExpireTime" : Me.m_ExpireTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ExpireCount" : Me.m_ExpireCount = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UseCount" : Me.m_UseCount = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TokenSourceName" : Me.m_TokenSourceName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TokenSourceID" : Me.m_TokenSourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TokenClass" : Me.m_TokenClass = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

    End Class

End Class

