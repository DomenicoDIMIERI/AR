Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing


Namespace Controls


    ''' <summary>
    ''' The methods of animation available.
    ''' </summary>
    Public Enum AnimationMethod
        ''' <summary>
        ''' Rolls out from edge when showing and into edge when hiding.
        ''' </summary>
        ''' <remarks>
        ''' This is the default animation method and requires a direction.
        ''' </remarks>
        Roll = &H0
        ''' <summary>
        ''' Expands out from centre when showing and collapses into centre when hiding.
        ''' </summary>
        Centre = &H10
        ''' <summary>
        ''' Slides out from edge when showing and slides into edge when hiding.
        ''' </summary>
        ''' <remarks>
        ''' Requires a direction.
        ''' </remarks>
        Slide = &H40000
        ''' <summary>
        ''' Fades from transaprent to opaque when showing and from opaque to transparent when hiding.
        ''' </summary>
        Blend = &H80000
    End Enum


    ''' <summary>
    ''' The directions in which the Roll and Slide animations can be shown.
    ''' </summary>
    ''' <remarks>
    ''' Horizontal and vertical directions can be combined to create diagonal animations.
    ''' </remarks>
    <Flags()> Public Enum AnimationDirection
        ''' <summary>
        ''' From left to right.
        ''' </summary>
        Right = &H1
        ''' <summary>
        ''' From right to left.
        ''' </summary>
        Left = &H2
        ''' <summary>
        ''' From top to bottom.
        ''' </summary>
        Down = &H4
        ''' <summary>
        ''' From bottom to top.
        ''' </summary>
        Up = &H8
    End Enum

    Public NotInheritable Class Toaster

        ''' <summary>
        ''' The list of currently open ToastForms.
        ''' </summary>
        Friend Shared openForms As New List(Of ToastForm)

        Private Sub New()
        End Sub

        Friend Shared Sub Remove(ByVal frm As ToastForm)
            SyncLock openForms
                'Move down any open forms above this one.
                For Each openForm As ToastForm In openForms
                    If openForm Is frm Then
                        'The remaining forms are below this one.
                        Exit For
                    End If

                    openForm.Top += frm.Height + 5
                Next

                'Remove this form from the open form list.
                openForms.Remove(frm)
            End SyncLock
        End Sub

        Public Shared Function Show(ByVal text As String, ByVal duration As Integer)
            SyncLock openForms
                Dim frm As New ToastForm(duration, text)


                'Display the form just above the system tray.
                Dim p As Point = New Point(Screen.PrimaryScreen.WorkingArea.Width - frm.Width - 5, Screen.PrimaryScreen.WorkingArea.Height - frm.Height - 5)
                frm.StartPosition = FormStartPosition.Manual
                frm.Location = p

                'Move each open form upwards to make room for this one.
                For Each openForm As ToastForm In openForms
                    openForm.Top -= frm.Height + 5
                Next

                'Add this form from the open form list.
                openForms.Add(frm)

                frm.Visible = True
                frm.TopMost = True
                frm.Show()
                frm.BringToFront()

                System.Windows.Forms.Application.DoEvents()

                Return frm

            End SyncLock
        End Function


    End Class


End Namespace
