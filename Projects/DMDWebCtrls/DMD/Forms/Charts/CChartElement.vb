Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms

   
    Public Enum ChartElementPosition
        Left = 0
        Top = 1
        Right = 2
        Bottom = 3
    End Enum

    <Serializable> _
    Public MustInherit Class CChartElement
        Implements DMD.XML.IDMDXMLSerializable

        <NonSerialized> Private m_Chart As CChart
        Private m_Text As String
        Private m_ForeColor As System.Drawing.Color
        Private m_BackColor As System.Drawing.Color
        Private m_Position As ChartElementPosition
        Private m_Visible As Boolean
        Private m_BorderSize As Single
        Private m_BorderColor As System.Drawing.Color
        Private m_Font As CFont


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Chart = Nothing
            Me.m_Text = ""
            Me.m_ForeColor = Drawing.Color.Black
            Me.m_BackColor = Drawing.Color.White
            Me.m_Position = ChartElementPosition.Right
            Me.m_Visible = True
            Me.m_BorderSize = 1
            Me.m_BorderColor = Drawing.Color.Gray
            Me.m_Font = Nothing
        End Sub

        Public Sub New(ByVal chart As CChart)
            Me.New()
            Me.SetChart(chart)
        End Sub

        Public ReadOnly Property Chart As CChart
            Get
                Return Me.m_Chart
            End Get
        End Property

        Protected Friend Overridable Sub SetChart(ByVal value As CChart)
            Me.m_Chart = value
        End Sub

        Public Property Text As String
            Get
                Return Me.m_Text
            End Get
            Set(value As String)
                Me.m_Text = value
            End Set
        End Property

        Public Property Font As CFont
            Get
                Return Me.m_Font
            End Get
            Set(value As CFont)
                If (Me.m_Font Is value) Then Return
                Me.m_Font = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Property ForeColor As System.Drawing.Color
            Get
                Return Me.m_ForeColor
            End Get
            Set(value As System.Drawing.Color)
                Me.m_ForeColor = value
            End Set
        End Property

        Public Property BackColor As System.Drawing.Color
            Get
                Return Me.m_BackColor
            End Get
            Set(value As System.Drawing.Color)
                Me.m_BackColor = value
            End Set
        End Property

        Public Property BorderColor As System.Drawing.Color
            Get
                Return Me.m_BorderColor
            End Get
            Set(value As System.Drawing.Color)
                Me.m_BorderColor = value
            End Set
        End Property

        Public Property BorderSize As Single
            Get
                Return Me.m_BorderSize
            End Get
            Set(value As Single)
                Me.m_BorderSize = value
            End Set
        End Property

        Public Property Position As ChartElementPosition
            Get
                Return Me.m_Position
            End Get
            Set(value As ChartElementPosition)
                Me.m_Position = value
            End Set
        End Property

        Protected Sub InvalidateChart()
            Me.Chart.Invalidate()
        End Sub


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Text" : Me.m_Text = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ForeColor" : Me.m_ForeColor = Utils.FormsUtils.FromRGBSTR(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "BackColor" : Me.m_BackColor = Utils.FormsUtils.FromRGBSTR(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "Position" : Me.m_Position = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Visible" : Me.m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "BorderSize" : Me.m_BorderSize = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "BorderColor" : Me.m_BorderColor = Utils.FormsUtils.FromRGBSTR(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "Font" : Me.m_Font = CType(fieldValue, CFont)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Text", Me.m_Text)
            writer.WriteAttribute("ForeColor", Utils.FormsUtils.ToRGBSTR(Me.m_ForeColor))
            writer.WriteAttribute("BackColor", Utils.FormsUtils.ToRGBSTR(Me.m_BackColor))
            writer.WriteAttribute("Position", Me.Position)
            writer.WriteAttribute("Visible", Me.m_Visible)
            writer.WriteAttribute("BorderSize", Me.m_BorderSize)
            writer.WriteAttribute("BorderColor", Utils.FormsUtils.ToRGBSTR(Me.m_BorderColor))
            writer.WriteTag("Font", Me.m_Font)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace