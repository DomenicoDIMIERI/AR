Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.composition
Imports entities = DMD.org.dmdpdf.documents.contents.entities
Imports fonts = DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.objects
Imports xObjects = DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.util.math.geom

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates some of the graphics operations available
    '  through the PrimitiveComposer and BlockComposer classes to compose a PDF document.</summary>
    '*/
    Public Class GraphicsSample
        Inherits Sample

        Private Shared ReadOnly SampleColor As DeviceRGBColor = DeviceRGBColor.Get(System.Drawing.Color.Red)
        Private Shared ReadOnly BackColor As DeviceRGBColor = New DeviceRGBColor(210 / 255D, 232 / 255D, 245 / 255D)

        Public Overrides Sub Run()
            ' 1. Instantiate a new PDF file!
            Dim file As File = New File()
            Dim document As Document = file.Document

            ' 2. Insert the contents into the document!
            BuildCurvesPage(document)
            BuildMiscellaneousPage(document)
            BuildSimpleTextPage(document)
            BuildTextBlockPage(document)
            BuildTextBlockPage2(document)
            BuildTextBlockPage3(document)
            BuildTextBlockPage4(document)

            ' 3. Serialize the PDF file!
            Serialize(file, "Composition elements", "applying the composition elements", "graphics, line styles, text alignment, shapes, circles, ellipses, spirals, polygons, rounded rectangles, images, clipping")
        End Sub

        Private Sub BuildCurvesPage(ByVal document As Document)
            Dim arcFrame As RectangleF

            ' 1. Add the page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page In the pages collection.

            Dim pageSize As SizeF = page.Size

            ' 2. Create a content composer for the page!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)

            ' 3. Drawing the page contents...
            composer.SetFont(
                            New fonts.StandardType1Font(
                                                  document,
                                                  fonts.StandardType1Font.FamilyEnum.Courier,
                                                  True,
                                                  False
                                                  ),
                            32
                            )

            '{
            Dim BlockComposer As BlockComposer = New BlockComposer(composer)
            BlockComposer.Begin(New RectangleF(30, 0, pageSize.Width - 60, 50), XAlignmentEnum.Center, YAlignmentEnum.Middle)
            BlockComposer.ShowText("Curves")
            BlockComposer.End()
            '}

            '// 3.1. Arcs.
            '{
            Dim y As Single = 100
            For rowIndex As Integer = 0 To 4 - 1
                Dim angleStep As Integer = 45
                Dim startAngle As Integer = 0
                Dim endAngle As Integer = angleStep
                Dim x As Single = 100
                Dim diameterX As Single
                Dim diameterY As Single
                Select Case (rowIndex)
                    Case 1
                        diameterX = 40
                        diameterY = 20
                        'break;
                    Case 2
                        diameterX = 20
                        diameterY = 40
                        'break;
                    Case 3
                        diameterX = 40
                        diameterY = 40
                        'break;
                    Case Else '        Case 0 : Default
                        diameterX = 40
                        diameterY = 40
                        'break;
                End Select
                Dim length As Integer = 360 \ angleStep
                For index As Integer = 0 To length - 1
                    arcFrame = New RectangleF(CSng(x), CSng(y), CSng(diameterX), CSng(diameterY))
                    ' Drawing the arc frame...
                    composer.BeginLocalState()
                    composer.SetLineWidth(0.25F)
                    composer.SetLineDash(New LineDash(New Double() {5, 5}, 3))
                    composer.DrawRectangle(arcFrame)
                    composer.Stroke()
                    composer.End()

                    ' Draw the arc!
                    composer.DrawArc(arcFrame, startAngle, endAngle)
                    composer.Stroke()

                    endAngle += angleStep
                    Select Case (rowIndex)
                        Case 3
                            startAngle += angleStep
                            'break;
                    End Select

                    x += 50
                Next

                y += diameterY + 10
            Next
            '}

            '// 3.2. Circle.
            '{
            arcFrame = New RectangleF(100, 300, 100, 100)

            ' Drawing the circle frame...
            composer.BeginLocalState()
            composer.SetLineWidth(0.25F)
            composer.SetLineDash(New LineDash(New Double() {5, 5}, 3))
            composer.DrawRectangle(arcFrame)
            composer.Stroke()
            composer.End()

            ' Drawing the circle...
            composer.SetFillColor(DeviceRGBColor.Get(System.Drawing.Color.Red))
            composer.DrawEllipse(arcFrame)
            composer.FillStroke()
            '}

            ' 3.3. Horizontal ellipse.
            '{
            arcFrame = New RectangleF(210, 300, 100, 50)

            ' Drawing the ellipse frame...
            composer.BeginLocalState()
            composer.SetLineWidth(0.25F)
            composer.SetLineDash(New LineDash(New Double() {5, 5}, 3))
            composer.DrawRectangle(arcFrame)
            composer.Stroke()
            composer.End()

            ' Drawing the ellipse...
            composer.SetFillColor(DeviceRGBColor.Get(System.Drawing.Color.Green))
            composer.DrawEllipse(arcFrame)
            composer.FillStroke()
            '}

            ' 3.4. Vertical ellipse.
            '{
            arcFrame = New RectangleF(320, 300, 50, 100)

            ' Drawing the ellipse frame...
            composer.BeginLocalState()
            composer.SetLineWidth(0.25F)
            composer.SetLineDash(New LineDash(New Double() {5, 5}, 3))
            composer.DrawRectangle(arcFrame)
            composer.Stroke()
            composer.End()

            ' Drawing the ellipse...
            composer.SetFillColor(DeviceRGBColor.Get(System.Drawing.Color.Blue))
            composer.DrawEllipse(arcFrame)
            composer.FillStroke()
            '}

            ' 3.5. Spirals.
            '{
            y = 500
            Dim spiralWidth As Single = 100
            composer.SetLineWidth(0.5F)
            For rowIndex As Integer = 0 To 3 - 1
                Dim x As Single = 150
                Dim branchWidth As Single = 0.5F
                Dim branchRatio As Single = 1
                For spiralIndex As Integer = 0 To 4 - 1
                    Dim spiralTurnsCount As Single
                    Select Case (rowIndex)
                        Case 1
                            spiralTurnsCount = CSng(spiralWidth / (branchWidth * 8 * (spiralIndex * 1.15 + 1)))
                            'break;
                        Case Else 'Case 0 : Default
                            spiralTurnsCount = spiralWidth / (branchWidth * 8)
                            'break;
                    End Select
                    Select Case (rowIndex)
                        Case 2
                            composer.SetLineDash(New LineDash(New Double() {10, 5}))
                            composer.SetLineCap(LineCapEnum.Round)
                            'break;
                        Case Else 'Default
                            'break;
                    End Select

                    composer.DrawSpiral(
                                  New PointF(CSng(x), CSng(y)),
                                  0,
                                  360 * spiralTurnsCount,
                                  branchWidth,
                                  branchRatio
                                  )
                    composer.Stroke()

                    x += spiralWidth + 10

                    Select Case (rowIndex)
                        Case 1
                            branchRatio += 0.035F
                            'break;
                        Case Else 'Case 0 : Default
                            branchWidth += 1
                            'break;
                    End Select
                    Select Case (rowIndex)
                        Case 2
                            composer.SetLineWidth(composer.State.LineWidth + 0.5F)
                            'break;
                    End Select
                Next

                y += spiralWidth + 10
            Next
            '     }

            ' 4. Flush the contents into the page!
            composer.Flush()
        End Sub

        Private Sub BuildMiscellaneousPage(ByVal document As Document)
            ' 1. Add the page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page In the pages collection.

            Dim pageSize As SizeF = page.Size

            ' 2. Create a content composer for the page!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)

            ' 3. Drawing the page contents...
            composer.SetFont(
                            New fonts.StandardType1Font(
                              document,
                              fonts.StandardType1Font.FamilyEnum.Courier,
                              True,
                              False
                              ),
                            32
                            )

            '{
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            blockComposer.Begin(New RectangleF(30, 0, pageSize.Width - 60, 50), XAlignmentEnum.Center, YAlignmentEnum.Middle)
            blockComposer.ShowText("Miscellaneous")
            blockComposer.End()
            '}

            composer.BeginLocalState()
            composer.SetLineJoin(LineJoinEnum.Round)
            composer.SetLineCap(LineCapEnum.Round)

            ' 3.1. Polygon.
            composer.DrawPolygon(
                            New PointF() {
                                          New PointF(100, 200),
                                          New PointF(150, 150),
                                          New PointF(200, 150),
                                          New PointF(250, 200)
                                        }
                            )

            ' 3.2. Polyline.
            composer.DrawPolyline(
                                New PointF() {
                                  New PointF(300, 200),
                                  New PointF(350, 150),
                                  New PointF(400, 150),
                                  New PointF(450, 200)
                                }
                                )

            composer.Stroke()

            ' 3.3. Rectangle (both squared And rounded).
            Dim x As Integer = 50
            Dim radius As Integer = 0
            While (x < 500)
                If (x > 300) Then
                    composer.SetLineDash(New LineDash(New Double() {5, 5}, 3))
                End If

                composer.SetFillColor(New DeviceRGBColor(1, x / 500D, x / 500D))
                composer.DrawRectangle(
                                          New RectangleF(x, 250, 150, 100),
                                          radius
                                          ) '// NOTE: radius parameter determines the rounded angle size.
                composer.FillStroke()

                x += 175
                radius += 10
            End While
            composer.End() ' End local state.

            composer.BeginLocalState()
            composer.SetFont(composer.State.Font, 12)

            ' 3.4. Line cap parameter.
            Dim y As Integer = 400
            For Each lineCap As LineCapEnum In CType([Enum].GetValues(GetType(LineCapEnum)), LineCapEnum())
                composer.ShowText(
                              lineCap.ToString & ":",
                              New PointF(50, y),
                              XAlignmentEnum.Left,
                              YAlignmentEnum.Middle,
                              0
                              )
                composer.SetLineWidth(12)
                composer.SetLineCap(lineCap)
                composer.DrawLine(New PointF(120, y), New PointF(220, y))
                composer.Stroke()

                composer.BeginLocalState()
                composer.SetLineWidth(1)
                composer.SetStrokeColor(DeviceRGBColor.White)
                composer.SetLineCap(LineCapEnum.Butt)
                composer.DrawLine(New PointF(120, y), New PointF(220, y))
                composer.Stroke()
                composer.End() ' End local state.

                y += 30
            Next

            ' 3.5. Line join parameter.
            y += 50
            For Each lineJoin As LineJoinEnum In CType([Enum].GetValues(GetType(LineJoinEnum)), LineJoinEnum())
                composer.ShowText(
                  lineJoin.ToString & ":",
                  New PointF(50, y),
                  XAlignmentEnum.Left,
                  YAlignmentEnum.Middle,
                  0
                  )
                composer.SetLineWidth(12)
                composer.SetLineJoin(lineJoin)
                Dim points As PointF() = New PointF() {
                                                New PointF(120, y + 25),
                                                New PointF(150, y - 25),
                                                New PointF(180, y + 25)
                                              }
                composer.DrawPolyline(points)
                composer.Stroke()

                composer.BeginLocalState()
                composer.SetLineWidth(1)
                composer.SetStrokeColor(DeviceRGBColor.White)
                composer.SetLineCap(LineCapEnum.Butt)
                composer.DrawPolyline(points)
                composer.Stroke()
                composer.End() ' End local state.

                y += 50
            Next
            composer.End() ' End local state.

            '// 3.6. Clipping.
            '/*
            '  NOTE:                   Clipping should be conveniently enclosed within a local state
            '  in order to easily resume the unaltered drawing area after the operation completes.
            '*/
            composer.BeginLocalState()
            composer.DrawPolygon(
                                New PointF() {
                                              New PointF(220, 410),
                                              New PointF(300, 490),
                                              New PointF(450, 360),
                                              New PointF(430, 520),
                                              New PointF(590, 565),
                                              New PointF(420, 595),
                                              New PointF(460, 730),
                                              New PointF(380, 650),
                                              New PointF(330, 765),
                                              New PointF(310, 640),
                                              New PointF(220, 710),
                                              New PointF(275, 570),
                                              New PointF(170, 500),
                                              New PointF(275, 510)
                                            }
                                )
            composer.Clip()
            ' Showing a clown image...
            ' Instantiate a jpeg image object!
            Dim image As entities.Image = entities.Image.Get(GetResourcePath("images" & System.IO.Path.DirectorySeparatorChar & "Clown.jpg")) ' Abstract image (entity).
            Dim imageXObject As xObjects.XObject = image.ToXObject(document)
            ' Show the image!
            composer.ShowXObject(
                            imageXObject,
                            New PointF(170, 320),
                            GeomUtils.Scale(imageXObject.Size, New SizeF(450, 0))
                            )
            composer.End() ' End local state.

            ' 4. Flush the contents into the page!
            composer.Flush()
        End Sub

        Private Sub BuildSimpleTextPage(ByVal document As Document)
            ' 1. Add the page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page In the pages collection.

            Dim pageSize As SizeF = page.Size

            ' 2. Create a content composer for the page!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)
            ' 3. Inserting contents...
            ' Set the font to use!
            composer.SetFont(
                            New fonts.StandardType1Font(
                              document,
                              fonts.StandardType1Font.FamilyEnum.Courier,
                              True,
                              False
                              ),
                            32
                            )

            Dim xAlignments As XAlignmentEnum() = CType([Enum].GetValues(GetType(XAlignmentEnum)), XAlignmentEnum())
            Dim yAlignments As YAlignmentEnum() = CType([Enum].GetValues(GetType(YAlignmentEnum)), YAlignmentEnum())
            Dim [step] As Integer = CInt(pageSize.Height) \ ((xAlignments.Length - 1) * yAlignments.Length + 1)

            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            Dim frame As RectangleF = New RectangleF(
                                            30,
                                            0,
                                            pageSize.Width - 60,
                                            [step] \ 2
                                            )
            blockComposer.Begin(frame, XAlignmentEnum.Center, YAlignmentEnum.Middle)
            blockComposer.ShowText("Simple alignment")
            blockComposer.End()

            frame = New RectangleF(
                                30,
                                pageSize.Height - [step] \ 2,
                                pageSize.Width - 60,
                                [step] \ 2 - 10
                                )
            blockComposer.Begin(frame, XAlignmentEnum.Left, YAlignmentEnum.Bottom)
            composer.SetFont(composer.State.Font, 10)
            blockComposer.ShowText("NOTE: showText(...) methods return the actual bounding box of the text shown." & vbCr &
                                    "NOTE: The rotation parameter can be freely defined as a floating point value.")
            blockComposer.End()

            composer.SetFont(composer.State.Font, 12)
            Dim x As Integer = 30
            Dim y As Integer = [step]
            Dim alignmentIndex As Integer = 0
            For Each xAlignment As XAlignmentEnum In CType([Enum].GetValues(GetType(XAlignmentEnum)), XAlignmentEnum())
                '/*
                '  NOTE: As text shown through PrimitiveComposer has no bounding box constraining its extension,
                '  applying the justified alignment has no effect (it degrades to center alignment);
                '  in order to get such an effect, use BlockComposer instead.
                '*/
                If (xAlignment.Equals(XAlignmentEnum.Justify)) Then Continue For

                For Each yAlignment As YAlignmentEnum In CType([Enum].GetValues(GetType(YAlignmentEnum)), YAlignmentEnum())
                    If (alignmentIndex Mod 2 = 0) Then
                        composer.BeginLocalState()
                        composer.SetFillColor(BackColor)
                        composer.DrawRectangle(
                                          New RectangleF(
                                            0,
                                            y - [step] \ 2,
                                            pageSize.Width,
                                            [step]
                                            )
                                          )
                        composer.Fill()
                        composer.End()
                    End If

                    composer.ShowText(xAlignment.ToString & " " & yAlignment.ToString & ":",
                                        New PointF(x, y),
                                        XAlignmentEnum.Left,
                                        YAlignmentEnum.Middle,
                                        0
                                        )

                    y += [step]
                    alignmentIndex += 1
                Next
            Next

            Dim rotationStep As Single = 0
            Dim rotation As Single = 0
            For columnIndex As Integer = 0 To 2 - 1
                Select Case (columnIndex)
                    Case 0
                        x = 200
                        rotationStep = 0
                        'break;
                    Case 1
                        x = CInt(pageSize.Width) \ 2 + 100
                        rotationStep = 360 / ((xAlignments.Length - 1) * yAlignments.Length - 1)
                        'break;
                End Select
                y = [step]
                rotation = 0
                For Each xAlignment As XAlignmentEnum In CType([Enum].GetValues(GetType(XAlignmentEnum)), XAlignmentEnum())
                    '/*
                    '  NOTE: As text shown through PrimitiveComposer has no bounding box constraining its extension,
                    '  applying the justified alignment has no effect (it degrades to center alignment);
                    '  in order to get such an effect, use BlockComposer instead.
                    '*/
                    If (xAlignment.Equals(XAlignmentEnum.Justify)) Then Continue For

                    For Each yAlignment As YAlignmentEnum In CType([Enum].GetValues(GetType(YAlignmentEnum)), YAlignmentEnum())
                        Dim startArcAngle As Single = 0
                        Select Case (xAlignment)
                            Case XAlignmentEnum.Left
                                ' OK -- NOOP.
                                'break;
                            Case XAlignmentEnum.Right, XAlignmentEnum.Center
                                startArcAngle = 180
                                'break;
                        End Select

                        composer.DrawArc(
                                      New RectangleF(
                                                        x - 10,
                                                        y - 10,
                                                        20,
                                                        20
                                                        ),
                                      startArcAngle,
                                      startArcAngle + rotation
                                      )

                        DrawText(composer, "PDF Clown", New PointF(x, y), xAlignment, yAlignment, rotation)
                        y += [step]
                        rotation += rotationStep
                    Next
                Next
            Next

            ' 4. Flush the contents into the page!
            composer.Flush()
        End Sub

        Private Sub BuildTextBlockPage(ByVal document As Document)
            ' 1. Add the page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page in the pages collection.

            Dim pageSize As SizeF = page.Size

            ' 2. Create a content composer for the page!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)

            ' 3. Drawing the page contents...
            Dim mainFont As fonts.Font = New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Courier, True, False)
            Dim [step] As Integer
            '{
            Dim xAlignments As XAlignmentEnum() = CType([Enum].GetValues(GetType(XAlignmentEnum)), XAlignmentEnum())
            Dim yAlignments As YAlignmentEnum() = CType([Enum].GetValues(GetType(YAlignmentEnum)), YAlignmentEnum())
            [step] = CInt(pageSize.Height) \ (xAlignments.Length * yAlignments.Length + 1)
            '}
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            '{
            blockComposer.Begin(
                              New RectangleF(
                                            30,
                                            0,
                                            pageSize.Width - 60,
                                            [step] * 0.8F
                                            ),
                              XAlignmentEnum.Center,
                              YAlignmentEnum.Middle
                              )
            composer.SetFont(mainFont, 32)
            blockComposer.ShowText("Block alignment")
            blockComposer.End()
            '}

            ' Drawing the text blocks...
            Dim sampleFont As fonts.Font = New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Times, False, False)
            Dim x As Integer = 30
            Dim y As Integer = CInt(Math.Floor([step] * 1.2))
            For Each xAlignment As XAlignmentEnum In CType([Enum].GetValues(GetType(XAlignmentEnum)), XAlignmentEnum())
                For Each yAlignment As YAlignmentEnum In CType([Enum].GetValues(GetType(YAlignmentEnum)), YAlignmentEnum())
                    composer.SetFont(mainFont, 12)
                    composer.ShowText(xAlignment.ToString & " " & yAlignment.ToString & ":",
                                    New PointF(x, y),
                                    XAlignmentEnum.Left,
                                    YAlignmentEnum.Middle,
                                    0
                                    )

                    composer.SetFont(sampleFont, 10)
                    For index As Integer = 0 To 2 - 1
                        Dim frameX As Integer
                        Select Case (index)
                            Case 0
                                frameX = 150
                                blockComposer.Hyphenation = False
                                'break;
                            Case 1
                                frameX = 360
                                blockComposer.Hyphenation = True
                                'break;
                            Case Else
                                Throw New Exception()
                        End Select

                        Dim frame As RectangleF = New RectangleF(
                                                              frameX,
                                                              y - [step] * 0.4F,
                                                              200,
                                                              [step] * 0.8F
                                                              )
                        blockComposer.Begin(frame, xAlignment, yAlignment)
                        blockComposer.ShowText("Demonstrating how to constrain text inside a page area using PDF Clown. See the other available code samples (such as TypesettingSample) to discover more functionality details.")
                        blockComposer.End()

                        composer.BeginLocalState()
                        composer.SetLineWidth(0.2F)
                        composer.SetLineDash(New LineDash(New Double() {5, 5}, 5))
                        composer.DrawRectangle(frame)
                        composer.Stroke()
                        composer.End()
                    Next

                    y += [step]
                Next
            Next

            ' 4. Flush the contents into the page!
            composer.Flush()
        End Sub

        Private Sub BuildTextBlockPage2(ByVal document As Document)
            ' 1. Add the page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page In the pages collection.

            Dim pageSize As SizeF = page.Size

            ' 2. Create a content composer for the page!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)

            ' 3. Drawing the page contents...
            Dim mainFont As fonts.Font = New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Courier, True, False)
            Dim stepCount As Integer = 5
            Dim [step] As Integer = CInt(pageSize.Height) \ (stepCount + 1)
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            '{
            blockComposer.Begin(
                              New RectangleF(
                                            30,
                                            0,
                                            pageSize.Width - 60,
                                            [step] * 0.8F
                                            ),
                              XAlignmentEnum.Center,
                              YAlignmentEnum.Middle
                              )
            composer.SetFont(mainFont, 32)
            blockComposer.ShowText("Block line alignment")
            blockComposer.End()
            '}

            ' Drawing the text block...
            '{
            Dim sampleFont As fonts.Font = New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Times, False, False)
            Dim sampleImage As entities.Image = entities.Image.Get(GetResourcePath("images" & System.IO.Path.DirectorySeparatorChar & "gnu.jpg"))
            Dim sampleImageXObject As xObjects.XObject = sampleImage.ToXObject(document)

            Dim lineAlignments As IList(Of LineAlignmentEnum) = New List(Of LineAlignmentEnum)(CType([Enum].GetValues(GetType(LineAlignmentEnum)), LineAlignmentEnum()))
            Dim frameHeight As Single = (pageSize.Height - 130 - 5 * lineAlignments.Count * 2) / (lineAlignments.Count * 2)
            Dim frameWidth As Single = (pageSize.Width - 60 - 5 * lineAlignments.Count) / lineAlignments.Count
            Dim imageSize As Integer = 7
            Dim length As Integer = lineAlignments.Count
            For index As Integer = 0 To length - 1
                Dim lineAlignment As LineAlignmentEnum = lineAlignments(index)
                Dim imageLength As Integer = lineAlignments.Count
                For imageIndex As Integer = 0 To imageLength - 1
                    Dim imageAlignment As LineAlignmentEnum = lineAlignments(imageIndex)
                    Dim length2 As Integer = 2
                    For index2 As Integer = 0 To length2 - 1
                        Dim frame As RectangleF = New RectangleF(
                                                        30 + (frameWidth + 5) * imageIndex,
                                                        100 + (frameHeight + 5) * (index * 2 + index2),
                                                        frameWidth,
                                                        frameHeight
                                                        )
                        blockComposer.Begin(frame, XAlignmentEnum.Left, YAlignmentEnum.Top)
                        '{
                        composer.SetFont(mainFont, 3)
                        blockComposer.ShowText("Text: " & lineAlignment)
                        blockComposer.ShowBreak()
                        blockComposer.ShowText("Image: " & imageAlignment)
                        '}
                        blockComposer.End()

                        blockComposer.Begin(frame, XAlignmentEnum.Left, YAlignmentEnum.Middle)
                        '{
                        composer.SetFont(sampleFont, 3)
                        blockComposer.ShowText("Previous row boundary.")
                        blockComposer.ShowBreak()
                        If (index2 = 0) Then
                            composer.SetFont(sampleFont, 3)
                        Else
                            composer.SetFont(sampleFont, 6)
                        End If

                        blockComposer.ShowText("Alignment:")

                        composer.SetFont(sampleFont, CDbl(IIf(index2 = 0, 6, 3)))
                        blockComposer.ShowText(" aligned to " & lineAlignment & " ", lineAlignment)
                        blockComposer.ShowXObject(sampleImageXObject, New SizeF(imageSize, imageSize), imageAlignment)
                        blockComposer.ShowBreak()
                        composer.SetFont(sampleFont, 3)
                        blockComposer.ShowText("Next row boundary.")
                        '}
                        blockComposer.End()

                        composer.BeginLocalState()
                        '{
                        composer.SetLineWidth(0.1F)
                        composer.SetLineDash(New LineDash(New Double() {1, 4}, 4))
                        composer.DrawRectangle(blockComposer.Frame)
                        composer.Stroke()
                        '}
                        composer.End()

                        composer.BeginLocalState()
                        '{
                        composer.SetLineWidth(0.1F)
                        composer.SetLineDash(New LineDash(New Double() {1, 1}, 1))
                        composer.DrawRectangle(blockComposer.BoundBox)
                        composer.Stroke()
                        '}
                        composer.End()
                    Next
                Next
            Next
            '}

            ' 4. Flush the contents into the page!
            composer.Flush()
        End Sub

        Private Sub BuildTextBlockPage3(ByVal document As Document)
            ' 1. Add the page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page In the pages collection.

            Dim pageSize As SizeF = page.Size

            ' 2. Create a content composer for the page!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)

            ' 3. Drawing the page contents...
            Dim mainFont As fonts.Font = New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Courier, True, False)
            Dim stepCount As Integer = 5
            Dim [step] As Integer = CInt(pageSize.Height) \ (stepCount + 1)

            ' 3.1. Drawing the page title...
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            '{
            blockComposer.Begin(
                          New RectangleF(
                                        30,
                                        0,
                                        pageSize.Width - 60,
                                        [step] * 0.8F
                                        ),
                          XAlignmentEnum.Center,
                          YAlignmentEnum.Middle
                          )
            composer.SetFont(mainFont, 32)
            blockComposer.ShowText("Block line space")
            blockComposer.End()
            '}

            ' 3.2. Drawing the text blocks...
            Dim sampleFont As fonts.Font = New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Times, False, False)
            Dim x As Integer = 30
            Dim y As Integer = CInt(Math.Floor([step] * 1.1))
            blockComposer.LineSpace.UnitMode = Length.UnitModeEnum.Relative
            For index As Integer = 0 To stepCount - 1
                Dim relativeLineSpace As Single = 0.5F * index
                blockComposer.LineSpace.Value = relativeLineSpace

                composer.SetFont(mainFont, 12)
                composer.ShowText(
                              relativeLineSpace & ":",
                              New PointF(x, y),
                              XAlignmentEnum.Left,
                              YAlignmentEnum.Middle,
                              0
                              )

                composer.SetFont(sampleFont, 10)
                Dim frame As RectangleF = New RectangleF(150, y - [step] * 0.4F, 350, [step] * 0.9F)
                blockComposer.Begin(frame, XAlignmentEnum.Left, YAlignmentEnum.Top)
                blockComposer.ShowText("Demonstrating how to set the block line space. Line space can be expressed either as an absolute value (in user-space units) or as a relative one (floating-point ratio); in the latter case the base value is represented by the current font's line height (so that, for example, 2 means ""a line space that's twice as the line height"").")
                blockComposer.End()

                composer.BeginLocalState()
                '{
                composer.SetLineWidth(0.2)
                composer.SetLineDash(New LineDash(New Double() {5, 5}, 5))
                composer.DrawRectangle(frame)
                composer.Stroke()
                '}
                composer.End()

                y += [step]
            Next

            ' 4. Flush the contents into the page!
            composer.Flush()
        End Sub

        Private Sub BuildTextBlockPage4(ByVal document As Document)
            ' 1. Add the page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page In the pages collection.

            Dim pageSize As SizeF = page.Size

            ' 2. Create a content composer for the page!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)

            ' 3. Drawing the page contents...
            Dim mainFont As fonts.Font = New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Courier, True, False)
            Dim stepCount As Integer = 5
            Dim [step] As Integer = CInt(pageSize.Height) \ (stepCount + 1)
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            '{
            blockComposer.Begin(
                                  New RectangleF(
                                    30,
                                    0,
                                    pageSize.Width - 60,
                                    [step] * 0.8F
                                    ),
                                  XAlignmentEnum.Center,
                                  YAlignmentEnum.Middle
                                  )
            composer.SetFont(mainFont, 32)
            blockComposer.ShowText("Unspaced block")
            blockComposer.End()
            '}

            ' Drawing the text block...
            '{
            Dim sampleFont As fonts.Font = New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Times, False, False)
            composer.SetFont(sampleFont, 15)

            Dim topMargin As Single = 100
            Dim boxMargin As Single = 30
            Dim boxWidth As Single = pageSize.Width - boxMargin * 2
            Dim boxHeight As Single = (pageSize.Height - topMargin - boxMargin - boxMargin) / 2
            '{
            Dim frame As RectangleF = New RectangleF(
                                        boxMargin,
                                        topMargin,
                                        boxWidth,
                                        boxHeight
                                        )
            blockComposer.Begin(frame, XAlignmentEnum.Left, YAlignmentEnum.Top)
            ' Add text until the frame area Is completely filled!
            While (blockComposer.ShowText("DemonstratingHowUnspacedTextIsManagedInCaseOfInsertionInADelimitedPageAreaThroughBlockComposerClass.") > 0)

            End While
            blockComposer.End()

            composer.BeginLocalState()
            '{
            composer.SetLineWidth(0.2)
            composer.SetLineDash(New LineDash(New Double() {5, 5}, 5))
            composer.DrawRectangle(frame)
            composer.Stroke()
            '}
            composer.End()
            '}
            '{
            frame = New RectangleF(
                                        boxMargin,
                                        topMargin + boxHeight + boxMargin,
                                        boxWidth,
                                        boxHeight
                                        )
            blockComposer.Begin(frame, XAlignmentEnum.Left, YAlignmentEnum.Top)
            ' Add text until the frame area Is completely filled!
            While (blockComposer.ShowText(" DemonstratingHowUnspacedTextWithLeadingSpaceIsManagedInCaseOfInsertionInADelimitedPageAreaThroughBlockComposerClass.") > 0)

            End While
            blockComposer.End()

            composer.BeginLocalState()
            '{
            composer.SetLineWidth(0.2)
            composer.SetLineDash(New LineDash(New Double() {5, 5}, 5))
            composer.DrawRectangle(frame)
            composer.Stroke()
            '}
            composer.End()
            '}

            ' 4. Flush the contents into the page!
            composer.Flush()
        End Sub

        Private Sub DrawCross(ByVal composer As PrimitiveComposer, ByVal center As PointF)
            composer.DrawLine(
                        New PointF(center.X - 10, center.Y),
                        New PointF(center.X + 10, center.Y)
                        )
            composer.DrawLine(
                            New PointF(center.X, center.Y - 10),
                            New PointF(center.X, center.Y + 10)
                            )
            composer.Stroke()
        End Sub

        Private Sub DrawFrame(ByVal composer As PrimitiveComposer, ByVal frameVertices As PointF())
            composer.BeginLocalState()
            composer.SetLineWidth(0.2F)
            composer.SetLineDash(New LineDash(New Double() {5, 5}, 5))
            composer.DrawPolygon(frameVertices)
            composer.Stroke()
            composer.End()
        End Sub

        Private Sub DrawText(ByVal composer As PrimitiveComposer, ByVal value As String, ByVal location As PointF, ByVal xAlignment As XAlignmentEnum, ByVal yAlignment As YAlignmentEnum, ByVal rotation As Single)
            ' Show the anchor point!
            DrawCross(composer, location)

            composer.BeginLocalState()
            composer.SetFillColor(SampleColor)
            ' Show the text onto the page!
            Dim textFrame As Quad = composer.ShowText(
                                                value,
                                                location,
                                                xAlignment,
                                                yAlignment,
                                                rotation
                                                )
            composer.End()

            ' Draw the frame binding the shown text!
            DrawFrame(
                    composer,
                    textFrame.Points
                    )

            composer.BeginLocalState()
            composer.SetFont(composer.State.Font, 8)
            ' Draw the rotation degrees!
            composer.ShowText(
                            "(" & CInt(rotation).ToString & " degrees)",
                            New PointF(
                              location.X + 70,
                              location.Y
                              ),
                            XAlignmentEnum.Left,
                            YAlignmentEnum.Middle,
                            0
                            )
            composer.End()
        End Sub

    End Class

End Namespace