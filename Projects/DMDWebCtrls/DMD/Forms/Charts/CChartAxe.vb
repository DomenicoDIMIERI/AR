Imports DMD
Imports DMD.Sistema
Imports DMD.Forms


Namespace Forms
 
 
    Public Class CChartAxe
        Inherits CChartElement

        Private m_Name As String
        Private m_DisplayValues As Boolean
        Private m_ScalaLogaritmica As Boolean
        Private m_Decimals As Integer
        Private m_LongStep As Double
        Private m_SmallStep As Double
        Private m_MinValue As Double?
        Private m_MaxValue As Double?

        Public Sub New()
            Me.m_Name = ""
            Me.m_DisplayValues = True
            Me.m_ScalaLogaritmica = False
            Me.m_Decimals = 2
            Me.m_LongStep = 0 'Auto
            Me.m_SmallStep = 0 'Auto
            Me.m_MinValue = Nothing
            Me.m_MaxValue = Nothing
        End Sub

        Public Sub New(ByVal name As String)
            Me.New
            Me.m_Name = Trim(name)
        End Sub

        Public Sub New(ByVal chart As CChart)
            Me.New()
            Me.SetChart(chart)
        End Sub

        Public Sub New(ByVal name As String, ByVal chart As CChart)
            Me.New()
            Me.m_Name = Trim(name)
            Me.SetChart(chart)
        End Sub

        Public Property MinValue As Double?
            Get
                Return Me.m_MinValue
            End Get
            Set(value As Double?)
                If (Me.m_MinValue = value) Then Return
                Me.m_MinValue = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Property MaxValue As Double?
            Get
                Return Me.m_MaxValue
            End Get
            Set(value As Double?)
                If (Me.m_MaxValue = value) Then Return
                Me.m_MaxValue = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Property LongStep As Double
            Get
                Return Me.m_LongStep
            End Get
            Set(value As Double)
                If (Me.m_LongStep = value) Then Return
                Me.m_LongStep = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Property SmallStep As Double
            Get
                Return Me.m_SmallStep
            End Get
            Set(value As Double)
                If (Me.m_SmallStep = value) Then Return
                Me.m_SmallStep = value
                Me.InvalidateChart()
            End Set
        End Property


        Public Property Decimals As Integer
            Get
                Return Me.m_Decimals
            End Get
            Set(value As Integer)
                If (Me.m_Decimals = value) Then Return
                Me.m_Decimals = value
                Me.InvalidateChart()
            End Set
        End Property


        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                Me.m_Name = Trim(value)
            End Set
        End Property

        Public Property ScalaLogaritmica As Boolean
            Get
                Return Me.m_ScalaLogaritmica
            End Get
            Set(value As Boolean)
                If (Me.m_ScalaLogaritmica = value) Then Exit Property
                Me.m_ScalaLogaritmica = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Property ShowValues As Boolean
            Get
                Return Me.m_DisplayValues
            End Get
            Set(value As Boolean)
                If (Me.m_DisplayValues = value) Then Exit Property
                Me.m_DisplayValues = value
                Me.InvalidateChart()
            End Set
        End Property

        Public Function GetSmallStep(ByVal min As Single, ByVal max As Single) As Single
            If (Me.SmallStep > 0) Then Return Me.SmallStep
            Return Me.GetLargeStep(min, max) / 2
        End Function

        Public Function GetLargeStep(ByVal min As Single, ByVal max As Single) As Single
            If (Me.LongStep > 0) Then Return Me.LongStep
            Dim l As Integer = 1
            If (Math.Abs(max) > 0) Then l = Math.Log10(Math.Abs(max)) ' - m.MinY)
            Return Math.Sign(max) * 10 ^ (l - 1)
        End Function

        Public Function GetLowestValue(ByVal min As Single, ByVal max As Single) As Single
            Dim s As Single = Me.GetLargeStep(min, max)
            If (max > 0) Then Return Math.Min(0, Math.Floor(max / s))
            Return 0
        End Function

        Public Function GetUpperValue(ByVal min As Single, ByVal max As Single) As Single
            Dim s As Single = Me.GetLargeStep(min, max)
            If (max > 0) Then Return Math.Max(0, Math.Floor(max / s))
            Return 0
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("DisplayValues", Me.m_DisplayValues)
            writer.WriteAttribute("ScalaLogaritmica", Me.m_ScalaLogaritmica)
            writer.WriteAttribute("Decimals", Me.m_Decimals)
            writer.WriteAttribute("LongStep", Me.m_LongStep)
            writer.WriteAttribute("SmallStep", Me.m_SmallStep)
            writer.WriteAttribute("MinValue", Me.m_MinValue)
            writer.WriteAttribute("MaxValue", Me.m_MaxValue)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DisplayValues" : Me.m_DisplayValues = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ScalaLogaritmica" : Me.m_ScalaLogaritmica = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Decimals" : Me.m_Decimals = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LongStep" : Me.m_LongStep = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SmallStep" : Me.m_SmallStep = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MinValue" : Me.m_MinValue = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MaxValue" : Me.m_MaxValue = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

  

End Namespace