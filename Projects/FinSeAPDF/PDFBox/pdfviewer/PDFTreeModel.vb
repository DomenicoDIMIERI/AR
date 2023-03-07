Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel

Namespace org.apache.pdfbox.pdfviewer

    '/**
    ' * A tree model that uses a cos document.
    ' *
    ' *
    ' * @author  wurtz
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.9 $
    ' */

    '/**
    ' * A class to model a PDF document as a tree structure.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.9 $
    ' */
    Public Class PDFTreeModel
        'Implements TreeModel

        Private document As PDDocument

        '/**
        ' * constructor.
        ' */
        Public Sub New()
            'default constructor
        End Sub

        '/**
        ' * Constructor to take a document.
        ' *
        ' * @param doc The document to display in the tree.
        ' */
        Public Sub New(ByVal doc As PDDocument)
            setDocument(doc)
        End Sub

        '/**
        ' * Set the document to display in the tree.
        ' *
        ' * @param doc The document to display in the tree.
        ' */
        Public Sub setDocument(ByVal doc As PDDocument)
            document = doc
        End Sub

        '/**
        ' * Adds a listener for the <code>TreeModelEvent</code>
        ' * posted after the tree changes.
        ' *
        ' * @param   l       the listener to add
        ' * @see     #removeTreeModelListener
        ' *
        ' */
        Public Sub addTreeModelListener(ByVal l As Object) 'TreeModelListener
            'required for interface
        End Sub

        '/**
        ' * Returns the child of <code>parent</code> at index <code>index</code>
        ' * in the parent's
        ' * child array.  <code>parent</code> must be a node previously obtained
        ' * from this data source. This should not return <code>null</code>
        ' * if <code>index</code>
        ' * is a valid index for <code>parent</code> (that is <code>index >= 0 &&
        ' * index < getChildCount(parent</code>)).
        ' *
        ' * @param   parent  a node in the tree, obtained from this data source
        ' * @param index The index into the parent object to location the child object.
        ' * @return  the child of <code>parent</code> at index <code>index</code>
        ' *
        ' */
        Public Function getChild(ByVal parent As Object, ByVal index As Integer) As Object
            Dim retval As Object = Nothing
            If (TypeOf (parent) Is COSArray) Then
                Dim entry As ArrayEntry = New ArrayEntry()
                entry.setIndex(index)
                entry.setValue(DirectCast(parent, COSArray).getObject(index))
                retval = entry
            ElseIf (TypeOf (parent) Is COSDictionary) Then
                Dim dict As COSDictionary = parent
                Dim keys As List(Of COSName) = New ArrayList(Of COSName)(dict.keySet())
                Collections.sort(Of COSName)(keys)
                Dim key As Object = keys.get(index)
                Dim value As Object = dict.getDictionaryObject(DirectCast(key, COSName))
                Dim entry As MapEntry = New MapEntry()
                entry.setKey(key)
                entry.setValue(value)
                retval = entry
            ElseIf (TypeOf (parent) Is MapEntry) Then
                retval = getChild(DirectCast(parent, MapEntry).getValue(), index)
            ElseIf (TypeOf (parent) Is ArrayEntry) Then
                retval = getChild(DirectCast(parent, ArrayEntry).getValue(), index)
            ElseIf (TypeOf (parent) Is COSDocument) Then
                retval = DirectCast(parent, COSDocument).getObjects().get(index)
            ElseIf (TypeOf (parent) Is COSObject) Then
                retval = DirectCast(parent, COSObject).getObject()
            Else
                Throw New RuntimeException("Unknown COS type " & parent.GetType().Name)
            End If
            Return retval
        End Function

        '/** Returns the number of children of <code>parent</code>.
        ' * Returns 0 if the node
        ' * is a leaf or if it has no children.  <code>parent</code> must be a node
        ' * previously obtained from this data source.
        ' *
        ' * @param   parent  a node in the tree, obtained from this data source
        ' * @return  the number of children of the node <code>parent</code>
        ' *
        ' */
        Public Function getChildCount(ByVal parent As Object) As Integer
            Dim retval As Integer = 0
            If (TypeOf (parent) Is COSArray) Then
                retval = DirectCast(parent, COSArray).size()
            ElseIf (TypeOf (parent) Is COSDictionary) Then
                retval = DirectCast(parent, COSDictionary).size()
            ElseIf (TypeOf (parent) Is MapEntry) Then
                retval = getChildCount(DirectCast(parent, MapEntry).getValue())
            ElseIf (TypeOf (parent) Is ArrayEntry) Then
                retval = getChildCount(DirectCast(parent, ArrayEntry).getValue())
            ElseIf (TypeOf (parent) Is COSDocument) Then
                retval = DirectCast(parent, COSDocument).getObjects().size()
            ElseIf (TypeOf (parent) Is COSObject) Then
                retval = 1
            End If
            Return retval
        End Function

        '/** Returns the index of child in parent.  If <code>parent</code>
        ' * is <code>null</code> or <code>child</code> is <code>null</code>,
        ' * returns -1.
        ' *
        ' * @param parent a note in the tree, obtained from this data source
        ' * @param child the node we are interested in
        ' * @return the index of the child in the parent, or -1 if either
        ' *    <code>child</code> or <code>parent</code> are <code>null</code>
        ' *
        ' */
        Public Function getIndexOfChild(ByVal parent As Object, ByVal child As Object) As Integer
            Dim retval As Integer = -1
            If (parent IsNot Nothing AndAlso child IsNot Nothing) Then
                If (TypeOf (parent) Is COSArray) Then
                    Dim array As COSArray = parent
                    If (TypeOf (child) Is ArrayEntry) Then
                        Dim arrayEntry As ArrayEntry = child
                        retval = arrayEntry.getIndex()
                    Else
                        retval = array.indexOf(DirectCast(child, COSBase))
                    End If
                ElseIf (TypeOf (parent) Is COSDictionary) Then
                    Dim entry As MapEntry = child
                    Dim dict As COSDictionary = parent
                    Dim keys As List(Of COSName) = New ArrayList(Of COSName)(dict.keySet())
                    Collections.sort(Of COSName)(keys)
                    For i As Integer = 0 To keys.size() - 1
                        If (retval <> -1) Then Exit For
                        If (keys.get(i).equals(entry.getKey())) Then
                            retval = i
                        End If
                    Next
                ElseIf (TypeOf (parent) Is MapEntry) Then
                    retval = getIndexOfChild(DirectCast(parent, MapEntry).getValue(), child)
                ElseIf (TypeOf (parent) Is ArrayEntry) Then
                    retval = getIndexOfChild(DirectCast(parent, ArrayEntry).getValue(), child)
                ElseIf (TypeOf (parent) Is COSDocument) Then
                    retval = DirectCast(parent, COSDocument).getObjects().indexOf(child)
                ElseIf (TypeOf (parent) Is COSObject) Then
                    retval = 0
                Else
                    Throw New RuntimeException("Unknown COS type " & parent.GetType().Name)
                End If
            End If
            Return retval
        End Function

        '/** Returns the root of the tree.  Returns <code>null</code>
        ' * only if the tree has no nodes.
        ' *
        ' * @return  the root of the tree
        ' *
        ' */
        Public Function getRoot() As Object
            Return document.getDocument().getTrailer()
        End Function

        '/** Returns <code>true</code> if <code>node</code> is a leaf.
        ' * It is possible for this method to return <code>false</code>
        ' * even if <code>node</code> has no children.
        ' * A directory in a filesystem, for example,
        ' * may contain no files; the node representing
        ' * the directory is not a leaf, but it also has no children.
        ' *
        ' * @param   node  a node in the tree, obtained from this data source
        ' * @return  true if <code>node</code> is a leaf
        ' *
        ' */
        Public Function isLeaf(ByVal node As Object) As Boolean
            Return Not (TypeOf (node) Is COSDictionary OrElse TypeOf (node) Is COSArray OrElse TypeOf (node) Is COSDocument OrElse TypeOf (node) Is COSObject OrElse (TypeOf (node) Is MapEntry AndAlso Not isLeaf(DirectCast(node, MapEntry).getValue())) OrElse (TypeOf (node) Is ArrayEntry AndAlso Not isLeaf(DirectCast(node, ArrayEntry).getValue())))
        End Function

        '/** Removes a listener previously added with
        ' * <code>addTreeModelListener</code>.
        ' *
        ' * @see     #addTreeModelListener
        ' * @param   l       the listener to remove
        ' *
        ' */

        Public Sub removeTreeModelListener(ByVal l As Object) ' TreeModelListener)
            'required for interface
        End Sub

        '/** Messaged when the user has altered the value for the item identified
        ' * by <code>path</code> to <code>newValue</code>.
        ' * If <code>newValue</code> signifies a truly new value
        ' * the model should post a <code>treeNodesChanged</code> event.
        ' *
        ' * @param path path to the node that the user has altered
        ' * @param newValue the new value from the TreeCellEditor
        ' *
        ' */
        Public Sub valueForPathChanged(ByVal path As Object, ByVal newValue As Object) 'TreePath
            'required for interface
        End Sub


    End Class

End Namespace
