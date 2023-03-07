Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO
Imports System.Windows.Forms

Namespace org.apache.pdfbox

    '/**
    ' * An application to read PDF documents.  This will provide Acrobat Reader like
    ' * funtionality.
    ' *
    ' * @author <a href="ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDFReader
        Inherits System.Windows.Forms.Form

        Private currentDir As New FinSeA.Io.File(".")
        Private saveAsImageMenuItem As ToolStripMenuItem
        Private exitMenuItem As ToolStripMenuItem
        Private fileMenu As ToolStripMenuItem
        Private menuBar As MenuStrip
        Private openMenuItem As ToolStripMenuItem
        Private printMenuItem As ToolStripMenuItem
        Private viewMenu As ToolStripMenuItem
        Private nextPageItem As ToolStripMenuItem
        Private previousPageItem As ToolStripMenuItem
        Private documentPanel As New Panel
        Private bottomStatusPanel As ReaderBottomPanel

        Private document As PDDocument = Nothing
        Private pages As List(Of PDPage) = Nothing

        Private currentPage As Integer = 0
        Private numberOfPages As Integer = 0
        Private currentFilename As String = vbNullString

        Private Const PASSWORD = "-password"
        Private Const NONSEQ = "-nonSeq"
        Private useNonSeqParser As Boolean = False

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            initComponents()
        End Sub

        Private Sub handleExitApplication(ByVal sender As Object, ByVal e As System.EventArgs)
            exitApplication()
        End Sub
        Private Sub handleOpenMenu(ByVal sender As Object, ByVal evt As System.EventArgs)
            openMenuItemActionPerformed(evt)

        End Sub

        '/**
        ' * This method is called from within the constructor to
        ' * initialize the form.
        ' * WARNING: Do NOT modify Me code. The content of Me method is
        ' * always regenerated by the Form Editor.
        ' */
        Private Sub initComponents()
            menuBar = New MenuStrip
            fileMenu = New ToolStripMenuItem
            openMenuItem = New ToolStripMenuItem
            saveAsImageMenuItem = New ToolStripMenuItem
            exitMenuItem = New ToolStripMenuItem
            printMenuItem = New ToolStripMenuItem
            viewMenu = New ToolStripMenuItem
            nextPageItem = New ToolStripMenuItem
            previousPageItem = New ToolStripMenuItem


            Me.Text = "PDFBox - PDF Reader"

            AddHandler Me.FormClosed, AddressOf handleExitApplication

            '    Dim documentScroller As New Panel
            '    documentScroller.setViewportView(documentPanel)


            '    getContentPane().add(documentScroller, java.awt.BorderLayout.CENTER)
            '    getContentPane().add(bottomStatusPanel, java.awt.BorderLayout.SOUTH)

            '    fileMenu.Text = "File"
            '    openMenuItem.Text = "Open"
            '    openMenuItem.ToolTipText = "Open PDF file"
            '    AddHandler openMenuItem.Click, AddressOf handleOpenMenu
            '    fileMenu.add(openMenuItem)

            'printMenuItem.setText( "Print" );
            'printMenuItem.addActionListener(new java.awt.event.ActionListener()
            '{
            '    public void actionPerformed(java.awt.event.ActionEvent evt)
            '    {
            '    Try
            '        {
            '        If (document IsNot Nothing) Then
            '            {
            '                PDPageable pageable = new PDPageable(document);
            '                PrinterJob job = pageable.getPrinterJob();
            '                job.setPageable(pageable);
            '            If (job.printDialog()) Then
            '                {
            '                    job.print();
            '                }
            '            }
            '        }
            '        catch( PrinterException e )
            '        {
            '            e.printStackTrace();
            '        }
            '    }
            '});
            'fileMenu.add( printMenuItem );

            'saveAsImageMenuItem.setText( "Save as image" );
            'saveAsImageMenuItem.addActionListener(new java.awt.event.ActionListener()
            '{
            '    public void actionPerformed(java.awt.event.ActionEvent evt)
            '    {
            '        If (document IsNot Nothing) Then
            '        {
            '            saveImage();
            '        }
            '    }
            '});
            'fileMenu.add( saveAsImageMenuItem );

            'exitMenuItem.setText("Exit");
            'exitMenuItem.addActionListener(new java.awt.event.ActionListener()
            '{
            '    public void actionPerformed(java.awt.event.ActionEvent evt)
            '    {
            '        exitApplication();
            '    }
            '});

            'fileMenu.add(exitMenuItem);

            'menuBar.add(fileMenu);

            'viewMenu.setText("View");
            'nextPageItem.setText("Next page");
            'nextPageItem.setAccelerator(KeyStroke.getKeyStroke('+'));
            'nextPageItem.addActionListener(new java.awt.event.ActionListener()
            '{
            '    public void actionPerformed(java.awt.event.ActionEvent evt)
            '    {
            '        nextPage();
            '    }
            '});
            'viewMenu.add(nextPageItem);

            'previousPageItem.setText("Previous page");
            'previousPageItem.setAccelerator(KeyStroke.getKeyStroke('-'));
            'previousPageItem.addActionListener(new java.awt.event.ActionListener()
            '{
            '    public void actionPerformed(java.awt.event.ActionEvent evt)
            '    {
            '        previousPage();
            '    }
            '});
            'viewMenu.add(previousPageItem);

            'menuBar.add(viewMenu);

            'setJMenuBar(menuBar);


            'java.awt.Dimension screenSize = java.awt.Toolkit.getDefaultToolkit().getScreenSize();
            'setBounds((screenSize.width-700)/2, (screenSize.height-600)/2, 700, 600);
        End Sub

        Private Sub updateTitle()
            Me.Text = "PDFBox - " & currentFilename & " (" & (currentPage + 1) & "/" & numberOfPages & ")"
        End Sub

        Private Sub nextPage()
            If (currentPage < numberOfPages - 1) Then
                currentPage += 1
                updateTitle()
                showPage(currentPage)
            End If
        End Sub

        Private Sub previousPage()
            If (currentPage > 0) Then
                currentPage -= 1
                updateTitle()
                showPage(currentPage)
            End If
        End Sub

        Private Sub openMenuItemActionPerformed(ByVal evt As System.EventArgs)
            Dim chooser As New OpenFileDialog
            'chooser.InitialDirectory = currentDir

            chooser.Filter = "PDF|PDF Files"
            Dim result As DialogResult = chooser.ShowDialog(Me)
            If (result = Windows.Forms.DialogResult.OK) Then
                Dim name As String = chooser.FileName
                currentDir = New FinSeA.Io.File(name).getParentFile()
                Try
                    openPDFFile(name, "")
                Catch e As Exception
                    Debug.Print(e.ToString)
                End Try
            End If
        End Sub

        Private Sub exitApplication()
            Try
                If (document IsNot Nothing) Then
                    document.close()
                End If
            Catch io As IOException
                'do nothing because we are closing the application
            End Try
            Me.Visible = False
            Me.Dispose()
        End Sub

        '    '/**
        '    ' * @param args the command line arguments
        '    ' *
        '    ' * @throws Exception If anything goes wrong.
        '    ' */
        'public static void main(String[] args) throws Exception
        '{
        '    PDFReader viewer = new PDFReader();
        '    String password = "";
        '    String filename = null;
        '    for( int i = 0; i < args.length; i++ )
        '    {
        '            If (args(i).equals(PASSWORD)) Then
        '        {
        '            i++;
        '                If (i >= args.length) Then
        '            {
        '                usage();
        '            }
        '            password = args(i);
        '        }
        '                    If (args(i).equals(NONSEQ)) Then
        '        {
        '            useNonSeqParser = true;
        '        }
        '                    Else
        '        {
        '            filename = args(i);
        '        }
        '    }
        '    // open the pdf if present
        '                        If (filename IsNot Nothing) Then
        '    {
        '        viewer.openPDFFile( filename, password );
        '    }
        '    viewer.setVisible(true);
        '}

        Private Sub openPDFFile(ByVal filename As String, ByVal password As String)
            If (document IsNot Nothing) Then
                document.close()
                documentPanel.Controls.Clear() '.removeAll()
            End If

            Dim file As New FinSeA.Io.File(filename)
            parseDocument(file, password)
            pages = document.getDocumentCatalog().getAllPages()
            numberOfPages = pages.size()
            currentFilename = file.getAbsolutePath()
            currentPage = 0
            updateTitle()
            showPage(0)
        End Sub

        Private Sub showPage(ByVal pageNumber As Integer)
            Try
                Dim wrapper As New PageWrapper(Me)
                wrapper.displayPage(pages.get(pageNumber))
                If (documentPanel.Controls.Count > 0) Then
                    documentPanel.Controls.RemoveAt(0)
                End If
                documentPanel.Controls.Add(wrapper.getPanel())
                'pack()
            Catch exception As IOException
                Debug.Print(exception.ToString)
            End Try
        End Sub

        Private Sub saveImage()
            Try
                Dim pageToSave As PDPage = pages.get(currentPage)
                Dim pageAsImage As BufferedImage = pageToSave.convertToImage()
                Dim imageFilename As String = currentFilename
                If (imageFilename.ToLower().EndsWith(".pdf")) Then
                    imageFilename = imageFilename.Substring(0, imageFilename.Length() - 4)
                End If
                imageFilename &= "_" & (currentPage + 1)
                ImageIOUtil.writeImage(pageAsImage, "png", imageFilename, BufferedImage.TYPE_USHORT_565_RGB, 300)
            Catch exception As IOException
                Debug.Print(exception.ToString)
            End Try
        End Sub

        '/**
        ' * This will parse a document.
        ' *
        ' * @param input The input stream for the document.
        ' *
        ' * @throws IOException If there is an error parsing the document.
        ' */
        Private Sub parseDocument(ByVal file As FinSeA.Io.File, ByVal password As String)
            document = Nothing
            If (useNonSeqParser) Then
                document = PDDocument.loadNonSeq(file, Nothing, password)
            Else
                document = PDDocument.load(file)
                If (document.isEncrypted()) Then
                    Try
                        document.decrypt(password)
                    Catch e As InvalidPasswordException
                        LOG.error("Error: The document is encrypted.")
                    Catch e As CryptographyException
                        Debug.Print(e.ToString)
                    End Try
                End If
            End If
        End Sub

        '/**
        ' * Get the bottom status panel.
        ' *
        ' * @return The bottom status panel.
        ' */
        Public Function getBottomStatusPanel() As ReaderBottomPanel
            Return bottomStatusPanel
        End Function

        '/**
        ' * This will print out a message telling how to use Me utility.
        ' */
        'private static void usage()
        '{
        '    System.err.println(
        '            "usage: java -jar pdfbox-app-x.y.z.jar PDFReader [OPTIONS] <input-file>\n" +
        '            "  -password <password>      Password to decrypt the document\n" +
        '            "  -nonSeq                   Enables the new non-sequential parser\n" +
        '            "  <input-file>              The PDF document to be loaded\n"
        '            );
        '}


    End Class

End Namespace