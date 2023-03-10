/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package org.apache.pdfbox;

import org.apache.pdfbox.exceptions.InvalidPasswordException;

import org.apache.pdfbox.pdfviewer.PDFTreeModel;
import org.apache.pdfbox.pdfviewer.PDFTreeCellRenderer;
import org.apache.pdfbox.pdfviewer.ArrayEntry;
import org.apache.pdfbox.pdfviewer.MapEntry;

import org.apache.pdfbox.pdmodel.PDDocument;

import org.apache.pdfbox.util.ExtensionFileFilter;

import org.apache.pdfbox.cos.COSBoolean;
import org.apache.pdfbox.cos.COSFloat;
import org.apache.pdfbox.cos.COSInteger;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.cos.COSNull;
import org.apache.pdfbox.cos.COSStream;
import org.apache.pdfbox.cos.COSString;

//import javax.swing.tree.*;
import javax.swing.tree.TreeModel;
import javax.swing.tree.TreePath;
import javax.swing.JFileChooser;
import javax.swing.JScrollPane;
import javax.swing.JPanel;
import javax.swing.UIManager;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.InputStream;
import java.io.IOException;

/**
 *
 * @author  wurtz
 * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
 * @version $Revision: 1.4 $
 */
public class PDFDebugger extends javax.swing.JFrame
{
    private File currentDir=new File(".");
    private PDDocument document = null;

    private static final String NONSEQ = "-nonSeq";
    private static final String PASSWORD = "-password";

    private static boolean useNonSeqParser = false; 

    /**
     * Constructor.
     */
    public PDFDebugger()
    {
        initComponents();
    }

    /**
     * This method is called from within the constructor to
     * initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is
     * always regenerated by the Form Editor.
     */
    private void initComponents()
    {
        jSplitPane1 = new javax.swing.JSplitPane();
        jScrollPane1 = new javax.swing.JScrollPane();
        jTree1 = new javax.swing.JTree();
        jScrollPane2 = new javax.swing.JScrollPane();
        jTextPane1 = new javax.swing.JTextPane();
        menuBar = new javax.swing.JMenuBar();
        fileMenu = new javax.swing.JMenu();
        openMenuItem = new javax.swing.JMenuItem();
        saveMenuItem = new javax.swing.JMenuItem();
        saveAsMenuItem = new javax.swing.JMenuItem();
        exitMenuItem = new javax.swing.JMenuItem();
        editMenu = new javax.swing.JMenu();
        cutMenuItem = new javax.swing.JMenuItem();
        copyMenuItem = new javax.swing.JMenuItem();
        pasteMenuItem = new javax.swing.JMenuItem();
        deleteMenuItem = new javax.swing.JMenuItem();
        helpMenu = new javax.swing.JMenu();
        contentsMenuItem = new javax.swing.JMenuItem();
        aboutMenuItem = new javax.swing.JMenuItem();

        jTree1.setCellRenderer( new PDFTreeCellRenderer() );
        jTree1.setModel( null );

        setTitle("PDFBox - PDF Viewer");
        addWindowListener(new java.awt.event.WindowAdapter()
        {
            public void windowClosing(java.awt.event.WindowEvent evt)
            {
                exitForm(evt);
            }
        });


        jScrollPane1.setBorder(new javax.swing.border.BevelBorder(javax.swing.border.BevelBorder.RAISED));
        jScrollPane1.setPreferredSize(new java.awt.Dimension(300, 500));
        jTree1.addTreeSelectionListener(new javax.swing.event.TreeSelectionListener()
        {
            public void valueChanged(javax.swing.event.TreeSelectionEvent evt)
            {
                jTree1ValueChanged(evt);
            }
        });

        jScrollPane1.setViewportView(jTree1);

        jSplitPane1.setRightComponent(jScrollPane2);

        jScrollPane2.setPreferredSize(new java.awt.Dimension(300, 500));
        jScrollPane2.setViewportView(jTextPane1);

        jSplitPane1.setLeftComponent(jScrollPane1);

        JScrollPane documentScroller = new JScrollPane();
        //documentScroller.setPreferredSize( new Dimension( 300, 500 ) );
        documentScroller.setViewportView( documentPanel );

        getContentPane().add( jSplitPane1, java.awt.BorderLayout.CENTER );

        fileMenu.setText("File");
        openMenuItem.setText("Open");
        openMenuItem.setToolTipText("Open PDF file");
        openMenuItem.addActionListener(new java.awt.event.ActionListener()
        {
            public void actionPerformed(java.awt.event.ActionEvent evt)
            {
                openMenuItemActionPerformed(evt);
            }
        });

        fileMenu.add(openMenuItem);

        saveMenuItem.setText("Save");

        saveAsMenuItem.setText("Save As ...");

        exitMenuItem.setText("Exit");
        exitMenuItem.addActionListener(new java.awt.event.ActionListener()
        {
            public void actionPerformed(java.awt.event.ActionEvent evt)
            {
                exitMenuItemActionPerformed(evt);
            }
        });

        fileMenu.add(exitMenuItem);

        menuBar.add(fileMenu);

        editMenu.setText("Edit");
        cutMenuItem.setText("Cut");
        editMenu.add(cutMenuItem);

        copyMenuItem.setText("Copy");
        editMenu.add(copyMenuItem);

        pasteMenuItem.setText("Paste");
        editMenu.add(pasteMenuItem);

        deleteMenuItem.setText("Delete");
        editMenu.add(deleteMenuItem);

        helpMenu.setText("Help");
        contentsMenuItem.setText("Contents");
        helpMenu.add(contentsMenuItem);

        aboutMenuItem.setText("About");
        helpMenu.add(aboutMenuItem);

        setJMenuBar(menuBar);


        java.awt.Dimension screenSize = java.awt.Toolkit.getDefaultToolkit().getScreenSize();
        setBounds((screenSize.width-700)/2, (screenSize.height-600)/2, 700, 600);
    }//GEN-END:initComponents

