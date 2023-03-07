'/*
'  Copyright 2006-2011 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it):
'      - porting and adaptation (extension to any bit depth other than 8) of [JT]
'        predictor-decoding implementation.
'    * Joshua Tauberer (code contributor, http://razor.occams.info):
'      - predictor-decoding contributor on .NET implementation.
'    * Jean-Claude Truy (bugfix contributor): [FIX:0.0.8:JCT].

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.objects

Imports System
Imports System.IO
Imports System.IO.Compression

Namespace DMD.org.dmdpdf.bytes.filters

    '/**
    '  <summary> zlib/deflate [RFC:1950,1951] filter [PDF:1.6:3.3.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class FlateFilter
        Inherits Filter

#Region "dynamic"

#Region "constructors"

        Friend Sub New()
        End Sub

#End Region

#Region "Interface"

#Region "Public"

        Public Overrides Function Decode(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer, ByVal parameters As PdfDictionary) As Byte()
            Dim outputStream As New MemoryStream()
            Dim inputStream As New MemoryStream(data, offset, length)
            Dim inputFilter As New DeflateStream(inputStream, CompressionMode.Decompress)
            inputStream.Position = 2 ' Skips zlib's 2-byte header [RFC 1950] [FIX:0.0.8:JCT].
            Transform(inputFilter, outputStream)
            Return DecodePredictor(outputStream.ToArray(), parameters)
        End Function

        Public Overrides Function Encode(ByVal data As Byte(), ByVal offset As Integer, ByVal length As Integer, ByVal parameters As PdfDictionary) As Byte()
            Dim inputStream As New MemoryStream(data, offset, length)
            Dim outputStream As New MemoryStream()
            Dim outputFilter As New DeflateStream(outputStream, CompressionMode.Compress, True)
            ' Add zlib's 2-byte header [RFC 1950] [FIX:0.0.8:JCT]!
            outputStream.WriteByte(&H78) ' CMF = {CINFO (bits 7-4) = 7; CM (bits 3-0) = 8} = 0x78.
            outputStream.WriteByte(&HDA) ' FLG = {FLEVEL (bits 7-6) = 3; FDICT (bit 5) = 0; FCHECK (bits 4-0) = {31 - ((CMF * 256 + FLG - FCHECK) Mod 31)} = 26} = 0xDA.
            Transform(inputStream, outputFilter)
            Return outputStream.ToArray()
        End Function

#End Region

