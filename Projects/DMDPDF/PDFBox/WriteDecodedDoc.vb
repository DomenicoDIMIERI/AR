Imports System.IO
Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.exceptions

Namespace org.apache.pdfbox

    '/**
    ' * load document and write with all streams decoded.
    ' *
    ' * @author Michael Traut
    ' * @version $Revision: 1.8 $
    ' */
    Public Class WriteDecodedDoc

        Private Const PASSWORD = "-password"
        Private Const NONSEQ = "-nonSeq"

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * This will perform the document reading, decoding and writing.
        ' *
        ' * @param in The filename used for input.
        ' * @param out The filename used for output.
        ' *
        ' * @throws IOException If there is an error parsing the document.
        ' * @throws COSVisitorException If there is an error while copying the document.
        ' * 
        ' * @deprecated use {@link WriteDecodedDoc#doIt(String, String, String)} instead.
        ' */
        Public Sub doIt(ByVal [in] As String, ByVal out As String) 'throws IOException, COSVisitorException
            doIt([in], out, "", False)
        End Sub

        '/**
        ' * This will perform the document reading, decoding and writing.
        ' *
        ' * @param in The filename used for input.
        ' * @param out The filename used for output.
        ' * @param password The password to open the document.
        ' * @param useNonSeqParser use the non sequential parser
        ' *
        ' * @throws IOException If there is an error parsing the document.
        ' * @throws COSVisitorException If there is an error while copying the document.
        ' */
        Public Sub doIt(ByVal [in] As String, ByVal out As String, ByVal password As String, ByVal useNonSeqParser As Boolean)
            Dim doc As PDDocument = Nothing
            Try
                If (useNonSeqParser) Then
                    doc = PDDocument.loadNonSeq(New FileInputStream([in]), Nothing, password)
                    doc.setAllSecurityToBeRemoved(True)
                Else
                    doc = PDDocument.load([in])
                    If (doc.isEncrypted()) Then
                        Try
                            doc.decrypt(password)
                            doc.setAllSecurityToBeRemoved(True)
                        Catch e As InvalidPasswordException
                            If (password.Trim().Length() = 0) Then
                                LOG.error("Password needed!!")
                            Else
                                LOG.error("Wrong password!!")
                            End If
                            Return
                        Catch e As CryptographyException
                            LOG.error(e.Message, e)
                            Return
                        End Try
                    End If
                End If
                For Each i As COSObject In doc.getDocument().getObjects()
                    Dim base As COSBase = i.getObject()
                    If (TypeOf (base) Is COSStream) Then
                        ' just kill the filters
                        Dim cosStream As COSStream = base
                        cosStream.getUnfilteredStream()
                        cosStream.setFilters(Nothing)
                    End If
                Next
                doc.save(out)
            Finally
                If (doc IsNot Nothing) Then
                    doc.close()
                End If
            End Try
        End Sub

        '/**
        ' * This will write a PDF document with completely decoded streams.
        ' * <br />
        ' * see usage() for commandline
        ' *
        ' * @param args command line arguments
        ' */
        Public Shared Sub main(ByVal args() As String)
            Dim app As WriteDecodedDoc = New WriteDecodedDoc()
            Dim password As String = ""
            Dim useNonSeqParser As Boolean = False
            Dim pdfFile As String = ""
            Dim outputFile As String = ""
            For i As Integer = 0 To args.Length - 1
                If (args(i).Equals(password)) Then
                    i += 1
                    If (i >= args.Length) Then
                        usage()
                    End If
                    password = args(i)
                Else
                    If (args(i).Equals(NONSEQ)) Then
                        useNonSeqParser = True
                    Else
                        If (pdfFile Is Nothing) Then
                            pdfFile = args(i)
                        Else
                            outputFile = args(i)
                        End If
                    End If
                End If
            Next

            If (pdfFile Is Nothing) Then
                usage()
            Else
                Try
                    If (outputFile Is Nothing) Then
                        outputFile = calculateOutputFilename(pdfFile)
                    End If
                    app.doIt(pdfFile, outputFile, password, useNonSeqParser)
                Catch e As Exception
                    LOG.error(e.Message, e)
                End Try
            End If
        End Sub

        Private Shared Function calculateOutputFilename(ByVal filename As String) As String
            Dim outputFilename As String
            If (filename.ToLower().EndsWith(".pdf")) Then
                outputFilename = filename.Substring(0, filename.Length() - 4)
            Else
                outputFilename = filename
            End If
            outputFilename &= "_unc.pdf"
            Return outputFilename
        End Function

        '/**
        ' * This will print out a message telling how to use this example.
        ' */
        Private Shared Sub usage()
            LOG.error(
                    "usage: java -jar pdfbox-app-x.y.z.jar WriteDecodedDoc [OPTIONS] <input-file> [output-file]" & vbCrLf & _
                    "  -password <password>      Password to decrypt the document" & vbCrLf & _
                    "  -nonSeq                   Enables the new non-sequential parser" & vbCrLf & _
                    "  <input-file>              The PDF document to be decompressed" & vbCrLf & _
                    "  [output-file]             The filename for the decompressed pdf" & vbCrLf _
                    )
        End Sub

    End Class

End Namespace
