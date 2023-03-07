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
    ' * This represents a file specification.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDComplexFileSpecification
        Inherits PDFileSpecification

        Private fs As COSDictionary

        '/**
        ' * Default Constructor.
        ' */
        Public Sub New()
            fs = New COSDictionary()
            fs.setItem(COSName.TYPE, COSName.FILESPEC)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict The dictionary that fulfils this file specification.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            fs = dict
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return fs
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return fs
        End Function

        '/**
        ' * <p>Preferred method for getting the filename.
        ' * It will determinate the recommended file name.</p>
        ' * <p>First of all we try to get the unicode filename if it exist.
        ' * If it doesn't exist we take a look at the DOS, MAC UNIX filenames.
        ' * If no one exist the required F entry will be returned.</p>
        ' *
        ' * @return The preferred file name.
        ' */
        Public Function getFilename() As String
            If (getUnicodeFile() <> "") Then
                Return getUnicodeFile()
            ElseIf (getFileDos() <> "") Then
                Return getFileDos()
            ElseIf (getFileMac() <> "") Then
                Return getFileMac()
            ElseIf (getFileUnix() <> "") Then
                Return getFileUnix()
            Else
                Return getFile()
            End If
        End Function

        '/**
        ' * This will get the unicode file name.
        ' *
        ' * @return The file name.
        ' */
        Public Function getUnicodeFile() As String
            Return fs.getString(COSName.UF)
        End Function

        '/**
        ' * This will get the file name.
        ' *
        ' * @return The file name.
        ' */
        Public Overrides Function getFile() As String
            Return fs.getString(COSName.F)
        End Function

        '/**
        ' * This will set the file name.
        ' *
        ' * @param file The name of the file.
        ' */
        Public Overrides Sub setFile(ByVal file As String)
            fs.setString(COSName.F, file)
        End Sub

        '/**
        ' * This will get the name representing a Dos file.
        ' *
        ' * @return The file name.
        ' */
        Public Function getFileDos() As String
            Return fs.getString(COSName.DOS)
        End Function

        '/**
        ' * This will set name representing a dos file.
        ' *
        ' * @param file The name of the file.
        ' */
        Public Sub setFileDos(ByVal file As String)
            fs.setString(COSName.DOS, file)
        End Sub

        '/**
        ' * This will get the name representing a Mac file.
        ' *
        ' * @return The file name.
        ' */
        Public Function getFileMac() As String
            Return fs.getString(COSName.MAC)
        End Function

        '/**
        ' * This will set name representing a Mac file.
        ' *
        ' * @param file The name of the file.
        ' */
        Public Sub setFileMac(ByVal file As String)
            fs.setString(COSName.MAC, file)
        End Sub

        '/**
        ' * This will get the name representing a Unix file.
        ' *
        ' * @return The file name.
        ' */
        Public Function getFileUnix() As String
            Return fs.getString(COSName.UNIX)
        End Function

        '/**
        ' * This will set name representing a Unix file.
        ' *
        ' * @param file The name of the file.
        ' */
        Public Sub setFileUnix(ByVal file As String)
            fs.setString(COSName.UNIX, file)
        End Sub

        '/**
        ' * Tell if the underlying file is volatile and should not be cached by the
        ' * reader application.  Default: false
        ' *
        ' * @param fileIsVolatile The new value for the volatility of the file.
        ' */
        Public Sub setVolatile(ByVal fileIsVolatile As Boolean)
            fs.setBoolean(COSName.V, fileIsVolatile)
        End Sub

        '/**
        ' * Get if the file is volatile.  Default: false
        ' *
        ' * @return True if the file is volatile attribute is set.
        ' */
        Public Function isVolatile() As Boolean
            Return fs.getBoolean(COSName.V, False)
        End Function

        '/**
        ' * Get the embedded file.
        ' *
        ' * @return The embedded file for this file spec.
        ' */
        Public Function getEmbeddedFile() As PDEmbeddedFile
            Dim file As PDEmbeddedFile = Nothing
            Dim stream As COSStream = fs.getObjectFromPath("EF/F")
            If (stream IsNot Nothing) Then
                file = New PDEmbeddedFile(stream)
            End If
            Return file
        End Function

        '/**
        ' * Set the embedded file for this spec.
        ' *
        ' * @param file The file to be embedded.
        ' */
        Public Sub setEmbeddedFile(ByVal file As PDEmbeddedFile)
            Dim ef As COSDictionary = fs.getDictionaryObject(COSName.EF)
            If (ef Is Nothing AndAlso file IsNot Nothing) Then
                ef = New COSDictionary()
                fs.setItem(COSName.EF, ef)
            End If
            If (ef IsNot Nothing) Then
                ef.setItem(COSName.F, file)
            End If
        End Sub

        '/**
        ' * Get the embedded dos file.
        ' *
        ' * @return The embedded file for this file spec.
        ' */
        Public Function getEmbeddedFileDos() As PDEmbeddedFile
            Dim file As PDEmbeddedFile = Nothing
            Dim stream As COSStream = fs.getObjectFromPath("EF/DOS")
            If (stream IsNot Nothing) Then
                file = New PDEmbeddedFile(stream)
            End If
            Return file
        End Function

        '/**
        ' * Set the embedded dos file for this spec.
        ' *
        ' * @param file The dos file to be embedded.
        ' */
        Public Sub setEmbeddedFileDos(ByVal file As PDEmbeddedFile)
            Dim ef As COSDictionary = fs.getDictionaryObject(COSName.DOS)
            If (ef Is Nothing AndAlso file IsNot Nothing) Then
                ef = New COSDictionary()
                fs.setItem(COSName.EF, ef)
            End If
            If (ef IsNot Nothing) Then
                ef.setItem(COSName.DOS, file)
            End If
        End Sub

        '/**
        ' * Get the embedded Mac file.
        ' *
        ' * @return The embedded file for this file spec.
        ' */
        Public Function getEmbeddedFileMac() As PDEmbeddedFile
            Dim file As PDEmbeddedFile = Nothing
            Dim stream As COSStream = fs.getObjectFromPath("EF/Mac")
            If (stream IsNot Nothing) Then
                file = New PDEmbeddedFile(stream)
            End If
            Return file
        End Function

        '/**
        ' * Set the embedded Mac file for this spec.
        ' *
        ' * @param file The Mac file to be embedded.
        ' */
        Public Sub setEmbeddedFileMac(ByVal file As PDEmbeddedFile)
            Dim ef As COSDictionary = fs.getDictionaryObject(COSName.MAC)
            If (ef Is Nothing AndAlso file IsNot Nothing) Then
                ef = New COSDictionary()
                fs.setItem(COSName.EF, ef)
            End If
            If (ef IsNot Nothing) Then
                ef.setItem(COSName.MAC, file)
            End If
        End Sub

        '/**
        ' * Get the embedded Unix file.
        ' *
        ' * @return The embedded file for this file spec.
        ' */
        Public Function getEmbeddedFileUnix() As PDEmbeddedFile
            Dim file As PDEmbeddedFile = Nothing
            Dim stream As COSStream = fs.getObjectFromPath("EF/Unix")
            If (stream IsNot Nothing) Then
                file = New PDEmbeddedFile(stream)
            End If
            Return file
        End Function

        '/**
        ' * Set the embedded Unix file for this spec.
        ' *
        ' * @param file The Unix file to be embedded.
        ' */
        Public Sub setEmbeddedFileUnix(ByVal file As PDEmbeddedFile)
            Dim ef As COSDictionary = fs.getDictionaryObject(COSName.UNIX)
            If (ef Is Nothing AndAlso file IsNot Nothing) Then
                ef = New COSDictionary()
                fs.setItem(COSName.EF, ef)
            End If
            If (ef IsNot Nothing) Then
                ef.setItem(COSName.UNIX, file)
            End If
        End Sub

        '/**
        ' * Set the file description.
        ' * 
        ' * @param description The file description
        ' */
        Public Sub setFileDescription(ByVal description As String)
            fs.setString(COSName.DESC, description)
        End Sub

        '/**
        ' * This will get the description.
        ' *
        ' * @return The file description.
        ' */
        Public Function getFileDescription() As String
            Return fs.getString(COSName.DESC)
        End Function

    End Class

End Namespace