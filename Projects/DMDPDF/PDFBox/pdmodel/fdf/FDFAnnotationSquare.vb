Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
'import org.w3c.dom.Element;

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This represents a Square FDF annotation.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class FDFAnnotationSquare
        Inherits FDFAnnotation

        ''' <summary>
        ''' COS Model value for SubType entry.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUBTYPE As String = "Square"

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            MyBase.New()
            annot.setName(COSName.SUBTYPE, SUBTYPE)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param a An existing FDF Annotation.
        ' */
        Public Sub New(ByVal a As COSDictionary)
            MyBase.New(a)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' *  @param element An XFDF element.
        ' *
        ' *  @throws IOException If there is an error extracting information from the element.
        ' */
        Public Sub New(ByVal element As System.Xml.XmlElement) 'throws IOException
            MyBase.New(element)
            annot.setName(COSName.SUBTYPE, SUBTYPE)
        End Sub
    End Class

End Namespace
