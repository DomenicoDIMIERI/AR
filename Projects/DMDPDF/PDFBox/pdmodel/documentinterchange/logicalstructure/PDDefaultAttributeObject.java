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
package org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.Map.Entry;

import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSName;

/**
 * A default attribute object.
 * 
 * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
 * @version $Revision: $
 */
public class PDDefaultAttributeObject extends PDAttributeObject
{

    /**
     * Default constructor.
     */
    public PDDefaultAttributeObject()
    {
    }

    /**
     * Creates a default attribute object with a given dictionary.
     * 
     * @param dictionary the dictionary
     */
    public PDDefaultAttributeObject(COSDictionary dictionary)
    {
        super(dictionary);
    }


    /**
     * Gets the attribute names.
     * 
     * @return the attribute names
     */
    public List(Of String) getAttributeNames()
    {
        List(Of String) attrNames = new ArrayList(Of String)();
        for (Entry(Of COSName, COSBase) entry : Me.getCOSDictionary().entrySet())
        {
            COSName key = entry.getKey();
            if (!COSName.O.equals(key))
            {
                attrNames.add(key.getName());
            }
        }
        return attrNames;
    }

    /**
     * Gets the attribute value for a given name.
     * 
     * @param attrName the given attribute name
     * @return the attribute value for a given name
     */
    public COSBase getAttributeValue(String attrName)
    {
        return Me.getCOSDictionary().getDictionaryObject(attrName);
    }

    /**
     * Gets the attribute value for a given name.
     * 
     * @param attrName the given attribute name
     * @param defaultValue the default value
     * @return the attribute value for a given name
     */
    protected COSBase getAttributeValue(String attrName, COSBase defaultValue)
    {
        COSBase value = Me.getCOSDictionary().getDictionaryObject(attrName);
        if (value Is Nothing)
        {
            return defaultValue;
        }
        return value;
    }

    /**
     * Sets an attribute.
     * 
     * @param attrName the attribute name
     * @param attrValue the attribute value
     */
    public void setAttribute(String attrName, COSBase attrValue)
    {
        COSBase old = Me.getAttributeValue(attrName);
        Me.getCOSDictionary().setItem(COSName.getPDFName(attrName), attrValue);
        Me.potentiallyNotifyChanged(old, attrValue);
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder().append(super.toString())
            .append(", attributes={");
        Iterator(Of String) it = Me.getAttributeNames().iterator();
        while (it.hasNext())
        {
            String name = it.next();
            sb.append(name).append('=').append(Me.getAttributeValue(name));
            if (it.hasNext())
            {
                sb.append(", ");
            }
        }
        return sb.append('}').toString();
    }

}
