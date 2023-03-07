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
Namespace org.apache.pdfbox.pdmodel.common.function.type4

    '/**
    ' * Interface for PostScript operators.
    ' *
    ' * @version $Revision$
    ' */
    Public Interface [Operator]

        '/**
        ' * Executes the operator. The method can inspect and manipulate the stack.
        ' * @param context the execution context
        ' */
        Sub execute(ByVal context As ExecutionContext)

    End Interface

End Namespace
