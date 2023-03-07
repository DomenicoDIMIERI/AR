Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure


    '/**
    ' * A User attribute object.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDUserAttributeObject
        Inherits PDAttributeObject

        ''' <summary>
        ''' Attribute owner for user properties
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_USER_PROPERTIES As String = "UserProperties"


        ''' <summary>
        ''' Default constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.setOwner(OWNER_USER_PROPERTIES)
        End Sub

        '/**
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Returns the user properties.
        ' * 
        ' * @return the user properties
        ' */
        Public Function getOwnerUserProperties() As List(Of PDUserProperty)
            Dim p As COSArray = Me.getCOSDictionary().getDictionaryObject(COSName.P)
            Dim properties As List(Of PDUserProperty) = New ArrayList(Of PDUserProperty)(p.size())
            For i As Integer = 0 To p.size() - 1
                properties.add(New PDUserProperty(p.getObject(i), Me)) 'COSDictionary
            Next
            Return properties
        End Function

        '/**
        ' * Sets the user properties.
        ' * 
        ' * @param userProperties the user properties
        ' */
        Public Sub setUserProperties(ByVal userProperties As List(Of PDUserProperty))
            Dim p As COSArray = New COSArray()
            For Each userProperty As PDUserProperty In userProperties
                p.add(userProperty)
            Next
            Me.getCOSDictionary().setItem(COSName.P, p)
        End Sub

        '/**
        ' * Adds a user property.
        ' * 
        ' * @param userProperty the user property
        ' */
        Public Sub addUserProperty(ByVal userProperty As PDUserProperty)
            Dim p As COSArray = Me.getCOSDictionary().getDictionaryObject(COSName.P)
            p.add(userProperty)
            Me.notifyChanged()
        End Sub

        '/**
        ' * Removes a user property.
        ' * 
        ' * @param userProperty the user property
        ' */
        Public Sub removeUserProperty(ByVal userProperty As PDUserProperty)
            If (userProperty Is Nothing) Then Return
            Dim p As COSArray = Me.getCOSDictionary().getDictionaryObject(COSName.P)
            p.remove(userProperty.getCOSObject())
            Me.notifyChanged()
        End Sub

        Public Sub userPropertyChanged(ByVal userProperty As PDUserProperty)
        End Sub


        Public Overrides Function toString() As String
            Return New StringBuilder().Append(MyBase.toString()).Append(", userProperties=").Append(Me.getOwnerUserProperties()).toString()
        End Function

    End Class

End Namespace
