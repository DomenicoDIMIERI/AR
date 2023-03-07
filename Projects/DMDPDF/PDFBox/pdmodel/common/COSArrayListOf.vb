Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO
Imports FinSeA

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * This is an implementation of a List that will sync its contents to a COSArray.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.15 $
    ' */
    Public Class COSArrayList(Of E)
        Inherits COSArrayList
        Implements List(Of E)

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param actualList The list of standard java objects
        ' * @param cosArray The COS array object to sync to.
        ' */
        Public Sub New(ByVal actualList As List(Of E), ByVal cosArray As COSArray)
            MyBase.New(actualList, cosArray)
        End Sub

        '/**
        ' * This is a really special constructor.  Sometimes the PDF spec says
        ' * that a dictionary entry can either be a single item or an array of those
        ' * items.  But in the PDModel interface we really just want to always return
        ' * a java.util.List.  In the case were we get the list and never modify it
        ' * we don't want to convert to COSArray and put one element, unless we append
        ' * to the list.  So here we are going to create this object with a single
        ' * item instead of a list, but allow more items to be added and then converted
        ' * to an array.
        ' *
        ' * @param actualObject The PDModel object.
        ' * @param item The COS Model object.
        ' * @param dictionary The dictionary that holds the item, and will hold the array if an item is added.
        ' * @param dictionaryKey The key into the dictionary to set the item.
        ' */
        Public Sub New(ByVal actualObject As E, ByVal item As COSBase, ByVal dictionary As COSDictionary, ByVal dictionaryKey As COSName)
            MyBase.New(actualObject, item, dictionary, dictionaryKey)
        End Sub

        '/**
        ' * @deprecated use the {@link #COSArrayList(E, COSBase, COSDictionary, COSName)} method instead
        ' */
        <Obsolete("use the {@link #COSArrayList(E, COSBase, COSDictionary, COSName)} method instead")> _
        Public Sub New(ByVal actualObject As E, ByVal item As COSBase, ByVal dictionary As COSDictionary, ByVal dictionaryKey As String)
            Me.New(actualObject, item, dictionary, COSName.getPDFName(dictionaryKey))
        End Sub


        Public Shadows Function contains(ByVal o As E) As Boolean Implements List(Of E).contains
            Return MyBase.contains(o)
        End Function


        Public Shadows Function containsAll(ByVal c As ICollection(Of E)) As Boolean Implements ICollection(Of E).containsAll
            Return MyBase.containsAll(c)
        End Function




        Public Shadows Function addAll(ByVal index As Integer, ByVal c As ICollection(Of E)) As Boolean Implements List(Of E).addAll
            Return MyBase.addAll(index, c)
        End Function


        Public Shadows Function removeAll(ByVal c As ICollection(Of E)) As Boolean Implements ICollection(Of E).removeAll
            Return MyBase.removeAll(c)
        End Function

        Public Shadows Function retainAll(ByVal c As ICollection(Of E)) As Boolean Implements ICollection(Of E).retainAll
            Return MyBase.retainAll(c)
        End Function

        Public Shadows Function [set](ByVal index As Integer, ByVal elem As E) As E Implements List(Of E).set
            Return MyBase.set(index, elem)
        End Function

        Public Shadows Sub add(ByVal index As Integer, ByVal elem As E) Implements List(Of E).add
            MyBase.add(index, elem)
        End Sub

        Public Shadows Function remove(ByVal index As Integer) As E Implements List(Of E).remove
            Return MyBase.remove(index)
        End Function

        Public Shadows Function indexOf(ByVal o As E) As Integer Implements List(Of E).indexOf
            Return MyBase.indexOf(o)
        End Function

        Public Shadows Function lastIndexOf(ByVal o As E) As Integer Implements List(Of E).lastIndexOf
            Return MyBase.lastIndexOf(o) '.indexOf(o)
        End Function


        'public ListIterator<E> listIterator()
        '{
        '    return actual.listIterator();
        '}

        '/**
        ' * {@inheritDoc}
        ' */
        'public ListIterator<E> listIterator(int index)
        '{
        '    return actual.listIterator( index );
        '}
        'Public Shadows Function GetEnumerator() As Global.System.Collections.Generic.IEnumerator(Of E) Implements ICollection(Of E).GetEnumerator
        '    Return MyBase.GetEnumerator
        'End Function

        Public Shadows Function subList(ByVal fromIndex As Integer, ByVal toIndex As Integer) As List(Of E) Implements List(Of E).subList
            Return MyBase.subList(fromIndex, toIndex)
        End Function

        Public Shadows Function [get](index As Integer) As E Implements List(Of E).get
            Return MyBase.get(index)
        End Function


        Private Function GetEnumerator1() As Global.System.Collections.Generic.IEnumerator(Of E) Implements Global.System.Collections.Generic.IEnumerable(Of E).GetEnumerator
            Return MyBase.GetEnumerator
        End Function

        
        Public Shadows Function add(ByVal elem As E) As Boolean Implements ICollection(Of E).add
            Return MyBase.add(elem)
        End Function

        Public Shadows Function remove(ByVal o As E) As Boolean Implements ICollection(Of E).remove
            Return MyBase.remove(o)
        End Function

        Public Shadows Function addAll(ByVal c As ICollection(Of E)) As Boolean Implements ICollection(Of E).addAll
            Return MyBase.addAll(c)
        End Function

        Private Function _add(o As E) As Boolean Implements List(Of E).add
            Return Me.add(o)
        End Function

        Private Function _addAll(col As ICollection(Of E)) As Boolean Implements List(Of E).addAll
            Return Me.addAll(col)
        End Function

        Private Function _remove(o As E) As Boolean Implements List(Of E).remove
            Return Me.remove(o)
        End Function
    End Class

End Namespace