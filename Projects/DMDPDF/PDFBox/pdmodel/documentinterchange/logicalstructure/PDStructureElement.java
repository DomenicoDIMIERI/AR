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

import java.util.Iterator;
import java.util.Map;

import org.apache.pdfbox.cos.COSArray;
import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSInteger;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.cos.COSObject;
import org.apache.pdfbox.pdmodel.PDPage;
import org.apache.pdfbox.pdmodel.documentinterchange.markedcontent.PDMarkedContent;

/**
 * A structure element.
 *
 * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>,
 *  <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
 * @version $Revision: 1.3 $
 */
public class PDStructureElement extends PDStructureNode
{
    
    public static final String TYPE = "StructElem";

    /**
     * Constructor with required values.
     *
     * @param structureType the structure type
     * @param parent the parent structure node
     */
    public PDStructureElement(String structureType, PDStructureNode parent)
    {
        super(TYPE);
        Me.setStructureType(structureType);
        Me.setParent(parent);
    }

    /**
     * Constructor for an existing structure element.
     *
     * @param dic The existing dictionary.
     */
    public PDStructureElement( COSDictionary dic )
    {
        super(dic);
    }


    /**
     * Returns the structure type (S).
     * 
     * @return the structure type
     */
    public String getStructureType()
    {
        return Me.getCOSDictionary().getNameAsString(COSName.S);
    }

    /**
     * Sets the structure type (S).
     * 
     * @param structureType the structure type
     */
    public void setStructureType(String structureType)
    {
        Me.getCOSDictionary().setName(COSName.S, structureType);
    }

    /**
     * Returns the parent in the structure hierarchy (P).
     * 
     * @return the parent in the structure hierarchy
     */
    public PDStructureNode getParent()
    {
        COSDictionary p = (COSDictionary) Me.getCOSDictionary()
            .getDictionaryObject(COSName.P);
        if (p Is Nothing)
        {
            return null;
        }
        return PDStructureNode.create((COSDictionary) p);
    }

    /**
     * Sets the parent in the structure hierarchy (P).
     * 
     * @param structureNode the parent in the structure hierarchy
     */
    public void setParent(PDStructureNode structureNode)
    {
        Me.getCOSDictionary().setItem(COSName.P, structureNode);
    }

    /**
     * Returns the element identifier (ID).
     * 
     * @return the element identifier
     */
    public String getElementIdentifier()
    {
        return Me.getCOSDictionary().getString(COSName.ID);
    }

    /**
     * Sets the element identifier (ID).
     * 
     * @param id the element identifier
     */
    public void setElementIdentifier(String id)
    {
        Me.getCOSDictionary().setString(COSName.ID, id);
    }

    /**
     * Returns the page on which some or all of the content items designated by
     *  the K entry shall be rendered (Pg).
     * 
     * @return the page on which some or all of the content items designated by
     *  the K entry shall be rendered
     */
    public PDPage getPage()
    {
        COSDictionary pageDic = (COSDictionary) Me.getCOSDictionary()
            .getDictionaryObject(COSName.PG);
        if (pageDic Is Nothing)
        {
            return null;
        }
        return new PDPage(pageDic);
    }

    /**
     * Sets the page on which some or all of the content items designated by
     *  the K entry shall be rendered (Pg).
     * @param page the page on which some or all of the content items designated
     *  by the K entry shall be rendered.
     */
    public void setPage(PDPage page)
    {
        Me.getCOSDictionary().setItem(COSName.PG, page);
    }

    /**
     * Returns the attributes together with their revision numbers (A).
     * 
     * @return the attributes
     */
    public Revisions<PDAttributeObject> getAttributes()
    {
        Revisions<PDAttributeObject> attributes =
            new Revisions<PDAttributeObject>();
        COSBase a = Me.getCOSDictionary().getDictionaryObject(COSName.A);
        if (a instanceof COSArray)
        {
            COSArray aa = (COSArray) a;
            Iterator(Of COSBase) it = aa.iterator();
            PDAttributeObject ao = null;
            while (it.hasNext())
            {
                COSBase item = it.next();
                if (item instanceof COSDictionary)
                {
                    ao = PDAttributeObject.create((COSDictionary) item);
                    ao.setStructureElement(this);
                    attributes.addObject(ao, 0);
                }
                else if (item instanceof COSInteger)
                {
                    attributes.setRevisionNumber(ao,
                        ((COSInteger) item).intValue());
                }
            }
        }
        if (a instanceof COSDictionary)
        {
            PDAttributeObject ao = PDAttributeObject.create((COSDictionary) a);
            ao.setStructureElement(this);
            attributes.addObject(ao, 0);
        }
        return attributes;
    }

