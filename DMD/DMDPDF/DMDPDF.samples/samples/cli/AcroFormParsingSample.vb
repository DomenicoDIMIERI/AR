Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.documents.interaction.forms
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to inspect the AcroForm fields of a PDF document.</summary>
    '*/
    Public Class AcroFormParsingSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Using file As File = New File(filePath)
                Dim document As Document = file.Document

                ' 2. Get the acroform!
                Dim form As Form = document.Form
                If (Not form.Exists()) Then
                    Me.OutputLine(vbLf & "No acroform available (AcroForm dictionary not found).")
                Else
                    Me.OutputLine(vbLf & "Iterating through the fields collection...")

                    ' 3. Showing the acroform fields...
                    Dim objCounters As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)()
                    For Each field As Field In form.Fields.Values
                        Me.OutputLine("* Field '" & field.FullName & "' (" & field.BaseObject.ToString & ")")

                        Dim typeName As String = field.GetType().Name
                        Me.OutputLine("    Type: " & typeName)
                        Me.OutputLine("    Value: " & field.Value)
                        Me.OutputLine("    Data: " & field.BaseDataObject.ToString())

                        Dim widgetIndex As Integer = 0
                        For Each widget As Widget In field.Widgets
                            Me.OutputLine("    Widget " & (++widgetIndex) & ":")
                            Dim widgetPage As Page = widget.Page
                            If (widgetPage Is Nothing) Then
                                Me.OutputLine("      Page: undefined")
                            Else
                                Me.OutputLine("      Page: " & (widgetPage.Index + 1) & " (" & widgetPage.BaseObject.ToString & ")")
                            End If


                            Dim widgetBox As RectangleF = widget.Box
                            Me.OutputLine("      Coordinates: {x:" & Math.Round(widgetBox.X) & "; y:" & Math.Round(widgetBox.Y) & "; width:" & Math.Round(widgetBox.Width) & "; height:" & Math.Round(widgetBox.Height) & "}")
                        Next

                        If (objCounters.ContainsKey(typeName)) Then
                            objCounters(typeName) = objCounters(typeName) + 1
                        Else
                            objCounters(typeName) = 1
                        End If
                    Next

                    Dim fieldCount As Integer = form.Fields.Count
                    If (fieldCount = 0) Then
                        Me.OutputLine("No field available.")
                    Else
                        Me.OutputLine(vbNewLine & "Fields partial counts (grouped by type):")
                        For Each entry As KeyValuePair(Of String, Integer) In objCounters
                            Me.OutputLine(" " & entry.Key & ": " & entry.Value)
                        Next
                        Me.OutputLine("Fields total count: " & fieldCount)
                    End If
                End If
            End Using
        End Sub

    End Class

End Namespace