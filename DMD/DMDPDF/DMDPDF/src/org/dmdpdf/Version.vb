'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.metadata

Imports System
Imports System.Collections.Generic
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf

    '/**
    '  <summary>Generic PDF version number [PDF:1.6:H.1].</summary>
    '  <seealso cref="VersionEnum"/>
    '*/
    Public NotInheritable Class Version
        Implements IVersion

#Region "Static"

#Region "fields"

        Private Shared ReadOnly versionPattern As Regex = New Regex("^(\d+)\.(\d+)$")
        Private Shared ReadOnly versions As New Dictionary(Of String, Version) 'IDictionary<string,Version>

#End Region

#Region "Interface"

#Region "Public"

        Public Shared Function [Get](ByVal version As PdfName) As Version
            Return [Get](version.RawValue)
        End Function

        Public Shared Function [Get](ByVal version As String) As Version
            If (Not versions.ContainsKey(version)) Then
                Dim versionMatch As Match = versionPattern.Match(version)
                If (Not versionMatch.Success) Then Throw New Exception("Invalid PDF version format: '" & versionPattern.ToString & "' pattern expected.")
                Dim versionObject As New Version(Int32.Parse(versionMatch.Groups(1).Value), Int32.Parse(versionMatch.Groups(2).Value))
                versions(version) = versionObject
            End If
            Return versions(version)
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private ReadOnly _major As Integer
        Private ReadOnly _minor As Integer

#End Region

#Region "constructors"

        Private Sub New(ByVal major As Integer, ByVal minor As Integer)
            Me._major = major
            Me._minor = minor
        End Sub

#End Region

#Region "Interface"

#Region "Public"

        Public ReadOnly Property Major As Integer
            Get
                Return Me._major
            End Get
        End Property

        Public ReadOnly Property Minor As Integer
            Get
                Return Me._minor
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return VersionUtils.ToString(Me)
        End Function

#Region "IVersion"

        Public ReadOnly Property Numbers() As IList(Of Integer) Implements IVersion.Numbers
            Get
                Return New List(Of Integer)({Major, Minor})
            End Get
        End Property


#Region "IComparable"

        Public Function CompareTo(ByVal value As IVersion) As Integer Implements IComparable(Of IVersion).CompareTo
            Return VersionUtils.CompareTo(Me, value)
        End Function

#End Region
#End Region
#End Region
#End Region
#End Region
    End Class

End Namespace
