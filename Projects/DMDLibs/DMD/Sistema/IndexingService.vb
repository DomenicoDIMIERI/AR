Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Sistema

Namespace Internals

    Public Class CIndexingService
        Public ReadOnly lockObject As New Object


        Const chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"

#Region "Word"

        ''' <summary>
        ''' Rappresenta una parola indicizzata
        ''' </summary>
        ''' <remarks></remarks>
        Public Class CachedWord
            Inherits DBObjectBase
            Implements IComparable

            Private m_Owner As CIndexingService
            Private m_Word As String
            Private m_LastUsed As Date
            Private m_Frequenza As Nullable(Of Integer)
            Private m_ConteggioUtilizzi As Integer
            Private m_Indexes() As Integer

            Public Sub New(ByVal owner As CIndexingService)
                DMD.DMDObject.IncreaseCounter(Me)
                If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
                Me.m_Owner = owner
                Me.m_Word = ""
                Me.m_LastUsed = Nothing
                Me.m_Frequenza = Nothing
                Me.m_ConteggioUtilizzi = 0
                Me.m_Indexes = Nothing
            End Sub

            Public Sub New(ByVal owner As CIndexingService, ByVal word As String)
                Me.New(owner)
                Me.m_Word = Left(Trim(UCase(word)), 255)
            End Sub

            Public ReadOnly Property Owner As CIndexingService
                Get
                    Return Me.m_Owner
                End Get
            End Property

            Protected Friend Sub SetOwner(ByVal owner As CIndexingService)
                Me.m_Owner = owner
            End Sub

            Public ReadOnly Property Word As String
                Get
                    Return Me.m_Word
                End Get
            End Property

            Public ReadOnly Property GetIndice As Integer()
                Get
                    SyncLock Me
                        If Me.m_Frequenza.HasValue = False Then
                            Me.Rebuild()
                            Me.Save()
                        End If
                        If Me.m_Indexes Is Nothing Then Me.m_Indexes = New Integer() {}
                        Return Me.m_Indexes
                    End SyncLock
                End Get
            End Property

            Public Function GetIndexedPosition(ByVal objectID As Integer) As Integer
                SyncLock Me
                    Return Sistema.Arrays.BinarySearch(Me.GetIndice, objectID)
                End SyncLock
            End Function

            Public Function IsIndexed(ByVal objectID As Integer) As Boolean
                Return Me.GetIndexedPosition(objectID) >= 0
            End Function

            Public Function Indicizza(ByVal objectID As Integer) As Integer
                SyncLock Me
                    Dim p As Integer = Me.GetIndexedPosition(objectID)
                    If (p >= 0) Then Return p

                    Me.m_Indexes = Me.GetIndice
                    p = Sistema.Arrays.GetInsertPosition(Me.m_Indexes, objectID, 0, Me.m_Indexes.Length)
                    Me.m_Indexes = Arrays.Insert(Me.m_Indexes, 0, Me.m_Indexes.Length, objectID, p)
#If DEBUG Then
                    If (p > 0 AndAlso p < UBound(Me.m_Indexes)) Then
                        Debug.Assert(Me.m_Indexes(p - 1) < Me.m_Indexes(p))
                        Debug.Assert(Me.m_Indexes(p) < Me.m_Indexes(p + 1))
                    ElseIf (p > 0) Then
                        Debug.Assert(Me.m_Indexes(p - 1) < Me.m_Indexes(p))
                    ElseIf (p < UBound(Me.m_Indexes)) Then
                        Debug.Assert(Me.m_Indexes(p) < Me.m_Indexes(p + 1))
                    End If

