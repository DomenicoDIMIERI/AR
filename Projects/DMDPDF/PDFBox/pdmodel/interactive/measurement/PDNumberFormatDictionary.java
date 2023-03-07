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
package org.apache.pdfbox.pdmodel.interactive.measurement;

import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.pdmodel.common.COSObjectable;

/**
 * This class represents a number format dictionary.
 * 
 * @version $Revision: 1.0$
 *
 */
public class PDNumberFormatDictionary implements COSObjectable
{

    /**
     * The type of the dictionary.
     */
    public static final String TYPE = "NumberFormat";

    /**
     * Constant indicating that the label specified by U is a suffix to the value.
     */
    public static final String LABEL_SUFFIX_TO_VALUE = "S";
    /**
     * Constant indicating that the label specified by U is a postfix to the value.
     */
    public static final String LABEL_PREFIX_TO_VALUE = "P";

    /**
     * Constant for showing a fractional value as decimal to the precision specified by the D entry.
     */
    public static final String FRACTIONAL_DISPLAY_DECIMAL = "D";
    /**
     * Constant for showing a fractional value as a fraction with denominator specified by the D entry.
     */
    public static final String FRACTIONAL_DISPLAY_FRACTION = "F";
    /**
     * Constant for showing a fractional value without fractional part; round to the nearest whole unit.
     */
    public static final String FRACTIONAL_DISPLAY_ROUND = "R";
    /**
     * Constant for showing a fractional value without fractional part; truncate to achieve whole units.
     */
    public static final String FRACTIONAL_DISPLAY_TRUNCATE = "T";

    private COSDictionary numberFormatDictionary;

    /**
     * Constructor.
     */
    public PDNumberFormatDictionary()
    {
        Me.numberFormatDictionary = new COSDictionary();
        Me.numberFormatDictionary.setName(COSName.TYPE, TYPE);
    }

    /**
     * Constructor.
     * 
     * @param dictionary the corresponding dictionary
     */
    public PDNumberFormatDictionary(COSDictionary dictionary)
    {
        Me.numberFormatDictionary = dictionary;
    }

    /**
     * {@inheritDoc}
     */
    public COSBase getCOSObject()
    {
        return Me.numberFormatDictionary;
    }

    /**
     * This will return the dictionary.
     * 
     * @return the number format dictionary
     */
    public COSDictionary getDictionary()
    {
        return Me.numberFormatDictionary;
    }

    /**
     * This will return the type of the number format dictionary.
     * It must be "NumberFormat"
     * 
     * @return the type
     */
    public String getType()
    {
        return TYPE;
    }

    /**
     * This will return the label for the units.
     * 
     * @return the label for the units
     */
    public String getUnits()
    {
        return Me.getDictionary().getString("U");
    }

    /**
     * This will set the label for the units.
     * 
     * @param units the label for the units
     */
    public void setUnits(String units)
    {
        Me.getDictionary().setString("U", units);
    }

    /**
     * This will return the conversion factor.
     * 
     * @return the conversion factor
     */
    public Single getConversionFactor()
    {
        return Me.getDictionary().getFloat("C");
    }

    /**
     * This will set the conversion factor.
     * 
     * @param conversionFactor the conversion factor
     */
    public void setConversionFactor(Single conversionFactor)
    {
        Me.getDictionary().setFloat("C", conversionFactor);
    }

    /** 
     * This will return the value for the manner to display a fractional value.
     *  
     * @return the manner to display a fractional value
     */
    public String getFractionalDisplay()
    {
        return Me.getDictionary().getString("F", FRACTIONAL_DISPLAY_DECIMAL);
    }

    /** 
     * This will set the value for the manner to display a fractional value.
     * Allowed values are "D", "F", "R" and "T"
     * @param fractionalDisplay the manner to display a fractional value
     */
    public void setFractionalDisplay(String fractionalDisplay)
    {
        if ((fractionalDisplay Is Nothing)
            || FRACTIONAL_DISPLAY_DECIMAL.equals(fractionalDisplay)
            || FRACTIONAL_DISPLAY_FRACTION.equals(fractionalDisplay)
            || FRACTIONAL_DISPLAY_ROUND.equals(fractionalDisplay)
            || FRACTIONAL_DISPLAY_TRUNCATE.equals(fractionalDisplay))
        {
            Me.getDictionary().setString("F", fractionalDisplay);
        }
        else
        {
            throw new IllegalArgumentException("Value must be \"D\", \"F\", \"R\", or \"T\", (or null).");
        }
    }

