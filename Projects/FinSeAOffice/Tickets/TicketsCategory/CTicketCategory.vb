Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Office

    ''' <summary>
    ''' Rappresenta una categoria e sottocategoria per una richiesta di assistenza
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CTicketCategory
        Inherits DBObject

        Private m_Categoria As String
        Private m_Sottocategoria As String
        Private m_NotifyUsers As CCollection(Of CUser)

        Public Sub New()
            Me.m_Categoria = vbNullString
            Me.m_Sottocategoria = vbNullString
            Me.m_NotifyUsers = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce l'elenco degli utenti a cui vengono notificate le richieste di assistenza per questo tipo di categoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NotifyUsers As CCollection(Of CUser)
            Get
                SyncLock Me
                    If Me.m_NotifyUsers Is Nothing Then Me.m_NotifyUsers = New CCollection(Of CUser)
                    Return Me.m_NotifyUsers
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della categoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Categoria
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della sottocategoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Sottocategoria As String
            Get
                Return Me.m_Sottocategoria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Sottocategoria
                If (oldValue = value) Then Exit Property
                Me.m_Sottocategoria = value
                Me.DoChanged("Sottocategoria", value, oldValue)
            End Set
        End Property

        Private Function SerializeUsers() As String
            Dim ret As New System.Text.StringBuilder
            For Each u As CUser In Me.NotifyUsers
                If (ret.Length > 0) Then ret.Append(",")
                ret.Append(GetID(u))
            Next
            Return ret.ToString
        End Function

        Private Function DeserializeUsers(ByVal str As String) As CCollection(Of CUser)
            Dim ret As New CCollection(Of CUser)
            str = Strings.Trim(str)
            Dim ids As String() = Split(str, ",")
            If (Arrays.Len(ids) > 0) Then
                For Each id As String In ids
                    Try
                        Dim user As CUser = Sistema.Users.GetItemById(Formats.ToInteger(id))
                        If (user IsNot Nothing) Then ret.Add(user)
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                    End Try
                Next
            End If
            Return ret
        End Function


        Public Overrides Function ToString() As String
            Return "{ " & Me.m_Categoria & ", " & Me.m_Sottocategoria & " }"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_Sottocategoria = reader.Read("Sottocategoria", Me.m_Sottocategoria)
            Me.m_NotifyUsers = Me.DeserializeUsers(reader.Read("NotifyUsers", ""))
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("Sottocategoria", Me.m_Sottocategoria)
            writer.Write("NotifyUsers", Me.SerializeUsers)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("Sottocategoria", Me.m_Sottocategoria)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("NotifyUsers", Me.SerializeUsers)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Sottocategoria" : Me.m_Sottocategoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotifyUsers" : Me.m_NotifyUsers = Me.DeserializeUsers(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return TicketCategories.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SupportTicketsCat"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            TicketCategories.UpdateCached(Me)
            TicketCategories.InvalidateUserAllowedCategories()
        End Sub



    End Class


End Class