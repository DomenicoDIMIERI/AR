Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.outline

    '/**
    ' * This represents an node in an outline in a pdf document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDOutlineNode
        Implements COSObjectable

        ''' <summary>
        ''' The dictionary for this node.
        ''' </summary>
        ''' <remarks></remarks>
        Protected node As COSDictionary

        '/**
        ' * Default Constructor.
        ' */
        Public Sub New()
            node = New COSDictionary()
        End Sub

        '/**
        ' * Default Constructor.
        ' *
        ' * @param dict The dictionary storage.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            node = dict
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return node
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return node
        End Function

        '/**
        ' * Get the parent of this object.  This will either be a DocumentOutline or an OutlineItem.
        ' *
        ' * @return The parent of this object, or null if this is the document outline and there
        ' * is no parent.
        ' */
        Protected Function getParent() As PDOutlineNode
            Dim retval As PDOutlineNode = Nothing
            Dim parent As COSDictionary = node.getDictionaryObject("Parent", "P")
            If (parent IsNot Nothing) Then
                If (parent.getDictionaryObject("Parent", "P") Is Nothing) Then
                    retval = New PDDocumentOutline(parent)
                Else
                    retval = New PDOutlineItem(parent)
                End If
            End If

            Return retval
        End Function

        '/**
        ' * Set the parent of this object, this is maintained by these objects and should not
        ' * be called by any clients of PDFBox code.
        ' *
        ' * @param parent The parent of this object.
        ' */
        Protected Sub setParent(ByVal parent As PDOutlineNode)
            node.setItem("Parent", parent)
        End Sub

        '/**
        ' * append a child node to this node.
        ' *
        ' * @param outlineNode The node to add.
        ' */
        Public Sub appendChild(ByVal outlineNode As PDOutlineItem)
            outlineNode.setParent(Me)
            If (getFirstChild() Is Nothing) Then
                Dim currentOpenCount As Integer = getOpenCount()
                setFirstChild(outlineNode)
                '1 for the the item we are adding;
                Dim numberOfOpenNodesWeAreAdding As Integer = 1
                If (outlineNode.isNodeOpen()) Then
                    numberOfOpenNodesWeAreAdding += outlineNode.getOpenCount()
                End If
                If (isNodeOpen()) Then
                    setOpenCount(currentOpenCount + numberOfOpenNodesWeAreAdding)
                Else
                    setOpenCount(currentOpenCount - numberOfOpenNodesWeAreAdding)
                End If
                updateParentOpenCount(numberOfOpenNodesWeAreAdding)
            Else
                Dim previousLastChild As PDOutlineItem = getLastChild()
                previousLastChild.insertSiblingAfter(outlineNode)
            End If

            Dim lastNode As PDOutlineItem = outlineNode
            While (lastNode.getNextSibling() IsNot Nothing)
                lastNode = lastNode.getNextSibling()
            End While
            setLastChild(lastNode)
        End Sub

        '/**
        ' * Return the first child or null if there is no child.
        ' *
        ' * @return The first child.
        ' */
        Public Function getFirstChild() As PDOutlineItem
            Dim last As PDOutlineItem = Nothing
            Dim lastDic As COSDictionary = node.getDictionaryObject("First")
            If (lastDic IsNot Nothing) Then
                last = New PDOutlineItem(lastDic)
            End If
            Return last
        End Function

        '/**
        ' * Set the first child, this will be maintained by this class.
        ' *
        ' * @param outlineNode The new first child.
        ' */
        Protected Sub setFirstChild(ByVal outlineNode As PDOutlineNode)
            node.setItem("First", outlineNode)
        End Sub

        '/**
        ' * Return the last child or null if there is no child.
        ' *
        ' * @return The last child.
        ' */
        Public Function getLastChild() As PDOutlineItem
            Dim last As PDOutlineItem = Nothing
            Dim lastDic As COSDictionary = node.getDictionaryObject("Last")
            If (lastDic IsNot Nothing) Then
                last = New PDOutlineItem(lastDic)
            End If
            Return last
        End Function

        '/**
        ' * Set the last child, this will be maintained by this class.
        ' *
        ' * @param outlineNode The new last child.
        ' */
        Protected Sub setLastChild(ByVal outlineNode As PDOutlineNode)
            node.setItem("Last", outlineNode)
        End Sub

        '/**
        ' * Get the number of open nodes.  Or a negative number if this node
        ' * is closed.  See PDF Reference for more details.  This value
        ' * is updated as you append children and siblings.
        ' *
        ' * @return The Count attribute of the outline dictionary.
        ' */
        Public Function getOpenCount() As Integer
            Return node.getInt("Count", 0)
        End Function

        '/**
        ' * Set the open count.  This number is automatically managed for you
        ' * when you add items to the outline.
        ' *
        ' * @param openCount The new open cound.
        ' */
        Protected Sub setOpenCount(ByVal openCount As Integer)
            node.setInt("Count", openCount)
        End Sub

        '/**
        ' * This will set this node to be open when it is shown in the viewer.  By default, when
        ' * a new node is created it will be closed.
        ' * This will do nothing if the node is already open.
        ' */
        Public Sub openNode()
            'if the node is already open then do nothing.
            If (Not isNodeOpen()) Then
                Dim openChildrenCount As Integer = 0
                Dim currentChild As PDOutlineItem = getFirstChild()
                While (currentChild IsNot Nothing)
                    'first increase by one for the current child
                    openChildrenCount += 1
                    'then increase by the number of open nodes the child has
                    If (currentChild.isNodeOpen()) Then
                        openChildrenCount += currentChild.getOpenCount()
                    End If
                    currentChild = currentChild.getNextSibling()
                End While
                setOpenCount(openChildrenCount)
                updateParentOpenCount(openChildrenCount)
            End If
        End Sub

        '/**
        ' * Close this node.
        ' *
        ' */
        Public Sub closeNode()
            'if the node is already closed then do nothing.
            If (isNodeOpen()) Then
                Dim openCount As Integer = getOpenCount()
                updateParentOpenCount(-openCount)
                setOpenCount(-openCount)
            End If
        End Sub

        '/**
        ' * Node is open if the open count is greater than zero.
        ' * @return true if this node is open.
        ' */
        Public Function isNodeOpen() As Boolean
            Return getOpenCount() > 0
        End Function

        '/**
        ' * The count parameter needs to be updated when you add or remove elements to
        ' * the outline.  When you add an element at a lower level then you need to
        ' * increase all of the parents.
        ' *
        ' * @param amount The amount to update by.
        ' */
        Protected Sub updateParentOpenCount(ByVal amount As Integer)
            Dim parent As PDOutlineNode = getParent()
            If (parent IsNot Nothing) Then
                Dim currentCount As Integer = parent.getOpenCount()
                'if the currentCount is negative or it is absent then
                'we will treat it as negative.  The default is to be negative.
                Dim negative As Boolean = currentCount < 0 OrElse parent.getCOSDictionary().getDictionaryObject("Count") Is Nothing
                currentCount = Math.Abs(currentCount)
                currentCount += amount
                If (negative) Then
                    currentCount = -currentCount
                End If
                parent.setOpenCount(currentCount)
                'recursively call parent to update count, but the parents count is only
                'updated if this is an open node
                If (Not negative) Then
                    parent.updateParentOpenCount(amount)
                End If
            End If
        End Sub

    End Class

End Namespace
