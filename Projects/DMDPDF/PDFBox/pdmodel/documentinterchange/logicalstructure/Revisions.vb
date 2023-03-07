Imports System.Text

Imports FinSeA

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure

    '/**
    ' * 
    ' * @author Koch
    ' * @version $Revision: $
    ' *
    ' * @param <T> the type of object to store the revision numbers with
    ' */
    Public Class Revisions(Of T)

        Private objects As List(Of T)
        Private revisionNumbers As List(Of NInteger)

        Private Function getObjects() As List(Of T)
            If (Me.objects Is Nothing) Then
                Me.objects = New ArrayList(Of T)
            End If
            Return Me.objects
        End Function

        Private Function getRevisionNumbers() As List(Of NInteger)
            If (Me.revisionNumbers Is Nothing) Then
                Me.revisionNumbers = New ArrayList(Of NInteger)
            End If
            Return Me.revisionNumbers
        End Function


        '/**
        ' * 
        ' */
        Public Sub New()
        End Sub


        '/**
        ' * Returns the object at the specified position.
        ' * 
        ' * @param index the position
        ' * @return the object
        ' * @throws IndexOutOfBoundsException if the index is out of range
        ' */
        Public Function getObject(ByVal index As Integer) As T ' throws IndexOutOfBoundsException
            Return Me.getObjects().get(index)
        End Function

        '/**
        ' * Returns the revision number at the specified position.
        ' * 
        ' * @param index the position
        ' * @return the revision number
        ' * @throws IndexOutOfBoundsException if the index is out of range
        ' */
        Public Function getRevisionNumber(ByVal index As Integer) As Integer ' throws IndexOutOfBoundsException
            Return Me.getRevisionNumbers().get(index)
        End Function

        '/**
        ' * Adds an object with a specified revision number.
        ' * 
        ' * @param object the object
        ' * @param revisionNumber the revision number
        ' */
        Public Sub addObject(ByVal [object] As T, ByVal revisionNumber As Integer)
            Me.getObjects().add([object])
            Me.getRevisionNumbers().add(revisionNumber)
        End Sub

        '/**
        ' * Sets the revision number of a specified object.
        ' * 
        ' * @param object the object
        ' * @param revisionNumber the revision number
        ' */
        Protected Friend Sub setRevisionNumber(ByVal [object] As T, ByVal revisionNumber As Integer)
            Dim index As Integer = Me.getObjects().indexOf([object])
            If (index > -1) Then
                Me.getRevisionNumbers().set(index, revisionNumber)
            End If
        End Sub

        '/**
        ' * Returns the size.
        ' * 
        ' * @return the size
        ' */
        Public Function size() As Integer
            Return Me.getObjects().size()
        End Function

        Public Overrides Function toString() As String
            Dim sb As New StringBuilder()
            For i As Integer = 0 To Me.getObjects().size()
                If (i > 0) Then sb.Append("; ")
                sb.Append("object=").Append(Me.getObjects().get(i)).Append(", revisionNumber=").Append(Me.getRevisionNumber(i))
            Next
            Return sb.ToString()
        End Function


    End Class

End Namespace