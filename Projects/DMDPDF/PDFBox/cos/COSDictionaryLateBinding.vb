Imports FinSeA.org.apache.pdfbox.pdfparser

Namespace org.apache.pdfbox.cos

    '   /**
    '*
    '* @author adam
    '*/
    Public Class COSDictionaryLateBinding
        Inherits COSDictionary

        'public shared ReadOnly  Log log = LogFactory.getLog(COSDictionaryLateBinding.class);
        Dim parser As ConformingPDFParser

        Public Sub New(ByVal parser As ConformingPDFParser)
            MyBase.New()
            Me.parser = parser
        End Sub

        '/**
        ' * This will get an object from this dictionary.  If the object is a reference then it will
        ' * dereference it and get it from the document.  If the object is COSNull then
        ' * null will be returned.
        ' * @param key The key to the object that we are getting.
        ' * @return The object that matches the key.
        ' */
        Public Overrides Function getDictionaryObject(ByVal key As COSName) As COSBase
            Dim retval As COSBase = items.get(key)
            If (TypeOf (retval) Is COSObject) Then
                Dim objectNumber As Integer = CType(retval, COSObject).getObjectNumber().intValue()
                Dim generation As Integer = CType(retval, COSObject).getGenerationNumber().intValue()
                Try
                    retval = parser.getObject(objectNumber, generation)
                Catch e As Exception
                    Debug.Print("Unable to read information for object " & objectNumber)
                End Try
            End If
            If (TypeOf (retval) Is COSNull) Then
                retval = Nothing
            End If
            Return retval
        End Function

    End Class

End Namespace