Imports System.IO

'imports FinSeA. org.apache.commons.logging.Log;
'import org.apache.commons.logging.LogFactory;
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * This class represents a PDF Number tree. See the PDF Reference 1.7 section
    ' * 7.9.7 for more details.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>,
    ' *         <a href="igor.podolskiy@ievvwi.uni-stuttgart.de">Igor Podolskiy</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDNumberTreeNode
        Implements COSObjectable

        'private shared final Log LOG = LogFactory.getLog( PDNumberTreeNode.class );

        Private node As COSDictionary
        Private valueType As System.Type 'Class<? extends COSObjectable>  = null;

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
        ' * Return the children of this node.  This list will contain PDNumberTreeNode objects.
        ' *
        ' * @return The list of children or null if there are no children.
        ' */
        Public Function getKids() As List(Of PDNumberTreeNode)
            Dim retval As List(Of PDNumberTreeNode) = Nothing
            Dim kids As COSArray = node.getDictionaryObject(COSName.KIDS)
            If (kids IsNot Nothing) Then
                Dim pdObjects As List(Of PDNumberTreeNode) = New ArrayList(Of PDNumberTreeNode)
                For i As Integer = 0 To kids.size() - 1
                    pdObjects.add(createChildNode(kids.getObject(i)))
                Next
                retval = New COSArrayList(Of PDNumberTreeNode)(pdObjects, kids)
            End If

            Return retval
        End Function

        '/**
        ' * Set the children of this number tree.
        ' *
        ' * @param kids The children of this number tree.
        ' */
        Public Sub setKids(Of T As PDNumberTreeNode)(ByVal kids As List(Of T)) '<? extends PDNumberTreeNode>)
            If (kids IsNot Nothing AndAlso kids.size() > 0) Then
                Dim firstKid As PDNumberTreeNode = kids.get(0)
                Dim lastKid As PDNumberTreeNode = kids.get(kids.size() - 1)
                Dim lowerLimit As NInteger = firstKid.getLowerLimit()
                Me.setLowerLimit(lowerLimit)
                Dim upperLimit As NInteger = lastKid.getUpperLimit()
                Me.setUpperLimit(upperLimit)
            ElseIf (node.getDictionaryObject(COSName.NUMS) Is Nothing) Then
                ' Remove limits if there are no kids and no numbers set.
                node.setItem(COSName.LIMITS, Nothing)
            End If
            node.setItem(COSName.KIDS, COSArrayList.converterToCOSArray(kids))
        End Sub

        '/**
        ' * Returns the value corresponding to an index in the number tree.
        ' *
        ' * @param index The index in the number tree.
        ' *
        ' * @return The value corresponding to the index.
        ' *
        ' * @throws IOException If there is a problem creating the values.
        ' */
        Public Function getValue(ByVal index As NInteger) As Object 'throws IOException
            Dim retval As Object = Nothing
            Dim names As Map(Of NInteger, COSObjectable) = getNumbers()
            If (names IsNot Nothing) Then
                retval = names.get(index)
            Else
                Dim kids As List(Of PDNumberTreeNode) = getKids()
                If (kids IsNot Nothing) Then
                    For i As Integer = 0 To kids.size() - 1
                        If (retval IsNot Nothing) Then Exit For
                        Dim childNode As PDNumberTreeNode = kids.get(i)
                        If (childNode.getLowerLimit().compareTo(index) <= 0 AndAlso childNode.getUpperLimit().compareTo(index) >= 0) Then
                            retval = childNode.getValue(index)
                        End If
                    Next

                Else
                    LOG.warn("NumberTreeNode does not have ""nums"" nor ""kids"" objects.")
                End If
            End If
            Return retval
        End Function


        '/**
        ' * This will return a map of numbers.  The key will be a java.lang.Integer, the value will
        ' * depend on where this class is being used.
        ' *
        ' * @return A map of COS objects.
        ' *
        ' * @throws IOException If there is a problem creating the values.
        ' */
        Public Function getNumbers() As Map(Of NInteger, COSObjectable)  '  throws IOException
            Dim indices As Map(Of NInteger, COSObjectable) = Nothing
            Dim namesArray As COSArray = node.getDictionaryObject(COSName.NUMS)
            If (namesArray IsNot Nothing) Then
                indices = New HashMap(Of NInteger, COSObjectable)
                For i As Integer = 0 To namesArray.size() - 1 Step 2 '; i+=2 )
                    Dim key As COSInteger = namesArray.getObject(i)
                    Dim cosValue As COSBase = namesArray.getObject(i + 1)
                    Dim pdValue As COSObjectable = convertCOSToPD(cosValue)
                    indices.put(New NInteger(key.intValue()), pdValue)
                Next
                indices = Collections.unmodifiableMap(indices)
            End If
            Return indices
        End Function

        '/**
        ' * Method to convert the COS value in the name tree to the PD Model object.  The
        ' * default implementation will simply use reflection to create the correct object
        ' * type.  Subclasses can do whatever they want.
        ' *
        ' * @param base The COS object to convert.
        ' * @return The converted PD Model object.
        ' * @throws IOException If there is an error during creation.
        ' */
        Protected Function convertCOSToPD(ByVal base As COSBase) As COSObjectable 'throws IOException
            Dim retval As COSObjectable = Nothing
            Try
                'Constructor<? extends COSObjectable> ctor = valueType.getConstructor( new Class[] { base.getClass() } );
                'retval = ctor.newInstance( new Object[] { base } );
                retval = Activator.CreateInstance(valueType, {base})
            Catch t As Exception
                Throw New IOException("Error while trying to create value in number tree:" & t.Message)

            End Try
            Return retval
        End Function

        '/**
        ' * Create a child node object.
        ' *
        ' * @param dic The dictionary for the child node object to refer to.
        ' * @return The new child node object.
        ' */
        Protected Function createChildNode(ByVal dic As COSDictionary) As PDNumberTreeNode
            Return New PDNumberTreeNode(dic, valueType)
        End Function

        '/**
        ' * Set the names of for this node.  The keys should be java.lang.String and the
        ' * values must be a COSObjectable.  This method will set the appropriate upper and lower
        ' * limits based on the keys in the map.
        ' *
        ' * @param numbers The map of names to objects.
        ' */
        Public Sub setNumbers(Of T As COSObjectable)(ByVal numbers As Map(Of NInteger, T))
            If (numbers Is Nothing) Then
                node.setItem(COSName.NUMS, Nothing)
                node.setItem(COSName.LIMITS, Nothing)
            Else
                Dim keys As List(Of NInteger) = New ArrayList(Of NInteger)(numbers.keySet())
                Collections.sort(Of NInteger)(keys)
                Dim array As COSArray = New COSArray()
                For i As Integer = 0 To keys.size() - 1
                    Dim key As NInteger = keys.get(i)
                    array.add(COSInteger.get(key))
                    Dim obj As COSObjectable = numbers.get(key)
                    array.add(obj)
                Next
                Dim lower As NInteger = Nothing
                Dim upper As NInteger = Nothing
                If (keys.size() > 0) Then
                    lower = keys.get(0)
                    upper = keys.get(keys.size() - 1)
                End If
                setUpperLimit(upper)
                setLowerLimit(lower)
                node.setItem(COSName.NUMS, array)
            End If
        End Sub

        '/**
        ' * Get the highest value for a key in the name map.
        ' *
        ' * @return The highest value for a key in the map.
        ' */
        Public Function getUpperLimit() As NInteger
            Dim retval As NInteger = Nothing
            Dim arr As COSArray = node.getDictionaryObject(COSName.LIMITS)
            If (arr IsNot Nothing AndAlso arr.get(0) IsNot Nothing) Then
                retval = arr.getInt(1)
            End If
            Return retval
        End Function

        '/**
        ' * Set the highest value for the key in the map.
        ' *
        ' * @param upper The new highest value for a key in the map.
        ' */
        Private Sub setUpperLimit(ByVal upper As NInteger)
            Dim arr As COSArray = node.getDictionaryObject(COSName.LIMITS)
            If (arr Is Nothing) Then
                arr = New COSArray()
                arr.add(Nothing)
                arr.add(Nothing)
                node.setItem(COSName.LIMITS, arr)
            End If
            If (upper.HasValue) Then
                arr.setInt(1, upper.Value)
            Else
                Dim tmp As COSBase = Nothing
                arr.set(1, tmp) ' Nothing)
            End If
        End Sub

        '/**
        ' * Get the lowest value for a key in the name map.
        ' *
        ' * @return The lowest value for a key in the map.
        ' */
        Public Function getLowerLimit() As NInteger
            Dim retval As NInteger = Nothing
            Dim arr As COSArray = node.getDictionaryObject(COSName.LIMITS)
            If (arr IsNot Nothing AndAlso arr.get(0) IsNot Nothing) Then
                retval = arr.getInt(0)
            End If
            Return retval
        End Function

        '/**
        ' * Set the lowest value for the key in the map.
        ' *
        ' * @param lower The new lowest value for a key in the map.
        ' */
        Private Sub setLowerLimit(ByVal lower As NInteger)
            Dim arr As COSArray = node.getDictionaryObject(COSName.LIMITS)
            If (arr Is Nothing) Then
                arr = New COSArray()
                arr.add(Nothing)
                arr.add(Nothing)
                node.setItem(COSName.LIMITS, arr)
            End If
            If (lower.HasValue) Then
                arr.setInt(0, lower.Value)
            Else
                Dim tmp As COSBase = Nothing
                arr.set(0, tmp)
            End If
        End Sub


    End Class

End Namespace
