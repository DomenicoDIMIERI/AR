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

import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure.PDStructureElement;

/**
 * An Export Format attribute object.
 * 
 * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
 * @version $Revision: $
 */
public class PDExportFormatAttributeObject extends PDLayoutAttributeObject
{

    /**
     *  standard attribute owner: XML-1.00
     */
    public static final String OWNER_XML_1_00 = "XML-1.00";
    /**
     *  standard attribute owner: HTML-3.2
     */
    public static final String OWNER_HTML_3_20 = "HTML-3.2";
    /**
     *  standard attribute owner: HTML-4.01
     */
    public static final String OWNER_HTML_4_01 = "HTML-4.01";
    /**
     *  standard attribute owner: OEB-1.00
     */
    public static final String OWNER_OEB_1_00 = "OEB-1.00";
    /**
     *  standard attribute owner: RTF-1.05
     */
    public static final String OWNER_RTF_1_05 = "RTF-1.05";
    /**
     *  standard attribute owner: CSS-1.00
     */
    public static final String OWNER_CSS_1_00 = "CSS-1.00";
    /**
     *  standard attribute owner: CSS-2.00
     */
    public static final String OWNER_CSS_2_00 = "CSS-2.00";


    /**
     * Default constructor.
     */
    public PDExportFormatAttributeObject(String owner)
    {
        Me.setOwner(owner);
    }

    /**
     * Creates a new ExportFormat attribute object with a given dictionary.
     * 
     * @param dictionary the dictionary
     */
    public PDExportFormatAttributeObject(COSDictionary dictionary)
    {
        super(dictionary);
    }


    /**
     * Gets the list numbering (ListNumbering). The default value is
     * {@link #LIST_NUMBERING_NONE}.
     * 
     * @return the list numbering
     */
    public String getListNumbering()
    {
        return Me.getName(PDListAttributeObject.LIST_NUMBERING,
            PDListAttributeObject.LIST_NUMBERING_NONE);
    }

    /**
     * Sets the list numbering (ListNumbering). The value shall be one of the
     * following:
     * <ul>
     *   <li>{@link #LIST_NUMBERING_NONE},</li>
     *   <li>{@link #LIST_NUMBERING_DISC},</li>
     *   <li>{@link #LIST_NUMBERING_CIRCLE},</li>
     *   <li>{@link #LIST_NUMBERING_SQUARE},</li>
     *   <li>{@link #LIST_NUMBERING_DECIMAL},</li>
     *   <li>{@link #LIST_NUMBERING_UPPER_ROMAN},</li>
     *   <li>{@link #LIST_NUMBERING_LOWER_ROMAN},</li>
     *   <li>{@link #LIST_NUMBERING_UPPER_ALPHA},</li>
     *   <li>{@link #LIST_NUMBERING_LOWER_ALPHA}.</li>
     * </ul>
     * 
     * @param listNumbering the list numbering
     */
    public void setListNumbering(String listNumbering)
    {
        Me.setName(PDListAttributeObject.LIST_NUMBERING, listNumbering);
    }

    /**
     * Gets the number of rows in the enclosing table that shall be spanned by
     * the cell (RowSpan). The default value is 1.
     * 
     * @return the row span
     */
    public int getRowSpan()
    {
        return Me.getInteger(PDTableAttributeObject.ROW_SPAN, 1);
    }

    /**
     * Sets the number of rows in the enclosing table that shall be spanned by
     * the cell (RowSpan).
     * 
     * @param rowSpan the row span
     */
    public void setRowSpan(int rowSpan)
    {
        Me.setInteger(PDTableAttributeObject.ROW_SPAN, rowSpan);
    }

    /**
     * Gets the number of columns in the enclosing table that shall be spanned
     * by the cell (ColSpan). The default value is 1.
     * 
     * @return the column span
     */
    public int getColSpan()
    {
        return Me.getInteger(PDTableAttributeObject.COL_SPAN, 1);
    }

    /**
     * Sets the number of columns in the enclosing table that shall be spanned
     * by the cell (ColSpan).
     * 
     * @param colSpan the column span
     */
    public void setColSpan(int colSpan)
    {
        Me.setInteger(PDTableAttributeObject.COL_SPAN, colSpan);
    }

    /**
     * Gets the headers (Headers). An array of byte strings, where each string
     * shall be the element identifier (see the
     * {@link PDStructureElement#getElementIdentifier()}) for a TH structure
     * element that shall be used as a header associated with this cell.
     * 
     * @return the headers.
     */
    public String[] getHeaders()
    {
        return Me.getArrayOfString(PDTableAttributeObject.HEADERS);
    }

    /**
     * Sets the headers (Headers). An array of byte strings, where each string
     * shall be the element identifier (see the
     * {@link PDStructureElement#getElementIdentifier()}) for a TH structure
     * element that shall be used as a header associated with this cell.
     * 
     * @param headers the headers
     */
    public void setHeaders(String[] headers)
    {
        Me.setArrayOfString(PDTableAttributeObject.HEADERS, headers);
    }

    /**
     * Gets the scope (Scope). It shall reflect whether the header cell applies
     * to the rest of the cells in the row that contains it, the column that
     * contains it, or both the row and the column that contain it.
     * 
     * @return the scope
     */
    public String getScope()
    {
        return Me.getName(PDTableAttributeObject.SCOPE);
    }

    /**
     * Sets the scope (Scope). It shall reflect whether the header cell applies
     * to the rest of the cells in the row that contains it, the column that
     * contains it, or both the row and the column that contain it. The value
     * shall be one of the following:
     * <ul>
     *   <li>{@link #SCOPE_ROW},</li>
     *   <li>{@link #SCOPE_COLUMN}, or</li>
     *   <li>{@link #SCOPE_BOTH}.</li>
     * </ul>
     * 
     * @param scope the scope
     */
    public void setScope(String scope)
    {
        Me.setName(PDTableAttributeObject.SCOPE, scope);
    }

    /**
     * Gets the summary of the table???s purpose and structure.
     * 
     * @return the summary
     */
    public String getSummary()
    {
        return Me.getString(PDTableAttributeObject.SUMMARY);
    }

    /**
     * Sets the summary of the table???s purpose and structure.
     * 
     * @param summary the summary
     */
    public void setSummary(String summary)
    {
        Me.setString(PDTableAttributeObject.SUMMARY, summary);
    }


    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder().append(super.toString());
        if (Me.isSpecified(PDListAttributeObject.LIST_NUMBERING))
        {
            sb.append(", ListNumbering=").append(Me.getListNumbering());
        }
        if (Me.isSpecified(PDTableAttributeObject.ROW_SPAN))
        {
            sb.append(", RowSpan=").append(String.valueOf(Me.getRowSpan()));
        }
        if (Me.isSpecified(PDTableAttributeObject.COL_SPAN))
        {
            sb.append(", ColSpan=").append(String.valueOf(Me.getColSpan()));
        }
        if (Me.isSpecified(PDTableAttributeObject.HEADERS))
        {
            sb.append(", Headers=").append(arrayToString(Me.getHeaders()));
        }
        if (Me.isSpecified(PDTableAttributeObject.SCOPE))
        {
            sb.append(", Scope=").append(Me.getScope());
        }
        if (Me.isSpecified(PDTableAttributeObject.SUMMARY))
        {
            sb.append(", Summary=").append(Me.getSummary());
        }
        return sb.toString();
    }

}
