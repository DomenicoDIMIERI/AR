Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.pattern
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.shading
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.pdmodel.markedcontent
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel

    '   /**
    '* This represents a set of resources available at the page/pages/stream level.
    '* 
    '* @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    '* 
    '*/
    Public Class PDResources
        Implements COSObjectable

        Private resources As COSDictionary
        Private fonts As Map(Of String, PDFont) = Nothing
        Private fontMappings As Map(Of PDFont, String) = New HashMap(Of PDFont, String)()
        Private colorspaces As Map(Of String, PDColorSpace) = Nothing
        Private xobjects As Map(Of String, PDXObject) = Nothing
        Private xobjectMappings As Map(Of PDXObject, String) = Nothing
        Private images As HashMap(Of String, PDXObjectImage) = Nothing
        Private graphicsStates As Map(Of String, PDExtendedGraphicsState) = Nothing
        Private patterns As Map(Of String, PDPatternResources) = Nothing
        Private shadings As Map(Of String, PDShadingResources) = Nothing

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDResources.class);

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            resources = New COSDictionary()
        End Sub

        '/**
        ' * Prepopulated resources.
        ' * 
        ' * @param resourceDictionary The cos dictionary for this resource.
        ' */
        Public Sub New(ByVal resourceDictionary As COSDictionary)
            resources = resourceDictionary
        End Sub

        '/**
        ' * This will get the underlying dictionary.
        ' * 
        ' * @return The dictionary for these resources.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return resources
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' * 
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return resources
        End Function

        '/**
        ' * Calling this will release all cached information.
        ' * 
        ' */
        Public Sub clear()
            If (fonts IsNot Nothing) Then
                fonts.clear()
                fonts = Nothing
            End If
            If (fontMappings IsNot Nothing) Then
                fontMappings.clear()
                fontMappings = Nothing
            End If
            If (colorspaces IsNot Nothing) Then
                colorspaces.clear()
                colorspaces = Nothing
            End If
            If (xobjects IsNot Nothing) Then
                xobjects.clear()
                xobjects = Nothing
            End If
            If (xobjectMappings IsNot Nothing) Then
                xobjectMappings.clear()
                xobjectMappings = Nothing
            End If
            If (images IsNot Nothing) Then
                images.clear()
                images = Nothing
            End If
            If (graphicsStates IsNot Nothing) Then
                graphicsStates.clear()
                graphicsStates = Nothing
            End If
            If (patterns IsNot Nothing) Then
                patterns.clear()
                patterns = Nothing
            End If
            If (shadings IsNot Nothing) Then
                shadings.clear()
                shadings = Nothing
            End If
        End Sub

        '/**
        ' * This will get the map of fonts. This will never return Nothing. The keys are string and the values are PDFont
        ' * objects.
        ' * 
        ' * @param fontCache A map of existing PDFont objects to reuse.
        ' * @return The map of fonts.
        ' * 
        ' * @throws IOException If there is an error getting the fonts.
        ' * 
        ' * @deprecated due to some side effects font caching is no longer supported, use {@link #getFonts()} instead
        ' */
        Public Function getFonts(ByVal fontCache As Map(Of String, PDFont)) As Map(Of String, PDFont) 'throws IOException
            Return getFonts()
        End Function

        '/**
        ' * This will get the map of fonts. This will never return Nothing.
        ' * 
        ' * @return The map of fonts.
        ' */
        Public Function getFonts() As Map(Of String, PDFont)
            If (fonts Is Nothing) Then
                ' at least an empty map will be returned
                ' TODO we should return Nothing instead of an empty map
                fonts = New HashMap(Of String, PDFont)()
                Dim fontsDictionary As COSDictionary = resources.getDictionaryObject(COSName.FONT)
                If (fontsDictionary Is Nothing) Then
                    fontsDictionary = New COSDictionary()
                    resources.setItem(COSName.FONT, fontsDictionary)
                Else
                    For Each fontName As COSName In fontsDictionary.keySet()
                        Dim font As COSBase = fontsDictionary.getDictionaryObject(fontName)
                        ' data-000174.pdf contains a font that is a COSArray, looks to be an error in the
                        ' PDF, we will just ignore entries that are not dictionaries.
                        If (TypeOf (font) Is COSDictionary) Then
                            Dim newFont As PDFont = Nothing
                            Try
                                newFont = PDFontFactory.createFont(font)
                            Catch exception As IOException
                                LOG.error("error while creating a font", exception)
                            End Try
                            If (newFont IsNot Nothing) Then
                                fonts.put(fontName.getName(), newFont)
                            End If
                        End If
                    Next
                End If
                setFonts(fonts)
            End If
            Return fonts
        End Function

        '/**
        ' * This will get the map of PDXObjects that are in the resource dictionary. This will never return Nothing.
        ' * 
        ' * @return The map of xobjects.
        ' */
        Public Function getXObjects() As Map(Of String, PDXObject)
            If (xobjects Is Nothing) Then
                ' at least an empty map will be returned
                ' TODO we should return Nothing instead of an empty map
                xobjects = New HashMap(Of String, PDXObject)()
                Dim xobjectsDictionary As COSDictionary = resources.getDictionaryObject(COSName.XOBJECT)
                If (xobjectsDictionary Is Nothing) Then
                    xobjectsDictionary = New COSDictionary()
                    resources.setItem(COSName.XOBJECT, xobjectsDictionary)
                Else
                    xobjects = New HashMap(Of String, PDXObject)()
                    For Each objName As COSName In xobjectsDictionary.keySet()
                        Dim xobject As PDXObject = Nothing
                        Try
                            xobject = PDXObject.createXObject(xobjectsDictionary.getDictionaryObject(objName))
                        Catch exception As IOException
                            LOG.error("error while creating a xobject", exception)
                        End Try
                        If (xobject IsNot Nothing) Then
                            xobjects.put(objName.getName(), xobject)
                        End If
                    Next
                End If
                setXObjects(xobjects)
            End If
            Return xobjects
        End Function

        '/**
        ' * This will get the map of images. An empty map will be returned if there are no underlying images. So far the keys
        ' * are COSName of the image and the value is the corresponding PDXObjectImage.
        ' * 
        ' * @author By BM
        ' * @return The map of images.
        ' * @throws IOException If there is an error writing the picture.
        ' * 
        ' * @deprecated use {@link #getXObjects()} instead, as the images map isn't synchronized with the XObjects map.
        ' */
        Public Function getImages() As Map(Of String, PDXObjectImage) ' throws IOException
            If (images Is Nothing) Then
                Dim allXObjects As Map(Of String, PDXObject) = getXObjects()
                images = New HashMap(Of String, PDXObjectImage)()
                For Each entry As Map.Entry(Of String, PDXObject) In allXObjects.entrySet()
                    Dim xobject As PDXObject = entry.Value
                    If (TypeOf (xobject) Is PDXObjectImage) Then
                        images.put(entry.Key, xobject)
                    End If
                Next
            End If
            Return images
        End Function

        '/**
        ' * This will set the map of fonts.
        ' * 
        ' * @param fontsValue The new map of fonts.
        ' */
        Public Sub setFonts(ByVal fontsValue As Map(Of String, PDFont))
            fonts = fontsValue
            If (fontsValue IsNot Nothing) Then
                resources.setItem(COSName.FONT, COSDictionaryMap.convert(fontsValue))
                fontMappings = reverseMap(fontsValue, GetType(PDFont))
            Else
                resources.removeItem(COSName.FONT)
                fontMappings = Nothing
            End If
        End Sub

        '/**
        ' * This will set the map of xobjects.
        ' * 
        ' * @param xobjectsValue The new map of xobjects.
        ' */
        Public Sub setXObjects(ByVal xobjectsValue As Map(Of String, PDXObject))
            xobjects = xobjectsValue
            If (xobjectsValue IsNot Nothing) Then
                resources.setItem(COSName.XOBJECT, COSDictionaryMap.convert(xobjectsValue))
                xobjectMappings = reverseMap(xobjects, GetType(PDXObject))
            Else
                resources.removeItem(COSName.XOBJECT)
                xobjectMappings = Nothing
            End If
        End Sub

        '/**
        ' * This will get the map of colorspaces. This will return Nothing if the underlying resources dictionary does not have
        ' * a colorspace dictionary. The keys are string and the values are PDColorSpace objects.
        ' * 
        ' * @return The map of colorspaces.
        ' */
        Public Function getColorSpaces() As Map(Of String, PDColorSpace)
            If (colorspaces Is Nothing) Then
                Dim csDictionary As COSDictionary = resources.getDictionaryObject(COSName.COLORSPACE)
                If (csDictionary IsNot Nothing) Then
                    colorspaces = New HashMap(Of String, PDColorSpace)()
                    For Each csName As COSName In csDictionary.keySet()
                        Dim cs As COSBase = csDictionary.getDictionaryObject(csName)
                        Dim colorspace As PDColorSpace = Nothing
                        Try
                            colorspace = PDColorSpaceFactory.createColorSpace(cs)
                        Catch exception As IOException
                            LOG.error("error while creating a colorspace", exception)
                        End Try
                        If (colorspace IsNot Nothing) Then
                            colorspaces.put(csName.getName(), colorspace)
                        End If
                    Next
                End If
            End If
            Return colorspaces
        End Function

        '/**
        ' * This will set the map of colorspaces.
        ' * 
        ' * @param csValue The new map of colorspaces.
        ' */
        Public Sub setColorSpaces(ByVal csValue As Map(Of String, PDColorSpace))
            colorspaces = csValue
            If (csValue IsNot Nothing) Then
                resources.setItem(COSName.COLORSPACE, COSDictionaryMap.convert(csValue))
            Else
                resources.removeItem(COSName.COLORSPACE)
            End If
        End Sub

        '/**
        ' * This will get the map of graphic states. This will return Nothing if the underlying resources dictionary does not
        ' * have a graphics dictionary. The keys are the graphic state name as a String and the values are
        ' * PDExtendedGraphicsState objects.
        ' * 
        ' * @return The map of extended graphic state objects.
        ' */
        Public Function getGraphicsStates() As Map(Of String, PDExtendedGraphicsState)
            If (graphicsStates Is Nothing) Then
                Dim states As COSDictionary = resources.getDictionaryObject(COSName.EXT_G_STATE)
                If (states IsNot Nothing) Then
                    graphicsStates = New HashMap(Of String, PDExtendedGraphicsState)()
                    For Each name As COSName In states.keySet()
                        Dim dictionary As COSDictionary = states.getDictionaryObject(name)
                        graphicsStates.put(name.getName(), New PDExtendedGraphicsState(dictionary))
                    Next
                End If
            End If
            Return graphicsStates
        End Function

        '/**
        ' * This will set the map of graphics states.
        ' * 
        ' * @param states The new map of states.
        ' */
        Public Sub setGraphicsStates(ByVal states As Map(Of String, PDExtendedGraphicsState))
            graphicsStates = states
            If (states IsNot Nothing) Then
                'Iterator(Of String) iter = states.keySet().iterator();
                Dim dic As COSDictionary = New COSDictionary()
                For Each name As String In states.keySet '   While (iter.hasNext())
                    '{
                    'String name = (String) iter.next();
                    Dim state As PDExtendedGraphicsState = states.get(name)
                    dic.setItem(COSName.getPDFName(name), state.getCOSObject())
                Next '}
                resources.setItem(COSName.EXT_G_STATE, dic)
            Else
                resources.removeItem(COSName.EXT_G_STATE)
            End If
        End Sub

        '/**
        ' * Returns the dictionary mapping resource names to property list dictionaries for marked content.
        ' * 
        ' * @return the property list
        ' */
        Public Function getProperties() As PDPropertyList
            Dim retval As PDPropertyList = Nothing
            Dim props As COSDictionary = resources.getDictionaryObject(COSName.PROPERTIES)

            If (props IsNot Nothing) Then
                retval = New PDPropertyList(props)
            End If
            Return retval
        End Function

        '/**
        ' * Sets the dictionary mapping resource names to property list dictionaries for marked content.
        ' * 
        ' * @param props the property list
        ' */
        Public Sub setProperties(ByVal props As PDPropertyList)
            resources.setItem(COSName.PROPERTIES, props.getCOSObject())
        End Sub

        '/**
        ' * This will get the map of patterns. This will return Nothing if the underlying resources dictionary does not have a
        ' * patterns dictionary. The keys are the pattern name as a String and the values are PDPatternResources objects.
        ' * 
        ' * @return The map of pattern resources objects.
        ' * 
        ' * @throws IOException If there is an error getting the pattern resources.
        ' */
        Public Function getPatterns() As Map(Of String, PDPatternResources) 'throws IOException
            If (patterns Is Nothing) Then
                Dim patternsDictionary As COSDictionary = resources.getDictionaryObject(COSName.PATTERN)
                If (patternsDictionary IsNot Nothing) Then
                    patterns = New HashMap(Of String, PDPatternResources)()
                    For Each name As COSName In patternsDictionary.keySet()
                        Dim dictionary As COSDictionary = patternsDictionary.getDictionaryObject(name)
                        patterns.put(name.getName(), PDPatternResources.create(dictionary))
                    Next
                End If
            End If
            Return patterns
        End Function

        '/**
        ' * This will set the map of patterns.
        ' * 
        ' * @param patternsValue The new map of patterns.
        ' */
        Public Sub setPatterns(ByVal patternsValue As Map(Of String, PDPatternResources))
            patterns = patternsValue
            If (patternsValue IsNot Nothing) Then
                'Iterator(Of String) iter = patternsValue.keySet().iterator();
                Dim dic As COSDictionary = New COSDictionary()
                For Each name As String In patternsValue.keySet() ' While (iter.hasNext())
                    '{
                    'String name = iter.next();
                    Dim pattern As PDPatternResources = patternsValue.get(name)
                    dic.setItem(COSName.getPDFName(name), pattern.getCOSObject())
                Next
                resources.setItem(COSName.PATTERN, dic)
            Else
                resources.removeItem(COSName.PATTERN)
            End If
        End Sub

        '/**
        ' * This will get the map of shadings. This will return Nothing if the underlying resources dictionary does not have a
        ' * shading dictionary. The keys are the shading name as a String and the values are PDShadingResources objects.
        ' * 
        ' * @return The map of shading resources objects.
        ' * 
        ' * @throws IOException If there is an error getting the shading resources.
        ' */
        Public Function getShadings() As Map(Of String, PDShadingResources) ' throws IOException
            If (shadings Is Nothing) Then
                Dim shadingsDictionary As COSDictionary = resources.getDictionaryObject(COSName.SHADING)
                If (shadingsDictionary IsNot Nothing) Then
                    shadings = New HashMap(Of String, PDShadingResources)()
                    For Each name As COSName In shadingsDictionary.keySet()
                        Dim dictionary As COSDictionary = shadingsDictionary.getDictionaryObject(name)
                        shadings.put(name.getName(), PDShadingResources.create(dictionary))
                    Next
                End If
            End If
            Return shadings
        End Function

        '/**
        ' * This will set the map of shadings.
        ' * 
        ' * @param shadingsValue The new map of shadings.
        ' */
        Public Sub setShadings(ByVal shadingsValue As Map(Of String, PDShadingResources))
            shadings = shadingsValue
            If (shadingsValue IsNot Nothing) Then
                'Iterator(Of String) iter = shadingsValue.keySet().iterator();
                Dim dic As COSDictionary = New COSDictionary()
                For Each name As String In shadingsValue.keySet() 'While (iter.hasNext())
                    '{
                    'String name = iter.next();
                    Dim shading As PDShadingResources = shadingsValue.get(name)
                    dic.setItem(COSName.getPDFName(name), shading.getCOSObject())
                Next
                resources.setItem(COSName.SHADING, dic)
            Else
                resources.removeItem(COSName.SHADING)
            End If
        End Sub

        '/**
        ' * Adds the given font to the resources of the current page.
        ' * 
        ' * @param font the font to be added
        ' * @return the font name to be used within the content stream.
        ' */
        Public Function addFont(ByVal font As PDFont) As String
            ' use the getter to initialize a possible empty fonts map
            Return addFont(font, MapUtil.getNextUniqueKey(getFonts(), "F"))
        End Function

        '/**
        ' * Adds the given font to the resources of the current page using the given font key.
        ' * 
        ' * @param font the font to be added
        ' * @param fontKey key to used to map to the given font
        ' * @return the font name to be used within the content stream.
        ' */
        Public Function addFont(ByVal font As PDFont, ByVal fontKey As String) As String
            If (fonts Is Nothing) Then
                ' initialize fonts map
                getFonts()
            End If

            Dim fontMapping As String = fontMappings.get(font)
            If (fontMapping Is Nothing) Then
                fontMapping = fontKey
                fontMappings.put(font, fontMapping)
                fonts.put(fontMapping, font)
                addFontToDictionary(font, fontMapping)
            End If
            Return fontMapping
        End Function

        Private Sub addFontToDictionary(ByVal font As PDFont, ByVal fontName As String)
            Dim fontsDictionary As COSDictionary = resources.getDictionaryObject(COSName.FONT)
            fontsDictionary.setItem(fontName, font)
        End Sub

        '/**
        ' * Adds the given XObject to the resources of the current the page.
        ' * 
        ' * @param xobject the XObject to be added
        ' * @param prefix the prefix to be used for the name
        ' * 
        ' * @return the XObject name to be used within the content stream.
        ' */
        Public Function addXObject(ByVal xobject As PDXObject, ByVal prefix As String) As String
            If (xobjects Is Nothing) Then
                ' initialize XObject map
                getXObjects()
            End If
            Dim objMapping As String = xobjectMappings.get(xobject)
            If (objMapping Is Nothing) Then
                objMapping = MapUtil.getNextUniqueKey(xobjects, prefix)
                xobjectMappings.put(xobject, objMapping)
                xobjects.put(objMapping, xobject)
                addXObjectToDictionary(xobject, objMapping)
            End If
            Return objMapping
        End Function

        Private Sub addXObjectToDictionary(ByVal xobject As PDXObject, ByVal xobjectName As String)
            Dim xobjectsDictionary As COSDictionary = resources.getDictionaryObject(COSName.XOBJECT)
            xobjectsDictionary.setItem(xobjectName, xobject)
        End Sub

        Private Function reverseMap(Of T)(ByVal map As Map(Of String, T), ByVal keyClass As System.Type) As Map(Of T, String)
            Dim reversed As Map(Of T, String) = New HashMap(Of T, String)
            For Each entry As Map.Entry(Of String, T) In map.entrySet()
                reversed.put(entry.Value, entry.Key)
            Next
            Return reversed
        End Function

    End Class

End Namespace
