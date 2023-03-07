Imports System.ComponentModel

Public Class FontItem

    Private m_SampleFont As String
    Private m_SampleSize As Single
    Private m_SampleText As String
    Private m_SampleLeft As Integer

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.m_SampleFont = Nothing
        Me.m_SampleText = ""
        Me.m_SampleLeft = 200
    End Sub

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property SampleFont As String
        Get
            Return Me.m_SampleFont
        End Get
        Set(value As String)
            If (Me.m_SampleFont = value) Then Exit Property
            Me.m_SampleFont = value
            Me.Refresh()
        End Set
    End Property

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property SampleSize As Single
        Get
            Return Me.m_SampleSize
        End Get
        Set(value As Single)
            If (Me.m_SampleSize = value) Then Exit Property
            Me.m_SampleSize = value
            Me.Refresh()
        End Set
    End Property


    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property SampleText As String
        Get
            Return Me.m_SampleText
        End Get
        Set(value As String)
            If (Me.m_SampleText = value) Then Exit Property
            Me.m_SampleText = value
            Me.Refresh()
        End Set
    End Property

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property SampleLeft As Integer
        Get
            Return Me.m_SampleLeft
        End Get
        Set(value As Integer)
            If (Me.m_SampleLeft = value) Then Exit Property
            Me.m_SampleLeft = value
            Me.Refresh()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As System.Drawing.Graphics = e.Graphics
        g.DrawString(Me.SampleFont, Me.Font, Brushes.Black, 1, 1)

        Try
            Dim f As New System.Drawing.Font(Me.SampleFont, Me.SampleSize)
            g.DrawString(Me.SampleText, f, Brushes.Blue, Me.m_SampleLeft, 1)
            f.Dispose()
        Catch ex As Exception
            g.DrawString(ex.Message, Me.Font, Brushes.Red, Me.m_SampleLeft, 1)

        End Try
    End Sub



End Class
