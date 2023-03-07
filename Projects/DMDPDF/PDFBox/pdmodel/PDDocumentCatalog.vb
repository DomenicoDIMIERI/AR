Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.optionalcontent
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.documentnavigation.outline
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.form
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.pagenavigation
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.viewerpreferences

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * This class represents the acroform of a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.21 $
    ' */
    Public Class PDDocumentCatalog
        Implements COSObjectable

        Private root As COSDictionary
        Private document As PDDocument

        Private acroForm As PDAcroForm = Nothing

        ''' <summary>
        ''' Page mode where neither the outline nor the thumbnails are displayed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_MODE_USE_NONE = "UseNone"
        ''' <summary>
        ''' Show bookmarks when pdf is opened.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_MODE_USE_OUTLINES = "UseOutlines"
        ''' <summary>
        ''' Show thumbnails when pdf is opened.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_MODE_USE_THUMBS = "UseThumbs"
        ''' <summary>
        ''' Full screen mode with no menu bar, window controls.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_MODE_FULL_SCREEN = "FullScreen"
        ''' <summary>
        ''' Optional content group panel is visible when opened.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_MODE_USE_OPTIONAL_CONTENT = "UseOC"
        ''' <summary>
        ''' Attachments panel is visible.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_MODE_USE_ATTACHMENTS = "UseAttachments"

        ''' <summary>
        ''' Display one page at a time.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_LAYOUT_SINGLE_PAGE = "SinglePage"

        ''' <summary>
        ''' Display the pages in one column.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_LAYOUT_ONE_COLUMN = "OneColumn"

        ''' <summary>
        ''' Display the pages in two columns, with odd numbered pagse on the left.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_LAYOUT_TWO_COLUMN_LEFT = "TwoColumnLeft"

        ''' <summary>
        ''' Display the pages in two columns, with odd numbered pagse on the right.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_LAYOUT_TWO_COLUMN_RIGHT = "TwoColumnRight"

        ''' <summary>
        ''' Display the pages two at a time, with odd-numbered pages on the left. @since PDF Version 1.5
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_LAYOUT_TWO_PAGE_LEFT = "TwoPageLeft"

        ''' <summary>
        ''' Display the pages two at a time, with odd-numbered pages on the right. @since PDF Version 1.5
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PAGE_LAYOUT_TWO_PAGE_RIGHT = "TwoPageRight"



        '/**
        ' * Constructor.
        ' *
        ' * @param doc The document that this catalog is part of.
        ' */
        Public Sub New(ByVal doc As PDDocument)
            document = doc
            root = New COSDictionary()
            root.setItem(COSName.TYPE, COSName.CATALOG)
            document.getDocument().getTrailer().setItem(COSName.ROOT, root)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param doc The document that this catalog is part of.
        ' * @param rootDictionary The root dictionary that this object wraps.
        ' */
        Public Sub New(ByVal doc As PDDocument, ByVal rootDictionary As COSDictionary)
            document = doc
            root = rootDictionary
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return root
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return root
        End Function

        '/**
        ' * This will get the documents acroform.  This will return null if
        ' * no acroform is part of the document.
        ' *
        ' * @return The documents acroform.
        ' */
        Public Function getAcroForm() As PDAcroForm
            If (acroForm Is Nothing) Then
                Dim acroFormDic As COSDictionary = root.getDictionaryObject(COSName.ACRO_FORM)
                If (acroFormDic IsNot Nothing) Then
                    acroForm = New PDAcroForm(document, acroFormDic)
                End If
            End If
            Return acroForm
        End Function

        '/**
        ' * Set the acro form for this catalog.
        ' *
        ' * @param acro The new acro form.
        ' */
        Public Sub setAcroForm(ByVal acro As PDAcroForm)
            root.setItem(COSName.ACRO_FORM, acro)
        End Sub

        '/**
        ' * This will get the root node for the pages.
        ' *
        ' * @return The parent page node.
        ' */
        Public Function getPages() As PDPageNode
            Return New PDPageNode(root.getDictionaryObject(COSName.PAGES))
        End Function

        '/**
        ' * The PDF document contains a hierarchical structure of PDPageNode and PDPages, which
        ' * is mostly just a way to store this information.  This method will return a flat list
        ' * of all PDPage objects in this document.
        ' *
        ' * @return A list of PDPage objects.
        ' */
        Public Function getAllPages() As List
            Dim retval As List = New ArrayList()
            Dim rootNode As PDPageNode = getPages()
            'old (slower):
            'getPageObjects( rootNode, retval );
            rootNode.getAllKids(retval)
            Return retval
        End Function

        '/**
        ' * Get the viewer preferences associated with this document or null if they
        ' * do not exist.
        ' *
        ' * @return The document's viewer preferences.
        ' */
        Public Function getViewerPreferences() As PDViewerPreferences
            Dim retval As PDViewerPreferences = Nothing
            Dim dict As COSDictionary = root.getDictionaryObject(COSName.VIEWER_PREFERENCES)
            If (dict IsNot Nothing) Then
                retval = New PDViewerPreferences(dict)
            End If

            Return retval
        End Function

        '/**
        ' * Set the viewer preferences.
        ' *
        ' * @param prefs The new viewer preferences.
        ' */
        Public Sub setViewerPreferences(ByVal prefs As PDViewerPreferences)
            root.setItem(COSName.VIEWER_PREFERENCES, prefs)
        End Sub

        '/**
        ' * Get the outline associated with this document or null if it
        ' * does not exist.
        ' *
        ' * @return The document's outline.
        ' */
        Public Function getDocumentOutline() As PDDocumentOutline
            Dim retval As PDDocumentOutline = Nothing
            Dim dict As COSDictionary = root.getDictionaryObject(COSName.OUTLINES)
            If (dict IsNot Nothing) Then
                retval = New PDDocumentOutline(dict)
            End If

            Return retval
        End Function

        '/**
        ' * Set the document outlines.
        ' *
        ' * @param outlines The new document outlines.
        ' */
        Public Sub setDocumentOutline(ByVal outlines As PDDocumentOutline)
            root.setItem(COSName.OUTLINES, outlines)
        End Sub

        '/**
        ' * Get the list threads for this pdf document.
        ' *
        ' * @return A list of PDThread objects.
        ' */
        Public Function getThreads() As List
            Dim array As COSArray = root.getDictionaryObject(COSName.THREADS)
            If (array Is Nothing) Then
                array = New COSArray()
                root.setItem(COSName.THREADS, array)
            End If
            Dim pdObjects As List = New ArrayList()
            For i As Integer = 0 To array.size() - 1
                pdObjects.add(New PDThread(array.getObject(i)))
            Next
            Return New COSArrayList(pdObjects, array)
        End Function

        '/**
        ' * Set the list of threads for this pdf document.
        ' *
        ' * @param threads The list of threads, or null to clear it.
        ' */
        Public Sub setThreads(ByVal threads As List)
            root.setItem(COSName.THREADS, COSArrayList.converterToCOSArray(threads))
        End Sub

        '/**
        ' * Get the metadata that is part of the document catalog.  This will
        ' * return null if there is no meta data for this object.
        ' *
        ' * @return The metadata for this object.
        ' */
        Public Function getMetadata() As PDMetadata
            Dim retval As PDMetadata = Nothing
            Dim stream As COSStream = root.getDictionaryObject(COSName.METADATA)
            If (Stream IsNot Nothing) Then
                retval = New PDMetadata(stream)
            End If
            Return retval
        End Function

        '/**
        ' * Set the metadata for this object.  This can be null.
        ' *
        ' * @param meta The meta data for this object.
        ' */
        Public Sub setMetadata(ByVal meta As PDMetadata)
            root.setItem(COSName.METADATA, meta)
        End Sub

        '/**
        ' * Set the Document Open Action for this object.
        ' *
        ' * @param action The action you want to perform.
        ' */
        Public Sub setOpenAction(ByVal action As PDDestinationOrAction)
            root.setItem(COSName.OPEN_ACTION, action)
        End Sub

        '/**
        ' * Get the Document Open Action for this object.
        ' *
        ' * @return The action to perform when the document is opened.
        ' *
        ' * @throws IOException If there is an error creating the destination
        ' * or action.
        ' */
        Public Function getOpenAction() As PDDestinationOrAction ' throws IOException
            Dim action As PDDestinationOrAction = Nothing
            Dim actionObj As COSBase = root.getDictionaryObject(COSName.OPEN_ACTION)

            If (actionObj Is Nothing) Then
                'no op
            ElseIf (TypeOf (actionObj) Is COSDictionary) Then
                action = PDActionFactory.createAction(actionObj)
            ElseIf (TypeOf (actionObj) Is COSArray) Then
                action = PDDestination.create(actionObj)
            Else
                Throw New IOException("Unknown OpenAction " & actionObj.ToString)
            End If
            Return action
        End Function

        '/**
        ' * @return The Additional Actions for this Document
        ' */
        Public Function getActions() As PDDocumentCatalogAdditionalActions
            Dim addAct As COSDictionary = root.getDictionaryObject(COSName.AA)
            If (addAct Is Nothing) Then
                addAct = New COSDictionary()
                root.setItem(COSName.AA, addAct)
            End If
            Return New PDDocumentCatalogAdditionalActions(addAct)
        End Function

        '/**
        ' * Set the additional actions for the document.
        ' *
        ' * @param actions The actions that are associated with this document.
        ' */
        Public Sub setActions(ByVal actions As PDDocumentCatalogAdditionalActions)
            root.setItem(COSName.AA, actions)
        End Sub

        '/**
        ' * @return The names dictionary for this document or null if none exist.
        ' */
        Public Function getNames() As PDDocumentNameDictionary
            Dim nameDic As PDDocumentNameDictionary = Nothing
            Dim names As COSDictionary = root.getDictionaryObject(COSName.NAMES)
            If (names IsNot Nothing) Then
                nameDic = New PDDocumentNameDictionary(Me, names)
            End If
            Return nameDic
        End Function

        '/**
        ' * Set the names dictionary for the document.
        ' *
        ' * @param names The names dictionary that is associated with this document.
        ' */
        Public Sub setNames(ByVal names As PDDocumentNameDictionary)
            root.setItem(COSName.NAMES, names)
        End Sub

        '/**
        ' * Get info about doc's usage of tagged features.  This will return
        ' * null if there is no information.
        ' *
        ' * @return The new mark info.
        ' */
        Public Function getMarkInfo() As PDMarkInfo
            Dim retval As PDMarkInfo = Nothing
            Dim dic As COSDictionary = root.getDictionaryObject(COSName.MARK_INFO)
            If (dic IsNot Nothing) Then
                retval = New PDMarkInfo(dic)
            End If
            Return retval
        End Function

        '/**
        ' * Set information about the doc's usage of tagged features.
        ' *
        ' * @param markInfo The new MarkInfo data.
        ' */
        Public Sub setMarkInfo(ByVal markInfo As PDMarkInfo)
            root.setItem(COSName.MARK_INFO, markInfo)
        End Sub

        '/**
        ' * Get the list of OutputIntents defined in the document.
        ' * 
        ' * @return The list of PDOoutputIntent
        ' */
        Public Function getOutputIntent() As List(Of PDOutputIntent)
            Dim retval As List(Of PDOutputIntent) = New ArrayList(Of PDOutputIntent)()
            Dim array As COSArray = root.getItem(COSName.OUTPUT_INTENTS)
            If (array IsNot Nothing) Then
                For Each cosBase As COSBase In array
                    Dim oi As PDOutputIntent = New PDOutputIntent(cosBase)
                    retval.add(oi)
                Next
            End If
            Return retval
        End Function

        '/**
        ' * Add an OutputIntent to the list.
        ' * 
        ' * If there is not OutputIntent, the list is created and the first
        ' * element added.
        ' * 
        ' * @param outputIntent the OutputIntent to add.
        ' */
        Public Sub addOutputIntent(ByVal outputIntent As PDOutputIntent)
            Dim array As COSArray = root.getItem(COSName.OUTPUT_INTENTS)
            If (array Is Nothing) Then
                array = New COSArray()
                root.setItem(COSName.OUTPUT_INTENTS, array)
            End If
            array.add(outputIntent.getCOSObject())
        End Sub

        '/**
        ' * Replace the list of OutputIntents of the document.
        ' * 
        ' * @param outputIntents the list of OutputIntents, if the list is empty all
        ' * OutputIntents are removed.
        ' */
        Public Sub setOutputIntents(ByVal outputIntents As List(Of PDOutputIntent))
            Dim array As COSArray = New COSArray()
            For Each intent As PDOutputIntent In outputIntents
                array.add(intent.getCOSObject())
            Next
            root.setItem(COSName.OUTPUT_INTENTS, array)
        End Sub

        '/**
        ' * Set the page display mode, see the PAGE_MODE_XXX constants.
        ' * @return A string representing the page mode.
        ' */
        Public Function getPageMode() As String
            Return root.getNameAsString(COSName.PAGE_MODE, PAGE_MODE_USE_NONE)
        End Function

        '/**
        ' * Set the page mode.  See the PAGE_MODE_XXX constants for valid values.
        ' * @param mode The new page mode.
        ' */
        Public Sub setPageMode(ByVal mode As String)
            root.setName(COSName.PAGE_MODE, mode)
        End Sub

        '/**
        ' * Set the page layout, see the PAGE_LAYOUT_XXX constants.
        ' * @return A string representing the page layout.
        ' */
        Public Function getPageLayout() As String
            Return root.getNameAsString(COSName.PAGE_LAYOUT, PAGE_LAYOUT_SINGLE_PAGE)
        End Function

        '/**
        ' * Set the page layout.  See the PAGE_LAYOUT_XXX constants for valid values.
        ' * @param layout The new page layout.
        ' */
        Public Sub setPageLayout(ByVal layout As String)
            root.setName(COSName.PAGE_LAYOUT, layout)
        End Sub

        '/**
        ' * Document level information in the URI.
        ' * @return Document level URI.
        ' */
        Public Function getURI() As PDURIDictionary
            Dim retval As PDURIDictionary = Nothing
            Dim uri As COSDictionary = root.getDictionaryObject(COSName.URI)
            If (Uri IsNot Nothing) Then
                retval = New PDURIDictionary(uri)
            End If
            Return retval
        End Function

        '/**
        ' * Set the document level uri.
        ' * @param uri The new document level uri.
        ' */
        Public Sub setURI(ByVal uri As PDURIDictionary)
            root.setItem(COSName.URI, uri)
        End Sub

        '/**
        ' * Get the document's structure tree root.
        ' *
        ' * @return The document's structure tree root or null if none exists.
        ' */
        Public Function getStructureTreeRoot() As PDStructureTreeRoot
            Dim treeRoot As PDStructureTreeRoot = Nothing
            Dim dic As COSDictionary = root.getDictionaryObject(COSName.STRUCT_TREE_ROOT)
            If (dic IsNot Nothing) Then
                treeRoot = New PDStructureTreeRoot(dic)
            End If
            Return treeRoot
        End Function

        '/**
        ' * Set the document's structure tree root.
        ' *
        ' * @param treeRoot The new structure tree.
        ' */
        Public Sub setStructureTreeRoot(ByVal treeRoot As PDStructureTreeRoot)
            root.setItem(COSName.STRUCT_TREE_ROOT, treeRoot)
        End Sub

        '/**
        ' * The language for the document.
        ' *
        ' * @return The language for the document.
        ' */
        Public Function getLanguage() As String
            Return root.getString(COSName.LANG)
        End Function

        '/**
        ' * Set the Language for the document.
        ' *
        ' * @param language The new document language.
        ' */
        Public Sub setLanguage(ByVal language As String)
            root.setString(COSName.LANG, language)
        End Sub

        '/**
        ' * Returns the PDF specification version this document conforms to.
        ' *
        ' * @return The PDF version.
        ' */
        Public Function getVersion() As String
            Return root.getNameAsString(COSName.VERSION)
        End Function

        '/**
        ' * Sets the PDF specification version this document conforms to.
        ' *
        ' * @param version the PDF version (ex. "1.4")
        ' */
        Public Sub setVersion(ByVal version As String)
            root.setName(COSName.VERSION, version)
        End Sub

        '/**
        ' * Returns the page labels descriptor of the document.
        ' *
        ' * @return the page labels descriptor of the document.
        ' *
        ' * @throws IOException If there is a problem retrieving the page labels.
        ' */
        Public Function getPageLabels() As PDPageLabels ' throws IOException
            Dim labels As PDPageLabels = Nothing
            Dim dict As COSDictionary = root.getDictionaryObject(COSName.PAGE_LABELS)
            If (dict IsNot Nothing) Then
                labels = New PDPageLabels(document, dict)
            End If
            Return labels
        End Function

        '/**
        ' * Set the page label descriptor for the document.
        ' *
        ' * @param labels the new page label descriptor to set.
        ' */
        Public Sub setPageLabels(ByVal labels As PDPageLabels)
            root.setItem(COSName.PAGE_LABELS, labels)
        End Sub

        '/**
        ' * Get the optional content properties dictionary associated with this document.
        ' *
        ' * @return the optional properties dictionary or null if it is not present
        ' * @since PDF 1.5
        ' */
        Public Function getOCProperties() As PDOptionalContentProperties
            Dim retval As PDOptionalContentProperties = Nothing
            Dim dict As COSDictionary = root.getDictionaryObject(COSName.OCPROPERTIES)
            If (dict IsNot Nothing) Then
                retval = New PDOptionalContentProperties(dict)
            End If

            Return retval
        End Function

        '/**
        ' * Set the optional content properties dictionary.
        ' *
        ' * @param ocProperties the optional properties dictionary
        ' * @since PDF 1.5
        ' */
        Public Sub setOCProperties(ByVal ocProperties As PDOptionalContentProperties)
            'TODO Check for PDF 1.5 or higher
            root.setItem(COSName.OCPROPERTIES, ocProperties)
        End Sub

    End Class

End Namespace