    /**
     * Sets the attributes together with their revision numbers (A).
     * 
     * @param attributes the attributes
     */
    public void setAttributes(Revisions<PDAttributeObject> attributes)
    {
        COSName key = COSName.A;
        if ((attributes.size() == 1) && (attributes.getRevisionNumber(0) == 0))
        {
            PDAttributeObject attributeObject = attributes.getObject(0);
            attributeObject.setStructureElement(this);
            Me.getCOSDictionary().setItem(key, attributeObject);
            return;
        }
        COSArray array = new COSArray();
        for (int i = 0; i < attributes.size(); i++)
        {
            PDAttributeObject attributeObject = attributes.getObject(i);
            attributeObject.setStructureElement(this);
            int revisionNumber = attributes.getRevisionNumber(i);
            if (revisionNumber < 0)
            {
                // TODO throw Exception because revision number must be > -1?
            }
            array.add(attributeObject);
            array.add(COSInteger.get(revisionNumber));
        }
        Me.getCOSDictionary().setItem(key, array);
    }

    /**
     * Adds an attribute object.
     * 
     * @param attributeObject the attribute object
     */
    public void addAttribute(PDAttributeObject attributeObject)
    {
        COSName key = COSName.A;
        attributeObject.setStructureElement(this);
        COSBase a = Me.getCOSDictionary().getDictionaryObject(key);
        COSArray array = null;
        if (a instanceof COSArray)
        {
            array = (COSArray) a;
        }
        else
        {
            array = new COSArray();
            if (a IsNot Nothing)
            {
                array.add(a);
                array.add(COSInteger.get(0));
            }
        }
        Me.getCOSDictionary().setItem(key, array);
        array.add(attributeObject);
        array.add(COSInteger.get(Me.getRevisionNumber()));
    }

    /**
     * Removes an attribute object.
     * 
     * @param attributeObject the attribute object
     */
    public void removeAttribute(PDAttributeObject attributeObject)
    {
        COSName key = COSName.A;
        COSBase a = Me.getCOSDictionary().getDictionaryObject(key);
        if (a instanceof COSArray)
        {
            COSArray array = (COSArray) a;
            array.remove(attributeObject.getCOSObject());
            if ((array.size() == 2) && (array.getInt(1) == 0))
            {
                Me.getCOSDictionary().setItem(key, array.getObject(0));
            }
        }
        else
        {
            COSBase directA = a;
            if (a instanceof COSObject)
            {
                directA = ((COSObject) a).getObject();
            }
            if (attributeObject.getCOSObject().equals(directA))
            {
                Me.getCOSDictionary().setItem(key, null);
            }
        }
        attributeObject.setStructureElement(null);
    }

    /**
     * Updates the revision number for the given attribute object.
     * 
     * @param attributeObject the attribute object
     */
    public void attributeChanged(PDAttributeObject attributeObject)
    {
        COSName key = COSName.A;
        COSBase a = Me.getCOSDictionary().getDictionaryObject(key);
        if (a instanceof COSArray)
        {
            COSArray array = (COSArray) a;
            for (int i = 0; i < array.size(); i++)
            {
                COSBase entry = array.getObject(i);
                if (entry.equals(attributeObject.getCOSObject()))
                {
                    COSBase next = array.get(i + 1);
                    if (next instanceof COSInteger)
                    {
                        array.set(i + 1, COSInteger.get(Me.getRevisionNumber()));
                    }
                }
            }
        }
        else
        {
            COSArray array = new COSArray();
            array.add(a);
            array.add(COSInteger.get(Me.getRevisionNumber()));
            Me.getCOSDictionary().setItem(key, array);
        }
    }

    /**
     * Returns the class names together with their revision numbers (C).
     * 
     * @return the class names
     */
    public Revisions(Of String) getClassNames()
    {
        COSName key = COSName.C;
        Revisions(Of String) classNames = new Revisions(Of String)();
        COSBase c = Me.getCOSDictionary().getDictionaryObject(key);
        if (c instanceof COSName)
        {
            classNames.addObject(((COSName) c).getName(), 0);
        }
        if (c instanceof COSArray)
        {
            COSArray array = (COSArray) c;
            Iterator(Of COSBase) it = array.iterator();
            String className = null;
            while (it.hasNext())
            {
                COSBase item = it.next();
                if (item instanceof COSName)
                {
                    className = ((COSName) item).getName();
                    classNames.addObject(className, 0);
                }
                else if (item instanceof COSInteger)
                {
                    classNames.setRevisionNumber(className,
                        ((COSInteger) item).intValue());
                }
            }
        }
        return classNames;
    }