#Region "Private"

        Private Function DecodePredictor(ByVal data As Byte(), ByVal parameters As PdfDictionary) As Byte()
            If (parameters Is Nothing) Then Return data

            Dim predictor As Integer
            If (parameters.ContainsKey(PdfName.Predictor)) Then
                predictor = CType(parameters(PdfName.Predictor), PdfInteger).RawValue
            Else
                predictor = 1
            End If
            If (predictor = 1) Then Return data '// No Then predictor was applied during data encoding.

            Dim sampleComponentBitsCount As Integer
            If (parameters.ContainsKey(PdfName.BitsPerComponent)) Then
                sampleComponentBitsCount = CType(parameters(PdfName.BitsPerComponent), PdfInteger).RawValue
            Else
                sampleComponentBitsCount = 8
            End If

            Dim sampleComponentsCount As Integer
            If (parameters.ContainsKey(PdfName.Colors)) Then
                sampleComponentsCount = CType(parameters(PdfName.Colors), PdfInteger).RawValue
            Else
                sampleComponentsCount = 1
            End If

            Dim rowSamplesCount As Integer
            If (parameters.ContainsKey(PdfName.Columns)) Then
                rowSamplesCount = CType(parameters(PdfName.Columns), PdfInteger).RawValue
            Else
                rowSamplesCount = 1
            End If

            Dim input As New MemoryStream(data)
            Dim output As New MemoryStream()

            Select Case (predictor)
                Case 2 ' TIFF Predictor 2 (component-based).
                    Dim sampleComponentPredictions As Integer() = New Integer(sampleComponentsCount - 1) {}
                    Dim sampleComponentDelta As Integer = 0
                    Dim sampleComponentIndex As Integer = 0
                    sampleComponentDelta = input.ReadByte()
                    While (sampleComponentDelta <> -1)
                        Dim sampleComponent As Integer = sampleComponentDelta + sampleComponentPredictions(sampleComponentIndex)
                        output.WriteByte(CByte(sampleComponent))

                        sampleComponentPredictions(sampleComponentIndex) = sampleComponent
                        'sampleComponentIndex = + +sampleComponentIndex % sampleComponentsCount
                        sampleComponentIndex = (sampleComponentIndex + 1) Mod sampleComponentsCount
                        sampleComponentDelta = input.ReadByte()
                    End While
                    'break;
                Case Else ' PNG Predictors [RFC 2083] (byte-based).
                    Dim sampleBytesCount = CInt(Math.Ceiling(sampleComponentBitsCount * sampleComponentsCount / 8D)) ' Number Of bytes per pixel (bpp).
                    Dim rowSampleBytesCount As Integer = CInt(Math.Ceiling(sampleComponentBitsCount * sampleComponentsCount * rowSamplesCount / 8D)) + sampleBytesCount ' Number Of bytes per row (comprising a leading upper-left sample (see Paeth method)).
                    Dim previousRowBytePredictions As Integer() = New Integer(rowSampleBytesCount - 1) {}
                    Dim currentRowBytePredictions As Integer() = New Integer(rowSampleBytesCount - 1) {}
                    Dim leftBytePredictions As Integer() = New Integer(sampleBytesCount - 1) {}
                    Dim predictionMethod As Integer
                    predictionMethod = input.ReadByte()
                    While (predictionMethod <> -1)
                        Array.Copy(currentRowBytePredictions, 0, previousRowBytePredictions, 0, currentRowBytePredictions.Length)
                        Array.Clear(leftBytePredictions, 0, leftBytePredictions.Length)
                        'Starts after the leading upper-left sample (see Paeth method).
                        For rowSampleByteIndex As Integer = sampleBytesCount To rowSampleBytesCount - 1
                            Dim byteDelta As Integer = input.ReadByte()
                            Dim sampleByteIndex As Integer = rowSampleByteIndex Mod sampleBytesCount
                            Dim sampleByte As Integer

                            Select Case (predictionMethod)
                                Case 0 ' None (no prediction).
                                    sampleByte = byteDelta
                                'break;
                                Case 1 ' Sub (predicts the same as the sample to the left).
                                    sampleByte = byteDelta + leftBytePredictions(sampleByteIndex)
                                'break;
                                Case 2 ' Up (predicts the same as the sample above).
                                    sampleByte = byteDelta + previousRowBytePredictions(rowSampleByteIndex)
                                'break;
                                Case 3     ' Average (predicts the average of the sample to the left And the sample above).
                                    sampleByte = byteDelta + CInt(Math.Floor(((leftBytePredictions(sampleByteIndex) + previousRowBytePredictions(rowSampleByteIndex)))) / 2D)
                                'break;
                                Case 4     ' Paeth (a nonlinear function of the sample above, the sample to the left, And the sample to the upper left).
                                    Dim paethPrediction As Integer
                                    '{
                                    Dim leftBytePrediction As Integer = leftBytePredictions(sampleByteIndex)
                                    Dim topBytePrediction As Integer = previousRowBytePredictions(rowSampleByteIndex)
                                    Dim topLeftBytePrediction As Integer = previousRowBytePredictions(rowSampleByteIndex - sampleBytesCount)
                                    Dim initialPrediction As Integer = leftBytePrediction + topBytePrediction - topLeftBytePrediction
                                    Dim leftPrediction As Integer = Math.Abs(initialPrediction - leftBytePrediction)
                                    Dim topPrediction As Integer = Math.Abs(initialPrediction - topBytePrediction)
                                    Dim topLeftPrediction As Integer = Math.Abs(initialPrediction - topLeftBytePrediction)
                                    If (leftPrediction <= topPrediction AndAlso leftPrediction <= topLeftPrediction) Then
                                        paethPrediction = leftBytePrediction
                                    ElseIf (topPrediction <= topLeftPrediction) Then
                                        paethPrediction = topBytePrediction
                                    Else
                                        paethPrediction = topLeftBytePrediction
                                    End If
                                    '}
                                    sampleByte = byteDelta + paethPrediction
                                    'break;
                                Case Else
                                    Throw New NotSupportedException("Prediction method " & predictionMethod & " unknown.")
                            End Select

                            output.WriteByte(CByte(sampleByte And &HFF))

                            currentRowBytePredictions(rowSampleByteIndex) = sampleByte
                            leftBytePredictions(sampleByteIndex) = sampleByte
                        Next

                        'break;
                        predictionMethod = input.ReadByte()
                    End While
            End Select

            Return output.ToArray()
        End Function

        Private Sub Transform(ByVal input As System.IO.Stream, ByVal output As System.IO.Stream)
            Dim buffer As Byte() = New Byte(8192 - 1) {}
            Dim bufferLength As Integer

            bufferLength = input.Read(buffer, 0, buffer.Length)
            While (bufferLength <> 0)
                output.Write(buffer, 0, bufferLength)
                bufferLength = input.Read(buffer, 0, buffer.Length)
            End While

            input.Close()
            output.Close()
        End Sub

#End Region
#End Region
#End Region

    End Class


End Namespace
