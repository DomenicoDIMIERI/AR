Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms

    <Serializable> _
    Public Class CChartLabel
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        <NonSerialized> Private m_Char As CChart
        Private m_Label As String
        Private m_X As Single
        Private m_Color As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Label = ""
            Me.m_X = 0
            Me.m_Color = ""
            Me.m_Char = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce il grafico a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Chart As CChart
            Get
                Return Me.m_Char
            End Get
        End Property

        Protected Friend Sub SetChart(ByVal value As CChart)
            Me.m_Char = value
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

        Public Property Color As String
            Get
                Return Me.m_Color
            End Get
            Set(value As String)
                Me.m_Color = Trim(value)
            End Set
        End Property

        Private Function CompareTo(obj As CChartLabel) As Integer
            If (Me.m_X < obj.m_X) Then Return -1
            If (Me.m_X > obj.m_X) Then Return 1
            Return 0
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Sub InvalidateChart()
            If (Me.Chart IsNot Nothing) Then Me.Chart.Invalidate()
        End Sub

        Protected Friend Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Label" : Me.m_Label = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "X" : Me.m_X = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Color" : Me.m_Color = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Friend Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Label", Me.m_Label)
            writer.WriteAttribute("X", Me.m_X)
            writer.WriteAttribute("Color", Me.m_Color)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Namespace