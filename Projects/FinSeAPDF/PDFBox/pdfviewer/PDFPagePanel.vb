Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D

Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdfviewer


    '/**
    ' * This is a simple JPanel that can be used to display a PDF page.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDFPagePanel
        Inherits Panel ' JPanel

        Private Const serialVersionUID As Long = -4629033339560890669L

        Private page As PDPage
        Private drawer As PageDrawer = Nothing
        Private pageDimension As SizeF = Nothing 'Dimension 
        Private drawDimension As SizeF = Nothing 'Dimension 

        '/**
        ' * Constructor.
        ' *
        ' * @throws IOException If there is an error creating the Page drawing objects.
        ' */
        Public Sub New() 'throws IOException
            drawer = New PageDrawer()
        End Sub

        '/**
        ' * This will set the page that should be displayed in this panel.
        ' *
        ' * @param pdfPage The page to draw.
        ' */
        Public Sub setPage(ByVal pdfPage As PDPage)
            page = pdfPage
            Dim cropBox As PDRectangle = page.findCropBox()
            drawDimension = cropBox.createDimension()
            Dim rotation As Integer = page.findRotation()
            If (rotation = 90 OrElse rotation = 270) Then
                pageDimension = New SizeF(drawDimension.Height, drawDimension.Width)
            Else
                pageDimension = drawDimension
            End If
            Me.Size = New Size(pageDimension.Width, pageDimension.Height)
            Me.BackColor = Color.White
        End Sub


        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Try
                Dim g2D As Graphics2D = Graphics2D.FromGraphics(e.Graphics)
                g2D.setColor(New JColor(Me.BackColor))
                g2D.fillRect(0, 0, Me.Width, Me.Height)

                Dim rotation As Integer = page.findRotation()
                If (rotation = 90 OrElse rotation = 270) Then
                    g2D.translate(pageDimension.Width, 0.0F)
                    g2D.rotate(Math.toRadians(rotation))
                End If

                drawer.drawPage(g2D, page, drawDimension)
            Catch ex As IOException
                Debug.Print(ex.ToString)
            End Try
        End Sub

    End Class

End Namespace
