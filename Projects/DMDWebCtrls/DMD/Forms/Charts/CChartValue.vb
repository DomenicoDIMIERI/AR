Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms

    <Serializable> _
    Public Class CChartValue
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Private m_Label As String
        Private m_X As Single
        Private m_Value As Single?
        Private m_Color As System.Drawing.Color
        <NonSerialized> Private m_Serie As CChartSerie
        Private m_RenderColor As System.Drawing.Color = Drawing.Color.Empty

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Label = ""
            Me.m_X = 0
            Me.m_Value = Nothing
            Me.m_Color = Drawing.Color.Empty
            Me.m_Serie = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce il grafico a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Chart As CChart
            Get
                If (Me.m_Serie) Is Nothing Then Return Nothing
                Return Me.m_Serie.Chart
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la serie a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Serie As CChartSerie
            Get
                Return Me.m_Serie
            End Get
        End Property
        Protected Friend Sub SetSerie(ByVal value As CChartSerie)
            Me.m_Serie = value
        End Sub

        ''' <summary>
        ''' Restituisce il valore dell'etichetta visualizzata in corrispondenza di questo elemento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Label As String
            Get
                Return Me.m_Label
            End Get
            Set(value As String)
                Me.m_Label = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore dell'elemento (ordinata)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Value As Single?
            Get
                Return Me.m_Value
            End Get
            Set(value As Single?)
                If (value = Me.m_Value) Then Exit Property
                Me.m_Value = value
                Me.InvalidateChart()
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la posizione dell'oggetto (ascissa)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property X As Single
            Get
                Return Me.m_X
            End Get
            Set(value As Single)
                If (Me.m_X = value) Then Exit Property
                Me.m_X = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Property Color As System.Drawing.Color
            Get
                Return Me.m_Color
            End Get
            Set(value As System.Drawing.Color)
                Me.m_Color = value
            End Set
        End Property

        Private Function CompareTo(obj As CChartValue) As Integer
            Return Arrays.Compare(Me.m_Value, obj.m_Value)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Sub InvalidateChart()
            If (Me.Chart IsNot Nothing) Then Me.Chart.Invalidate()
        End Sub

        Public Function GetRenderColor() As System.Drawing.Color
            If Me.Color.Equals(System.Drawing.Color.Empty) Then
                If (Me.m_RenderColor.Equals(System.Drawing.Color.Empty)) Then
                    Dim i As Integer = Me.Serie.Values.IndexOf(Me)
                    If (i >= 0 AndAlso i <= UBound(CChart.DefaultColors)) Then
                        Me.m_RenderColor = Colors.FromWeb(CChart.DefaultColors(i))
                    Else
                        Me.m_RenderColor = Utils.FormsUtils.GetRandomColor
                    End If
                End If
                Return Me.m_RenderColor
            Else
                Return Me.Color
            End If
        End Function


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Label" : Me.m_Label = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "X" : Me.m_X = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Value" : Me.m_Value = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Color" : Me.m_Color = Utils.FormsUtils.FromRGBSTR(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "RenderColor", Me.m_RenderColor = Utils.FormsUtils.FromRGBSTR(XML.Utils.Serializer.DeserializeString(fieldValue))
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Label", Me.m_Label)
            writer.WriteAttribute("X", Me.m_X)
            writer.WriteAttribute("Value", Me.m_Value)
            writer.WriteAttribute("Color", Utils.FormsUtils.ToRGBSTR(Me.m_Color))
            writer.WriteAttribute("RenderColor", Utils.FormsUtils.ToRGBSTR(Me.m_RenderColor))
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace