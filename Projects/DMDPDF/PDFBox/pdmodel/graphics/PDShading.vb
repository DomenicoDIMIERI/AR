Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics

    '/**
    ' * This class represents a Shading Pattern color space.
    ' *  See section 4.6.3 of the PDF 1.7 specification.
    ' *
    ' * @author <a href="mailto:Daniel.Wilson@BlackLocustSoftware.com">Daniel wilson</a>
    ' * @version $Revision: 1.0 $
    ' */
    Public Class PDShading
        Implements COSObjectable

        Private DictShading As COSDictionary
        Private shadingname As COSName
        Private domain As COSArray = Nothing
        Private extend As COSArray = Nothing
        Private [function] As PDFunction = Nothing
        Private colorspace As PDColorSpace = Nothing

        ''' <summary>
        ''' The name of this object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "Shading"

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            DictShading = New COSDictionary()
            'DictShading.add( COSName.getPDFName( NAME ) );
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param shading The shading dictionary.
        ' */
        Public Sub New(ByVal name As COSName, ByVal shading As COSDictionary)
            DictShading = shading
            shadingname = name
        End Sub

        '/**
        ' * This will return the name of the object.
        ' *
        ' * @return The name of the object.
        ' */
        Public Function getName() As String
            Return NAME
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return COSName.SHADING
        End Function

        '/**
        '* This will return the name of this particular shading dictionary
        '*
        '* @return The name of the shading dictionary
        '*/
        Public Function getShadingName() As COSName
            Return shadingname
        End Function

        '/**
        '* This will return the ShadingType -- an integer between 1 and 7 that specifies the gradient type.
        '* Required in all Shading Dictionaries.
        '*
        '* @return The Shading Type
        '*/
        Public Function getShadingType() As Integer
            Return DictShading.getInt(COSName.SHADING_TYPE)
        End Function

        '/**
        '* This will return the Color Space.
        '* Required in all Shading Dictionaries.
        '*
        '* @return The Color Space of the shading dictionary
        '*/
        Public Function getColorSpace() As PDColorSpace ' throws IOException
            If (colorspace Is Nothing) Then
                colorspace = PDColorSpaceFactory.createColorSpace(DictShading.getDictionaryObject(COSName.COLORSPACE))
            End If
            Return colorspace
        End Function

        '/**
        '* This will return a boolean flag indicating whether to antialias the shading pattern.
        '*
        '* @return The antialias flag, defaulting to False
        '*/
        Public Function getAntiAlias() As Boolean
            Return DictShading.getBoolean(COSName.ANTI_ALIAS, False)
        End Function

        '/**
        '* Returns the coordinate array used by several of the gradient types. Interpretation depends on the ShadingType.
        '*
        '* @return The coordinate array.
        '*/
        Public Function getCoords() As COSArray
            Return DictShading.getDictionaryObject(COSName.COORDS)
        End Function

        '/**
        '* Returns the function used by several of the gradient types. Interpretation depends on the ShadingType.
        '*
        '* @return The gradient function.
        '*/
        Public Function getFunction() As PDFunction ' throws IOException
            If ([function] Is Nothing) Then
                [function] = PDFunction.create(DictShading.getDictionaryObject(COSName.FUNCTION))
            End If
            Return [function]
        End Function

        '/**
        '* Returns the Domain array used by several of the gradient types. Interpretation depends on the ShadingType.
        '*
        '* @return The Domain array.
        '*/
        Public Function getDomain() As COSArray
            If (domain Is Nothing) Then
                domain = DictShading.getDictionaryObject(COSName.DOMAIN)
                ' use default values
                If (domain Is Nothing) Then
                    domain = New COSArray()
                    domain.add(New COSFloat(0.0F))
                    domain.add(New COSFloat(1.0F))
                End If
            End If
            Return domain
        End Function

        '/**
        '* Returns the Extend array used by several of the gradient types. Interpretation depends on the ShadingType.
        '* Default is {false, false}.
        '*
        '* @return The Extend array.
        '*/
        Public Function getExtend() As COSArray
            If (extend Is Nothing) Then
                extend = DictShading.getDictionaryObject(COSName.EXTEND)
                ' use default values
                If (extend Is Nothing) Then
                    extend = New COSArray()
                    extend.add(COSBoolean.FALSE)
                    extend.add(COSBoolean.FALSE)
                End If
            End If
            Return extend
        End Function

    
        Public Overrides Function toString() As String
            Dim sColorSpace As String
            Dim sFunction As String
            Try
                sColorSpace = getColorSpace().toString()
            Catch e As IOException
                sColorSpace = "Failure retrieving ColorSpace: " & e.ToString()
            End Try
            Try
                sFunction = getFunction().ToString()
            Catch e As IOException
                sFunction = "n/a"
            End Try
            Dim s As String = "Shading " & shadingname.toString & vbLf & vbTab & "ShadingType: " & getShadingType() & vbLf & _
                    vbTab & "ColorSpace: " & sColorSpace & vbLf & _
                    vbTab & "AntiAlias: " & getAntiAlias() & vbLf & _
                    vbTab & "Coords: "
            If (getCoords() IsNot Nothing) Then
                s = s & getCoords().toString()
            End If
            s = s & vbLf & vbTab & "Domain: " & getDomain().toString() & vbLf & _
                    vbTab & "Function: " & sFunction & vbLf & _
                    vbTab & "Extend: " & getExtend().toString() & vbLf & _
                    vbTab & "Raw Value:" & vbLf & DictShading.toString()

            Return s
        End Function

    End Class

End Namespace
