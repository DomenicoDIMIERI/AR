Imports DMD
Imports DMD.Sistema
Imports DMD.Forms

Namespace Forms

    Public NotInheritable Class Colors

        Public Shared Function FromWeb(ByVal value As String) As System.Drawing.Color
            Dim r, g, b As Byte
            value = Right("000000" & Replace(value, " ", ""), 6)
            r = CByte(CInt("&H" & Left(value, 2)))
            g = CByte(CInt("&H" & Mid(value, 3, 2)))
            b = CByte(CInt("&H" & Right(value, 2)))
            Return System.Drawing.Color.FromArgb(255, r, g, b)
        End Function

    End Class

    <Serializable>
    Public Class CChartSerie
        Inherits CChartElement

        Private m_Name As String
        Private m_Values As CChartValues
        Private m_Tag As Object
        Private m_ShowLabels As Boolean
        Private m_RenderColor As System.Drawing.Color
        Private m_Tipo As ChartTypes

        Public Sub New()
            Me.m_Tipo = ChartTypes.Unset
            Me.m_Name = ""
            Me.m_Values = Nothing
            Me.m_ShowLabels = False
            Me.m_RenderColor = System.Drawing.Color.Empty
            Me.BackColor = Drawing.Color.Empty
        End Sub

        Public Property Tipo As ChartTypes
            Get
                Return Me.m_Tipo
            End Get
            Set(value As ChartTypes)
                Dim oldValue As ChartTypes = Me.m_Tipo
                If (oldValue = value) Then Return
                Me.m_Tipo = value
                'Me.DoChanged("Tipo", value, oldValue)
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

        Public ReadOnly Property Values As CChartValues
            Get
                If Me.m_Values Is Nothing Then Me.m_Values = New CChartValues(Me)
                Return Me.m_Values
            End Get
        End Property

        Public Property Tag As Object
            Get
                Return Me.m_Tag
            End Get
            Set(value As Object)
                Me.m_Tag = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce le misure dei valori della serie
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMeasures() As ChartMeasure
            Return Me.Values.GetMeasures
        End Function

        Public Property ShowLabels() As Boolean
            Get
                Return Me.m_ShowLabels
            End Get
            Set(value As Boolean)
                If (Me.m_ShowLabels = value) Then Exit Property
                Me.m_ShowLabels = value
                Me.InvalidateChart()
            End Set
        End Property

        Private Function GetRandomColor() As System.Drawing.Color
            Dim r, g, b As Byte
            r = Rnd(1) * 255
            g = Rnd(1) * 255
            b = Rnd(1) * 255
            Return System.Drawing.Color.FromArgb(255, r, g, b)
        End Function

        Public Function GetRenderColor() As System.Drawing.Color
            If Me.BackColor.Equals(System.Drawing.Color.Empty) Then
                If (Me.m_RenderColor.Equals(System.Drawing.Color.Empty)) Then
                    Dim i As Integer = Me.Chart.Series.IndexOf(Me)
                    If (i >= 0 AndAlso i <= UBound(CChart.DefaultColors)) Then
                        Me.m_RenderColor = Colors.FromWeb(CChart.DefaultColors(i))
                    Else
                        Me.m_RenderColor = Me.GetRandomColor
                    End If
                End If
                Return Me.m_RenderColor
            Else
                Return Me.BackColor
            End If
        End Function


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("ShowLabels", Me.m_ShowLabels)
            writer.WriteAttribute("RenderColor", Utils.FormsUtils.ToRGBSTR(Me.m_RenderColor))
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Values", Me.Values)
            'm_Tag As Object
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ShowLabels" : Me.m_ShowLabels = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "RenderColor" : Me.m_RenderColor = Utils.FormsUtils.FromRGBSTR(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "Values" : Me.m_Values = fieldValue : Me.m_Values.SetSerie(Me)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

    End Class


End Namespace