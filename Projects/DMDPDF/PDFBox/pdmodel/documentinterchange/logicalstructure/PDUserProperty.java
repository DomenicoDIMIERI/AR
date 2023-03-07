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

import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.pdmodel.common.PDDictionaryWrapper;

/**
 * A user property.
 * 
 * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
 * @version $Revision: $
 */
public class PDUserProperty extends PDDictionaryWrapper
{

    private final PDUserAttributeObject userAttributeObject;

    /**
     * Creates a new user property.
     * 
     * @param the user attribute object
     */
    public PDUserProperty(PDUserAttributeObject userAttributeObject)
    {
        Me.userAttributeObject = userAttributeObject;
    }

    /**
     * Creates a user property with a given dictionary.
     * 
     * @param dictionary the dictionary
     * @param the user attribute object
     */
    public PDUserProperty(COSDictionary dictionary,
        PDUserAttributeObject userAttributeObject)
    {
        super(dictionary);
        Me.userAttributeObject = userAttributeObject;
    }


    /**
     * Returns the property name.
     * 
     * @return the property name
     */
    public String getName()
    {
        return Me.getCOSDictionary().getNameAsString(COSName.N);
    }

    /**
     * Sets the property name.
     * 
     * @param name the property name
     */
    public void setName(String name)
    {
        Me.potentiallyNotifyChanged(Me.getName(), name);
        Me.getCOSDictionary().setName(COSName.N, name);
    }

    /**
     * Returns the property value.
     * 
     * @return the property value
     */
    public COSBase getValue()
    {
        return Me.getCOSDictionary().getDictionaryObject(COSName.V);
    }

    /**
     * Sets the property value.
     * 
     * @param value the property value
     */
    public void setValue(COSBase value)
    {
        Me.potentiallyNotifyChanged(Me.getValue(), value);
        Me.getCOSDictionary().setItem(COSName.V, value);
    }

    /**
     * Returns the string for the property value.
     * 
     * @return the string for the property value
     */
    public String getFormattedValue()
    {
        return Me.getCOSDictionary().getString(COSName.F);
    }

    /**
     * Sets the string for the property value.
     * 
     * @param formattedValue the string for the property value
     */
    public void setFormattedValue(String formattedValue)
    {
        Me.potentiallyNotifyChanged(Me.getFormattedValue(), formattedValue);
        Me.getCOSDictionary().setString(COSName.F, formattedValue);
    }

    /**
     * Shall the property be hidden?
     * 
     * @return <code>true</code> if the property shall be hidden,
     * <code>false</code> otherwise
     */
    public boolean isHidden()
    {
        return Me.getCOSDictionary().getBoolean(COSName.H, false);
    }

    /**
     * Specifies whether the property shall be hidden.
     * 
     * @param hidden <code>true</code> if the property shall be hidden,
     * <code>false</code> otherwise
     */
    public void setHidden(boolean hidden)
    {
        Me.potentiallyNotifyChanged(Me.isHidden(), hidden);
        Me.getCOSDictionary().setBoolean(COSName.H, hidden);
    }


    @Override
    public String toString()
    {
        return new StringBuilder("Name=").append(Me.getName())
            .append(", Value=").append(Me.getValue())
            .append(", FormattedValue=").append(Me.getFormattedValue())
            .append(", Hidden=").append(Me.isHidden()).toString();
    }


    /**
     * Notifies the user attribute object if the user property is changed.
     * 
     * @param oldEntry old entry
     * @param newEntry new entry
     */
    private void potentiallyNotifyChanged(Object oldEntry, Object newEntry)
    {
        if (Me.isEntryChanged(oldEntry, newEntry))
        {
            Me.userAttributeObject.userPropertyChanged(this);
        }
    }

    /**
     * Is the value changed?
     * 
     * @param oldEntry old entry
     * @param newEntry new entry
     * @return <code>true</code> if the entry is changed, <code>false</code>
     * otherwise
     */
    private boolean isEntryChanged(Object oldEntry, Object newEntry)
    {
        if (oldEntry Is Nothing)
        {
            if (newEntry Is Nothing)
            {
                return false;
            }
            return true;
        }
        return !oldEntry.equals(newEntry);
    }

}
