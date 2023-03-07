Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class Sistema

    <Flags> _
    Public Enum BackupFlags As Integer
        None = 0
        PartialBackupEnabled = 1
        FullBackupEnabled = 2
    End Enum


    Public NotInheritable Class CBackupsConfiguration
        Inherits DBObject

        Private m_Flags As BackupFlags
        Private m_BackupFolder As String
        Private m_ExludedDirs As String()
        Private m_IncludeDirs As String()
        Private m_FullBackupInterval As Integer
        Private m_PartialBackupInterval As Integer
        Private m_LastFullBackupDate As Nullable(Of Date)
        Private m_LastPartialBackupDate As Nullable(Of Date)
        Private m_RunBackupAt As Date 'Ora in cui eseguire il backup
        Private m_CompressionMethod As CompressionMethods
        Private m_CompressionLevel As CompressionLevels
        Private m_MaxSegmentSize As Integer

        Public Sub New()
            Me.m_Flags = BackupFlags.None
            Me.m_BackupFolder = "\Backup"
            Me.m_ExludedDirs = Nothing
            Me.m_IncludeDirs = Nothing
            Me.m_FullBackupInterval = 30
            Me.m_PartialBackupInterval = 7
            Me.m_LastFullBackupDate = Nothing
            Me.m_LastPartialBackupDate = Nothing
            Me.m_RunBackupAt = New Date(2000, 1, 1, 23, 0, 0)
            Me.m_MaxSegmentSize = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la dimensione massima di un elemento del file di backup (se > 0 il backup sarà diviso in n spezzoni di dimensione massima MaxSegmentSize)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MaxSegmentSize As Integer
            Get
                Return Me.m_MaxSegmentSize
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_MaxSegmentSize
                If (oldValue = value) Then Exit Property
                Me.m_MaxSegmentSize = value
                Me.DoChanged("MaxSegmentSize", value, oldValue)
            End Set
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
        ''' Restituisce o imposta l'ora in cui eseguire il backup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RunBackupAt As Date
            Get
                Return Me.m_RunBackupAt
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_RunBackupAt
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_RunBackupAt = value
                Me.DoChanged("RunBackupAt", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As BackupFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As BackupFlags)
                Dim oldValue As BackupFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve effettuare il backup completo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FullBackupEnabled As Boolean
            Get
                Return TestFlag(Me.m_Flags, BackupFlags.FullBackupEnabled)
            End Get
            Set(value As Boolean)
                If (Me.FullBackupEnabled = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, BackupFlags.FullBackupEnabled, value)
                Me.DoChanged("FullBackupEnabled", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve effettuare i backup parziali
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartialBackupEnabled As Boolean
            Get
                Return TestFlag(Me.m_Flags, BackupFlags.PartialBackupEnabled)
            End Get
            Set(value As Boolean)
                If (Me.PartialBackupEnabled = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, BackupFlags.PartialBackupEnabled, value)
                Me.DoChanged("PartialBackupEnabled", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'intervallo di tmpo tra due backup completi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FullBackupInterval As Integer
            Get
                Return Me.m_FullBackupInterval
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_FullBackupInterval
                If (oldValue = value) Then Exit Property
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("FullBackupInterval")
                Me.m_FullBackupInterval = value
                Me.DoChanged("FullBackupInterval", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'intervallo di tmpo tra due backup parziali
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartialBackupInterval As Integer
            Get
                Return Me.m_PartialBackupInterval
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_PartialBackupInterval
                If (oldValue = value) Then Exit Property
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("PartialBackupInterval")
                Me.m_PartialBackupInterval = value
                Me.DoChanged("PartialBackupInterval", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo backup completo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastFullBackupDate As Nullable(Of Date)
            Get
                Return Me.m_LastFullBackupDate
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_LastFullBackupDate
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_LastFullBackupDate = value
                Me.DoChanged("LastFullBackupDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo backup parziale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastPartialBackupDate As Nullable(Of Date)
            Get
                Return Me.m_LastPartialBackupDate
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_LastPartialBackupDate
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_LastPartialBackupDate = value
                Me.DoChanged("LastPartialBackupDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il percorso predefinito in cui vengono memorizzati i files di backup
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDefaultBackupFolder() As String
            Return "\Backups"
        End Function

        ''' <summary>
        ''' Restituisce o imposta il percorso in cui vengono memorizzati i files di backup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BackupFolder As String
            Get
                Return Me.m_BackupFolder
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_BackupFolder
                If (oldValue = value) Then Exit Property
                If (value = "") Then value = GetDefaultBackupFolder()
                Me.m_BackupFolder = value
                Me.DoChanged("BackupFolder", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce un array contenente tutte le cartelle escluse dai backup
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetExcludedDirs() As String()
            'Dim ret As New System.Collections.ArrayList
            ''Dim tmp As String = Me.m_ExludedDirs ' Backups.Module.Settings.GetValueString("ExcludedDirs", "")
            'Dim items() As String = Me.m_ExludedDirs ' Strings.Split(tmp, ",")
            'Dim bkFolder As String = Sistema.FileSystem.NormalizePath(Me.BackupFolder)
            'ret.Add(bkFolder)
            'ret.Add(Sistema.FileSystem.NormalizePath(Sistema.ApplicationContext.TmporaryFolder))
            'If (items IsNot Nothing) Then
            '    For i As Integer = 0 To Arrays.Len(items) - 1
            '        items(i) = Sistema.FileSystem.NormalizePath(Strings.Trim(items(i)))
            '        If (items(i) <> "") AndAlso (LCase(items(i)) <> LCase(bkFolder)) Then
            '            ret.Add(items(i))
            '        End If
            '    Next
            'End If
            'Return ret.ToArray(GetType(String))
            If Me.m_ExludedDirs Is Nothing Then Return Nothing
            Return Me.m_ExludedDirs.Clone
        End Function

        ''' <summary>
        ''' Aggiunge una cartella all'elenco delle cartelle escluse
        ''' </summary>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Public Sub AddExcludedDir(ByVal path As String)
            path = Trim(path)
            If (path = "") Then Throw New ArgumentNullException("Directory di esclusione non valida: NULL")
            Dim items() As String = GetExcludedDirs()
            path = Sistema.FileSystem.NormalizePath(path)
            For Each s As String In items
                If (LCase(s) = LCase(path)) Then Exit Sub
            Next
            items = Arrays.Push(items, path)
            Me.m_ExludedDirs = items ', ",") '  Backups.Module.Settings.SetValueString("ExcludedDirs", Join(items, ","))
        End Sub

        ''' <summary>
        ''' Rimuove una cartella dall'elenco delle cartelle escluse dal backup
        ''' </summary>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Public Sub RemoveExcludedDir(ByVal path As String)
            path = Trim(path)
            If (path = "") Then Throw New ArgumentNullException("Directory di esclusione non valida: NULL")
            Dim items() As String = GetExcludedDirs()
            path = Sistema.FileSystem.NormalizePath(path)
            For i As Integer = 0 To Arrays.Len(items) - 1
                If (LCase(items(i)) = LCase(path)) Then
                    items = Arrays.RemoveAt(items, i)
                    Exit For
                End If
            Next
            Me.m_ExludedDirs = items ' Backups.Module.Settings.SetValueString("ExcludedDirs", Join(items, ","))
        End Sub

        ''' <summary>
        ''' Restituisce un array contenente tutte le cartelle aggiuntive
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetIncludedDirs() As String()
            'Dim ret As New System.Collections.ArrayList
            'Dim tmp As String = Backups.Module.Settings.GetValueString("IncludedDirs", "")
            'Dim items() As String = Me.m_IncludeDirs ' Strings.Split(tmp, ",")
            'Dim bkFolder As String = Sistema.FileSystem.NormalizePath(BackupFolder)
            'If (items IsNot Nothing) Then
            '    For i As Integer = 0 To Arrays.Len(items) - 1
            '        items(i) = Sistema.FileSystem.NormalizePath(Strings.Trim(items(i)))
            '        If (items(i) <> "") AndAlso (LCase(items(i)) <> LCase(bkFolder)) Then
            '            ret.Add(items(i))
            '        End If
            '    Next
            'End If
            If (Me.m_IncludeDirs Is Nothing) Then Return New String() {}
            Return Me.m_IncludeDirs.Clone
        End Function

        ''' <summary>
        ''' Aggiunge una cartella all'elenco delle cartelle aggiuntive
        ''' </summary>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Public Sub AddIncludedDir(ByVal path As String)
            path = Trim(path)
            If (path = "") Then Throw New ArgumentNullException("Directory di inclusione non valida: NULL")
            Dim items() As String = GetIncludedDirs()
            path = Sistema.FileSystem.NormalizePath(path)
            For Each s As String In items
                If (LCase(s) = LCase(path)) Then Exit Sub
            Next
            items = Arrays.Push(items, path)
            Me.m_IncludeDirs = items ' Backups.Module.Settings.SetValueString("IncludedDirs", Join(items, ","))
        End Sub

        ''' <summary>
        ''' Rimuove una cartella dall'elenco delle cartelle aggiuntive 
        ''' </summary>
        ''' <param name="path"></param>
        ''' <remarks></remarks>
        Public Sub RemoveIncludedDir(ByVal path As String)
            path = Trim(path)
            If (path = "") Then Throw New ArgumentNullException("Directory di inclusione non valida: NULL")
            Dim items() As String = GetIncludedDirs()
            path = Sistema.FileSystem.NormalizePath(path)
            For i As Integer = 0 To Arrays.Len(items) - 1
                If (LCase(items(i)) = LCase(path)) Then
                    items = Arrays.RemoveAt(items, i)
                    Exit For
                End If
            Next
            Me.m_IncludeDirs = items '   Backups.Module.Settings.SetValueString("IncludedDirs", Join(items, ","))
        End Sub

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Sistema.Backups.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_BackupConfiguration"
        End Function

        Private Function MakeArray(ByVal value As String) As String()
            Return Strings.Split(value, ",")
        End Function

        Private Function ToArray(ByVal value As String()) As String
            If (value Is Nothing) Then Return ""
            Return Strings.Join(value, ",")
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_BackupFolder = reader.Read("BackupFolder", Me.m_BackupFolder)
            Me.m_ExludedDirs = Me.MakeArray(reader.Read("ExcludedDirs", ""))
            Me.m_IncludeDirs = Me.MakeArray(reader.Read("IncludedDirs", ""))
            Me.m_FullBackupInterval = reader.Read("FullBackupInterval", Me.m_FullBackupInterval)
            Me.m_PartialBackupInterval = reader.Read("PartialBackupInterval", Me.m_PartialBackupInterval)
            Me.m_LastFullBackupDate = reader.Read("LastFullBackupDate", Me.m_LastFullBackupDate)
            Me.m_LastPartialBackupDate = reader.Read("LastPartialBackupDate", Me.m_LastPartialBackupDate)
            Me.m_RunBackupAt = reader.Read("RunBackupAt", Me.m_RunBackupAt)
            Me.m_CompressionMethod = reader.Read("CompressionMethod", Me.m_CompressionMethod)
            Me.m_CompressionLevel = reader.Read("CompressionLevel", Me.m_CompressionLevel)
            Me.m_MaxSegmentSize = reader.Read("MaxSegmentSize", Me.m_MaxSegmentSize)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Flags", Me.m_Flags)
            writer.Write("BackupFolder", Me.m_BackupFolder)
            writer.Write("ExcludedDirs", Me.ToArray(Me.m_ExludedDirs))
            writer.Write("IncludedDirs", Me.ToArray(Me.m_IncludeDirs))
            writer.Write("FullBackupInterval", Me.m_FullBackupInterval)
            writer.Write("PartialBackupInterval", Me.m_PartialBackupInterval)
            writer.Write("LastFullBackupDate", Me.m_LastFullBackupDate)
            writer.Write("LastPartialBackupDate", Me.m_LastPartialBackupDate)
            writer.Write("RunBackupAt", Me.m_RunBackupAt)
            writer.Write("CompressionMethod", Me.m_CompressionMethod)
            writer.Write("CompressionLevel", Me.m_CompressionLevel)
            writer.Write("MaxSegmentSize", Me.m_MaxSegmentSize)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("BackupFolder", Me.m_BackupFolder)
            writer.WriteAttribute("FullBackupInterval", Me.m_FullBackupInterval)
            writer.WriteAttribute("PartialBackupInterval", Me.m_PartialBackupInterval)
            writer.WriteAttribute("LastFullBackupDate", Me.m_LastFullBackupDate)
            writer.WriteAttribute("LastPartialBackupDate", Me.m_LastPartialBackupDate)
            writer.WriteAttribute("RunBackupAt", Me.m_RunBackupAt)
            writer.WriteAttribute("CompressionMethod", Me.m_CompressionMethod)
            writer.WriteAttribute("CompressionLevel", Me.m_CompressionLevel)
            writer.WriteAttribute("MaxSegmentSize", Me.m_MaxSegmentSize)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("ExcludedDirs", Me.ToArray(Me.m_ExludedDirs))
            writer.WriteTag("IncludedDirs", Me.ToArray(Me.m_IncludeDirs))
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "BackupFolder" : Me.m_BackupFolder = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FullBackupInterval" : Me.m_FullBackupInterval = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PartialBackupInterval" : Me.m_PartialBackupInterval = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LastFullBackupDate" : Me.m_LastFullBackupDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "LastPartialBackupDate" : Me.m_LastPartialBackupDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RunBackupAt" : Me.m_RunBackupAt = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ExcludedDirs" : Me.m_ExludedDirs = Me.MakeArray(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "IncludedDirs" : Me.m_IncludeDirs = Me.MakeArray(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "CompressionMethod" : Me.m_CompressionMethod = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CompressionLevel" : Me.m_CompressionLevel = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MaxSegmentSize" : Me.m_MaxSegmentSize = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Sub Load()
            Dim dbRis As System.Data.IDataReader = Me.GetConnection.ExecuteReader("SELECT * FROM [" & Me.GetTableName & "] ORDER BY [ID] ASC")
            If dbRis.Read Then
                Me.GetConnection.Load(Me, dbRis)
            End If
            dbRis.Dispose()
        End Sub

        Public Shadows Sub Save()
            MyBase.Save(True)
            Sistema.Backups.SetConfiguration(Me)
        End Sub

    End Class

    Partial Class CBackupsClass

        ''' <summary>
        ''' Evento generato quando viene modificata la configurazione del sistema dei backup
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ConfigurationChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Private m_Configuration As CBackupsConfiguration = Nothing

        Public ReadOnly Property Configuration As CBackupsConfiguration
            Get
                SyncLock Me
                    If (Me.m_Configuration Is Nothing) Then
                        Me.m_Configuration = New CBackupsConfiguration
                        Me.m_Configuration.Load()
                    End If
                    Return Me.m_Configuration
                End SyncLock
            End Get
        End Property

        Friend Sub SetConfiguration(ByVal c As CBackupsConfiguration)
            SyncLock Me
                m_Configuration = c
            End SyncLock
            Dim e As New System.EventArgs
            RaiseEvent ConfigurationChanged(Me, e)
        End Sub

    End Class
End Class

 