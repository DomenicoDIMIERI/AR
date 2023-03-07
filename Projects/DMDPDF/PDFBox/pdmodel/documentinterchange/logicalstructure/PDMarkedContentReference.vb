Imports System.Text

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure


    '/**
    ' * A marked-content reference.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDMarkedContentReference
        Implements COSObjectable

        Public Const TYPE As String = "MCR"

        Private dictionary As COSDictionary

        Protected Function getCOSDictionary() As COSDictionary
            Return Me.dictionary
        End Function

        '/**
        ' * Default constructor
        ' */
        Public Sub New()
            Me.dictionary = New COSDictionary()
            Me.dictionary.setName(COSName.TYPE, TYPE)
        End Sub

        '/**
        ' * Constructor for an existing marked content reference.
        ' * 
        ' * @param pageDic the page dictionary
        ' * @param mcid the marked content indentifier
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            Me.dictionary = dictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.dictionary
        End Function

        '/**
        ' * Gets the page.
        ' * 
        ' * @return the page
        ' */
        Public Function getPage() As PDPage
            Dim pg As COSDictionary = Me.getCOSDictionary().getDictionaryObject(COSName.PG)
            If (pg IsNot Nothing) Then
                Return New PDPage(pg)
            End If
            Return Nothing
        End Function

        '/**
        ' * Sets the page.
        ' * 
        ' * @param page the page
        ' */
        Public Sub setPage(ByVal page As PDPage)
            Me.getCOSDictionary().setItem(COSName.PG, page)
        End Sub

        '/**
        ' * Gets the marked content identifier.
        ' * 
        ' * @return the marked content identifier
        ' */
        Public Function getMCID() As Integer
            Return Me.getCOSDictionary().getInt(COSName.MCID)
        End Function

        '/**
        ' * Sets the marked content identifier.
        ' * 
        ' * @param mcid the marked content identifier
        ' */
        Public Sub setMCID(ByVal mcid As Integer)
            Me.getCOSDictionary().setInt(COSName.MCID, mcid)
        End Sub

        Public Overrides Function toString() As String
            Return New StringBuilder().Append("mcid=").Append(Me.getMCID()).ToString()
        End Function

    End Class

End Namespace