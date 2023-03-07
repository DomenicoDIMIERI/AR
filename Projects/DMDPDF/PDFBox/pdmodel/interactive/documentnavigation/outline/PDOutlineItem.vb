Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.outline


    '/**
    ' * This represents an outline in a pdf document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.7 $
    ' */
    Public Class PDOutlineItem
        Inherits PDOutlineNode

        Private Const ITALIC_FLAG = 1
        Private Const BOLD_FLAG = 2

        '/**
        ' * Default Constructor.
        ' */
        Public Sub New()
            MyBase.New()
        End Sub

        '/**
        ' * Constructor for an existing outline item.
        ' *
        ' * @param dic The storage dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            MyBase.New(dic)
        End Sub

        '/**
        ' * Insert a sibling after this node.
        ' *
        ' * @param item The item to insert.
        ' */
        Public Sub insertSiblingAfter(ByVal item As PDOutlineItem)
            item.setParent(getParent())
            Dim [next] As PDOutlineItem = getNextSibling()
            setNextSibling(item)
            item.setPreviousSibling(Me)
            If ([next] IsNot Nothing) Then
                item.setNextSibling([next])
                [next].setPreviousSibling(item)
            End If
            updateParentOpenCount(1)
        End Sub


        'Public Function getParent() As PDOutlineNode
        '    Return MyBase.getParent()
        'End Function

        '/**
        ' * Return the previous sibling or null if there is no sibling.
        ' *
        ' * @return The previous sibling.
        ' */
        Public Function getPreviousSibling() As PDOutlineItem
            Dim last As PDOutlineItem = Nothing
            Dim lastDic As COSDictionary = node.getDictionaryObject(COSName.PREV)
            If (lastDic IsNot Nothing) Then
                last = New PDOutlineItem(lastDic)
            End If
            Return last
        End Function

        '/**
        ' * Set the previous sibling, this will be maintained by this class.
        ' *
        ' * @param outlineNode The new previous sibling.
        ' */
        Protected Sub setPreviousSibling(ByVal outlineNode As PDOutlineNode)
            node.setItem(COSName.PREV, outlineNode)
        End Sub

        '/**
        ' * Return the next sibling or null if there is no next sibling.
        ' *
        ' * @return The next sibling.
        ' */
        Public Function getNextSibling() As PDOutlineItem
            Dim last As PDOutlineItem = Nothing
            Dim lastDic As COSDictionary = node.getDictionaryObject(COSName.NEXT)
            If (lastDic IsNot Nothing) Then
                last = New PDOutlineItem(lastDic)
            End If
            Return last
        End Function

        '/**
        ' * Set the next sibling, this will be maintained by this class.
        ' *
        ' * @param outlineNode The new next sibling.
        ' */
        Protected Sub setNextSibling(ByVal outlineNode As PDOutlineNode)
            node.setItem(COSName.NEXT, outlineNode)
        End Sub

        '/**
        ' * Get the title of this node.
        ' *
        ' * @return The title of this node.
        ' */
        Public Function getTitle() As String
            Return node.getString(COSName.TITLE)
        End Function

        '/**
        ' * Set the title for this node.
        ' *
        ' * @param title The new title for this node.
        ' */
        Public Sub setTitle(ByVal title As String)
            node.setString(COSName.TITLE, title)
        End Sub

        '/**
        ' * Get the page destination of this node.
        ' *
        ' * @return The page destination of this node.
        ' * @throws IOException If there is an error creating the destination.
        ' */
        Public Function getDestination() As PDDestination ' throws IOException
            Return PDDestination.create(node.getDictionaryObject(COSName.DEST))
        End Function

        '/**
        ' * Set the page destination for this node.
        ' *
        ' * @param dest The new page destination for this node.
        ' */
        Public Sub setDestination(ByVal dest As PDDestination)
            node.setItem(COSName.DEST, dest)
        End Sub

        '/**
        ' * A convenience method that will create an XYZ destination using only the defaults.
        ' *
        ' * @param page The page to refer to.
        ' */
        Public Sub setDestination(ByVal page As PDPage)
            Dim dest As PDPageXYZDestination = Nothing
            If (page IsNot Nothing) Then
                dest = New PDPageXYZDestination()
                dest.setPage(page)
            End If
            setDestination(dest)
        End Sub

        '/**
        ' * This method will attempt to find the page in this PDF document that this outline points to.
        ' * If the outline does not point to anything then this method will return null.  If the outline
        ' * is an action that is not a GoTo action then this methods will throw the OutlineNotLocationException
        ' *
        ' * @param doc The document to get the page from.
        ' *
        ' * @return The page that this outline will go to when activated or null if it does not point to anything.
        ' * @throws IOException If there is an error when trying to find the page.
        ' */
        Public Function findDestinationPage(ByVal doc As PDDocument) As PDPage ' throws IOException
            Dim page As PDPage = Nothing
            Dim rawDest As PDDestination = getDestination()
            If (rawDest Is Nothing) Then
                Dim outlineAction As PDAction = getAction()
                If (TypeOf (outlineAction) Is PDActionGoTo) Then
                    rawDest = DirectCast(outlineAction, PDActionGoTo).getDestination()
                ElseIf (outlineAction Is Nothing) Then
                    'if the outline action is null then this outline does not refer
                    'to anything and we will just return null.
                Else
                    Throw New OutlineNotLocalException("Error: Outline does not reference a local page.")
                End If
            End If

            Dim pageDest As PDPageDestination = Nothing
            If (TypeOf (rawDest) Is PDNamedDestination) Then
                'if we have a named destination we need to lookup the PDPageDestination
                Dim namedDest As PDNamedDestination = rawDest
                Dim namesDict As PDDocumentNameDictionary = doc.getDocumentCatalog().getNames()
                If (namesDict IsNot Nothing) Then
                    Dim destsTree As PDDestinationNameTreeNode = namesDict.getDests()
                    If (destsTree IsNot Nothing) Then
                        pageDest = destsTree.getValue(namedDest.getNamedDestination())
                    End If
                End If
            ElseIf (TypeOf (rawDest) Is PDPageDestination) Then
                pageDest = rawDest
            ElseIf (rawDest Is Nothing) Then
                'if the destination is null then we will simply return a null page.
            Else
                Throw New IOException("Error: Unknown destination type " & rawDest.ToString)
            End If

            If (pageDest IsNot Nothing) Then
                page = pageDest.getPage()
                If (page Is Nothing) Then
                    Dim pageNumber As Integer = pageDest.getPageNumber()
                    If (pageNumber <> -1) Then
                        Dim allPages As List = doc.getDocumentCatalog().getAllPages()
                        page = allPages.get(pageNumber)
                    End If
                End If
            End If

            Return page
        End Function

        '/**
        ' * Get the action of this node.
        ' *
        ' * @return The action of this node.
        ' */
        Public Function getAction() As PDAction
            Return PDActionFactory.createAction(node.getDictionaryObject(COSName.A))
        End Function

        '/**
        ' * Set the action for this node.
        ' *
        ' * @param action The new action for this node.
        ' */
        Public Sub setAction(ByVal action As PDAction)
            node.setItem(COSName.A, action)
        End Sub

        '/**
        ' * Get the structure element of this node.
        ' *
        ' * @return The structure element of this node.
        ' */
        Public Function getStructureElement() As PDStructureElement
            Dim se As PDStructureElement = Nothing
            Dim dic As COSDictionary = node.getDictionaryObject(COSName.SE)
            If (dic IsNot Nothing) Then
                se = New PDStructureElement(dic)
            End If
            Return se
        End Function

        '/**
        ' * Set the structure element for this node.
        ' *
        ' * @param structureElement The new structure element for this node.
        ' */
        Public Sub setStructuredElement(ByVal structureElement As PDStructureElement)
            node.setItem(COSName.SE, structureElement)
        End Sub

        '/**
        ' * Get the text color of this node.  Default is black and this method
        ' * will never return null.
        ' *
        ' * @return The structure element of this node.
        ' */
        Public Function getTextColor() As PDColorState
            Dim retval As PDColorState = Nothing
            Dim csValues As COSArray = node.getDictionaryObject(COSName.C)
            If (csValues Is Nothing) Then
                csValues = New COSArray()
                csValues.growToSize(3, New COSFloat(0))
                node.setItem(COSName.C, csValues)
            End If
            retval = New PDColorState(csValues)
            retval.setColorSpace(PDDeviceRGB.INSTANCE)
            Return retval
        End Function

        '/**
        ' * Set the text color for this node.  The colorspace must be a PDDeviceRGB.
        ' *
        ' * @param textColor The text color for this node.
        ' */
        Public Sub setTextColor(ByVal textColor As PDColorState)
            node.setItem(COSName.C, textColor.getCOSColorSpaceValue())
        End Sub

        '/**
        ' * Set the text color for this node.  The colorspace must be a PDDeviceRGB.
        ' *
        ' * @param textColor The text color for this node.
        ' */
        Public Sub setTextColor(ByVal textColor As System.Drawing.Color)
            Dim array As COSArray = New COSArray()
            array.add(New COSFloat(CSng(textColor.R / 255)))
            array.add(New COSFloat(CSng(textColor.G / 255.0F)))
            array.add(New COSFloat(CSng(textColor.B / 255.0F)))
            node.setItem(COSName.C, array)
        End Sub

        '/**
        ' * A flag telling if the text should be italic.
        ' *
        ' * @return The italic flag.
        ' */
        Public Function isItalic() As Boolean
            Return BitFlagHelper.getFlag(node, COSName.F, ITALIC_FLAG)
        End Function

        '/**
        ' * Set the italic property of the text.
        ' *
        ' * @param italic The new italic flag.
        ' */
        Public Sub setItalic(ByVal italic As Boolean)
            BitFlagHelper.setFlag(node, COSName.F, ITALIC_FLAG, italic)
        End Sub

        '/**
        ' * A flag telling if the text should be bold.
        ' *
        ' * @return The bold flag.
        ' */
        Public Function isBold() As Boolean
            Return BitFlagHelper.getFlag(node, COSName.F, BOLD_FLAG)
        End Function

        '/**
        ' * Set the bold property of the text.
        ' *
        ' * @param bold The new bold flag.
        ' */
        Public Sub setBold(ByVal bold As Boolean)
            BitFlagHelper.setFlag(node, COSName.F, BOLD_FLAG, bold)
        End Sub

    End Class

End Namespace
