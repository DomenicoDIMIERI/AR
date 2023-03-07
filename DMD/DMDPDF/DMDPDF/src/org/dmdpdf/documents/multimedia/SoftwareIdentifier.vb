'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.interaction
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.math
Imports DMD.org.dmdpdf.util.metadata

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Software identifier [PDF:1.7:9.1.6].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class SoftwareIdentifier
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
        '/**
        '  <summary>Software version number [PDF:1.7:9.1.6].</summary>
        '*/
        Public NotInheritable Class VersionObject
            Inherits PdfObjectWrapper(Of PdfArray)
            Implements IVersion

            Public Sub New(ParamArray numbers As Integer())
                MyBase.New(New PdfArray())
                Dim baseDataObject As PdfArray = Me.BaseDataObject
                For Each number As Integer In numbers
                    baseDataObject.Add(New PdfInteger(number))
                Next
            End Sub

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub


            Public Function CompareTo(ByVal value As IVersion) As Integer Implements IVersion.CompareTo
                Return VersionUtils.CompareTo(Me, value)
            End Function

            Public ReadOnly Property Numbers As IList(Of Integer) Implements IVersion.Numbers
                Get
                    Dim _numbers As IList(Of Integer) = New List(Of Integer)
                    For Each numberObject As PdfDirectObject In BaseDataObject
                        _numbers.Add(CType(numberObject, PdfInteger).IntValue)
                    Next
                    Return _numbers
                End Get
            End Property

            Public Overrides Function ToString() As String
                Return VersionUtils.ToString(Me)
            End Function
        End Class


#End Region

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As SoftwareIdentifier
            If (baseObject IsNot Nothing) Then
                Return New SoftwareIdentifier(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the operating system identifiers that indicate which operating systems this
        '  object applies to.</summary>
        '  <remarks>The defined values are the same as those defined for SMIL 2.0's systemOperatingSystem
        '  attribute. An empty list is considered to represent all operating systems.</remarks>
        '*/
        Public ReadOnly Property OSes As IList(Of String)
            Get
                Dim _oses As IList(Of String) = New List(Of String)
                Dim osesObject As PdfArray = CType(Me.BaseDataObject(PdfName.OS), PdfArray)
                If (osesObject IsNot Nothing) Then
                    For Each osObject As PdfDirectObject In osesObject
                        _oses.Add(CType(osObject, PdfString).StringValue)
                    Next
                End If
                Return _oses
            End Get
        End Property

        '/**
        '  <summary>Gets the URI that identifies a piece of software.</summary>
        '  <remarks>It is interpreted according to its scheme; the only presently defined scheme is
        '  vnd.adobe.swname. The scheme name is case-insensitive; if is not recognized by the viewer
        '  application, the software must be considered a non-match. The syntax of URIs of this scheme is
        '  "vnd.adobe.swname:" software_name where software_name is equivalent to reg_name as defined in
        '  Internet RFC 2396, Uniform Resource Identifiers (URI): Generic Syntax.</remarks>
        '*/
        Public ReadOnly Property URI As Uri
            Get
                Dim uriObject As PdfString = CType(Me.BaseDataObject(PdfName.U), PdfString)
                If (uriObject IsNot Nothing) Then
                    Return New Uri(uriObject.StringValue)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        '/**
        '  <summary>Gets the software version bounds.</summary>
        '*/
        Public ReadOnly Property Version As Interval(Of VersionObject)
            Get
                Dim baseDataObject As PdfDictionary = Me.BaseDataObject
                Return New Interval(Of VersionObject)(
                            New VersionObject(CType(baseDataObject(PdfName.L), PdfArray)),
                            New VersionObject(CType(baseDataObject(PdfName.H), PdfArray)),
                            CBool(PdfBoolean.GetValue(baseDataObject(PdfName.LI), True)),
                            CBool(PdfBoolean.GetValue(baseDataObject(PdfName.HI), True))
                            )
            End Get
        End Property

        'TODO:setters!!!
#End Region
#End Region
#End Region

    End Class

End Namespace
