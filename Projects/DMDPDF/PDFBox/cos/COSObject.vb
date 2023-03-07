'/*
' * Licensed to the Apache Software Foundation (ASF) under one or more
' * contributor license agreements.  See the NOTICE file distributed with
' * this work for additional information regarding copyright ownership.
' * The ASF licenses this file to You under the Apache License, Version 2.0
' * (the "License"); you may not use this file except in compliance with
' * the License.  You may obtain a copy of the License at
' *
' *      http://www.apache.org/licenses/LICENSE-2.0
' *
' * Unless required by applicable law or agreed to in writing, software
' * distributed under the License is distributed on an "AS IS" BASIS,
' * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' * See the License for the specific language governing permissions and
' * limitations under the License.
' */

'import org.apache.pdfbox.exceptions.COSVisitorException;

'import java.io.IOException;
Namespace org.apache.pdfbox.cos

    '/**
    ' * This class represents a PDF object.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.37 $
    ' */
    Public Class COSObject
        Inherits COSBase

        Private baseObject As COSBase
        Private objectNumber As COSInteger
        Private generationNumber As COSInteger

        '/**
        ' * Constructor.
        ' *
        ' * @param object The object that this encapsulates.
        ' *
        ' * @throws IOException If there is an error with the object passed in.
        ' */
        Public Sub New(ByVal [object] As COSBase) ' throws IOException
            Me.setObject([object])
        End Sub

        '/**
        ' * This will get the dictionary object in this object that has the name key and
        ' * if it is a pdfobjref then it will dereference that and return it.
        ' *
        ' * @param key The key to the value that we are searching for.
        ' *
        ' * @return The pdf object that matches the key.
        ' */
        Public Function getDictionaryObject(ByVal key As COSName) As COSBase
            Dim retval As COSBase = Nothing
            If (TypeOf (Me.baseObject) Is COSDictionary) Then
                retval = DirectCast(baseObject, COSDictionary).getDictionaryObject(key)
            End If
            Return retval
        End Function

        '/**
        ' * This will get the dictionary object in this object that has the name key.
        ' *
        ' * @param key The key to the value that we are searching for.
        ' *
        ' * @return The pdf object that matches the key.
        ' */
        Public Function getItem(ByVal key As COSName) As COSBase
            Dim retval As COSBase = Nothing
            If (TypeOf (Me.baseObject) Is COSDictionary) Then
                retval = DirectCast(Me.baseObject, COSDictionary).getItem(key)
            End If
            Return retval
        End Function

        '/**
        ' * This will get the object that this object encapsulates.
        ' *
        ' * @return The encapsulated object.
        ' */
        Public Function getObject() As COSBase
            Return Me.baseObject
        End Function

        '/**
        ' * This will set the object that this object encapsulates.
        ' *
        ' * @param object The new object to encapsulate.
        ' *
        ' * @throws IOException If there is an error setting the updated object.
        ' */
        Public Sub setObject(ByVal [object] As COSBase) 'throws IOException
            Me.baseObject = [object]
            '/*if( baseObject Is Nothing )
            '{
            '    baseObject = object;
            '}
            '    Else
            '{
            '    //This is for when an object appears twice in the
            '    //pdf file we really want to replace it such that
            '    //object references still work correctly.
            '    //see owcp-as-received.pdf for an example
            '    if( baseObject instanceof COSDictionary )
            '    {
            '        COSDictionary dic = (COSDictionary)baseObject;
            '        COSDictionary dicObject = (COSDictionary)object;
            '        dic.clear();
            '        dic.addAll( dicObject );
            '    }
            '    else if( baseObject instanceof COSArray )
            '    {
            '        COSArray array = (COSArray)baseObject;
            '        COSArray arrObject = (COSArray)object;
            '        array.clear();
            '        for( int i=0; i<arrObject.size(); i++ )
            '        {
            '            array.add( arrObject.get( i ) );
            '        }
            '    }
            '    else if( baseObject instanceof COSStream )
            '    {
            '        COSStream oldStream = (COSStream)baseObject;
            '        System.out.println( "object:" +  object.getClass().getName() );
            '        COSStream newStream = (COSStream)object;
            '        oldStream.replaceWithStream( newStream );
            '    }
            '    else if( baseObject instanceof COSInteger )
            '    {
            '        COSInteger oldInt = (COSInteger)baseObject;
            '        COSInteger newInt = (COSInteger)object;
            '        oldInt.setValue( newInt.longValue() );
            '    }
            '    else if( baseObject Is Nothing )
            '    {
            '        baseObject = object;
            '    }
            '    else
            '    {
            '        throw new IOException( "Unknown object substitution type:" + baseObject );
            '    }
            '}*/

        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function toString() As String
            Dim ret As String = "COSObject{"
            If (objectNumber Is Nothing) Then
                ret &= "unknown"
            Else
                ret &= objectNumber.intValue() & ", "
            End If
            If (generationNumber Is Nothing) Then
                ret &= "unknown"
            Else
                ret &= generationNumber.intValue()
            End If
            ret &= "}"
            Return ret
        End Function

        '/** Getter for property objectNumber.
        ' * @return Value of property objectNumber.
        ' */
        Public Function getObjectNumber() As COSInteger
            Return Me.objectNumber
        End Function

        '/** Setter for property objectNumber.
        ' * @param objectNum New value of property objectNumber.
        ' */
        Public Sub setObjectNumber(ByVal objectNum As COSInteger)
            Me.objectNumber = objectNum
        End Sub

        '/** Getter for property generationNumber.
        ' * @return Value of property generationNumber.
        ' */
        Public Function getGenerationNumber() As COSInteger
            Return Me.generationNumber
        End Function

        '/** Setter for property generationNumber.
        ' * @param generationNumberValue New value of property generationNumber.
        ' */
        Public Sub setGenerationNumber(ByVal generationNumberValue As COSInteger)
            Me.generationNumber = generationNumberValue
        End Sub

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) 'throws COSVisitorException
            If (Me.getObject() IsNot Nothing) Then
                Return Me.getObject().accept(visitor)
            Else
                Return COSNull.NULL.accept(visitor)
            End If

        End Function

    End Class

End Namespace