Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.layers
Imports DMD.org.dmdpdf.files

Imports System

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to parse existing layers.</summary>
    '*/
    Public Class LayerParsingSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As New File(filePath)
                Dim document As Document = file.Document
                ' 2. Get the layer definition!
                Dim layerDefinition As LayerDefinition = document.Layer
                If (Not layerDefinition.Exists()) Then
                    Me.OutputLine(vbLf & "No layer definition available.")
                Else
                    Me.OutputLine(vbLf & "Iterating through the layers..." & vbLf)

                    ' 3. Parse the layer hierarchy!
                    Parse(layerDefinition.Layers, 0)
                End If
            End Using
        End Sub

        Private Sub Parse(ByVal layers As Layers, ByVal level As Integer)
            Dim indentation As String = GetIndentation(level)
            For Each layer As ILayerNode In layers
                Me.OutputLine(indentation & layer.Title)
                Parse(layer.Layers, level + 1)
            Next
        End Sub

    End Class

End Namespace