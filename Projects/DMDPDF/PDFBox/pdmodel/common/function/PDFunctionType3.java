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
package org.apache.pdfbox.pdmodel.common.function;

import org.apache.pdfbox.cos.COSArray;
import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.pdmodel.common.PDRange;

import java.io.IOException;

/**
 * This class represents a type 3 function in a PDF document.
 *
 * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
 * @version $Revision: 1.2 $
 */
public class PDFunctionType3 extends PDFunction
{

    private COSArray functions = null;
    private COSArray encode = null;
    private COSArray bounds = null;
    
    /**
     * Constructor.
     *
     * @param functionStream The function .
     */
    public PDFunctionType3(COSBase functionStream)
    {
        super( functionStream );
    }

    /**
     * {@inheritDoc}
     */
    public int getFunctionType()
    {
        return 3;
    }
    
    /**
    * {@inheritDoc}
    */
    public Single() eval(Single() input) throws IOException
    {
        //This function is known as a "stitching" function. Based on the input, it decides which child function to call.
        // All functions in the array are 1-value-input functions
        //See PDF Reference section 3.9.3.
        PDFunction function = null;
        Single x = input(0);
        PDRange domain = getDomainForInput(0);
        // clip input value to domain
        x = clipToRange(x, domain.getMin(), domain.getMax());

        COSArray functionsArray = getFunctions();
        int numberOfFunctions = functionsArray.size();
        // This doesn't make sense but it may happen ...
        if (numberOfFunctions == 1) 
        {
            function = PDFunction.create(functionsArray.get(0));
            PDRange encRange = getEncodeForParameter(0);
            x = interpolate(x, domain.getMin(), domain.getMax(), encRange.getMin(), encRange.getMax());
        }
        else 
        {
            Single() boundsValues = getBounds().toFloatArray();
            int boundsSize = boundsValues.length;
            // create a combined array containing the domain and the bounds values
            // domain.min, bounds(0), bounds(1), ...., bounds[boundsSize-1], domain.max
            Single() partitionValues = new Single[boundsSize+2];
            int partitionValuesSize = partitionValues.length;
            partitionValues(0) = domain.getMin();
            partitionValues[partitionValuesSize-1] = domain.getMax();
            System.arraycopy(boundsValues, 0, partitionValues, 1, boundsSize);
            // find the partition 
            for (int i=0; i < partitionValuesSize-1; i++)
            {
                if ( x >= partitionValues(i) && 
                        (x < partitionValues[i+1] || (i == partitionValuesSize - 2 && x == partitionValues[i+1])))
                {
                    function = PDFunction.create(functionsArray.get(i));
                    PDRange encRange = getEncodeForParameter(i);
                    x = interpolate(x, partitionValues(i), partitionValues[i+1], encRange.getMin(), encRange.getMax());
                    break;
                }
            }
        }
        Single() functionValues = new Single(){x};
        // calculate the output values using the chosen function
        Single() functionResult = function.eval(functionValues);
        // clip to range if available
        return clipToRange(functionResult);
    }
    
    /**
     * Returns all functions values as COSArray.
     * 
     * @return the functions array. 
     */
    public COSArray getFunctions()
    {
        if (functions Is Nothing)
        {
            functions = (COSArray)(getDictionary().getDictionaryObject( COSName.FUNCTIONS ));
        }
        return functions;
    }
    
    /**
     * Returns all bounds values as COSArray.
     * 
     * @return the bounds array. 
     */
    public COSArray getBounds()
    {
        if (bounds Is Nothing) 
        {
            bounds = (COSArray)(getDictionary().getDictionaryObject( COSName.BOUNDS ));
        }
        return bounds;
    }
    
    /**
     * Returns all encode values as COSArray.
     * 
     * @return the encode array. 
     */
    public COSArray getEncode()
    {
        if (encode Is Nothing)
        {
            encode = (COSArray)(getDictionary().getDictionaryObject( COSName.ENCODE ));
        }
        return encode;
    }
    
    /**
     * Get the encode for the input parameter.
     *
     * @param paramNum The function parameter number.
     *
     * @return The encode parameter range or null if none is set.
     */
    private PDRange getEncodeForParameter(int n) 
    {
        COSArray encodeValues = getEncode();
        return new PDRange( encodeValues, n );
    }

}
