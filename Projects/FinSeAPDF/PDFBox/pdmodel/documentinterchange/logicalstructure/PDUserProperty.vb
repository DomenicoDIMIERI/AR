Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure

    '/**
    ' * A user property.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDUserProperty
        Inherits PDDictionaryWrapper

        Private userAttributeObject As PDUserAttributeObject

        '/**
        ' * Creates a new user property.
        ' * 
        ' * @param the user attribute object
        ' */
        Public Sub New(ByVal userAttributeObject As PDUserAttributeObject)
            Me.userAttributeObject = userAttributeObject
        End Sub

        '/**
        ' * Creates a user property with a given dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' * @param the user attribute object
        ' */
        Public Sub New(ByVal dictionary As COSDictionary, ByVal userAttributeObject As PDUserAttributeObject)
            MyBase.New(dictionary)
            Me.userAttributeObject = userAttributeObject
        End Sub


        '/**
        ' * Returns the property name.
        ' * 
        ' * @return the property name
        ' */
        Public Function getName() As String
            Return Me.getCOSDictionary().getNameAsString(COSName.N)
        End Function

        '/**
        ' * Sets the property name.
        ' * 
        ' * @param name the property name
        ' */
        Public Sub setName(ByVal name As String)
            Me.potentiallyNotifyChanged(Me.getName(), name)
            Me.getCOSDictionary().setName(COSName.N, name)
        End Sub

        '/**
        ' * Returns the property value.
        ' * 
        ' * @return the property value
        ' */
        Public Function getValue() As COSBase
            Return Me.getCOSDictionary().getDictionaryObject(COSName.V)
        End Function

        '/**
        ' * Sets the property value.
        ' * 
        ' * @param value the property value
        ' */
        Public Sub setValue(ByVal value As COSBase)
            Me.potentiallyNotifyChanged(Me.getValue(), value)
            Me.getCOSDictionary().setItem(COSName.V, value)
        End Sub

        '/**
        ' * Returns the string for the property value.
        ' * 
        ' * @return the string for the property value
        ' */
        Public Function getFormattedValue() As String
            Return Me.getCOSDictionary().getString(COSName.F)
        End Function

        '/**
        ' * Sets the string for the property value.
        ' * 
        ' * @param formattedValue the string for the property value
        ' */
        Public Sub setFormattedValue(ByVal formattedValue As String)
            Me.potentiallyNotifyChanged(Me.getFormattedValue(), formattedValue)
            Me.getCOSDictionary().setString(COSName.F, formattedValue)
        End Sub

        '/**
        ' * Shall the property be hidden?
        ' * 
        ' * @return <code>true</code> if the property shall be hidden,
        ' * <code>false</code> otherwise
        ' */
        Public Function isHidden() As Boolean
            Return Me.getCOSDictionary().getBoolean(COSName.H, False)
        End Function

        '/**
        ' * Specifies whether the property shall be hidden.
        ' * 
        ' * @param hidden <code>true</code> if the property shall be hidden,
        ' * <code>false</code> otherwise
        ' */
        Public Sub setHidden(ByVal hidden As Boolean)
            Me.potentiallyNotifyChanged(Me.isHidden(), hidden)
            Me.getCOSDictionary().setBoolean(COSName.H, hidden)
        End Sub


        Public Overrides Function toString() As String
            Return New StringBuilder("Name=").append(Me.getName()).append(", Value=").append(Me.getValue()).append(", FormattedValue=").append(Me.getFormattedValue()).append(", Hidden=").append(Me.isHidden()).toString()
        End Function


        '/**
        ' * Notifies the user attribute object if the user property is changed.
        ' * 
        ' * @param oldEntry old entry
        ' * @param newEntry new entry
        ' */
        Private Sub potentiallyNotifyChanged(ByVal oldEntry As Object, ByVal newEntry As Object)
            If (Me.isEntryChanged(oldEntry, newEntry)) Then
                Me.userAttributeObject.userPropertyChanged(Me)
            End If
        End Sub

        '/**
        ' * Is the value changed?
        ' * 
        ' * @param oldEntry old entry
        ' * @param newEntry new entry
        ' * @return <code>true</code> if the entry is changed, <code>false</code>
        ' * otherwise
        ' */
        Private Function isEntryChanged(ByVal oldEntry As Object, ByVal newEntry As Object) As Boolean
            If (oldEntry Is Nothing) Then
                If (newEntry Is Nothing) Then
                    Return False
                End If
                Return True
            End If
            Return Not oldEntry.Equals(newEntry)
        End Function

    End Class

End Namespace
