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

Imports System

Namespace DMD.org.dmdpdf.documents.contents.layers

    '/**
    '  <summary>Optional content properties [PDF:1.7:4.10.3].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public Class LayerDefinition
        Inherits PdfObjectWrapper(Of PdfDictionary)
        Implements ILayerConfiguration

#Region "Static"
#Region "Interface"
#Region "Public"

        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As LayerDefinition
            If (baseObject IsNot Nothing) Then
                Return New LayerDefinition(baseObject)
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
            Me.Initialize()
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
            Me.Initialize()
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the layer configurations used under particular circumstances.</summary>
        '*/
        Public Property AlternateConfigurations As Array(Of LayerConfiguration)
            Get
                Return Array(Of LayerConfiguration).Wrap(Of LayerConfiguration)(Me.baseDataObject.Get(Of PdfArray)(PdfName.Configs))
            End Get
            Set(ByVal value As Array(Of LayerConfiguration))
                Me.baseDataObject(PdfName.Configs) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary> Gets the Default layer configuration, that Is the initial state Of the Optional
        '  content groups When a document Is first opened.</summary>
        '*/
        Public Property DefaultConfiguration As LayerConfiguration
            Get
                Return New LayerConfiguration(Me.baseDataObject(PdfName.D))
            End Get
            Set(ByVal value As LayerConfiguration)
                Me.baseDataObject(PdfName.D) = value.BaseObject
            End Set
        End Property

#Region "ILayerConfiguration"

        Public Property Creator As String Implements ILayerConfiguration.Creator
            Get
                Return DefaultConfiguration.Creator
            End Get
            Set(ByVal value As String)
                DefaultConfiguration.Creator = value
            End Set
        End Property

        Public Property Layers As Layers Implements ILayerConfiguration.Layers
            Get
                Return DefaultConfiguration.Layers
            End Get
            Set(ByVal value As Layers)
                DefaultConfiguration.Layers = value
            End Set
        End Property

        Public Property ListMode As ListModeEnum Implements ILayerConfiguration.ListMode
            Get
                Return DefaultConfiguration.ListMode
            End Get
            Set(ByVal value As ListModeEnum)
                DefaultConfiguration.ListMode = value
            End Set
        End Property

        Public ReadOnly Property OptionGroups As Array(Of LayerGroup) Implements ILayerConfiguration.OptionGroups
            Get
                Return DefaultConfiguration.OptionGroups
            End Get
        End Property

        Public Property Title As String Implements ILayerConfiguration.Title
            Get
                Return DefaultConfiguration.Title
            End Get
            Set(ByVal value As String)
                DefaultConfiguration.Title = value
            End Set
        End Property

        Public Property Visible As Boolean? Implements ILayerConfiguration.Visible
            Get
                Return DefaultConfiguration.Visible
            End Get
            Set(ByVal value As Boolean?)
                DefaultConfiguration.Visible = value
            End Set
        End Property

#End Region
#End Region

#Region "internal"

        '/**
        '  <summary> Gets the collection Of all the layer objects In the document.</summary>
        '*/
        '/*
        ' * TODO: manage layer removal from file (unregistration) -- attach a removal listener
        ' * to the IndirectObjects collection: anytime a PdfDictionary With Type==PdfName.OCG Is removed,
        ' * that listener MUST update this collection.
        ' * Listener MUST be instantiated when LayerDefinition Is associated to the document.
        ' */
        Friend ReadOnly Property AllLayersObject As PdfArray
            Get
                Return CType(Me.baseDataObject.Resolve(PdfName.OCGs), PdfArray)
            End Get
        End Property

#End Region

#Region "Private"

        Private Sub Initialize()
            Dim baseDataObject As PdfDictionary = Me.BaseDataObject
            If (baseDataObject.Count = 0) Then
                baseDataObject.Updateable = False
                baseDataObject(PdfName.OCGs) = New PdfArray()
                baseDataObject(PdfName.D) = New LayerConfiguration(Document).BaseObject
                'TODO: as this Is optional, verify whether it can be lazily instantiated later.
                DefaultConfiguration.Layers = New Layers(Document)
                baseDataObject.Updateable = True
            End If
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace

