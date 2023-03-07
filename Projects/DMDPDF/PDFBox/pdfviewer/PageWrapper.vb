Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Imports FinSeA.org.apache.pdfbox.PDFReader
Imports FinSeA.org.apache.pdfbox.pdmodel

Namespace org.apache.pdfbox.pdfviewer


    '/**
    ' * A class to handle some prettyness around a single PDF page.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PageWrapper
        'Implements MouseMotionListener

        Private pageWrapper As Panel '= new JPanel();
        Private pagePanel As PDFPagePanel = Nothing
        Private reader As PDFReader = Nothing

        Private Const SPACE_AROUND_DOCUMENT As Integer = 20

        '/**
        ' * Constructor.
        ' *
        ' * @param aReader The reader application that holds this page.
        ' *
        ' * @throws IOException If there is an error creating the page drawing objects.
        ' */
        Public Sub New(ByVal aReader As PDFReader) 'throws IOException
            reader = aReader
            pagePanel = New PDFPagePanel()
            'pageWrapper.LayoutEngine (Nothing)
            pageWrapper.Controls.Add(pagePanel)
            pagePanel.Location = New Point(SPACE_AROUND_DOCUMENT, SPACE_AROUND_DOCUMENT)
            pageWrapper.BorderStyle = BorderStyle.FixedSingle '(javax.swing.border.LineBorder.createBlackLineBorder())
            AddHandler pagePanel.MouseMove, AddressOf _handleMouseMove
        End Sub

        Private Sub _handleMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)

        End Sub

        '/**
        ' * This will display the PDF page in this component.
        ' *
        ' * @param page The PDF page to display.
        ' */
        Public Sub displayPage(ByVal page As PDPage)
            pagePanel.setPage(page)
            'pagePanel.setPreferredSize(pagePanel.getSize())
            Dim d As Size = pagePanel.Size 'Dimension 
            d.Width += (SPACE_AROUND_DOCUMENT * 2)
            d.Height += (SPACE_AROUND_DOCUMENT * 2)

            'pageWrapper.setPreferredSize(d)
            'pageWrapper.validate()
        End Sub

        '/**
        ' * This will get the JPanel that can be displayed.
        ' *
        ' * @return The panel with the displayed PDF page.
        ' */
        Public Function getPanel() As Panel 'JPanel 
            Return pageWrapper
        End Function

        'Public Sub mouseDragged(ByVal e As MouseEventArgs)
        '    'do nothing when mouse moves.
        'End Sub

        'Public Sub mouseMoved(ByVal e As MouseEventArgs)
        '    'reader.getBottomStatusPanel().getStatusLabel().setText( e.getX() + "," + (pagePanel.getHeight() - e.getY()) );
        '    'reader.getBottomStatusPanel().getStatusLabel().setText(e.getX() & "," & e.getY())
        'End Sub

    End Class

End Namespace
