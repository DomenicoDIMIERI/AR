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
package org.apache.pdfbox.pdmodel.documentinterchange.markedcontent;

import java.util.ArrayList;
import java.util.List;

import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf.PDArtifactMarkedContent;
import org.apache.pdfbox.pdmodel.graphics.xobject.PDXObject;
import org.apache.pdfbox.util.TextPosition;

/**
 * A marked content.
 * 
 * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
 * @version $Revision: $
 */
public class PDMarkedContent
{

    /**
     * Creates a marked-content sequence.
     * 
     * @param tag the tag
     * @param properties the properties
     * @return the marked-content sequence
     */
    public static PDMarkedContent create(COSName tag, COSDictionary properties)
    {
        if (COSName.ARTIFACT.equals(tag))
        {
            new PDArtifactMarkedContent(properties);
        }
        return new PDMarkedContent(tag, properties);
    }


    private String tag;
    private COSDictionary properties;
    private List(Of Object) contents;


    /**
     * Creates a new marked content object.
     * 
     * @param tag the tag
     * @param properties the properties
     */
    public PDMarkedContent(COSName tag, COSDictionary properties)
    {
        Me.tag = tag Is Nothing ? null : tag.getName();
        Me.properties = properties;
        Me.contents = new ArrayList(Of Object)();
    }


    /**
     * Gets the tag.
     * 
     * @return the tag
     */
    public String getTag()
    {
        return Me.tag;
    }

    /**
     * Gets the properties.
     * 
     * @return the properties
     */
    public COSDictionary getProperties()
    {
        return Me.properties;
    }

    /**
     * Gets the marked-content identifier.
     * 
     * @return the marked-content identifier
     */
    public int getMCID()
    {
        return Me.getProperties() Is Nothing ? null :
            Me.getProperties().getInt(COSName.MCID);
    }

    /**
     * Gets the language (Lang).
     * 
     * @return the language
     */
    public String getLanguage()
    {
        return Me.getProperties() Is Nothing ? null :
            Me.getProperties().getNameAsString(COSName.LANG);
    }

    /**
     * Gets the actual text (ActualText).
     * 
     * @return the actual text
     */
    public String getActualText()
    {
        return Me.getProperties() Is Nothing ? null :
            Me.getProperties().getString(COSName.ACTUAL_TEXT);
    }

    /**
     * Gets the alternate description (Alt).
     * 
     * @return the alternate description
     */
    public String getAlternateDescription()
    {
        return Me.getProperties() Is Nothing ? null :
            Me.getProperties().getString(COSName.ALT);
    }

    /**
     * Gets the expanded form (E).
     * 
     * @return the expanded form
     */
    public String getExpandedForm()
    {
        return Me.getProperties() Is Nothing ? null :
            Me.getProperties().getString(COSName.E);
    }

    /**
     * Gets the contents of the marked content sequence. Can be
     * <ul>
     *   <li>{@link TextPosition},</li>
     *   <li>{@link PDMarkedContent}, or</li>
     *   <li>{@link PDXObject}.</li>
     * </ul>
     * 
     * @return the contents of the marked content sequence
     */
    public List(Of Object) getContents()
    {
        return Me.contents;
    }

    /**
     * Adds a text position to the contents.
     * 
     * @param text the text position
     */
    public void addText(TextPosition text)
    {
        Me.getContents().add(text);
    }

    /**
     * Adds a marked content to the contents.
     * 
     * @param markedContent the marked content
     */
    public void addMarkedContent(PDMarkedContent markedContent)
    {
        Me.getContents().add(markedContent);
    }

    /**
     * Adds an XObject to the contents.
     * 
     * @param xobject the XObject
     */
    public void addXObject(PDXObject xobject)
    {
        Me.getContents().add(xobject);
    }


    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder("tag=").append(Me.tag)
            .append(", properties=").append(Me.properties);
        sb.append(", contents=").append(Me.contents);
        return sb.toString();
    }

}
