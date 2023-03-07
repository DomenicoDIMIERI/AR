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
Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common


Namespace org.apache.pdfbox.pdmodel.common.filespecification


    '/**
    ' * This represents a file specification.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public MustInherit Class PDFileSpecification
        Implements COSObjectable

        '/**
        ' * A file specfication can either be a COSString or a COSDictionary.  This
        ' * will create the file specification either way.
        ' *
        ' * @param base The cos object that describes the fs.
        ' *
        ' * @return The file specification for the COSBase object.
        ' *
        ' * @throws IOException If there is an error creating the file spec.
        ' */
        Public Shared Function createFS(ByVal base As COSBase) As PDFileSpecification 'throws IOException
            Dim retval As PDFileSpecification = Nothing
            If (base Is Nothing) Then
                'then simply return null
            ElseIf (TypeOf (base) Is COSString) Then
                retval = New PDSimpleFileSpecification(base)
            ElseIf (TypeOf (base) Is COSDictionary) Then
                retval = New PDComplexFileSpecification(base)
            Else
                Throw New IOException("Error: Unknown file specification " & base.ToString)
            End If
            Return retval
        End Function

        ''' <summary>
        ''' This will get the file name.
        ''' </summary>
        ''' <remarks></remarks>
        Public MustOverride Function getFile() As String

        ''' <summary>
        ''' This will set the file name.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub setFile(ByVal file As String)

        Public MustOverride Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject


    End Class

End Namespace
