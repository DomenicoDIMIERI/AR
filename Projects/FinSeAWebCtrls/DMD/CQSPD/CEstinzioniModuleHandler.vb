Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms

    Public Class CEstinzioniModuleHandler
        Inherits CBaseModuleHandler

        Private nomiLock As New Object
        Private m_Loaded As Boolean = False
        Private m_NomiAgenzie As String() = New String() {}
        Private m_NomiFiliali As String() = New String() {}

        Public Sub New()
            AddHandler CQSPD.Estinzioni.ItemCreated, AddressOf handleEstinzioneChanged
            AddHandler CQSPD.Estinzioni.ItemDeleted, AddressOf handleEstinzioneChanged
            AddHandler CQSPD.Estinzioni.ItemModified, AddressOf handleEstinzioneChanged
        End Sub

        Protected Overrides Sub Finalize()
            RemoveHandler CQSPD.Estinzioni.ItemCreated, AddressOf handleEstinzioneChanged
            RemoveHandler CQSPD.Estinzioni.ItemDeleted, AddressOf handleEstinzioneChanged
            RemoveHandler CQSPD.Estinzioni.ItemModified, AddressOf handleEstinzioneChanged
            MyBase.Finalize()
        End Sub

        Public Sub InvalidateNames()
            SyncLock Me.nomiLock
                Me.m_Loaded = False
            End SyncLock
        End Sub

        Private Sub checkLoaded()
            If Me.m_Loaded Then Return

            Dim dbRis As System.Data.IDataReader = Nothing
            Dim list As New System.Collections.ArrayList
            Dim text As String
            Try
                dbRis = CQSPD.Database.ExecuteReader("SELECT [NomeAgenzia] FROM [tbl_Estinzioni] WHERE [Stato]=1 GROUP BY [NomeAgenzia]")
                While dbRis.Read
                    text = Strings.Trim(Formats.ToString(dbRis("NomeAgenzia")))
                    If (text <> "") Then list.Add(text)
                End While
                dbRis.Dispose() : dbRis = Nothing
                m_NomiAgenzie = list.ToArray(GetType(String))
                Array.Sort(m_NomiAgenzie)

                dbRis = CQSPD.Database.ExecuteReader("SELECT [NomeFiliale] FROM [tbl_Estinzioni] WHERE [Stato]=1 GROUP BY [NomeFiliale]")
                list.Clear()
                While dbRis.Read
                    text = Strings.Trim(Formats.ToString(dbRis("NomeFiliale")))
                    If (text <> "") Then list.Add(text)
                End While
                dbRis.Dispose() : dbRis = Nothing
                m_NomiFiliali = list.ToArray(GetType(String))
                Array.Sort(m_NomiFiliali)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                'throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Me.m_Loaded = True

        End Sub

        Private Sub handleEstinzioneChanged(ByVal sender As Object, ByVal e As ItemEventArgs)
            SyncLock Me.nomiLock
                Me.checkLoaded()


                Dim item As CEstinzione = e.Item
                Dim i As Integer = Array.BinarySearch(m_NomiAgenzie, item.NomeAgenzia)
                If (i < 0) Then
                    i = Arrays.GetInsertPosition(m_NomiAgenzie, item.NomeAgenzia, 0, m_NomiAgenzie.Length)
                    m_NomiAgenzie = Arrays.Insert(m_NomiAgenzie, 0, m_NomiAgenzie.Length, item.NomeAgenzia, i)
                End If
                i = Array.BinarySearch(m_NomiFiliali, item.NomeFiliale)
                If (i < 0) Then
                    i = Arrays.GetInsertPosition(m_NomiFiliali, item.NomeFiliale, 0, m_NomiFiliali.Length)
                    m_NomiFiliali = Arrays.Insert(m_NomiFiliali, 0, m_NomiFiliali.Length, item.NomeFiliale, i)
                End If
            End SyncLock
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CEstinzioniCursor
        End Function

        Public Function GetPrestitiAttivi(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim at As Date? = RPC.n2date(Me.GetParameter(renderer, "at", ""))
            Dim col As CCollection(Of CEstinzione)
            Dim persona As CPersona = Anagrafica.Persone.GetItemById(pid)
            If (at.HasValue) Then
                col = CQSPD.Estinzioni.GetPrestitiAttivi(persona, at.Value)
            Else
                col = CQSPD.Estinzioni.GetPrestitiAttivi(persona)
            End If
            Return XML.Utils.Serializer.Serialize(col)
        End Function


        Public Function getEstinzioniByPersona(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            'Dim col As CEstinzioniPersona = DMD.CQSPD.Estinzioni.GetEstinzioniByPersona(pid)
            Dim col As New CCollection(Of CEstinzione)
            If (pid <> 0) Then
                Dim cursor As New CEstinzioniCursor
                Try
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.IDPersona.Value = pid
                    cursor.DataInizio.SortOrder = SortEnum.SORT_ASC
                    cursor.IgnoreRights = True
                    While Not cursor.EOF
                        col.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                End Try
            End If
            If col.Count > 0 Then
                Return XML.Utils.Serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function GetEstinzioniXEstintore(ByVal renderer As Object) As String
            Dim idEstintore As Integer = RPC.n2int(GetParameter(renderer, "ei", ""))
            Dim tipoEstintore As String = RPC.n2str(GetParameter(renderer, "et", ""))
            Dim items As CCollection(Of EstinzioneXEstintore) = DMD.CQSPD.Estinzioni.GetEstinzioniXEstintore(idEstintore, tipoEstintore)
            If (items.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetEstinzioniByPratica(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim items As CCollection(Of CEstinzione) = CQSPD.Estinzioni.GetEstinzioniByPratica(pid)
            Return XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
        End Function

        Public Function CreateElencoRichiesteDiFinanziamento(ByVal renderer As Object) As String
            Dim idCliente As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", "0"))
            Return Utils.CQSPDUtils.CreateElencoRichiesteDiFinanziamento(idCliente, id)
        End Function

        Public Function CheckCanDelete(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "ei", "0"))
            Return CQSPD.Estinzioni.CanDelete(id)
        End Function

        Private Function IsLikeName(ByVal nome As String, ByVal strFind As String) As Boolean
            nome = Strings.OnlyChars(nome)
            strFind = Strings.OnlyChars(strFind)
            If (String.IsNullOrEmpty(nome) OrElse String.IsNullOrEmpty(strFind)) Then Return False
            Return InStr(nome, strFind, CompareMethod.Text) > 0
        End Function

        Public Function GetNomiAgenzie(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "_q", "")))
            If (Len(text) < 3) Then Return ""

            Me.checkLoaded()

            Dim ret As New System.Text.StringBuilder
            ret.Append("<list>")
            SyncLock Me.nomiLock
                For Each nomeAgenzia In Me.m_NomiAgenzie
                    If (Me.IsLikeName(nomeAgenzia, text)) Then
                        ret.Append("<item>")
                        ret.Append("<text>")
                        ret.Append(Strings.HtmlEncode(nomeAgenzia))
                        ret.Append("</text>")
                        ret.Append("<value>")
                        ret.Append(Strings.HtmlEncode(nomeAgenzia))
                        ret.Append("</value>")
                        ret.Append("</item>")
                    End If
                Next
            End SyncLock

            ret.Append("</list>")

            Return ret.ToString
        End Function

        Public Function GetNomiFiliali(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "_q", "")))
            If (Len(text) < 3) Then Return ""

            Me.checkLoaded()

            Dim ret As New System.Text.StringBuilder
            ret.Append("<list>")
            SyncLock Me.nomiLock
                For Each nomeFiliale In Me.m_NomiFiliali
                    If (Me.IsLikeName(nomeFiliale, text)) Then
                        ret.Append("<item>")
                        ret.Append("<text>")
                        ret.Append(Strings.HtmlEncode(nomeFiliale))
                        ret.Append("</text>")
                        ret.Append("<value>")
                        ret.Append(Strings.HtmlEncode(nomeFiliale))
                        ret.Append("</value>")
                        ret.Append("</item>")
                    End If
                Next
            End SyncLock
            ret.Append("</list>")

            Return ret.ToString
        End Function


    End Class


End Namespace