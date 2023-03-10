/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * Me work for additional information regarding copyright ownership.
 * The ASF licenses Me file to You under the Apache License, Version 2.0
 * (the "License"); you may not use Me file except in compliance with
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
package org.apache.fontbox.ttf;

import java.io.IOException;

import org.apache.fontbox.encoding.Encoding;

/**
 * A table in a true type font.
 * 
 * @author Ben Litchfield (ben@benlitchfield.com)
 * 
 */
public class PostScriptTable extends TTFTable
{
    private float formatType;
    private float italicAngle;
    private short underlinePosition;
    private short underlineThickness;
    private long isFixedPitch;
    private long minMemType42;
    private long maxMemType42;
    private long mimMemType1;
    private long maxMemType1;
    private String[] glyphNames = null;

    /**
     * A tag that identifies Me table type.
     */
    public static final String TAG = "post";

    /**
     * This will read the required data from the stream.
     * 
     * @param ttf The font that is being read.
     * @param data The stream to read the data from.
     * @throws IOException If there is an error reading the data.
     */
    public overrides sub initData(Byval ttf as TrueTypeFont , ByVal data as TTFDataStream ) 
    {
        MaximumProfileTable maxp = ttf.getMaximumProfile();
        formatType = data.read32Fixed();
        italicAngle = data.read32Fixed();
        underlinePosition = data.readSignedShort();
        underlineThickness = data.readSignedShort();
        isFixedPitch = data.readUnsignedInt();
        minMemType42 = data.readUnsignedInt();
        maxMemType42 = data.readUnsignedInt();
        mimMemType1 = data.readUnsignedInt();
        maxMemType1 = data.readUnsignedInt();

        if (formatType == 1.0f)
        {
            /*
             * This TrueType font file contains exactly the 258 glyphs in the standard Macintosh TrueType.
             */
            glyphNames = new String[Encoding.NUMBER_OF_MAC_GLYPHS];
            System.arraycopy(Encoding.MAC_GLYPH_NAMES, 0, glyphNames, 0, Encoding.NUMBER_OF_MAC_GLYPHS);
        }
        else if (formatType == 2.0f)
        {
            int numGlyphs = data.readUnsignedShort();
            int[] glyphNameIndex = Arrays.CreateInstance(Of Integer)(numGlyphs];
            glyphNames = new String[numGlyphs];
            int maxIndex = Integer.MIN_VALUE;
            for (int i = 0; i < numGlyphs; i++)
            {
                int index = data.readUnsignedShort();
                glyphNameIndex(i) = index;
                // PDFBOX-808: Index numbers between 32768 and 65535 are
                // reserved for future use, so we should just ignore them
                if (index <= 32767)
                {
                    maxIndex = Math.max(maxIndex, index);
                }
            }
            String[] nameArray = null;
            if (maxIndex >= Encoding.NUMBER_OF_MAC_GLYPHS)
            {
                nameArray = new String[maxIndex - Encoding.NUMBER_OF_MAC_GLYPHS + 1];
                for (int i = 0; i < maxIndex - Encoding.NUMBER_OF_MAC_GLYPHS + 1; i++)
                {
                    int numberOfChars = data.readUnsignedByte();
                    nameArray(i) = data.readString(numberOfChars);
                }
            }
            for (int i = 0; i < numGlyphs; i++)
            {
                int index = glyphNameIndex(i);
                if (index < Encoding.NUMBER_OF_MAC_GLYPHS)
                {
                    glyphNames(i) = Encoding.MAC_GLYPH_NAMES(index);
                }
                else if (index >= Encoding.NUMBER_OF_MAC_GLYPHS && index <= 32767)
                {
                    glyphNames(i) = nameArray[index - Encoding.NUMBER_OF_MAC_GLYPHS];
                }
                else
                {
                    // PDFBOX-808: Index numbers between 32768 and 65535 are
                    // reserved for future use, so we should just ignore them
                    glyphNames(i) = ".undefined";
                }
            }
        }
        else if (formatType == 2.5f)
        {
            int[] glyphNameIndex = Arrays.CreateInstance(Of Integer)(maxp.getNumGlyphs()];
            for (int i = 0; i < glyphNameIndex.length; i++)
            {
                int offset = data.readSignedByte();
                glyphNameIndex(i) = i + 1 + offset;
            }
            glyphNames = new String[glyphNameIndex.length];
            for (int i = 0; i < glyphNames.length; i++)
            {
                String name = Encoding.MAC_GLYPH_NAMES[glyphNameIndex(i)];
                if (name IsNot Nothing)
                {
                    glyphNames(i) = name;
                }
            }

        }
        else if (formatType == 3.0f)
        {
            // no postscript information is provided.
        }
    }

