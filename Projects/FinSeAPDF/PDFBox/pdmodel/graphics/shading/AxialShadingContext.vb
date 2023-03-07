Imports FinSeA.Drawings
Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics.shading

    '/**
    ' * This class represents the PaintContext of an axial shading.
    ' * 
    ' * @author lehmi
    ' * 
    ' */
    Public Class AxialShadingContext
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
        Private d1d0 As Single
        Private denom As Double

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(AxialShadingContext.class);

        '/**
        ' * Constructor creates an instance to be used for fill operations.
        ' * 
        ' * @param shadingType2 the shading type to be used
        ' * @param colorModelValue the color model to be used
        ' * @param xform transformation for user to device space
        ' * @param ctm current transformation matrix
        ' * @param pageHeight height of the current page
        ' * 
        ' */
        Public Sub New(ByVal shadingType2 As PDShadingType2, ByVal colorModelValue As ColorModel, ByVal xform As AffineTransform, ByVal ctm As Matrix, ByVal pageHeight As Integer)
            coords = shadingType2.getCoords().toFloatArray()
            If (ctm IsNot Nothing) Then
                ' the shading is used in combination with the sh-operator
                Dim coordsTemp As Single() = Array.CreateInstance(GetType(Single), coords.Length)
                ' transform the coords from shading to user space
                ctm.createAffineTransform().transform(coords, 0, coordsTemp, 0, 2)
                ' move the 0,0-reference
                coordsTemp(1) = pageHeight - coordsTemp(1)
                coordsTemp(2) = pageHeight - coordsTemp(2)
                ' transform the coords from user to device space
                xform.transform(coordsTemp, 0, coords, 0, 2)
            Else
                ' the shading is used as pattern colorspace in combination
                ' with a fill-, stroke- or showText-operator
                Dim translateY As Single = xform.getTranslateY()
                ' move the 0,0-reference including the y-translation from user to device space
                coords(1) = pageHeight + translateY - coords(1)
                coords(2) = pageHeight + translateY - coords(2)
            End If
            ' colorSpace 
            Try
                Dim cs As PDColorSpace = shadingType2.getColorSpace()
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
                    colorModel = shadingType2.getColorSpace().createColorModel(8)
                Catch exception As IOException
                    LOG.error("error while creating colorModel", exception)
                End Try
            End If
            ' shading function
            Try
                [function] = shadingType2.getFunction()
            Catch exception As IOException
                LOG.error("error while creating a function", exception)
            End Try
            ' domain values
            If (shadingType2.getDomain() IsNot Nothing) Then
                domain = shadingType2.getDomain().toFloatArray()
            Else
                ' set default values
                domain = New Single() {0, 1}
            End If
            ' extend values
            Dim extendValues As COSArray = shadingType2.getExtend()
            If (shadingType2.getExtend() IsNot Nothing) Then
                extend = Array.CreateInstance(GetType(Boolean), 2)
                extend(0) = DirectCast(extendValues.get(0), COSBoolean).getValue()
                extend(1) = DirectCast(extendValues.get(1), COSBoolean).getValue()
            Else
                ' set default values
                extend = {False, False}
            End If
            ' calculate some constants to be used in getRaster
            x1x0 = coords(2) - coords(0)
            y1y0 = coords(2) - coords(1)
            d1d0 = domain(1) - domain(0)
            denom = Math.Pow(x1x0, 2) + Math.Pow(y1y0, 2)
            ' TODO take a possible Background value into account

        End Sub

        Public Sub dispose() Implements IDisposable.Dispose
            colorModel = Nothing
            [function] = Nothing
            shadingColorSpace = Nothing
            shadingTinttransform = Nothing
            ' MyBase.dispose()
        End Sub

        Public Function getColorModel() As ColorModel
            Return colorModel
        End Function

        Public Function getRaster(ByVal x As Integer, ByVal y As Integer, ByVal w As Integer, ByVal h As Integer) As Raster Implements PaintContext.getRaster
            ' create writable raster
            Dim raster As WritableRaster = getColorModel().createCompatibleWritableRaster(w, h)
            Dim input As Single() = Array.CreateInstance(GetType(Single), 1)
            Dim data() As Integer = Array.CreateInstance(GetType(Integer), w * h * 3)
            Dim saveExtend0 As Boolean = False
            Dim saveExtend1 As Boolean = False
            For j As Integer = 0 To h - 1
                For i As Integer = 0 To w - 1
                    Dim index As Integer = (j * w + i) * 3
                    Dim inputValue As Double = x1x0 * (x + i - coords(0))
                    inputValue += y1y0 * (y + j - coords(1))
                    inputValue /= denom
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
                    ElseIf (inputValue > domain(1)) Then ' input value is out of range
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

    End Class


End Namespace
