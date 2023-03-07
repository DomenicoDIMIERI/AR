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

Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common.filespecification


    '/**
    ' * A file specification that is just a string.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDSimpleFileSpecification
        Inherits PDFileSpecification

        Private file As COSString

        '/**
        ' * Constructor.
        ' *
        ' */
        Public Sub New()
            file = New COSString("")
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fileName The file that this spec represents.
        ' */
        Public Sub New(ByVal fileName As COSString)
            file = fileName
        End Sub

        '/**
        ' * This will get the file name.
        ' *
        ' * @return The file name.
        ' */
        Public Overrides Function getFile() As String
            Return file.getString()
        End Function

        '/**
        ' * This will set the file name.
        ' *
        ' * @param fileName The name of the file.
        ' */
        Public Overrides Sub setFile(ByVal fileName As String)
            file = New COSString(fileName)
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return file
        End Function

    End Class

End Namespace