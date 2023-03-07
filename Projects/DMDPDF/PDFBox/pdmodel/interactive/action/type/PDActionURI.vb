Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.action.type

    '/**
    ' * This represents a URI action that can be executed in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDActionURI
        Inherits PDAction

        ''' <summary>
        ''' This type of action this object represents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "URI"

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            action = New COSDictionary()
            setSubType(SUB_TYPE)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param a The action dictionary.
        ' */
        Public Sub New(ByVal a As COSDictionary)
            MyBase.New(a)
        End Sub

        '    '/**
        '    ' * Convert this standard java object to a COS object.
        '    ' *
        '    ' * @return The cos object that matches this Java object.
        '    ' */
        '    Public Function getCOSObject() As COSBase
        '{
        '    return action;
        '}

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        'public COSDictionary getCOSDictionary()
        '{
        '    return action;
        '}

        '/**
        ' * This will get the type of action that the actions dictionary describes.
        ' * It must be URI for a URI action.
        ' *
        ' * @return The S entry of the specific URI action dictionary.
        ' */
        Public Function getS() As String
            Return action.getNameAsString("S")
        End Function

        '/**
        ' * This will set the type of action that the actions dictionary describes.
        ' * It must be URI for a URI action.
        ' *
        ' * @param s The URI action.
        ' */
        Public Sub setS(ByVal s As String)
            action.setName("S", s)
        End Sub

        '/**
        ' * This will get the uniform resource identifier to resolve, encoded in 7-bit ASCII.
        ' *
        ' * @return The URI entry of the specific URI action dictionary.
        ' */
        Public Function getURI() As String
            Return action.getString("URI")
        End Function

        '/**
        ' * This will set the uniform resource identifier to resolve, encoded in 7-bit ASCII.
        ' *
        ' * @param uri The uniform resource identifier.
        ' */
        Public Sub setURI(ByVal uri As String)
            action.setString("URI", uri)
        End Sub

        '/**
        ' * This will specify whether to track the mouse position when the URI is resolved.
        ' * Default value: false.
        ' * This entry applies only to actions triggered by the user's clicking an annotation;
        ' * it is ignored for actions associated with outline items or with a document's OpenAction entry.
        ' *
        ' * @return A flag specifying whether to track the mouse position when the URI is resolved.
        ' */
        Public Function shouldTrackMousePosition() As Boolean
            Return Me.action.getBoolean("IsMap", False)
        End Function

        '/**
        ' * This will specify whether to track the mouse position when the URI is resolved.
        ' *
        ' * @param value The flag value.
        ' */
        Public Sub setTrackMousePosition(ByVal value As Boolean)
            Me.action.setBoolean("IsMap", value)
        End Sub

        '// TODO this must go into PDURIDictionary
        '/**
        ' * This will get the base URI to be used in resolving relative URI references.
        ' * URI actions within the document may specify URIs in partial form, to be interpreted
        ' * relative to this base address. If no base URI is specified, such partial URIs
        ' * will be interpreted relative to the location of the document itself.
        ' * The use of this entry is parallel to that of the body element &lt;BASE&gt;, as described
        ' * in the HTML 4.01 Specification.
        ' *
        ' * @return The URI entry of the specific URI dictionary.
        ' * @deprecated use {@link PDURIDictionary#getBase()} instead
        ' */
        Public Function getBase() As String
            Return action.getString("Base")
        End Function

        '// TODO this must go into PDURIDictionary
        '/**
        ' * This will set the base URI to be used in resolving relative URI references.
        ' * URI actions within the document may specify URIs in partial form, to be interpreted
        ' * relative to this base address. If no base URI is specified, such partial URIs
        ' * will be interpreted relative to the location of the document itself.
        ' * The use of this entry is parallel to that of the body element &lt;BASE&gt;, as described
        ' * in the HTML 4.01 Specification.
        ' *
        ' * @param base The the base URI to be used.
        ' * @deprecated use {@link PDURIDictionary#setBase(String)} instead
        ' */
        Public Sub setBase(ByVal base As String)
            action.setString("Base", base)
        End Sub
    End Class

End Namespace