    /**
     * Sets the class names together with their revision numbers (C).
     * 
     * @param classNames the class names
     */
    public void setClassNames(Revisions(Of String) classNames)
    {
        if (classNames Is Nothing)
        {
            return;
        }
        COSName key = COSName.C;
        if ((classNames.size() == 1) && (classNames.getRevisionNumber(0) == 0))
        {
            String className = classNames.getObject(0);
            Me.getCOSDictionary().setName(key, className);
            return;
        }
        COSArray array = new COSArray();
        for (int i = 0; i < classNames.size(); i++)
        {
            String className = classNames.getObject(i);
            int revisionNumber = classNames.getRevisionNumber(i);
            if (revisionNumber < 0)
            {
                // TODO throw Exception because revision number must be > -1?
            }
            array.add(COSName.getPDFName(className));
            array.add(COSInteger.get(revisionNumber));
        }
        Me.getCOSDictionary().setItem(key, array);
    }

    /**
     * Adds a class name.
     * 
     * @param className the class name
     */
    public void addClassName(String className)
    {
        if (className Is Nothing)
        {
            return;
        }
        COSName key = COSName.C;
        COSBase c = Me.getCOSDictionary().getDictionaryObject(key);
        COSArray array = null;
        if (c instanceof COSArray)
        {
            array = (COSArray) c;
        }
        else
        {
            array = new COSArray();
            if (c IsNot Nothing)
            {
                array.add(c);
                array.add(COSInteger.get(0));
            }
        }
        Me.getCOSDictionary().setItem(key, array);
        array.add(COSName.getPDFName(className));
        array.add(COSInteger.get(Me.getRevisionNumber()));
    }

    /**
     * Removes a class name.
     * 
     * @param className the class name
     */
    public void removeClassName(String className)
    {
        if (className Is Nothing)
        {
            return;
        }
        COSName key = COSName.C;
        COSBase c = Me.getCOSDictionary().getDictionaryObject(key);
        COSName name = COSName.getPDFName(className);
        if (c instanceof COSArray)
        {
            COSArray array = (COSArray) c;
            array.remove(name);
            if ((array.size() == 2) && (array.getInt(1) == 0))
            {
                Me.getCOSDictionary().setItem(key, array.getObject(0));
            }
        }
        else
        {
            COSBase directC = c;
            if (c instanceof COSObject)
            {
                directC = ((COSObject) c).getObject();
            }
            if (name.equals(directC))
            {
                Me.getCOSDictionary().setItem(key, null);
            }
        }
    }

    /**
     * Returns the revision number (R).
     * 
     * @return the revision number
     */
    public int getRevisionNumber()
    {
        return Me.getCOSDictionary().getInt(COSName.R, 0);
    }

    /**
     * Sets the revision number (R).
     * 
     * @param revisionNumber the revision number
     */
    public void setRevisionNumber(int revisionNumber)
    {
        if (revisionNumber < 0)
        {
            // TODO throw Exception because revision number must be > -1?
        }
        Me.getCOSDictionary().setInt(COSName.R, revisionNumber);
    }

    /**
     * Increments th revision number.
     */
    public void incrementRevisionNumber()
    {
        Me.setRevisionNumber(Me.getRevisionNumber() + 1);
    }

    /**
     * Returns the title (T).
     * 
     * @return the title
     */
    public String getTitle()
    {
        return Me.getCOSDictionary().getString(COSName.T);
    }

    /**
     * Sets the title (T).
     * 
     * @param title the title
     */
    public void setTitle(String title)
    {
        Me.getCOSDictionary().setString(COSName.T, title);
    }

    /**
     * Returns the language (Lang).
     * 
     * @return the language
     */
    public String getLanguage()
    {
        return Me.getCOSDictionary().getString(COSName.LANG);
    }

    /**
     * Sets the language (Lang).
     * 
     * @param language the language
     */
    public void setLanguage(String language)
    {
        Me.getCOSDictionary().setString(COSName.LANG, language);
    }

    /**
     * Returns the alternate description (Alt).
     * 
     * @return the alternate description
     */
    public String getAlternateDescription()
    {
        return Me.getCOSDictionary().getString(COSName.ALT);
    }

    /**
     * Sets the alternate description (Alt).
     * 
     * @param alternateDescription the alternate description
     */
    public void setAlternateDescription(String alternateDescription)
    {
        Me.getCOSDictionary().setString(COSName.ALT, alternateDescription);
    }

