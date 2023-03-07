Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class Sistema

    Public NotInheritable Class CBackupsClass
        Inherits CGeneralClass(Of CBackup)


        'Private Shared m_Backups As CKeyCollection(Of CBackup)
        Friend Sub New()
            MyBase.New("modBackups", GetType(CBackupCursor))
        End Sub
         
        Public Function Create(ByVal fromDate As Nullable(Of Date), ByVal compressionMethod As CompressionMethods, ByVal compressionLevel As CompressionLevels) As CBackup
            Dim bk As CBackup
            bk = New CBackup
            bk.FileDate = Now
            bk.Name = bk.GetDefaultName()
            bk.FileName = Configuration.BackupFolder & "\" & bk.Name
            bk.CompressionLevel = compressionLevel
            bk.CompressionMethod = compressionMethod

            Me.Module.DispatchEvent(New EventDescription("backup_begin", "Inizio il backup ", bk))

            Dim dirs As Object = Me.Configuration.GetIncludedDirs

            Sistema.ApplicationContext.EnterMaintenance()

            'System.Threading.Thread.Sleep(3000)
#If Not Debug Then
            Try
#End If
            bk.Create(fromDate)
#If Not Debug Then
            Catch ex As Exception
                Throw
            Finally
#End If
            Sistema.ApplicationContext.QuitMaintenance()
#If Not Debug Then
            End Try
#End If

            
            'If (bk IsNot Nothing) Then
            bk.Stato = ObjectStatus.OBJECT_VALID
            bk.Save()
            Me.Module.DispatchEvent(New EventDescription("backup_end", "Fine del backup ", bk))
            'End If

            Return bk
        End Function

        Public Function Restore(ByVal id As Integer) As CBackup
            Return Me.Restore(GetItemById(id))
        End Function

        Public Function Restore(ByVal bk As CBackup) As CBackup
            If (bk Is Nothing) Then Throw New ArgumentNullException("Backup non trovato")
            Me.Module.DispatchEvent(New EventDescription("restore_begin", "Inizio il ripristino del sistema dal file: " & bk.FileName, bk))

            Sistema.ApplicationContext.EnterMaintenance()
#If Not Debug Then
            try
#End If
            bk.Restore()
#If Not Debug Then
            Catch ex As Exception
                Throw
            Finally
#End If
            Sistema.ApplicationContext.QuitMaintenance()
#If Not Debug Then
            End Try
#End If
            Me.Module.DispatchEvent(New EventDescription("restore_end", "Fine del ripristino del sistema dal file: " & bk.FileName, bk))

            Return bk
        End Function

        Public Function Delete(ByVal bkID As Integer) As CBackup
            Dim bk As CBackup = GetItemById(bkID)
            bk.Destroy()
            Return bk
        End Function

       
        ''' <summary>
        ''' Restituisce l'elemento in base al suo nome nel DB
        ''' </summary>
        ''' <param name="name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal name As String) As CBackup
            name = Trim(name)
            If (name = "") Then Return Nothing

            Dim cursor As New CBackupCursor
            Try
                cursor.Name.Value = name
                cursor.IgnoreRights = True
                cursor.PageSize = 1
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                Return cursor.Item
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        

        Public Function GetLastFullBackup() As CBackup
            Dim cursor As New CBackupCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.FileDate.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True
                Return cursor.Item
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        Public Overrides Sub Initialize()
            MyBase.Initialize()
            Me.Configuration.Load()
        End Sub

        ''' <summary>
        ''' Verifica se deve essere effettuato il backup sulla base della configurazione attuale ed eventualmente lo effettua
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreateFullBackup()
            Dim d As Date = DateUtils.Now
            Me.Create(Nothing, Me.Configuration.CompressionMethod, Me.Configuration.CompressionLevel)
            Me.Configuration.LastFullBackupDate = d
            Me.Configuration.Save()
        End Sub

        ''' <summary>
        ''' Verifica se deve essere effettuato il backup sulla base della configurazione attuale ed eventualmente lo effettua
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreatePartialBackup(ByVal fromDate As Date)
            Me.Create(fromDate, Me.Configuration.CompressionMethod, Me.Configuration.CompressionLevel)
            Me.Configuration.LastPartialBackupDate = fromDate
            Me.Configuration.Save()
        End Sub

    End Class

    Private Shared m_Backups As CBackupsClass = Nothing

    Public Shared ReadOnly Property Backups As CBackupsClass
        Get
            If (m_Backups Is Nothing) Then m_Backups = New CBackupsClass
            Return m_Backups
        End Get
    End Property

End Class