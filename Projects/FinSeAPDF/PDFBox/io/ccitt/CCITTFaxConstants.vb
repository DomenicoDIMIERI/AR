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

'/* $Id: CCITTFaxConstants.java 1156213 2011-08-10 15:06:20Z jeremias $ */

Namespace org.apache.pdfbox.io.ccitt

    '/**
    ' * Constants for CCITT Fax Filter.
    ' * @version $Revision$
    ' */
    Public Class CCITTFaxConstants

        ''' <summary>
        ''' A constant for group 3 1D encoding (ITU T.4).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const COMPRESSION_GROUP3_1D As Integer = 0

        ''' <summary>
        ''' A constant for group 3 2D encoding (ITU T.4). 
        ''' </summary>
        ''' <remarks></remarks>
        Public Const COMPRESSION_GROUP3_2D As Integer = 1

        ''' <summary>
        ''' A constant for group 4 2D encoding (ITU T.6).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const COMPRESSION_GROUP4_2D As Integer = 2

        'Format: First 8 bits: length of pattern, Second 8 bits: pattern

        ''' <summary>
        ''' The white terminating code words.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly WHITE_TERMINATING() As Integer = { _
                &H835, &H607, &H407, &H408, &H40B, &H40C, &H40E, &H40F, _
                &H513, &H514, &H507, &H508, &H608, &H603, &H634, &H635, _
                &H62A, &H62B, &H727, &H70C, &H708, &H717, &H703, &H704, _
                &H728, &H72B, &H713, &H724, &H718, &H802, &H803, &H81A, _
                &H81B, &H812, &H813, &H814, &H815, &H816, &H817, &H828, _
                &H829, &H82A, &H82B, &H82C, &H82D, &H804, &H805, &H80A, _
                &H80B, &H852, &H853, &H854, &H855, &H824, &H825, &H858, _
                &H859, &H85A, &H85B, &H84A, &H84B, &H832, &H833, &H834}

        ''' <summary>
        ''' The black terminating code words.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly BLACK_TERMINATING() As Integer = { _
                &HA37, &H302, &H203, &H202, &H303, &H403, &H402, &H503, _
                &H605, &H604, &H704, &H705, &H707, &H804, &H807, &H918, _
                &HA17, &HA18, &HA08, &HB67, &HB68, &HB6C, &HB37, &HB28, _
                &HB17, &HB18, &HCCA, &HCCB, &HCCC, &HCCD, &HC68, &HC69, _
                &HC6A, &HC6B, &HCD2, &HCD3, &HCD4, &HCD5, &HCD6, &HCD7, _
                &HC6C, &HC6D, &HCDA, &HCDB, &HC54, &HC55, &HC56, &HC57, _
                &HC64, &HC65, &HC52, &HC53, &HC24, &HC37, &HC38, &HC27, _
                &HC28, &HC58, &HC59, &HC2B, &HC2C, &HC5A, &HC66, &HC67}

        ''' <summary>
        ''' The white make-up code words.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly WHITE_MAKE_UP() As Integer = { _
                &H51B, &H512, &H617, &H737, &H836, &H837, &H864, &H865, _
                &H868, &H867, &H9CC, &H9CD, &H9D2, &H9D3, &H9D4, &H9D5, _
                &H9D6, &H9D7, &H9D8, &H9D9, &H9DA, &H9DB, &H998, &H999, _
                &H99A, &H618, &H99B}

        ''' <summary>
        ''' The black make-up code words.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly BLACK_MAKE_UP() As Integer = { _
                &HA0F, &HCC8, &HCC9, &HC5B, &HC33, &HC34, &HC35, &HD6C, _
                &HD6D, &HD4A, &HD4B, &HD4C, &HD4D, &HD72, &HD73, &HD74, _
                &HD75, &HD76, &HD77, &HD52, &HD53, &HD54, &HD55, &HD5A, _
                &HD5B, &HD64, &HD65}

        ''' <summary>
        ''' The long make-up code words.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly LONG_MAKE_UP() As Integer = { _
                &HB08, &HB0C, &HB0D, &HC12, &HC13, &HC14, &HC15, &HC16, _
                &HC17, &HC1C, &HC1D, &HC1E, &HC1F}

        ''' <summary>
        ''' The EOL code word.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly EOL_CODE As Integer = &HC01

    End Class

End Namespace
