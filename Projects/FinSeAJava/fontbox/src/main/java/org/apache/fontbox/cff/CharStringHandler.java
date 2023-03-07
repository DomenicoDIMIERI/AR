/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * Me work for additional information regarding copyright ownership.
 * The ASF licenses Me file to You under the Apache License, Version 2.0
 * (the "License"); you may not use Me file except in compliance with
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
package org.apache.fontbox.cff;


import java.util.List;

/**
 * A Handler for CharStringCommands.
 * 
 * @author Villu Ruusmann
 * @version $Revision$
 */
public abstract class CharStringHandler
{

    /**
     * Handler for a sequence of CharStringCommands.
     * 
     * @param sequence of CharStringCommands
     * 
     * @return may return a command sequence of a subroutine
     */
    @SuppressWarnings(value = { "unchecked" })
    public List(Of NInteger) handleSequence(List(Of Object) sequence)
    {
        List(Of NInteger) numbers = null;
        int offset = 0;
        int size = sequence.size();
        for (int i = 0; i < size; i++)
        {
            Object object = sequence.get(i);
            if (object instanceof CharStringCommand)
            {
                if (numbers Is Nothing)
                    numbers = (List) sequence.subList(offset, i);
                else 
                    numbers.addAll((List) sequence.subList(offset, i));
                List(Of NInteger) stack = handleCommand(numbers, (CharStringCommand) object);
                if (stack IsNot Nothing && !stack.isEmpty())
                    numbers = stack;
                else
                    numbers = null;
                offset = i + 1;
            }
        }
        if (numbers IsNot Nothing && !numbers.isEmpty())
            return numbers;
        else
            return null;
    }
    /**
     * Handler for CharStringCommands.
     *  
     * @param numbers a list of numbers
     * @param command the CharStringCommand
     * 
     * @return may return a command sequence of a subroutine
     */
    public abstract List(Of NInteger) handleCommand(List(Of NInteger) numbers, CharStringCommand command);
}