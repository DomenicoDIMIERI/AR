Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


 
    ''' <summary>
    ''' Relazione tra un utente ed un ufficio
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CUtenteXUfficio
        Inherits DBObjectBase

        Private m_IDUtente As Integer
        <NonSerialized> _
        Private m_Utente As CUser
        Private m_IDUfficio As Integer
        <NonSerialized> _
        Private m_Ufficio As CUfficio

        Public Sub New()
            Me.m_IDUtente = 0
            Me.m_Utente = Nothing
            Me.m_IDUfficio = 0
            Me.m_Ufficio = Nothing
        End Sub

        Public Sub New(ByVal idUfficio As Integer, ByVal idUtente As Integer)
            Me.m_IDUfficio = idUfficio
            Me.m_IDUtente = idUtente
            Me.m_Utente = Nothing
            Me.m_Ufficio = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

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

        Public Property IDUfficio As Integer
            Get
                Return GetID(Me.m_Ufficio, Me.m_IDUfficio)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUfficio
                If oldValue = value Then Exit Property
                Me.m_IDUfficio = value
                Me.m_Ufficio = Nothing
                Me.DoChanged("IDUfficio", value, oldValue)
            End Set
        End Property

        Public Property Ufficio As CUfficio
            Get
                If Me.m_Ufficio Is Nothing Then Me.m_Ufficio = Anagrafica.Uffici.GetItemById(Me.m_IDUfficio)
                Return Me.m_Ufficio
            End Get
            Set(value As CUfficio)
                Dim oldValue As CUfficio = Me.Ufficio
                If (oldValue = value) Then Exit Property
                Me.m_Ufficio = value
                Me.m_IDUfficio = GetID(value)
                Me.DoChanged("Ufficio", value, oldValue)
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UtentiXUfficio"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDUtente = reader.Read("Utente", Me.m_IDUtente)
            Me.m_IDUfficio = reader.Read("Ufficio", Me.m_IDUfficio)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Utente", Me.IDUtente)
            writer.Write("Ufficio", Me.IDUfficio)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return "UtenteXUfficio[" & Me.IDUtente & ", " & Me.IDUfficio & "]"
        End Function


        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_IDUtente", Me.IDUtente)
            writer.WriteAttribute("m_IDUfficio", Me.IDUfficio)
            MyBase.XMLSerialize(writer)
        End Sub



        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "m_IDUtente" : Me.m_IDUtente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_IDUfficio" : Me.m_IDUfficio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

    
End Class