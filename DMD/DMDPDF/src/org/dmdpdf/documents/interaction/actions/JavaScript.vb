'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>'Cause a script to be compiled and executed by the JavaScript interpreter'
    '  action [PDF:1.6:8.6.4].</summary>
    '*/
    <PDF(VersionEnum.PDF13)>
    Public NotInheritable Class JavaScript
        Inherits Action

#Region "static"
#Region "interface"
#Region "internal"

        '/**
        '  <summary>Gets the Javascript script from the specified base data object.</summary>
        '*/
        Friend Shared Function GetScript(ByVal baseDataObject As PdfDictionary, ByVal key As PdfName) As String
            Dim scriptObject As PdfDataObject = baseDataObject.Resolve(key)
            If (scriptObject Is Nothing) Then
                Return Nothing
            ElseIf (TypeOf (scriptObject) Is PdfTextString) Then
                Return CType(scriptObject, PdfTextString).StringValue
            Else
                Dim scriptBuffer As bytes.IBuffer = CType(scriptObject, PdfStream).Body
                Return scriptBuffer.GetString(0, CInt(scriptBuffer.Length))
            End If
        End Function

        '/**
        '  <summary>Sets the Javascript script into the specified base data object.</summary>
        '*/
        Friend Shared Sub SetScript(ByVal baseDataObject As PdfDictionary, ByVal key As PdfName, ByVal value As String)
            Dim scriptObject As PdfDataObject = baseDataObject.Resolve(key)
            If (Not (TypeOf (scriptObject) Is PdfStream) AndAlso value.Length > 256) Then
                scriptObject = New PdfStream()
                baseDataObject(key) = baseDataObject.File.Register(scriptObject)
            End If
            ' Insert the script!
            If (TypeOf (scriptObject) Is PdfStream) Then
                Dim scriptBuffer As bytes.IBuffer = CType(scriptObject, PdfStream).Body
                scriptBuffer.SetLength(0)
                scriptBuffer.Append(value)
            Else
                baseDataObject(key) = New PdfTextString(value)
            End If
        End Sub

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new action within the given document context.</summary>
        '*/
        Public Sub New(ByVal context As Document, ByVal script As String)
            MyBase.New(context, PdfName.JavaScript)
            Me.Script = script
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the JavaScript script to be executed.</summary>
        '*/
        Public Property Script As String
            Get
                Return GetScript(BaseDataObject, PdfName.JS)
            End Get
            Set(ByVal value As String)
                SetScript(BaseDataObject, PdfName.JS, value)
            End Set
        End Property

#Region "IPdfNamedObjectWrapper"

        Public ReadOnly Property Name As PdfString
            Get
                Return RetrieveName()
            End Get
        End Property

        Public ReadOnly Property NamedBaseObject As PdfDirectObject
            Get
                Return RetrieveNamedBaseObject()
            End Get
        End Property

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace