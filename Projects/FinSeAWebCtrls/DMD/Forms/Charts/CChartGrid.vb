Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 
 
    Public Class CChartGrid
        Inherits CChartElement

        Private m_ShowSecondaryGrid As Boolean = False
        Private m_ShowXGrid As Boolean = True
        Private m_ShowYGrid As Boolean = True
        
        Public Sub New()
        End Sub

        Public Sub New(ByVal chart As CChart)
            Me.New()
            Me.SetChart(chart)
        End Sub
          
        Public Property ShowXGrid As Boolean
            Get
                Return Me.m_ShowXGrid
            End Get
            Set(value As Boolean)
                If (Me.m_ShowXGrid = value) Then Exit Property
                Me.m_ShowXGrid = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Property ShowYGrid As Boolean
            Get
                Return Me.m_ShowYGrid
            End Get
            Set(value As Boolean)
                If (Me.m_ShowYGrid = value) Then Exit Property
                Me.m_ShowYGrid = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Property ShowSecondaryGrid As Boolean
            Get
                Return Me.m_ShowSecondaryGrid
            End Get
            Set(value As Boolean)
                If (Me.m_ShowSecondaryGrid = value) Then Return
                Me.m_ShowSecondaryGrid = value
                Me.InvalidateChart()
            End Set
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("ShowXGrid", Me.m_ShowXGrid)
            writer.WriteAttribute("ShowYGrid", Me.m_ShowYGrid)
            writer.WriteAttribute("ShowSecondaryGrid", Me.m_ShowSecondaryGrid)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "ShowXGrid" : Me.m_ShowXGrid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ShowYGrid" : Me.m_ShowYGrid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ShowSecondaryGrid" : Me.m_ShowSecondaryGrid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub
    End Class

  

End Namespace