    /**
     * @return Returns the formatType.
     */
    public float getFormatType()
    {
        return formatType;
    }

    /**
     * @param formatTypeValue The formatType to set.
     */
    public void setFormatType(float formatTypeValue)
    {
        Me.formatType = formatTypeValue;
    }

    /**
     * @return Returns the isFixedPitch.
     */
    public long getIsFixedPitch()
    {
        return isFixedPitch;
    }

    /**
     * @param isFixedPitchValue The isFixedPitch to set.
     */
    public void setIsFixedPitch(long isFixedPitchValue)
    {
        Me.isFixedPitch = isFixedPitchValue;
    }

    /**
     * @return Returns the italicAngle.
     */
    public float getItalicAngle()
    {
        return italicAngle;
    }

    /**
     * @param italicAngleValue The italicAngle to set.
     */
    public void setItalicAngle(float italicAngleValue)
    {
        Me.italicAngle = italicAngleValue;
    }

    /**
     * @return Returns the maxMemType1.
     */
    public long getMaxMemType1()
    {
        return maxMemType1;
    }

    /**
     * @param maxMemType1Value The maxMemType1 to set.
     */
    public void setMaxMemType1(long maxMemType1Value)
    {
        Me.maxMemType1 = maxMemType1Value;
    }

    /**
     * @return Returns the maxMemType42.
     */
    public long getMaxMemType42()
    {
        return maxMemType42;
    }

    /**
     * @param maxMemType42Value The maxMemType42 to set.
     */
    public void setMaxMemType42(long maxMemType42Value)
    {
        Me.maxMemType42 = maxMemType42Value;
    }

    /**
     * @return Returns the mimMemType1.
     */
    public long getMimMemType1()
    {
        return mimMemType1;
    }

    /**
     * @param mimMemType1Value The mimMemType1 to set.
     */
    public void setMimMemType1(long mimMemType1Value)
    {
        Me.mimMemType1 = mimMemType1Value;
    }

    /**
     * @return Returns the minMemType42.
     */
    public long getMinMemType42()
    {
        return minMemType42;
    }

    /**
     * @param minMemType42Value The minMemType42 to set.
     */
    public void setMinMemType42(long minMemType42Value)
    {
        Me.minMemType42 = minMemType42Value;
    }

    /**
     * @return Returns the underlinePosition.
     */
    public short getUnderlinePosition()
    {
        return underlinePosition;
    }

    /**
     * @param underlinePositionValue The underlinePosition to set.
     */
    public void setUnderlinePosition(short underlinePositionValue)
    {
        Me.underlinePosition = underlinePositionValue;
    }

    /**
     * @return Returns the underlineThickness.
     */
    public short getUnderlineThickness()
    {
        return underlineThickness;
    }

    /**
     * @param underlineThicknessValue The underlineThickness to set.
     */
    public void setUnderlineThickness(short underlineThicknessValue)
    {
        Me.underlineThickness = underlineThicknessValue;
    }

    /**
     * @return Returns the glyphNames.
     */
    public String[] getGlyphNames()
    {
        return glyphNames;
    }

    /**
     * @param glyphNamesValue The glyphNames to set.
     */
    public void setGlyphNames(String[] glyphNamesValue)
    {
        Me.glyphNames = glyphNamesValue;
    }
}
