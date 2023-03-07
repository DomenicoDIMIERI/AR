Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This class represents an appearance characteristics dictionary.
    ' *
    ' * @version $Revision: 1.0 $ 
    ' *
    ' */
    Public Class PDAppearanceCharacteristicsDictionary
        Implements COSObjectable

        Private dictionary As COSDictionary

        '/**
        ' * Constructor.
        ' * 
        ' * @param dict dictionary
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            Me.dictionary = dict
        End Sub


        '/**
        ' * returns the dictionary.
        ' * @return the dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return Me.dictionary
        End Function
		 
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.dictionary
        End Function

        '/**
        ' * This will retrieve the rotation of the annotation widget.
        ' * It must be a multiple of 90. Default is 0 
        ' * @return the rotation
        ' */
        Public Function getRotation() As Integer
            Return Me.getDictionary().getInt(COSName.R, 0)
        End Function

        '/**
        ' * This will set the rotation.
        ' * 
        ' * @param rotation the rotation as a multiple of 90
        ' */
        Public Sub setRotation(ByVal rotation As Integer)
            Me.getDictionary().setInt(COSName.R, rotation)
        End Sub

        '/**
        ' * This will retrieve the border color.
        ' * 
        ' * @return the border color.
        ' */
        Public Function getBorderColour() As PDGamma
            Dim c As COSBase = Me.getDictionary().getItem(COSName.getPDFName("BC"))
            If (TypeOf (c) Is COSArray) Then
                Return New PDGamma(c)
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the border color.
        ' * 
        ' * @param c the border color
        ' */
        Public Sub setBorderColour(ByVal c As PDGamma)
            Me.getDictionary().setItem("BC", c)
        End Sub

        '/**
        ' * This will retrieve the background color.
        ' * 
        ' * @return the background color.
        ' */
        Public Function getBackground() As PDGamma
            Dim c As COSBase = Me.getDictionary().getItem(COSName.getPDFName("BG"))
            If (TypeOf (c) Is COSArray) Then
                Return New PDGamma(c)
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the background color.
        ' * 
        ' * @param c the background color
        ' */
        Public Sub setBackground(ByVal c As PDGamma)
            Me.getDictionary().setItem("BG", c)
        End Sub

        '/**
        ' * This will retrieve the normal caption.
        ' * 
        ' * @return the normal caption.
        ' */
        Public Function getNormalCaption() As String
            Return Me.getDictionary().getString("CA")
        End Function

        '/**
        ' * This will set the normal caption.
        ' * 
        ' * @param caption the normal caption
        ' */
        Public Sub setNormalCaption(ByVal caption As String)
            Me.getDictionary().setString("CA", caption)
        End Sub

        '/**
        ' * This will retrieve the rollover caption.
        ' * 
        ' * @return the rollover caption.
        ' */
        Public Function getRolloverCaption() As String
            Return Me.getDictionary().getString("RC")
        End Function

        '/**
        ' * This will set the rollover caption.
        ' * 
        ' * @param caption the rollover caption
        ' */
        Public Sub setRolloverCaption(ByVal caption As String)
            Me.getDictionary().setString("RC", caption)
        End Sub

        '/**
        ' * This will retrieve the alternate caption.
        ' * 
        ' * @return the alternate caption.
        ' */
        Public Function getAlternateCaption() As String
            Return Me.getDictionary().getString("AC")
        End Function

        '/**
        ' * This will set the alternate caption.
        ' * 
        ' * @param caption the alternate caption
        ' */
        Public Sub setAlternateCaption(ByVal caption As String)
            Me.getDictionary().setString("AC", caption)
        End Sub

        '/**
        ' * This will retrieve the normal icon.
        ' * 
        ' * @return the normal icon.
        ' */
        Public Function getNormalIcon() As PDXObjectForm
            Dim i As COSBase = Me.getDictionary().getDictionaryObject("I")
            If (TypeOf (i) Is COSStream) Then
                Return New PDXObjectForm(i)
            End If
            Return Nothing
        End Function

        '/**
        ' * This will retrieve the rollover icon.
        ' * 
        ' * @return the rollover icon
        ' */
        Public Function getRolloverIcon() As PDXObjectForm
            Dim i As COSBase = Me.getDictionary().getDictionaryObject("RI")
            If (TypeOf (i) Is COSStream) Then
                Return New PDXObjectForm(i)
            End If
            Return Nothing
        End Function

        '/**
        ' * This will retrieve the alternate icon.
        ' * 
        ' * @return the alternate icon.
        ' */
        Public Function getAlternateIcon() As PDXObjectForm
            Dim i As COSBase = Me.getDictionary().getDictionaryObject("IX")
            If (TypeOf (i) Is COSStream) Then
                Return New PDXObjectForm(i)
            End If
            Return Nothing
        End Function

    End Class

End Namespace