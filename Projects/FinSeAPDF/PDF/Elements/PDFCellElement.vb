Namespace PDF.Elements

    Public Class PDFCellElement
        Inherits PDFObject

        Private m_xw As Single
        Private m_xh As Single
        Private m_xtxt As String
        Private m_xborder As Single
        Private m_xln As Single
        Private m_xalign As String
        Private m_xfill As Single
        Private m_xlink As String

        Public Sub New()
            Me.m_xw = 0
            Me.m_xh = 0
            Me.m_xtxt = vbNullString
            Me.m_xborder = 0
            Me.m_xln = 0
            Me.m_xalign = vbNullString
            Me.m_xfill = 0
            Me.m_xlink = vbNullString
        End Sub

        Public Property xW As Single
            Get
                Return Me.m_xw
            End Get
            Set(value As Single)
                Me.m_xw = value
            End Set
        End Property

        Public Property xH As Single
            Get
                Return Me.m_xh
            End Get
            Set(value As Single)
                Me.m_xh = value
            End Set
        End Property

        Public Property xTxt As String
            Get
                Return Me.m_xtxt
            End Get
            Set(value As String)
                Me.m_xtxt = value
            End Set
        End Property

        Public Property xBorder As Single
            Get
                Return Me.m_xborder
            End Get
            Set(value As Single)
                Me.m_xborder = value
            End Set
        End Property

        Public Property xLn As Single
            Get
                Return Me.m_xln
            End Get
            Set(value As Single)
                Me.m_xln = value
            End Set
        End Property

        Public Property xAlign As String
            Get
                Return Me.m_xalign
            End Get
            Set(value As String)
                Me.m_xalign = value
            End Set
        End Property

        Public Property xFill As Single
            Get
                Return Me.m_xfill
            End Get
            Set(value As Single)
                Me.m_xfill = value
            End Set
        End Property

        Public Property xLink As String
            Get
                Return Me.m_xlink
            End Get
            Set(value As String)
                Me.m_xlink = value
            End Set
        End Property

        Friend Overrides Sub Write(writer As PDFWriter)
            writer.Cell(Me.xW, Me.xH, Me.xTxt, Me.xBorder, Me.xLn, Me.xAlign, Me.xFill, Me.xLink)
        End Sub

    End Class


End Namespace