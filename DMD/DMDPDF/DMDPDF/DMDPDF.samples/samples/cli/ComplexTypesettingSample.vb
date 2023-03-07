Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.entities
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.interaction
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.documents.interaction.navigation.page
Imports DMD.org.dmdpdf.documents.interchange.metadata
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Drawing
Imports System.IO

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to create a new PDF document populating it with various
    '  graphics elements.</summary>
    '  <remarks>
    '    <para>This implementation features an enlightening example of an embryonic typesetter
    '    that exploits the new typographic primitives defined in PDF Clown (see BlockComposer
    '    class in use); this is just a humble experiment -- anybody could develop a typesetter
    '    sitting upon PDF Clown!</para>
    '    <para>Anyway, PDF Clown currently lacks support for content flow composition (i.e. paragraphs
    '    spread across multiple pages): since 0.0.3 release offers a static-composition facility
    '    (BlockComposer class) that is meant to be the base for more advanced functionalities (such as the
    '    above-mentioned content flow composition), to be made available in the next releases.</para>
    '  </remarks>
    '*/
    Public Class ComplexTypesettingSample
        Inherits Sample

        Private Shared ReadOnly TextColor_Highlight As colorSpaces.Color = New colorSpaces.DeviceRGBColor(255 / 255D, 50 / 255D, 50 / 255D)

        Public Overrides Sub Run()
            ' 1. PDF file instantiation.
            Dim file As DMD.org.dmdpdf.files.File = New DMD.org.dmdpdf.files.File()
            Dim document As Document = file.Document
            ' Set default page size (A4)!
            document.PageSize = PageFormat.GetSize()

            ' 2. Content creation.
            Dim creationDate As DateTime = DateTime.Now
            ' 2.1. Template.
            Dim template As FormXObject = BuildTemplate(document, creationDate)
            ' 2.2. Welcome page.
            BuildWelcomePage(document, template)
            ' 2.3. Free Software definition.
            BuildFreeSoftwareDefinitionPages(document, template)
            ' 2.4. Bookmarks.
            BuildBookmarks(document)

            ' 3. Serialization.
            Serialize(file, "Complex Typesetting", "complex typesetting", "typesetting, bookmarks, hyphenation, block composer, primitive composer, text alignment, image insertion, article threads")
        End Sub

        Private Sub BuildBookmarks(ByVal document As Document)
            Dim pages As Pages = document.Pages
            Dim bookmarks As Bookmarks = document.Bookmarks
            document.PageMode = Document.PageModeEnum.Bookmarks
            Dim page As Page = pages(0)
            Dim rootBookmark As Bookmark = New Bookmark(document, "Creation Sample", New LocalDestination(page))
            bookmarks.Add(rootBookmark)
            bookmarks = rootBookmark.Bookmarks
            page = pages(1)
            Dim bookmark As Bookmark = New Bookmark(document, "2nd page (close-up view)", New LocalDestination(page, Destination.ModeEnum.XYZ, New PointF(0, 250), 2))
            bookmarks.Add(bookmark)
            bookmark.Bookmarks.Add(New Bookmark(document, "2nd page (mid view)", New LocalDestination(page, Destination.ModeEnum.XYZ, New PointF(0, page.Size.Height - 250), 1)))
            page = pages(2)
            bookmarks.Add(New Bookmark(document, "3rd page (fit horizontal view)", New LocalDestination(page, Destination.ModeEnum.FitHorizontal, 0, Nothing)))
            bookmark = New Bookmark(document, "PDF Clown Home Page", New actions.GoToURI(document, New Uri("http://www.dmdstore.it")))
            bookmarks.Add(bookmark)
            bookmark.Flags = Bookmark.FlagsEnum.Bold Or Bookmark.FlagsEnum.Italic
            bookmark.Color = New colorSpaces.DeviceRGBColor(0.5, 0.5, 1)
        End Sub

        Private Sub BuildFreeSoftwareDefinitionPages(ByVal document As Document, ByVal template As FormXObject)
            ' Add page!
            Dim page As Page = New Page(document)
            document.Pages.Add(page)
            Dim pageSize As SizeF = page.Size

            Dim title As String = "The Free Software Definition"

            ' Create the article thread!
            Dim article As Article = New Article(document)
            '{
            Dim articleInfo As Information = article.Information
            articleInfo.Title = title
            articleInfo.Author = "Free Software Foundation, Inc."
            '}
            ' Get the article beads collection to populate!
            Dim articleElements As ArticleElements = article.Elements

            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)
            ' Add the background template!
            composer.ShowXObject(template)
            '// Wrap the content composer inside a block composer in order to achieve higher-level typographic control!
            '/*
            '  NOTE: BlockComposer provides block-level typographic features as text And paragraph alignment.
            '  Flow-level typographic features are currently Not supported: block-level typographic features
            '  are the foundations upon which flow-level typographic features will sit.
            '*/
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            blockComposer.Hyphenation = True

            Dim breakSize As SizeF = New SizeF(0, 10)
            ' Add the font to the document!
            Dim font As fonts.Font = fonts.Font.Get(document, GetResourcePath("fonts" & Path.DirectorySeparatorChar & "TravelingTypewriter.otf"))

            Dim frame As RectangleF = New RectangleF(20, 150, (pageSize.Width - 90 - 20) / 2, pageSize.Height - 250)

            ' Showing the 'GNU' image...
            ' Instantiate a jpeg image object!
            Dim Image As entities.Image = entities.Image.Get(GetResourcePath("images" & Path.DirectorySeparatorChar & "gnu.jpg")) ' Abstract image (entity)
            ' Show the image!
            composer.ShowXObject(Image.ToXObject(document), New PointF((pageSize.Width - 90 - Image.Width) / 2 + 20, pageSize.Height - 100 - Image.Height))

            ' Showing the title...
            blockComposer.Begin(frame, XAlignmentEnum.Left, YAlignmentEnum.Top)
            composer.SetFont(font, 24)
            blockComposer.ShowText(title)
            blockComposer.End()

            ' Showing the copyright note...
            frame = New RectangleF(CSng(blockComposer.BoundBox.X), CSng(blockComposer.BoundBox.Y + blockComposer.BoundBox.Height + 32),
                                CSng(blockComposer.BoundBox.Width),
                                CSng(pageSize.Height - 100 - Image.Height - 10) - (blockComposer.BoundBox.Y + blockComposer.BoundBox.Height + 32)
                                )
            blockComposer.Begin(frame, XAlignmentEnum.Justify, YAlignmentEnum.Bottom)
            composer.SetFont(font, 6)
            blockComposer.ShowText("Copyright 2004, 2005, 2006 Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA Verbatim copying and distribution of this entire article are permitted worldwide, without royalty, in any medium, provided this notice is preserved.")

            ' Showing the body...
            blockComposer.ShowBreak(breakSize)
            composer.SetFont(font, 8.25F)
            Dim frames As RectangleF() = New RectangleF() {
                              New RectangleF(
                                CSng(blockComposer.BoundBox.X),
                                CSng(pageSize.Height - 100 - Image.Height - 10),
                                CSng(blockComposer.BoundBox.Width - Image.Width / 2),
                                CSng(Image.Height + 10)
                                ),
                              New RectangleF(
                                CSng(20 + 20 + (pageSize.Width - 90 - 20) / 2),
                                150,
                                CSng(pageSize.Width - 90 - 20) / 2,
                                CSng(pageSize.Height - 100 - Image.Height - 10) - 150
                                ),
                              New RectangleF(
                                CSng(20 + 20 + (pageSize.Width - 90 - 20) / 2 + Image.Width / 2),
                                CSng(pageSize.Height - 100 - Image.Height - 10),
                                CSng(blockComposer.BoundBox.Width - Image.Width / 2),
                                CSng(Image.Height + 10)
                                ),
                              New RectangleF(
                                20,
                                150,
                                CSng(pageSize.Width - 90 - 20) / 2,
                                CSng(pageSize.Height - 100) - 150
                                ),
                              New RectangleF(
                                CSng(20 + 20 + (pageSize.Width - 90 - 20) / 2),
                                150,
                                CSng(pageSize.Width - 90 - 20) / 2,
                                CSng(pageSize.Height - 100) - 150
                                )
                            }
            Dim yAlignments As YAlignmentEnum() = New YAlignmentEnum() {
                                                          YAlignmentEnum.Top,
                                                          YAlignmentEnum.Bottom,
                                                          YAlignmentEnum.Top,
                                                          YAlignmentEnum.Top,
                                                          YAlignmentEnum.Top
                                                        }
            Dim paragraphs As String() = New String() {
          "We maintain this free software definition to show clearly what must be true about a particular software program for it to be considered free software.",
          """Free software"" Is a matter of liberty, Not price. To understand the concept, you should think of ""free"" as in ""free speech"", Not as in ""free beer"".",
          "Free software Is a matter of the users' freedom to run, copy, distribute, study, change and improve the software. More precisely, it refers to four kinds of freedom, for the users of the software:",
          "* The freedom to run the program, for any purpose (freedom 0).",
          "* The freedom to study how the program works, and adapt it to your needs (freedom 1). Access to the source code is a precondition for this.",
          "* The freedom to redistribute copies so you can help your neighbor (freedom 2).",
          "* The freedom to improve the program, and release your improvements to the public, so that the whole community benefits (freedom 3). Access to the source code is a precondition for this.",
          "A program is free software if users have all of these freedoms. Thus, you should be free to redistribute copies, either with or without modifications, either gratis or charging a fee for distribution, to anyone anywhere. Being free to do these things means (among other things) that you do not have to ask or pay for permission.",
          "You should also have the freedom to make modifications and use them privately in your own work or play, without even mentioning that they exist. If you do publish your changes, you should not be required to notify anyone in particular, or in any particular way.",
          "The freedom to use a program means the freedom for any kind of person or organization to use it on any kind of computer system, for any kind of overall job, and without being required to communicate subsequently with the developer or any other specific entity.",
          "The freedom to redistribute copies must include binary or executable forms of the program, as well as source code, for both modified and unmodified versions. (Distributing programs in runnable form is necessary for conveniently installable free operating systems.) It is ok if there is no way to produce a binary or executable form for a certain program (since some languages don't support that feature), but you must have the freedom to redistribute such forms should you find or develop a way to make them.",
          "In order for the freedoms to make changes, and to publish improved versions, to be meaningful, you must have access to the source code of the program. Therefore, accessibility of source code is a necessary condition for free software.",
          "In order for these freedoms to be real, they must be irrevocable as long as you do nothing wrong; if the developer of the software has the power to revoke the license, without your doing anything to give cause, the software is not free.",
          "However, certain kinds of rules about the manner of distributing free software are acceptable, when they don't conflict with the central freedoms. For example, copyleft (very simply stated) is the rule that when redistributing the program, you cannot add restrictions to deny other people the central freedoms. This rule does not conflict with the central freedoms; rather it protects them.",
          "You may have paid money to get copies of free software, or you may have obtained copies at no charge. But regardless of how you got your copies, you always have the freedom to copy and change the software, even to sell copies.",
          """Free software"" does not mean ""non-commercial"". A free program must be available for commercial use, commercial development, and commercial distribution. Commercial development of free software is no longer unusual; such free commercial software is very important.",
          "Rules about how to package a modified version are acceptable, if they don't substantively block your freedom to release modified versions. Rules that ""if you make the program available in this way, you must make it available in that way also"" can be acceptable too, on the same condition. (Note that such a rule still leaves you the choice of whether to publish the program or not.) It is also acceptable for the license to require that, if you have distributed a modified version and a previous developer asks for a copy of it, you must send one, or that you identify yourself on your modifications.",
          "In the GNU project, we use ""copyleft"" to protect these freedoms legally for everyone. But non-copylefted free software also exists. We believe there are important reasons why it is better to use copyleft, but if your program is non-copylefted free software, we can still use it.",
          "See Categories of Free Software for a description of how ""free software,"" ""copylefted software"" and other categories of software relate to each other.",
          "Sometimes government export control regulations and trade sanctions can constrain your freedom to distribute copies of programs internationally. Software developers do not have the power to eliminate or override these restrictions, but what they can and must do is refuse to impose them as conditions of use of the program. In this way, the restrictions will not affect activities and people outside the jurisdictions of these governments.",
          "Most free software licenses are based on copyright, and there are limits on what kinds of requirements can be imposed through copyright. If a copyright-based license respects freedom in the ways described above, it is unlikely to have some other sort of problem that we never anticipated (though this does happen occasionally). However, some free software licenses are based on contracts, and contracts can impose a much larger range of possible restrictions. That means there are many possible ways such a license could be unacceptably restrictive and non-free.",
          "We can't possibly list all the possible contract restrictions that would be unacceptable. If a contract-based license restricts the user in an unusual way that copyright-based licenses cannot, and which isn't mentioned here as legitimate, we will have to think about it, and we will probably decide it is non-free.",
          "When talking about free software, it is best to avoid using terms like ""give away"" or ""for free"", because those terms imply that the issue is about price, not freedom. Some common terms such as ""piracy"" embody opinions we hope you won't endorse. See Confusing Words and Phrases that are Worth Avoiding for a discussion of these terms. We also have a list of translations of ""free software"" into various languages.",
          "Finally, note that criteria such as those stated in this free software definition require careful thought for their interpretation. To decide whether a specific software license qualifies as a free software license, we judge it based on these criteria to determine whether it fits their spirit as well as the precise words. If a license includes unconscionable restrictions, we reject it, even if we did not anticipate the issue in these criteria. Sometimes a license requirement raises an issue that calls for extensive thought, including discussions with a lawyer, before we can decide if the requirement is acceptable. When we reach a conclusion about a new issue, we often update these criteria to make it easier to see why certain licenses do or don't qualify.",
          "If you are interested in whether a specific license qualifies as a free software license, see our list of licenses. If the license you are concerned with is not listed there, you can ask us about it by sending us email at <licensing@fsf.org>.",
          "If you are contemplating writing a new license, please contact the FSF by writing to that address. The proliferation of different free software licenses means increased work for users in understanding the licenses; we may be able to help you find an existing Free Software license that meets your needs.",
          "If that isn't possible, if you really need a new license, with our help you can ensure that the license really is a Free Software license and avoid various practical problems.",
          "Another group has started using the term ""open source"" to mean something close (but not identical) to ""free software"". We prefer the term ""free software"" because, once you have heard it refers to freedom rather than price, it calls to mind freedom. The word ""open"" never does that."
        }
            Dim paragraphIndex As Integer = 0
            Dim paragraphTextIndex As Integer = 0
            Dim frameIndex As Integer = -1
            Dim paragraphCount As Integer = paragraphs.Length
            For paragraphIndex = 0 To paragraphCount - 1
                Dim paragraph As String = paragraphs(paragraphIndex)
                'System.Diagnostics.Debug.Print("paragraph: " & paragraph)
                paragraphTextIndex = blockComposer.ShowText(paragraph.Substring(paragraphTextIndex)) + paragraphTextIndex
                'System.Diagnostics.Debug.Print("paragraphTextIndex: " & paragraphTextIndex)
                If (paragraphTextIndex < paragraph.Length) Then
                    frameIndex += 1
                    If (frameIndex < frames.Length) Then
                        'System.Diagnostics.Debug.Print("frameIndex: " & frameIndex)
                        blockComposer.End()
                        ' Add the bead to the article thread!
                        articleElements.Add(New ArticleElement(page, blockComposer.BoundBox))

                        ' New page?
                        If (frameIndex = 3) Then
                            ' Close current page!
                            composer.Flush()

                            ' Create a New page!
                            page = New Page(document)
                            document.Pages.Add(page)
                            composer = New PrimitiveComposer(page)
                            ' Add the background template!
                            composer.ShowXObject(template)
                            blockComposer = New BlockComposer(composer)
                            blockComposer.Hyphenation = True
                        End If

                        blockComposer.Begin(frames(frameIndex), XAlignmentEnum.Justify, yAlignments(frameIndex))
                        composer.SetFont(font, 8.25F)

                        ' Come back to complete the interrupted paragraph!
                        paragraphIndex -= 1
                    Else
                        Exit For
                    End If
                Else
                    paragraphTextIndex = 0
                    blockComposer.ShowBreak(breakSize)
                End If
            Next
            blockComposer.End()

            ' Add the bead to the article thread!
            articleElements.Add(New ArticleElement(page, blockComposer.BoundBox))

            blockComposer.Begin(frames(frames.Length - 1), XAlignmentEnum.Justify, YAlignmentEnum.Bottom)
            composer.SetFont(font, 6)
            blockComposer.ShowText("This article was crafted with the nice Traveling_Typewriter font (by Carl Krull, www.carlkrull.dk).")
            blockComposer.End()

            composer.Flush()
        End Sub

        Private Sub BuildWelcomePage(ByVal document As Document, ByVal template As FormXObject)
            ' Add welcome page to the document!
            Dim page As Page = New Page(document) ' Instantiates the page inside the document context.
            document.Pages.Add(page) ' Puts the page In the pages collection.
            Dim pageSize As SizeF = page.Size

            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)
            ' Add the background template!
            composer.ShowXObject(template)
            '// Wrap the content composer inside a block composer in order to achieve higher-level typographic control!
            '/*
            '  NOTE:               blockComposer provides block-level typographic features as text And paragraph alignment.
            '  Flow-level typographic features are currently Not supported: block-level typographic features
            '  are the foundations upon which flow-level typographic features will sit.
            '*/
            Dim blockComposer As BlockComposer = New BlockComposer(composer)

            Dim breakSize As SizeF = New SizeF(0, 20) ' Size of a paragraph break.
            ' Instantiate the page body's font!
            Dim font As fonts.Font = fonts.Font.Get(document, GetResourcePath("fonts" & Path.DirectorySeparatorChar & "lazyDog.ttf"))

            ' Showing the page title...
            ' Define the box frame to force the page title within!
            Dim frame As RectangleF = New RectangleF(
                                            20,
                                            150,
                                            CSng(pageSize.Width - 90),
                                            CSng(pageSize.Height - 250)
                                            )
            ' Begin the block!
            blockComposer.Begin(frame, XAlignmentEnum.Center, YAlignmentEnum.Top)
            ' Set the font to use!
            composer.SetFont(font, 56)
            ' Set the text rendering mode (outline only)!
            composer.SetTextRenderMode(TextRenderModeEnum.Stroke)
            ' Show the page title!
            blockComposer.ShowText("Welcome")
            ' End the block!
            blockComposer.End()

            ' Showing the clown photo...
            ' Instantiate a jpeg image object!
            Dim Image As entities.Image = entities.Image.Get(GetResourcePath("images" & Path.DirectorySeparatorChar & "Clown.jpg")) ' Abstract image (entity)
            Dim imageLocation As PointF = New PointF(
                                            blockComposer.BoundBox.X + blockComposer.BoundBox.Width - Image.Width,
                                            blockComposer.BoundBox.Y + blockComposer.BoundBox.Height + 25
                                            )
            ' Show the image!
            composer.ShowXObject(Image.ToXObject(document), imageLocation)

            Dim descriptionFrame As RectangleF = New RectangleF(imageLocation.X, imageLocation.Y + Image.Height + 5, Image.Width, 20)

            frame = New RectangleF(blockComposer.BoundBox.X, imageLocation.Y, blockComposer.BoundBox.Width - Image.Width - 20, Image.Height)
            blockComposer.Begin(frame, XAlignmentEnum.Left, YAlignmentEnum.Middle)
            '{
            composer.SetFont(font, 30)
            blockComposer.ShowText("This is a sample document that merely demonstrates some basic graphics features supported by PDF Clown.")
            blockComposer.ShowBreak(XAlignmentEnum.Center)
            blockComposer.ShowText("Enjoy!")
            '}
            blockComposer.End()

            frame = New RectangleF(
                            blockComposer.BoundBox.X,
                            blockComposer.BoundBox.Y + blockComposer.BoundBox.Height,
                            pageSize.Width - 90,
                            pageSize.Height - 100 - (blockComposer.BoundBox.Y + blockComposer.BoundBox.Height)
                            )
            blockComposer.Begin(frame, XAlignmentEnum.Justify, YAlignmentEnum.Bottom)
            '{
            composer.SetFont(font, 14)
            blockComposer.ShowText("PS: As promised, since version 0.0.3 PDF Clown has supported")
            '// Begin local state!
            '/*
            '  NOTE:             Local state Is a powerful feature of PDF format as it lets you nest
            '  multiple Graphics contexts on the graphics state stack.
            '*/
            composer.BeginLocalState()
            '{
            composer.SetFillColor(TextColor_Highlight)
            blockComposer.ShowText(" embedded latin OpenFont/TrueType and non-embedded Type 1 fonts")
            '}
            composer.End()
            blockComposer.ShowText(" along with")
            composer.BeginLocalState()
            '{
            composer.SetFillColor(TextColor_Highlight)
            blockComposer.ShowText(" paragraph construction facilities")
            '}
            composer.End()
            blockComposer.ShowText(" through the BlockComposer class.")
            blockComposer.ShowBreak(breakSize)

            blockComposer.ShowText("Since version 0.0.4 the content stream stack has been completed, providing ")
            composer.BeginLocalState()
            '{
            composer.SetFillColor(TextColor_Highlight)
            blockComposer.ShowText("fully object-oriented access to the graphics objects that describe the contents on a page.")
            '}
            composer.End()
            blockComposer.ShowText(" It's a great step towards a whole bunch of possibilities, such as text extraction/replacement, that next releases will progressively exploit.")
            blockComposer.ShowBreak(breakSize)

            blockComposer.ShowText("Since version 0.0.6 it has supported ")
            composer.BeginLocalState()
            '{
            composer.SetFillColor(TextColor_Highlight)
            blockComposer.ShowText("Unicode")
            '}
            composer.End() '
            blockComposer.ShowText(" for OpenFont/TrueType fonts.")
            blockComposer.ShowBreak(breakSize)

            composer.SetFont(font, 8)
            blockComposer.ShowText("This page was crafted with the nice")
            composer.BeginLocalState()
            '{
            composer.SetFont(font, 10)
            blockComposer.ShowText(" LazyDog font")
            '}
            composer.End()
            blockComposer.ShowText(" (by Paul Neave, www.neave.com)")
            '}
            blockComposer.End()

            blockComposer.Begin(descriptionFrame, XAlignmentEnum.Right, YAlignmentEnum.Top)
            '{
            composer.SetFont(font, 8)
            blockComposer.ShowText("Source: http://www.wikipedia.org/")
            '}
            blockComposer.End()

            composer.Flush()
        End Sub

        Private Function BuildTemplate(ByVal document As Document, ByVal creationDate As DateTime) As FormXObject
            ' Create a template (form)!
            Dim template As FormXObject = New FormXObject(document, document.PageSize.Value)
            Dim templateSize As SizeF = template.Size

            ' Get form content stream!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(template)

            '// Showing the header image inside the common content stream...
            '// Instantiate a jpeg image object!
            Dim Image As entities.Image = entities.Image.Get(GetResourcePath("images" & Path.DirectorySeparatorChar & "mountains.jpg")) ' Abstract image (entity).
            ' Show the image inside the common content stream!
            composer.ShowXObject(Image.ToXObject(document), New PointF(0, 0), New SizeF(templateSize.Width - 50, 125))

            ' Showing the 'PDFClown' label inside the common content stream...
            composer.BeginLocalState()
            composer.SetFillColor(New colorSpaces.DeviceRGBColor(115.0F / 255, 164.0F / 255, 232.0F / 255))
            ' Set the font to use!
            composer.SetFont(New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Times, True, False), 120)
            ' Show the text!
            composer.ShowText("DMDPDF", New PointF(0, templateSize.Height - CSng(composer.State.Font.GetAscent(composer.State.FontSize))))

            ' Drawing the side rectangle...
            composer.DrawRectangle(New RectangleF(CSng(templateSize.Width - 50), 0, 50, CSng(templateSize.Height)))
            composer.Fill()
            composer.End()

            ' Showing the side text inside the common content stream...
            composer.BeginLocalState()
            '{
            composer.SetFont(New fonts.StandardType1Font(document, fonts.StandardType1Font.FamilyEnum.Helvetica, False, False), 8)
            composer.SetFillColor(colorSpaces.DeviceRGBColor.White)
            composer.BeginLocalState()
            '{
            composer.Rotate(90, New PointF(templateSize.Width - 50, templateSize.Height - 25))
            Dim blockComposer As BlockComposer = New BlockComposer(composer)
            blockComposer.Begin(New RectangleF(0, 0, 300, 50), XAlignmentEnum.Left, YAlignmentEnum.Middle)
            '{
            blockComposer.ShowText("Generated by PDF Clown on " & creationDate)
            blockComposer.ShowBreak()
            blockComposer.ShowText("For more info, visit http://www.dmdstore.it")
            '}
            blockComposer.End()
            '}
            composer.End()
            '}
            composer.End()

            composer.Flush()

            Return template
        End Function

    End Class

End Namespace
