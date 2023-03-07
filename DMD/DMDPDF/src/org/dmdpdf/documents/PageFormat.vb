'/*
'  Copyright 2007-2010 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

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

Imports System
Imports System.Drawing
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.documents

    '/**
    '  <summary>Page format.</summary>
    '  <remarks>This utility provides an easy access to the dimension of common page formats.</remarks>
    '*/
    Public NotInheritable Class PageFormat

#Region "types"
        '/**
        '  <summary>Paper size.</summary>
        '  <remarks>
        '    References:
        '    <list type="bullet">
        '      <item>{ 'A' digit+ }: [ISO 216] "A" series: Paper and boards, trimmed sizes.</item>
        '      <item>{ 'B' digit+ }: [ISO 216] "B" series: Posters, wall charts and similar items.</item>
        '      <item>{ 'C' digit+ }: [ISO 269] "C" series: Envelopes or folders suitable for A-size
        '      stationery.</item>
        '      <item>{ 'Ansi' letter }: [ANSI/ASME Y14.1] ANSI series: US engineering drawing series.</item>
        '      <item>{ 'Arch' letter }: Architectural series.</item>
        '      <item>{ "Letter", "Legal", "Executive", "Statement", "Tabloid" }: Traditional north-american
        '      sizes.</item>
        '    </list>
        '  </remarks>
        '*/
        Public Enum SizeEnum As Integer
            A0
            A1
            A2
            A3
            A4
            A5
            A6
            A7
            A8
            A9
            A10

            B0
            B1
            B2
            B3
            B4
            B5
            B6
            B7
            B8
            B9
            B10

            C0
            C1
            C2
            C3
            C4
            C5
            C6
            C7
            C8
            C9
            C10

            Letter
            Legal
            Executive
            Statement
            Tabloid

            ArchA
            ArchB
            ArchC
            ArchD
            ArchE

            AnsiA
            AnsiB
            AnsiC
            AnsiD
            AnsiE
        End Enum

        '/**
        '  <summary>Page orientation.</summary>
        '*/
        Public Enum OrientationEnum As Integer
            Portrait
            Landscape
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly IsoSeriesSize_A As String = "A"
        Private Shared ReadOnly IsoSeriesSize_B As String = "B"
        Private Shared ReadOnly IsoSeriesSize_C As String = "C"

        Private Shared ReadOnly IsoSeriesSizePattern As Regex = New Regex("([" & IsoSeriesSize_A & IsoSeriesSize_B & IsoSeriesSize_C & "])([\d]+)")

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets the default page size.</summary>
        '  <remarks>The returned dimension corresponds to the widely-established ISO A4 standard paper
        '  format, portrait orientation.</remarks>
        '*/
        Public Shared Function GetSize() As Size
            Return GetSize(SizeEnum.A4)
        End Function

        '/**
        '  <summary>Gets the page size of the given format, portrait orientation.</summary>
        '  <param name="size">Page size.</param>
        '*/
        Public Shared Function GetSize(ByVal size As SizeEnum) As Size
            Return GetSize(Size, OrientationEnum.Portrait)
        End Function

        '/**
        '  <summary>Gets the page size of the given format and orientation.</summary>
        '  <param name="size">Page size.</param>
        '  <param name="orientation">Page orientation.</param>
        '*/
        Public Shared Function GetSize(ByVal size As SizeEnum, ByVal orientation As OrientationEnum) As Size
            Dim width As Integer, height As Integer = 0

            ' Size.
            Dim sizeName As String = size.ToString()
            Dim match As Match = IsoSeriesSizePattern.Match(sizeName)
            ' Is it an ISO standard size?
            If (match.Success) Then
                Dim baseWidth As Integer, baseHeight As Integer = 0
                Dim isoSeriesSize As String = match.Groups(1).Value
                If (isoSeriesSize.Equals(IsoSeriesSize_A)) Then
                    baseWidth = 2384
                    baseHeight = 3370
                ElseIf (isoSeriesSize.Equals(IsoSeriesSize_B)) Then
                    baseWidth = 2834
                    baseHeight = 4008
                ElseIf (isoSeriesSize.Equals(IsoSeriesSize_C)) Then
                    baseWidth = 2599
                    baseHeight = 3676
                Else
                    Throw New NotImplementedException("Paper format " & size & " not supported yet.")
                End If

                Dim isoSeriesSizeIndex As Integer = Int32.Parse(match.Groups(2).Value)
                Dim isoSeriesSizeFactor As Double = 1D / Math.Pow(2, isoSeriesSizeIndex / 2D)

                width = CInt(Math.Floor(baseWidth * isoSeriesSizeFactor))
                height = CInt(Math.Floor(baseHeight * isoSeriesSizeFactor))
            Else ' Non-ISO size.
                Select Case (size)
                    Case SizeEnum.ArchA : width = 648 : height = 864' break;
                    Case SizeEnum.ArchB : width = 864 : height = 1296' break;
                    Case SizeEnum.ArchC : width = 1296 : height = 1728' break;
                    Case SizeEnum.ArchD : width = 1728 : height = 2592' break;
                    Case SizeEnum.ArchE : width = 2592 : height = 3456' break;
                    Case SizeEnum.AnsiA : Case SizeEnum.Letter : width = 612 : height = 792' break;
                    Case SizeEnum.AnsiB : Case SizeEnum.Tabloid : width = 792 : height = 1224' break;
                    Case SizeEnum.AnsiC : width = 1224 : height = 1584' break;
                    Case SizeEnum.AnsiD : width = 1584 : height = 2448' break;
                    Case SizeEnum.AnsiE : width = 2448 : height = 3168' break;
                    Case SizeEnum.Legal : width = 612 : height = 1008' break;
                    Case SizeEnum.Executive : width = 522 : height = 756' break;
                    Case SizeEnum.Statement : width = 396 : height = 612 ' break;
                    Case Else : Throw New NotImplementedException("Paper format " & size & " not supported yet.")
                End Select
            End If

            ' Orientation.
            Select Case (orientation)
                Case OrientationEnum.Portrait : Return New Size(width, height)
                Case OrientationEnum.Landscape : Return New Size(height, width)
                Case Else : Throw New NotImplementedException("Orientation " & orientation & " not supported yet.")
            End Select
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Private Sub New()
        End Sub

#End Region
#End Region

    End Class

End Namespace
