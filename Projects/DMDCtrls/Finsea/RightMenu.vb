Imports System.ComponentModel
Imports FinSeA
Imports FinSeA.Sistema
Imports FinSeA.Databases

Public Class RightMenu
    Public Class RightMenuEventArgs
        Inherits System.EventArgs

        Private m_Module As CModule

        Public Sub New()
        End Sub

        Public Sub New(ByVal m As CModule)
            Me.m_Module = m
        End Sub

        Public ReadOnly Property [Module] As CModule
            Get
                Return Me.m_Module
            End Get
        End Property

    End Class

    Public Event ModuleClick(ByVal sender As Object, ByVal e As RightMenuEventArgs)

    Private m_Module As CModule

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property [Module] As CModule
        Get
            Return Me.m_Module
        End Get
        Set(value As CModule)
            If (Me.m_Module Is value) Then Exit Property
            Me.m_Module = value
            Me.Recreate()
        End Set
    End Property

    Private Sub CreateBottomMenu()
        Dim items As CCollection(Of CModule) = Sistema.Modules.GetUserModules(Users.CurrentUser)
        Me.bottomMenu.Controls.Clear()
        For Each m As CModule In items
            If (m.Visible) Then
                Dim btn As New System.Windows.Forms.Button
                btn.Name = "mod" & GetID(m)
                btn.Text = m.DisplayName
                btn.Height = 20
                btn.Dock = Windows.Forms.DockStyle.Top
                btn.TextAlign = Drawing.ContentAlignment.MiddleLeft
                Me.bottomMenu.Controls.Add(btn)
                AddHandler btn.Click, AddressOf btnModule_Click
            End If
        Next
    End Sub

    Private Sub btnModule_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As System.Windows.Forms.Button = sender
        Me.OnModuleClick(New RightMenuEventArgs(btn.Tag))
    End Sub

    Protected Overridable Sub OnModuleClick(ByVal e As RightMenuEventArgs)
        RaiseEvent ModuleClick(Me, e)
    End Sub


    Public Sub Recreate()
        Me.CreateBottomMenu()
    End Sub
End Class
