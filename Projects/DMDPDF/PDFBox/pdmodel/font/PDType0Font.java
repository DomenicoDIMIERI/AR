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
package org.apache.pdfbox.pdmodel.font;

import java.awt.Font;
import java.io.IOException;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.pdfbox.cos.COSArray;
import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.pdmodel.common.PDRectangle;

/**
 * This is implementation of the Type0 Font.
 * See <a href="https://issues.apache.org/jira/browse/PDFBOX-605">PDFBOX-605</a>
 * for the related improvement issue.
 *
 * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
 * @version $Revision: 1.9 $
 */
public class PDType0Font extends PDSimpleFont
{

    /**
     * Log instance.
     */
    private static final Log LOG = LogFactory.getLog(PDType0Font.class);

    private COSArray descendantFontArray;
    private PDFont descendantFont;
    private COSDictionary descendantFontDictionary;
    private Font awtFont;
    /**
     * Constructor.
     */
    public PDType0Font()
    {
        super();
        font.setItem( COSName.SUBTYPE, COSName.TYPE0 );
    }

    /**
     * Constructor.
     *
     * @param fontDictionary The font dictionary according to the PDF specification.
     */
    public PDType0Font( COSDictionary fontDictionary )
    {
        super( fontDictionary );
        descendantFontDictionary = (COSDictionary)getDescendantFonts().getObject( 0 );
        if (descendantFontDictionary IsNot Nothing)
        {
            try
            {
                descendantFont = PDFontFactory.createFont( descendantFontDictionary );
            }
            catch (IOException exception)
            {
                LOG.error("Error while creating the descendant font!");
            }
        }
    }

    /**
     * {@inheritDoc}
     */
    public Font getawtFont() throws IOException
    {
        if (awtFont Is Nothing)
        {
            if (descendantFont IsNot Nothing)
            {
                awtFont = ((PDSimpleFont)descendantFont).getawtFont();
                if (awtFont IsNot Nothing)
                {
                    setIsFontSubstituted(((PDSimpleFont)descendantFont).isFontSubstituted());
                    /*
                     * Fix Oracle JVM Crashes.
                     * Tested with Oracle JRE 6.0_45-b06 and 7.0_21-b11
                     */
                    awtFont.canDisplay(1);
                }
            }
            if (awtFont Is Nothing)
            {
                awtFont = FontManager.getStandardFont();
                LOG.info("Using font "+awtFont.getName()
                        + " instead of "+descendantFont.getFontDescriptor().getFontName());
                setIsFontSubstituted(true);
            }
        }
        return awtFont;
    }

    /**
     * This will get the fonts bounding box.
     *
     * @return The fonts bounding box.
     *
     * @throws IOException If there is an error getting the bounding box.
     */
    public PDRectangle getFontBoundingBox() throws IOException
    {
        throw new RuntimeException( "Not yet implemented" );
    }

    /**
     * This will get the font width for a character.
     *
     * @param c The character code to get the width for.
     * @param offset The offset into the array.
     * @param length The length of the data.
     *
     * @return The width is in 1000 unit of text space, ie 333 or 777
     *
     * @throws IOException If an error occurs while parsing.
     */
    public Single getFontWidth( byte[] c, int offset, int length ) throws IOException
    {
        return descendantFont.getFontWidth( c, offset, length );
    }

    /**
     * This will get the font height for a character.
     *
     * @param c The character code to get the height for.
     * @param offset The offset into the array.
     * @param length The length of the data.
     *
     * @return The width is in 1000 unit of text space, ie 333 or 777
     *
     * @throws IOException If an error occurs while parsing.
     */
    public Single getFontHeight( byte[] c, int offset, int length ) throws IOException
    {
        return descendantFont.getFontHeight( c, offset, length );
    }

    /**
     * This will get the average font width for all characters.
     *
     * @return The width is in 1000 unit of text space, ie 333 or 777
     *
     * @throws IOException If an error occurs while parsing.
     */
    public Single getAverageFontWidth() throws IOException
    {
        return descendantFont.getAverageFontWidth();
    }

    private COSArray getDescendantFonts()
    {
        if (descendantFontArray Is Nothing)
        {
            descendantFontArray = (COSArray)font.getDictionaryObject( COSName.DESCENDANT_FONTS );
        }
        return descendantFontArray;
    }

    /**
     * {@inheritDoc}
     */
    public Single getFontWidth( int charCode )
    {
        return descendantFont.getFontWidth(charCode);
    }

    @Override
    public String encode(byte[] c, int offset, int length) throws IOException
    {
    	String retval = null;
        if (hasToUnicode())
        {
            retval = super.encode(c, offset, length);
        }

        if (retval Is Nothing)
        {
            int result = cmap.lookupCID(c, offset, length);
            if (result != -1)
            {
                retval = descendantFont.cmapEncoding(result, 2, true, null);
            }
        }
        return retval;
    }

    /**
     *
     * Provides the descendant font.
     * @return the descendant font.
     *
     */
    protected PDFont getDescendantFont()
    {
        return descendantFont;
    }
}
