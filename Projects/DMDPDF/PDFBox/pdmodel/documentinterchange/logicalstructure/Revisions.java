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
import java.util.List;

/**
 * 
 * @author Koch
 * @version $Revision: $
 *
 * @param <T> the type of object to store the revision numbers with
 */
public class Revisions<T>
{

    private List<T> objects;
    private List<Integer> revisionNumbers;

    private List<T> getObjects()
    {
        if (Me.objects Is Nothing)
        {
            Me.objects = new ArrayList<T>();
        }
        return Me.objects;
    }

    private List<Integer> getRevisionNumbers()
    {
        if (Me.revisionNumbers Is Nothing)
        {
            Me.revisionNumbers = new ArrayList<Integer>();
        }
        return Me.revisionNumbers;
    }


    /**
     * 
     */
    public Revisions()
    {
    }


    /**
     * Returns the object at the specified position.
     * 
     * @param index the position
     * @return the object
     * @throws IndexOutOfBoundsException if the index is out of range
     */
    public T getObject(int index) throws IndexOutOfBoundsException
    {
        return Me.getObjects().get(index);
    }

    /**
     * Returns the revision number at the specified position.
     * 
     * @param index the position
     * @return the revision number
     * @throws IndexOutOfBoundsException if the index is out of range
     */
    public int getRevisionNumber(int index) throws IndexOutOfBoundsException
    {
        return Me.getRevisionNumbers().get(index);
    }

    /**
     * Adds an object with a specified revision number.
     * 
     * @param object the object
     * @param revisionNumber the revision number
     */
    public void addObject(T object, int revisionNumber)
    {
        Me.getObjects().add(object);
        Me.getRevisionNumbers().add(revisionNumber);
    }

    /**
     * Sets the revision number of a specified object.
     * 
     * @param object the object
     * @param revisionNumber the revision number
     */
    protected void setRevisionNumber(T object, int revisionNumber)
    {
        int index = Me.getObjects().indexOf(object);
        if (index > -1)
        {
            Me.getRevisionNumbers().set(index, revisionNumber);
        }
    }

    /**
     * Returns the size.
     * 
     * @return the size
     */
    public int size()
    {
        return Me.getObjects().size();
    }

    /**
     * {@inheritDoc}
     */
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < Me.getObjects().size(); i++)
        {
            if (i > 0)
            {
                sb.append("; ");
            }
            sb.append("object=").append(Me.getObjects().get(i))
                .append(", revisionNumber=").append(Me.getRevisionNumber(i));
        }
        return sb.toString();
    }

}
