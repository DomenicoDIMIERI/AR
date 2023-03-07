Imports System.IO

Public Class BriaDialer
    Inherits DialerBaseClass

    Private m_Path As String

    Public Sub New()
        Me.m_Path = GetBriaPath()
    End Sub

    Public Overrides Sub Dial(number As String)
        If Not Me.IsInstalled Then Return
        Shell(Me.m_Path & " -dial=sip:" & number)
    End Sub

    Public Overrides Function IsInstalled() As Boolean
        Return (Me.m_Path <> vbNullString)
    End Function

    Private Shared Function GetBriaPath() As String
        Dim p As String = Path.Combine(GetProgramFilesFolder, "CounterPath\Bria\bria.exe")
        If File.Exists(p) Then Return p
        p = Path.Combine(GetProgramFilesFolder() & " (x86)", "CounterPath\Bria\bria.exe")
        If File.Exists(p) Then Return p
        Return vbNullString
    End Function

    Public Overrides ReadOnly Property Name As String
        Get
            Return "CounterPath Bria"
        End Get
    End Property

    Public Overrides Sub HangUp()

    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf (obj) Is C3CXDialer) Then Return False
        Return MyBase.Equals(obj) AndAlso DirectCast(obj, BriaDialer).m_Path = Me.m_Path
    End Function

End Class
