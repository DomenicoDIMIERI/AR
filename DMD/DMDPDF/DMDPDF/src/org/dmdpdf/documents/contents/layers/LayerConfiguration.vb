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

Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.contents.layers

    '/**
    '  <summary>Optional content configuration [PDF:1.7:4.10.3].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public Class LayerConfiguration
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements ILayerConfiguration

        '    /**
        '  <summary>Base state used to initialize the states of all the layers in a document when this
        '  configuration is applied.</summary>
        '*/
        Friend Enum BaseStateEnum

            '  /**
            '  <summary>All the layers are visible.</summary>
            '*/
            [On]
            '/**
            '  <summary>All the layers are invisible.</summary>
            '*/
            Off
            '/**
            '  <summary>All the layers are left unchanged.</summary>
            '*/
            Unchanged
        End Enum

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Public Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"
#Region "ILayerConfiguration"

        Public Property Creator As String Implements ILayerConfiguration.Creator
            Get
                Return CStr(PdfSimpleObject(Of Object).GetValue(Me.BaseDataObject(PdfName.Creator)))
            End Get
            Set(ByVal value As String)
                Me.BaseDataObject(PdfName.Creator) = PdfTextString.Get(value)
            End Set
        End Property

        Public Property Layers As Layers Implements ILayerConfiguration.Layers
            Get
                Return Layers.Wrap(BaseDataObject.Get(Of PdfArray)(PdfName.Order))
            End Get
            Set(ByVal value As Layers)
                Me.BaseDataObject(PdfName.Order) = value.BaseObject
            End Set
        End Property

        Public Property ListMode As ListModeEnum Implements ILayerConfiguration.ListMode
            Get
                Return ListModeEnumExtension.Get(CType(Me.BaseDataObject(PdfName.ListMode), PdfName))
            End Get
            Set(ByVal value As ListModeEnum)
                Me.BaseDataObject(PdfName.ListMode) = value.GetName()
            End Set
        End Property

        Public ReadOnly Property OptionGroups As Array(Of LayerGroup) Implements ILayerConfiguration.OptionGroups
            Get
                Return Array(Of LayerGroup).Wrap(Of LayerGroup)(BaseDataObject.Get(Of PdfArray)(PdfName.RBGroups))
            End Get
        End Property

        Public Property Title As String Implements ILayerConfiguration.Title
            Get
                Return CStr(PdfSimpleObject(Of Object).GetValue(Me.BaseDataObject(PdfName.Name)))
            End Get
            Set(ByVal value As String)
                Me.BaseDataObject(PdfName.Name) = PdfTextString.Get(value)
            End Set
        End Property

        Public Property Visible As Boolean? Implements ILayerConfiguration.Visible
            Get
                Return BaseStateEnumExtension.Get(CType(BaseDataObject(PdfName.BaseState), PdfName)).IsEnabled()
            End Get
            Set(ByVal value As Boolean?)
                '/*
                '  NOTE: Base state can be altered only in case of alternate configuration; default ones MUST
                '  be set to default state (that is ON).
                '*/
                If (Not (TypeOf (BaseObject.Parent) Is PdfDictionary)) Then ' Not theThen default configuration?
                    BaseDataObject(PdfName.BaseState) = BaseStateEnumExtension.Get(value).GetName()
                End If
            End Set
        End Property

#End Region
#End Region

