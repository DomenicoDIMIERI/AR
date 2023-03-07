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
'import java.util.HashMap;

Namespace org.apache.pdfbox.encoding.conversion

    '/**
    ' * This class provides a mapping from char code to unicode mapping files used for CJK-encoding.
    ' * @author Andreas Lehmkühler
    ' * @version $Revision: 1.0 $
    ' *
    ' */

    Public Class CMapSubstitution

        Private Shared cmapSubstitutions As New HashMap(Of String, String)

        Private Sub New()
        End Sub

        Shared Sub New()
            '// I don't know if these mappings are complete. Perhaps there 
            '// has to be added still one or more

            ' chinese simplified
            cmapSubstitutions.put("Adobe-GB1-4", "Adobe-GB1-UCS2")
            cmapSubstitutions.put("GBK-EUC-H", "GBK-EUC-UCS2")
            cmapSubstitutions.put("GBK-EUC-V", "GBK-EUC-UCS2")
            cmapSubstitutions.put("GBpc-EUC-H", "GBpc-EUC-UCS2C")
            cmapSubstitutions.put("GBpc-EUC-V", "GBpc-EUC-UCS2C")

            ' chinese traditional
            cmapSubstitutions.put("Adobe-CNS1-4", "Adobe-CNS1-UCS2")
            cmapSubstitutions.put("B5pc-H", "B5pc-UCS2")
            cmapSubstitutions.put("B5pc-V", "B5pc-UCS2")
            cmapSubstitutions.put("ETen-B5-H", "ETen-B5-UCS2")
            cmapSubstitutions.put("ETen-B5-V", "ETen-B5-UCS2")
            cmapSubstitutions.put("ETenms-B5-H", "ETen-B5-UCS2")
            cmapSubstitutions.put("ETenms-B5-V", "ETen-B5-UCS2")

            ' japanese
            cmapSubstitutions.put("90ms-RKSJ-H", "90ms-RKSJ-UCS2")
            cmapSubstitutions.put("90ms-RKSJ-V", "90ms-RKSJ-UCS2")
            cmapSubstitutions.put("90msp-RKSJ-H", "90ms-RKSJ-UCS2")
            cmapSubstitutions.put("90msp-RKSJ-V", "90ms-RKSJ-UCS2")
            cmapSubstitutions.put("90pv-RKSJ-H", "90pv-RKSJ-UCS2")
            cmapSubstitutions.put("UniJIS-UCS2-HW-H", "UniJIS-UCS2-H")
            cmapSubstitutions.put("Adobe-Japan1-4", "Adobe-Japan1-UCS2")

            cmapSubstitutions.put("Adobe-Identity-0", "Identity-H")
            cmapSubstitutions.put("Adobe-Identity-1", "Identity-H")
        End Sub

        '/**
        ' * 
        ' * @param cmapName The name of a cmap for which we have to find a possible substitution
        ' * @return the substitution for the given cmap name
        ' */
        Public Shared Function substituteCMap(ByVal cmapName As String) As String
            If (cmapSubstitutions.containsKey(cmapName)) Then
                Return cmapSubstitutions.get(cmapName)
            End If
            Return cmapName
        End Function

    End Class

End Namespace
