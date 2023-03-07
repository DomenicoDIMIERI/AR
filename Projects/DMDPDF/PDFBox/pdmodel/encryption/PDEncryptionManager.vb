Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO
Imports FinSeA

Namespace org.apache.pdfbox.pdmodel.encryption

    '/**
    ' * This class will handle loading of the different security handlers.
    ' *
    ' * See PDF Reference 1.4 section "3.5 Encryption"
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.7 $
    ' * @deprecated Made deprecated by the new security layer of PDFBox. Use SecurityHandlers instead.
    ' */

    Public NotInheritable Class PDEncryptionManager

        Private Shared handlerMap As Map = Collections.synchronizedMap(New HashMap())

        Shared Sub New()
            registerSecurityHandler(PDStandardEncryption.FILTER_NAME, GetType(PDStandardEncryption))
        End Sub

        Private Sub New()
        End Sub

        '/**
        ' * This will allow the user to register new security handlers when unencrypting a
        ' * document.
        ' *
        ' * @param filterName As described in the encryption dictionary.
        ' * @param handlerClass A subclass of PDEncryptionDictionary that has a constructor that takes
        ' *        a COSDictionary.
        ' */
        Public Shared Sub registerSecurityHandler(ByVal filterName As String, ByVal handlerClass As System.Type)
            handlerMap.put(COSName.getPDFName(filterName), handlerClass)
        End Sub

        '/**
        ' * This will get the correct security handler for the encryption dictionary.
        ' *
        ' * @param dictionary The encryption dictionary.
        ' *
        ' * @return An implementation of PDEncryptionDictionary(PDStandardEncryption for most cases).
        ' *
        ' * @throws IOException If a security handler could not be found.
        ' */
        Public Shared Function getEncryptionDictionary(ByVal dictionary As COSDictionary) As PDEncryptionDictionary  'throws IOException
            Dim retval As Object = Nothing
            If (dictionary IsNot Nothing) Then
                Dim filter As COSName = dictionary.getDictionaryObject(COSName.FILTER)
                Dim handlerClass As System.Type = handlerMap.get(filter)
                If (handlerClass Is Nothing) Then
                    Throw New IOException("No handler for security handler '" & filter.getName() & "'")
                Else
                    Try
                        'Constructor ctor = handlerClass.getConstructor( new Class[] { COSDictionary.class() } );
                        'retval = ctor.newInstance( new Object[] { dictionary() } );
                        retval = Activator.CreateInstance(handlerClass, {dictionary})
                        'Catch e As NoSuchMethodException
                        '    Throw New IOException(e.Message)
                        'Catch e As InstantiationException
                        '    Throw New IOException(e.getMessage())
                        'Catch e As IllegalAccessException
                        '    Throw New IOException(e.getMessage())
                        'Catch e As InvocationTargetException
                        '    Throw New IOException(e.getMessage())
                    Catch e As Exception
                        Throw New IOException(e.Message, e)
                    End Try
                End If
            End If
            Return retval

        End Function

    End Class

End Namespace
