Imports FinSeA.Drawings
Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics.shading

    '/**
    ' * This class represents the PaintContext of an radial shading.
    ' * 
    ' * @author lehmi
    ' * 
    ' */
    Public Class RadialShadingContext
        Implements PaintContext

        Private colorModel As ColorModel
        Private [function] As PDFunction
        Private shadingColorSpace As ColorSpace
        Private shadingTinttransform As PDFunction

        Private coords As Single()
        Private domain As Single()
        Private extend0Values As Integer()
        Private extend1Values As Integer()
        Private extend As Boolean()
        Private x1x0 As Double
        Private y1y0 As Double
        Private r1r0 As Double
        Private x1x0pow2 As Double
        Private y1y0pow2 As Double
        Private r0pow2 As Double

        Private d1d0 As Single
        Private denom As Double

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(RadialShadingContext.class);

        '/**
        ' * Constructor creates an instance to be used for fill operations.
        ' * 
        ' * @param shadingType3 the shading type to be used
        ' * @param colorModelValue the color model to be used
        ' * @param xform transformation for user to device space
        ' * @param ctm current transformation matrix
        ' * @param pageHeight height of the current page
        ' * 
        ' */
        Public Sub New(ByVal shadingType3 As PDShadingType3, ByVal colorModelValue As ColorModel, ByVal xform As AffineTransform, ByVal ctm As Matrix, ByVal pageHeight As Integer)
            coords = shadingType3.getCoords().toFloatArray()
            If (ctm IsNot Nothing) Then
                ' the shading is used in combination with the sh-operator
                Dim coordsTemp As Single() = Array.CreateInstance(GetType(Single), coords.Length)
                ' transform the coords from shading to user space
                ctm.createAffineTransform().transform(coords, 0, coordsTemp, 0, 1)
                ctm.createAffineTransform().transform(coords, 3, coordsTemp, 3, 1)
                ' move the 0,0-reference
                coordsTemp(1) = pageHeight - coordsTemp(1)
                coordsTemp(4) = pageHeight - coordsTemp(4)
                ' transform the coords from user to device space
                xform.transform(coordsTemp, 0, coords, 0, 1)
                xform.transform(coordsTemp, 3, coords, 3, 1)
            Else
                ' the shading is used as pattern colorspace in combination
                ' with a fill-, stroke- or showText-operator
                Dim translateY As Single = xform.getTranslateY()
                ' move the 0,0-reference including the y-translation from user to device space
                coords(1) = pageHeight + translateY - coords(1)
                coords(4) = pageHeight + translateY - coords(4)
            End If
            ' colorSpace 
            Try
                Dim cs As PDColorSpace = shadingType3.getColorSpace()
                If (Not (TypeOf (cs) Is PDDeviceRGB)) Then
                    ' we have to create an instance of the shading colorspace if it isn't RGB
                    shadingColorSpace = cs.getJavaColorSpace()
                    If (TypeOf (cs) Is PDDeviceN) Then
                        shadingTinttransform = DirectCast(cs, PDDeviceN).getTintTransform()
                    ElseIf (TypeOf (cs) Is PDSeparation) Then
                        shadingTinttransform = DirectCast(cs, PDSeparation).getTintTransform()
                    End If
                End If
            Catch exception As IOException
                LOG.error("error while creating colorSpace", exception)
            End Try
            ' colorModel
            If (colorModelValue IsNot Nothing) Then
                colorModel = colorModelValue
            Else
                Try
                    ' TODO bpc != 8 ??  
                    colorModel = shadingType3.getColorSpace().createColorModel(8)
                Catch exception As IOException
                    LOG.error("error while creating colorModel", exception)
                End Try
            End If
            ' shading function
            Try
                [function] = shadingType3.getFunction()
            Catch exception As IOException
                LOG.error("error while creating a function", exception)
            End Try
            ' domain values
            If (shadingType3.getDomain() IsNot Nothing) Then
                domain = shadingType3.getDomain().toFloatArray()
            Else
                ' set default values
                domain = {0, 1}
            End If
            ' extend values
            Dim extendValues As COSArray = shadingType3.getExtend()
            If (shadingType3.getExtend() IsNot Nothing) Then
                extend = Array.CreateInstance(GetType(Boolean), 2)
                extend(0) = DirectCast(extendValues.get(0), COSBoolean).getValue()
                extend(1) = DirectCast(extendValues.get(1), COSBoolean).getValue()
            Else
                ' set default values
                extend = {False, False}
            End If
            ' calculate some constants to be used in getRaster
            x1x0 = coords(2) - coords(0)
            y1y0 = coords(4) - coords(1)
            r1r0 = coords(5) - coords(2)
            x1x0pow2 = Math.Pow(x1x0, 2)
            y1y0pow2 = Math.Pow(y1y0, 2)
            r0pow2 = Math.Pow(coords(2), 2)
            denom = x1x0pow2 + y1y0pow2 - Math.Pow(r1r0, 2)
            d1d0 = domain(1) - domain(0)
            ' TODO take a possible Background value into account

        End Sub

        Public Sub dispose() Implements IDisposable.Dispose
            colorModel = Nothing
            [function] = Nothing
            shadingColorSpace = Nothing
            shadingTinttransform = Nothing
        End Sub

        Public Function getColorModel() As ColorModel
            Return colorModel
        End Function

        Public Function getRaster(ByVal x As Integer, ByVal y As Integer, ByVal w As Integer, ByVal h As Integer) As Raster Implements PaintContext.getRaster
            ' create writable raster
            Dim raster As WritableRaster = getColorModel().createCompatibleWritableRaster(w, h)
            Dim input As Single() = Array.CreateInstance(GetType(Single), 1)
            Dim inputValue As Single
            Dim saveExtend0 As Boolean = False
            Dim saveExtend1 As Boolean = False
            Dim data As Integer() = Array.CreateInstance(GetType(Integer), w * h * 3)
            For j As Integer = 0 To h - 1
                For i As Integer = 0 To w - 1
                    Dim index As Integer = (j * w + i) * 3
                    Dim inputValues As Single() = calculateInputValues(x + i, y + j)
                    ' choose 1 of the 2 values
                    If (inputValues(0) >= domain(0) AndAlso inputValues(0) <= domain(1)) Then
                        ' both values are in the domain -> choose the larger one 
                        If (inputValues(1) >= domain(0) AndAlso inputValues(1) <= domain(1)) Then
                            inputValue = Math.Max(inputValues(0), inputValues(1))
                        Else ' first value is in the domain, the second not -> choose first value
                            inputValue = inputValues(0)
                        End If
                    Else
                        ' first value is not in the domain, but the second -> choose second value
                        If (inputValues(1) >= domain(0) AndAlso inputValues(1) <= domain(1)) Then
                            inputValue = inputValues(1)
                        Else
                            ' TODO
                            ' both are not in the domain -> choose the first as I don't know it better
                            inputValue = inputValues(0)
                        End If
                    End If
                    ' input value is out of range
                    If (inputValue < domain(0)) Then
                        ' the shading has to be extended if extend(0) == true
                        If (extend(0)) Then
                            If (extend0Values Is Nothing) Then
                                inputValue = domain(0)
                                saveExtend0 = True
                            Else
                                ' use the chached values
                                Array.Copy(extend0Values, 0, data, index, 3)
                                Continue For
                            End If
                        Else
                            Continue For
                        End If
                    ElseIf (inputValue > domain(1)) Then '// input value is out of range
                        ' the shading has to be extended if extend(1) == true
                        If (extend(1)) Then
                            If (extend1Values Is Nothing) Then
                                inputValue = domain(1)
                                saveExtend1 = True
                            Else
                                ' use the chached values 
                                Array.Copy(extend1Values, 0, data, index, 3)
                                Continue For
                            End If
                        Else
                            Continue For
                        End If
                    End If
                    input(0) = (domain(0) + (d1d0 * inputValue))
                    Dim values As Single() = Nothing
                    Try
                        values = [function].eval(input)
                        ' convert color values from shading colorspace to RGB 
                        If (shadingColorSpace IsNot Nothing) Then
                            If (shadingTinttransform IsNot Nothing) Then
                                values = shadingTinttransform.eval(values)
                            End If
                            values = shadingColorSpace.toRGB(values)
                        End If
                    Catch exception As IOException
                        LOG.error("error while processing a function", exception)
                    End Try
                    data(index) = (values(0) * 255)
                    data(index + 1) = (values(1) * 255)
                    data(index + 2) = (values(2) * 255)
                    If (saveExtend0) Then
                        ' chache values
                        extend0Values = Array.CreateInstance(GetType(Integer), 2)
                        Array.Copy(data, index, extend0Values, 0, 3)
                    End If
                    If (saveExtend1) Then
                        ' chache values
                        extend1Values = Array.CreateInstance(GetType(Integer), 2)
                        Array.Copy(data, index, extend1Values, 0, 3)
                    End If
                Next
            Next
            raster.setPixels(0, 0, w, h, data)
            Return raster
        End Function

        Private Function calculateInputValues(ByVal x As Integer, ByVal y As Integer) As Single()

            '/** 
            ' *  According to Adobes Technical Note #5600 we have to do the following 
            ' *  
            ' *  x0, y0, r0 defines the start circle
            ' *  x1, y1, r1 defines the end circle
            ' *  
            ' *  The parametric equations for the center and radius of the gradient fill
            ' *  circle moving between the start circle and the end circle as a function 
            ' *  of s are as follows:
            ' *  
            ' *  xc(s) = x0 + s * (x1 - x0)
            ' *  yc(s) = y0 + s * (y1 - y0)
            ' *  r(s)  = r0 + s * (r1 - r0)
            ' * 
            ' *  Given a geometric coordinate position (x, y) in or along the gradient fill, 
            ' *  the corresponding value of s can be determined by solving the quadratic 
            ' *  constraint equation:
            ' *  
            ' *  [x - xc(s)]2 + [y - yc(s)]2 = [r(s)]2
            ' *  
            ' *  The following code calculates the 2 possible values of s
            ' */

            Dim values(2 - 1) As Single
            Dim p As Double = (-0.25) * ((x - coords(0)) * x1x0 + (y - coords(1)) * y1y0 - r1r0) / denom
            Dim q As Double = (Math.Pow(x - coords(0), 2) + Math.Pow(y - coords(1), 2) - r0pow2) / denom
            Dim root As Double = Math.Sqrt(Math.Pow(p, 2) - q)
            values(0) = ((-1) * p + root)
            values(1) = ((-1) * p - root)
            Return values
        End Function

    End Class

End Namespace