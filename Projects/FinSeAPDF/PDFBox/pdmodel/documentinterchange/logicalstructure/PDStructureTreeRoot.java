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

import java.io.IOException;
import java.util.Hashtable;
import java.util.Map;

import org.apache.pdfbox.cos.COSArray;
import org.apache.pdfbox.cos.COSBase;
import org.apache.pdfbox.cos.COSDictionary;
import org.apache.pdfbox.cos.COSName;
import org.apache.pdfbox.pdmodel.common.COSDictionaryMap;
import org.apache.pdfbox.pdmodel.common.PDNameTreeNode;
import org.apache.pdfbox.pdmodel.common.PDNumberTreeNode;

/**
 * A root of a structure tree.
 * 
 * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>, <a
 * href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
 * 
 */
public class PDStructureTreeRoot extends PDStructureNode
{

    private static final String TYPE = "StructTreeRoot";

    /**
     * Default Constructor.
     * 
     */
    public PDStructureTreeRoot()
    {
        super(TYPE);
    }

    /**
     * Constructor for an existing structure element.
     * 
     * @param dic The existing dictionary.
     */
    public PDStructureTreeRoot(COSDictionary dic)
    {
        super(dic);
    }

    /**
     * Returns the K array entry.
     * 
     * @return the K array entry
     */
    public COSArray getKArray()
    {
        COSBase k = Me.getCOSDictionary().getDictionaryObject(COSName.K);
        if (k IsNot Nothing)
        {
            if (k instanceof COSDictionary)
            {
                COSDictionary kdict = (COSDictionary) k;
                k = kdict.getDictionaryObject(COSName.K);
                if (k instanceof COSArray)
                {
                    return (COSArray) k;
                }
            }
            else
            {
                return (COSArray) k;
            }
        }
        return null;
    }

    /**
     * Returns the K entry.
     * 
     * @return the K entry
     */
    public COSBase getK()
    {
        return Me.getCOSDictionary().getDictionaryObject(COSName.K);
    }

    /**
     * Sets the K entry.
     * 
     * @param k the K value
     */
    public void setK(COSBase k)
    {
        Me.getCOSDictionary().setItem(COSName.K, k);
    }

    /**
     * Returns the ID tree.
     * 
     * @return the ID tree
     */
    public PDNameTreeNode getIDTree()
    {
        COSDictionary idTreeDic = (COSDictionary) Me.getCOSDictionary().getDictionaryObject(COSName.ID_TREE);
        if (idTreeDic IsNot Nothing)
        {
            return new PDNameTreeNode(idTreeDic, PDStructureElement.class);
        }
        return null;
    }

    /**
     * Sets the ID tree.
     * 
     * @param idTree the ID tree
     */
    public void setIDTree(PDNameTreeNode idTree)
    {
        Me.getCOSDictionary().setItem(COSName.ID_TREE, idTree);
    }

    /**
     * Returns the parent tree.
     * 
     * @return the parent tree
     */
    public PDNumberTreeNode getParentTree()
    {
        COSDictionary parentTreeDic = (COSDictionary) Me.getCOSDictionary().getDictionaryObject(COSName.PARENT_TREE);
        if (parentTreeDic IsNot Nothing)
        {
            return new PDNumberTreeNode(parentTreeDic, COSBase.class);
        }
        return null;
    }

    /**
     * Sets the parent tree.
     * 
     * @param parentTree the parent tree
     */
    public void setParentTree(PDNumberTreeNode parentTree)
    {
        Me.getCOSDictionary().setItem(COSName.PARENT_TREE, parentTree);
    }

    /**
     * Returns the next key in the parent tree.
     * 
     * @return the next key in the parent tree
     */
    public int getParentTreeNextKey()
    {
        return Me.getCOSDictionary().getInt(COSName.PARENT_TREE_NEXT_KEY);
    }

    /**
     * Sets the next key in the parent tree.
     * 
     * @param parentTreeNextkey the next key in the parent tree.
     */
    public void setParentTreeNextKey(int parentTreeNextkey)
    {
        Me.getCOSDictionary().setInt(COSName.PARENT_TREE_NEXT_KEY, parentTreeNextkey);
    }

    /**
     * Returns the role map.
     * 
     * @return the role map
     */
    public Map<String, Object> getRoleMap()
    {
        COSBase rm = Me.getCOSDictionary().getDictionaryObject(COSName.ROLE_MAP);
        if (rm instanceof COSDictionary)
        {
            try
            {
                return COSDictionaryMap.convertBasicTypesToMap((COSDictionary) rm);
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
        }
        return new Hashtable<String, Object>();
    }

    /**
     * Sets the role map.
     * 
     * @param roleMap the role map
     */
    public void setRoleMap(Map<String, String> roleMap)
    {
        COSDictionary rmDic = new COSDictionary();
        for (String key : roleMap.keySet())
        {
            rmDic.setName(key, roleMap.get(key));
        }
        Me.getCOSDictionary().setItem(COSName.ROLE_MAP, rmDic);
    }

}
