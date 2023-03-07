Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.interaction
Imports DMD.org.dmdpdf.documents.interchange.metadata
Imports DMD.org.dmdpdf.documents.interaction.viewer
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Collections.Generic
Imports System.IO

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>Abstract sample.</summary>
    '*/
    Public MustInherit Class Sample

#Region "dynamic"
#Region "fields"

        Private _inputPath As String = My.Application.Info.DirectoryPath
        Private _outputPath As String = My.Application.Info.DirectoryPath

        Private _quit As Boolean

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary> Gets whether the sample was exited before its completion.</summary>
        '*/
        Public Function IsQuit() As Boolean
            Return _quit
        End Function

        '/**
        '  <summary> Executes the sample.</summary>
        '  <returns> Whether the sample has been completed.</returns>
        '*/
        Public MustOverride Sub Run()

#End Region

#Region "Protected"

        Protected Function GetIndentation(ByVal level As Integer) As String
            Return New String(" ", level)
        End Function

        '/**
        '  <summary> Gets the path used To serialize output files.</summary>
        '  <param name = "fileName" > Relative output file path.</param>
        '*/
        Protected Function GetOutputPath(ByVal fileName As String) As String
            If (fileName IsNot Nothing) Then
                Return System.IO.Path.Combine(_outputPath, fileName)
            Else
                Return _outputPath
            End If
        End Function

        '/**
        '  <summary> Gets the path To a sample resource.</summary>
        '  <param name = "resourceName" > Relative resource path.</param>
        '*/
        Protected Function GetResourcePath(ByVal resourceName As String) As String
            Return _inputPath & Path.DirectorySeparatorChar & resourceName
        End Function

        '/**
        '  <summary> Gets the path used To serialize output files.</summary>
        '*/
        Protected ReadOnly Property OutputPath As String
            Get
                Return GetOutputPath(Nothing)
            End Get
        End Property

        '/**
        '  <summary> Prompts a message To the user.</summary>
        '  <param name = "message" > Text To show.</param>
        '*/
        Protected Sub Prompt(ByVal message As String)
            Utils.Prompt(message)
        End Sub

        '/**
        '  <summary> Gets the user's choice from the given request.</summary>
        '  <param name = "message" > Description Of the request To show To the user.</param>
        '  <returns> User choice.</returns>
        '*/
        Protected Function PromptChoice(ByVal message As String) As String
            Try
                Return Me.ReadLine(message)
            Catch
                Return Nothing
            End Try
        End Function

        Public Function ReadLine(ByVal message As String) As String
            Return InputBox(message)
        End Function

        '/**
        '  <summary> Gets the user's choice from the given options.</summary>
        '  <param name = "options" > Available options To show To the user.</param>
        '  <returns> Chosen Option key.</returns>
        '*/
        Protected Function PromptChoice(ByVal options As IDictionary(Of String, String)) As String
            Me.OutputLine()
            For Each [option] As KeyValuePair(Of String, String) In options
                Me.OutputLine(IIf([option].Key.Equals(""), "ENTER", "[" & [option].Key & "]") & " " & [option].Value)
            Next
            Return Me.ReadLine("Please select: ")
        End Function

        Public Sub Output(ByVal text As String)
            Form1.TextBox1.Text = Form1.TextBox1.Text & text
        End Sub

        Public Sub OutputLine(Optional ByVal text As String = vbNullString)
            Form1.TextBox1.Text = Form1.TextBox1.Text & text & vbNewLine
        End Sub

        Protected Function PromptFileChoice(ByVal inputDescription As String) As String
            Dim resourcePath As String = Path.GetFullPath(_inputPath & "pdf")
            Dim ofd As New OpenFileDialog
            Dim ret As String = ""
            ofd.Title = "Select a PDF file"
            ofd.Filter = "PDF File (*.PDF)|*.pdf"
            If (ofd.ShowDialog = DialogResult.OK) Then
                ret = ofd.FileName
            End If
            ofd.Dispose()
            Return ret
        End Function

        '/**
        '  <summary> Prompts the user For advancing To the Next page.</summary>
        '  <param name = "page" >Next page.</param>
        '  <param name = "skip" > Whether the prompt has To be skipped.</param>
        '  <returns> Whether To advance.</returns>
        '*/
        Protected Function PromptNextPage(ByVal page As Page, ByVal skip As Boolean) As Boolean
            Dim pageIndex As Integer = page.Index
            If (pageIndex > 0 AndAlso Not skip) Then
                Dim options As IDictionary(Of String, String) = New Dictionary(Of String, String)()
                options("") = "Scan next page"
                options("Q") = "End scanning"
                If (Not PromptChoice(options).Equals("")) Then
                    Return False
                End If
            End If
            Me.OutputLine(vbLf & "Scanning page " & (pageIndex + 1) & "..." & vbLf)
            Return True
        End Function

        '/**
        '  <summary> Prompts the user For a page index To Select.</summary>
        '  <param name = "inputDescription" > Message prompted To the user.</param>
        '  <param name = "pageCount" > page count.</param>
        '  <returns> Selected page index.</returns>
        '*/
        Protected Function PromptPageChoice(ByVal inputDescription As String, ByVal pageCount As Integer) As Integer
            Return PromptPageChoice(inputDescription, 0, pageCount)
        End Function

        '/**
        '  <summary> Prompts the user For a page index To Select.</summary>
        '  <param name = "inputDescription" > Message prompted To the user.</param>
        '  <param name = "startIndex" > First page index, inclusive.</param>
        '  <param name = "endIndex" > Last page index, exclusive.</param>
        '  <returns> Selected page index.</returns>
        '*/
        Protected Function PromptPageChoice(ByVal inputDescription As String, ByVal startIndex As Integer, ByVal endIndex As Integer) As Integer
            Dim pageIndex As Integer
            Try
                pageIndex = Int32.Parse(PromptChoice(inputDescription & " [" & (startIndex + 1) & "-" & endIndex & "]: ")) - 1
            Catch
                pageIndex = startIndex
            End Try
            If (pageIndex < startIndex) Then
                pageIndex = startIndex
            ElseIf (pageIndex >= endIndex) Then
                pageIndex = endIndex - 1
            End If

            Return pageIndex
        End Function

        '/**
        '  <summary> Indicates that the sample was exited before its completion.</summary>
        '*/
        Protected Sub Quit()
            _quit = True
        End Sub

        '/**
        '  <summary> Serializes the given DMDPDF file Object.</summary>
        '  <param name = "file" > PDF file To serialize.</param>
        '  <returns> Serialization path.</returns>
        '*/
        Protected Function Serialize(ByVal file As DMD.org.dmdpdf.files.File) As String
            Return Serialize(file, Nothing, Nothing, Nothing)
        End Function

        '/**
        '  <summary> Serializes the given DMDPDF file Object.</summary>
        '  <param name = "file" > PDF file To serialize.</param>
        '  <param name = "serializationMode" > Serialization mode.</param>
        '  <returns> Serialization path.</returns>
        '*/
        Protected Function Serialize(ByVal file As DMD.org.dmdpdf.files.File, ByVal serializationMode As SerializationModeEnum?) As String
            Return Serialize(file, serializationMode, Nothing, Nothing, Nothing)
        End Function

        '/**
        '  <summary> Serializes the given DMDPDF file Object.</summary>
        '  <param name = "file" > PDF file To serialize.</param>
        '  <param name = "fileName" > Output file name.</param>
        '  <returns> Serialization path.</returns>
        '*/
        Protected Function Serialize(ByVal file As DMD.org.dmdpdf.files.File, ByVal fileName As String) As String
            Return Serialize(file, fileName, Nothing, Nothing)
        End Function

        '/**
        '  <summary> Serializes the given DMDPDF file Object.</summary>
        '  <param name = "file" > PDF file To serialize.</param>
        '  <param name = "fileName" > Output file name.</param>
        '  <param name = "serializationMode" > Serialization mode.</param>
        '  <returns> Serialization path.</returns>
        '*/
        Protected Function Serialize(ByVal file As DMD.org.dmdpdf.files.File, ByVal fileName As String, ByVal serializationMode As SerializationModeEnum?) As String
            Return Serialize(file, fileName, serializationMode, Nothing, Nothing, Nothing)
        End Function

        '/**
        '  <summary> Serializes the given DMDPDF file Object.</summary>
        '  <param name = "file" > PDF file To serialize.</param>
        '  <param name = "title" > document title.</param>
        '  <param name = "subject" > document subject.</param>
        '  <param name = "keywords" > document keywords.</param>
        '  <returns> Serialization path.</returns>
        '*/
        Protected Function Serialize(ByVal file As DMD.org.dmdpdf.files.File, ByVal title As String, ByVal subject As String, ByVal keywords As String) As String
            Return Serialize(file, Nothing, title, subject, keywords)
        End Function

        '/**
        '  <summary> Serializes the given DMDPDF file Object.</summary>
        '  <param name = "file" > PDF file To serialize.</param>
        '  <param name = "serializationMode" > Serialization mode.</param>
        '  <param name = "title" > Document title.</param>
        '  <param name = "subject" > Document subject.</param>
        '  <param name = "keywords" > Document keywords.</param>
        '  <returns> Serialization path.</returns>
        '*/
        Protected Function Serialize(
                              ByVal file As DMD.org.dmdpdf.files.File,
                              ByVal serializationMode As SerializationModeEnum?,
                              ByVal title As String,
                              ByVal subject As String,
                              ByVal Keywords As String
                              ) As String
            Return Serialize(file, Me.GetType().Name, serializationMode, title, subject, Keywords)
        End Function

        '/**
        '  <summary> Serializes the given DMDPDF file Object.</summary>
        '  <param name = "file" > PDF file To serialize.</param>
        '  <param name = "fileName" > Output file name.</param>
        '  <param name = "serializationMode" > Serialization mode.</param>
        '  <param name = "title" > Document title.</param>
        '  <param name = "subject" > Document subject.</param>
        '  <param name = "keywords" > Document keywords.</param>
        '  <returns> Serialization path.</returns>
        '*/
        Protected Function Serialize(
                          ByVal file As DMD.org.dmdpdf.files.File,
                          ByVal fileName As String,
                          ByVal serializationMode As SerializationModeEnum?,
                          ByVal title As String,
                          ByVal subject As String,
                          ByVal keywords As String
                          ) As String
            ApplyDocumentSettings(file.Document, title, subject, keywords)

            Me.OutputLine()

            If (Not serializationMode.HasValue) Then
                If (file.Reader Is Nothing) Then ' New File.
                    serializationMode = SerializationModeEnum.Standard
                Else ' Existing file.
                    Me.OutputLine("[0] Standard serialization")
                    Me.OutputLine("[1] Incremental update")
                    Try
                        serializationMode = CType(Int32.Parse(Me.ReadLine("Please select a serialization mode: ")), SerializationModeEnum)
                    Catch
                        serializationMode = SerializationModeEnum.Standard
                    End Try
                End If
            End If

            Dim outputFilePath As String = System.IO.Path.Combine(_outputPath, fileName & "." & serializationMode & ".pdf")

            '// Save the file!
            '/*
            '  NOTE: You can also save To a generic target stream (see Save() method Overloads).
            '*/
#If Not DEBUG Then
            Try
#End If
            file.Save(outputFilePath, serializationMode.Value)
#If Not DEBUG Then
            Catch e As Exception
                Me.OutputLine("File writing failed: " & e.Message)
                Me.OutputLine(e.StackTrace)
            End Try
#End If
            Me.OutputLine(vbLf & "Output: " + outputFilePath)

            Return outputFilePath
        End Function
#End Region

#Region "internal"

        Friend Sub Initialize(ByVal inputPath As String, ByVal outputPath As String)
            Me._inputPath = inputPath
            Me._outputPath = outputPath
        End Sub

#End Region

#Region "Private"

        Private Sub ApplyDocumentSettings(ByVal document As Document, ByVal title As String, ByVal subject As String, ByVal keywords As String)
            If (title Is Nothing) Then Return

            ' Viewer preferences.
            Dim view As ViewerPreferences = New ViewerPreferences(document) ' Instantiates viewer preferences inside the document context.
            document.ViewerPreferences = view ' Assigns the viewer preferences object to the viewer preferences function.
            view.DisplayDocTitle = True

            ' Document metadata.
            Dim info As Information = document.Information
            info.Clear()
            info.Author = "Domenico DI MIERI"
            info.CreationDate = DateTime.Now
            info.Creator = Me.GetType().FullName
            info.Title = "DMDPDF - " & title & " sample"
            info.Subject = "Sample about " + subject & " using DMDPDF"
            info.Keywords = keywords
        End Sub

#End Region
#End Region
#End Region

    End Class
End Namespace