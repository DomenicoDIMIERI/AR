Imports Ionic.Zip

Partial Class Sistema

    Public Class CBackupItem
        Implements DMD.XML.IDMDXMLSerializable

        Public m_Owner As CBackup
        Public PercorsoOrigine As String
        Public FileDestinazione As String
        Public DimensioneOrigine As Long
        Public DimensioneCompressa As Long
        Public DataUltimaModifica As Date
        Public DataCompressione As Date
        Public Messages As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public ReadOnly Property Owner As CBackup
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Overridable Sub SetOwner(ByVal owner As CBackup)
            Me.m_Owner = owner
        End Sub

        
        Public Sub Comprimi(ByVal percorsoOrigine As String, ByVal percorsoBase As String)
            Me.PercorsoOrigine = Sistema.FileSystem.GetRelativePath(percorsoOrigine, percorsoBase)
            Me.DimensioneOrigine = Sistema.FileSystem.GetFileSize(percorsoOrigine)
            Me.DataUltimaModifica = Sistema.FileSystem.GetLastModifiedTime(percorsoOrigine)

            Dim out As New System.IO.StringWriter
            Dim zip As New ZipFile
            zip.StatusMessageTextWriter = out
            zip.ZipErrorAction = ZipErrorAction.Skip
            'zip.AddDirectory(Sistema.ApplicationContext.WorkingFolder)
            zip.AddFile(percorsoOrigine, "")
            zip.CompressionLevel = Me.m_Owner.CompressionLevel
            zip.CompressionMethod = Me.m_Owner.CompressionMethod
            zip.UseZip64WhenSaving = Zip64Option.AsNecessary

            Dim dir As String = Sistema.FileSystem.CombinePath(Sistema.FileSystem.GetFolderName(Me.m_Owner.FileName), Sistema.FileSystem.GetBaseName(Me.m_Owner.FileName))
            Dim stream As System.IO.Stream = Nothing
            SyncLock Me.m_Owner
                Me.FileDestinazione = ASPSecurity.GetRandomKey(8) & ".ZIP"
                While Sistema.FileSystem.FileExists(Sistema.FileSystem.CombinePath(dir, Me.FileDestinazione))
                    Me.FileDestinazione = ASPSecurity.GetRandomKey(8) & ".ZIP"
                End While
                stream = New System.IO.FileStream(Sistema.FileSystem.CombinePath(dir, Me.FileDestinazione), System.IO.FileMode.Create)
            End SyncLock
            zip.Save(stream)
            Me.DataCompressione = Calendar.Now
            Me.DimensioneCompressa = Sistema.FileSystem.GetFileSize(Sistema.FileSystem.CombinePath(dir, Me.FileDestinazione))
            stream.Dispose()
            Me.Messages = out.ToString
            out.Dispose()
        End Sub

        Public Sub Decomprimi(ByVal percorsoBase As String)
            Dim dir As String = Sistema.FileSystem.CombinePath(Sistema.FileSystem.GetFolderName(Me.m_Owner.FileName), Sistema.FileSystem.GetBaseName(Me.m_Owner.FileName))
            Dim f As String = Sistema.FileSystem.GetFileName(Me.FileDestinazione)
            Dim zip As New ZipFile(Sistema.FileSystem.CombinePath(dir, f))
            'AddHandler zip.ExtractExceptio, AddressOf zipExtractError
            Dim p As String = Sistema.FileSystem.GetAbsolutePath(Me.PercorsoOrigine, percorsoBase)
            p = Sistema.FileSystem.GetFolderName(p)
            Sistema.FileSystem.CreateRecursiveFolder(p)
            zip.ExtractAll(p, ExtractExistingFileAction.OverwriteSilently)
            zip.Dispose()
        End Sub

        'Public Sub Delete()
        '    Dim dir As String = Sistema.FileSystem.CombinePath(Sistema.FileSystem.GetFolderName(Me.m_Owner.FileName), Sistema.FileSystem.GetBaseName(Me.m_Owner.FileName))
        '    Dim f As String = Sistema.FileSystem.GetFileName(Me.FileDestinazione)
        '    Dim p As String = Sistema.FileSystem.GetAbsolutePath(Me.PercorsoOrigine, percorsoBase)
        '    p = Sistema.FileSystem.GetFolderName(p)
        '    Sistema.FileSystem.CreateRecursiveFolder(p)
        '    zip.ExtractAll(p, ExtractExistingFileAction.OverwriteSilently)
        '    zip.Dispose()
        'End Sub


        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "PercorsoOrigine" : Me.PercorsoOrigine = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FileDestinazione" : Me.FileDestinazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DimensioneOrigine" : Me.DimensioneOrigine = XML.Utils.Serializer.DeserializeLong(fieldValue)
                Case "DimensioneCompressa" : Me.DimensioneCompressa = XML.Utils.Serializer.DeserializeLong(fieldValue)
                Case "DataUltimaModifica" : Me.DataUltimaModifica = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataCompressione" : Me.DataCompressione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Messages" : Me.Messages = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("PercorsoOrigine", Me.PercorsoOrigine)
            writer.WriteAttribute("FileDestinazione", Me.FileDestinazione)
            writer.WriteAttribute("DimensioneOrigine", Me.DimensioneOrigine)
            writer.WriteAttribute("DimensioneCompressa", Me.DimensioneCompressa)
            writer.WriteAttribute("DataUltimaModifica", Me.DataUltimaModifica)
            writer.WriteAttribute("DataCompressione", Me.DataCompressione)
            writer.WriteTag("Messages", Me.Messages)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class