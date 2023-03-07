Imports System.IO
Imports DMD
Imports DMD.XML
Imports DMD.Sistema
Imports System.Collections.Specialized
Imports System.Runtime.InteropServices

Public Class Scansioni
    Public Class ScansioneEventArgs
        Inherits System.EventArgs

        Public S As Scansione

        Public Sub New()
        End Sub

        Public Sub New(ByVal s As Scansione)
            Me.S = s
        End Sub

    End Class

    Public Shared Event NuovaScansione(ByVal sender As Object, ByVal e As ScansioneEventArgs)

    Private Shared m_Items As ConfigItem() = {}
    Private Shared watched As New System.Collections.ArrayList
    Private Shared scansioni As New System.Collections.ArrayList

    Public Shared Function GetConfiguration() As ConfigItem()
        Return m_Items.Clone
    End Function

    Public Shared Sub SetConfiguration(ByVal items As ConfigItem())
        StopWatching()
        m_Items = items.Clone
        StartWatching()
    End Sub

    Public Shared Sub PersistConfig()
        Dim fileName As String = GetConfigFileName()
        Dim folder As String = Sistema.FileSystem.GetFolderName(fileName)
        Sistema.FileSystem.CreateRecursiveFolder(folder)
        Sistema.FileSystem.SetTextFileContents(fileName, XML.Utils.Serializer.Serialize(GetConfiguration))
    End Sub

    Public Shared Sub LoadConfig()
        Dim fileName As String = GetConfigFileName()
        Dim folder As String = Sistema.FileSystem.GetFolderName(fileName)
        Sistema.FileSystem.CreateRecursiveFolder(folder)
        Try
            Dim text As String = Sistema.FileSystem.GetTextFileContents(fileName)
            SetConfiguration(Arrays.Convert(Of ConfigItem)(XML.Utils.Serializer.Deserialize(text)))
        Catch ex As Exception

        End Try
    End Sub



    Public Shared Function GetConfigFileName() As String
        Return Sistema.ApplicationContext.UserDataFolder & "\DMDScanHandler\config.dat"
    End Function


    Public Shared Sub StopWatching()
        For Each w As System.IO.FileSystemWatcher In watched
            w.Dispose()
        Next
        watched.Clear()
    End Sub

    Public Shared Sub StartWatching()
        StopWatching()

        For Each c As ConfigItem In m_Items
            Try
                WatchFolder(c.Percorso)
            Catch ex As Exception
                Sistema.ApplicationContext.Log("Errore in WatchFolder: " & c.NomeUtente & ", " & c.Percorso & vbNewLine & ex.StackTrace)
            End Try
        Next
    End Sub

    Private Shared Sub WatchFolder(ByVal folder As String)
        Dim WatchFolder As New System.IO.FileSystemWatcher()
        'this is the path we want to monitor
        WatchFolder.Path = folder

        'Add a list of Filter we want to specify
        'make sure you use OR for each Filter as we need to
        'all of those 

        WatchFolder.NotifyFilter = NotifyFilters.DirectoryName
        WatchFolder.NotifyFilter = WatchFolder.NotifyFilter Or NotifyFilters.FileName
        'WatchFolder.NotifyFilter = WatchFolder.NotifyFilter Or IO.NotifyFilters.Attributes
        WatchFolder.IncludeSubdirectories = True

        ' add the handler to each event
        'AddHandler WatchFolder.Changed, AddressOf logchange
        AddHandler WatchFolder.Created, AddressOf filecreated
        AddHandler WatchFolder.Deleted, AddressOf filedeleted

        ' add the rename handler as the signature is different
        AddHandler WatchFolder.Renamed, AddressOf filerenamed
        AddHandler WatchFolder.Changed, AddressOf filechanged

        watched.Add(WatchFolder)

        'Set this property to true to start watching
        WatchFolder.EnableRaisingEvents = True
    End Sub



    Private Shared Sub filecreated(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        Dim s As New Scansione
        s.Percorso = e.FullPath
        s.DataScansione = Now

        For Each c As ConfigItem In m_Items
            If s.Percorso.StartsWith(c.Percorso, StringComparison.InvariantCultureIgnoreCase) Then
                s.C = c
                RaiseEvent NuovaScansione(Nothing, New ScansioneEventArgs(s))
                s.UploadFile(c)
            End If
        Next


    End Sub

    Private Shared Sub filechanged(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        'Dim s As New Scansione
        's.Percorso = e.FullPath
        's.DataScansione = Now

        'For Each c As ConfigItem In m_Items
        '    If s.Percorso.StartsWith(c.Percorso, StringComparison.InvariantCultureIgnoreCase) Then
        '        UploadFile(c, s)
        '    End If
        'Next

        'RaiseEvent FileChange(Nothing, e)
    End Sub

    Private Shared Sub filedeleted(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)

    End Sub

    Private Shared Sub filerenamed(ByVal source As Object, ByVal e As System.IO.RenamedEventArgs)


    End Sub


End Class
