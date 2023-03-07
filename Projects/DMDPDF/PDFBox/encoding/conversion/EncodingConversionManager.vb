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

'import java.util.Iterator;
'import java.util.HashMap;

Namespace org.apache.pdfbox.encoding.conversion

    '/**
    ' *  EncodingConversionManager maintains relationship between PDF encoding name
    ' *  and respective EncodingConverter instance. Those PDF encoding name like
    ' *  GBK-EUC-H should be converted to java charset name before constructing a
    ' *  java string instance
    ' *  
    ' *  @author  Pin Xue (http://www.pinxue.net), Holly Lee (holly.lee (at) gmail.com)
    ' *  @version $Revision: 1.0 $
    ' */
    Public Class EncodingConversionManager

        '/**
        ' *  Mapping from PDF encoding name to EncodingConverter instance.
        ' */
        Private Shared encodingMap As New HashMap(Of String, EncodingConverter)

        Private Sub New()
        End Sub

        '/**
        ' *  Initialize the encodingMap before anything calls us.
        ' */
        Shared Sub New()
            ' Add CJK encodings to map
            Dim it As Iterator = CJKEncodings.getEncodingIterator()

            While (it.hasNext())
                Dim encodingName As String = it.next()
                encodingMap.put(encodingName, New CJKConverter(encodingName))
            End While
            ' If there is any other encoding conversions, please add it here.
        End Sub


        '/**
        ' *  Get converter from given encoding name. If no converted defined,
        ' *  a null is returned.
        ' *  
        ' *  @param encoding search for a converter for the given encoding name
        ' *  @return the converter for the given encoding name
        ' */
        Public Shared Function getConverter(ByVal encoding As String) As EncodingConverter
            Return encodingMap.get(encoding)
        End Function

    End Class

End Namespace