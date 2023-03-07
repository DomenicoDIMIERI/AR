Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.util


Namespace org.apache.pdfbox.pdmodel.documentinterchange.markedcontent

    '/**
    ' * A marked content.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDMarkedContent


        '/**
        ' * Creates a marked-content sequence.
        ' * 
        ' * @param tag the tag
        ' * @param properties the properties
        ' * @return the marked-content sequence
        ' */
        Public Shared Function create(ByVal tag As COSName, ByVal properties As COSDictionary) As PDMarkedContent
            If (COSName.ARTIFACT.equals(tag)) Then
                Return New PDArtifactMarkedContent(properties)
            End If
            Return New PDMarkedContent(tag, properties)
        End Function


        Private tag As String
        Private properties As COSDictionary
        Private contents As List(Of Object)


        '/**
        ' * Creates a new marked content object.
        ' * 
        ' * @param tag the tag
        ' * @param properties the properties
        ' */
        Public Sub New(ByVal tag As COSName, ByVal properties As COSDictionary)
            If (tag Is Nothing) Then
                Me.tag = ""
            Else
                Me.tag = tag.getName
            End If
            Me.properties = properties
            Me.contents = New ArrayList(Of Object)()
        End Sub

        ' /**
        '* Gets the tag.
        '* 
        '* @return the tag
        '*/
        Public Function getTag() As String
            Return Me.tag
        End Function

        '/**
        ' * Gets the properties.
        ' * 
        ' * @return the properties
        ' */
        Public Function getProperties() As COSDictionary
            Return Me.properties
        End Function

        '/**
        ' * Gets the marked-content identifier.
        ' * 
        ' * @return the marked-content identifier
        ' */
        Public Function getMCID() As NInteger
            If (Me.getProperties Is Nothing) Then
                Return Nothing
            Else
                Return Me.getProperties().getInt(COSName.MCID)
            End If
        End Function

        '/**
        ' * Gets the language (Lang).
        ' * 
        ' * @return the language
        ' */
        Public Function getLanguage() As String
            If (Me.getProperties Is Nothing) Then
                Return ""
            Else
                Return Me.getProperties().getNameAsString(COSName.LANG)
            End If
        End Function

        '/**
        ' * Gets the actual text (ActualText).
        ' * 
        ' * @return the actual text
        ' */
        Public Function getActualText() As String
            If (Me.getProperties Is Nothing) Then
                Return ""
            Else
                Return Me.getProperties().getString(COSName.ACTUAL_TEXT)
            End If
        End Function

        '/**
        ' * Gets the alternate description (Alt).
        ' * 
        ' * @return the alternate description
        ' */
        Public Function getAlternateDescription() As String
            If (Me.getProperties Is Nothing) Then
                Return ""
            Else
                Return Me.getProperties().getString(COSName.ALT)
            End If
        End Function

        '/**
        ' * Gets the expanded form (E).
        ' * 
        ' * @return the expanded form
        ' */
        Public Function getExpandedForm() As String
            If (Me.getProperties Is Nothing) Then
                Return ""
            Else
                Return Me.getProperties().getString(COSName.E)
            End If
        End Function

        '/**
        ' * Gets the contents of the marked content sequence. Can be
        ' * <ul>
        ' *   <li>{@link TextPosition},</li>
        ' *   <li>{@link PDMarkedContent}, or</li>
        ' *   <li>{@link PDXObject}.</li>
        ' * </ul>
        ' * 
        ' * @return the contents of the marked content sequence
        ' */
        Public Function getContents() As List(Of Object)
            Return Me.contents
        End Function

        '/**
        ' * Adds a text position to the contents.
        ' * 
        ' * @param text the text position
        ' */
        Public Sub addText(ByVal text As TextPosition)
            Me.getContents().add(text)
        End Sub

        '/**
        ' * Adds a marked content to the contents.
        ' * 
        ' * @param markedContent the marked content
        ' */
        Public Sub addMarkedContent(ByVal markedContent As PDMarkedContent)
            Me.getContents().add(markedContent)
        End Sub

        '/**
        ' * Adds an XObject to the contents.
        ' * 
        ' * @param xobject the XObject
        ' */
        Public Sub addXObject(ByVal xobject As PDXObject)
            Me.getContents().add(xobject)
        End Sub


        Public Overrides Function toString() As String
            Dim sb As New StringBuilder("tag=")
            sb.Append(Me.tag).Append(", properties=").Append(Me.properties)
            sb.append(", contents=").append(Me.contents)
            Return sb.toString()
        End Function

    End Class

End Namespace
