'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.forms.styles

    '/**
    '  <summary>Default field appearance style.</summary>
    '*/
    Public NotInheritable Class DefaultStyle
        Inherits FieldStyle

#Region "dynamic"
#Region "constructors"

        Public Sub New()
            Me.BackColor = New DeviceRGBColor(0.9, 0.9, 0.9)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Sub Apply(ByVal field As Field)
            If (TypeOf (field) Is PushButton) Then
                Apply(CType(field, PushButton))
            ElseIf (TypeOf (field) Is CheckBox) Then
                Apply(CType(field, CheckBox))
            ElseIf (TypeOf (field) Is TextField) Then
                Apply(CType(field, TextField))
            ElseIf (TypeOf (field) Is ComboBox) Then
                Apply(CType(field, ComboBox))
            ElseIf (TypeOf (field) Is ListBox) Then
                Apply(CType(field, ListBox))
            ElseIf (TypeOf (field) Is RadioButton) Then
                Apply(CType(field, RadioButton))
            Else
                Debug.Print("Field Type Unknown: " & field.GetType.FullName)
            End If
        End Sub

        Private Overloads Sub Apply(ByVal field As CheckBox)
            Dim document As Document = field.Document
            Dim composer As PrimitiveComposer
            For Each widget As Widget In field.Widgets
                Dim widgetDataObject As PdfDictionary = widget.BaseDataObject
                widgetDataObject(PdfName.DA) = New PdfString("/ZaDb 0 Tf 0 0 0 rg")
                widgetDataObject(PdfName.MK) = New PdfDictionary(New PdfName() {PdfName.BG, PdfName.BC, PdfName.CA}, New PdfDirectObject() {New PdfArray(New PdfDirectObject() {PdfReal.Get(0.9412), PdfReal.Get(0.9412), PdfReal.Get(0.9412)}), New PdfArray(New PdfDirectObject() {PdfInteger.Default, PdfInteger.Default, PdfInteger.Default}), New PdfString("4")})
                widgetDataObject(PdfName.BS) = New PdfDictionary(New PdfName() {PdfName.W, PdfName.S}, New PdfDirectObject() {PdfReal.Get(0.8), PdfName.S})
                widgetDataObject(PdfName.H) = PdfName.P


                Dim appearance As Appearance = widget.Appearance
                If (appearance Is Nothing) Then
                    appearance = New Appearance(document)
                    widget.Appearance = appearance
                End If

                Dim Size As SizeF = widget.Box.Size

                Dim normalAppearance As AppearanceStates = appearance.Normal
                Dim onState As FormXObject = New FormXObject(document, Size)
                normalAppearance(PdfName.Yes) = onState

                '//TODO verify!!!
                '//   appearance.getRollover().put(PdfName.Yes,onState);
                '//   appearance.getDown().put(PdfName.Yes,onState);
                '//   appearance.getRollover().put(PdfName.Off_,offState);
                '//   appearance.getDown().put(PdfName.Off_,offState);

                Dim lineWidth As Single = 1
                Dim frame As RectangleF = New RectangleF(lineWidth / 2, lineWidth / 2, Size.Width - lineWidth, Size.Height - lineWidth)

                composer = New PrimitiveComposer(onState)
                If (GraphicsVisibile) Then
                    composer.BeginLocalState()
                    composer.SetLineWidth(lineWidth)
                    composer.SetFillColor(BackColor)
                    composer.SetStrokeColor(ForeColor)
                    composer.DrawRectangle(frame, 5)
                    composer.FillStroke()
                    composer.End()
                End If

                Dim blockComposer As BlockComposer = New BlockComposer(composer)
                blockComposer.Begin(frame, XAlignmentEnum.Center, YAlignmentEnum.Middle)
                composer.SetFillColor(ForeColor)
                composer.SetFont(New StandardType1Font(
                                            document,
                                            StandardType1Font.FamilyEnum.ZapfDingbats, True, False),
                                Size.Height * 0.8
                                )
                blockComposer.ShowText(New String(New Char() {CheckSymbol}))
                blockComposer.End()

                composer.Flush()

                Dim offState As FormXObject = New FormXObject(document, Size)
                normalAppearance(PdfName.Off_) = offState
                If (GraphicsVisibile) Then
                    composer = New PrimitiveComposer(offState)
                    composer.BeginLocalState()
                    composer.SetLineWidth(lineWidth)
                    composer.SetFillColor(BackColor)
                    composer.SetStrokeColor(ForeColor)
                    composer.DrawRectangle(frame, 5)
                    composer.FillStroke()
                    composer.End()

                    composer.Flush()
                End If
            Next
        End Sub


        Private Overloads Sub Apply(ByVal field As RadioButton)
            Dim document As Document = field.Document
            Dim composer As PrimitiveComposer
            For Each widget As Widget In field.Widgets
                Dim widgetDataObject As PdfDictionary = widget.BaseDataObject
                widgetDataObject(PdfName.DA) = New PdfString("/ZaDb 0 Tf 0 0 0 rg")
                widgetDataObject(PdfName.MK) = New PdfDictionary(
                                            New PdfName() {PdfName.BG, PdfName.BC, PdfName.CA},
                                            New PdfDirectObject() {
                                                                New PdfArray(New PdfDirectObject() {PdfReal.Get(0.9412), PdfReal.Get(0.9412), PdfReal.Get(0.9412)}),
                                                                New PdfArray(New PdfDirectObject() {PdfInteger.Default, PdfInteger.Default, PdfInteger.Default}),
                                                                New PdfString("l")
                                                                    }
                                            )
                widgetDataObject(PdfName.BS) = New PdfDictionary(New PdfName() {PdfName.W, PdfName.S}, New PdfDirectObject() {PdfReal.Get(0.8), PdfName.S})
                widgetDataObject(PdfName.H) = PdfName.P

                Dim appearance As Appearance = widget.Appearance
                If (appearance Is Nothing) Then
                    appearance = New Appearance(document)
                    widget.Appearance = appearance
                End If

                Dim normalAppearance As AppearanceStates = appearance.Normal
                Dim onState As FormXObject = normalAppearance(New PdfName(CType(widget, DualWidget).WidgetName))

                '//TODO verify!!!
                '//   appearance.getRollover().put(New PdfName(...),onState);
                '//   appearance.getDown().put(New PdfName(...),onState);
                '//   appearance.getRollover().put(PdfName.Off_,offState);
                '//   appearance.getDown().put(PdfName.Off_,offState);

                Dim Size As SizeF = widget.Box.Size
                Dim lineWidth As Single = 1
                Dim frame As RectangleF = New RectangleF(lineWidth / 2, lineWidth / 2, Size.Width - lineWidth, Size.Height - lineWidth)

                composer = New PrimitiveComposer(onState)
                If (GraphicsVisibile) Then
                    composer.BeginLocalState()
                    composer.SetLineWidth(lineWidth)
                    composer.SetFillColor(BackColor)
                    composer.SetStrokeColor(ForeColor)
                    composer.DrawEllipse(frame)
                    composer.FillStroke()
                    composer.End()
                End If

                Dim blockComposer As BlockComposer = New BlockComposer(composer)
                blockComposer.Begin(frame, XAlignmentEnum.Center, YAlignmentEnum.Middle)
                composer.SetFillColor(ForeColor)
                composer.SetFont(
                                New StandardType1Font(document, StandardType1Font.FamilyEnum.ZapfDingbats, True, False),
                                Size.Height * 0.8
                            )
                blockComposer.ShowText(New String(New Char() {RadioSymbol}))
                blockComposer.End()

                composer.Flush()

                Dim offState As FormXObject = New FormXObject(document, Size)
                normalAppearance(PdfName.Off_) = offState
                If (GraphicsVisibile) Then
                    composer = New PrimitiveComposer(offState)
                    composer.BeginLocalState()
                    composer.SetLineWidth(lineWidth)
                    composer.SetFillColor(BackColor)
                    composer.SetStrokeColor(ForeColor)
                    composer.DrawEllipse(frame)
                    composer.FillStroke()
                    composer.End()

                    composer.Flush()
                End If

            Next
        End Sub

        Private Overloads Sub Apply(ByVal field As PushButton)
            Dim document As Document = field.Document
            Dim widget As Widget = field.Widgets(0)

            Dim appearance As Appearance = widget.Appearance
            If (appearance IsNot Nothing) Then
                appearance = New Appearance(document)
                widget.Appearance = appearance
            End If

            Dim normalAppearanceState As FormXObject
            Dim Size As SizeF = widget.Box.Size
            normalAppearanceState = New FormXObject(document, Size)
            Dim composer As PrimitiveComposer = New PrimitiveComposer(normalAppearanceState)

            Dim lineWidth As Single = 1
            Dim frame As RectangleF = New RectangleF(lineWidth / 2, lineWidth / 2, Size.Width - lineWidth, Size.Height - lineWidth)
            If (GraphicsVisibile) Then
                composer.BeginLocalState()
                composer.SetLineWidth(lineWidth)
                composer.SetFillColor(BackColor)
                composer.SetStrokeColor(ForeColor)
                composer.DrawRectangle(frame, 5)
                composer.FillStroke()
                composer.End()
            End If

            Dim title As String = CStr(field.Value)
            If (title IsNot Nothing) Then
                Dim blockComposer As BlockComposer = New BlockComposer(composer)
                blockComposer.Begin(frame, XAlignmentEnum.Center, YAlignmentEnum.Middle)
                composer.SetFillColor(ForeColor)
                composer.SetFont(
                            New StandardType1Font(
                                            document,
                                            StandardType1Font.FamilyEnum.Helvetica,
                                            True,
                                            False
                                            ),
                            Size.Height * 0.5
                            )
                blockComposer.ShowText(title)
                blockComposer.End()
            End If

            composer.Flush()

            appearance.Normal(Nothing) = normalAppearanceState
        End Sub

        Private Overloads Sub Apply(ByVal field As TextField)
            Dim document As Document = field.Document
            Dim widget As Widget = field.Widgets(0)

            Dim appearance As Appearance = widget.Appearance
            If (appearance Is Nothing) Then
                appearance = New Appearance(document)
                widget.Appearance = appearance
            End If

            widget.BaseDataObject(PdfName.DA) = New PdfString("/Helv " & Me.FontSize & " Tf 0 0 0 rg")

            Dim normalAppearanceState As FormXObject
            Dim Size As SizeF = widget.Box.Size
            normalAppearanceState = New FormXObject(document, Size)
            Dim composer As PrimitiveComposer = New PrimitiveComposer(normalAppearanceState)

            Dim lineWidth As Single = 1
            Dim frame As RectangleF = New RectangleF(lineWidth / 2, lineWidth / 2, Size.Width - lineWidth, Size.Height - lineWidth)
            If (GraphicsVisibile) Then
                composer.BeginLocalState()
                composer.SetLineWidth(lineWidth)
                composer.SetFillColor(BackColor)
                composer.SetStrokeColor(ForeColor)
                composer.DrawRectangle(frame, 5)
                composer.FillStroke()
                composer.End()
            End If

            composer.BeginMarkedContent(PdfName.Tx)
            composer.SetFont(
                        New StandardType1Font(
                                document,
                                StandardType1Font.FamilyEnum.Helvetica,
                                False,
                                False
                            ),
                        FontSize
                        )
            composer.ShowText(
                            CStr(field.Value),
                            New PointF(0, Size.Height / 2),
                            XAlignmentEnum.Left,
                            YAlignmentEnum.Middle,
                            0
                            )
            composer.End()

            composer.Flush()
            appearance.Normal(Nothing) = normalAppearanceState
        End Sub


        Private Overloads Sub Apply(ByVal field As ComboBox)
            Dim document As Document = field.Document
            Dim widget As Widget = field.Widgets(0)

            Dim appearance As Appearance = widget.Appearance
            If (appearance Is Nothing) Then
                appearance = New Appearance(document)
                widget.Appearance = appearance
            End If

            widget.BaseDataObject(PdfName.DA) = New PdfString("/Helv " & Me.FontSize & " Tf 0 0 0 rg")

            Dim normalAppearanceState As FormXObject

            Dim Size As SizeF = widget.Box.Size
            normalAppearanceState = New FormXObject(document, Size)
            Dim composer As PrimitiveComposer = New PrimitiveComposer(normalAppearanceState)

            Dim lineWidth As Single = 1
            Dim frame As RectangleF = New RectangleF(lineWidth / 2, lineWidth / 2, Size.Width - lineWidth, Size.Height - lineWidth)
            If (GraphicsVisibile) Then
                composer.BeginLocalState()
                composer.SetLineWidth(lineWidth)
                composer.SetFillColor(BackColor)
                composer.SetStrokeColor(ForeColor)
                composer.DrawRectangle(frame, 5)
                composer.FillStroke()
                composer.End()
            End If

            composer.BeginMarkedContent(PdfName.Tx)
            composer.SetFont(
                            New StandardType1Font(
                                document,
                                StandardType1Font.FamilyEnum.Helvetica,
                                False,
                                False
                                ),
                            FontSize
                            )
            composer.ShowText(
                            CStr(field.Value),
                            New PointF(0, Size.Height / 2),
                            XAlignmentEnum.Left,
                            YAlignmentEnum.Middle,
                            0
                            )
            composer.End()

            composer.Flush()

            appearance.Normal(Nothing) = normalAppearanceState
        End Sub

        Private Overloads Sub Apply(ByVal field As ListBox)
            Dim document As Document = field.Document
            Dim widget As Widget = field.Widgets(0)

            Dim appearance As Appearance = widget.Appearance
            If (appearance Is Nothing) Then
                appearance = New Appearance(document)
                widget.Appearance = appearance
            End If

            Dim widgetDataObject As PdfDictionary = widget.BaseDataObject
            widgetDataObject(PdfName.DA) = New PdfString("/Helv " & Me.FontSize & " Tf 0 0 0 rg")
            widgetDataObject(PdfName.MK) = New PdfDictionary(
                                                New PdfName() {PdfName.BG, PdfName.BC},
                                                New PdfDirectObject() {New PdfArray(New PdfDirectObject() {PdfReal.Get(0.9), PdfReal.Get(0.9), PdfReal.Get(0.9)}), New PdfArray(New PdfDirectObject() {PdfInteger.Default, PdfInteger.Default, PdfInteger.Default})}
                                            )

            Dim normalAppearanceState As FormXObject
            Dim size As SizeF = widget.Box.Size
            normalAppearanceState = New FormXObject(document, size)
            Dim composer As PrimitiveComposer = New PrimitiveComposer(normalAppearanceState)

            Dim lineWidth As Single = 1
            Dim frame As RectangleF = New RectangleF(lineWidth / 2, lineWidth / 2, size.Width - lineWidth, size.Height - lineWidth)
            If (GraphicsVisibile) Then
                composer.BeginLocalState()
                composer.SetLineWidth(lineWidth)
                composer.SetFillColor(BackColor)
                composer.SetStrokeColor(ForeColor)
                composer.DrawRectangle(frame, 5)
                composer.FillStroke()
                composer.End()
            End If

            composer.BeginLocalState()
            If (GraphicsVisibile) Then
                composer.DrawRectangle(frame, 5)
                composer.Clip() ' Ensures that the visible content Is clipped within the rounded frame.
            End If
            composer.BeginMarkedContent(PdfName.Tx)
            composer.SetFont(New StandardType1Font(document, StandardType1Font.FamilyEnum.Helvetica, False, False), FontSize)
            Dim y As Double = 3
            For Each item As ChoiceItem In field.Items
                composer.ShowText(item.Text,
                            New PointF(0, CSng(y))
                            )
                y += FontSize * 1.175
                If (y > size.Height) Then
                    Exit For
                End If

            Next

            composer.End()
            composer.End()

            composer.Flush()
            appearance.Normal(Nothing) = normalAppearanceState
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace