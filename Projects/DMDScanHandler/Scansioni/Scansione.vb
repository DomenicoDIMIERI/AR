Imports DMD
Imports DMD.XML
Imports DMD.Sistema
Imports System.IO
Imports System.Collections.Specialized
Imports System.Runtime.InteropServices
Imports DMDScanHandler.Scansioni

Public Enum StatoScansione As Integer
    Attesa = 0
    Uploading = 1
    Uploaded = 2
    Rimandata = 3
    Errore = 4
End Enum


Public Class Scansione
    Inherits DMDObject
    Implements DMD.XML.IDMDXMLSerializable

    Public Event StatoChanged(ByVal sender As Object, ByVal e As ScansioneEventArgs)

    Public C As ConfigItem
    Public Percorso As String
    Public DataScansione As Date
    Public Stato As StatoScansione

    Public Sub New()
        Me.Percorso = ""
        Me.DataScansione = Now
    End Sub

    Protected Overridable Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Percorso", Me.Percorso)
        writer.WriteAttribute("DataScansione", Me.DataScansione)
        writer.WriteAttribute("Stato", Me.Stato)
    End Sub

    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Percorso" : Me.Percorso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "DataScansione" : Me.DataScansione = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "Stato" : Me.Stato = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
        End Select
    End Sub


    Private Shared Function IsFileLocked(ByVal filePath As String) As Boolean
        Try
            Using (File.Open(filePath, FileMode.Open))

            End Using
        Catch e As IOException
            Dim errorCode As Integer = Marshal.GetHRForException(e) And ((1 << 16) - 1)

            Return errorCode = 32 OrElse errorCode = 33
        End Try

        Return False
    End Function


    Private Sub CopyFile(ByVal src As String, ByVal dest As String)
        Dim i As Integer = 0
        While (IsFileLocked(src) AndAlso (i < 10))
            System.Threading.Thread.Sleep(1000)
            i += 1
        End While

        If IsFileLocked(src) Then
            Throw New IOException
        Else
            System.IO.File.Copy(src, dest, True)
        End If
    End Sub



    Public Sub UploadFile(ByVal c As ConfigItem)
        Me.C = c
        Dim info As New System.IO.FileInfo(Me.Percorso)
        Dim data As Date = Me.DataScansione
        Dim serverName As String = c.UploadService
        Dim userName As String = c.NomeUtente
        Dim tmpName As String = System.IO.Path.GetTempFileName
        Try
            CopyFile(Me.Percorso, tmpName)
        Catch ex As IOException
            Me.Stato = StatoScansione.Rimandata
            RaiseEvent StatoChanged(Me, New ScansioneEventArgs(Me))
            Return
        Catch ex As Exception
            Me.Stato = StatoScansione.Errore
            RaiseEvent StatoChanged(Me, New ScansioneEventArgs(Me))
            Return
        End Try
        Me.Stato = StatoScansione.Uploading
        RaiseEvent StatoChanged(Me, New ScansioneEventArgs(Me))
        'Dim url As String = serverName & "/widgets/websvc/dialtpuploader.aspx?p=" & My.Computer.Name & "&u=" & userName & "&f=" & fileName & "&d=" & DMD.Sistema.RPC.date2n(data)
        Dim url As String = serverName & "?p=" & My.Computer.Name & "&u=" & userName & "&f=" & Me.Percorso & "&d=" & DMD.Sistema.RPC.date2n(data)
        Dim nvc As New NameValueCollection()
        'nvc.Add("id", "TTR")
        nvc.Add("File0", "Upload")
        Dim ret As String = RPC.HttpUploadFile(url, tmpName, "file", "image/jpeg", nvc)
        System.IO.File.Delete(tmpName)
        Me.Stato = StatoScansione.Uploaded
        RaiseEvent StatoChanged(Me, New ScansioneEventArgs(Me))
    End Sub
End Class
