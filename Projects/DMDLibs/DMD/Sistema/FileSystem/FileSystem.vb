Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema
Imports DMD.Internals

Namespace Internals


    Public NotInheritable Class CFileSystemClass
        Private m_LIMITROOT As String = vbNullString




        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce lo spazio libero nel disco specificato (in bytes)
        ''' </summary>
        ''' <param name="driveLetter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDiskFreeSpace(ByVal driveLetter As String) As Long
            driveLetter = UCase(Left(Trim(driveLetter), 1) & ":\")
            For Each drive As System.IO.DriveInfo In My.Computer.FileSystem.Drives
                If UCase(drive.Name) = driveLetter Then Return drive.TotalFreeSpace
            Next
            Return -1
        End Function

        ''' <summary>
        ''' Restituisce la dimensione totale del disco specificato (in bytes)
        ''' </summary>
        ''' <param name="driveLetter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDiskTotalSpace(ByVal driveLetter As String) As Long
            driveLetter = UCase(Left(Trim(driveLetter), 1) & ":\")
            For Each drive As System.IO.DriveInfo In My.Computer.FileSystem.Drives
                If UCase(drive.Name) = driveLetter Then Return drive.TotalSize
            Next
            Return -1
        End Function

        ''' <summary>
        ''' Restituisce l'etichetta del disco specificato
        ''' </summary>
        ''' <param name="driveLetter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDiskLabel(ByVal driveLetter As String) As String
            driveLetter = UCase(Left(Trim(driveLetter), 1) & ":\")
            For Each drive As System.IO.DriveInfo In My.Computer.FileSystem.Drives
                If UCase(drive.Name) = driveLetter Then Return drive.VolumeLabel
            Next
            Return ""
        End Function

        ''' <summary>
        ''' Restituisce o imposta il percorso minimo di lavoro. Se diverso da NULL la libreria potrà operare solo su cartelle e files contenuti all'interno di questo percorso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LimitRoot As String
            Get
                Return m_LIMITROOT
            End Get
            Set(value As String)
                m_LIMITROOT = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce un nome di file temporaneo
        ''' </summary>
        ''' <param name="extension"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTempFileName(Optional ByVal extension As String = "tmp") As String
            Dim strName As String
            Dim path As String = Sistema.ApplicationContext.TmporaryFolder
            Do
                strName = System.IO.Path.Combine(path, ASPSecurity.GetRandomKey(8) & "." & extension)
            Loop While FileExists(strName)
            Return strName
        End Function

        ''' <summary>
        ''' Restituisce un nome di file temporaneo
        ''' </summary>
        ''' <param name="prefix"></param>
        ''' <param name="extension"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTempFileName(ByVal prefix As String, ByVal extension As String) As String
            Dim path As String = Sistema.ApplicationContext.TmporaryFolder
            Dim strName As String = System.IO.Path.Combine(path, prefix & "." & extension)
            Dim i As Integer = 0
            While FileExists(strName)
                i += 1
                strName = System.IO.Path.Combine(path, prefix & " (" & i & ")." & extension)
            End While
            Return strName
        End Function


        Public Function CreateTemporaryFileName(ByVal folderName As String, ByVal prefix As String, Optional ByVal extension As String = "tmp") As String
            Dim i As Integer
            If Right(folderName, 1) <> "\" Then folderName = folderName & "\"
            Do
                i = Fix(Rnd(1) * 100000)
            Loop While FileSystem.FileExists(folderName & prefix & i & "." & extension)
            Return folderName & prefix & i & "." & extension
        End Function

        ''' <summary>
        '''  Estrae il solo nome con l'eventuale estensione
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileName(ByVal fileName As String) As String
            Dim ret As String = fileName
            Dim p As Integer = InStrRev(ret, "\")
            If (p > 0) Then ret = Mid(ret, p + 1)
            Return ret
        End Function

        ''' <summary>
        ''' Estrae il solo nome 
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBaseName(ByVal fileName As String) As String
            Dim p As Integer
            Dim ret As String
            ret = GetFileName(fileName)
            p = InStrRev(ret, ".")
            If (p > 0) Then ret = Left(ret, p - 1)
            Return ret
        End Function

        ''' <summary>
        ''' Estrae l'estensione dal nome del file
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetExtensionName(ByVal fileName As String) As String
            Dim p As Integer
            Dim ret As String
            ret = Trim(fileName)
            'Rimuoviamo il percorso
            p = InStrRev(ret, "\")
            If (p > 0) Then ret = Mid(ret, p + 1)
            'Estraiamo l'estensione
            p = InStrRev(ret, ".")
            If (p > 0) Then
                ret = Mid(ret, p + 1)
            Else
                ret = ""
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Crea o sostituisce il file indicato dal percorso inserendovi il contenuto specificato nella stringa content
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="content"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateTextFile(ByVal path As String, ByVal content As String) As Long
            System.IO.File.WriteAllText(path, content)
            Return Len(content)
        End Function

        ''' <summary>
        ''' Crea il percorso (se il percorso esiste genera errore)
        ''' </summary>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Public Sub CreateFolder(ByVal path As String)
            System.IO.Directory.CreateDirectory(path)
        End Sub

        ''' <summary>
        ''' Restituisce vero se il folder esiste
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FolderExists(ByVal path As String) As Boolean
            Return System.IO.Directory.Exists(path)
        End Function

        ''' <summary>
        ''' Restituisce vero se il file esiste
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FileExists(ByVal path As String) As Boolean
            Return System.IO.File.Exists(path)
        End Function

        ''' <summary>
        ''' Crea il percorso (se il percorso esiste genera errore)
        ''' </summary>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Public Sub CreateRecursiveFolder(ByVal path As String)
            If (System.IO.Directory.Exists(path)) Then Return

            Dim items() As String
            Dim i As Integer
            Dim current As String
            path = Trim(path)
            If (Right(path, 1) = "\") Then path = Left(path, Len(path) - 1)
            items = Split(path, "\")
            current = items(i)
            If Left(LimitRoot, Len(current)) <> current And Not FolderExists(current) Then
                Try
                    CreateFolder(current)
                Catch ex As Exception
                End Try

            End If
            For i = 1 To UBound(items)
                current = current & "\" & items(i)
                If Left(LimitRoot, Len(current)) <> current And Not FolderExists(current) Then
                    Try
                        CreateFolder(current)
                    Catch ex As Exception

                    End Try

                End If
            Next

            If (Not FolderExists(path)) Then
                Throw New ArgumentException("Impossibile creare il percorso: " & path)
            End If
        End Sub

        ''' <summary>
        ''' Estrae il percorso del folder che contiene il file o la cartella
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFolderName(ByVal path As String) As String
            Dim i As Integer
            Dim ret As String
            i = InStrRev(path, "\")
            If (i > 0) Then
                ret = Left(path, i - 1)
            Else
                ret = path
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Ottiene un buffer stringa contenente l'intero file
        ''' </summary>
        ''' <param name="filePath"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTextFileContents(ByVal filePath As String) As String
            Return System.IO.File.ReadAllText(filePath)
        End Function

        Public Sub SetTextFileContents(fName As String, text As String)
            System.IO.File.WriteAllText(fName, text)
        End Sub

        ''' <summary>
        ''' Crea una copia del file
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="destination"></param>
        ''' <param name="overwrite"></param>
        ''' <remarks></remarks>
        Public Sub CopyFile(ByVal source As String, ByVal destination As String, Optional ByVal overwrite As Boolean = False)
            System.IO.File.Copy(source, destination, overwrite)
        End Sub


        ''' <summary>
        ''' Rimuove i caratteri non utilizzabili in un nome di files o di cartella
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RemoveSpecialChars(ByVal value As String) As String
            Const invalidChars As String = "\/:;?<>"
            Dim i As Integer
            Dim ret As String
            ret = value
            For i = 1 To Len(invalidChars)
                ret = Replace(ret, Mid(invalidChars, i, 1), "_")
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce un oggetto CCollection contenente tutti i nome di files e folder contenuti nel percorso specificato
        ''' </summary>
        ''' <param name="sFolder"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllFiles(ByVal sFolder As String) As CCollection(Of String)
            Dim c As New System.IO.DirectoryInfo(sFolder)
            Dim ret As New CCollection(Of String)
            For Each f As System.IO.FileInfo In c.GetFiles()
                ret.Add(f.FullName)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce un oggetto CCollection contenente tutti i nome di files e folder contenuti nel percorso specificato
        ''' </summary>
        ''' <param name="sFolder"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllFiles(ByVal sFolder As String, ByVal searchPattern As String) As CCollection(Of String)
            Dim c As New System.IO.DirectoryInfo(sFolder)
            Dim ret As New CCollection(Of String)
            For Each f As System.IO.FileInfo In c.GetFiles(searchPattern)
                ret.Add(f.FullName)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la data di creazione del file
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCreationTime(ByVal fileName As String) As Date
            Return System.IO.File.GetCreationTime(fileName)
        End Function

        ''' <summary>
        ''' Restituisce la data dell'ultima modifica del file
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetLastModifiedTime(ByVal fileName As String) As Date
            Return System.IO.File.GetLastWriteTime(fileName)
        End Function

        ''' <summary>
        ''' Restituisce la data dell'ultima modifica del file
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetLastAccessTime(ByVal fileName As String) As Date
            Return System.IO.File.GetLastAccessTime(fileName)
        End Function

        ''' <summary>
        ''' Restituisce la dimensione in bytes del file
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileSize(ByVal fileName As String) As Long
            Dim file As New System.IO.FileInfo(fileName)
            Return file.Length
        End Function

        ''' <summary>
        ''' Elimina uno o più files (wildchars)
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="force"></param>
        ''' <remarks></remarks>
        Public Sub DeleteFile(ByVal path As String, Optional ByVal force As Boolean = False)
            If InStr(path, "*") > 0 Or InStr(path, "?") > 0 Then
                Dim files As String()
                Dim i As Integer
                Dim searchPattern As String
                i = InStrRev(path, "\")
                If (i > 0) Then
                    searchPattern = Mid(path, i + 1)
                    path = Left(path, i - 1)
                Else
                    searchPattern = path
                    path = LimitRoot
                End If
                files = System.IO.Directory.GetFiles(path, searchPattern)
                For Each file As String In files
                    If (force) Then
                        Try
                            System.IO.File.Delete(file)
                        Catch ex As Exception
                            Sistema.ApplicationContext.Log("File non eliminato: " & file & " -> " & ex.Message)
                        End Try
                    Else
                        System.IO.File.Delete(file)
                    End If

                Next
            Else
                If (force) Then
                    Try
                        System.IO.File.Delete(path)
                    Catch ex As Exception
                        Sistema.ApplicationContext.Log("File non eliminato: " & path & " -> " & ex.Message)
                    End Try
                Else
                    System.IO.File.Delete(path)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Sposta il file
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="target"></param>
        ''' <remarks></remarks>
        Public Sub MoveFile(ByVal source As String, ByVal target As String)
            System.IO.File.Move(source, target)
        End Sub

        ''' <summary>
        ''' Combina il percorso ad un percorso base
        ''' </summary>
        ''' <param name="folderName"></param>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CombinePath(ByVal folderName As String, ByVal fileName As String) As String
            folderName = Trim(folderName)
            fileName = Trim(fileName)
            If (Right(folderName, 1) <> "\") Then folderName &= "\"
            Return folderName & fileName
        End Function

        ''' <summary>
        ''' Restituisce il nome del percorso "normalizzato" cioè aggiungendo "\" alla fine (se non presente)
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NormalizePath(value As String) As String
            value = Trim(value)
            If (value = "") Then Return ""
            If (Right(value, 1) <> "\") Then value = value & "\"
            Return value
        End Function

        ''' <summary>
        ''' Effettua una copia dallo stream inStream allo stream outStream utilizzando le rispettive posizioni correnti.
        ''' La copia viene effettuata passando per un buffer temporaneo di 2048 bytes
        ''' </summary>
        ''' <param name="inStream"></param>
        ''' <param name="outStream"></param>
        ''' <remarks></remarks>
        Public Sub CopyStream(ByVal inStream As System.IO.Stream, ByVal outStream As System.IO.Stream)
            Const BUFF_SIZE As Integer = 2048
            Dim buffer() As Byte
            ReDim buffer(BUFF_SIZE - 1)

            Dim n As Integer
            n = inStream.Read(buffer, 0, BUFF_SIZE)
            While (n > 0)
                outStream.Write(buffer, 0, n)
                n = inStream.Read(buffer, 0, BUFF_SIZE)
            End While
        End Sub



        ''' <summary>
        ''' Se il percorso è una sottodirectori del percorso radice restituisce solo la parte "relativa" 
        ''' altriemnti restituisce il percorso stesso
        ''' </summary>
        ''' <param name="path">[in] Percorso da analizzare</param>
        ''' <param name="fromPath">[in] Percorso base rispetto a cui "relativizzare"</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRelativePath(ByVal path As String, ByVal fromPath As String) As String
            path = GetAbsolutePath(path)
            fromPath = GetAbsolutePath(fromPath)

            If (Right(path, 1) = "\") Then path = Left(path, Len(path) - 1)
            If (Right(fromPath, 1) = "\") Then fromPath = Left(fromPath, Len(fromPath) - 1)

            Dim i1() As String = Split(path, "\")
            Dim i2() As String = Split(fromPath, "\")
            Dim l1 As Integer = Arrays.Len(i1)
            Dim l2 As Integer = Arrays.Len(i2)

            Dim cnt As Integer = 0
            While (cnt < l1) AndAlso (cnt < l2) AndAlso (Strings.Compare(i1(cnt), i2(cnt), CompareMethod.Text) = 0)
                cnt += 1
            End While

            Dim ret As New System.Text.StringBuilder
            While (l2 - 1 > cnt)
                ret.Append("..\")
                l2 -= 1
            End While
            While (l1 > cnt + 1)
                ret.Append(i1(cnt) & "\")
                cnt += 1
            End While
            If (l1 > cnt) Then
                ret.Append(i1(cnt))
                cnt += 1
            End If

            Return ret.ToString
        End Function

        ''' <summary>
        ''' Restituisce il percorso assoluto calcolato sulla base del percorso di avvio dell'applicazione.
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAbsolutePath(ByVal path As String) As String
            Return Me.GetAbsolutePath(path, ApplicationContext.StartupFloder)
        End Function


        ''' <summary>
        ''' Restituisce il percorso assoluto calcolato sulla base del percorso specificato.
        ''' Se path è già un percorso assoluto la funziona restituisce path stesso
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="basePath"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAbsolutePath(ByVal path As String, ByVal basePath As String) As String
            path = Trim(path)
            basePath = Trim(basePath)
            If Me.IsRelativePath(path) Then
                If (Right(basePath, 1) = "\") Then basePath = Left(basePath, Len(basePath) - 1)
                Dim p() As String = Split(basePath, "\")
                Dim l As Integer = Arrays.Len(p)
                While (Left(path, 3) = "..\") AndAlso l > 0
                    l = l - 1
                    path = Mid(path, 4)
                End While
                If (Left(path, 1) = "\") Then path = Mid(path, 2)
                Dim ret As New System.Text.StringBuilder
                For i As Integer = 0 To l - 1
                    ret.Append(p(i) & "\")
                Next
                ret.Append(path)
                Return ret.ToString
            Else
                Return path
            End If
        End Function

        ''' <summary>
        ''' Restituisce vero se il percorso è un percorso relativo 
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsRelativePath(ByVal path As String) As Boolean
            path = Strings.Trim(path)
            Return (Mid(path, 2, 1) <> ":") AndAlso Left(path, 2) <> "\\" ' (Left(path, 1) = "\") OrElse (Left(path, 1) = "~") OrElse (Left(path, 3) = "..\")
        End Function

        ''' <summary>
        ''' Restituisce il percorso utilizzato dal sistema per i files temporanei
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSystemTempFolder() As String
            Return ApplicationContext.TmporaryFolder
        End Function



        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace

Partial Class Sistema

    Public Enum FileOpenEnum As Integer
        ForReading = 1
        ForWriting = 2
        ForAppending = 3
    End Enum

    Private Shared m_FileSystem As CFileSystemClass

    Public Shared ReadOnly Property FileSystem As CFileSystemClass
        Get
            If m_FileSystem Is Nothing Then m_FileSystem = New CFileSystemClass
            Return m_FileSystem
        End Get
    End Property

End Class