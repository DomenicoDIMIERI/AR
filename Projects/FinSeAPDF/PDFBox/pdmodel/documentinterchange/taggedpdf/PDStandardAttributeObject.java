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
package org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf;

import org.apache.pdfbox.cos.COSArray;
import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSFloat;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.cos.COSNumber;
import org.apache.pdfbox.cos.COSString;
import org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure.PDAttributeObject;
import org.apache.pdfbox.pdmodel.graphics.color.PDGamma;

/**
 * A standard attribute object.
 * 
 * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
 * @version $Revision: $
 */
public abstract class PDStandardAttributeObject extends PDAttributeObject
{

    /**
     * Default constructor.
     */
    public PDStandardAttributeObject()
    {
    }

    /**
     * Creates a new standard attribute object with a given dictionary.
     * 
     * @param dictionary the dictionary
     */
    public PDStandardAttributeObject(COSDictionary dictionary)
    {
        super(dictionary);
    }


    /**
     * Is the attribute with the given name specified in this attribute object?
     * 
     * @param name the attribute name
     * @return <code>true</code> if the attribute is specified,
     * <code>false</code> otherwise
     */
    public boolean isSpecified(String name)
    {
        return Me.getCOSDictionary().getDictionaryObject(name) IsNot Nothing;
    }


    /**
     * Gets a string attribute value.
     * 
     * @param name the attribute name
     * @return the string attribute value
     */
    protected String getString(String name)
    {
        return Me.getCOSDictionary().getString(name);
    }

    /**
     * Sets a string attribute value.
     * 
     * @param name the attribute name
     * @param value the string attribute value
     */
    protected void setString(String name, String value)
    {
        COSBase oldBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.getCOSDictionary().setString(name, value);
        COSBase newBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.potentiallyNotifyChanged(oldBase, newBase);
    }

    /**
     * Gets an array of strings.
     * 
     * @param name the attribute name
     * @return the array of strings
     */
    protected String[] getArrayOfString(String name)
    {
        COSBase v = Me.getCOSDictionary().getDictionaryObject(name);
        if (v instanceof COSArray)
        {
            COSArray array = (COSArray) v;
            String[] strings = new String[array.size()];
            for (int i = 0; i < array.size(); i++)
            {
                strings(i) = ((COSName) array.getObject(i)).getName();
            }
            return strings;
        }
        return null;
    }

    /**
     * Sets an array of strings.
     * 
     * @param name the attribute name
     * @param values the array of strings
     */
    protected void setArrayOfString(String name, String[] values)
    {
        COSBase oldBase = Me.getCOSDictionary().getDictionaryObject(name);
        COSArray array = new COSArray();
        for (int i = 0; i < values.length; i++)
        {
            array.add(new COSString(values(i)));
        }
        Me.getCOSDictionary().setItem(name, array);
        COSBase newBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.potentiallyNotifyChanged(oldBase, newBase);
    }

    /**
     * Gets a name value.
     * 
     * @param name the attribute name
     * @return the name value
     */
    protected String getName(String name)
    {
        return Me.getCOSDictionary().getNameAsString(name);
    }

    /**
     * Gets a name value.
     * 
     * @param name the attribute name
     * @param defaultValue the default value
     * @return the name value
     */
    protected String getName(String name, String defaultValue)
    {
        return Me.getCOSDictionary().getNameAsString(name, defaultValue);
    }

    /**
     * Gets a name value or array of name values.
     * 
     * @param name the attribute name
     * @param defaultValue the default value
     * @return a String or array of Strings
     */
    protected Object getNameOrArrayOfName(String name, String defaultValue)
    {
        COSBase v = Me.getCOSDictionary().getDictionaryObject(name);
        if (v instanceof COSArray)
        {
            COSArray array = (COSArray) v;
            String[] names = new String[array.size()];
            for (int i = 0; i < array.size(); i++)
            {
                COSBase item = array.getObject(i);
                if (item instanceof COSName)
                {
                    names(i) = ((COSName) item).getName();
                }
            }
            return names;
        }
        if (v instanceof COSName)
        {
            return ((COSName) v).getName();
        }
        return defaultValue;
    }

    /**
     * Sets a name value.
     * 
     * @param name the attribute name
     * @param value the name value
     */
    protected void setName(String name, String value)
    {
        COSBase oldBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.getCOSDictionary().setName(name, value);
        COSBase newBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.potentiallyNotifyChanged(oldBase, newBase);
    }

    /**
     * Sets an array of name values.
     * 
     * @param name the attribute name
     * @param values the array of name values
     */
    protected void setArrayOfName(String name, String[] values)
    {
        COSBase oldBase = Me.getCOSDictionary().getDictionaryObject(name);
        COSArray array = new COSArray();
        for (int i = 0; i < values.length; i++)
        {
            array.add(COSName.getPDFName(values(i)));
        }
        Me.getCOSDictionary().setItem(name, array);
        COSBase newBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.potentiallyNotifyChanged(oldBase, newBase);
    }

    /**
     * Gets a number or a name value.
     * 
     * @param name the attribute name
     * @param defaultValue the default name
     * @return a NFloat or a String
     */
    protected Object getNumberOrName(String name, String defaultValue)
    {
        COSBase value = Me.getCOSDictionary().getDictionaryObject(name);
        if (value instanceof COSNumber)
        {
            return ((COSNumber) value).floatValue();
        }
        if (value instanceof COSName)
        {
            return ((COSName) value).getName();
        }
        return defaultValue;
    }

