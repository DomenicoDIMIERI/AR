Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure

    '/**
    ' * An attribute object.
    ' *
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' *
    ' */
    Public MustInherit Class PDAttributeObject
        Inherits PDDictionaryWrapper

        '/**
        ' * Creates an attribute object.
        ' * 
        ' * @param dictionary the dictionary
        ' * @return the attribute object
        ' */
        Public Shared Function create(ByVal dictionary As COSDictionary) As PDAttributeObject
            Dim owner As String = dictionary.getNameAsString(COSName.O)
            If (PDUserAttributeObject.OWNER_USER_PROPERTIES.equals(owner)) Then
                Return New PDUserAttributeObject(dictionary)
            ElseIf (PDListAttributeObject.OWNER_LIST.equals(owner)) Then
                Return New PDListAttributeObject(dictionary)
            ElseIf (PDPrintFieldAttributeObject.OWNER_PRINT_FIELD.equals(owner)) Then
                Return New PDPrintFieldAttributeObject(dictionary)
            ElseIf (PDTableAttributeObject.OWNER_TABLE.equals(owner)) Then
                Return New PDTableAttributeObject(dictionary)
            ElseIf (PDLayoutAttributeObject.OWNER_LAYOUT.equals(owner)) Then
                Return New PDLayoutAttributeObject(dictionary)
            ElseIf (PDExportFormatAttributeObject.OWNER_XML_1_00.equals(owner) _
                OrElse PDExportFormatAttributeObject.OWNER_HTML_3_20.equals(owner) _
                OrElse PDExportFormatAttributeObject.OWNER_HTML_4_01.equals(owner) _
                OrElse PDExportFormatAttributeObject.OWNER_OEB_1_00.equals(owner) _
                OrElse PDExportFormatAttributeObject.OWNER_RTF_1_05.equals(owner) _
                OrElse PDExportFormatAttributeObject.OWNER_CSS_1_00.equals(owner) _
                OrElse PDExportFormatAttributeObject.OWNER_CSS_2_00.equals(owner)) Then
                Return New PDExportFormatAttributeObject(dictionary)
            End If
            Return New PDDefaultAttributeObject(dictionary)
        End Function

        Private structureElement As PDStructureElement

        '/**
        ' * Gets the structure element.
        ' * 
        ' * @return the structure element
        ' */
        Private Function getStructureElement() As PDStructureElement
            Return Me.structureElement
        End Function

        '/**
        ' * Sets the structure element.
        ' * 
        ' * @param structureElement the structure element
        ' */
        Protected Friend Sub setStructureElement(ByVal structureElement As PDStructureElement)
            Me.structureElement = structureElement
        End Sub


        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Creates a new attribute object with a given dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Returns the owner of the attributes.
        ' * 
        ' * @return the owner of the attributes
        ' */
        Public Function getOwner() As String
            Return Me.getCOSDictionary().getNameAsString(COSName.O)
        End Function

        '/**
        ' * Sets the owner of the attributes.
        ' * 
        ' * @param owner the owner of the attributes
        ' */
        Protected Sub setOwner(ByVal owner As String)
            Me.getCOSDictionary().setName(COSName.O, owner)
        End Sub

        '/**
        ' * Detects whether there are no properties in the attribute object.
        ' * 
        ' * @return <code>true</code> if the attribute object is empty,
        ' *  <code>false</code> otherwise
        ' */
        Public Function isEmpty() As Boolean
            ' only entry is the owner?
            Return (Me.getCOSDictionary().size() = 1) AndAlso (Me.getOwner() IsNot Nothing)
        End Function


        '/**
        ' * Notifies the attribute object change listeners if the attribute is changed.
        ' * 
        ' * @param oldBase old value
        ' * @param newBase new value
        ' */
        Protected Sub potentiallyNotifyChanged(ByVal oldBase As COSBase, ByVal newBase As COSBase)
            If (Me.isValueChanged(oldBase, newBase)) Then
                Me.notifyChanged()
            End If
        End Sub

        '/**
        ' * Is the value changed?
        ' * 
        ' * @param oldValue old value
        ' * @param newValue new value
        ' * @return <code>true</code> if the value is changed, <code>false</code>
        ' * otherwise
        ' */
        Private Function isValueChanged(ByVal oldValue As COSBase, ByVal newValue As COSBase) As Boolean
            If (oldValue Is Nothing) Then
                If (newValue Is Nothing) Then
                    Return False
                End If
                Return True
            End If
            Return Not oldValue.Equals(newValue)
        End Function

        '/**
        ' * Notifies the attribute object change listeners about a change in this
        ' * attribute object.
        ' */
        Protected Sub notifyChanged()
            If (Me.getStructureElement() IsNot Nothing) Then
                Me.getStructureElement().attributeChanged(Me)
            End If
        End Sub

        Public Overrides Function toString() As String
            Return New StringBuilder("O=").Append(Me.getOwner()).ToString()
        End Function

        '/**
        ' * Creates a String representation of an Object array.
        ' * 
        ' * @param array the Object array
        ' * @return the String representation
        ' */
        Protected Shared Function arrayToString(ByVal array() As Object) As String
            Dim sb As StringBuilder = New StringBuilder("[")
            For i As Integer = 0 To array.Length - 1
                If (i > 0) Then
                    sb.Append(", ")
                End If
                sb.Append(array(i))
            Next
            Return sb.Append("]").ToString()
        End Function

        '/**
        ' * Creates a String representation of a Single array.
        ' * 
        ' * @param array the Single array
        ' * @return the String representation
        ' */
        Protected Shared Function arrayToString(ByVal array() As Single) As String
            Dim sb As StringBuilder = New StringBuilder("[")
            For i As Integer = 0 To array.Length - 1
                If (i > 0) Then
                    sb.Append(", ")
                End If
                sb.Append(array(i))
            Next
            Return sb.Append("]").ToString()
        End Function

    End Class

End Namespace
