Imports System.IO
Imports System.Text

Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure


    '/**
    ' * A default attribute object.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDDefaultAttributeObject
        Inherits PDAttributeObject

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Creates a default attribute object with a given dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Gets the attribute names.
        ' * 
        ' * @return the attribute names
        ' */
        Public Function getAttributeNames() As List(Of String)
            Dim attrNames As List(Of String) = New ArrayList(Of String)
            For Each entry As FinSeA.java.Map.Entry(Of COSName, COSBase) In Me.getCOSDictionary().entrySet()
                Dim key As COSName = entry.Key
                If (Not COSName.O.equals(key)) Then
                    attrNames.add(key.getName())
                End If
            Next
            Return attrNames
        End Function

        '/**
        ' * Gets the attribute value for a given name.
        ' * 
        ' * @param attrName the given attribute name
        ' * @return the attribute value for a given name
        ' */
        Public Function getAttributeValue(ByVal attrName As String) As COSBase
            Return Me.getCOSDictionary().getDictionaryObject(attrName)
        End Function

        '/**
        ' * Gets the attribute value for a given name.
        ' * 
        ' * @param attrName the given attribute name
        ' * @param defaultValue the default value
        ' * @return the attribute value for a given name
        ' */
        Protected Function getAttributeValue(ByVal attrName As String, ByVal defaultValue As COSBase) As COSBase
            Dim value As COSBase = Me.getCOSDictionary().getDictionaryObject(attrName)
            If (value Is Nothing) Then
                Return defaultValue
            End If
            Return value
        End Function

        '/**
        ' * Sets an attribute.
        ' * 
        ' * @param attrName the attribute name
        ' * @param attrValue the attribute value
        ' */
        Public Sub setAttribute(ByVal attrName As String, ByVal attrValue As COSBase)
            Dim old As COSBase = Me.getAttributeValue(attrName)
            Me.getCOSDictionary().setItem(COSName.getPDFName(attrName), attrValue)
            Me.potentiallyNotifyChanged(old, attrValue)
        End Sub

        Public Overrides Function toString() As String
            Dim sb As StringBuilder = New StringBuilder().append(MyBase.toString()).append(", attributes={")
            Dim i As Integer = 0
            'Iterator(Of String) it = Me.getAttributeNames().iterator();
            For Each name As String In Me.getAttributeNames '   While (it.hasNext())
                If (i > 0) Then sb.Append(", ")
                i += 1
                'String name = it.next();
                sb.Append(name).Append("=").Append(Me.getAttributeValue(name))
                'If (it.hasNext()) Then
                '{
                'sb.append(", ");
            Next '}
            '}
            Return sb.append("}").toString()
        End Function

    End Class

End Namespace
