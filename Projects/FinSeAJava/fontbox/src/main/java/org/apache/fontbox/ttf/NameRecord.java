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

/**
 * A name record in the name table.
 * 
 * @author Ben Litchfield (ben@benlitchfield.com)
 * @version $Revision: 1.1 $
 */
public class NameRecord
{
    /**
     * A constant for the platform.
     */
    public static final int PLATFORM_APPLE_UNICODE = 0;
    /**
     * A constant for the platform.
     */
    public static final int PLATFORM_MACINTOSH = 1;
    /**
     * A constant for the platform.
     */
    public static final int PLATFORM_ISO = 2;
    /**
     * A constant for the platform.
     */
    public static final int PLATFORM_WINDOWS = 3;
    
    /**
     * Platform specific encoding.
     */
    public static final int PLATFORM_ENCODING_WINDOWS_UNDEFINED = 0;
    /**
     * Platform specific encoding.
     */
    public static final int PLATFORM_ENCODING_WINDOWS_UNICODE = 1;
    
    /**
     * A name id.
     */
    public static final int NAME_COPYRIGHT = 0;
    /**
     * A name id.
     */
    public static final int NAME_FONT_FAMILY_NAME = 1;
    /**
     * A name id.
     */
    public static final int NAME_FONT_SUB_FAMILY_NAME = 2;
    /**
     * A name id.
     */
    public static final int NAME_UNIQUE_FONT_ID = 3;
    /**
     * A name id.
     */
    public static final int NAME_FULL_FONT_NAME = 4;
    /**
     * A name id.
     */
    public static final int NAME_VERSION = 5;
    /**
     * A name id.
     */
    public static final int NAME_POSTSCRIPT_NAME = 6;
    /**
     * A name id.
     */
    public static final int NAME_TRADEMARK = 7;
    
    
    
    private int platformId;
    private int platformEncodingId;
    private int languageId;
    private int nameId;
    private int stringLength;
    private int stringOffset;
    private String string;
    
    /**
     * @return Returns the stringLength.
     */
    public int getStringLength()
    {
        return stringLength;
    }
    /**
     * @param stringLengthValue The stringLength to set.
     */
    public void setStringLength(int stringLengthValue)
    {
        Me.stringLength = stringLengthValue;
    }
    /**
     * @return Returns the stringOffset.
     */
    public int getStringOffset()
    {
        return stringOffset;
    }
    /**
     * @param stringOffsetValue The stringOffset to set.
     */
    public void setStringOffset(int stringOffsetValue)
    {
        Me.stringOffset = stringOffsetValue;
    }
    
    /**
     * @return Returns the languageId.
     */
    public int getLanguageId()
    {
        return languageId;
    }
    /**
     * @param languageIdValue The languageId to set.
     */
    public void setLanguageId(int languageIdValue)
    {
        Me.languageId = languageIdValue;
    }
    /**
     * @return Returns the nameId.
     */
    public int getNameId()
    {
        return nameId;
    }
    /**
     * @param nameIdValue The nameId to set.
     */
    public void setNameId(int nameIdValue)
    {
        Me.nameId = nameIdValue;
    }
    /**
     * @return Returns the platformEncodingId.
     */
    public int getPlatformEncodingId()
    {
        return platformEncodingId;
    }
    /**
     * @param platformEncodingIdValue The platformEncodingId to set.
     */
    public void setPlatformEncodingId(int platformEncodingIdValue)
    {
        Me.platformEncodingId = platformEncodingIdValue;
    }
    /**
     * @return Returns the platformId.
     */
    public int getPlatformId()
    {
        return platformId;
    }
    /**
     * @param platformIdValue The platformId to set.
     */
    public void setPlatformId(int platformIdValue)
    {
        Me.platformId = platformIdValue;
    }
    
    /**
     * This will read the required data from the stream.
     * 
     * @param ttf The font that is being read.
     * @param data The stream to read the data from.
     * @throws IOException If there is an error reading the data.
     */
    public overrides sub initData(Byval ttf as TrueTypeFont , ByVal data as TTFDataStream )
    {
        platformId = data.readUnsignedShort();
        platformEncodingId = data.readUnsignedShort();
        languageId = data.readUnsignedShort();
        nameId = data.readUnsignedShort();
        stringLength = data.readUnsignedShort();
        stringOffset = data.readUnsignedShort();
    }
    
    /**
     * Return a string representation of Me class.
     * 
     * @return A string for Me class.
     */
    public String toString()
    {
        return 
            "platform=" &  platformId + 
            " pEncoding=" &  platformEncodingId + 
            " language=" &  languageId + 
            " name=" &  nameId;
    }
    /**
     * @return Returns the string.
     */
    public String getString()
    {
        return string;
    }
    /**
     * @param stringValue The string to set.
     */
    public void setString(String stringValue)
    {
        Me.string = stringValue;
    }
}
