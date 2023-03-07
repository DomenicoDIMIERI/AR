Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports DIALTPLib
Imports LumiSoft.Media.Wave

<Serializable>
Public Class InterfonoParams
    Public srcID As Integer
    Public codec As Integer
    Public srcIP As String
    Public srcPort As Integer
    Public targetIP As String
    Public targetPort As Integer
    Public Result As String

    Public Sub New()
        Me.codec = 0
    End Sub






End Class
