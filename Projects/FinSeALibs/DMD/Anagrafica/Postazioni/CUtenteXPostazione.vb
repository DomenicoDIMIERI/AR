Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica



    ''' <summary>
    ''' Relazione tra gli utenti e le postazioni di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CUtenteXPostazione
        Inherits DBObjectBase

        Private m_UserName As String
        Private m_UserType As String
        Private m_IDUtente As Integer
        <NonSerialized> Private m_Utente As CUser
        Private m_IDPostazione As Integer
        <NonSerialized> Private m_Postazione As CPostazione

        Public Sub New()
            Me.m_UserName = ""
            Me.m_UserType = ""
            Me.m_IDUtente = 0
            Me.m_Utente = Nothing
            Me.m_IDPostazione = 0
            Me.m_Postazione = Nothing
        End Sub


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property UserName As String
            Get
                Return Me.m_UserName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_UserName
                value = Strings.Trim(value)
                If (value = oldValue) Then Return
                Me.m_UserName = value
                Me.DoChanged("UserName", value, oldValue)
            End Set
        End Property

        Public Property UserType As String
            Get
                Return Me.m_UserType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_UserType
                value = Strings.Trim(value)
                If (value = oldValue) Then Return
                Me.m_UserType = value
                Me.DoChanged("UserType", value, oldValue)
            End Set
        End Property

        Public Property IDUtente As Integer
            Get
                Return GetID(Me.m_Utente, Me.m_IDUtente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUtente
                If oldValue = value Then Exit Property
                Me.m_IDUtente = value
                Me.m_Utente = Nothing
                Me.DoChanged("IDUtente", value, oldValue)
            End Set
        End Property

        Public Property Utente As CUser
            Get
                If Me.m_Utente Is Nothing Then Me.m_Utente = Sistema.Users.GetItemById(Me.m_IDUtente)
                Return Me.m_Utente
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Utente
                If (oldValue = value) Then Exit Property
                Me.m_Utente = value
                Me.m_IDUtente = GetID(value)
                Me.DoChanged("Utente", value, oldValue)
            End Set
        End Property

        Public Property IDPostazione As Integer
            Get
                Return GetID(Me.m_Postazione, Me.m_IDPostazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPostazione
                If oldValue = value Then Exit Property
                Me.m_IDPostazione = value
                Me.m_Postazione = Nothing
                Me.DoChanged("IDPostazione", value, oldValue)
            End Set
        End Property

        Public Property Postazione As CPostazione
            Get
                If Me.m_Postazione Is Nothing Then Me.m_Postazione = Anagrafica.Postazioni.GetItemById(Me.m_IDPostazione)
                Return Me.m_Postazione
            End Get
            Set(value As CPostazione)
                Dim oldValue As CPostazione = Me.Postazione
                If (oldValue = value) Then Exit Property
                Me.m_Postazione = value
                Me.m_IDPostazione = GetID(value)
                Me.DoChanged("Postazione", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPostazione(ByVal value As CPostazione)
            Me.m_Postazione = value
            Me.m_IDPostazione = GetID(value)
        End Sub

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UtentiXPostazione"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_UserName = reader.Read("UserName", Me.m_UserName)
            Me.m_UserType = reader.Read("UserType", Me.m_UserType)
            Me.m_IDUtente = reader.Read("Utente", Me.m_IDUtente)
            Me.m_IDPostazione = reader.Read("Postazione", Me.m_IDPostazione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Utente", Me.IDUtente)
            writer.Write("Postazione", Me.IDPostazione)
            writer.Write("UserName", Me.m_UserName)
            writer.Write("UserType", Me.m_UserType)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return "UtenteXPostazione[" & Me.IDUtente & ", " & Me.IDPostazione & "]"
        End Function


        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDUtente", Me.IDUtente)
            writer.WriteAttribute("IDPostazione", Me.IDPostazione)
            writer.WriteAttribute("UserName", Me.m_UserName)
            writer.WriteAttribute("UserType", Me.m_UserType)
            MyBase.XMLSerialize(writer)
        End Sub



        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDUtente" : Me.m_IDUtente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPostazione" : Me.m_IDPostazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UserType" : Me.m_UserType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else
                    MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class


End Class