Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases

Partial Public Class Sistema

    Public Class SysDebuInfo
        Implements DMD.XML.IDMDXMLSerializable

        Public MemoryStatus As New CKeyCollection
        Public DiskStatus As New CKeyCollection
        Public Notes As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub


        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "MemoryStatus" : Me.MemoryStatus = fieldValue
                Case "DiskStatus" : Me.MemoryStatus = fieldValue
                Case "Notes" : Me.Notes = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("MemoryStatus", Me.MemoryStatus)
            writer.WriteTag("DiskStatus", Me.DiskStatus)
            writer.WriteTag("Notes", Me.Notes)
        End Sub

        Public Overridable Sub Initialize()
            Dim rootPath As String = Left(ApplicationContext.MapPath("/"), 1)
            Me.DiskStatus.Add("FreeSpace (" & rootPath & ":)", Me.GetDiskFreeSpace(rootPath))
            Me.DiskStatus.Add("TotalSize (" & rootPath & ":)", Me.GetDiskTotalSize(rootPath))
            Me.MemoryStatus.Add("TotalPhysicalMemory", Me.GetTotalPhysicalMemory)
            Me.MemoryStatus.Add("TotalVirtualMemory", Me.GetTotalVirtualMemory)
            Me.MemoryStatus.Add("AvailablePhysicalMemory", Me.GetAvailablePhysicalMemory)
            Me.MemoryStatus.Add("GetAvailableVirtualMemory", Me.GetAvailableVirtualMemory)

            Me.Notes = ""
            Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine
            Me.Notes &= "STATO DEL SISTEMA" & vbNewLine
            Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine
            Me.Notes &= vbNewLine
            Me.Notes &= "FreeSpace (" & rootPath & "): " & Formats.FormatBytes(Me.GetDiskFreeSpace(rootPath)) & vbNewLine
            Me.Notes &= "TotalSize (" & rootPath & "): " & Formats.FormatBytes(Me.GetDiskTotalSize(rootPath)) & vbNewLine
            Me.Notes &= "Physical Memory: " & Formats.FormatBytes(Me.GetAvailablePhysicalMemory) & " / " & Formats.FormatBytes(Me.GetTotalPhysicalMemory) & vbNewLine
            Me.Notes &= "Virtual Memory: " & Formats.FormatBytes(Me.GetAvailableVirtualMemory) & " / " & Formats.FormatBytes(Me.GetTotalVirtualMemory) & vbNewLine
            Me.Notes &= vbNewLine
            Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine
            Me.Notes &= "ELENCO DELLE CONNESSIONI DB APERTE" & vbNewLine
            Me.Notes &= DMD.Sistema.Strings.NChars(80, "-") & vbNewLine
            For Each db As CDBConnection In DMD.Databases.DBUtils.GetAllOpenedConnections
                Me.Notes &= db.Path & " (" & Formats.FormatBytes(Me.GetFileSize(db.Path)) & ")" & vbNewLine
            Next


        End Sub

        Private Function GetDiskTotalSize(ByVal c As String) As String
            Try
                Return DMD.Sistema.FileSystem.GetDiskTotalSpace(c)
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Private Function GetDiskFreeSpace(ByVal c As String) As String
            Try
                Return DMD.Sistema.FileSystem.GetDiskFreeSpace(c)
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Private Function GetFileSize(ByVal c As String) As String
            Try
                Return DMD.Sistema.FileSystem.GetFileSize(c)
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Private Function GetTotalPhysicalMemory() As String
            Try
                Return My.Computer.Info.TotalPhysicalMemory
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Private Function GetAvailablePhysicalMemory() As String
            Try
                Return My.Computer.Info.AvailablePhysicalMemory
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Private Function GetTotalVirtualMemory() As String
            Try
                Return My.Computer.Info.TotalVirtualMemory
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Private Function GetAvailableVirtualMemory() As String
            Try
                Return My.Computer.Info.AvailableVirtualMemory
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class

