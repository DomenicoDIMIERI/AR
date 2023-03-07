Imports System.Drawing

Namespace PDF

   
    Public Class PDFReader

        Private m_PDFVersion As String
        Private m_Stream As System.IO.Stream
        Private m_Document As PDFDocument
        Private m_Ckunks As New CCollection(Of PDFChunk)

        Private Class PDFChunk
            Public Offset As Integer
            Public Number As Integer
            Public Generation As Integer
            Public ObjectString As String
            Public StreamContent As String
            Public Fields As New CKeyCollection(Of String)

            Public Sub ParseFields()
                Me.Fields.Clear()
                Dim status As Integer = 0
                Dim i As Integer
                Dim name As String = ""
                Dim value As String = ""
                Dim ch As String

                For i = 1 To Len(Me.ObjectString)
                    ch = Mid(Me.ObjectString, i, 1)
                    Select Case status
                        Case 0 'Cerco il nome
                            If (ch = "/") Then
                                name = ""
                                value = ""
                                status = 1
                            End If
                        Case 1 'Costruisco il nome
                            Select Case ch
                                Case "/"
                                    Me.Fields.Add(name, "")
                                    name = ""
                                    value = ""
                                    status = 1
                                Case " "
                                    status = 2
                                Case Else
                                    name &= ch
                            End Select
                        Case 2
                            Select Case ch
                                Case "/"
                                    Me.Fields.Add(name, value)
                                    status = 1
                                    name = ""
                                    value = ""
                                Case Else
                                    value &= ch
                            End Select
                    End Select
                Next
                If (name <> "") Then
                    Me.Fields.Add(name, value)
                End If
            End Sub

            Public Overrides Function ToString() As String
                Dim ret As New System.Text.StringBuilder
                ret.Append(Me.Number & " " & Me.Generation & " obj<<" & Me.ObjectString & ">>")
                If (Me.StreamContent <> "") Then
                    ret.Append("stream" & Me.StreamContent & "endstream")
                End If
                ret.Append("endobj")
                Return ret.ToString
            End Function

        End Class

        Public Sub New(ByVal stream As System.IO.Stream)
            If (stream Is Nothing) Then Throw New ArgumentNullException("stream")
            Me.m_Document = Nothing
            Me.m_Stream = stream
        End Sub

        Public ReadOnly Property Stream As System.IO.Stream
            Get
                Return Me.m_Stream
            End Get
        End Property

        Public ReadOnly Property Document As PDFDocument
            Get
                If (Me.m_Document Is Nothing) Then Me.Load()
                Return Me.m_Document
            End Get
        End Property

        Private Enum ParserStatus As Integer
            LookForPDF
            LookForPDFVersion
            BuildObjects
            LookForEndStream
            LookForEndObj
        End Enum

        Protected Sub Load()
            ' Const BUFFSIZE As Integer = 1024
            Dim byteBuffer() As Byte
            Dim parserStatus As ParserStatus = parserStatus.LookForPDF
            Dim buffPos As Integer
            Dim ch As String
            Dim subStatus As Integer
            Dim currObject As PDFChunk = Nothing
            Dim strLen As Integer
            Dim parNum As Integer
            Dim crlfCount As Integer
            Dim buffer As String
            Dim token As String
            Dim lookForCrLf As Boolean
            Dim lookForCr As Boolean
            Dim lookForLf As Boolean

            'ReDim byteBuffer(BUFFSIZE - 1)
            ReDim byteBuffer(Me.m_Stream.Length - 1)
            Me.m_Stream.Read(byteBuffer, 0, Me.m_Stream.Length)

            buffer = System.Text.Encoding.UTF7.GetString(byteBuffer)

            Me.m_Document = New PDFDocument
            token = ""
            Me.m_Ckunks.Clear()

            buffPos = 1
            lookForCrLf = False
            lookForCr = False
            While (buffPos <= Len(Buffer))
                ch = Mid(Buffer, buffPos, 1)
                buffPos += 1

                If (lookForCrLf) Then
                    token = token & ch
                    If Len(token) = 2 Then
                        If (token = vbCrLf) Then
                            lookForCrLf = False
                            token = ""
                        Else
                            Throw New FormatException("Expected \r\n")
                        End If
                    End If
                ElseIf (lookForCr) Then
                    If (ch = vbCr) Then
                        token = ""
                        lookForCr = False
                    Else
                        Throw New FormatException("Expected \r")
                    End If
                ElseIf (lookForLf) Then
                    If (ch = vbLf) Then
                        token = ""
                        lookForLf = False
                    Else
                        Throw New FormatException("Expected \n")
                    End If
                Else
                    Select Case parserStatus
                        Case PDFReader.ParserStatus.LookForPDF
                            Select Case ch
                                Case " "
                                Case Else
                                    token = token & ch
                            End Select

                            Select Case token
                                Case "%PDF-"
                                    token = ""
                                    parserStatus = PDFReader.ParserStatus.LookForPDFVersion
                            End Select
                        Case PDFReader.ParserStatus.LookForPDFVersion
                            Select Case ch
                                Case "%"
                                    Me.m_PDFVersion = Trim(token)
                                    token = ""
                                    parserStatus = PDFReader.ParserStatus.BuildObjects
                                    subStatus = 0
                                    currObject = New PDFChunk
                                Case Else
                                    token = token & ch
                            End Select
                        Case PDFReader.ParserStatus.BuildObjects
                            Select Case subStatus
                                Case 0 'Object number
                                    Select Case ch
                                        Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
                                            token = token & ch
                                        Case " "
                                            If token <> "" Then
                                                currObject = New PDFChunk
                                                Me.m_Ckunks.Add(currObject)
                                                currObject.Number = Val(token)
                                                token = ""
                                                subStatus = 1
                                            End If
                                        Case Else
                                            token = ""
                                    End Select
                                Case 1 ' Generation
                                    Select Case ch
                                        Case " "
                                            If token <> "" Then
                                                currObject.Generation = Val(token)
                                                token = ""
                                                subStatus = 2
                                            End If
                                        Case Else
                                            token = token & ch
                                    End Select
                                Case 2 'Object string <<
                                    token &= ch
                                    Select Case token
                                        Case "obj"
                                            subStatus = 7
                                            lookForCr = True
                                            token = ""
                                    End Select
                                Case 7
                                    token = token & ch
                                    Select Case token
                                        Case "<<"
                                            token = ""
                                            subStatus = 3
                                    End Select
                                Case 3 ' Object string >>
                                    Select Case ch
                                        Case ">"
                                            If (parNum > 0) Then
                                                currObject.ObjectString &= ch
                                            End If
                                            token = token & ch
                                        Case "<"
                                            token = token & ch
                                            currObject.ObjectString &= ch
                                        Case Else
                                            currObject.ObjectString &= ch
                                            token = ""
                                    End Select

                                    Select Case token
                                        Case "<<"
                                            token = ""
                                            parNum += 1
                                            ' currObject.ObjectString
                                        Case ">>"
                                            parNum -= 1
                                            If (parNum < 0) Then
                                                parNum = 0
                                                currObject.ParseFields()
                                                token = ""
                                                If (currObject.Fields.GetItemByKey("Length") <> "") Then
                                                    subStatus = 4
                                                    strLen = CInt(currObject.Fields.GetItemByKey("Length"))
                                                Else
                                                    lookForCr = True
                                                    parserStatus = PDFReader.ParserStatus.LookForEndObj
                                                End If
                                            Else
                                                ' currObject.ObjectString &= ">>"
                                            End If
                                    End Select

                                Case 4  'Cerco l'inizio dello stream
                                    token &= ch
                                    If (token = "stream") Then
                                        lookForCrLf = True
                                        subStatus = 6
                                        crlfCount = 0
                                        token = ""
                                    End If
                                Case 6 'Inizio dello stream
                                    If (Len(token) < strLen) Then
                                        token &= ch
                                    Else
                                        currObject.StreamContent = token
                                        token = ch
                                        lookForCrLf = True
                                        parserStatus = PDFReader.ParserStatus.LookForEndStream
                                        subStatus = 0
                                    End If
                                    'End If
                            End Select
                        Case PDFReader.ParserStatus.LookForEndStream
                            token = token & ch
                            If (Len(token) = Len("endstream")) Then
                                If (token) = "endstream" Then
                                    token = ""
                                    lookForCr = True
                                    parserStatus = PDFReader.ParserStatus.LookForEndObj
                                    subStatus = 0
                                Else
                                    Throw New FormatException("endstream expected")
                                End If
                            End If


                        Case PDFReader.ParserStatus.LookForEndObj
                            token = token & ch
                            If (token = "endobj") Then
                                token = ""
                                lookForCr = True
                                parserStatus = PDFReader.ParserStatus.BuildObjects
                                subStatus = 0
                            End If
                    End Select
                End If



            End While


        End Sub

    End Class

End Namespace