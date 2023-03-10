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
package org.apache.pdfbox.pdmodel.common;

import org.apache.pdfbox.cos.COSArray;
import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSInteger;
import org.apache.pdfbox.cos.COSFloat;
import org.apache.pdfbox.cos.COSString;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.cos.COSNull;
import org.apache.pdfbox.cos.COSNumber;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;

/**
 * This is an implementation of a List that will sync its contents to a COSArray.
 *
 * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
 * @version $Revision: 1.15 $
 */
public class COSArrayList<E> implements List<E>
{
    private COSArray array;
    private List<E> actual;

    private COSDictionary parentDict;
    private COSName dictKey;

    /**
     * Default constructor.
     */
    public COSArrayList()
    {
        array = new COSArray();
        actual = new ArrayList<E>();
    }

    /**
     * Constructor.
     *
     * @param actualList The list of standard java objects
     * @param cosArray The COS array object to sync to.
     */
    public COSArrayList( List<E> actualList, COSArray cosArray )
    {
        actual = actualList;
        array = cosArray;
    }

    /**
     * This is a really special constructor.  Sometimes the PDF spec says
     * that a dictionary entry can either be a single item or an array of those
     * items.  But in the PDModel interface we really just want to always return
     * a java.util.List.  In the case were we get the list and never modify it
     * we don't want to convert to COSArray and put one element, unless we append
     * to the list.  So here we are going to create this object with a single
     * item instead of a list, but allow more items to be added and then converted
     * to an array.
     *
     * @param actualObject The PDModel object.
     * @param item The COS Model object.
     * @param dictionary The dictionary that holds the item, and will hold the array if an item is added.
     * @param dictionaryKey The key into the dictionary to set the item.
     */
    public COSArrayList( E actualObject, COSBase item, COSDictionary dictionary, COSName dictionaryKey )
    {
        array = new COSArray();
        array.add( item );
        actual = new ArrayList<E>();
        actual.add( actualObject );

        parentDict = dictionary;
        dictKey = dictionaryKey;
    }

    /**
     * @deprecated use the {@link #COSArrayList(E, COSBase, COSDictionary, COSName)} method instead
     */
    public COSArrayList( E actualObject, COSBase item, COSDictionary dictionary, String dictionaryKey )
    {
        this( actualObject, item, dictionary, COSName.getPDFName(dictionaryKey) );
    }

    /**
     * {@inheritDoc}
     */
    public int size()
    {
        return actual.size();
    }

    /**
     * {@inheritDoc}
     */
    public boolean isEmpty()
    {
        return actual.isEmpty();
    }

    /**
     * {@inheritDoc}
     */
    public boolean contains(Object o)
    {
        return actual.contains(o);
    }

    /**
     * {@inheritDoc}
     */
    public Iterator<E> iterator()
    {
        return actual.iterator();
    }

    /**
     * {@inheritDoc}
     */
    public Object[] toArray()
    {
        return actual.toArray();
    }

    /**
     * {@inheritDoc}
     */
    public <X>X[] toArray(X[] a)
    {
        return actual.toArray(a);

    }

    /**
     * {@inheritDoc}
     */
    public boolean add(E o)
    {
        //when adding if there is a parentDict then change the item
        //in the dictionary from a single item to an array.
        if( parentDict IsNot Nothing )
        {
            parentDict.setItem( dictKey, array );
            //clear the parent dict so it doesn't happen again, there might be
            //a usecase for keeping the parentDict around but not now.
            parentDict = null;
        }
        //string is a special case because we can't subclass to be COSObjectable
        if( o instanceof String )
        {
            array.add( new COSString( (String)o ) );
        }
        else if( o instanceof DualCOSObjectable )
        {
            DualCOSObjectable dual = (DualCOSObjectable)o;
            array.add( dual.getFirstCOSObject() );
            array.add( dual.getSecondCOSObject() );
        }
        else
        {
            if(array IsNot Nothing)
            {
                array.add(((COSObjectable)o).getCOSObject());
            }
        }
        return actual.add(o);
    }

