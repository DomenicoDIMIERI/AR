Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Class FolderWatch

    Public Shared Event FileChange(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs)


    Private Shared included As System.Collections.ArrayList = Nothing
    Private Shared excluded As System.Collections.ArrayList = Nothing
    Private Shared watched As New System.Collections.ArrayList

    Public Shared Function GetIncludedFolders() As System.Collections.ArrayList
        If (included Is Nothing) Then
            included = New System.Collections.ArrayList
            Try
                Dim tmp As String = My.Settings.FoldersToWatch
                For Each Str As String In Split(tmp, vbNewLine)
                    If (Trim(Str) <> "") AndAlso System.IO.Directory.Exists(Str) Then included.Add(Str)
                Next
            Catch ex As Exception

            End Try
        End If
        Return included
    End Function

    Public Shared Sub SetIncludedFolders(ByVal items As System.Collections.ArrayList)
        included = items
        'Dim tmp As New System.Text.StringBuilder
        'For Each Str As String In items
        '    If Trim(Str) <> "" Then
        '        If tmp.Length > 0 Then tmp.Append(vbNewLine)
        '        tmp.Append(Str)
        '    End If
        'Next
        'Settings.FoldersToWatch = tmp.ToString
        'Settings.Save()
    End Sub

    Public Shared Function GetExcludedFolders() As System.Collections.ArrayList
        If (excluded Is Nothing) Then
            excluded = New System.Collections.ArrayList
            'Try
            'Dim tmp As String = My.Settings.FoldersToExclude
            'For Each Str As String In Split(tmp, vbNewLine)
            'If (Trim(Str) <> "") AndAlso System.IO.Directory.Exists(Str) Then excluded.Add(Str)
            'Next
            'Catch ex As Exception

            'End Try
        End If
        Return excluded
    End Function

    Public Shared Sub SetExcludedFolders(ByVal items As System.Collections.ArrayList)
        excluded = items
        'Dim tmp As New System.Text.StringBuilder
        'For Each Str As String In items
        '    If Trim(Str) <> "" Then
        '        If tmp.Length > 0 Then tmp.Append(vbNewLine)
        '        tmp.Append(Str)
        '    End If
        'Next
        'Settings.FoldersToExclude = tmp.ToString
        ' Settings.Save()
    End Sub


    Public Shared Sub StopWatching()
        For Each w As System.IO.FileSystemWatcher In watched
            w.Dispose()
        Next
        watched.Clear()
    End Sub

    Public Shared Sub StartWatching()
        For Each Str As String In GetIncludedFolders()
            If System.IO.Directory.Exists(Str) Then
                WatchFolder(Str)
            End If
        Next
    End Sub

    Public Shared Sub WatchFolder(ByVal folder As String)
        Dim WatchFolder As New System.IO.FileSystemWatcher()
        'this is the path we want to monitor
        WatchFolder.Path = folder

        'Add a list of Filter we want to specify
        'make sure you use OR for each Filter as we need to
        'all of those 

        WatchFolder.NotifyFilter = IO.NotifyFilters.DirectoryName
        WatchFolder.NotifyFilter = WatchFolder.NotifyFilter Or IO.NotifyFilters.FileName
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
        If IsExcluded(e.FullPath) Then Exit Sub
        'DIALTPLib.Log.LogFileChanged(e)

        RaiseEvent FileChange(Nothing, e)
    End Sub

    Private Shared Sub filedeleted(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        If IsExcluded(e.FullPath) Then Exit Sub
        'DIALTPLib.Log.LogFileChanged(e)

        RaiseEvent FileChange(Nothing, e)
    End Sub

    Private Shared Sub filechanged(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        If IsExcluded(e.FullPath) Then Exit Sub
        'DIALTPLib.Log.LogFileChanged(e)

        RaiseEvent FileChange(Nothing, e)
    End Sub

    Private Shared Sub filerenamed(ByVal source As Object, ByVal e As System.IO.RenamedEventArgs)
        If IsExcluded(e.FullPath) Then Exit Sub
        'DIALTPLib.Log.LogFileChanged(e)
        'Log.GetCurrSession.AppendFile("Il file " & e.OldFullPath & " è stato rinominato in " & e.FullPath)

        RaiseEvent FileChange(Nothing, e)
    End Sub

    Public Shared Function IsExcluded(ByVal p As String) As Boolean
        For Each str As String In GetExcludedFolders()
            If Strings.StrComp(Left(p, Len(str)), str, CompareMethod.Text) = 0 Then Return True
        Next
        Return False
    End Function

    Public Shared Sub AddFolder(ByVal path As String)
        Dim items As ArrayList = GetIncludedFolders()
        For Each p As String In items
            If (p = path) Then Return
        Next
        items.Add(path)
        SetIncludedFolders(items)
    End Sub

    Public Shared Sub ExcludeFolder(ByVal path As String)
        Dim items As ArrayList = GetExcludedFolders()
        For Each p As String In items
            If (p = path) Then Return
        Next
        items.Add(path)
        SetExcludedFolders(items)
    End Sub

End Class
