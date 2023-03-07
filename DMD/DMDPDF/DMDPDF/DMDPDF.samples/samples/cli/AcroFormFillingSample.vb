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
    '  <summary>This sample demonstrates how to fill AcroForm fields of a PDF document.</summary>
    '*/
    Public Class AcroFormFillingSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. Opening the PDF file...
            Dim filePath As String = PromptFileChoice("Please select a PDF file")
            Dim file As File = New File(filePath)
            Dim document As Document = file.Document

            ' 2. Get the acroform!
            Dim form As Form = document.Form
            If (Not form.Exists()) Then
                Me.OutputLine(vbLf & "No acroform available.")
            Else
                ' 3. Filling the acroform fields...
                Dim mode As Integer
                Try
                    Dim options As IDictionary(Of String, String) = New Dictionary(Of String, String)()
                    options("0") = "Automatic filling"
                    options("1") = "Manual filling"
                    mode = Int32.Parse(PromptChoice(options))

                Catch
                    mode = 0
                End Try
                Select Case (mode)
                    Case 0 ' Automatic filling.
                        Me.OutputLine(vbLf & "Acroform is being filled with random values..." & vbLf)

                        For Each field As Field In form.Fields.Values
                            Dim value As String
                            If (TypeOf (field) Is RadioButton) Then
                                value = CType(field.Widgets(0), DualWidget).WidgetName ' Selects the first widget in the group.
                            ElseIf (TypeOf (field) Is ChoiceField) Then
                                value = CType(field, ChoiceField).Items(0).Value ' Selects the first item in the list.
                            Else
                                value = field.Name ' Arbitrary value (just to get something to fill with).
                            End If
                            field.Value = value
                        Next
                'break;
                    Case 1 ' Manual filling.
                        Me.OutputLine(vbLf & "Please insert a value for each field listed below (or type 'quit' to end this sample)." & vbLf)
                        For Each field As Field In form.Fields.Values
                            Me.OutputLine("* " & field.GetType().Name & " '" & field.FullName & "' (" & field.BaseObject.ToString & "): ")
                            Me.OutputLine("    Current Value:" & field.Value)
                            Dim newValue As String = PromptChoice("    New Value:")
                            If (newValue IsNot Nothing AndAlso newValue.Equals("quit")) Then
                                Exit For
                            End If
                            field.Value = newValue
                        Next
                        'break;
                End Select
            End If

            '4. Serialize the PDF file!
            Serialize(file)
        End Sub

    End Class

End Namespace