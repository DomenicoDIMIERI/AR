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
'imports java.io.IOException;'
'imports java.io.OutputStream;

'imports org.apache.pdfbox.exceptions.COSVisitorException;
Imports System.IO

Namespace org.apache.pdfbox.cos

    '   /**
    '* This class represents a null PDF object.
    '*
    '* @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    '* @version $Revision: 1.13 $
    '*/
    Public Class COSNull
        Inherits COSBase
        '/**
        ' * The null token.
        ' */
        Public Shared ReadOnly NULL_BYTES As Byte() = {110, 117, 108, 108} ' //"null".getBytes( "ISO-8859-1" );

        '/**
        ' * The one null object in the system.
        ' */
        Public Shared ReadOnly NULL As New COSNull()

        '/**
        ' * Constructor.
        ' */
        Private Sub New()
            'limit creation to one instance.
        End Sub

        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object 'throws COSVisitorException
            Return visitor.visitFromNull(Me)
        End Function

        '/**
        ' * This will output this string as a PDF object.
        ' *
        ' * @param output The stream to write to.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub writePDF(ByVal output As Stream) 'throws IOException
            output.Write(NULL_BYTES, 0, 1 + UBound(NULL_BYTES))
        End Sub

    End Class

End Namespace
