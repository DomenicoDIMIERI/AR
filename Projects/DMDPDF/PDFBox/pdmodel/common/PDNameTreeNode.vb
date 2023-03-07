Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * This class represents a PDF Name tree.  See the PDF Reference 1.5 section 3.8.5
    ' * for more details.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDNameTreeNode
        Implements COSObjectable

        '0private shared final Log LOG = LogFactory.getLog(PDNameTreeNode.class);
        Private node As COSDictionary

        Private valueType As System.Type ' Class<? extends COSObjectable> valueType = null;

        Private parent As PDNameTreeNode = Nothing

        '/**
        ' * Constructor.
        ' *
        ' * @param valueClass The PD Model type of object that is the value.
        ' */
        Public Sub New(ByVal valueClass As System.Type) 'Class<? extends COSObjectable> 
            node = New COSDictionary()
            valueType = valueClass
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict The dictionary that holds the name information.
        ' * @param valueClass The PD Model type of object that is the value.
        ' */
        Public Sub New(ByVal dict As COSDictionary, ByVal valueClass As System.Type) 'Class<? extends COSObjectable> 
            node = dict
            valueType = valueClass
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
        ' * Returns the parent node.
        ' * 
        ' * @return parent node
        ' */
        Public Function getParent() As PDNameTreeNode
            Return parent
        End Function

        '/**
        ' * Sets the parent to the given node.
        ' * 
        ' * @param parentNode the node to be set as parent
        ' */
        Public Sub setParent(ByVal parentNode As PDNameTreeNode)
            parent = parentNode
            calculateLimits()
        End Sub

        '/**
        ' * Determines if this is a root node or not.
        ' * 
        ' * @return true if this is a root node
        ' */
        Public Function isRootNode() As Boolean
            Return parent Is Nothing
        End Function


        '/**     * Return the children of this node.  This list will contain PDNameTreeNode objects.
        ' *
        ' * @return The list of children or null if there are no children.
        ' */
        Public Function getKids() As List(Of PDNameTreeNode)
            Dim retval As List(Of PDNameTreeNode) = Nothing
            Dim kids As COSArray = node.getDictionaryObject(COSName.KIDS)
            If (kids IsNot Nothing) Then
                Dim pdObjects As List(Of PDNameTreeNode) = New ArrayList(Of PDNameTreeNode)
                For i As Integer = 0 To kids.size() - 1
                    pdObjects.add(createChildNode(kids.getObject(i)))
                Next
                retval = New COSArrayList(Of PDNameTreeNode)(pdObjects, kids)
            End If

            Return retval
        End Function

        '/**
        ' * Set the children of this named tree.
        ' *
        ' * @param kids The children of this named tree.
        ' */
        Public Sub setKids(Of T As PDNameTreeNode)(ByVal kids As List(Of T))
            If (kids IsNot Nothing AndAlso kids.size() > 0) Then
                For Each kidsNode As PDNameTreeNode In kids
                    kidsNode.setParent(Me)
                    node.setItem(COSName.KIDS, COSArrayList.converterToCOSArray(kids))
                    ' root nodes with kids don't have Names
                    If (isRootNode()) Then
                        node.setItem(COSName.NAMES, DirectCast(Nothing, COSObjectable))
                    End If
                Next
            Else
                ' remove kids
                node.setItem(COSName.KIDS, Nothing)
                ' remove Limits 
                node.setItem(COSName.LIMITS, DirectCast(Nothing, COSObjectable))
            End If
            calculateLimits()
        End Sub

        Private Sub calculateLimits()
            If (isRootNode()) Then
                node.setItem(COSName.LIMITS, DirectCast(Nothing, COSObjectable))
            Else
                Dim kids As List(Of PDNameTreeNode) = getKids()
                If (kids IsNot Nothing AndAlso kids.size() > 0) Then
                    Dim firstKid As PDNameTreeNode = kids.get(0)
                    Dim lastKid As PDNameTreeNode = kids.get(kids.size() - 1)
                    Dim lowerLimit As String = firstKid.getLowerLimit()
                    setLowerLimit(lowerLimit)
                    Dim upperLimit As String = lastKid.getUpperLimit()
                    setUpperLimit(upperLimit)
                Else
                    Try
                        Dim names As Map(Of String, COSObjectable) = getNames()
                        If (names IsNot Nothing AndAlso names.size() > 0) Then
                            Dim keys As Object() = names.keySet().toArray()
                            Dim lowerLimit As String = keys(0)
                            setLowerLimit(lowerLimit)
                            Dim upperLimit As String = keys(keys.Length - 1)
                            setUpperLimit(upperLimit)
                        Else
                            node.setItem(COSName.LIMITS, DirectCast(Nothing, COSObjectable))
                        End If
                    Catch exception As IOException
                        node.setItem(COSName.LIMITS, DirectCast(Nothing, COSObjectable))
                        LOG.error("Error while calculating the Limits of a PageNameTreeNode:", exception)
                    End Try
                End If
            End If
        End Sub

        '/**
        ' * The name to retrieve.
        ' *
        ' * @param name The name in the tree.
        ' *
        ' * @return The value of the name in the tree.
        ' *
        ' * @throws IOException If an there is a problem creating the destinations.
        ' */
        Public Function getValue(ByVal name As String) As Object 'throws IOException
            Dim retval As Object = Nothing
            Dim names As Map(Of String, COSObjectable) = getNames()
            If (names IsNot Nothing) Then
                retval = names.get(name)
            Else
                Dim kids As List(Of PDNameTreeNode) = getKids()
                If (kids IsNot Nothing) Then
                    For i As Integer = 0 To kids.size() - 1
                        If (retval IsNot Nothing) Then Exit For
                        Dim childNode As PDNameTreeNode = kids.get(i)
                        If (childNode.getLowerLimit().compareTo(name) <= 0 AndAlso childNode.getUpperLimit().compareTo(name) >= 0) Then
                            retval = childNode.getValue(name)
                        End If
                    Next
                Else
                    LOG.warn("NameTreeNode does not have ""names"" nor ""kids"" objects.")
                End If
            End If
            Return retval
        End Function


        '/**
        ' * This will return a map of names. The key will be a string, and the
        ' * value will depend on where this class is being used.
        ' *
        ' * @return ordered map of cos objects or <code>null</code> if dictionary
        ' *         contains no 'Names' entry
        ' * @throws IOException If there is an error while creating the sub types.
        ' */
        Public Function getNames() As Map(Of String, COSObjectable) ' throws IOException
            Dim namesArray As COSArray = node.getDictionaryObject(COSName.NAMES)
            If (namesArray IsNot Nothing) Then
                Dim names As Map(Of String, COSObjectable) = New LinkedHashMap(Of String, COSObjectable)
                For i As Integer = 0 To namesArray.size() Step 2
                    Dim key As COSString = namesArray.getObject(i)
                    Dim cosValue As COSBase = namesArray.getObject(i + 1)
                    names.put(key.getString(), convertCOSToPD(cosValue))
                Next
                Return Collections.unmodifiableMap(names)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * Method to convert the COS value in the name tree to the PD Model object. The
        ' * default implementation will simply return the given COSBase object.
        ' * Subclasses should do something specific.
        ' *
        ' * @param base The COS object to convert.
        ' * @return The converted PD Model object.
        ' * @throws IOException If there is an error during creation.
        ' */
        Protected Overridable Function convertCOSToPD(ByVal base As COSBase) As COSObjectable ' throws IOException
            Return base
        End Function

        '/**
        ' * Create a child node object.
        ' *
        ' * @param dic The dictionary for the child node object to refer to.
        ' * @return The new child node object.
        ' */
        Protected Overridable Function createChildNode(ByVal dic As COSDictionary) As PDNameTreeNode
            Return New PDNameTreeNode(dic, valueType)
        End Function

        '/**
        ' * Set the names of for this node.  The keys should be java.lang.String and the
        ' * values must be a COSObjectable.  This method will set the appropriate upper and lower
        ' * limits based on the keys in the map.
        ' *
        ' * @param names map of names to objects, or <code>null</code>
        ' */
        Public Sub setNames(Of T As COSBase)(ByVal names As Map(Of String, T)) 'Map<String, ? extends COSObjectable> 
            If (names Is Nothing) Then
                node.setItem(COSName.NAMES, DirectCast(Nothing, COSObjectable))
                node.setItem(COSName.LIMITS, DirectCast(Nothing, COSObjectable))
            Else
                Dim array As COSArray = New COSArray()
                Dim keys As List(Of String) = New ArrayList(Of String)(names.keySet())
                Collections.sort(Of String)(keys)
                For Each key As String In keys
                    array.add(New COSString(key))
                    array.add(names.get(key))
                Next
                node.setItem(COSName.NAMES, array)
                calculateLimits()
            End If
        End Sub

        '/**
        ' * Get the highest value for a key in the name map.
        ' *
        ' * @return The highest value for a key in the map.
        ' */
        Public Function getUpperLimit() As String
            Dim retval As String = ""
            Dim arr As COSArray = node.getDictionaryObject(COSName.LIMITS)
            If (arr IsNot Nothing) Then
                retval = arr.getString(1)
            End If
            Return retval
        End Function

        '/**
        ' * Set the highest value for the key in the map.
        ' *
        ' * @param upper The new highest value for a key in the map.
        ' */
        Private Sub setUpperLimit(ByVal upper As String)
            Dim arr As COSArray = node.getDictionaryObject(COSName.LIMITS)
            If (arr Is Nothing) Then
                arr = New COSArray()
                arr.add(DirectCast(Nothing, COSObjectable))
                arr.add(DirectCast(Nothing, COSObjectable))
                node.setItem(COSName.LIMITS, arr)
            End If
            arr.setString(1, upper)
        End Sub

        '/**
        ' * Get the lowest value for a key in the name map.
        ' *
        ' * @return The lowest value for a key in the map.
        ' */
        Public Function getLowerLimit() As String
            Dim retval As String = ""
            Dim arr As COSArray = node.getDictionaryObject(COSName.LIMITS)
            If (arr IsNot Nothing) Then
                retval = arr.getString(0)
            End If
            Return retval
        End Function

        '/**
        ' * Set the lowest value for the key in the map.
        ' *
        ' * @param lower The new lowest value for a key in the map.
        ' */
        Private Sub setLowerLimit(ByVal lower As String)
            Dim arr As COSArray = node.getDictionaryObject(COSName.LIMITS)
            If (arr Is Nothing) Then
                arr = New COSArray()
                arr.add(DirectCast(Nothing, COSObjectable))
                arr.add(DirectCast(Nothing, COSObjectable))
                node.setItem(COSName.LIMITS, arr)
            End If
            arr.setString(0, lower)
        End Sub


    End Class

End Namespace
