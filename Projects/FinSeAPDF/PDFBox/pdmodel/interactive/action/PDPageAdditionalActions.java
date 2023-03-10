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
package org.apache.pdfbox.pdmodel.interactive.action;

import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSDictionary;

import org.apache.pdfbox.pdmodel.common.COSObjectable;
import org.apache.pdfbox.pdmodel.interactive.action.type.PDAction;

/**
 * This class represents a page object's dictionary of actions
 * that occur due to events.
 *
 * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
 * @author Panagiotis Toumasis (ptoumasis@mail.gr)
 * @version $Revision: 1.2 $
 */
public class PDPageAdditionalActions implements COSObjectable
{
    private COSDictionary actions;

    /**
     * Default constructor.
     */
    public PDPageAdditionalActions()
    {
        actions = new COSDictionary();
    }

    /**
     * Constructor.
     *
     * @param a The action dictionary.
     */
    public PDPageAdditionalActions( COSDictionary a )
    {
        actions = a;
    }

    /**
     * Convert this standard java object to a COS object.
     *
     * @return The cos object that matches this Java object.
     */
    public COSBase getCOSObject()
    {
        return actions;
    }

    /**
     * Convert this standard java object to a COS object.
     *
     * @return The cos object that matches this Java object.
     */
    public COSDictionary getCOSDictionary()
    {
        return actions;
    }

    /**
     * This will get an action to be performed when the page
     * is opened. This action is independent of any that may be
     * defined by the OpenAction entry in the document catalog,
     * and is executed after such an action.
     *
     * @return The O entry of page object's additional actions dictionary.
     */
    public PDAction getO()
    {
        COSDictionary o = (COSDictionary)actions.getDictionaryObject( "O" );
        PDAction retval = null;
        if( o IsNot Nothing )
        {
            retval = PDActionFactory.createAction( o );
        }
        return retval;
    }

    /**
     * This will set an action to be performed when the page
     * is opened. This action is independent of any that may be
     * defined by the OpenAction entry in the document catalog,
     * and is executed after such an action.
     *
     * @param o The action to be performed.
     */
    public void setO( PDAction o )
    {
        actions.setItem( "O", o );
    }

    /**
     * This will get an action to be performed when the page
     * is closed. This action applies to the page being closed,
     * and is executed before any other page opened.
     *
     * @return The C entry of page object's additional actions dictionary.
     */
    public PDAction getC()
    {
        COSDictionary c = (COSDictionary)actions.getDictionaryObject( "C" );
        PDAction retval = null;
        if( c IsNot Nothing )
        {
            retval = PDActionFactory.createAction( c );
        }
        return retval;
    }

    /**
     * This will set an action to be performed when the page
     * is closed. This action applies to the page being closed,
     * and is executed before any other page opened.
     *
     * @param c The action to be performed.
     */
    public void setC( PDAction c )
    {
        actions.setItem( "C", c );
    }
}
