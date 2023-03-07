Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination

    '/**
    ' * This represents a destination to a page by referencing it with a name.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDNamedDestination
        Inherits PDDestination

        Private namedDestination As COSBase

        '/**
        ' * Constructor.
        ' *
        ' * @param dest The named destination.
        ' */
        Public Sub New(ByVal dest As COSString)
            namedDestination = dest
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dest The named destination.
        ' */
        Public Sub New(ByVal dest As COSName)
            namedDestination = dest
        End Sub

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            'default, so do nothing
        End Sub

        '/**
        ' * Default constructor.
        ' *
        ' * @param dest The named destination.
        ' */
        Public Sub New(ByVal dest As String)
            namedDestination = New COSString(dest)
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return namedDestination
        End Function

        '/**
        ' * This will get the name of the destination.
        ' *
        ' * @return The name of the destination.
        ' */
        Public Function getNamedDestination() As String
            Dim retval As String = ""
            If (TypeOf (namedDestination) Is COSString) Then
                retval = DirectCast(namedDestination, COSString).getString()
            ElseIf (TypeOf (namedDestination) Is COSName) Then
                retval = DirectCast(namedDestination, COSName).getName()
            End If
            Return retval
        End Function

        '/**
        ' * Set the named destination.
        ' *
        ' * @param dest The new named destination.
        ' *
        ' * @throws IOException If there is an error setting the named destination.
        ' */
        Public Sub setNamedDestination(ByVal dest As String)  'throws IOException
            If (TypeOf (namedDestination) Is COSString) Then
                Dim [string] As COSString = namedDestination
                [string].reset()
                [string].append(Sistema.Strings.GetBytes(dest, "ISO-8859-1"))
            ElseIf (dest Is Nothing) Then
                namedDestination = Nothing
            Else
                namedDestination = New COSString(dest)
            End If
        End Sub

    End Class

End Namespace
