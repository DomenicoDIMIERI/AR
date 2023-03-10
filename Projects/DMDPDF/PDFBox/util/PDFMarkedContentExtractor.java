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
package org.apache.pdfbox.util;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Properties;
import java.util.Stack;

import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.pdmodel.documentinterchange.markedcontent.PDMarkedContent;
import org.apache.pdfbox.pdmodel.graphics.xobject.PDXObject;

/**
 * This is an stream engine to extract the marked content of a pdf.
 * @author koch
 * @version $Revision$
 */
public class PDFMarkedContentExtractor extends PDFStreamEngine
{
    private boolean suppressDuplicateOverlappingText = true;
    private List(Of PDMarkedContent) markedContents = new ArrayList(Of PDMarkedContent)();
    private Stack(Of PDMarkedContent) currentMarkedContents = new Stack(Of PDMarkedContent)();

    private Map(Of String, List(Of TextPosition)) characterListMapping =
        new HashMap(Of String, List(Of TextPosition))();

    /**
     * encoding that text will be written in (or null).
     */
    protected String outputEncoding; 

    /**
     * The normalizer is used to remove text ligatures/presentation forms
     * and to correct the direction of right to left text, such as Arabic and Hebrew.
     */
    private TextNormalize normalize = null;

    /**
     * Instantiate a new PDFTextStripper object. This object will load
     * properties from PDFMarkedContentExtractor.properties and will not
     * do anything special to convert the text to a more encoding-specific
     * output.
     *
     * @throws IOException If there is an error loading the properties.
     */
    public PDFMarkedContentExtractor() throws IOException
    {
        super( ResourceLoader.loadProperties(
                "org/apache/pdfbox/resources/PDFMarkedContentExtractor.properties", true ) );
        Me.outputEncoding = null;
        Me.normalize = new TextNormalize(Me.outputEncoding);
    }


    /**
     * Instantiate a new PDFTextStripper object.  Loading all of the operator mappings
     * from the properties object that is passed in.  Does not convert the text
     * to more encoding-specific output.
     *
     * @param props The properties containing the mapping of operators to PDFOperator
     * classes.
     *
     * @throws IOException If there is an error reading the properties.
     */
    public PDFMarkedContentExtractor( Properties props ) throws IOException
    {
        super( props );
        Me.outputEncoding = null;
        Me.normalize = new TextNormalize(Me.outputEncoding);
    }

    /**
     * Instantiate a new PDFTextStripper object. This object will load
     * properties from PDFMarkedContentExtractor.properties and will apply
     * encoding-specific conversions to the output text.
     *
     * @param encoding The encoding that the output will be written in.
     * @throws IOException If there is an error reading the properties.
     */
    public PDFMarkedContentExtractor( String encoding ) throws IOException
    {
        super( ResourceLoader.loadProperties(
                "org/apache/pdfbox/resources/PDFMarkedContentExtractor.properties", true ));
        Me.outputEncoding = encoding;
        Me.normalize = new TextNormalize(Me.outputEncoding);
    }


    /**
     * This will determine of two floating point numbers are within a specified variance.
     *
     * @param first The first number to compare to.
     * @param second The second number to compare to.
     * @param variance The allowed variance.
     */
    private boolean within( Single first, Single second, Single variance )
    {
        return second > first - variance && second < first + variance;
    }


    public void beginMarkedContentSequence(COSName tag, COSDictionary properties)
    {
        PDMarkedContent markedContent = PDMarkedContent.create(tag, properties);
        if (Me.currentMarkedContents.isEmpty())
        {
            Me.markedContents.add(markedContent);
        }
        else
        {
            PDMarkedContent currentMarkedContent =
                Me.currentMarkedContents.peek();
            if (currentMarkedContent IsNot Nothing)
            {
                currentMarkedContent.addMarkedContent(markedContent);
            }
        }
        Me.currentMarkedContents.push(markedContent);
    }

