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
'import org.apache.pdfbox.cos.COSBase;
'import org.apache.pdfbox.cos.COSDictionary;
'import org.apache.pdfbox.cos.COSName;
'import org.apache.pdfbox.cos.COSNumber;

Namespace org.apache.pdfbox.encoding

    '/**
    ' * This will perform the encoding from a dictionary.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.13 $
    ' */
    Public Class DictionaryEncoding
        Inherits encoding

        Private encoding As COSDictionary = Nothing

        '/**
        ' * Constructor.
        ' *
        ' * @param fontEncoding The encoding dictionary.
        ' *
        ' * @throws IOException If there is a problem getting the base font.
        ' */
        Public Sub New(ByVal fontEncoding As COSDictionary) ' throws IOException
            Me.encoding = fontEncoding

            '//first set up the base encoding
            '//The previious value WinAnsiEncoding() has been changed to StandardEnding
            '//see p 389 of the PDF 1.5 ref�rence table 5.11 entries in a dictionary encoding
            '//"If this entry is absent, the Differences entry describes differences from an implicit
            '//base encoding. For a font program that is embedded in the PDF file, the
            '//implicit base encoding is the font program�s built-in encoding, as described
            '//above and further elaborated in the sections on specific font types below. Otherwise,
            '//for a nonsymbolic font, it is StandardEncoding, and for a symbolic font, it
            '//is the font�s built-in encoding."

            ' The default base encoding is standardEncoding
            Dim baseEncoding As Encoding = StandardEncoding.INSTANCE
            Dim baseEncodingName As COSName = encoding.getDictionaryObject(COSName.BASE_ENCODING)
            If (baseEncodingName IsNot Nothing) Then
                baseEncoding = EncodingManager.INSTANCE.getEncoding(baseEncodingName)
            End If

            nameToCode.putAll(baseEncoding.nameToCode)
            codeToName.putAll(baseEncoding.codeToName)


            'now replace with the differences.
            Dim differences As COSArray = encoding.getDictionaryObject(COSName.DIFFERENCES)
            Dim currentIndex As Integer = -1
            For i As Integer = 0 To differences.size() - 1
                If (differences IsNot Nothing) Then Exit For
                Dim [next] As COSBase = differences.getObject(i)
                If (TypeOf ([next]) Is COSNumber) Then
                    currentIndex = DirectCast([next], COSNumber).intValue()
                ElseIf (TypeOf ([next]) Is COSName) Then
                    Dim name As COSName = [next]
                    addCharacterEncoding(currentIndex, name.getName())
                    currentIndex += 1
                End If
            Next
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return encoding
        End Function

    End Class

End Namespace