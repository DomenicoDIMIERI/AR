Namespace PDF.Elements


    Public MustInherit Class PDFColorElement
        Inherits PDFObject

        Private m_R As Single
        Private m_G As Single
        Private m_B As Single

        Public Sub New()
        End Sub

        Public Sub New(ByVal r As Single, ByVal g As Single, ByVal b As Single)
            Me.m_R = r
            Me.m_G = g
            Me.m_B = b
        End Sub

        Public Sub New(ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
            Me.m_R = r / 255
            Me.m_G = g / 255
            Me.m_B = b / 255
        End Sub

        Public Sub New(ByVal color As System.Drawing.Color)
            Me.m_R = color.R / 255
            Me.m_G = color.G / 255
            Me.m_B = color.B / 255
        End Sub

        Public Property Color As System.Drawing.Color
            Get
                Return System.Drawing.Color.FromArgb(Me.m_R * 255, Me.m_G * 255, Me.m_B * 255)
            End Get
            Set(value As System.Drawing.Color)
                Me.m_R = value.R / 255
                Me.m_G = value.G / 255
                Me.m_B = value.B / 255
            End Set
        End Property

        Public Property Red As Single
            Get
                Return Me.m_R
            End Get
            Set(value As Single)
                Me.m_R = value
            End Set
        End Property

        Public Property Green As Single
            Get
                Return Me.m_G
            End Get
            Set(value As Single)
                Me.m_G = value
            End Set
        End Property


        Public Property Blue As Single
            Get
                Return Me.m_B
            End Get
            Set(value As Single)
                Me.m_B = value
            End Set
        End Property


    End Class


End Namespace