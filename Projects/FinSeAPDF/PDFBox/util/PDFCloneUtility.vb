Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.util

    '/**
    ' * Utility class used to clone PDF objects. It keeps track of objects it has already cloned.
    ' *
    ' * @version $Revision$
    ' */
    Public Class PDFCloneUtility

        Private destination As PDDocument
        Private clonedVersion As New HashMap(Of Object, COSBase)

        '/**
        ' * Creates a new instance for the given target document.
        ' * @param dest the destination PDF document that will receive the clones
        ' */
        Public Sub New(ByVal dest As PDDocument)
            Me.destination = dest
        End Sub

        '/**
        ' * Returns the destination PDF document this cloner instance is set up for.
        ' * @return the destination PDF document
        ' */
        Public Function getDestination() As PDDocument
            Return Me.destination
        End Function

        '/**
        ' * 
        ' * @param base 
        ' * @return 
        ' * @throws IOException if an I/O error occurs
        ' */

        ''' <summary>
        ''' Deep-clones the given object for inclusion into a different PDF document identified by the destination parameter.
        ''' </summary>
        ''' <param name="base">the initial object as the root of the deep-clone operation</param>
        ''' <returns>the cloned instance of the base object</returns>
        ''' <remarks></remarks>
        Public Function cloneForNewDocument(ByVal base As Object) As COSBase 'throws IOException
            If (base Is Nothing) Then Return Nothing
            Dim retval As COSBase = clonedVersion.get(base)
            If (retval IsNot Nothing) Then
                'we are done, it has already been converted.
            ElseIf (TypeOf (base) Is List) Then
                Dim array As New COSArray()
                Dim list As List = base
                For i As Integer = 0 To list.size() - 1
                    array.add(cloneForNewDocument(list.get(i)))
                Next
                retval = array
            ElseIf (TypeOf (base) Is COSObjectable AndAlso Not (TypeOf (base) Is COSBase)) Then
                retval = cloneForNewDocument(DirectCast(base, COSObjectable).getCOSObject())
                clonedVersion.put(base, retval)
            ElseIf (TypeOf (base) Is COSObject) Then
                Dim [object] As COSObject = base
                retval = cloneForNewDocument([object].getObject())
                clonedVersion.put(base, retval)
            ElseIf (TypeOf (base) Is COSArray) Then
                Dim newArray As New COSArray()
                Dim array As COSArray = base
                For i As Integer = 0 To array.size() - 1
                    newArray.add(cloneForNewDocument(array.get(i)))
                Next
                retval = newArray
                clonedVersion.put(base, retval)
            ElseIf (TypeOf (base) Is COSStream) Then
                Dim originalStream As COSStream = base
                Dim stream As New PDStream(destination, originalStream.getFilteredStream(), True)
                clonedVersion.put(base, stream.getStream())
                For Each entry As Map.Entry(Of COSName, COSBase) In originalStream.entrySet()
                    stream.getStream().setItem(entry.Key, cloneForNewDocument(entry.Value))
                Next
                retval = stream.getStream()
            ElseIf (TypeOf (base) Is COSDictionary) Then
                Dim dic As COSDictionary = base
                retval = New COSDictionary()
                clonedVersion.put(base, retval)
                For Each entry As Map.Entry(Of COSName, COSBase) In dic.entrySet()
                    DirectCast(retval, COSDictionary).setItem(entry.Key, cloneForNewDocument(entry.Value))
                Next
            Else
                retval = base
            End If
            clonedVersion.put(base, retval)
            Return retval
        End Function


        '/**
        ' * Merges two objects of the same type by deep-cloning its members.
        ' * <br/>
        ' * Base and target must be instances of the same class.
        ' * @param base the base object to be cloned
        ' * @param target the merge target
        ' * @throws IOException if an I/O error occurs
        ' */
        Public Sub cloneMerge(ByVal base As COSObjectable, ByVal target As COSObjectable) ' throws IOException
            If (base Is Nothing) Then Return
            Dim retval As COSBase = clonedVersion.get(base)
            If (retval IsNot Nothing) Then
                Return
                'we are done, it has already been converted. // ### Is that correct for cloneMerge???
            ElseIf (TypeOf (base) Is List) Then
                Dim array As New COSArray()
                Dim list As List = base
                For i As Integer = 0 To list.size() - 1
                    array.add(cloneForNewDocument(list.get(i)))
                Next
                DirectCast(target, List).add(array)
            ElseIf (TypeOf (base) Is COSObjectable AndAlso Not (TypeOf (base) Is COSBase)) Then
                cloneMerge(DirectCast(base, COSObjectable).getCOSObject(), DirectCast(target, COSObjectable).getCOSObject())
                clonedVersion.put(base, retval)
            ElseIf (TypeOf (base) Is COSObject) Then
                If (TypeOf (target) Is COSObject) Then
                    cloneMerge(DirectCast(base, COSObject).getObject(), DirectCast(target, COSObject).getObject())
                ElseIf (TypeOf (target) Is COSDictionary) Then
                    cloneMerge(DirectCast(base, COSObject).getObject(), DirectCast(target, COSDictionary))
                End If
                clonedVersion.put(base, retval)
            ElseIf (TypeOf (base) Is COSArray) Then
                Dim array As COSArray = base
                For i As Integer = 0 To array.size() - 1
                    DirectCast(target, COSArray).add(cloneForNewDocument(array.get(i)))
                Next
                clonedVersion.put(base, retval)
            ElseIf (TypeOf (base) Is COSStream) Then
                ' does that make sense???
                Dim originalStream As COSStream = base
                Dim stream As New PDStream(destination, originalStream.getFilteredStream(), True)
                clonedVersion.put(base, stream.getStream())
                For Each entry As Map.Entry(Of COSName, COSBase) In originalStream.entrySet()
                    stream.getStream().setItem(entry.Key, cloneForNewDocument(entry.Value))
                Next
                retval = stream.getStream()
                target = retval
            ElseIf (TypeOf (base) Is COSDictionary) Then
                Dim dic As COSDictionary = base
                clonedVersion.put(base, retval)
                For Each entry As Map.Entry(Of COSName, COSBase) In dic.entrySet()
                    Dim key As COSName = entry.Key
                    Dim value As COSBase = entry.Value
                    If (DirectCast(target, COSDictionary).getItem(key) IsNot Nothing) Then
                        cloneMerge(value, DirectCast(target, COSDictionary).getItem(key))
                    Else
                        DirectCast(target, COSDictionary).setItem(key, cloneForNewDocument(value))
                    End If
                Next
            Else
                retval = base
            End If
            clonedVersion.put(base, retval)
        End Sub

    End Class

End Namespace