    private void openMenuItemActionPerformed(java.awt.event.ActionEvent evt)
    {
        JFileChooser chooser = new JFileChooser();
        chooser.setCurrentDirectory(currentDir);

        ExtensionFileFilter pdfFilter = new ExtensionFileFilter(new String[] {"pdf", "PDF"}, "PDF Files");
        chooser.setFileFilter(pdfFilter);
        int result = chooser.showOpenDialog(PDFDebugger.this);
        if (result == JFileChooser.APPROVE_OPTION)
        {
            String name = chooser.getSelectedFile().getPath();
            currentDir = new File(name).getParentFile();
            try
            {
                readPDFFile(name, "");
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
        }
    }//GEN-LAST:event_openMenuItemActionPerformed

    private void jTree1ValueChanged(javax.swing.event.TreeSelectionEvent evt)
    {
        TreePath path = jTree1.getSelectionPath();
        if (path IsNot Nothing)
        {
            try
            {
                Object selectedNode = path.getLastPathComponent();
                String data=convertToString(selectedNode);



                if (data IsNot Nothing)
                {
                    jTextPane1.setText(data);
                }
                else
                {
                    jTextPane1.setText( "" );
                }
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
        }
    }//GEN-LAST:event_jTree1ValueChanged

    private String convertToString( Object selectedNode )
    {
        String data = null;
        if(selectedNode instanceof COSBoolean)
        {
            data = "" + ((COSBoolean)selectedNode).getValue();
        }
        else if( selectedNode instanceof COSFloat )
        {
            data = "" + ((COSFloat)selectedNode).floatValue();
        }
        else if( selectedNode instanceof COSNull )
        {
            data = "null";
        }
        else if( selectedNode instanceof COSInteger )
        {
            data = "" + ((COSInteger)selectedNode).intValue();
        }
        else if( selectedNode instanceof COSName )
        {
            data = "" + ((COSName)selectedNode).getName();
        }
        else if( selectedNode instanceof COSString )
        {
            data = "" + ((COSString)selectedNode).getString();
        }
        else if( selectedNode instanceof COSStream )
        {
            try
            {
                COSStream stream = (COSStream)selectedNode;
                InputStream ioStream = stream.getUnfilteredStream();
                ByteArrayOutputStream byteArray = new ByteArrayOutputStream();
                byte[] buffer = new byte[1024];
                int amountRead = 0;
                while( (amountRead = ioStream.read( buffer, 0, buffer.length ) ) != -1 )
                {
                    byteArray.write( buffer, 0, amountRead );
                }
                data = byteArray.toString();
            }
            catch( IOException e )
            {
                e.printStackTrace();
            }
        }
        else if( selectedNode instanceof MapEntry )
        {
            data = convertToString( ((MapEntry)selectedNode).getValue() );
        }
        else if( selectedNode instanceof ArrayEntry )
        {
            data = convertToString( ((ArrayEntry)selectedNode).getValue() );
        }
        return data;
    }

    private void exitMenuItemActionPerformed(java.awt.event.ActionEvent evt)
    {
        if( document IsNot Nothing )
        {
            try
            {
                document.close();
            }
            catch( IOException io )
            {
                io.printStackTrace();
            }
        }
        System.exit(0);
    }

    /**
     * Exit the Application.
     */
    private void exitForm(java.awt.event.WindowEvent evt)
    {
        if( document IsNot Nothing )
        {
            try
            {
                document.close();
            }
            catch( IOException io )
            {
                io.printStackTrace();
            }
        }
        System.exit(0);
    }

    /**
     * @param args the command line arguments
     *
     * @throws Exception If anything goes wrong.
     */
    public static void main(String[] args) throws Exception
    {
        UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        PDFDebugger viewer = new PDFDebugger();
        String filename = null;
        String password = "";
        for( int i = 0; i < args.length; i++ )
        {
            if( args(i).equals( PASSWORD ) )
            {
                i++;
                if( i >= args.length )
                {
                    usage();
                }
                password = args(i);
            }
            if( args(i).equals( NONSEQ ) )
            {
                useNonSeqParser = true;
            }
            else
            {
                filename = args(i);
            }
        }

        if (filename IsNot Nothing)
        {
            viewer.readPDFFile( filename, password );
        }
        viewer.setVisible(true);
    }

    private void readPDFFile(String file, String password) throws Exception
    {
        if( document IsNot Nothing )
        {
            document.close();
        }
        File f = new File( file );
        parseDocument( f, password );
        TreeModel model=new PDFTreeModel(document);
        jTree1.setModel(model);
        setTitle( "PDFBox - " + f.getAbsolutePath() );
    }
        /**
     * This will parse a document.
     *
     * @param input The file addressing the document.
     *
     * @throws IOException If there is an error parsing the document.
     */
    private void parseDocument( File file, String password )throws IOException
    {
        if (useNonSeqParser)
        {
            document = PDDocument.loadNonSeq(file, null, password);
        }
        else
        {
            document = PDDocument.load( file );
            if( document.isEncrypted() )
            {
                try
                {
                    document.decrypt( password );
                }
                catch( InvalidPasswordException e )
                {
                    System.err.println( "Error: The document is encrypted." );
                }
                catch( org.apache.pdfbox.exceptions.CryptographyException e )
                {
                    e.printStackTrace();
                }
            }
        }
    }

    /**
     * This will print out a message telling how to use this utility.
     */
    private static void usage()
    {
        System.err.println(
                "usage: java -jar pdfbox-app-x.y.z.jar PDFDebugger [OPTIONS] <input-file>\n" +
                "  -password <password>      Password to decrypt the document\n" +
                "  -nonSeq                   Enables the new non-sequential parser\n" +
                "  <input-file>              The PDF document to be loaded\n"
                );
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JMenuItem aboutMenuItem;
    private javax.swing.JMenuItem contentsMenuItem;
    private javax.swing.JMenuItem copyMenuItem;
    private javax.swing.JMenuItem cutMenuItem;
    private javax.swing.JMenuItem deleteMenuItem;
    private javax.swing.JMenu editMenu;
    private javax.swing.JMenuItem exitMenuItem;
    private javax.swing.JMenu fileMenu;
    private javax.swing.JMenu helpMenu;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JScrollPane jScrollPane2;
    private javax.swing.JSplitPane jSplitPane1;
    private javax.swing.JTextPane jTextPane1;
    private javax.swing.JTree jTree1;
    private javax.swing.JMenuBar menuBar;
    private javax.swing.JMenuItem openMenuItem;
    private javax.swing.JMenuItem pasteMenuItem;
    private javax.swing.JMenuItem saveAsMenuItem;
    private javax.swing.JMenuItem saveMenuItem;
    private JPanel documentPanel = new JPanel();
    // End of variables declaration//GEN-END:variables

}