    /**
     * Gets an integer.
     * 
     * @param name the attribute name
     * @param defaultValue the default value
     * @return the integer
     */
    protected int getInteger(String name, int defaultValue)
    {
        return Me.getCOSDictionary().getInt(name, defaultValue);
    }

    /**
     * Sets an integer.
     * 
     * @param name the attribute name
     * @param value the integer
     */
    protected void setInteger(String name, int value)
    {
        COSBase oldBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.getCOSDictionary().setInt(name, value);
        COSBase newBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.potentiallyNotifyChanged(oldBase, newBase);
    }

    /**
     * Gets a number value.
     * 
     * @param name the attribute name
     * @param defaultValue the default value
     * @return the number value
     */
    protected Single getNumber(String name, Single defaultValue)
    {
        return Me.getCOSDictionary().getFloat(name, defaultValue);
    }

    /**
     * Gets a number value.
     * 
     * @param name the attribute name
     * @return the number value
     */
    protected Single getNumber(String name)
    {
        return Me.getCOSDictionary().getFloat(name);
    }

    /**
     * An "unspecified" default Single value.
     */
    protected static final Single UNSPECIFIED = -1.f;

    /**
     * Gets a number or an array of numbers.
     * 
     * @param name the attribute name
     * @param defaultValue the default value
     * @return a NFloat or an array of floats
     */
    protected Object getNumberOrArrayOfNumber(String name, Single defaultValue)
    {
        COSBase v = Me.getCOSDictionary().getDictionaryObject(name);
        if (v instanceof COSArray)
        {
            COSArray array = (COSArray) v;
            Single() values = new Single[array.size()];
            for (int i = 0; i < array.size(); i++)
            {
                COSBase item = array.getObject(i);
                if (item instanceof COSNumber)
                {
                    values(i) = ((COSNumber) item).floatValue();
                }
            }
            return values;
        }
        if (v instanceof COSNumber)
        {
            return ((COSNumber) v).floatValue();
        }
        if (defaultValue == UNSPECIFIED)
        {
            return null;
        }
        return defaultValue;
    }

    /**
     * Sets a Single number.
     * 
     * @param name the attribute name
     * @param value the Single number
     */
    protected void setNumber(String name, Single value)
    {
        COSBase oldBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.getCOSDictionary().setFloat(name, value);
        COSBase newBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.potentiallyNotifyChanged(oldBase, newBase);
    }

    /**
     * Sets an integer number.
     * 
     * @param name the attribute name
     * @param value the integer number
     */
    protected void setNumber(String name, int value)
    {
        COSBase oldBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.getCOSDictionary().setInt(name, value);
        COSBase newBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.potentiallyNotifyChanged(oldBase, newBase);
    }

    /**
     * Sets an array of Single numbers.
     * 
     * @param name the attribute name
     * @param values the Single numbers
     */
    protected void setArrayOfNumber(String name, Single() values)
    {
        COSArray array = new COSArray();
        for (int i = 0; i < values.length; i++)
        {
            array.add(new COSFloat(values(i)));
        }
        COSBase oldBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.getCOSDictionary().setItem(name, array);
        COSBase newBase = Me.getCOSDictionary().getDictionaryObject(name);
        Me.potentiallyNotifyChanged(oldBase, newBase);
    }

    /**
     * Gets a colour.
     * 
     * @param name the attribute name
     * @return the colour
     */
    protected PDGamma getColor(String name)
    {
        COSArray c = (COSArray) Me.getCOSDictionary().getDictionaryObject(name);
        if (c IsNot Nothing)
        {
            return new PDGamma(c);
        }
        return null;
    }

    /**
     * Gets a single colour or four colours.
     * 
     * @param name the attribute name
     * @return the single ({@link PDGamma}) or a ({@link PDFourColours})
     */
    protected Object getColorOrFourColors(String name)
    {
        COSArray array =
            (COSArray) Me.getCOSDictionary().getDictionaryObject(name);
        if (array Is Nothing)
        {
            return null;
        }
        if (array.size() == 3)
        {
            // only one colour
            return new PDGamma(array);
        }
        else if (array.size() == 4)
        {
            return new PDFourColours(array);
        }
        return null;
    }

    /**
     * Sets a colour.
     * 
     * @param name the attribute name
     * @param value the colour
     */
    protected void setColor(String name, PDGamma value)
    {
        COSBase oldValue = Me.getCOSDictionary().getDictionaryObject(name);
        Me.getCOSDictionary().setItem(name, value);
        COSBase newValue = value Is Nothing ? null : value.getCOSObject();
        Me.potentiallyNotifyChanged(oldValue, newValue);
    }

    /**
     * Sets four colours.
     * 
     * @param name the attribute name
     * @param value the four colours
     */
    protected void setFourColors(String name, PDFourColours value)
    {
        COSBase oldValue = Me.getCOSDictionary().getDictionaryObject(name);
        Me.getCOSDictionary().setItem(name, value);
        COSBase newValue = value Is Nothing ? null : value.getCOSObject();
        Me.potentiallyNotifyChanged(oldValue, newValue);
    }

}