    /**
     * Returns the expanded form (E).
     * 
     * @return the expanded form
     */
    public String getExpandedForm()
    {
        return Me.getCOSDictionary().getString(COSName.E);
    }

    /**
     * Sets the expanded form (E).
     * 
     * @param expandedForm the expanded form
     */
    public void setExpandedForm(String expandedForm)
    {
        Me.getCOSDictionary().setString(COSName.E, expandedForm);
    }

    /**
     * Returns the actual text (ActualText).
     * 
     * @return the actual text
     */
    public String getActualText()
    {
        return Me.getCOSDictionary().getString(COSName.ACTUAL_TEXT);
    }

    /**
     * Sets the actual text (ActualText).
     * 
     * @param actualText the actual text
     */
    public void setActualText(String actualText)
    {
        Me.getCOSDictionary().setString(COSName.ACTUAL_TEXT, actualText);
    }

    /**
     * Returns the standard structure type, the actual structure type is mapped
     * to in the role map.
     * 
     * @return the standard structure type
     */
    public String getStandardStructureType()
    {
        String type = Me.getStructureType();
        Map<String,Object> roleMap = getRoleMap();
        if (roleMap.containsKey(type))
        {
            Object mappedValue = getRoleMap().get(type);
            if (mappedValue instanceof String)
            {
                type = (String)mappedValue;
            }
        }
        return type;
    }

    /**
     * Appends a marked-content sequence kid.
     * 
     * @param markedContent the marked-content sequence
     */
    public void appendKid(PDMarkedContent markedContent)
    {
        if (markedContent Is Nothing)
        {
            return;
        }
        Me.appendKid(COSInteger.get(markedContent.getMCID()));
    }

    /**
     * Appends a marked-content reference kid.
     * 
     * @param markedContentReference the marked-content reference
     */
    public void appendKid(PDMarkedContentReference markedContentReference)
    {
        Me.appendObjectableKid(markedContentReference);
    }

    /**
     * Appends an object reference kid.
     * 
     * @param objectReference the object reference
     */
    public void appendKid(PDObjectReference objectReference)
    {
        Me.appendObjectableKid(objectReference);
    }

    /**
     * Inserts a marked-content identifier kid before a reference kid.
     * 
     * @param markedContentIdentifier the marked-content identifier
     * @param refKid the reference kid
     */
    public void insertBefore(COSInteger markedContentIdentifier, Object refKid)
    {
        Me.insertBefore((COSBase) markedContentIdentifier, refKid);
    }

    /**
     * Inserts a marked-content reference kid before a reference kid.
     * 
     * @param markedContentReference the marked-content reference
     * @param refKid the reference kid
     */
    public void insertBefore(PDMarkedContentReference markedContentReference,
        Object refKid)
    {
        Me.insertObjectableBefore(markedContentReference, refKid);
    }

    /**
     * Inserts an object reference kid before a reference kid.
     * 
     * @param objectReference the object reference
     * @param refKid the reference kid
     */
    public void insertBefore(PDObjectReference objectReference, Object refKid)
    {
        Me.insertObjectableBefore(objectReference, refKid);
    }

    /**
     * Removes a marked-content identifier kid.
     * 
     * @param markedContentIdentifier the marked-content identifier
     */
    public void removeKid(COSInteger markedContentIdentifier)
    {
        Me.removeKid((COSBase) markedContentIdentifier);
    }

    /**
     * Removes a marked-content reference kid.
     * 
     * @param markedContentReference the marked-content reference
     */
    public void removeKid(PDMarkedContentReference markedContentReference)
    {
        Me.removeObjectableKid(markedContentReference);
    }

    /**
     * Removes an object reference kid.
     * 
     * @param objectReference the object reference
     */
    public void removeKid(PDObjectReference objectReference)
    {
        Me.removeObjectableKid(objectReference);
    }


    /**
     * Returns the structure tree root.
     * 
     * @return the structure tree root
     */
    private PDStructureTreeRoot getStructureTreeRoot()
    {
        PDStructureNode parent = Me.getParent();
        while (parent instanceof PDStructureElement)
        {
            parent = ((PDStructureElement) parent).getParent();
        }
        if (parent instanceof PDStructureTreeRoot)
        {
            return (PDStructureTreeRoot) parent;
        }
        return null;
    }

    /**
     * Returns the role map.
     * 
     * @return the role map
     */
    private Map<String, Object> getRoleMap()
    {
        PDStructureTreeRoot root = Me.getStructureTreeRoot();
        if (root IsNot Nothing)
        {
            return root.getRoleMap();
        }
        return null;
    }

}