#End If
                    Me.m_Frequenza = Arrays.Len(Me.m_Indexes)
                    Me.SetChanged(True)
                    Return p

                End SyncLock
            End Function

            Public Function UnIndicizza(ByVal objectID As Integer) As Integer
                SyncLock Me
                    Dim p As Integer = Me.GetIndexedPosition(objectID)
                    If (p < 0) Then Return -1

                    Me.m_Indexes = Arrays.RemoveAt(Me.m_Indexes, p)
                    Me.m_Frequenza = Arrays.Len(Me.m_Indexes)

                    Me.SetChanged(True)

                    Return p
                End SyncLock
            End Function

            Public Sub Rebuild()
                SyncLock Me
                    Dim da As System.Data.OleDb.OleDbDataAdapter = Nothing
                    Dim dt As DataTable = Nothing
                    Try
                        If Me.Owner Is Nothing Then Throw New ArgumentException("Owner")
                        Dim conn As CDBConnection = Me.Owner.Database
                        If (conn Is Nothing) Then Throw New ArgumentNullException("conn")
                        Dim dbSQL1 As String = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word]=" & DBUtils.DBString(Me.m_Word) & " ORDER BY [ObjectID] ASC"
                        da = New System.Data.OleDb.OleDbDataAdapter(dbSQL1, DirectCast(conn.GetConnection, OleDb.OleDbConnection))
                        dt = New DataTable
                        da.Fill(dt)
                        da.Dispose() : da = Nothing

                        If (dt.Rows.Count > 0) Then
                            ReDim Me.m_Indexes(dt.Rows.Count - 1)
                            For i As Integer = 0 To dt.Rows.Count - 1
                                With dt.Rows(i)
                                    Me.m_Indexes(i) = Formats.ToInteger(.Item("ObjectID"))
                                End With
                            Next
                        Else
                            Me.m_Indexes = New Integer() {}
                        End If
                        dt.Dispose() : dt = Nothing

                        Me.m_Frequenza = Arrays.Len(Me.m_Indexes)
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                        Throw
                    Finally
                        If (dt IsNot Nothing) Then dt.Dispose() : dt = Nothing
                        If (da IsNot Nothing) Then da.Dispose() : da = Nothing
                    End Try
                End SyncLock
            End Sub

            Public Sub Load()
                Dim dbRis As System.Data.IDataReader = Nothing
                Try
                    If (Me.Owner Is Nothing) Then Throw New ArgumentNullException("Owner")
                    Dim conn As CDBConnection = Me.Owner.Database
                    If (conn Is Nothing) Then Throw New ArgumentNullException("conn")
                    Dim dbSQL1 As String = "SELECT * FROM [tbl_WordStats] WHERE [Word]=" & DBUtils.DBString(Me.m_Word)
                    dbRis = conn.ExecuteReader(dbSQL1)
                    If (dbRis.Read) Then
                        Me.LoadFromRecordset(New DBReader(dbRis))
                    Else
                        Me.m_Frequenza = 0
                        Me.m_Indexes = New Integer() {}
                    End If

                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try
            End Sub

            Private Function GetWordIndexFileName() As String
                If (GetID(Me) = 0) Then Return ""
                Return System.IO.Path.Combine(Me.Owner.WordIndexFolder, "W" & GetID(Me) & ".idx")
            End Function

            Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
                Me.m_Word = reader.Read("Word", Me.m_Word)
                If (Me.Owner.WordIndexFolder = "") Then Me.m_Indexes = reader.Read("Indice", Me.m_Indexes)
                Me.m_Frequenza = reader.Read("Frequenza", Me.m_Frequenza)
                Dim ret As Boolean = MyBase.LoadFromRecordset(reader)
                If (Me.Owner.WordIndexFolder <> "") Then
                    Dim fileName As String = Me.GetWordIndexFileName
                    If (System.IO.File.Exists(fileName)) Then
                        Me.m_Indexes = Me.LoadIndexFromFile(fileName) ' reader.Read("Indice", Me.m_Indexes)
                    Else
                        Me.m_Indexes = {}
                    End If
                End If
                Return ret
            End Function

            Private Function LoadIndexFromFile(ByVal fileName As String) As Integer()
                Dim stream As System.IO.FileStream = Nothing
                Dim reader As System.IO.BinaryReader = Nothing
                Dim bytes() As Byte = Nothing
                Dim ret As Integer() = {}
                Try
                    stream = New System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None)
                    If (stream.Length > 0) Then
                        reader = New System.IO.BinaryReader(stream)
                        bytes = Array.CreateInstance(GetType(Byte), stream.Length)
                        reader.Read(bytes, 0, bytes.Length)
                        ret = Array.CreateInstance(GetType(Integer), CInt(bytes.Length / 4))
                        Buffer.BlockCopy(bytes, 0, ret, 0, bytes.Length)
                    End If
                    Return ret
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (reader IsNot Nothing) Then reader = Nothing
                    If (stream IsNot Nothing) Then stream.Dispose() : stream = Nothing
                    If (bytes IsNot Nothing) Then Erase bytes
                End Try
            End Function

            Private Sub SaveIndexToFile(ByVal fileName As String)
                Dim stream As System.IO.FileStream = Nothing
                Dim writer As System.IO.BinaryWriter = Nothing
                Dim bytes() As Byte = Nothing
                Try
                    stream = New System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)
                    writer = New System.IO.BinaryWriter(stream)
                    'writer.Write()
                    bytes = Array.CreateInstance(GetType(Byte), Me.m_Indexes.Length * 4)
                    Buffer.BlockCopy(Me.m_Indexes, 0, bytes, 0, bytes.Length)
                    writer.Write(bytes, 0, bytes.Length)

                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (writer IsNot Nothing) Then writer = Nothing
                    If (stream IsNot Nothing) Then stream.Dispose() : stream = Nothing
                    If (bytes IsNot Nothing) Then Erase bytes
                End Try
            End Sub

            Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
                writer.Write("Word", Me.m_Word)
                If (Me.Owner.WordIndexFolder <> "") Then
                    writer.Write("Indice", DBNull.Value)
                Else
                    writer.Write("Indice", Me.GetIndice)
                End If
                writer.Write("Frequenza", Me.m_Frequenza)
                Return MyBase.SaveToRecordset(writer)
            End Function

            Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
                Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
                If (ret AndAlso Me.Owner.WordIndexFolder <> "") Then
                    Dim fileName As String = Me.GetWordIndexFileName
                    'DMD.Sistema.FileSystem.CreateFolder(Me.Owner.WordIndexFolder)
                    Me.SaveIndexToFile(fileName)
                End If
                Return ret
            End Function


            Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
                Dim other As CachedWord = obj
                Dim ret As Integer = -DateUtils.Compare(Me.m_LastUsed, other.m_LastUsed)
                If (ret = 0) Then ret = other.m_ConteggioUtilizzi - Me.m_ConteggioUtilizzi
                Return ret
            End Function

            Protected Friend Overrides Function GetConnection() As CDBConnection
                Return Me.Owner.Database
            End Function

            Public Overrides Function GetModule() As CModule
                Return Nothing
            End Function

            Public Overrides Function GetTableName() As String
                Return "tbl_WordStats"
            End Function

            Sub Consume()
                Me.m_LastUsed = Now
                Me.m_ConteggioUtilizzi += 1
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class


