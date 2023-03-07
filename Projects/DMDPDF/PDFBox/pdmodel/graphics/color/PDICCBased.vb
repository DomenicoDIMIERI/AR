Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * This class represents a ICC profile color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PDICCBased
        Inherits PDColorSpace


        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDICCBased.class);

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "ICCBased"

        ''' <summary>
        ''' private COSArray array;
        ''' </summary>
        ''' <remarks></remarks>
        Private stream As PDStream

        ''' <summary>
        ''' Number of color components.
        ''' </summary>
        ''' <remarks></remarks>
        Private numberOfComponents As Integer = -1

        '/**
        ' * Default constructor, creates empty stream.
        ' *
        ' * @param doc The document to store the icc data.
        ' */
        Public Sub New(ByVal doc As PDDocument)
            array = New COSArray()
            array.add(COSName.ICCBASED)
            array.add(New PDStream(doc))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param iccArray The ICC stream object.
        ' */
        Public Sub New(ByVal iccArray As COSArray)
            array = iccArray
            stream = New PDStream(iccArray.getObject(1))
        End Sub

        '/**
        ' * This will return the name of the color space.
        ' *
        ' * @return The name of the color space.
        ' */
        Public Overrides Function getName() As String
            Return NAME
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return array
        End Function

        '/**
        ' * Get the pd stream for this icc color space.
        ' *
        ' * @return Get the stream for this icc based color space.
        ' */
        Public Function getPDStream() As PDStream
            Return stream
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color space.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace ' throws IOException
            Dim profile As InputStream = Nothing
            Dim cSpace As ColorSpace = Nothing
            Try
                profile = stream.createInputStream()
                Dim iccProfile As ICC_Profile = ICC_Profile.getInstance(profile)
                cSpace = New ICC_ColorSpace(iccProfile)
                Dim components As Single() = System.Array.CreateInstance(GetType(Single), numberOfComponents)
                ' there maybe a ProfileDataException or a CMMException as there
                ' are some issues when loading ICC_Profiles, see PDFBOX-1295
                ' Try to create a color as test ...
                Dim tmp As New JColor(cSpace, components, 1.0F)
            Catch e As RuntimeException
                ' we are using an alternate colorspace as fallback
                LOG.debug("Can't read ICC-profile, using alternate colorspace instead")
                Dim alternateCSList As List = getAlternateColorSpaces()
                Dim alternate As PDColorSpace = alternateCSList.get(0)
                cSpace = alternate.getJavaColorSpace()
            Finally
                If (profile IsNot Nothing) Then
                    profile.Close()
                End If
            End Try
            Return cSpace
        End Function

        '/**
        ' * Create a Java color model for this colorspace.
        ' *
        ' * @param bpc The number of bits per component.
        ' *
        ' * @return A color model that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color model.
        ' */
        Public Overrides Function createColorModel(ByVal bpc As Integer) As ColorModel ' throws IOException
            Dim nbBits() As Integer
            Dim numOfComponents As Integer = getNumberOfComponents()
            Select Case (numOfComponents)
                Case 1
                    ' DeviceGray
                    nbBits = {bpc}
                Case 3
                    ' DeviceRGB
                    nbBits = {bpc, bpc, bpc}
                Case 4
                    ' DeviceCMYK
                    nbBits = {bpc, bpc, bpc, bpc}
                Case Else
                    Throw New IOException("Unknown colorspace number of components:" & numOfComponents)
            End Select
            Dim componentColorModel As ComponentColorModel = New ComponentColorModel(getJavaColorSpace(), nbBits, False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
            Return componentColorModel
        End Function

        '/**
        ' * This will return the number of color components.  As of PDF 1.4 this will
        ' * be 1,3,4.
        ' *
        ' * @return The number of components in this color space.
        ' *
        ' * @throws IOException If there is an error getting the number of color components.
        ' */
        Public Overrides Function getNumberOfComponents() As Integer ' throws IOException
            If (numberOfComponents < 0) Then
                numberOfComponents = stream.getStream().getInt(COSName.N)
            End If
            Return numberOfComponents
        End Function

        '/**
        ' * This will set the number of color components.
        ' *
        ' * @param n The number of color components.
        ' */
        Public Sub setNumberOfComponents(ByVal n As Integer)
            numberOfComponents = n
            stream.getStream().setInt(COSName.N, n)
        End Sub

        '/**
        ' * This will return a list of alternate color spaces(PDColorSpace) if the display application
        ' * does not support this icc stream.
        ' *
        ' * @return A list of alternate color spaces.
        ' *
        ' * @throws IOException If there is an error getting the alternate color spaces.
        ' */
        Public Function getAlternateColorSpaces() As List  ' throws IOException
            Dim alternate As COSBase = stream.getStream().getDictionaryObject(COSName.ALTERNATE)
            Dim alternateArray As COSArray = Nothing
            If (alternate Is Nothing) Then
                alternateArray = New COSArray()
                Dim numComponents As Integer = getNumberOfComponents()
                Dim csName As COSName = Nothing
                If (numComponents = 1) Then
                    csName = COSName.DEVICEGRAY
                ElseIf (numComponents = 3) Then
                    csName = COSName.DEVICERGB
                ElseIf (numComponents = 4) Then
                    csName = COSName.DEVICECMYK
                Else
                    Throw New IOException("Unknown colorspace number of components:" & numComponents)
                End If
                alternateArray.add(csName)
            Else
                If (TypeOf (alternate) Is COSArray) Then
                    alternateArray = alternate
                ElseIf (TypeOf (alternate) Is COSName) Then
                    alternateArray = New COSArray()
                    alternateArray.add(alternate)
                Else
                    Throw New IOException("Error: expected COSArray or COSName and not " & alternate.GetType.Name)
                End If
            End If
            Dim retval As List = New ArrayList()
            For i As Integer = 0 To alternateArray.size() - 1
                retval.add(PDColorSpaceFactory.createColorSpace(alternateArray.get(i)))
            Next
            Return New COSArrayList(retval, alternateArray)
        End Function

        '/**
        ' * This will set the list of alternate color spaces.  This should be a list
        ' * of PDColorSpace objects.
        ' *
        ' * @param list The list of colorspace objects.
        ' */
        Public Sub setAlternateColorSpaces(ByVal list As List)
            Dim altArray As COSArray = Nothing
            If (list IsNot Nothing) Then
                altArray = COSArrayList.converterToCOSArray(list)
            End If
            stream.getStream().setItem(COSName.ALTERNATE, altArray)
        End Sub

        Private Function getRangeArray(ByVal n As Integer) As COSArray
            Dim rangeArray As COSArray = stream.getStream().getDictionaryObject(COSName.RANGE)
            If (rangeArray Is Nothing) Then
                rangeArray = New COSArray()
                stream.getStream().setItem(COSName.RANGE, rangeArray)
                While (rangeArray.size() < n * 2)
                    rangeArray.add(New COSFloat(-100))
                    rangeArray.add(New COSFloat(100))
                End While
            End If
            Return rangeArray
        End Function

        '/**
        ' * This will get the range for a certain component number.  This is will never
        ' * return null.  If it is not present then the range -100 to 100 will
        ' * be returned.
        ' *
        ' * @param n The component number to get the range for.
        ' *
        ' * @return The range for this component.
        ' */
        Public Function getRangeForComponent(ByVal n As Integer) As PDRange
            Dim rangeArray As COSArray = getRangeArray(n)
            Return New PDRange(rangeArray, n)
        End Function

        '/**
        ' * This will set the a range for this color space.
        ' *
        ' * @param range The new range for the a component.
        ' * @param n The component to set the range for.
        ' */
        Public Sub setRangeForComponent(ByVal range As PDRange, ByVal n As Integer)
            Dim rangeArray As COSArray = getRangeArray(n)
            rangeArray.set(n * 2, New COSFloat(range.getMin()))
            rangeArray.set(n * 2 + 1, New COSFloat(range.getMax()))
        End Sub

        '/**
        ' * This will get the metadata stream for this object.  Null if there is no
        ' * metadata stream.
        ' *
        ' * @return The metadata stream, if it exists.
        ' */
        Public Function getMetadata() As COSStream
            Return stream.getStream().getDictionaryObject(COSName.METADATA)
        End Function

        '/**
        ' * This will set the metadata stream that is associated with this color space.
        ' *
        ' * @param metadata The new metadata stream.
        ' */
        Public Sub setMetadata(ByVal metadata As COSStream)
            stream.getStream().setItem(COSName.METADATA, metadata)
        End Sub

        '// Need more info on the ICCBased ones ... Array contains very little.
        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function toString() As String
            Dim retVal As String = MyBase.toString() + vbLf & vbTab & " Number of Components: "
            Try
                retVal = retVal & getNumberOfComponents()
            Catch exception As IOException
                retVal = retVal & exception.toString()
            End Try
            Return retVal
        End Function

    End Class

End Namespace