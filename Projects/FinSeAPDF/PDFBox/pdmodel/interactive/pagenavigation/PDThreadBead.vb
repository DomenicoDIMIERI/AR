Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.pagenavigation

    '/**
    ' * This a single bead in a thread in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDThreadBead
        Implements COSObjectable


        Private bead As COSDictionary

        '/**
        ' * Constructor that is used for a preexisting dictionary.
        ' *
        ' * @param b The underlying dictionary.
        ' */
        Public Sub New(ByVal b As COSDictionary)
            bead = b
        End Sub

        '/**
        ' * Default constructor.
        ' *
        ' */
        Public Sub New()
            bead = New COSDictionary()
            bead.setName("Type", "Bead")
            setNextBead(Me)
            setPreviousBead(Me)
        End Sub

        '/**
        ' * This will get the underlying dictionary that this object wraps.
        ' *
        ' * @return The underlying info dictionary.
        ' */
        Public Function getDictionary() As COSDictionary
            Return bead
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return bead
        End Function

        '/**
        ' * This will get the thread that this bead is part of.  This is only required
        ' * for the first bead in a thread, so other beads 'may' return null.
        ' *
        ' * @return The thread that this bead is part of.
        ' */
        Public Function getThread() As PDThread
            Dim retval As PDThread = Nothing
            Dim dic As COSDictionary = bead.getDictionaryObject("T")
            If (dic IsNot Nothing) Then
                retval = New PDThread(dic)
            End If
            Return retval
        End Function

        '/**
        ' * Set the thread that this bead is part of.  This is only required for the
        ' * first bead in a thread.  Note: This property is set for you by the PDThread.setFirstBead() method.
        ' *
        ' * @param thread The thread that this bead is part of.
        ' */
        Public Sub setThread(ByVal thread As PDThread)
            bead.setItem("T", thread)
        End Sub

        '/**
        ' * This will get the next bead.  If this bead is the last bead in the list then this
        ' * will return the first bead.
        ' *
        ' * @return The next bead in the list or the first bead if this is the last bead.
        ' */
        Public Function getNextBead() As PDThreadBead
            Return New PDThreadBead(bead.getDictionaryObject("N"))
        End Function

        '/**
        ' * Set the next bead in the thread.
        ' *
        ' * @param next The next bead.
        ' */
        Protected Sub setNextBead(ByVal [next] As PDThreadBead)
            bead.setItem("N", [next])
        End Sub

        '/**
        ' * This will get the previous bead.  If this bead is the first bead in the list then this
        ' * will return the last bead.
        ' *
        ' * @return The previous bead in the list or the last bead if this is the first bead.
        ' */
        Public Function getPreviousBead() As PDThreadBead
            Return New PDThreadBead(bead.getDictionaryObject("V"))
        End Function

        '/**
        ' * Set the previous bead in the thread.
        ' *
        ' * @param previous The previous bead.
        ' */
        Protected Sub setPreviousBead(ByVal previous As PDThreadBead)
            bead.setItem("V", previous)
        End Sub

        '/**
        ' * Append a bead after this bead.  This will correctly set the next/previous beads in the
        ' * linked list.
        ' *
        ' * @param append The bead to insert.
        ' */
        Public Sub appendBead(ByVal append As PDThreadBead)
            Dim nextBead As PDThreadBead = getNextBead()
            nextBead.setPreviousBead(append)
            append.setNextBead(nextBead)
            setNextBead(append)
            append.setPreviousBead(Me)
        End Sub

        '/**
        ' * Get the page that this bead is part of.
        ' *
        ' * @return The page that this bead is part of.
        ' */
        Public Function getPage() As PDPage
            Dim page As PDPage = Nothing
            Dim dic As COSDictionary = bead.getDictionaryObject("P")
            If (dic IsNot Nothing) Then
                page = New PDPage(dic)
            End If
            Return page
        End Function

        '/**
        ' * Set the page that this bead is part of.  This is a required property and must be
        ' * set when creating a new bead.  The PDPage object also has a list of beads in the natural
        ' * reading order.  It is recommended that you add this object to that list as well.
        ' *
        ' * @param page The page that this bead is on.
        ' */
        Public Sub setPage(ByVal page As PDPage)
            bead.setItem("P", page)
        End Sub

        '/**
        ' * The rectangle on the page that this bead is part of.
        ' *
        ' * @return The part of the page that this bead covers.
        ' */
        Public Function getRectangle() As PDRectangle
            Dim rect As PDRectangle = Nothing
            Dim array As COSArray = bead.getDictionaryObject(COSName.R)
            If (Array IsNot Nothing) Then
                rect = New PDRectangle(array)
            End If
            Return rect
        End Function

        '/**
        ' * Set the rectangle on the page that this bead covers.
        ' *
        ' * @param rect The portion of the page that this bead covers.
        ' */
        Public Sub setRectangle(ByVal rect As PDRectangle)
            bead.setItem(COSName.R, rect)
        End Sub
    End Class

End Namespace