    /**
     * {@inheritDoc}
     */
    public boolean remove(Object o)
    {
        boolean retval = true;
        int index = actual.indexOf( o );
        if( index >= 0 )
        {
            actual.remove( index );
            array.remove( index );
        }
        else
        {
            retval = false;
        }
        return retval;
    }

    /**
     * {@inheritDoc}
     */
    public boolean containsAll(Collection<?> c)
    {
        return actual.containsAll( c );
    }

    /**
     * {@inheritDoc}
     */
    public boolean addAll(Collection<? extends E> c)
    {
        //when adding if there is a parentDict then change the item
        //in the dictionary from a single item to an array.
        if( parentDict IsNot Nothing && c.size() > 0)
        {
            parentDict.setItem( dictKey, array );
            //clear the parent dict so it doesn't happen again, there might be
            //a usecase for keeping the parentDict around but not now.
            parentDict = null;
        }
        array.addAll( toCOSObjectList( c ) );
        return actual.addAll( c );
    }

    /**
     * {@inheritDoc}
     */
    public boolean addAll(int index, Collection<? extends E> c)
    {
        //when adding if there is a parentDict then change the item
        //in the dictionary from a single item to an array.
        if( parentDict IsNot Nothing && c.size() > 0)
        {
            parentDict.setItem( dictKey, array );
            //clear the parent dict so it doesn't happen again, there might be
            //a usecase for keeping the parentDict around but not now.
            parentDict = null;
        }

        if( c.size() >0 && c.toArray()(0) instanceof DualCOSObjectable )
        {
            array.addAll( index*2, toCOSObjectList( c ) );
        }
        else
        {
            array.addAll( index, toCOSObjectList( c ) );
        }
        return actual.addAll( index, c );
    }

    /**
     * This will take an array of COSNumbers and return a COSArrayList of
     * java.lang.Integer values.
     *
     * @param intArray The existing integer Array.
     *
     * @return A list that is part of the core Java collections.
     */
    public static List<Integer> convertIntegerCOSArrayToList( COSArray intArray )
    {
        List<Integer> retval = null;
        if (intArray IsNot Nothing)
        {
            List<Integer> numbers = new ArrayList<Integer>();
            for( int i=0; i<intArray.size(); i++ )
            {
                numbers.add( new Integer( ((COSNumber)intArray.get( i )).intValue() ) );
            }
            retval = new COSArrayList<Integer>( numbers, intArray );
        }
        return retval;
    }

    /**
     * This will take an array of COSNumbers and return a COSArrayList of
     * java.lang.NFloat values.
     *
     * @param floatArray The existing Single Array.
     *
     * @return The list of NFloat objects.
     */
    public static List(Of Single) convertFloatCOSArrayToList( COSArray floatArray )
    {
        List(Of Single) retval = null;
        if( floatArray IsNot Nothing )
        {
            List(Of Single) numbers = new ArrayList(Of Single)();
            for( int i=0; i<floatArray.size(); i++ )
            {
                numbers.add( new NFloat( ((COSNumber)floatArray.get( i )).floatValue() ) );
            }
            retval = new COSArrayList(Of Single)( numbers, floatArray );
        }
        return retval;
    }

    /**
     * This will take an array of COSName and return a COSArrayList of
     * java.lang.String values.
     *
     * @param nameArray The existing name Array.
     *
     * @return The list of String objects.
     */
    public static List(Of String) convertCOSNameCOSArrayToList( COSArray nameArray )
    {
        List(Of String) retval = null;
        if( nameArray IsNot Nothing )
        {
            List(Of String)names = new ArrayList(Of String)();
            for( int i=0; i<nameArray.size(); i++ )
            {
                names.add( ((COSName)nameArray.getObject( i )).getName() );
            }
            retval = new COSArrayList(Of String)( names, nameArray );
        }
        return retval;
    }

