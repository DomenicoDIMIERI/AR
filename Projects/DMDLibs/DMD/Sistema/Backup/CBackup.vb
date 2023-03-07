Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports Ionic.Zip


Partial Public Class Sistema

    Public Enum CompressionLevels As Integer
        None = 0
        BestSpeed = 1
        'Level2 = 2
        'Level3 = 3
        'Level4 = 4
        'Level5 = 5
        [Default] = 6
        'Level7 = 7
        'Level8 = 8
        BestCompression = 9
    End Enum

    Public Enum CompressionMethods As Integer
        [Default] = 0
        Filtered = 1
        HuffmanOnly = 2
    End Enum


    <Serializable> _
    Public Class CBackup
        Inherits DBObject

        Private m_Name As String
        Private m_FileName As String
        Private m_FileDate As Date
        Private m_FileSize As Long
        Private m_LogMessages As String
        Private m_ExecTime As Single
        Private m_CompressionLevel As CompressionLevels
        Private m_CompressionMethod As CompressionMethods
        Private m_Items As CBackupItems

        Public Sub New()
            Me.m_Name = ""
            Me.m_FileName = ""
            Me.m_FileDate = Nothing
            Me.m_FileSize = 0
            Me.m_LogMessages = ""
            Me.m_ExecTime = 0
            Me.m_CompressionLevel = CompressionLevels.Default
            Me.m_CompressionMethod = CompressionMethods.Default
            Me.m_Items = Nothing
        End Sub

        Public ReadOnly Property Items As CBackupItems
            Get
                SyncLock Me
                    If Me.m_Items Is Nothing Then Me.m_Items = New CBackupItems(Me)
                    Return Me.m_Items
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il livello di compressione utilizzato per comprimere l'archivio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CompressionLevel As CompressionLevels
            Get
                Return Me.m_CompressionLevel
            End Get
            Set(value As CompressionLevels)
                Dim oldValue As CompressionLevels = Me.m_CompressionLevel
                If (oldValue = value) Then Exit Property
                Me.m_CompressionLevel = value
                Me.DoChanged("CompressionLevel", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'algoritmo utilizzato per comprimere l'archivio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CompressionMethod As CompressionMethods
            Get
                Return Me.m_CompressionMethod
            End Get
            Set(value As CompressionMethods)
                Dim oldValue As CompressionMethods = Me.m_CompressionMethod
                If (oldValue = value) Then Exit Property
                Me.m_CompressionMethod = value
                Me.DoChanged("CompressionMethod", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del backup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del file in cui è memorizzato il backup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileName As String
            Get
                Return Me.m_FileName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_FileName
                If (oldValue = value) Then Exit Property

                Me.m_FileName = Sistema.FileSystem.GetAbsolutePath(value, ApplicationContext.StartupFloder)

                Me.DoChanged("FileName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di creazione del file di backup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileDate As Date
            Get
                Return Me.m_FileDate
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_FileDate
                If (oldValue = value) Then Exit Property
                Me.m_FileDate = value
                Me.DoChanged("FileDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la dimensione in bytes del file di backup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileSize As Long
            Get
                Return Me.m_FileSize
            End Get
            Set(value As Long)
                Dim oldValue As Long = Me.m_FileSize
                If (oldValue = value) Then Exit Property
                Me.m_FileSize = value
                Me.DoChanged("FileSize", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'output 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LogMessages As String
            Get
                Return Me.m_LogMessages
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il tempo di esecuzione del backup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ExecTime As Single
            Get
                Return Me.m_ExecTime
            End Get
        End Property

        Public Function Load(ByVal fileName As String) As Boolean
            Me.m_Name = FileSystem.GetBaseName(fileName)
            Me.m_FileName = fileName
            Me.m_FileDate = FileSystem.GetCreationTime(fileName)
            Me.m_FileSize = FileSystem.GetFileSize(fileName)
            Me.m_LogMessages = ""
            Return True
        End Function

        Friend Function GetDefaultName() As String
            Dim ret As String = "BK"
            ret &= Strings.PadLeft(Year(Me.m_FileDate), 4, "0")
            ret &= Strings.PadLeft(Month(Me.m_FileDate), 2, "0")
            ret &= Strings.PadLeft(Day(Me.m_FileDate), 2, "0")
            ret &= Strings.PadLeft(Hour(Me.m_FileDate), 2, "0")
            ret &= Strings.PadLeft(Minute(Me.m_FileDate), 2, "0")
            ret &= Strings.PadLeft(Second(Me.m_FileDate), 2, "0")
            ret &= ".BKP"
            Return ret
        End Function

        ''' <summary>
        ''' Effettua il backup
        ''' </summary>
        ''' <remarks></remarks>
        Friend Sub Create(ByVal fd As Nullable(Of Date))
            Dim include() As String = Backups.Configuration.GetIncludedDirs
            Dim exclude() As String = Backups.Configuration.GetExcludedDirs
            'Dim out As New System.IO.StringWriter
            Dim t1, t2 As Double
            t1 = Timer
            ' note: this does not recurse directories! 
            
            'Try
            '    zip.AddFile(APPConn.Path)
            'Catch ex As Exception
            '    zip.StatusMessageTextWriter.WriteLine("Errore " & ex.Message & " sul file " & APPConn.Path)
            'End Try
            'Try
            '    zip.AddFile(LOGConn.Path)
            'Catch ex As Exception
            '    zip.StatusMessageTextWriter.WriteLine("Errore " & ex.Message & " sul file " & LOGConn.Path)
            'End Try

            For i As Integer = 0 To Arrays.Len(include) - 1
                include(i) = Trim(include(i))
                If (include(i) <> "") Then
                    include(i) = LCase(Sistema.FileSystem.GetAbsolutePath(include(i), Sistema.ApplicationContext.StartupFloder))
                    Sistema.ApplicationContext.Log("Includo la cartella " & include(i))
                End If
            Next
            For i As Integer = 0 To Arrays.Len(exclude) - 1
                exclude(i) = Trim(exclude(i))
                If (exclude(i) <> "") Then
                    exclude(i) = LCase(Sistema.FileSystem.GetAbsolutePath(exclude(i), Sistema.ApplicationContext.StartupFloder))
                    Sistema.ApplicationContext.Log("Escludo la cartella " & exclude(i))
                End If

            Next

            'Dim zip As New ZipFile
            'zip.StatusMessageTextWriter = out
            'zip.ZipErrorAction = ZipErrorAction.Skip

            'zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
            Dim bp As String = ApplicationContext.MapPath("/")
            Dim dir As String = Sistema.FileSystem.CombinePath(Sistema.FileSystem.GetFolderName(Me.FileName), Sistema.FileSystem.GetBaseName(Me.FileName))
            Sistema.FileSystem.CreateRecursiveFolder(dir)

            For i As Integer = 0 To Arrays.Len(include) - 1
                Me.ProcessFolder(exclude, include(i), bp, fd)
            Next

            ''zip.AlternateEncoding = System.Text.Encoding.Unicode
            'If (Sistema.Backups.Configuration.MaxSegmentSize > 0) Then zip.MaxOutputSegmentSize = Sistema.Backups.Configuration.MaxSegmentSize
            'zip.CompressionLevel = Me.CompressionLevel
            'zip.CompressionMethod = Me.CompressionMethod
            'zip.UseZip64WhenSaving = Zip64Option.AsNecessary
            '' zip.TempFileFolder = DMD.Sistema.FileSystem.GetSystemTempFolder
            'Sistema.ApplicationContext.Log("Inizio la compressione " & Me.FileName)
            'zip.Save(Me.FileName)

            t2 = Timer
            Me.m_ExecTime = t2 - t1

            Me.Items.Save()

            Me.m_FileDate = Sistema.FileSystem.GetCreationTime(Me.FileName)
            Me.m_FileSize = 0 'Sistema.FileSystem.GetFileSize(Me.FileName)
            For Each item As CBackupItem In Me.Items
                Me.m_FileSize += item.DimensioneCompressa
            Next

            'Me.m_LogMessages = out.ToString
            'out.Dispose()


            'Sistema.ApplicationContext.Log("Compressione terminata " & Me.m_LogMessages)
        End Sub

        Private Sub ProcessFolder(ByVal exclude() As String, ByVal folder As String, ByVal percorsoBase As String, ByVal fd As Nullable(Of Date))
            folder = Sistema.FileSystem.NormalizePath(folder)
            'Sistema.ApplicationContext.Log("Elaboro la cartella " & folder)

            If (Not Me.IsFileIncluded(exclude, folder)) Then
                Sistema.ApplicationContext.Log("Percorso escluso: " & folder)
                Exit Sub
            End If


            'Aggiungiamo tutti i files
            Dim fCursor As FindFileCursor
            Dim numitems As Integer
            Try
                fCursor = New FindFileCursor(folder & "*.*", FileAttribute.Normal, False)
                numitems = fCursor.Count
            Catch ex As AccessViolationException
                Sistema.ApplicationContext.Log("Errore di accesso al percorso: " & folder)
                Exit Sub
            Catch ex As Exception
                'zip.StatusMessageTextWriter.WriteLine(ex.Message)
                Sistema.ApplicationContext.Log(Formats.FormatUserDateTime(Now) & " " & ex.Message & vbNewLine)
                Me.m_LogMessages &= Formats.FormatUserDateTime(Now) & " " & ex.Message & vbNewLine
                Exit Sub
            End Try

            While Not fCursor.EOF
                Dim fName As String = fCursor.Item
                If fName = Me.FileName Then
                    Sistema.ApplicationContext.Log("Escludo il file di destinazione del backup: " & fName)
                End If
                If (fd.HasValue = False OrElse System.IO.File.GetLastWriteTime(fName) >= fd.Value) Then
                    'zip.AddFile(fCursor.Item, Me.GetInternalPath(Sistema.FileSystem.GetFolderName(fCursor.Item)))
                    Dim item As New CBackupItem
                    Me.Items.Add(item)
                    item.Comprimi(fCursor.Item, percorsoBase)
                    Sistema.ApplicationContext.Log("zip.AddFile(" & fCursor.Item & ", " & Me.GetInternalPath(Sistema.FileSystem.GetFolderName(fCursor.Item)))
                End If
                fCursor.MoveNext()
            End While
            fCursor.Reset()

            'Aggiungiamo le sottocartelle 
            Dim dCursor As FindFolderCursor
            Try
                dCursor = New FindFolderCursor(folder & "*.*", FileAttribute.Normal, False)
                numitems = dCursor.Count
            Catch ex As AccessViolationException
                Sistema.ApplicationContext.Log("Errore di accesso al percorso: " & folder)
                Exit Sub
            Catch ex As Exception
                Sistema.ApplicationContext.Log(ex.Message)
                Exit Sub
            Finally
                'If dCursor isnot Nothing then dCursor .Reset 
            End Try

            While Not dCursor.EOF
                Me.ProcessFolder(exclude, dCursor.Item, percorsoBase, fd)
                dCursor.MoveNext()
            End While
            dCursor.Reset()



        End Sub

        ''' <summary>
        ''' Ripristina tutti i files
        ''' </summary>
        ''' <remarks></remarks>
        Friend Sub Restore()
            'Dim zip As New ZipFile(Me.FileName)
            'Dim bp As String = ApplicationContext.MapPath("/")
            ''AddHandler zip.ExtractExceptio, AddressOf zipExtractError
            'zip.ExtractAll(bp, ExtractExistingFileAction.OverwriteSilently)
            Dim bp As String = ApplicationContext.MapPath("/")

            For Each item As CBackupItem In Me.Items
                item.Decomprimi(bp)
            Next
        End Sub



        'Private Sub zipExtractError(ByVal sender As Object, ByVal e As Ionic.Zip.ExtractExceptionEventArgs)
        '    Debug.Print("Problema nell'estrarre: " & e.TargetPath & ": " & e.Exception.Message)
        '    e.Terminate = False
        'End Sub

        ''' <summary>
        ''' Elimina i file di backup e cancella la registrazione nel DB
        ''' </summary>
        ''' <remarks></remarks>
        Friend Sub Destroy()
            Dim bp As String = ApplicationContext.MapPath("/")
            Dim dir As String = Sistema.FileSystem.CombinePath(Sistema.FileSystem.GetFolderName(Me.FileName), Sistema.FileSystem.GetBaseName(Me.FileName))

            'Dim zip As New ZipFile(Me.FileName)
            'Dim bp As String = ApplicationContext.MapPath("/")
            ''AddHandler zip.ExtractExceptio, AddressOf zipExtractError
            'zip.ExtractAll(bp, ExtractExistingFileAction.OverwriteSilently)
            'For Each item As CBackupItem In Me.Items
            '    item.Delete()
            'Next

            If (System.IO.Directory.Exists(dir)) Then System.IO.Directory.Delete(dir, True)
            If (System.IO.File.Exists(Me.FileName)) Then System.IO.File.Delete(Me.FileName)
            Me.Delete()
        End Sub


        Private Function IsFileIncluded(ByVal excluded() As String, ByVal fileName As String) As Boolean
            If (excluded Is Nothing OrElse excluded.Length = 0) Then Return True
            Dim fPath As String = LCase(Sistema.FileSystem.NormalizePath(Sistema.FileSystem.GetFolderName(fileName)))
            For i As Integer = 0 To UBound(excluded)
                Dim f As String = excluded(i)
                If (Len(f) > 0) AndAlso (LCase(f) = Left(fPath, Len(f))) Then Return False
            Next
            Return True
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Backups.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Backups"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Me.m_FileName = reader.Read("FileName", Me.m_FileName)
            Me.m_FileDate = reader.Read("FileDate", Me.m_FileDate)
            Me.m_FileSize = reader.Read("FileSize", Me.m_FileSize)
            Me.m_LogMessages = reader.Read("LogMessages", Me.m_LogMessages)
            Me.m_ExecTime = reader.Read("ExecTime", Me.m_ExecTime)
            Me.m_CompressionLevel = reader.Read("CompressionLevel", Me.m_CompressionLevel)
            Me.m_CompressionMethod = reader.Read("CompressionMethod", Me.m_CompressionMethod)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("FileName", Me.m_FileName)
            writer.Write("FileDate", Me.m_FileDate)
            writer.Write("FileSize", Me.m_FileSize)
            writer.Write("LogMessages", Me.m_LogMessages)
            writer.Write("ExecTime", Me.m_ExecTime)
            writer.Write("CompressionLevel", Me.m_CompressionLevel)
            writer.Write("CompressionMethod", Me.m_CompressionMethod)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("FileName", Me.m_FileName)
            writer.WriteAttribute("FileDate", Me.m_FileDate)
            writer.WriteAttribute("FileSize", Me.m_FileSize)
            writer.WriteAttribute("ExecTime", Me.m_ExecTime)
            writer.WriteAttribute("CompressionLevel", Me.m_CompressionLevel)
            writer.WriteAttribute("CompressionMethod", Me.m_CompressionMethod)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("LogMessages", Me.m_LogMessages)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FileName" : Me.m_FileName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FileDate" : Me.m_FileDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "FileSize" : Me.m_FileSize = XML.Utils.Serializer.DeserializeLong(fieldValue)
                Case "LogMessages" : Me.m_LogMessages = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ExecTime" : Me.m_ExecTime = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "CompressionLevel" : Me.m_CompressionLevel = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CompressionMethod" : Me.m_CompressionMethod = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Name & ", backup del " & Formats.FormatUserDate(Me.m_FileDate) & ", dimensione file: " & Formats.FormatBytes(Me.m_FileSize)
        End Function

        Private Function GetInternalPath(ByVal path As String) As String
            'Dim bp As String = ApplicationContext.MapPath("/")
            'If (Strings.Left(path, Len(bp)) = bp) Then Return Mid(path, Len(bp))
            'Return path
            Return DMD.Sistema.FileSystem.GetRelativePath(path, ApplicationContext.StartupFloder)
        End Function

    End Class


End Class

 