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
'import java.io.File;
'import java.io.FileInputStream;
'import java.io.InputStream;
'import java.io.IOException;

'import java.util.Properties;
Imports System.IO

Namespace org.apache.pdfbox.util

    '/**
    ' * This class will handle loading resource files(AFM/CMAP).
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.9 $
    ' */
    Public Class ResourceLoader


        '/**
        ' * private constructor for utility class.
        ' */
        Private Sub New()
            '//private utility class
        End Sub

        '/**
        ' * This will attempt to load the resource given the resource name.
        ' *
        ' * @param resourceName The resource to try and load.
        ' *
        ' * @return The resource as a stream or null if it could not be found.
        ' *
        ' * @throws IOException If there is an error while attempting to load the resource.
        ' */
        Public Shared Function loadResource(ByVal resourceName As String) As FinSeA.Io.InputStream  ' throws IOException
            resourceName = Replace(resourceName, "/", "\")
            Dim fn As String = FinSeA.Sistema.FileSystem.GetBaseName(resourceName)
            fn = Replace(fn, "-", "_")

            Dim [is] As System.IO.Stream = Nothing
            Dim tmp As Object = My.Resources.ResourceManager.GetObject(fn)
            Dim bytes() As Byte = Nothing

            If (TypeOf (tmp) Is Byte()) Then
                Try
                    bytes = tmp
                Catch ex As Exception
                    'ignore
                End Try
            ElseIf (TypeOf (tmp) Is String) Then
                bytes = System.Text.Encoding.Default.GetBytes(tmp)
            Else
                Debug.Print(TypeName(tmp))
            End If

            If (bytes IsNot Nothing) Then [is] = New FinSeA.Io.InputStream(New System.IO.MemoryStream(bytes))
            bytes = Nothing
            tmp = Nothing

            If ([is] Is Nothing) Then
                If (File.Exists(resourceName)) Then
                    [is] = New FinSeA.Io.FileInputStream(resourceName)
                End If
            End If

            Return [is]
        End Function

        '/**
        ' * This will attempt to load the resource given the resource name.
        ' *
        ' * @param resourceName The resource to try and load.
        ' * @param failIfNotFound Throw an error message if the properties were not found.
        ' *
        ' * @return The resource as a stream or null if it could not be found.
        ' *
        ' * @throws IOException If there is an error loading the properties.
        ' */
        Public Shared Function loadProperties(ByVal resourceName As String, ByVal failIfNotFound As Boolean) As Properties 'throws IOException 
            Dim properties As Properties = Nothing
            Dim [is] As Stream = Nothing 'InputStream 
            Try
                [is] = loadResource(resourceName)
                If ([is] IsNot Nothing) Then
                    properties = New Properties()
                    properties.load([is])
                Else
                    If (failIfNotFound) Then
                        Throw New IOException("Error: could not find resource '" & resourceName & "' on classpath.")
                    End If
                End If
            Finally
                If ([is] IsNot Nothing) Then
                    [is].Close()
                End If
            End Try
            Return properties
        End Function

        '/**
        ' * This will attempt to load the resource given the resource name.
        ' *
        ' * @param resourceName The resource to try and load.
        ' * @param defaults A stream of default properties.
        ' *
        ' * @return The resource as a stream or null if it could not be found.
        ' *
        ' * @throws IOException If there is an error loading the properties.
        ' */
        Public Shared Function loadProperties(ByVal resourceName As String, ByVal defaults As Properties) As Properties ' throws IOException 
            Dim [is] As Stream = Nothing
            Try
                [is] = loadResource(resourceName)
                If ([is] IsNot Nothing) Then
                    defaults.load([is])
                End If
            Finally
                If ([is] IsNot Nothing) Then
                    [is].close()
                End If
            End Try
            Return defaults
        End Function

    End Class

End Namespace