    /**
     * This will take an array of COSString and return a COSArrayList of
     * java.lang.String values.
     *
     * @param stringArray The existing name Array.
     *
     * @return The list of String objects.
     */
    public static List(Of String) convertCOSStringCOSArrayToList( COSArray stringArray )
    {
        List(Of String) retval = null;
        if( stringArray IsNot Nothing )
        {
            List(Of String) string = new ArrayList(Of String)();
            for( int i=0; i<stringArray.size(); i++ )
            {
                string.add( ((COSString)stringArray.getObject( i )).getString() );
            }
            retval = new COSArrayList(Of String)( string, stringArray );
        }
        return retval;
    }

    /**
     * This will take an list of string objects and return a COSArray of COSName
     * objects.
     *
     * @param strings A list of strings
     *
     * @return An array of COSName objects
     */
    public static COSArray convertStringListToCOSNameCOSArray( List(Of String) strings )
    {
        COSArray retval = new COSArray();
        for( int i=0; i<strings.size(); i++ )
        {
            retval.add( COSName.getPDFName( strings.get( i ) ) );
        }
        return retval;
    }

    /**
     * This will take an list of string objects and return a COSArray of COSName
     * objects.
     *
     * @param strings A list of strings
     *
     * @return An array of COSName objects
     */
    public static COSArray convertStringListToCOSStringCOSArray( List(Of String) strings )
    {
        COSArray retval = new COSArray();
        for( int i=0; i<strings.size(); i++ )
        {
            retval.add( new COSString( strings.get( i ) ) );
        }
        return retval;
    }

    /**
     * This will convert a list of COSObjectables to an
     * array list of COSBase objects.
     *
     * @param cosObjectableList A list of COSObjectable.
     *
     * @return A list of COSBase.
     */
    public static COSArray converterToCOSArray( List<?> cosObjectableList )
    {
        COSArray array = null;
        if( cosObjectableList IsNot Nothing )
        {
            if( cosObjectableList instanceof COSArrayList )
            {
                //if it is already a COSArrayList then we don't want to recreate the array, we want to reuse it.
                array = ((COSArrayList<?>)cosObjectableList).array;
            }
            else
            {
                array = new COSArray();
                Iterator<?> iter = cosObjectableList.iterator();
                while( iter.hasNext() )
                {
                    Object next = iter.next();
                    if( next instanceof String )
                    {
                        array.add( new COSString( (String)next ) );
                    }
                    else if( next instanceof Integer || next instanceof Long )
                    {
                        array.add( COSInteger.get( ((Number)next).longValue() ) );
                    }
                    else if( next instanceof NFloat || next instanceof Double )
                    {
                        array.add( new COSFloat( ((Number)next).floatValue() ) );
                    }
                    else if( next instanceof COSObjectable )
                    {
                        COSObjectable object = (COSObjectable)next;
                        array.add( object.getCOSObject() );
                    }
                    else if( next instanceof DualCOSObjectable )
                    {
                        DualCOSObjectable object = (DualCOSObjectable)next;
                        array.add( object.getFirstCOSObject() );
                        array.add( object.getSecondCOSObject() );
                    }
                    else if( next Is Nothing )
                    {
                        array.add( COSNull.NULL );
                    }
                    else
                    {
                        throw new RuntimeException( "Error: Don't know how to convert type to COSBase '" +
                        next.getClass().getName() + "'" );
                    }
                }
            }
        }
        return array;
    }

    private List(Of COSBase) toCOSObjectList( Collection<?> list )
    {
        List(Of COSBase) cosObjects = new ArrayList(Of COSBase)();
        Iterator<?> iter = list.iterator();
        while( iter.hasNext() )
        {
            Object next = iter.next();
            if( next instanceof String )
            {
                cosObjects.add( new COSString( (String)next ) );
            }
            else if( next instanceof DualCOSObjectable )
            {
                DualCOSObjectable object = (DualCOSObjectable)next;
                array.add( object.getFirstCOSObject() );
                array.add( object.getSecondCOSObject() );
            }
            else
            {
                COSObjectable cos = (COSObjectable)next;
                cosObjects.add( cos.getCOSObject() );
            }
        }
        return cosObjects;
    }

    /**
     * {@inheritDoc}
     */
    public boolean removeAll(Collection<?> c)
    {
        array.removeAll( toCOSObjectList( c ) );
        return actual.removeAll( c );
    }

