Imports FinSeA.org.apache.fontbox.afm
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.encoding


    '/**
    ' * This will handle the encoding from an AFM font.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.8 $
    ' */
    Public Class AFMEncoding
        Inherits Encoding

        Private metric As FontMetric = Nothing

        '/**
        ' * Constructor.
        ' *
        ' * @param fontInfo The font metric information.
        ' */
        Public Sub New(ByVal fontInfo As FontMetric)
            Me.metric = fontInfo
            Dim characters As Iterator(Of CharMetric) = metric.getCharMetrics().GetEnumerator()
            While (characters.hasNext())
                Dim nextMetric As CharMetric = characters.next()
                addCharacterEncoding(nextMetric.getCharacterCode(), nextMetric.getName())
            End While
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return Nothing
        End Function

    End Class

End Namespace