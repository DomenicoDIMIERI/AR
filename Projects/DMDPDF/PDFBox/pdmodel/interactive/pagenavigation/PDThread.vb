Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.pagenavigation

    '/**
    ' * This a single thread in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDThread
        Implements COSObjectable

        Private thread As COSDictionary

        '/**
        ' * Constructor that is used for a preexisting dictionary.
        ' *
        ' * @param t The underlying dictionary.
        ' */
        Public Sub New(ByVal t As COSDictionary)
            thread = t
        End Sub

        '/**
        ' * Default constructor.
        ' *
        ' */
        Public Sub New()
            thread = New COSDictionary()
            thread.setName("Type", "Thread")
        End Sub

        '/**
        ' * This will get the underlying dictionary that this object wraps.
        ' *
        ' * @return The underlying info dictionary.
        ' */
        Public Function getDictionary() As COSDictionary
            Return thread
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return thread
        End Function

        '/**
        ' * Get info about the thread, or null if there is nothing.
        ' *
        ' * @return The thread information.
        ' */
        Public Function getThreadInfo() As PDDocumentInformation
            Dim retval As PDDocumentInformation = Nothing
            Dim info As COSDictionary = thread.getDictionaryObject("I")
            If (info IsNot Nothing) Then
                retval = New PDDocumentInformation(info)
            End If
            Return retval
        End Function

        '/**
        ' * Set the thread info, can be null.
        ' *
        ' * @param info The info dictionary about this thread.
        ' */
        Public Sub setThreadInfo(ByVal info As PDDocumentInformation)
            thread.setItem("I", info)
        End Sub

        '/**
        ' * Get the first bead in the thread, or null if it has not been set yet.  This
        ' * is a required field for this object.
        ' *
        ' * @return The first bead in the thread.
        ' */
        Public Function getFirstBead() As PDThreadBead
            Dim retval As PDThreadBead = Nothing
            Dim bead As COSDictionary = thread.getDictionaryObject("F")
            If (bead IsNot Nothing) Then
                retval = New PDThreadBead(bead)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the first bead in the thread.  When this is set it will
        ' * also set the thread property of the bead object.
        ' *
        ' * @param bead The first bead in the thread.
        ' */
        Public Sub setFirstBead(ByVal bead As PDThreadBead)
            If (bead IsNot Nothing) Then
                bead.setThread(Me)
            End If
            thread.setItem("F", bead)
        End Sub

    End Class

End Namespace
