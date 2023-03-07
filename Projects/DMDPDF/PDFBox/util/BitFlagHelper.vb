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


Namespace org.apache.pdfbox.util

    'import org.apache.pdfbox.cos.COSName;

    '/**
    ' * This class will be used for bit flag operations.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class BitFlagHelper

        Private Sub New()
        End Sub

        '/**
        ' * Sets the given boolean value at bitPos in the flags.
        ' *
        ' * @param dic The dictionary to set the value into.
        ' * @param field The name of the field to set the value into.
        ' * @param bitFlag the bit position to set the value in.
        ' * @param value the value the bit position should have.
        ' * 
        ' * @deprecated  use {@link #setFlag(COSDictionary, COSName, int, boolean)} using COSName constants instead
        ' */
        Public Shared Sub setFlag(ByVal dic As COSDictionary, ByVal field As String, ByVal bitFlag As Integer, ByVal value As Boolean)
            setFlag(dic, COSName.getPDFName(field), bitFlag, value)
        End Sub

        '/**
        ' * Sets the given boolean value at bitPos in the flags.
        ' *
        ' * @param dic The dictionary to set the value into.
        ' * @param field The COSName of the field to set the value into.
        ' * @param bitFlag the bit position to set the value in.
        ' * @param value the value the bit position should have.
        ' */
        Public Shared Sub setFlag(ByVal dic As COSDictionary, ByVal field As COSName, ByVal bitFlag As Integer, ByVal value As Boolean)
            Dim currentFlags As Integer = dic.getInt(field, 0)
            If (value) Then
                currentFlags = currentFlags Or bitFlag
            Else
                currentFlags = currentFlags And Not bitFlag
            End If
            dic.setInt(field, currentFlags)
        End Sub

        '/**
        ' * Gets the boolean value from the flags at the given bit
        ' * position.
        ' *
        ' * @param dic The dictionary to get the field from.
        ' * @param field The name of the field to get the flag from.
        ' * @param bitFlag the bitPosition to get the value from.
        ' *
        ' * @return true if the number at bitPos is '1'
        ' *
        ' * @deprecated  use {@link #getFlag(COSDictionary, COSName, boolean)} using COSName constants instead
        ' */
        Public Shared Function getFlag(ByVal dic As COSDictionary, ByVal field As String, ByVal bitFlag As Integer) As Boolean
            Return getFlag(dic, COSName.getPDFName(field), bitFlag)
        End Function

        '/**
        ' * Gets the boolean value from the flags at the given bit
        ' * position.
        ' *
        ' * @param dic The dictionary to get the field from.
        ' * @param field The COSName of the field to get the flag from.
        ' * @param bitFlag the bitPosition to get the value from.
        ' *
        ' * @return true if the number at bitPos is '1'
        ' */
        Public Shared Function getFlag(ByVal dic As COSDictionary, ByVal field As COSName, ByVal bitFlag As Integer) As Boolean
            Dim ff As Integer = dic.getInt(field, 0)
            Return (ff And bitFlag) = bitFlag
        End Function

    End Class


End Namespace