Imports System.Drawing
Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.pattern
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.Io

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * This class represents a color space in a pdf document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.11 $
    ' */
    Public NotInheritable Class PDColorSpaceFactory

        '/**
        ' * Private constructor for utility classes.
        ' */
        Private Sub New()
            'utility class should not be implemented
        End Sub

        '/**
        ' * This will create the correct color space given the name.
        ' *
        ' * @param colorSpace The color space object.
        ' *
        ' * @return The color space.
        ' *
        ' * @throws IOException If the color space name is unknown.
        ' */
        Public Shared Function createColorSpace(ByVal colorSpace As COSBase) As PDColorSpace 'throws IOException
            Return createColorSpace(colorSpace, Nothing)
        End Function

        '/**
        ' * This will create the correct color space given the name.
        ' *
        ' * @param colorSpace The color space object.
        ' * @param colorSpaces The ColorSpace dictionary from the current resources, if any.
        ' *
        ' * @return The color space.
        ' *
        ' * @throws IOException If the color space name is unknown.
        ' */
        Public Shared Function createColorSpace(ByVal colorSpace As COSBase, ByVal colorSpaces As Map(Of String, PDColorSpace)) As PDColorSpace  'throws IOException
            Return createColorSpace(colorSpace, colorSpaces, Nothing)
        End Function

        '/**
        ' * This will create the correct color space given the name.
        ' *
        ' * @param colorSpace The color space object.
        ' * @param colorSpaces The ColorSpace dictionary from the current resources, if any.
        ' * @param patterns The patterns dictionary from the current resources, if any
        ' * @return The color space.
        ' *
        ' * @throws IOException If the color space name is unknown.
        ' */
        Public Shared Function createColorSpace(ByVal colorSpace As COSBase, ByVal colorSpaces As Map(Of String, PDColorSpace), ByVal patterns As Map(Of String, PDPatternResources)) As PDColorSpace 'throws IOException
            Dim retval As PDColorSpace = Nothing
            If (TypeOf (colorSpace) Is COSObject) Then
                retval = createColorSpace(DirectCast(colorSpace, COSObject).getObject(), colorSpaces)
            ElseIf (TypeOf (colorSpace) Is COSName) Then
                retval = createColorSpace(DirectCast(colorSpace, COSName).getName(), colorSpaces)
            ElseIf (TypeOf (colorSpace) Is COSArray) Then
                Dim array As COSArray = colorSpace
                Dim name As String = DirectCast(array.getObject(0), COSName).getName()
                If (name.equals(PDCalGray.NAME)) Then
                    retval = New PDCalGray(array)
                ElseIf (name.equals(PDDeviceRGB.NAME)) Then
                    retval = PDDeviceRGB.INSTANCE
                ElseIf (name.equals(PDDeviceGray.NAME)) Then
                    retval = New PDDeviceGray()
                ElseIf (name.equals(PDDeviceCMYK.NAME)) Then
                    retval = PDDeviceCMYK.INSTANCE
                ElseIf (name.equals(PDCalRGB.NAME)) Then
                    retval = New PDCalRGB(array)
                ElseIf (name.equals(PDDeviceN.NAME)) Then
                    retval = New PDDeviceN(array)
                ElseIf (name.Equals(PDIndexed.NAME) OrElse name.Equals(PDIndexed.ABBREVIATED_NAME)) Then
                    retval = New PDIndexed(array)
                ElseIf (name.equals(PDLab.NAME)) Then
                    retval = New PDLab(array)
                ElseIf (name.equals(PDSeparation.NAME)) Then
                    retval = New PDSeparation(array)
                ElseIf (name.equals(PDICCBased.NAME)) Then
                    retval = New PDICCBased(array)
                ElseIf (name.equals(PDPattern.NAME)) Then
                    retval = New PDPattern(array)
                Else
                    Throw New IOException("Unknown colorspace array type:" & name)
                End If
            Else
                Throw New IOException("Unknown colorspace type:" & colorSpace.ToString)
            End If
            Return retval
        End Function

        '/**
        ' * This will create the correct color space given the name.
        ' *
        ' * @param colorSpaceName The name of the colorspace.
        ' *
        ' * @return The color space.
        ' *
        ' * @throws IOException If the color space name is unknown.
        ' */
        Public Shared Function createColorSpace(ByVal colorSpaceName As String) As PDColorSpace 'throws IOException
            Return createColorSpace(colorSpaceName, Nothing)
        End Function

        '/**
        ' * This will create the correct color space given the name.
        ' *
        ' * @param colorSpaceName The name of the colorspace.
        ' * @param colorSpaces The ColorSpace dictionary from the current resources, if any.
        ' *
        ' * @return The color space.
        ' *
        ' * @throws IOException If the color space name is unknown.
        ' */
        Public Shared Function createColorSpace(ByVal colorSpaceName As String, ByVal colorSpaces As Map(Of String, PDColorSpace)) As PDColorSpace 'throws IOException
            Dim cs As PDColorSpace = Nothing
            If (colorSpaceName.Equals(PDDeviceCMYK.NAME) OrElse colorSpaceName.Equals(PDDeviceCMYK.ABBREVIATED_NAME)) Then
                cs = PDDeviceCMYK.INSTANCE
            ElseIf (colorSpaceName.Equals(PDDeviceRGB.NAME) OrElse colorSpaceName.Equals(PDDeviceRGB.ABBREVIATED_NAME)) Then
                cs = PDDeviceRGB.INSTANCE
            ElseIf (colorSpaceName.Equals(PDDeviceGray.NAME) OrElse colorSpaceName.Equals(PDDeviceGray.ABBREVIATED_NAME)) Then
                cs = New PDDeviceGray()
            ElseIf (colorSpaces IsNot Nothing AndAlso colorSpaces.get(colorSpaceName) IsNot Nothing) Then
                cs = colorSpaces.get(colorSpaceName)
            ElseIf (colorSpaceName.equals(PDLab.NAME)) Then
                cs = New PDLab()
            ElseIf (colorSpaceName.Equals(PDPattern.NAME)) Then
                cs = New PDPattern()
            Else
                Throw New IOException("Error: Unknown colorspace '" & colorSpaceName & "'")
            End If
            Return cs
        End Function

        '/**
        ' * This will create the correct color space from a java colorspace.
        ' *
        ' * @param doc The doc to potentiall write information to.
        ' * @param cs The awt colorspace.
        ' *
        ' * @return The color space.
        ' *
        ' * @throws IOException If the color space name is unknown.
        ' */
        Public Shared Function createColorSpace(ByVal doc As PDDocument, ByVal cs As ColorSpace) As PDColorSpace 'throws IOException
            Dim retval As PDColorSpace = Nothing
            If (cs.isCS_sRGB()) Then
                retval = PDDeviceRGB.INSTANCE
            ElseIf (TypeOf (cs) Is ICC_ColorSpace) Then
                Dim ics As ICC_ColorSpace = cs
                Dim pdCS As PDICCBased = New PDICCBased(doc)
                retval = pdCS
                Dim ranges As COSArray = New COSArray()
                For i As Integer = 0 To cs.getNumComponents() - 1
                    ranges.add(New COSFloat(ics.getMinValue(i)))
                    ranges.add(New COSFloat(ics.getMaxValue(i)))
                Next
                Dim iccData As PDStream = pdCS.getPDStream()
                Dim output As OutputStream = Nothing
                Try
                    output = iccData.createOutputStream()
                    output.Write(ics.getProfile().getData())
                Finally
                    If (output IsNot Nothing) Then
                        output.Close()
                    End If
                End Try
                pdCS.setNumberOfComponents(cs.getNumComponents())
            Else
                Throw New IOException("Not yet implemented:" & cs.ToString)
            End If
            Return retval
        End Function

    End Class

End Namespace