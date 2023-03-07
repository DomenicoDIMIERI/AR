Imports System.Drawing
Imports System.Drawing.Image
Imports System.IO

Imports FinSeA.org.apache.fontbox.util
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.util


Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * This class will handle creating an image for a type 3 glyph.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.10 $
    ' */
    Public Class Type3StreamParser
        Inherits PDFStreamEngine

        Private image As PDInlinedImage = Nothing
        Private box As BoundingBox = Nothing


        '/**
        ' * This will parse a type3 stream and create an image from it.
        ' *
        ' * @param type3Stream The stream containing the operators to draw the image.
        ' *
        ' * @return The image that was created.
        ' *
        ' * @throws IOException If there is an error processing the stream.
        ' */
        Public Function createImage(ByVal type3Stream As COSStream) As FinSeA.Drawings.BufferedImage
            processStream(Nothing, Nothing, type3Stream)
            Return image.createImage()
        End Function

        '/**
        ' * This is used to handle an operation.
        ' *
        ' * @param operator The operation to perform.
        ' * @param arguments The list of arguments.
        ' *
        ' * @throws IOException If there is an error processing the operation.
        ' */
        Protected Overrides Sub processOperator(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase)) 'throws IOException
            MyBase.processOperator([operator], arguments)
            Dim operation As String = [operator].getOperation()
            '/**
            '    If (operation.equals("b")) Then
            '        'Close, fill, and stroke path using nonzero winding number rule
            '    ElseIf (operation.equals("B")) Then
            '        'Fill and stroke path using nonzero winding number rule
            '    ElseIf (operation.equals("b*")) Then
            '        'Close, fill, and stroke path using even-odd rule
            '    ElseIf (operation.equals("B*")) Then
            '        'Fill and stroke path using even-odd rule
            '    ElseIf (operation.equals("BDC")) Then
            '        '(PDF 1.2) Begin marked-content sequence with property list
            'else **/
            If (operation.Equals("BI")) Then
                Dim params As ImageParameters = [operator].getImageParameters()
                image = New PDInlinedImage()
                image.setImageParameters(params)
                image.setImageData([operator].getImageData())
                'begin inline image object
            End If
            '/**
            '            ElseIf (operation.Equals("BMC")) Then
            '        {
            '            //(PDF 1.2) Begin marked-content sequence
            '        }
            '            ElseIf (operation.Equals("BT")) Then
            '        {
            '            log.debug( "<BT>" );
            '            textMatrix = new Matrix();
            '            textLineMatrix = new Matrix();
            '        }
            '            ElseIf (operation.Equals("BX")) Then
            '        {
            '            //(PDF 1.1) Begin compatibility section
            '        }
            '            ElseIf (operation.Equals("c")) Then
            '        {
            '            //Append curved segment to path (three control points)
            '        }
            '            ElseIf (operation.Equals("cm")) Then
            '        {
            '        }
            '            ElseIf (operation.Equals("cs")) Then
            '        {
            '        }
            '            ElseIf (operation.Equals("CS")) Then
            '        {
            '        }
            '            ElseIf (operation.Equals("d")) Then
            '        {
            '            //Set the line dash pattern in the graphics state
            '        }
            '        else */
            If (operation.Equals("d0")) Then
                'set glyph with for a type3 font
                'COSNumber horizontalWidth = (COSNumber)arguments.get( 0 );
                'COSNumber verticalWidth = (COSNumber)arguments.get( 1 );
                'width = horizontalWidth.intValue();
                'height = verticalWidth.intValue();
            ElseIf (operation.Equals("d1")) Then

                'set glyph with and bounding box for type 3 font
                'COSNumber horizontalWidth = (COSNumber)arguments.get( 0 );
                'COSNumber verticalWidth = (COSNumber)arguments.get( 1 );
                Dim llx As COSNumber = arguments.get(2)
                Dim lly As COSNumber = arguments.get(3)
                Dim urx As COSNumber = arguments.get(4)
                Dim ury As COSNumber = arguments.get(5)

                'width = horizontalWidth.intValue();
                'height = verticalWidth.intValue();
                box = New BoundingBox()
                box.setLowerLeftX(llx.floatValue())
                box.setLowerLeftY(lly.floatValue())
                box.setUpperRightX(urx.floatValue())
                box.setUpperRightY(ury.floatValue())
            End If
            '/*
            '    ElseIf (operation.Equals("Do")) Then
            '{
            '    //invoke named object.
            '}
            '    ElseIf (operation.Equals("DP")) Then
            '{
            '    //(PDF 1.2) De.ne marked-content point with property list
            '}
            '    ElseIf (operation.Equals("EI")) Then
            '{
            '    //end inline image object
            '}
            '    ElseIf (operation.Equals("EMC")) Then
            '{
            '    //End inline image object
            '}
            '    ElseIf (operation.Equals("ET")) Then
            '{
            '    log.debug( "<ET>" );
            '    textMatrix = null;
            '    textLineMatrix = null;
            '}
            '    ElseIf (operation.Equals("EX")) Then
            '{
            '    //(PDF 1.1) End compatibility section
            '}
            '    ElseIf (operation.Equals("f")) Then
            '{
            '    //Fill the path, using the nonzero winding number rule to determine the region to .ll
            '}
            '    ElseIf (operation.Equals("F")) Then
            '{
            '}
            '    ElseIf (operation.Equals("f*")) Then
            '{
            '    //Fill path using even-odd rule
            '}
            '    ElseIf (operation.Equals("g")) Then
            '{
            '}
            '    ElseIf (operation.Equals("G")) Then
            '{
            '}
            '    ElseIf (operation.Equals("gs")) Then
            '{
            '}
            '    ElseIf (operation.Equals("h")) Then
            '{
            '    //close subpath
            '}
            '    ElseIf (operation.Equals("i")) Then
            '{
            '    //set flatness tolerance, not sure what this does
            '}
            '    ElseIf (operation.Equals("ID")) Then
            '{
            '    //begin inline image data
            '}
            '    ElseIf (operation.Equals("j")) Then
            '{
            '    //Set the line join style in the graphics state
            '    //System.out.println( "<j>" );
            '}
            '    ElseIf (operation.Equals("J")) Then
            '{
            '    //Set the line cap style in the graphics state
            '    //System.out.println( "<J>" );
            '}
            '    ElseIf (operation.Equals("k")) Then
            '{
            '    //Set CMYK color for nonstroking operations
            '}
            '    ElseIf (operation.Equals("K")) Then
            '{
            '    //Set CMYK color for stroking operations
            '}
            '    ElseIf (operation.Equals("l")) Then
            '{
            '    //append straight line segment from the current point to the point.
            '    COSNumber x = (COSNumber)arguments.get( 0 );
            '    COSNumber y = (COSNumber)arguments.get( 1 );
            '    linePath.lineTo( x.floatValue(), pageSize.getHeight()-y.floatValue() );
            '}
            '    ElseIf (operation.Equals("m")) Then
            '{
            '    COSNumber x = (COSNumber)arguments.get( 0 );
            '    COSNumber y = (COSNumber)arguments.get( 1 );
            '    linePath.reset();
            '    linePath.moveTo( x.floatValue(), pageSize.getHeight()-y.floatValue() );
            '    //System.out.println( "<m x=\"" + x.getValue() + "\" y=\"" + y.getValue() + "\" >" );
            '}
            '    ElseIf (operation.Equals("M")) Then
            '{
            '    //System.out.println( "<M>" );
            '}
            '    ElseIf (operation.Equals("MP")) Then
            '{
            '    //(PDF 1.2) Define marked-content point
            '}
            '    ElseIf (operation.Equals("n")) Then
            '{
            '    //End path without .lling or stroking
            '    //System.out.println( "<n>" );
            '}
            '    ElseIf (operation.Equals("q")) Then
            '{
            '    //save graphics state
            '        If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug( "<" + operation + "> - save state" );
            '    }
            '    graphicsStack.push(graphicsState.clone());
            '}
            '        ElseIf (operation.Equals("Q")) Then
            '{
            '    //restore graphics state
            '            If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug( "<" + operation + "> - restore state" );
            '    }
            '    graphicsState = (PDGraphicsState)graphicsStack.pop();
            '}
            '            ElseIf (operation.Equals("re")) Then
            '{
            '}
            '            ElseIf (operation.Equals("rg")) Then
            '{
            '    //Set RGB color for nonstroking operations
            '}
            '            ElseIf (operation.Equals("RG")) Then
            '{
            '    //Set RGB color for stroking operations
            '}
            '            ElseIf (operation.Equals("ri")) Then
            '{
            '    //Set color rendering intent
            '}
            '            ElseIf (operation.Equals("s")) Then
            '{
            '    //Close and stroke path
            '}
            '            ElseIf (operation.Equals("S")) Then
            '{
            '    graphics.draw( linePath );
            '}
            '            ElseIf (operation.Equals("sc")) Then
            '{
            '    //set color for nonstroking operations
            '    //System.out.println( "<sc>" );
            '}
            '            ElseIf (operation.Equals("SC")) Then
            '{
            '    //set color for stroking operations
            '    //System.out.println( "<SC>" );
            '}
            '            ElseIf (operation.Equals("scn")) Then
            '{
            '    //set color for nonstroking operations special
            '}
            '            ElseIf (operation.Equals("SCN")) Then
            '{
            '    //set color for stroking operations special
            '}
            '            ElseIf (operation.Equals("sh")) Then
            '{
            '    //(PDF 1.3) Paint area de.ned by shading pattern
            '}
            '            ElseIf (operation.Equals("T*")) Then
            '{
            '                If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<T* graphicsState.getTextState().getLeading()=\"" +
            '                    GraphicsState.getTextState().getLeading(+"\" > ");")
            '    }
            '    //move to start of next text line
            '    if( graphicsState.getTextState().getLeading() == 0 )
            '    {
            '        graphicsState.getTextState().setLeading( -.01f );
            '    }
            '    Matrix td = new Matrix();
            '    td.setValue( 2, 1, -1 * graphicsState.getTextState().getLeading() * textMatrix.getValue(1,1));
            '    textLineMatrix = textLineMatrix.multiply( td );
            '    textMatrix = textLineMatrix.copy();
            '}
            '                    ElseIf (operation.Equals("Tc")) Then
            '{
            '    //set character spacing
            '    COSNumber characterSpacing = (COSNumber)arguments.get( 0 );
            '                        If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<Tc characterSpacing=\"" + characterSpacing.floatValue() + "\" />");
            '    }
            '    graphicsState.getTextState().setCharacterSpacing( characterSpacing.floatValue() );
            '}
            '                        ElseIf (operation.Equals("Td")) Then
            '{
            '    COSNumber x = (COSNumber)arguments.get( 0 );
            '    COSNumber y = (COSNumber)arguments.get( 1 );
            '                            If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<Td x=\"" + x.floatValue() + "\" y=\"" + y.floatValue() + "\">");
            '    }
            '    Matrix td = new Matrix();
            '    td.setValue( 2, 0, x.floatValue() * textMatrix.getValue(0,0) );
            '    td.setValue( 2, 1, y.floatValue() * textMatrix.getValue(1,1) );
            '    //log.debug( "textLineMatrix before " + textLineMatrix );
            '    textLineMatrix = textLineMatrix.multiply( td );
            '    //log.debug( "textLineMatrix after " + textLineMatrix );
            '    textMatrix = textLineMatrix.copy();
            '}
            '                            ElseIf (operation.Equals("TD")) Then
            '{
            '    //move text position and set leading
            '    COSNumber x = (COSNumber)arguments.get( 0 );
            '    COSNumber y = (COSNumber)arguments.get( 1 );
            '                                If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<TD x=\"" + x.floatValue() + "\" y=\"" + y.floatValue() + "\">");
            '    }
            '    graphicsState.getTextState().setLeading( -1 * y.floatValue() );
            '    Matrix td = new Matrix();
            '    td.setValue( 2, 0, x.floatValue() * textMatrix.getValue(0,0) );
            '    td.setValue( 2, 1, y.floatValue() * textMatrix.getValue(1,1) );
            '    //log.debug( "textLineMatrix before " + textLineMatrix );
            '    textLineMatrix = textLineMatrix.multiply( td );
            '    //log.debug( "textLineMatrix after " + textLineMatrix );
            '    textMatrix = textLineMatrix.copy();
            '}
            '                                ElseIf (operation.Equals("Tf")) Then
            '{
            '    //set font and size
            '    COSName fontName = (COSName)arguments.get( 0 );
            '    graphicsState.getTextState().setFontSize( ((COSNumber)arguments.get( 1 ) ).floatValue() );

            '                                    If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<Tf font=\"" + fontName.getName() + "\" size=\"" +
            '                                        GraphicsState.getTextState().getFontSize(+"\" > ");")
            '    }

            '    //old way
            '    //graphicsState.getTextState().getFont() = (COSObject)stream.getDictionaryObject( fontName );
            '    //if( graphicsState.getTextState().getFont() Is Nothing )
            '    //{
            '    //    graphicsState.getTextState().getFont() = (COSObject)graphicsState.getTextState().getFont()
            '    //                                           Dictionary.getItem( fontName );
            '    //}
            '    graphicsState.getTextState().setFont( (PDFont)fonts.get( fontName.getName() ) );
            '    if( graphicsState.getTextState().getFont() Is Nothing )
            '    {
            '        throw new IOException( "Error: Could not find font(" + fontName + ") in map=" + fonts );
            '    }
            '    //log.debug( "Font Resource=" + fontResource );
            '    //log.debug( "Current Font=" + graphicsState.getTextState().getFont() );
            '    //log.debug( "graphicsState.getTextState().getFontSize()=" + graphicsState.getTextState().getFontSize() );
            '}
            '                                        ElseIf (operation.Equals("Tj")) Then
            '{
            '    COSString string = (COSString)arguments.get( 0 );
            '    TextPosition pos = showString( string.getBytes() );
            '                                            If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<Tj string=\"" + string.getString() + "\">");
            '    }
            '}
            '                                            ElseIf (operation.Equals("TJ")) Then
            '{
            '    Matrix td = new Matrix();

            '    COSArray array = (COSArray)arguments.get( 0 );
            '    for( int i=0; i<array.size(); i++ )
            '    {
            '        COSBase next = array.get( i );
            '        if( next instanceof COSNumber )
            '        {
            '            Single value = -1*
            '                          (((COSNumber)next).floatValue()/1000) *
            '                          graphicsState.getTextState().getFontSize() *
            '                          textMatrix.getValue(1,1);

            '                                                        If (LOG.isDebugEnabled()) Then
            '            {
            '                log.debug( "<TJ(" + i + ") value=\"" + value +
            '                           "\", param=\"" + ((COSNumber)next).floatValue() +
            '                           "\", fontsize=\"" + graphicsState.getTextState().getFontSize() + "\">" );
            '            }
            '            td.setValue( 2, 0, value );
            '            textMatrix = textMatrix.multiply( td );
            '        }
            '        else if( next instanceof COSString )
            '        {
            '            TextPosition pos = showString( ((COSString)next).getBytes() );
            '                                                            If (LOG.isDebugEnabled()) Then
            '            {
            '                log.debug("<TJ(" + i + ") string=\"" + pos.getString() + "\">");
            '            }
            '        }
            '                                                            Else
            '        {
            '            throw new IOException( "Unknown type in array for TJ operation:" + next );
            '        }
            '    }
            '}
            'else if( operation.equals( "TL" ) )
            '{
            '    COSNumber leading = (COSNumber)arguments.get( 0 );
            '    graphicsState.getTextState().setLeading( leading.floatValue() );
            '                                                                If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<TL leading=\"" + leading.floatValue() + "\" >");
            '    }
            '}
            '                                                                ElseIf (operation.Equals("Tm")) Then
            '{
            '    //Set text matrix and text line matrix
            '    COSNumber a = (COSNumber)arguments.get( 0 );
            '    COSNumber b = (COSNumber)arguments.get( 1 );
            '    COSNumber c = (COSNumber)arguments.get( 2 );
            '    COSNumber d = (COSNumber)arguments.get( 3 );
            '    COSNumber e = (COSNumber)arguments.get( 4 );
            '    COSNumber f = (COSNumber)arguments.get( 5 );

            '                                                                    If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<Tm " +
            '                  "a=\"" + a.floatValue() + "\" " +
            '                  "b=\"" + b.floatValue() + "\" " +
            '                  "c=\"" + c.floatValue() + "\" " +
            '                  "d=\"" + d.floatValue() + "\" " +
            '                  "e=\"" + e.floatValue() + "\" " +
            '                  "f=\"" + f.floatValue() + "\" >");
            '    }

            '    textMatrix = new Matrix();
            '    textMatrix.setValue( 0, 0, a.floatValue() );
            '    textMatrix.setValue( 0, 1, b.floatValue() );
            '    textMatrix.setValue( 1, 0, c.floatValue() );
            '    textMatrix.setValue( 1, 1, d.floatValue() );
            '    textMatrix.setValue( 2, 0, e.floatValue() );
            '    textMatrix.setValue( 2, 1, f.floatValue() );
            '    textLineMatrix = textMatrix.copy();
            '}
            '                                                                    ElseIf (operation.Equals("Tr")) Then
            '{
            '    //Set text rendering mode
            '    //System.out.println( "<Tr>" );
            '}
            '                                                                    ElseIf (operation.Equals("Ts")) Then
            '{
            '    //Set text rise
            '    //System.out.println( "<Ts>" );
            '}
            '                                                                    ElseIf (operation.Equals("Tw")) Then
            '{
            '    //set word spacing
            '    COSNumber wordSpacing = (COSNumber)arguments.get( 0 );
            '                                                                        If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<Tw wordSpacing=\"" + wordSpacing.floatValue() + "\" />");
            '    }
            '    graphicsState.getTextState().setWordSpacing( wordSpacing.floatValue() );
            '}
            '                                                                        ElseIf (operation.Equals("Tz")) Then
            '{
            '    //Set horizontal text scaling
            '}
            '                                                                        ElseIf (operation.Equals("v")) Then
            '{
            '    //Append curved segment to path (initial point replicated)
            '}
            '                                                                        ElseIf (operation.Equals("w")) Then
            '{
            '    //Set the line width in the graphics state
            '    //System.out.println( "<w>" );
            '}
            '                                                                        ElseIf (operation.Equals("W")) Then
            '{
            '    //Set clipping path using nonzero winding number rule
            '    //System.out.println( "<W>" );
            '}
            '                                                                        ElseIf (operation.Equals("W*")) Then
            '{
            '    //Set clipping path using even-odd rule
            '}
            '                                                                        ElseIf (operation.Equals("y")) Then
            '{
            '    //Append curved segment to path (final point replicated)
            '}
            '                                                                        ElseIf (operation.Equals("'")) Then
            '{
            '    // Move to start of next text line, and show text
            '    //
            '    COSString string = (COSString)arguments.get( 0 );
            '                                                                            If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<' string=\"" + string.getString() + "\">");
            '    }

            '    Matrix td = new Matrix();
            '    td.setValue( 2, 1, -1 * graphicsState.getTextState().getLeading() * textMatrix.getValue(1,1));
            '    textLineMatrix = textLineMatrix.multiply( td );
            '    textMatrix = textLineMatrix.copy();

            '    showString( string.getBytes() );
            '}
            'else if( operation.equals( "\"" ) )
            '{
            '    //Set word and character spacing, move to next line, and show text
            '    //
            '    COSNumber wordSpacing = (COSNumber)arguments.get( 0 );
            '    COSNumber characterSpacing = (COSNumber)arguments.get( 1 );
            '    COSString string = (COSString)arguments.get( 2 );

            '                                                                                If (LOG.isDebugEnabled()) Then
            '    {
            '        log.debug("<\" wordSpacing=\"" + wordSpacing +
            '                  "\", characterSpacing=\"" + characterSpacing +
            '                  "\", string=\"" + string.getString() + "\">");
            '    }

            '    graphicsState.getTextState().setCharacterSpacing( characterSpacing.floatValue() );
            '    graphicsState.getTextState().setWordSpacing( wordSpacing.floatValue() );

            '    Matrix td = new Matrix();
            '    td.setValue( 2, 1, -1 * graphicsState.getTextState().getLeading() * textMatrix.getValue(1,1));
            '    textLineMatrix = textLineMatrix.multiply( td );
            '    textMatrix = textLineMatrix.copy();

            '    showString( string.getBytes() );
            '}*/
        End Sub


    End Class

End Namespace
