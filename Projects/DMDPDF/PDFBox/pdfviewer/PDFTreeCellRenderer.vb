Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdfviewer


    '/**
    ' * A class to render tree cells for the pdfviewer.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PDFTreeCellRenderer
        'Inherits  DefaultTreeCellRenderer


        Public Function getTreeCellRendererComponent(ByVal tree As System.Windows.Forms.TreeView, ByVal nodeValue As Object, ByVal isSelected As Boolean, ByVal expanded As Boolean, ByVal leaf As Boolean, ByVal row As Integer, ByVal componentHasFocus As Boolean) As System.Windows.Forms.Control
            nodeValue = convertToTreeObject(nodeValue)
            'Return MyBase.getTreeCellRendererComponent(tree, nodeValue, isSelected, expanded, leaf, row, componentHasFocus)
            Return Nothing
        End Function

        Private Function convertToTreeObject(ByVal nodeValue As Object) As Object
            If (TypeOf (nodeValue) Is MapEntry) Then
                Dim entry As MapEntry = nodeValue
                Dim key As COSName = entry.getKey()
                Dim value As COSBase = entry.getValue()
                nodeValue = key.getName() + ":" + convertToTreeObject(value)
            ElseIf (TypeOf (nodeValue) Is COSFloat) Then
                nodeValue = "" & DirectCast(nodeValue, COSFloat).floatValue()
            ElseIf (TypeOf (nodeValue) Is COSInteger) Then
                nodeValue = "" & DirectCast(nodeValue, COSInteger).intValue()
            ElseIf (TypeOf (nodeValue) Is COSString) Then
                nodeValue = DirectCast(nodeValue, COSString).getString()
            ElseIf (TypeOf (nodeValue) Is COSName) Then
                nodeValue = DirectCast(nodeValue, COSName).getName()
            ElseIf (TypeOf (nodeValue) Is ArrayEntry) Then
                Dim entry As ArrayEntry = nodeValue
                nodeValue = "[" & entry.getIndex() & "]" & convertToTreeObject(entry.getValue())
            ElseIf (TypeOf (nodeValue) Is COSNull) Then
                nodeValue = "null"
            ElseIf (TypeOf (nodeValue) Is COSDictionary) Then
                Dim dict As COSDictionary = nodeValue
                If (TypeOf (nodeValue) Is COSStream) Then
                    nodeValue = "Stream"
                Else
                    nodeValue = "Dictionary"
                End If

                Dim type As COSName = dict.getDictionaryObject(COSName.TYPE)
                If (type IsNot Nothing) Then
                    nodeValue = nodeValue & "(" & type.getName()
                    Dim subType As COSName = dict.getDictionaryObject(COSName.SUBTYPE)
                    If (subType IsNot Nothing) Then
                        nodeValue = nodeValue & ":" & subType.getName()
                    End If

                    nodeValue = nodeValue + ")"
                End If
            ElseIf (TypeOf (nodeValue) Is COSArray) Then
                nodeValue = "Array"
            ElseIf (TypeOf (nodeValue) Is COSString) Then
                nodeValue = DirectCast(nodeValue, COSString).getString()
            End If
            Return nodeValue

        End Function

    End Class

End Namespace