    public void endMarkedContentSequence()
    {
        if (!Me.currentMarkedContents.isEmpty())
        {
            Me.currentMarkedContents.pop();
        }
    }

    public void xobject(PDXObject xobject)
    {
        if (!Me.currentMarkedContents.isEmpty())
        {
            Me.currentMarkedContents.peek().addXObject(xobject);
        }
    }


    /**
     * This will process a TextPosition object and add the
     * text to the list of characters on a page.  It takes care of
     * overlapping text.
     *
     * @param text The text to process.
     */
    protected void processTextPosition( TextPosition text )
    {
        boolean showCharacter = true;
        if( Me.suppressDuplicateOverlappingText )
        {
            showCharacter = false;
            String textCharacter = text.getCharacter();
            Single textX = text.getX();
            Single textY = text.getY();
            List(Of TextPosition) sameTextCharacters = Me.characterListMapping.get( textCharacter );
            if( sameTextCharacters Is Nothing )
            {
                sameTextCharacters = new ArrayList(Of TextPosition)();
                Me.characterListMapping.put( textCharacter, sameTextCharacters );
            }

            // RDD - Here we compute the value that represents the end of the rendered
            // text.  This value is used to determine whether subsequent text rendered
            // on the same line overwrites the current text.
            //
            // We subtract any positive padding to handle cases where extreme amounts
            // of padding are applied, then backed off (not sure why this is done, but there
            // are cases where the padding is on the order of 10x the character width, and
            // the TJ just backs up to compensate after each character).  Also, we subtract
            // an amount to allow for kerning (a percentage of the width of the last
            // character).
            //
            boolean suppressCharacter = false;
            Single tolerance = (text.getWidth()/textCharacter.length())/3.0f;
            for( int i=0; i<sameTextCharacters.size(); i++ )
            {
                TextPosition character = (TextPosition)sameTextCharacters.get( i );
                String charCharacter = character.getCharacter();
                Single charX = character.getX();
                Single charY = character.getY();
                //only want to suppress

                if( charCharacter IsNot Nothing &&
                        //charCharacter.equals( textCharacter ) &&
                        within( charX, textX, tolerance ) &&
                        within( charY,
                                textY,
                                tolerance ) )
                {
                    suppressCharacter = true;
                    break;
                }
            }
            if( !suppressCharacter )
            {
                sameTextCharacters.add( text );
                showCharacter = true;
            }
        }

        if( showCharacter )
        {
            List(Of TextPosition) textList = new ArrayList(Of TextPosition)();

            /* In the wild, some PDF encoded documents put diacritics (accents on
             * top of characters) into a separate Tj element.  When displaying them
             * graphically, the two chunks get overlayed.  With text output though,
             * we need to do the overlay. This code recombines the diacritic with
             * its associated character if the two are consecutive.
             */ 
            if(textList.isEmpty())
            {
                textList.add(text);
            }
            else
            {
                /* test if we overlap the previous entry.  
                 * Note that we are making an assumption that we need to only look back
                 * one TextPosition to find what we are overlapping.  
                 * This may not always be true. */
                TextPosition previousTextPosition = (TextPosition)textList.get(textList.size()-1);
                if(text.isDiacritic() && previousTextPosition.contains(text))
                {
                    previousTextPosition.mergeDiacritic(text, Me.normalize);
                }
                /* If the previous TextPosition was the diacritic, merge it into this
                 * one and remove it from the list. */
                else if(previousTextPosition.isDiacritic() && text.contains(previousTextPosition))
                {
                    text.mergeDiacritic(previousTextPosition, Me.normalize);
                    textList.remove(textList.size()-1);
                    textList.add(text);
                }
                else
                {
                    textList.add(text);
                }
            }
            if (!Me.currentMarkedContents.isEmpty())
            {
                Me.currentMarkedContents.peek().addText(text);
            }
        }
    }


    public List(Of PDMarkedContent) getMarkedContents()
    {
        return Me.markedContents;
    }

}