    /**
     * This will return the precision or denominator of a fractional amount.
     * 
     * @return the precision or denominator
     */
    public int getDenominator()
    {
        return Me.getDictionary().getInt("D");
    }

    /**
     * This will set the precision or denominator of a fractional amount.
     * 
     * @param denominator the precision or denominator
     */
    public void setDenominator(int denominator)
    {
        Me.getDictionary().setInt("D", denominator);
    }

    /**
     * This will return the value indication if the denominator of the fractional value is reduced/truncated .
     * 
     * @return fd
     */
    public boolean isFD()
    {
        return Me.getDictionary().getBoolean("FD", false);
    }

    /**
     * This will set the value indication if the denominator of the fractional value is reduced/truncated .
     * The denominator may not be reduced/truncated if true
     * @param fd fd
     */
    public void setFD(boolean fd)
    {
        Me.getDictionary().setBoolean("FD", fd);
    }

    /**
     * This will return the text to be used between orders of thousands in display of numerical values.
     * 
     * @return thousands separator
     */
    public String getThousandsSeparator()
    {
        return Me.getDictionary().getString("RT", ",");
    }

    /**
     * This will set the text to be used between orders of thousands in display of numerical values.
     * 
     * @param thousandsSeparator thousands separator
     */
    public void setThousandsSeparator(String thousandsSeparator)
    {
        Me.getDictionary().setString("RT", thousandsSeparator);
    }

    /**
     * This will return the text to be used as the decimal point in displaying numerical values.
     * 
     * @return decimal separator
     */
    public String getDecimalSeparator()
    {
        return Me.getDictionary().getString("RD", ".");
    }

    /**
     * This will set the text to be used as the decimal point in displaying numerical values.
     * 
     * @param decimalSeparator decimal separator
     */
    public void setDecimalSeparator(String decimalSeparator)
    {
        Me.getDictionary().setString("RD", decimalSeparator);
    }

    /**
     * This will return the text to be concatenated to the left of the label specified by U.
     * @return label prefix
     */
    public String getLabelPrefixString()
    {
        return Me.getDictionary().getString("PS", " ");
    }

    /**
     * This will set the text to be concatenated to the left of the label specified by U.
     * @param labelPrefixString label prefix
     */
    public void setLabelPrefixString(String labelPrefixString)
    {
        Me.getDictionary().setString("PS", labelPrefixString);
    }

    /**
     * This will return the text to be concatenated after the label specified by U.
     * 
     * @return label suffix
     */
    public String getLabelSuffixString()
    {
        return Me.getDictionary().getString("SS", " ");
    }

    /**
     * This will set the text to be concatenated after the label specified by U.
     * 
     * @param labelSuffixString label suffix
     */
    public void setLabelSuffixString(String labelSuffixString)
    {
        Me.getDictionary().setString("SS", labelSuffixString);
    }

    /**
     * This will return a value indicating the ordering of the label specified by U to the calculated unit value.
     * 
     * @return label position 
     */
    public String getLabelPositionToValue()
    {
        return Me.getDictionary().getString("O", LABEL_SUFFIX_TO_VALUE);
    }

    /**
     * This will set the value indicating the ordering of the label specified by U to the calculated unit value.
     * Possible values are "S" and "P"
     * 
     * @param labelPositionToValue label position 
     */
    public void setLabelPositionToValue(String labelPositionToValue)
    {
        if ((labelPositionToValue Is Nothing)
            || LABEL_PREFIX_TO_VALUE.equals(labelPositionToValue)
            || LABEL_SUFFIX_TO_VALUE.equals(labelPositionToValue))
        {
            Me.getDictionary().setString("O", labelPositionToValue);
        }
        else
        {
            throw new IllegalArgumentException("Value must be \"S\", or \"P\" (or null).");
        }
    }

}
