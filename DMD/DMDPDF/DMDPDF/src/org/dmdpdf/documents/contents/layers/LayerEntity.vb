'/*
'  Copyright 2011-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.contents.layers

    '/**
    '  <summary>Layer entity.</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public MustInherit Class LayerEntity
        Inherits PropertyList

#Region "types"
        '/**
        '  <summary>Membership visibility policy [PDF:1.7:4.10.1].</summary>
        '*/
        Public Enum VisibilityPolicyEnum
            '/**
            '  <summary>Visible only if all of the visibility layers are ON.</summary>
            '*/
            AllOn
            '/**
            '  <summary>Visible if any of the visibility layers are ON.</summary>
            '*/
            AnyOn
            '/**
            '  <summary>Visible if any of the visibility layers are OFF.</summary>
            '*/
            AnyOff
            '/**
            '  <summary>Visible only if all of the visibility layers are OFF.</summary>
            '*/
            AllOff
        End Enum

#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal typeName As PdfName)
            MyBase.New(
                    context,
                    New PdfDictionary(
                                New PdfName() {PdfName.Type},
                                New PdfDirectObject() {typeName}
                                ))
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the Default membership.</summary>
        '  <remarks> This collection corresponds To the hierarchical relation between this layer entity
        '  And its ascendants.</remarks>
        '*/
        Public Overridable ReadOnly Property Membership As LayerMembership
            Get
                Return Nothing
            End Get
        End Property

        '/**
        '  <summary> Gets the layers whose states determine the visibility Of content controlled by this
        '  entity.</summary>
        '*/
        Public Overridable ReadOnly Property VisibilityLayers As IList(Of Layer)
            Get
                Return Nothing
            End Get
        End Property

        '/**
        '  <summary> Gets/Sets the visibility policy Of this entity.</summary>
        '*/
        Public Overridable Property VisibilityPolicy As VisibilityPolicyEnum
            Get
                Return VisibilityPolicyEnum.AllOn
            End Get
            Set(ByVal value As VisibilityPolicyEnum)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

    Module VisibilityPolicyEnumExtension

        Private ReadOnly codes As BiDictionary(Of LayerMembership.VisibilityPolicyEnum, PdfName)

        Sub New()
            codes = New BiDictionary(Of LayerMembership.VisibilityPolicyEnum, PdfName)
            codes(LayerMembership.VisibilityPolicyEnum.AllOn) = PdfName.AllOn
            codes(LayerMembership.VisibilityPolicyEnum.AnyOn) = PdfName.AnyOn
            codes(LayerMembership.VisibilityPolicyEnum.AnyOff) = PdfName.AnyOff
            codes(LayerMembership.VisibilityPolicyEnum.AllOff) = PdfName.AllOff
        End Sub

        Public Function [Get](ByVal name As PdfName) As LayerMembership.VisibilityPolicyEnum
            If (name Is Nothing) Then Return LayerMembership.VisibilityPolicyEnum.AnyOn
            Dim visibilityPolicy As LayerMembership.VisibilityPolicyEnum? = codes.GetKey(name)
            If (Not visibilityPolicy.HasValue) Then Throw New NotSupportedException("Visibility policy unknown: " & name.ToString)
            Return visibilityPolicy.Value
        End Function

        <Extension>
        Public Function GetName(ByVal visibilityPolicy As LayerMembership.VisibilityPolicyEnum) As PdfName
            Return codes(visibilityPolicy)
        End Function
    End Module

End Namespace