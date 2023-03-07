Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.documents.interaction.forms
Imports DMD.org.dmdpdf.documents.interaction.forms.styles
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to insert AcroForm fields into a PDF document.</summary>
    '*/
    Public Class AcroFormCreationSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. PDF file instantiation.
            Dim file As File = New File()
            Dim document As Document = file.Document

            ' 2. Content creation.
            Populate(document)

            ' 3. Serialize the PDF file!
            Serialize(file, "AcroForm", "inserting AcroForm fields", "Acroform, creation, annotations, actions, javascript, button, combo, textbox, radio button")
        End Sub

        Private Sub Populate(ByVal document As Document)
            '/*
            '  NOTE: In order to insert a field into a document, you have to follow these steps:
            '  1. Define the form fields collection that will gather your fields (NOTE: the form field collection is global to the document);
            '  2. Define the pages where to place the fields;
            '  3. Define the appearance style to render your fields;
            '  4. Create each field of yours:
            '    4.1. instantiate your field into the page;
            '    4.2. apply the appearance style to your field;
            '    4.3. insert your field into the fields collection.
            '*/

            '// 1. Define the form fields collection!
            Dim form As Form = document.Form
            Dim fields As Fields = form.Fields

            ' 2. Define the page where to place the fields!
            Dim page As Page = New Page(document)
            document.Pages.Add(page)

            ' 3. Define the appearance style to apply to the fields!
            Dim fieldStyle As DefaultStyle = New DefaultStyle()
            fieldStyle.FontSize = 12
            fieldStyle.GraphicsVisibile = True

            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)
            composer.SetFont(
                            New StandardType1Font(
                              document,
                              StandardType1Font.FamilyEnum.Courier,
                              True,
                              False
                              ),
                            14
                            )

            ' 4. Field creation.
            ' 4.a. Push button.
            '{
            composer.ShowText(
                            "PushButton:",
                          New PointF(140, 68),
                          XAlignmentEnum.Right,
                          YAlignmentEnum.Middle,
                          0
                          )

            Dim fieldWidget As Widget = New Widget(
                                              page,
                                              New RectangleF(150, 50, 136, 36)
                                              )
            Dim fieldWidgetActions As WidgetActions = New WidgetActions(fieldWidget)
            fieldWidget.Actions = fieldWidgetActions
            fieldWidgetActions.OnActivate = New JavaScript(
                          document,
                          "app.alert(""Radio button currently selected:  '"" + this.getField(""myRadio"").value + ""'."",3,0,""Activation event"");"
                          )
            Dim field1 As PushButton = New PushButton(
                                          "okButton",
                                          fieldWidget,
                                          "Push"
                                          ) '// Current value. ; // 4.1. Field instantiation.
            fields.Add(field1) ' 4.2. Field insertion into the fields collection.
            fieldStyle.Apply(field1) ' 4.3. Appearance style applied.

            '{
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            blockComposer.Begin(New RectangleF(296, 50, page.Size.Width - 336, 36), XAlignmentEnum.Left, YAlignmentEnum.Middle)
            If (composer.State.Font Is Nothing) Then
                Debug.Print("oops 4")
            End If
            composer.SetFont(composer.State.Font, 7)
            blockComposer.ShowText("If you click this push button, a javascript action should prompt you an alert box responding to the activation event triggered by your PDF viewer.")
            blockComposer.End()
            blockComposer = Nothing
            '}
            '}

            ' 4.b. Check box.
            '{
            composer.ShowText(
                          "CheckBox:",
                          New PointF(140, 118),
                          XAlignmentEnum.Right,
                          YAlignmentEnum.Middle,
                          0
                          )
            Dim field2 As CheckBox = New CheckBox(
                          "myCheck",
                          New Widget(
                            page,
                            New RectangleF(150, 100, 36, 36)
                            ),
                          True
                          ) ' // Current value. // 4.1. Field instantiation.
            fieldStyle.Apply(field2)
            fields.Add(field2)
            field2 = New CheckBox(
                      "myCheck2",
                      New Widget(
                        page,
                        New RectangleF(200, 100, 36, 36)
                        ),
                      True
                      ) ' // Current value.; // 4.1. Field instantiation.
            fieldStyle.Apply(field2)
            fields.Add(field2)
            field2 = New CheckBox(
                          "myCheck3",
                          New Widget(
                            page,
                            New RectangleF(250, 100, 36, 36)
                            ),
                          False
                          ) ' // Current value.; // 4.1. Field instantiation.
            fields.Add(field2) ' 4.2. Field insertion into the fields collection.
            fieldStyle.Apply(field2) ' 4.3. Appearance style applied.
            '}

            '// 4.c. Radio button.
            '{
            composer.ShowText(
                              "RadioButton:",
                              New PointF(140, 168),
                              XAlignmentEnum.Right,
                              YAlignmentEnum.Middle,
                              0
                              )

            'Note:       A radio button field typically combines multiple alternative widgets.
            Dim field3 As RadioButton = New RadioButton(
                                  "myRadio",
                                  New DualWidget() {
                                    New DualWidget(page, New RectangleF(150, 150, 36, 36), "first"),
                                    New DualWidget(page, New RectangleF(200, 150, 36, 36), "second"),
                                    New DualWidget(page, New RectangleF(250, 150, 36, 36), "third")
                                  },
                                  "second"
                                  ) '// Selected item (it MUST correspond to one of the available widgets' names). // 4.1. Field instantiation.
            fields.Add(field3) ' 4.2. Field insertion into the fields collection.
            fieldStyle.Apply(field3) ' 4.3. Appearance style applied.
            '}

            ' 4.d. Text field.
            '{
            composer.ShowText(
                          "TextField:",
                          New PointF(140, 218),
                          XAlignmentEnum.Right,
                          YAlignmentEnum.Middle,
                          0
                          )

            Dim field4 As TextField = New TextField(
                                  "myText",
                                  New Widget(
                                    page,
                                    New RectangleF(150, 200, 200, 36)
                                    ),
                                  "Carmen Consoli"
                                  ) ' // Current value. // 4.1. Field instantiation.
            field4.SpellChecked = False ' Avoids text spell check.
            Dim fieldActions As FieldActions = New FieldActions(document)
            field4.Actions = fieldActions
            fieldActions.OnValidate = New JavaScript(
                              document,
                              "app.alert(""Text '"" + this.getField(""myText"").value + ""' has changed!"",3,0,""Validation event"");"
                              )
            fields.Add(field4) ' 4.2. Field insertion into the fields collection.
            fieldStyle.Apply(field4) '/ 4.3. Appearance style applied.

            '{
            blockComposer = New BlockComposer(composer)
            blockComposer.Begin(New RectangleF(360, 200, page.Size.Width - 400, 36), XAlignmentEnum.Left, YAlignmentEnum.Middle)
            If (composer.State.Font Is Nothing) Then
                Debug.Print("oops 5")
            End If
            composer.SetFont(composer.State.Font, 7)
            blockComposer.ShowText("If you leave this text field after changing its content, a javascript action should prompt you an alert box responding to the validation event triggered by your PDF viewer.")
            blockComposer.End()
            '}
            '}

            ' 4.e. Choice fields.
            '{
            ' Preparing the item list that we'll use for choice fields (a list box and a combo box (see below))...
            Dim items As ChoiceItems = New ChoiceItems(document)
            items.Add("Tori Amos")
            items.Add("Anouk")
            items.Add("Joan Baez")
            items.Add("Rachele Bastreghi")
            items.Add("Anna Calvi")
            items.Add("Tracy Chapman")
            items.Add("Carmen Consoli")
            items.Add("Ani DiFranco")
            items.Add("Cristina Dona'")
            items.Add("Nathalie Giannitrapani")
            items.Add("PJ Harvey")
            items.Add("Billie Holiday")
            items.Add("Joan As Police Woman")
            items.Add("Joan Jett")
            items.Add("Janis Joplin")
            items.Add("Angelique Kidjo")
            items.Add("Patrizia Laquidara")
            items.Add("Annie Lennox")
            items.Add("Loreena McKennitt")
            items.Add("Joni Mitchell")
            items.Add("Alanis Morissette")
            items.Add("Yael Naim")
            items.Add("Noa")
            items.Add("Sinead O'Connor")
            items.Add("Dolores O'Riordan")
            items.Add("Nina Persson")
            items.Add("Brisa Roche'")
            items.Add("Roberta Sammarelli")
            items.Add("Cristina Scabbia")
            items.Add("Nina Simone")
            items.Add("Skin")
            items.Add("Patti Smith")
            items.Add("Fatima Spar")
            items.Add("Thony (F.V.Caiozzo)")
            items.Add("Paola Turci")
            items.Add("Sarah Vaughan")
            items.Add("Nina Zilli")

            '// 4.e1. List box.
            '{
            composer.ShowText(
                            "ListBox:",
                            New PointF(140, 268),
                            XAlignmentEnum.Right,
                            YAlignmentEnum.Middle,
                            0
                            )
            Dim field5 As ListBox = New ListBox(
                                    "myList",
                                    New Widget(
                                      page,
                                      New RectangleF(150, 250, 200, 70)
                                      )
                                    ) ' 4.1. Field instantiation.
            field5.Items = items ' List items assignment.
            field5.MultiSelect = False ' Multiple items may Not be selected simultaneously.
            field5.Value = "Carmen Consoli" ' Selected item.
            fields.Add(field5) ' 4.2. Field insertion into the fields collection.
            fieldStyle.Apply(field5) ' 4.3. Appearance style applied.
            '}

            '// 4.e2. Combo box.
            '{
            composer.ShowText(
                                "ComboBox:",
                                New PointF(140, 350),
                                XAlignmentEnum.Right,
                                YAlignmentEnum.Middle,
                                0
                                )
            Dim field6 As ComboBox = New ComboBox(
                                "myCombo",
                                New Widget(
                                  page,
                                  New RectangleF(150, 334, 200, 36)
                                  )
                                ) ' 4.1. Field instantiation.
            field6.Items = items '// Combo items assignment.
            field6.Editable = True ' // Text may be edited.
            field6.SpellChecked = False ' // Avoids text spell check.
            field6.Value = "Carmen Consoli" ' // Selected item.
            fields.Add(field6) '; // 4.2. Field insertion into the fields collection.
            fieldStyle.Apply(field6) ' // 4.3. Appearance style applied.
            '}
            '}

            composer.Flush()
        End Sub

    End Class
End Namespace