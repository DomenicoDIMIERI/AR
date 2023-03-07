Imports System.Drawing
Imports System.IO
Imports FinSeA.org.apache.fontbox.cff.encoding

Namespace org.apache.fontbox.cff

    '/**
    ' * This class creates all needed AFM font metric data from a CFFFont ready to be read from a AFMPaser.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public Class AFMFormatter

        Private Sub New()
        End Sub

        '/**
        ' * Create font metric data for the given CFFFont.
        ' * @param font the CFFFont
        ' * @return the created font metric data
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Shared Function format(ByVal font As CFFFont) As Byte() 'throws IOException
            Dim output As New DataOutput()
            Dim ret() As Byte
            printFont(font, output)
            ret = output.getBytes()
            output.Dispose()
            Return ret
        End Function

        Private Shared Sub printFont(ByVal font As CFFFont, ByVal output As DataOutput) 'throws IOException
            printFontMetrics(font, output)
        End Sub

        '@SuppressWarnings(value = { "unchecked" })
        Private Shared Sub printFontMetrics(ByVal font As CFFFont, ByVal output As DataOutput) 'throws IOException
            Dim metrics As List(Of CharMetric) = renderFont(font)
            output.println("StartFontMetrics 2.0")
            output.println("FontName " & font.getName())
            output.println("FullName " & font.getProperty("FullName"))
            output.println("FamilyName " & font.getProperty("FamilyName"))
            output.println("Weight " & font.getProperty("Weight"))
            Dim encoding As CFFEncoding = font.getEncoding()
            If (encoding.isFontSpecific()) Then
                output.println("EncodingScheme FontSpecific")
            End If
            Dim bounds As RectangleF = getBounds(metrics)
            output.println("FontBBox " & CInt(bounds.Left) & " " & CInt(bounds.Top) & " " & CInt(bounds.Right) & " " & CInt(bounds.Bottom))
            printDirectionMetrics(font, output)
            printCharMetrics(font, metrics, output)
            output.println("EndFontMetrics")
        End Sub

        Private Shared Sub printDirectionMetrics(ByVal font As CFFFont, ByVal output As DataOutput) 'throws IOException
            output.println("UnderlinePosition " & font.getProperty("UnderlinePosition"))
            output.println("UnderlineThickness " & font.getProperty("UnderlineThickness"))
            output.println("ItalicAngle " & font.getProperty("ItalicAngle"))
            output.println("IsFixedPitch " & font.getProperty("isFixedPitch"))
        End Sub

        Private Shared Sub printCharMetrics(ByVal font As CFFFont, ByVal metrics As List(Of CharMetric), ByVal output As DataOutput) 'throws IOException
            output.println("StartCharMetrics " & metrics.size())
            Collections.sort(Of CharMetric)(metrics)
            For Each metric As CharMetric In metrics
                output.print("C " & metric.code & " ;")
                output.print(" ")
                output.print("WX " & metric.width & " ;")
                output.print(" ")
                output.print("N " & metric.name & " ;")
                output.print(" ")
                output.print("B " & CInt(metric.bounds.X) & " " & CInt(metric.bounds.Y) & " " & CInt(metric.bounds.Right) & " " & CInt(metric.bounds.Bottom) & " ;")
                output.println()
            Next
            output.println("EndCharMetrics")
        End Sub

        Private Shared Function renderFont(ByVal font As CFFFont) As List(Of CharMetric)  'throws IOException
            Dim metrics As List(Of CharMetric) = New ArrayList(Of CharMetric)()
            Dim renderer As CharStringRenderer = font.createRenderer()
            Dim mappings As ICollection(Of CFFFont.Mapping) = font.getMappings()
            For Each mapping As CFFFont.Mapping In mappings
                Dim metric As CharMetric = New CharMetric()
                metric.code = mapping.getCode()
                metric.name = mapping.getName()
                renderer.render(mapping.toType1Sequence())
                metric.width = renderer.getWidth()
                metric.bounds = renderer.getBounds()
                metrics.add(metric)
            Next
            Return metrics
        End Function

        Private Shared Function getBounds(ByVal metrics As List(Of CharMetric)) As RectangleF
            Dim bounds As Nullable(Of RectangleF) = Nothing
            For Each metric As CharMetric In metrics
                If (bounds.HasValue = False) Then
                    bounds = New RectangleF(metric.bounds.Location, metric.bounds.Size)
                    'bounds.setFrame(metric.bounds)
                Else
                    bounds = RectangleF.Union(metric.bounds, bounds.Value)
                End If
            Next
            Return bounds
        End Function

        '/**
        ' * This class represents the metric of one single character. 
        ' *
        ' */
        Private Class CharMetric
            Implements IComparable(Of CharMetric)

            Friend code As Integer
            Friend name As String
            Friend width As Integer
            Friend bounds As RectangleF

            Public Function compareTo(ByVal that As CharMetric) As Integer Implements IComparable(Of FinSeA.org.apache.fontbox.cff.AFMFormatter.CharMetric).CompareTo
                Return code - that.code
            End Function
        End Class

    End Class

End Namespace