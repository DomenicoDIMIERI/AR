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

Namespace org.apache.pdfbox.exceptions

    '/**
    ' * This exception will be thrown when a local destination(page within the same PDF) is required
    ' * but the bookmark(PDOutlineItem) refers to an external destination or an action that does not
    ' * point to a page.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class OutlineNotLocalException
        Inherits System.IO.IOException

        '/**
        ' * Constructor.
        ' *
        ' * @param msg An error message.
        ' */
        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub

    End Class

End Namespace
