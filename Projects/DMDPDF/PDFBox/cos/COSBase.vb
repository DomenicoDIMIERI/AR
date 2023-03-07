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
Imports FinSeA.org.apache.pdfbox.filter
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.exceptions

Namespace org.apache.pdfbox.cos


    '/**
    ' * The base object that all objects in the PDF document will extend.
    ' *
    ' * @author <a href="ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.14 $
    ' */
    Public MustInherit Class COSBase
        Implements COSObjectable

        '/**
        ' * Constructor.
        ' */

        Private needToBeUpdate As Boolean

        Private direct As Boolean

        Public Sub New()
            Me.needToBeUpdate = False
        End Sub

        '/**
        ' * This will get the filter manager to use to filter streams.
        ' *
        ' * @return The filter manager.
        ' */
        Public Function getFilterManager() As FilterManager
            '/**
            ' * @todo move this to PDFdocument or something better
            ' */
            Return New FilterManager()
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me
        End Function

        Public Function hashCode() As Integer
            Return Me.GetHashCode
        End Function

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public MustOverride Function accept(ByVal visitor As ICOSVisitor) As Object  'throws COSVisitorException;

        Public Sub setNeedToBeUpdate(ByVal flag As Boolean)
            Me.needToBeUpdate = flag
        End Sub

        '/**
        ' * If the state is set true, the dictionary will be written direct into the called object. 
        ' * This means, no indirect object will be created.
        ' * 
        ' * @return the state
        ' */
        Public Function isDirect() As Boolean
            Return Me.direct
        End Function

        '/**
        ' * Set the state true, if the dictionary should be written as a direct object and not indirect.
        ' * 
        ' * @param direct set it true, for writting direct object
        ' */
        Public Sub setDirect(ByVal direct As Boolean)
            Me.direct = direct
        End Sub

        Public Function isNeedToBeUpdate() As Boolean
            Return Me.needToBeUpdate
        End Function

    End Class

End Namespace
