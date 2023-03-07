Imports System.IO

Public MustInherit Class DialerBaseClass
    Public Class DialEventArgs
        Inherits System.EventArgs

        Public Number As String

        Public Sub New()
            Me.Number = ""
        End Sub

        Public Sub New(ByVal number As String)
            Me.Number = number
        End Sub

    End Class



    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Event BeginDial(ByVal sender As Object, ByVal e As DialEventArgs)

    Public Event EndDial(ByVal sender As Object, ByVal e As DialEventArgs)


    Public MustOverride ReadOnly Property Name As String

    Public MustOverride Function IsInstalled() As Boolean



    Public MustOverride Sub Dial(ByVal number As String)

    Protected Overridable Sub OnBegidDial(ByVal e As DialEventArgs)
        RaiseEvent BeginDial(Me, e)
    End Sub

    Protected Overridable Sub OnEndDial(ByVal e As DialEventArgs)
        RaiseEvent BeginDial(Me, e)
    End Sub


    Public MustOverride Sub HangUp()

    '  Private Delegate Function fH(ByVal number As String) As Integer

    'Private Class Handler
    '    Public d As DialerBaseClass

    '    Public Sub New(ByVal d As DialerBaseClass)
    '        DMD.DMDObject.IncreaseCounter(Me)
    '        Me.d = d
    '    End Sub

    '    Protected Overrides Sub Finalize()
    '        MyBase.Finalize()
    '        DMD.DMDObject.DecreaseCounter(Me)
    '    End Sub
    'End Class

    'Public Sub DialAsync(ByVal number As String)
    '    Dim a As fH = AddressOf Me.Dial
    '    a.BeginInvoke(number, AddressOf cb, New Handler(Me))
    'End Sub

    'Private Sub cb(ByVal a As IAsyncResult)
    '    Dim h As Handler = DirectCast(a.AsyncState, Handler)
    '    h.d.doDialComplete(New System.EventArgs)
    'End Sub

    'Friend Sub doDialComplete(ByVal e As System.EventArgs)
    '    RaiseEvent EndDial(Me, e)
    'End Sub

    Protected Shared Function GetProgramFilesFolder() As String
        Return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
    End Function

    Protected Shared Function GetRoamingFilesFolder() As String
        Return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    End Function

    Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If (Me Is obj) Then Return True
            If Not (TypeOf (obj) Is DialerBaseClass) Then Return False
            With DirectCast(obj, DialerBaseClass)
                Return Me.Name = .Name
            End With
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

