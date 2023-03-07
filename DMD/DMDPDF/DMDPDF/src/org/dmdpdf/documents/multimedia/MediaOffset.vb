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
'Imports DMD.actions = org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Media offset [PDF:1.7:9.1.5].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public MustInherit Class MediaOffset
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
        '/**
        '  <summary>Media offset frame [PDF:1.7:9.1.5].</summary>
        '*/
        Public NotInheritable Class Frame
            Inherits MediaOffset

            Public Sub New(ByVal context As Document, ByVal value As Integer)
                MyBase.New(context, PdfName.F)
                Me.Value = value
            End Sub

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

            '/**
            '  <summary>Gets/Sets the (zero-based) frame within a media object.</summary>
            '*/
            Public Overrides Property Value As Object
                Get
                    Return CType(BaseDataObject(PdfName.F), PdfInteger).IntValue
                End Get
                Set(ByVal value As Object)
                    Dim intValue As Integer = CInt(value)
                    If (intValue < 0) Then Throw New ArgumentException("MUST be non-negative.")
                    BaseDataObject(PdfName.F) = PdfInteger.Get(intValue)
                End Set
            End Property
        End Class

        '/**
        '  <summary>Media offset marker [PDF:1.7:9.1.5].</summary>
        '*/
        Public NotInheritable Class Marker
            Inherits MediaOffset

            Public Sub New(ByVal context As Document, ByVal value As String)
                MyBase.New(context, PdfName.M)
                Me.Value = value
            End Sub

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

            '/**
            '  <summary>Gets a named offset within a media object.</summary>
            '*/
            Public Overrides Property Value As Object
                Get
                    Return CType(BaseDataObject(PdfName.M), PdfTextString).StringValue
                End Get
                Set(ByVal value As Object)
                    BaseDataObject(PdfName.M) = PdfTextString.Get(CStr(value))
                End Set
            End Property
        End Class

        '/**
        '  <summary>Media offset time [PDF:1.7:9.1.5].</summary>
        '*/
        Public NotInheritable Class Time
            Inherits MediaOffset

            Public Sub New(ByVal context As Document, ByVal value As Double)
                MyBase.New(context, PdfName.T)
                BaseDataObject(PdfName.T) = New Timespan(value).BaseObject
            End Sub

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

            '/**
            '  <summary>Gets/Sets the temporal offset (in seconds).</summary>
            '*/
            Public Overrides Property Value As Object
                Get
                    Return Timespan.Time
                End Get
                Set(ByVal value As Object)
                    Me.Timespan.Time = CDbl(value)
                End Set
            End Property

            Private ReadOnly Property Timespan As Timespan
                Get
                    Return New Timespan(BaseDataObject(PdfName.T))
                End Get
            End Property
        End Class

#End Region

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As MediaOffset
            If (baseObject Is Nothing) Then Return Nothing
            Dim dataObject As PdfDictionary = CType(baseObject.Resolve(), PdfDictionary)
            Dim offsetType As PdfName = CType(dataObject(PdfName.S), PdfName)
            If (
                offsetType Is Nothing OrElse
                (dataObject.ContainsKey(PdfName.Type) AndAlso Not dataObject(PdfName.Type).Equals(PdfName.MediaOffset))
                ) Then
                Return Nothing
            End If
            If (offsetType.Equals(PdfName.F)) Then
                Return New Frame(baseObject)
            ElseIf (offsetType.Equals(PdfName.M)) Then
                Return New Marker(baseObject)
            ElseIf (offsetType.Equals(PdfName.T)) Then
                Return New Time(baseObject)
            Else
                Throw New NotSupportedException()
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal subtype As PdfName)
            MyBase.New(context, New PdfDictionary(New PdfName() {PdfName.Type, PdfName.S}, New PdfDirectObject() {PdfName.MediaOffset, subtype}))
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary>Gets/Sets the offset value.</summary>
        '*/
        Public MustOverride Property Value As Object


#End Region
#End Region
#End Region

    End Class

End Namespace
