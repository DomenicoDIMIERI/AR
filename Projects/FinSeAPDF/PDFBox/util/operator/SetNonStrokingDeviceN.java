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
package org.apache.pdfbox.util.operator;

import java.io.IOException;
import java.util.List;

import org.apache.pdfbox.cos.COSArray;
import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.pdmodel.graphics.color.PDColorSpace;
import org.apache.pdfbox.pdmodel.graphics.color.PDColorState;
import org.apache.pdfbox.pdmodel.graphics.color.PDDeviceN;
import org.apache.pdfbox.util.PDFOperator;

/**
 * 
 * @version $Revision: 1.0 $
 */
public class SetNonStrokingDeviceN extends OperatorProcessor 
{

    /**
     * scn Set color space for non stroking operations.
     * @param operator The operator that is being executed.
     * @param arguments List
     * @throws IOException If an error occurs while processing the font.
     */
    public void process(PDFOperator operator, List(Of COSBase) arguments) throws IOException
    {
        PDColorState colorInstance = context.getGraphicsState().getNonStrokingColor();
        PDColorSpace colorSpace = colorInstance.getColorSpace();
        
        if (colorSpace IsNot Nothing) 
        {
            PDDeviceN sep = (PDDeviceN) colorSpace;
            colorSpace = sep.getAlternateColorSpace();
            COSArray colorValues = sep.calculateColorValues(arguments);
            colorInstance.setColorSpaceValue(colorValues.toFloatArray());
        }
    }
}
