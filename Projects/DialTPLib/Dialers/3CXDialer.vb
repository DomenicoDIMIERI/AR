Imports System.IO

Public Class C3CXDialer
    Inherits DialerBaseClass

    Private m_Path As String

    Public Sub New()
        Me.m_Path = Get3CXPath()
    End Sub

    Function PrepareNumber(ByVal number As String) As String
        Return Trim(number)
    End Function

    Public Overrides Sub Dial(number As String)
        If Not Me.IsInstalled Then Return
        Shell(Me.m_Path & " dial:" & Me.PrepareNumber(number))
    End Sub

    Public Overrides Function IsInstalled() As Boolean
        Return (Me.m_Path <> vbNullString)
    End Function

    Private Shared Function Get3CXPath() As String
        Dim p As String = Path.Combine(GetProgramFilesFolder, "3CXPhone\3CXPhone.exe")
        If (File.Exists(p)) Then Return p
        p = Path.Combine(GetProgramFilesFolder() & " (x86)", "3CXPhone\3CXPhone.exe")
        If (File.Exists(p)) Then Return p
        Return vbNullString
    End Function

    Public Overrides ReadOnly Property Name As String
        Get
            Return "3CX VoIP Phone"
        End Get
    End Property

    Public Overrides Sub HangUp()

    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf (obj) Is C3CXDialer) Then Return False
        Return MyBase.Equals(obj) AndAlso DirectCast(obj, C3CXDialer).m_Path = Me.m_Path
    End Function

End Class
