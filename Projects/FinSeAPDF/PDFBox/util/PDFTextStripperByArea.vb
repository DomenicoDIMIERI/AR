Imports System.Drawing
Imports System.IO
Imports FinSeA.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.util

    '/**
    ' * This will extract text from a specified region in the PDF.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDFTextStripperByArea
        Inherits PDFTextStripper

        Private regions As ArrayList(Of String) = New ArrayList(Of String)()
        Private regionArea As Map(Of String, RectangleF) = New HashMap(Of String, RectangleF)()
        Private regionCharacterList As Map(Of String, Vector(Of ArrayList(Of TextPosition))) = New HashMap(Of String, Vector(Of ArrayList(Of TextPosition)))()
        Private regionText As Map(Of String, FinSeA.Io.StringWriter) = New HashMap(Of String, FinSeA.Io.StringWriter)()

        '/**
        ' * Constructor.
        ' * @throws IOException If there is an error loading properties.
        ' */
        Public Sub New() 'throws IOException
            MyBase.New()
            setPageSeparator("")
        End Sub


        '/**
        ' * Instantiate a new PDFTextStripperArea object. Loading all of the operator
        ' * mappings from the properties object that is passed in. Does not convert
        ' * the text to more encoding-specific output.
        ' * 
        ' * @param props
        ' *            The properties containing the mapping of operators to
        ' *            PDFOperator classes.
        ' * 
        ' * @throws IOException
        ' *             If there is an error reading the properties.
        ' */
        Public Sub New(ByVal props As Properties)  'throws IOException
            MyBase.New(props)
            setPageSeparator("")
        End Sub

        '/**
        ' * Instantiate a new PDFTextStripperArea object. This object will load
        ' * properties from PDFTextStripper.properties and will apply
        ' * encoding-specific conversions to the output text.
        ' * 
        ' * @param encoding
        ' *            The encoding that the output will be written in.
        ' * @throws IOException
        ' *             If there is an error reading the properties.
        ' */
        Public Sub New(ByVal encoding As String)  'throws IOException
            MyBase.New(encoding)
            setPageSeparator("")
        End Sub

        '/**
        '  * Add a new region to group text by.
        '  *
        '  * @param regionName The name of the region.
        '  * @param rect The rectangle area to retrieve the text from.
        '  */
        Public Sub addRegion(ByVal regionName As String, ByVal rect As RectangleF)
            regions.add(regionName)
            regionArea.put(regionName, rect)
        End Sub

        '/**
        ' * Get the list of regions that have been setup.
        ' *
        ' * @return A list of java.lang.String objects to identify the region names.
        ' */
        Public Function getRegions() As List(Of String)
            Return regions
        End Function

        '/**
        ' * Get the text for the region, this should be called after extractRegions().
        ' *
        ' * @param regionName The name of the region to get the text from.
        ' * @return The text that was identified in that region.
        ' */
        Public Function getTextForRegion(ByVal regionName As String) As String
            Dim text As FinSeA.Io.StringWriter = regionText.get(regionName)
            Return text.ToString()
        End Function

        '/**
        ' * Process the page to extract the region text.
        ' *
        ' * @param page The page to extract the regions from.
        ' * @throws IOException If there is an error while extracting text.
        ' */
        Public Sub extractRegions(ByVal page As PDPage)  'throws IOException
            Dim regionIter As Global.System.Collections.Generic.IEnumerator(Of String) = regions.iterator()
            While (regionIter.MoveNext())
                setStartPage(getCurrentPageNo())
                setEndPage(getCurrentPageNo())
                'reset the stored text for the region so this class
                'can be reused.
                Dim regionName As String = regionIter.Current
                Dim regionCharactersByArticle As Vector(Of ArrayList(Of TextPosition)) = New Vector(Of ArrayList(Of TextPosition))()
                regionCharactersByArticle.add(New ArrayList(Of TextPosition)())
                regionCharacterList.put(regionName, regionCharactersByArticle)
                regionText.put(regionName, New FinSeA.Io.StringWriter())
            End While

            Dim contentStream As PDStream = page.getContents()
            If (contentStream IsNot Nothing) Then
                Dim contents As COSStream = contentStream.getStream()
                processPage(page, contents)
            End If
        End Sub


    
        Protected Overrides Sub processTextPosition(ByVal text As TextPosition)
            Dim regionIter As Global.System.Collections.Generic.IEnumerator(Of String) = regionArea.keySet().iterator()
            While (regionIter.MoveNext())
                Dim region As String = regionIter.Current
                Dim rect As RectangleF = regionArea.get(region)
                If (rect.contains(text.getX(), text.getY())) Then
                    charactersByArticle = regionCharacterList.get(region)
                    MyBase.processTextPosition(text)
                End If
            End While
        End Sub


        '/**
        ' * This will print the processed page text to the output stream.
        ' *
        ' * @throws IOException If there is an error writing the text.
        ' */
        Protected Overrides Sub writePage() ' throws IOException
            Dim regionIter As Global.System.Collections.Generic.IEnumerator(Of String) = regionArea.keySet().iterator()
            While (regionIter.MoveNext)
                Dim region As String = regionIter.Current
                charactersByArticle = regionCharacterList.get(region)
                output = regionText.get(region)
                MyBase.writePage()
            End While
        End Sub


    End Class

End Namespace