#End Region

        Private m_Db As CDBConnection = Nothing
        Private m_MAXCACHESIZE As Integer = 100
        Private m_UnloadFactor As Single = 0.25
        Private m_WordIndexFolder As String = ""
        Private m_CachedWords As CKeyCollection(Of CachedWord) = Nothing    'Collezione delle parole indicizzate



        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal conn As CDBConnection)
            Me.New()
            If (conn Is Nothing) Then Throw New ArgumentNullException("conn")
            Me.m_Db = conn
        End Sub




        ''' <summary>
        ''' Restituisce o imposta la collezione delle parole indicizzate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CachedWords As CKeyCollection(Of CachedWord)
            Get
                SyncLock Me.lockObject
                    If (Me.m_CachedWords Is Nothing) Then
                        Me.m_CachedWords = New CKeyCollection(Of CachedWord)
                    End If
                    Return Me.m_CachedWords
                End SyncLock
            End Get
        End Property

        Public Function GetCachedWord(ByVal word As String) As CachedWord
            SyncLock Me.lockObject
                word = Strings.Left(Trim(word), 255)
                Dim ret As CachedWord = Me.CachedWords.GetItemByKey(word)
                If (ret Is Nothing) Then
                    ret = Me.GetNonChangedWord(word)
                    Me.AddToCache(ret)
                End If
                ret.Consume()
                Return ret
            End SyncLock
        End Function

        Protected Overridable Function GetNonChangedWord(ByVal word As String) As CachedWord
            Dim ret As New CachedWord(Me, word)
            ret.Load()
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o impsota la connessione al database che contiene le tabelle per l'indicizzazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Database As CDBConnection
            Get
                Return Me.m_Db
            End Get
            Set(value As CDBConnection)
                Me.m_Db = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso in cui vengono salvati gli indici relativi a ciascuna parola memorizzata
        ''' Se il percorso è vuoto gli indici vengono memorizzati nel database
        ''' </summary>
        ''' <returns></returns>
        Public Property WordIndexFolder As String
            Get
                Return Me.m_WordIndexFolder
            End Get
            Set(value As String)
                Me.m_WordIndexFolder = Strings.Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la dimensione massima della cache
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MaxCacheSize As Integer
            Get
                Return Me.m_MAXCACHESIZE
            End Get
            Set(value As Integer)
                If value < 0 Then
                    Throw New ArgumentOutOfRangeException("MaxCacheSize")
                End If

                If (Me.m_MAXCACHESIZE = value) Then Exit Property

                SyncLock Me.lockObject
                    Me.m_MAXCACHESIZE = value
                    If (Me.m_CachedWords IsNot Nothing) Then
                        If (Me.m_MAXCACHESIZE > 0) Then
                            While (Me.m_CachedWords.Count > Me.m_MAXCACHESIZE)
                                Me.m_CachedWords.RemoveAt(Me.m_MAXCACHESIZE)
                            End While
                        ElseIf (Me.m_MAXCACHESIZE = 0) Then
                            Me.m_CachedWords.Clear()
                        End If
                    End If
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale della cache che viene svuotata quando si supera la dimensione massima
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UnloadFactor As Single
            Get
                Return Me.m_UnloadFactor
            End Get
            Set(value As Single)
                If (Me.m_UnloadFactor = value) Then Exit Property

                SyncLock Me.lockObject
                    If Math.Floor(value * Me.MaxCacheSize) < 1 Then
                        Throw New ArgumentNullException("UnloadFactor")
                    End If
                    Me.m_UnloadFactor = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Restituisce vero l'oggetto è stato indicizzato
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsIndexed(ByVal o As Object) As Boolean
            Dim objectID As Integer = GetID(o)
            Dim dbSQL As String
            dbSQL = "SELECT Count(*) FROM [tbl_Index] WHERE [ObjectID]=" & DBUtils.DBNumber(objectID)
            Dim value As Nullable(Of Integer) = Formats.ParseInteger(Me.Database.ExecuteScalar(dbSQL))
            Return (value.HasValue AndAlso value.Value > 0)
        End Function


        Private Function IsIndexedWord(ByVal items() As String, ByVal w As String) As Boolean
            Return Arrays.BinarySearch(items, 0, Arrays.Len(items), w) >= 0
        End Function

        ''' <summary>
        ''' Rimuove gli indici relativi all'oggetto
        ''' </summary>
        ''' <param name="o"></param>
        ''' <remarks></remarks>
        Public Sub Unindex(ByVal o As Object)
            SyncLock Me.lockObject
                Dim objectID As Integer = GetID(o)
                Dim words() As WordInfo = Me.GetIndexedWords(o)
                Dim i As Integer


                If (words IsNot Nothing) Then
                    Me.GetWordStats(words)
                    For i = 0 To UBound(words)
                        words(i).Frequenza -= 1
                        'If (Me.m_CachedWords IsNot Nothing) Then
                        'Dim item As CachedWord = Me.m_CachedWords.GetItemByKey(words(i).Word)
                        'If item Is Nothing Then item = Me.CachedWords.L
                        'If (item IsNot Nothing) Then
                        'Dim j As Integer = Arrays.BinarySearch(item.Indexes, 0, Arrays.Len(item.Indexes), objectID)
                        'If (j >= 0) Then item.Indexes = Arrays.RemoveAt(item.Indexes, j, 1)
                        'End If
                        'End If
                        'Me.Database.ExecuteCommand("UPDATE [tbl_WordStats] Set [Frequenza]= " & words(i).Frequenza & " WHERE [Word]=" & DBUtils.DBString(words(i).Word))
                        Dim cw As CachedWord = Me.GetCachedWord(words(i).Word)
                        cw.UnIndicizza(objectID)
                        cw.Save(True)

                        Try
                            Me.Database.ExecuteCommand("DELETE * FROM [tbl_Index] WHERE [ObjectID]=" & DBUtils.DBNumber(objectID))
                        Catch ex As Exception

                        End Try
                    Next
                End If
            End SyncLock
        End Sub

        ''' <summary>
        ''' Restituisce le parole indicizzate per l'oggetto
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetIndexedWords(ByVal o As Object) As WordInfo()
            Dim objectID As Integer = GetID(o)
            Dim dbSQL As String = "SELECT * FROM [tbl_Index] WHERE [ObjectID]=" & DBUtils.DBNumber(objectID)
            Dim da As New System.Data.OleDb.OleDbDataAdapter(dbSQL, DirectCast(Me.Database.GetConnection, OleDb.OleDbConnection))
            Dim dt As New DataTable
            da.Fill(dt)
            da.Dispose()
            Dim ret() As WordInfo = Nothing
            If (dt.Rows.Count > 0) Then
                ReDim ret(dt.Rows.Count - 1)
                For i As Integer = 0 To dt.Rows.Count - 1
                    ret(i) = New WordInfo
                    With dt.Rows(i)
                        ret(i).Word = Formats.ToString(.Item("Word"))
                        ret(i).Frequenza = Formats.ToInteger(.Item("Rank"))
                    End With
                Next
            End If
            dt.Dispose()
            Return ret
        End Function

        Public Function MergeWords(ByVal words() As String) As WordInfo()
            Dim ret As New CKeyCollection(Of WordInfo)
            Me.MergeWords(ret, words)
            If (ret.Count > 1) Then
                ret(ret.Count - 1).IsLike = Len(ret(ret.Count - 1).Word) >= 3
            End If
            Return ret.ToArray
        End Function

        Private Sub MergeWords(ByVal ret As CKeyCollection(Of WordInfo), ByVal words() As String)
            Dim info As WordInfo
            For i = 0 To Arrays.Len(words) - 1
                words(i) = UCase(Trim(words(i)))
                words(i) = Replace(words(i), vbCrLf, " ")
                words(i) = Replace(words(i), vbCr, " ")
                words(i) = Replace(words(i), vbLf, " ")
                words(i) = Replace(words(i), "  ", " ")
                Dim w() As String = Split(words(i), " ")
                For j As Integer = 0 To Arrays.Len(w) - 1
                    w(j) = Strings.OnlyCharsAndNumbers(w(j))

                    If (w(j) <> "") Then
                        info = ret.GetItemByKey(w(j))
                        If (info Is Nothing) Then
                            info = New WordInfo
                            info.Word = w(j)
                            info.Position = i
                            ret.Add(w(j), info)
                        End If
                        info.Frequenza += 1
                    End If
                Next
            Next
        End Sub

        Public Function GetIndexableWords(ByVal o As Object) As WordInfo()
            'Me.Index(o, Me.SplitWords(text))
            Dim ret As New CKeyCollection(Of WordInfo)
            Dim tmpWords As String() = DirectCast(o, IIndexable).GetIndexedWords
            Dim words As New System.Collections.ArrayList
            For Each s As String In tmpWords
                words.Add(s)
                Dim tmp As String = ""
                For i As Integer = 1 To Len(s)
                    Dim ch As String = Mid(s, i, 1)
                    If (InStr(chars, ch) > 0) Then
                        tmp &= ch
                    Else
                        If (tmp <> "") Then
                            words.Add(tmp)
                            tmp = ""
                        End If
                    End If
                Next
                If (tmp <> "") Then words.Add(tmp)
            Next

            Me.MergeWords(ret, words.ToArray(GetType(String)))

            Dim ret1 As New CKeyCollection(Of WordInfo)
            Me.MergeWords(ret1, DirectCast(o, IIndexable).GetKeyWords)
            For Each w As WordInfo In ret
                If ret1.ContainsKey(w.Word) Then
                    ret1.RemoveByKey(w.Word)
                End If
            Next

            For Each w As WordInfo In ret1
                Dim info As WordInfo = ret.GetItemByKey(w.Word)
                If (info Is Nothing) Then
                    ret.Add(w.Word, w)
                Else
                    info.Frequenza += 1
                End If
            Next

            Return ret.ToArray
        End Function

        ''' <summary>
        ''' Indicizza l'oggetto
        ''' </summary>
        ''' <param name="o"></param>
        ''' <remarks></remarks>
        Public Sub Index(ByVal o As Object)
            Me.Index(o, Me.GetIndexableWords(o))
        End Sub


        Private Sub Index(ByVal o As Object, ByVal words() As WordInfo, Optional ByVal force As Boolean = False)
            SyncLock Me.lockObject
                Dim objectID As Integer = GetID(o)

                'Eliminiamo le vecchie parole
                'Me.Unindex(o)
                Dim oldWords() As WordInfo = Me.GetIndexedWords(o)

                'Dim joinWords As New System.Collections.ArrayList
                'Dim wordsToAdd As New System.Collections.ArrayList
                Dim i As Integer = 0
                If (Not force) Then
                    While (i < Arrays.Len(words))
                        Dim i1 As Integer = -1
                        For j As Integer = 0 To Arrays.Len(oldWords) - 1
                            If (oldWords(j).Word = words(i).Word) Then
                                i1 = j
                                Exit For
                            End If
                        Next
                        If i1 >= 0 Then
                            'joinWords.Add(words(i))
                            oldWords = Arrays.RemoveAt(oldWords, i1)
                            words = Arrays.RemoveAt(words, i)
                        Else
                            i += 1
                        End If
                    End While
                End If

                'Eliminiamo le parole non più indicizzate
                For i = 0 To Arrays.Len(oldWords) - 1
                    Dim word As String = UCase(Trim(oldWords(i).Word))
                    If (word <> "") Then
                        oldWords(i).Frequenza -= 1

                        Dim cw As CachedWord = Me.GetCachedWord(word)
                        SyncLock cw
                            cw.UnIndicizza(objectID)
                            cw.Save(True)
                        End SyncLock
                        Try
                            Me.Database.ExecuteCommand("DELETE * FROM [tbl_Index] WHERE [ObjectID]=" & DBUtils.DBNumber(objectID) & " AND [Word]=" & DBUtils.DBString(word))
                        Catch ex As Exception

                        End Try
                    End If
                Next

                'Inseriamo le nuove parole nel database
                For i = 0 To Arrays.Len(words) - 1
                    Dim word As String = UCase(Trim(words(i).Word))
                    If (word <> "") Then
                        words(i).Frequenza += 1

                        Dim cw As CachedWord = Me.GetCachedWord(word)
                        SyncLock cw
                            cw.Indicizza(objectID)
                            cw.Save(True)
                        End SyncLock
                        Try
                            Me.Database.ExecuteCommand("INSERT INTO [tbl_Index] ([ObjectID], [Word], [Rank]) VALUES (" & DBUtils.DBNumber(objectID) & ", " & DBUtils.DBString(word) & ", " & DBUtils.DBNumber(-i) & ")")
                        Catch ex As Exception
                            Debug.Print(ex.Message)
                        End Try
                    End If
                Next




            End SyncLock
        End Sub


        Public Class CResult
            Implements IComparable

            Public OwnerID As Integer
            Public Count As Integer

            Public Sub New(ByVal ownerID As Integer)
                Me.OwnerID = ownerID
                Me.Count = 1
            End Sub

            Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
                Return DirectCast(obj, CResult).Count - Me.Count
            End Function
        End Class

        Public Class WordInfo
            Implements IComparable, ICloneable

            Public Word As String
            Public Position As Integer
            Public IsLike As Boolean = False
            Public Frequenza As Integer
            Public IsNew As Boolean = False

            Public Function CompareTo(ByVal o As Object) As Integer Implements IComparable.CompareTo
                Dim other As WordInfo = o
                Dim a1 As Integer = IIf(Me.IsLike, 1, 0)
                Dim a2 As Integer = IIf(other.IsLike, 1, 0)
                Dim ret As Integer = a1 - a2
                If (ret = 0) Then ret = Me.Frequenza - other.Frequenza
                If (ret = 0) Then ret = Len(other.Word) - Len(Me.Word)
                Return ret
            End Function

            Public Function Clone() As Object Implements ICloneable.Clone
                Return Me.MemberwiseClone
            End Function
        End Class

        Public Function SplitWords(ByVal text As String) As WordInfo()
            text = UCase(text)
            text = Replace(text, "  ", " ")
            'text = Replace(text, "'", "")
            'text = Replace(text, " ", ";")
            'text = Replace(text, vbCr, ";")
            'text = Replace(text, vbLf, ";")

            Dim tmp As String = ""
            For i As Integer = 1 To Len(text)
                Dim ch As String = Mid(text, i, 1)
                If (InStr(chars, ch) > 0) Then
                    tmp &= ch
                ElseIf ch = " " OrElse ch = ";" Then
                    tmp &= ";"
                Else
                    tmp &= ""
                End If
            Next
            text = tmp

            Dim ret As New CKeyCollection(Of WordInfo)
            Dim tokenizer As New DMD.Text.StringTokenizer(text, ";")
            While tokenizer.hasMoreTokens
                Dim token As String = tokenizer.nextToken
                Dim item As WordInfo

                If (tokenizer.hasMoreTokens = False) Then
                    item = ret.GetItemByKey(token)
                    If (item Is Nothing) Then
                        item = New WordInfo
                        ret.Add(token, item)
                        item.Word = token
                        item.Frequenza += 1
                        item.IsLike = True
                    Else
                        item = New WordInfo
                        ret.Add(token, item)
                        item.Word = token
                        item.Frequenza += 1
                        item.IsLike = True
                    End If
                Else
                    item = ret.GetItemByKey(token)
                    If (item Is Nothing) Then
                        item = New WordInfo
                        ret.Add(token, item)
                    End If
                    item.Word = token
                    item.Frequenza += 1
                End If
            End While

            Return ret.ToArray
        End Function

        'Private Function ParseWords(ByVal items() As String) As String()
        '    Dim tmp As New CKeyCollection(Of String)
        '    For i As Integer = 0 To UBound(items)
        '        items(i) = Strings.OnlyChars(UCase(items(i)))
        '        If tmp.ContainsKey(items(i)) = False Then tmp.Add(items(i), items(i))
        '    Next
        '    Dim ret() As String = Nothing
        '    If (tmp.Count > 0) Then
        '        ReDim ret(tmp.Count - 1)
        '        tmp.CopyTo(ret, 0)
        '    End If
        '    Return ret
        'End Function

        Private Sub GetWordStats(ByVal words() As WordInfo)
            Dim dbSQL As String = "SELECT * FROM [tbl_WordStats] WHERE [Word] In ("
            For i As Integer = 0 To UBound(words)
                words(i).Frequenza = 0
                words(i).IsNew = True
                If (i > 0) Then dbSQL &= ","
                dbSQL &= DBUtils.DBString(words(i).Word)
            Next
            dbSQL &= ")"
            Dim da As New System.Data.OleDb.OleDbDataAdapter(dbSQL, DirectCast(Me.Database.GetConnection, OleDb.OleDbConnection))
            Dim dt As New DataTable
            da.Fill(dt)
            da.Dispose()

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim w As String = Formats.ToString(dt.Rows(i).Item("Word"))
                For j As Integer = 0 To UBound(words)
                    If (w = words(j).Word) Then
                        words(j).Frequenza = Formats.ToInteger(dt.Rows(i).Item("Frequenza"))
                        words(j).IsNew = False
                        Exit For
                    End If
                Next
            Next
            dt.Dispose()
        End Sub





        '#If 1 Then

        '        Private Function GetSQL1(ByVal word As WordInfo) As String
        '            If (Len(word.Word) > 3) Then
        '                Return "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] Like " & DBUtils.DBString(word.Word & "%")
        '            Else
        '                Return "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] = " & DBUtils.DBString(word.Word)
        '            End If
        '        End Function

        '        Private Function GetSQL2(ByVal word1 As WordInfo, ByVal word2 As WordInfo) As String
        '            Dim dbSQL As String = ""
        '            If (Len(word2.Word) > 3) Then
        '                dbSQL = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] Like " & DBUtils.DBString(word2.Word & "%")
        '            Else
        '                dbSQL = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] = " & DBUtils.DBString(word2.Word)
        '            End If
        '            dbSQL = "SELECT [tbl_Index].[ObjectID] FROM [tbl_Index] INNER JOIN (" & dbSQL & ") AS [T1] ON [tbl_Index].[ObjectID]=[T1].[ObjectID] WHERE [tbl_Index].[Word]=" & DBUtils.DBString(word1.Word)
        '            Return dbSQL
        '        End Function

        '        Private Function GetSQL3(ByVal word1 As WordInfo, ByVal word2 As WordInfo, ByVal word3 As WordInfo) As String
        '            Dim dbSQL As String = ""
        '            'If (Len(word3) > 3) Then
        '            '    dbSQL = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] Like " & DBUtils.DBString(word3 & "%")
        '            'Else
        '            '    dbSQL = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] = " & DBUtils.DBString(word3)
        '            'End If
        '            'dbSQL = "SELECT [tbl_Index].[ObjectID] FROM [tbl_Index] INNER JOIN (" & dbSQL & ") AS [T1] ON [tbl_Index].[ObjectID]=[T1].[ObjectID] WHERE [tbl_Index].[Word]=" & DBUtils.DBString(word2)
        '            'dbSQL = "SELECT [tbl_Index].[ObjectID] FROM [tbl_Index] INNER JOIN (" & dbSQL & ") AS [T2] ON [tbl_Index].[ObjectID]=[T2].[ObjectID] WHERE [tbl_Index].[Word]=" & DBUtils.DBString(word1)

        '            dbSQL &= "SELECT [T3].[ObjectID] FROM "
        '            dbSQL &= "(SELECT [T1].[ObjectID] FROM "
        '            dbSQL &= "(SELECT [ObjectID] FROM [tbl_Index] WHERE [Word]=" & DBUtils.DBString(word1.Word) & ")  AS [T1] "
        '            dbSQL &= "INNER JOIN "
        '            dbSQL &= "(SELECT [ObjectID] FROM [tbl_Index] WHERE [Word]=" & DBUtils.DBString(word2.Word) & ") AS [T2] "
        '            dbSQL &= "ON [T1].[ObjectID]=[T2].[ObjectID]) As [T3] "
        '            dbSQL &= "INNER JOIN ("
        '            If (Len(word3.Word) > 3) Then
        '                dbSQL &= "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] Like " & DBUtils.DBString(word3.Word & "%")
        '            Else
        '                dbSQL &= "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] = " & DBUtils.DBString(word3.Word)
        '            End If
        '            dbSQL &= ") AS [T4] "
        '            dbSQL &= "ON [T3].[ObjectID]=[T4].[ObjectID]"

        '            Return dbSQL
        '        End Function

        '        Private Function GetSQLN(ByVal words() As WordInfo) As String
        '            Dim dbSQL As String = ""
        '            Dim word As String = Trim(words(0).Word)
        '            Dim localQuery As String = ""
        '            Dim i As Integer

        '            dbSQL = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] = " & DBUtils.DBString(word)

        '            For i = 1 To Math.Min(10, UBound(words) - 1)
        '                word = Trim(words(i).Word)
        '                If (word = "") Then Continue For
        '                localQuery = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] = " & DBUtils.DBString(word)
        '                dbSQL = "SELECT [TA" & i & "].* FROM (" & dbSQL & ") As [TA" & i & "] INNER JOIN (" & localQuery & ") As [TB" & i & "] ON [TA" & i & "].[ObjectID]=[TB" & i & "].[ObjectID]"
        '            Next

        '            If (i = UBound(words)) Then
        '                word = Trim(words(i).Word)
        '                If (Len(word) > 3) Then
        '                    localQuery = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] Like " & DBUtils.DBString(word & "%")
        '                Else
        '                    localQuery = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] =" & DBUtils.DBString(word)
        '                End If
        '                dbSQL = "SELECT [TA" & i & "].* FROM (" & dbSQL & ") As [TA" & i & "] INNER JOIN (" & localQuery & ") As [TB" & i & "] ON [TA" & i & "].[ObjectID]=[TB" & i & "].[ObjectID]"
        '            End If

        '            Return dbSQL
        '        End Function

        Public Function Find(ByVal words() As WordInfo) As Integer()
            Dim ret() As Integer = Nothing
            For i As Integer = 0 To UBound(words)
                ret = Me.Find(words(i), ret)
                If (ret Is Nothing) OrElse (UBound(ret) = -1) Then Return ret
            Next
            Return ret
        End Function

        Public Function Find(ByVal words() As WordInfo, ByVal findIn() As Integer) As Integer()
            Dim ret() As Integer = findIn
            For i As Integer = 0 To UBound(words)
                ret = Me.Find(words(i), ret)
                If (ret Is Nothing) OrElse (UBound(ret) = -1) Then Return ret
            Next
            Return ret
        End Function

        Private Function Join(ByVal arr() As Integer) As String
            Dim ret As New System.Text.StringBuilder
            For i As Integer = 0 To arr.Length - 1
                If (i > 0) Then ret.Append(",")
                ret.Append(DBUtils.DBNumber(arr(i)))
            Next
            Return ret.ToString
        End Function

        Public Function Find(ByVal word As WordInfo, ByVal arr() As Integer) As Integer()
            Dim ret() As Integer = Nothing
            If (word.IsLike) Then
                Dim dbSQL As String
                Dim arr1() As Integer = Nothing

                dbSQL = "SELECT [ObjectID] FROM [tbl_Index] WHERE [Word] Like '" & Replace(word.Word, "'", "''") & "%'"
                If (arr IsNot Nothing) Then
                    If arr.Length = 0 Then
                        dbSQL &= " AND (0<>0) "
                    Else
                        dbSQL &= " AND [ObjectID] In (" & Me.Join(arr) & ")"
                    End If
                End If

                Dim da As New System.Data.OleDb.OleDbDataAdapter(dbSQL, DirectCast(Me.Database.GetConnection, OleDb.OleDbConnection))
                Dim dt As New DataTable

                da.Fill(dt)
                da.Dispose()

                If (dt.Rows.Count > 0) Then
                    ReDim arr1(dt.Rows.Count - 1)
                    For i As Integer = 0 To dt.Rows.Count - 1
                        arr1(i) = Formats.ToInteger(dt.Rows(i).Item("ObjectID"))
                    Next

                    Arrays.Sort(arr1, 0, Arrays.Len(arr1))
                    ret = arr1
                Else
                    ret = Nothing
                End If

                'If (arr Is Nothing) Then
                '    ret = arr1
                'Else
                '    ret = Arrays.Join(arr1, arr)
                'End If
                dt.Dispose()

            Else
                Dim item As CachedWord = Me.GetCachedWord(word.Word)
                If (arr Is Nothing) Then
                    ret = item.GetIndice
                Else
                    ret = Arrays.Join(item.GetIndice, arr)
                End If
            End If

            Return ret
        End Function

        Public Function Find(ByVal text As String, ByVal nMax As Integer?) As CCollection(Of CResult)
            Return Me.Find(text, Nothing, nMax)
        End Function

        Public Function Find(ByVal text As String, ByVal findIn() As Integer, ByVal nMax As Integer?) As CCollection(Of CResult)
            Dim ret As New CCollection(Of CResult)
            text = Trim(text)
            If (text = "") Then Return ret

            Dim words() As WordInfo

            'Estraggo le parole da cercare
            words = Me.SplitWords(text)

            'Ottengo le informazioni sulle parole da cercare per migliorare la ricerca
            If (words IsNot Nothing AndAlso UBound(words) > 0) Then

                Me.GetWordStats(words)

                'Ordino le parole da cercare in modo da migliorare la ricerca
                Array.Sort(words, 0, Arrays.Len(words)) '- 1
            End If

            'Lancio la ricerca
            If (Arrays.Len(words) > 0) Then words(UBound(words)).IsLike = False
            Dim items() As Integer = Me.Find(words, findIn)

            'Se non ci sono risultati provo a rilassare la ricerca
            If (Arrays.Len(items) = 0 AndAlso Arrays.Len(words) > 0) Then
                If (Arrays.Len(words) > 1) Then
                    words(UBound(words)).IsLike = True
                Else
                    words(0).IsLike = Len(words(0).Word) > 3
                End If

                'Array.Sort(words, 0, Arrays.Len(words)) '- 1
                items = Me.Find(words, findIn)
            End If


            If (items IsNot Nothing) Then
                Dim i As Integer = 0
                While (i <= UBound(items)) AndAlso (nMax.HasValue = False OrElse i < nMax.Value)
                    ret.Add(New CResult(items(i)))
                    i += 1
                End While
            End If




            Return ret
        End Function

        Private Sub AddToCache(ByVal item As CachedWord)
            SyncLock Me.lockObject
                If (Me.m_MAXCACHESIZE <> 0) Then
                    Me.CachedWords.Add(item.Word, item)
                    If (Me.m_MAXCACHESIZE > 0) Then
                        If Me.CachedWords.Count > Me.MaxCacheSize Then
                            Me.CachedWords.Sort()
                            While Me.CachedWords.Count > (Me.MaxCacheSize * Me.m_UnloadFactor)
                                Me.CachedWords.RemoveAt(Me.CachedWords.Count - 1)
                            End While
                        End If
                    End If
                End If
            End SyncLock
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Azzera completamente l'indice
        ''' </summary>
        Public Sub Reset()
            SyncLock Me.lockObject
                Me.m_CachedWords = Nothing
                Me.Database.ExecuteCommand("DELETE * FROM [tbl_Index]")
                Me.Database.ExecuteCommand("DELETE * FROM [tbl_WordStats]")
                If (Me.WordIndexFolder <> "") Then
                    Dim folder As New System.IO.DirectoryInfo(Me.WordIndexFolder)
                    If folder.Exists Then
                        Dim files() As System.IO.FileInfo = folder.GetFiles("*.idx")
                        For Each file As System.IO.FileInfo In files
                            file.Delete()
                        Next
                    End If
                End If
            End SyncLock
        End Sub
    End Class


End Namespace


Partial Public Class Sistema

    Public Interface IIndexable

        Function GetIndexedWords() As String()

        Function GetKeyWords() As String()

    End Interface


    Public Shared ReadOnly lock As New Object
    Private Shared m_IndexingService As CIndexingService = Nothing

    Public Shared ReadOnly Property IndexingService As CIndexingService
        Get
            SyncLock Sistema.lock
                If (m_IndexingService Is Nothing) Then m_IndexingService = New CIndexingService
                Return m_IndexingService
            End SyncLock
        End Get
    End Property

End Class

