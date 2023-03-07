Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Office


    ''' <summary>
    ''' Rappresenta un documento o una comunicazione pubblicata sul sito
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CComunicazione
        Inherits DBObject
        Implements IIndexable

        Private m_DataInizio As Nullable(Of Date) 'Data
        Private m_DataFine As Nullable(Of Date) 'Data Fine
        Private m_Categoria As String 'Categoria
        Private m_Descrizione As String 'Descrizione
        Private m_Note As String
        Private m_Priorita As PriorityEnum
        Private m_Path As String
        Private m_PrimaPagina As Boolean 'Vero se visibile in prima pagina
        Private m_Progressivo As Integer
        Private m_Attachments As CCollection(Of CAttachment)

        Public Sub New()
            Me.m_DataInizio = Now
            Me.m_DataFine = Nothing
            Me.m_Categoria = ""
            Me.m_Descrizione = ""
            Me.m_Note = ""
            Me.m_Priorita = 0
            Me.m_Path = ""
            Me.m_PrimaPagina = False
            Me.m_Progressivo = 0
            Me.m_Attachments = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Comunicazioni.Module
        End Function

        ''' <summary>
        ''' Restituisce la collezione degli allegati alla comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attachments As CCollection(Of CAttachment)
            Get
                If (Me.m_Attachments Is Nothing) Then Me.m_Attachments = New CCollection(Of CAttachment)
                Return Me.m_Attachments
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero progressivo del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Progressivo As Integer
            Get
                Return Me.m_Progressivo
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Progressivo
                If (oldValue = value) Then Exit Property
                Me.m_Progressivo = value
                Me.DoChanged("Progressivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio validità del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine validità del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la categoria del documento
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
        ''' Restituisce o imposta la descrizione del documento
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
        ''' Restituisce o imposta il livello di priorità del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priorita As PriorityEnum
            Get
                Return Me.m_Priorita
            End Get
            Set(value As PriorityEnum)
                Dim oldValue As PriorityEnum = Me.m_Priorita
                If (oldValue = value) Then Exit Property
                Me.m_Priorita = value
                Me.DoChanged("Priorita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del collegamento esterno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property URL As String
            Get
                Return Me.m_Path
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Path
                If (oldValue = value) Then Exit Property
                Me.m_Path = value
                Me.DoChanged("URL", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il documento deve essere visualizzato nell'elenco delle comunicazioni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrimaPagina As Boolean
            Get
                Return Me.m_PrimaPagina
            End Get
            Set(value As Boolean)
                If (Me.m_PrimaPagina = value) Then Exit Property
                Me.m_PrimaPagina = value
                Me.DoChanged("PrimaPagina", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta le note
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public Sub AddAttachment(ByVal a As CAttachment)
            If (Me.Attachments.Contains(a)) Then Throw New ArgumentOutOfRangeException("Allegato già associato alla comunicazione")
            a.Stato = ObjectStatus.OBJECT_VALID
            a.Save()
            Me.Attachments.Add(a)
            Me.Save(True)
        End Sub

        Public Sub DeleteAttachment(ByVal a As CAttachment)
            Me.Attachments.Remove(a)
            a.Delete()
            Me.Save(True)
        End Sub

        'Public Function GetLink() As String
        '    Return WebSite.Configuration.URL & "/?_m=" & GetID(Comunicazioni.Module) & "&_a=get&ID=" & GetID(Me)
        'End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Comunicazioni"
        End Function

        Protected Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Priorita = reader.Read("Priorita", Me.m_Priorita)
            Me.m_Path = reader.Read("path", Me.m_Path)
            Me.m_PrimaPagina = reader.Read("PrimaPagina", Me.m_PrimaPagina)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_Progressivo = reader.Read("Progressivo", Me.m_Progressivo)
            Try
                Me.m_Attachments = New CCollection(Of CAttachment)
                Me.m_Attachments.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("Attachments", "")))
            Catch ex As Exception
                Me.m_Attachments = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Priorita", Me.m_Priorita)
            writer.Write("path", Me.m_Path)
            writer.Write("PrimaPagina", Me.m_PrimaPagina)
            writer.Write("Note", Me.m_Note)
            writer.Write("Progressivo", Me.m_Progressivo)
            writer.Write("Attachments", XML.Utils.Serializer.Serialize(Me.Attachments))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Descrizione
        End Function



        ''' <summary>
        ''' Restituisce un oggetto CCollection contenente gli utenti interessati alla modifica
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAffectedUsers() As CCollection(Of CUser)
            Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader("SELECT * FROM [tbl_ComunicazioniXGruppo] WHERE [Comunicazione] = " & GetID(Me))
            Dim items As New CKeyCollection(Of ItemAllowNegate(Of CUser))
            Dim group As CGroup
            Dim user As CUser
            Dim item As ItemAllowNegate(Of CUser)
            Dim ret As New CCollection(Of CUser)

            While dbRis.Read
                group = Sistema.Groups.GetItemById(Formats.ToInteger(dbRis("Gruppo")))
                If (group IsNot Nothing AndAlso group.Stato = ObjectStatus.OBJECT_VALID) Then
                    For Each user In group.Members
                        item = items.GetItemByKey(user.UserName)
                        If (item Is Nothing) Then
                            item = New ItemAllowNegate(Of CUser)
                            item.Item = user
                            items.Add(user.UserName, item)
                        End If
                        item.GroupAllow = Formats.ToBool(dbRis("Allow"))
                        item.GroupNegate = Formats.ToBool(dbRis("Negate"))
                    Next
                End If
            End While
            dbRis.Dispose()
            dbRis = Nothing

            dbRis = APPConn.ExecuteReader("SELECT * FROM [tbl_ComunicazioniXUtente] WHERE [Comunicazione] = " & GetID(Me))
            While dbRis.Read
                user = Sistema.Users.GetItemById(Formats.ToInteger(dbRis("Utente")))
                If (user IsNot Nothing AndAlso user.Stato = ObjectStatus.OBJECT_VALID) Then
                    item = items.GetItemByKey(user.UserName)
                    If (item Is Nothing) Then
                        item = New ItemAllowNegate(Of CUser)
                        item.Item = user
                        items.Add(user.UserName, item)
                    End If
                    item.UserAllow = Formats.ToBool(dbRis("Allow"))
                    item.UserNegate = Formats.ToBool(dbRis("Negate"))
                End If
            End While
            dbRis.Dispose()
            dbRis = Nothing

            For Each item In items
                If item.IsAllowed And Not item.IsNegated And item.Item.UserStato = UserStatus.USER_ENABLED Then
                    ret.Add(item.Item)
                End If
            Next
            ret.Comparer = New CUserComparer
            ret.Sort()

            Return ret

        End Function

        Public Function SetGroupAllowNegate(ByVal grpID As Integer, ByVal a As Boolean, ByVal n As Boolean) As Boolean
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            If (a <> n) Then
                dbSQL = "SELECT * FROM [tbl_ComunicazioniXGruppo] WHERE [Gruppo]=" & grpID & " AND [Comunicazione]=" & Me.ID & ";"
                dbRis = APPConn.ExecuteReader(dbSQL)
                If dbRis.Read Then
                    dbRis.Dispose()
                    dbSQL = "UPDATE [tbl_ComunicazioniXGruppo] SET [allow]=" & DBUtils.DBBool(a) & ", [negate]=" & DBUtils.DBBool(n) & " WHERE [Gruppo]=" & grpID & " AND [Comunicazione]=" & Me.ID & ";"
                    APPConn.ExecuteCommand(dbSQL)
                Else
                    dbRis.Dispose()
                    dbSQL = "INSERT INTO [tbl_ComunicazioniXGruppo] ([Gruppo], [Comunicazione], [allow], [negate]) VALUES (" & grpID & ", " & Me.ID & ", " & DBUtils.DBBool(a) & ", " & DBUtils.DBBool(n) & ");"
                    APPConn.ExecuteCommand(dbSQL)
                End If
                dbRis = Nothing
            Else
                APPConn.ExecuteCommand("DELETE * FROM [tbl_ComunicazioniXGruppo] WHERE [Gruppo]=" & grpID & " AND [Comunicazione]=" & Me.ID & ";")
            End If

            Return True
        End Function

        Public Function SetUserAllowNegate(ByVal usrID As Integer, ByVal allow As Boolean, ByVal negate As Boolean) As Boolean
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            If (allow <> negate) Then
                dbSQL = "SELECT * FROM [tbl_ComunicazioniXUtente] WHERE [Utente]=" & usrID & " AND [Comunicazione]=" & Me.ID & ";"
                dbRis = APPConn.ExecuteReader(dbSQL)
                If dbRis.Read Then
                    dbRis.Dispose()
                    dbSQL = "UPDATE [tbl_ComunicazioniXUtente] SET [allow]=" & DBUtils.DBBool(allow) & ", [negate]=" & DBUtils.DBBool(negate) & " WHERE [Utente]=" & usrID & " AND [Comunicazione]=" & Me.ID & ";"
                    APPConn.ExecuteCommand(dbSQL)
                Else
                    dbRis.Dispose()
                    dbSQL = "INSERT INTO [tbl_ComunicazioniXUtente] ([Utente], [Comunicazione], [allow], [negate]) VALUES (" & usrID & ", " & Me.ID & ", " & DBUtils.DBBool(allow) & ", " & DBUtils.DBBool(negate) & ");"
                    APPConn.ExecuteCommand(dbSQL)
                End If
                dbRis = Nothing
            Else
                APPConn.ExecuteCommand("DELETE * FROM [tbl_ComunicazioniXUtente] WHERE [Utente]=" & usrID & " AND [Comunicazione]=" & Me.ID & ";")
            End If

            Return True
        End Function

        Public Function IsUserAllowed(ByVal userId As Integer) As Boolean
            Throw New NotImplementedException
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Priorita", Me.m_Priorita)
            writer.WriteAttribute("Path", Me.m_Path)
            writer.WriteAttribute("PrimaPagina", Me.m_PrimaPagina)
            writer.WriteAttribute("Progressivo", Me.m_Progressivo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
            writer.WriteTag("Attachments", Me.Attachments)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Priorita" : Me.m_Priorita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Path" : Me.m_Path = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PrimaPagina" : Me.m_PrimaPagina = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Progressivo" : Me.m_Progressivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attachments" : Me.m_Attachments = New CCollection(Of CAttachment) : Me.m_Attachments.AddRange(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Function GetIndexedWords() As String() Implements IIndexable.GetIndexedWords
            Dim ret As New System.Collections.ArrayList
            Dim str As String = Replace(Me.Categoria & " " & Me.Descrizione & " " & Strings.RemoveHTMLTags(Me.Note), vbCrLf, " ")
            str = Replace(str, vbCr, " ")
            str = Replace(str, vbLf, " ")
            str = Replace(str, "  ", " ")

            Dim a() As String = Split(str, " ")
            If (Arrays.Len(a) > 0) Then ret.AddRange(a)

            Return ret.ToArray(GetType(String))
        End Function

        Public Function GetKeyWords() As String() Implements IIndexable.GetKeyWords
            Dim ret As New System.Collections.ArrayList
            ret.Add(Me.Categoria)

            Dim str As String = Replace(Me.Descrizione, vbCrLf, " ")
            str = Replace(str, vbCr, " ")
            str = Replace(str, vbLf, " ")
            str = Replace(str, "  ", " ")

            Dim a() As String = Split(str, " ")
            If (Arrays.Len(a) > 0) Then ret.AddRange(a)

            Return ret.ToArray(GetType(String))
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)

            If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                Office.Comunicazioni.Index.Index(Me)
            Else
                Office.Comunicazioni.Index.Unindex(Me)
            End If
        End Sub

    End Class



End Class