    /**
     * {@inheritDoc}
     */
    public boolean retainAll(Collection<?> c)
    {
        array.retainAll( toCOSObjectList( c ) );
        return actual.retainAll( c );
    }

    /**
     * {@inheritDoc}
     */
    public void clear()
    {
        //when adding if there is a parentDict then change the item
        //in the dictionary from a single item to an array.
        if( parentDict IsNot Nothing )
        {
            parentDict.setItem( dictKey, (COSBase)null );
        }
        actual.clear();
        array.clear();
    }

    /**
     * {@inheritDoc}
     */
    public boolean equals(Object o)
    {
        return actual.equals( o );
    }

    /**
     * {@inheritDoc}
     */
    public int hashCode()
    {
        return actual.hashCode();
    }

    /**
     * {@inheritDoc}
     */
    public E get(int index)
    {
        return actual.get( index );

    }

    /**
     * {@inheritDoc}
     */
    public E set(int index, E element)
    {
        if( element instanceof String )
        {
            COSString item = new COSString( (String)element );
            if( parentDict IsNot Nothing && index == 0 )
            {
                parentDict.setItem( dictKey, item );
            }
            array.set( index, item );
        }
        else if( element instanceof DualCOSObjectable )
        {
            DualCOSObjectable dual = (DualCOSObjectable)element;
            array.set( index*2, dual.getFirstCOSObject() );
            array.set( index*2+1, dual.getSecondCOSObject() );
        }
        else
        {
            if( parentDict IsNot Nothing && index == 0 )
            {
                parentDict.setItem( dictKey, ((COSObjectable)element).getCOSObject() );
            }
            array.set( index, ((COSObjectable)element).getCOSObject() );
        }
        return actual.set( index, element );
    }

    /**
     * {@inheritDoc}
     */
    public void add(int index, E element)
    {
        //when adding if there is a parentDict then change the item
        //in the dictionary from a single item to an array.
        if( parentDict IsNot Nothing )
        {
            parentDict.setItem( dictKey, array );
            //clear the parent dict so it doesn't happen again, there might be
            //a usecase for keeping the parentDict around but not now.
            parentDict = null;
        }
        actual.add( index, element );
        if( element instanceof String )
        {
            array.add( index, new COSString( (String)element ) );
        }
        else if( element instanceof DualCOSObjectable )
        {
            DualCOSObjectable dual = (DualCOSObjectable)element;
            array.add( index*2, dual.getFirstCOSObject() );
            array.add( index*2+1, dual.getSecondCOSObject() );
        }
        else
        {
            array.add( index, ((COSObjectable)element).getCOSObject() );
        }
    }

    /**
     * {@inheritDoc}
     */
    public E remove(int index)
    {
        if( array.size() > index && array.get( index ) instanceof DualCOSObjectable )
        {
            //remove both objects
            array.remove( index );
            array.remove( index );
        }
        else
        {
            array.remove( index );
        }
        return actual.remove( index );
    }

    /**
     * {@inheritDoc}
     */
    public int indexOf(Object o)
    {
        return actual.indexOf( o );
    }

    /**
     * {@inheritDoc}
     */
    public int lastIndexOf(Object o)
    {
        return actual.indexOf( o );

    }

    /**
     * {@inheritDoc}
     */
    public ListIterator<E> listIterator()
    {
        return actual.listIterator();
    }

    /**
     * {@inheritDoc}
     */
    public ListIterator<E> listIterator(int index)
    {
        return actual.listIterator( index );
    }

    /**
     * {@inheritDoc}
     */
    public List<E> subList(int fromIndex, int toIndex)
    {
        return actual.subList( fromIndex, toIndex );
    }
    
    /**
     * {@inheritDoc}
     */
    public String toString()
    {
        return "COSArrayList{" + array.toString() + "}";
    }
    
    /**
     * This will return then underlying COSArray.
     * 
     * @return the COSArray
     */
    public COSArray toList() 
    {
        return array;
    }

}