#Region "internal"

        Friend Function IsVisible(ByVal layer As Layer) As Boolean
            Dim visible As Boolean? = Me.Visible
            If (Not visible.HasValue OrElse visible.Value) Then
                Return Not OffLayersObject.Contains(layer.BaseObject)
            Else
                Return OnLayersObject.Contains(layer.BaseObject)
            End If
        End Function

        '/**
        '  <summary>Sets the usage application for the specified factors.</summary>
        '  <param name="event_">Situation in which this usage application should be used. May be
        '    <see cref="PdfName.View">View</see>, <see cref="PdfName.Print">Print</see> or <see
        '    cref="PdfName.Export">Export</see>.</param>
        '  <param name="category">Layer usage entry to consider when managing the states of the layer.
        '  </param>
        '  <param name="layer">Layer which should have its state automatically managed based on its usage
        '    information.</param>
        '  <param name="retain">Whether this usage application has to be kept or removed.</param>
        '*/
        Friend Sub SetUsageApplication(
                                       ByVal event_ As PdfName,
                                       ByVal category As PdfName,
                                        ByVal layer As Layer,
                                       ByVal retain As Boolean
                                       )
            Dim matched As Boolean = False
            Dim usages As PdfArray = Me.BaseDataObject.Resolve(Of PdfArray)(PdfName.AS)
            For Each usage As PdfDirectObject In usages
                Dim usageDictionary As PdfDictionary = CType(usage, PdfDictionary)
                If (
                        usageDictionary(PdfName.Event).Equals(event_) AndAlso
                        CType(usageDictionary(PdfName.Category), PdfArray).Contains(category)
                    ) Then
                    Dim usageLayers As PdfArray = usageDictionary.Resolve(Of PdfArray)(PdfName.OCGs)
                    If (usageLayers.Contains(layer.BaseObject)) Then
                        If (Not retain) Then
                            usageLayers.Remove(layer.BaseObject)
                        End If
                    Else
                        If (retain) Then
                            usageLayers.Add(layer.BaseObject)
                        End If
                    End If
                    matched = True
                End If
            Next
            If (Not matched AndAlso retain) Then
                Dim usageDictionary As PdfDictionary = New PdfDictionary()
                '{
                usageDictionary(PdfName.Event) = event_
                usageDictionary.Resolve(Of PdfArray)(PdfName.Category).Add(category)
                usageDictionary.Resolve(Of PdfArray)(PdfName.OCGs).Add(layer.BaseObject)
                usages.Add(usageDictionary)
            End If
        End Sub


        Friend Sub SetVisible(ByVal layer As Layer, ByVal value As Boolean)
            Dim layerObject As PdfDirectObject = layer.BaseObject
            Dim offLayersObject As PdfArray = Me.OffLayersObject
            Dim onLayersObject As PdfArray = Me.OnLayersObject
            Dim visible As Boolean? = Me.Visible
            If (Not visible.HasValue) Then
                If (value AndAlso Not onLayersObject.Contains(layerObject)) Then
                    onLayersObject.Add(layerObject)
                    offLayersObject.Remove(layerObject)
                ElseIf (Not value AndAlso Not offLayersObject.Contains(layerObject)) Then
                    offLayersObject.Add(layerObject)
                    onLayersObject.Remove(layerObject)
                End If
            ElseIf (Not visible.Value) Then
                If (value AndAlso Not onLayersObject.Contains(layerObject)) Then
                    onLayersObject.Add(layerObject)
                End If
            Else
                If (Not value AndAlso Not offLayersObject.Contains(layerObject)) Then
                    offLayersObject.Add(layerObject)
                End If
            End If
        End Sub

#End Region

#Region "private"

        '/**
        '  <summary>Gets the collection Of the layer objects whose state Is Set To OFF.</summary>
        '*/
        Private ReadOnly Property OffLayersObject As PdfArray
            Get
                Return BaseDataObject.Resolve(Of PdfArray)(PdfName.OFF)
            End Get
        End Property

        '/**
        '  <summary>Gets the collection Of the layer objects whose state Is Set To On.</summary>
        '*/
        Private ReadOnly Property OnLayersObject As PdfArray
            Get
                Return Me.BaseDataObject.Resolve(Of PdfArray)(PdfName.ON)
            End Get
        End Property

#End Region
#End Region
#End Region
    End Class


    Module BaseStateEnumExtension

        Private ReadOnly codes As BiDictionary(Of LayerConfiguration.BaseStateEnum, PdfName)

        Sub New()
            codes = New BiDictionary(Of LayerConfiguration.BaseStateEnum, PdfName)
            codes(LayerConfiguration.BaseStateEnum.On) = PdfName.ON
            codes(LayerConfiguration.BaseStateEnum.Off) = PdfName.OFF
            codes(LayerConfiguration.BaseStateEnum.Unchanged) = PdfName.Unchanged
        End Sub

        Public Function [Get](ByVal name As PdfName) As LayerConfiguration.BaseStateEnum
            If (name Is Nothing) Then Return LayerConfiguration.BaseStateEnum.On
            Dim baseState As LayerConfiguration.BaseStateEnum? = codes.GetKey(name)
            If (Not baseState.HasValue) Then Throw New NotSupportedException("Base state unknown: " & name.ToString)
            Return baseState.Value
        End Function

        Public Function [Get](ByVal enabled As Boolean?) As LayerConfiguration.BaseStateEnum
            Return IIF(
                       enabled.HasValue,
                       IIF(enabled.Value, LayerConfiguration.BaseStateEnum.On, LayerConfiguration.BaseStateEnum.Off),
                       LayerConfiguration.BaseStateEnum.Unchanged
                       )
        End Function

        <Extension>
        Public Function GetName(ByVal baseState As LayerConfiguration.BaseStateEnum) As PdfName
            Return codes(baseState)
        End Function

        <Extension>
        Public Function IsEnabled(ByVal baseState As LayerConfiguration.BaseStateEnum) As Boolean
            Select Case (baseState)
                Case LayerConfiguration.BaseStateEnum.On : Return True
                Case LayerConfiguration.BaseStateEnum.Off : Return False
                Case LayerConfiguration.BaseStateEnum.Unchanged : Return Nothing
                Case Else : Throw New NotSupportedException()
            End Select
        End Function

    End Module

End Namespace