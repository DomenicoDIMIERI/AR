Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.layers
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Drawing

Namespace org.dmdpdf.samples.cli

    '/**
    '  <summary>This sample demonstrates how to define layers to control content visibility.</summary>
    '*/
    Public Class LayerCreationSample
        Inherits Sample

        Public Overrides Sub Run()
            ' 1. PDF file instantiation.
            Dim file As File = New File()
            Dim document As Document = file.Document

            ' 2. Content creation.
            Populate(document)

            ' 3. PDF file serialization.
            Serialize(file, "Layer", "inserting layers", "layers, optional content")
        End Sub

        '/**
        '  <summary>Populates a PDF file with contents.</summary>
        '*/
        Private Sub Populate(ByVal document As Document)
            ' Initialize a new page!
            Dim page As Page = New Page(document)
            document.Pages.Add(page)

            ' Initialize the primitive composer (within the new page context)!
            Dim composer As PrimitiveComposer = New PrimitiveComposer(page)
            composer.SetFont(New StandardType1Font(document, StandardType1Font.FamilyEnum.Helvetica, True, False), 12)

            ' Initialize the block composer (wrapping the primitive one)!
            Dim blockComposer As BlockComposer = New BlockComposer(composer)

            ' Initialize the document layer configuration!
            Dim layerDefinition As LayerDefinition = document.Layer
            document.PageMode = Document.PageModeEnum.Layers ' Shows the layers tab on document opening.

            ' Get the root layers collection!
            Dim rootLayers As Layers = layerDefinition.Layers

            ' 1. Nested layers.
            '{
            Dim nestedLayer As Layer = New Layer(document, "Nested layers")
            rootLayers.Add(nestedLayer)
            Dim nestedSubLayers As Layers = nestedLayer.Layers

            Dim nestedLayer1 As Layer = New Layer(document, "Nested layer 1")
            nestedSubLayers.Add(nestedLayer1)

            Dim nestedLayer2 As Layer = New Layer(document, "Nested layer 2")
            nestedSubLayers.Add(nestedLayer2)
            nestedLayer2.Locked = True

            '/*
            '  NOTE: Text in this section is shown using PrimitiveComposer.
            '*/
            composer.BeginLayer(nestedLayer)
            composer.ShowText(nestedLayer.Title, New PointF(50, 50))
            composer.End()

            composer.BeginLayer(nestedLayer1)
            composer.ShowText(nestedLayer1.Title, New PointF(50, 75))
            composer.End()

            composer.BeginLayer(nestedLayer2)
            composer.ShowText(nestedLayer2.Title, New PointF(50, 100))
            composer.End()
            '}

            ' 2. Simple group (labeled group of non-nested, inclusive-state layers).
            '{
            Dim simpleGroup As Layers = New Layers(document, "Simple group")
            rootLayers.Add(simpleGroup)

            Dim layer1 As Layer = New Layer(document, "Grouped layer 1")
            simpleGroup.Add(layer1)

            Dim layer2 As Layer = New Layer(document, "Grouped layer 2")
            simpleGroup.Add(layer2)

            '/*
            '  NOTE: Text in this section is shown using BlockComposer along with PrimitiveComposer
            '  to demonstrate their flexible cooperation.
            '*/
            blockComposer.Begin(New RectangleF(50, 125, 200, 50), XAlignmentEnum.Left, YAlignmentEnum.Middle)

            composer.BeginLayer(layer1)
            blockComposer.ShowText(layer1.Title)
            composer.End()

            blockComposer.ShowBreak(New SizeF(0, 15))

            composer.BeginLayer(layer2)
            blockComposer.ShowText(layer2.Title)
            composer.End()

            blockComposer.End()
            '}

            ' 3. Radio group (labeled group of non-nested, exclusive-state layers).
            '{
            Dim radioGroup As Layers = New Layers(document, "Radio group")
            rootLayers.Add(radioGroup)

            Dim radio1 As Layer = New Layer(document, "Radiogrouped layer 1")
            radioGroup.Add(radio1)
            radio1.Visible = True

            Dim radio2 As Layer = New Layer(document, "Radiogrouped layer 2")
            radioGroup.Add(radio2)
            radio2.Visible = False

            Dim radio3 As Layer = New Layer(document, "Radiogrouped layer 3")
            radioGroup.Add(radio3)
            radio3.Visible = False

            ' Register this option group in the layer configuration!
            Dim options As LayerGroup = New LayerGroup(document)
            options.Add(radio1)
            options.Add(radio2)
            options.Add(radio3)
            layerDefinition.OptionGroups.Add(options)

            '/*
            '  NOTE: Text in this section is shown using BlockComposer along with PrimitiveComposer
            '  to demonstrate their flexible cooperation.
            '*/
            blockComposer.Begin(New RectangleF(50, 185, 200, 75), XAlignmentEnum.Left, YAlignmentEnum.Middle)

            composer.BeginLayer(radio1)
            blockComposer.ShowText(radio1.Title)
            composer.End()

            blockComposer.ShowBreak(New SizeF(0, 15))

            composer.BeginLayer(radio2)
            blockComposer.ShowText(radio2.Title)
            composer.End()

            blockComposer.ShowBreak(New SizeF(0, 15))

            composer.BeginLayer(radio3)
            blockComposer.ShowText(radio3.Title)
            composer.End()

            blockComposer.End()
            '}

            ' 4. Print-only layer.
            '{
            Dim printOnlyLayer As Layer = New Layer(document, "Print-only layer")
            printOnlyLayer.Visible = False
            printOnlyLayer.Printable = True
            printOnlyLayer.Locked = True
            rootLayers.Add(printOnlyLayer)

            composer.BeginLayer(printOnlyLayer)
            composer.ShowText(printOnlyLayer.Title, New PointF(50, 270))
            composer.End()
            '}
            composer.Flush()
        End Sub
    End Class
End Namespace