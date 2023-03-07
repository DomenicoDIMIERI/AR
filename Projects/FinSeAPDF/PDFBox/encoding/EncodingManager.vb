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

'import java.io.IOException;
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.encoding

    '/**
    ' * This class will handle getting the appropriate encodings.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.9 $
    ' */
    Public Class EncodingManager

        '/**
        ' * Default singleton instance of this class.
        ' *
        ' * @since Apache PDFBox 1.3.0
        ' */
        Public Shared ReadOnly INSTANCE As New EncodingManager()

        '/**
        ' * This will get the standard encoding.
        ' *
        ' * @return The standard encoding.
        ' */
        Public Function getStandardEncoding() As Encoding
            Return StandardEncoding.INSTANCE
        End Function

        '/**
        ' * This will get an encoding by name.
        ' *
        ' * @param name The name of the encoding to get.
        ' * @return The encoding that matches the name.
        ' * @throws IOException if there is no encoding with that name.
        ' */
        Public Function getEncoding(ByVal name As COSName) As Encoding 'throws IOException {
            If (COSName.STANDARD_ENCODING.equals(name)) Then
                Return StandardEncoding.INSTANCE
            ElseIf (COSName.WIN_ANSI_ENCODING.equals(name)) Then
                Return WinAnsiEncoding.INSTANCE
            ElseIf (COSName.MAC_ROMAN_ENCODING.equals(name)) Then
                Return MacRomanEncoding.INSTANCE
            ElseIf (COSName.PDF_DOC_ENCODING.equals(name)) Then
                Return PdfDocEncoding.INSTANCE
            Else
                Throw New NotSupportedException("Unknown encoding for '" & name.getName() & "'")
            End If
        End Function

    End Class

End Namespace
