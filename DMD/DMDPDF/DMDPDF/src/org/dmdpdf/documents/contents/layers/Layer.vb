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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util
Imports DMD.org.dmdpdf.util.math

Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.contents.layers

    '/**
    '  <summary>Optional content group [PDF:1.7:4.10.1].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class Layer
        Inherits LayerEntity
        Implements ILayerNode

#Region "types"

        '/**
        '  <summary>Sublayers location within a configuration structure.</summary>
        '*/
        Private Class LayersLocation

            '      /**
            '  <summary>Sublayers ordinal position within the parent sublayers.</summary>
            '*/
            Public Index As Integer

            '/**
            '  <summary>Parent layer object.</summary>
            '*/
            Public ParentLayerObject As PdfDirectObject

            '      /**
            '  <summary>Parent sublayers object.</summary>
            '*/
            Public ParentLayersObject As PdfArray

            '/**
            '  <summary>Upper levels.</summary>
            '*/
            Public Levels As Stack(Of Object())

            Public Sub New(
                            ByVal parentLayerObject As PdfDirectObject,
                            ByVal parentLayersObject As PdfArray,
                            ByVal index As Integer,
                            ByVal levels As Stack(Of Object())
                            )
                Me.ParentLayerObject = parentLayerObject
                Me.ParentLayersObject = parentLayersObject
                Me.Index = index
                Me.Levels = levels
            End Sub
        End Class

        '/**
        '  <summary> Layer state.</summary>
        '*/
        Friend Enum StateEnum
            '/**
            '  <summary> Active.</summary>
            '*/
            [On]
            '/**
            '  <summary> Inactive.</summary>
            '*/
            Off
        End Enum

#End Region

#Region "Static"
#Region "fields"

        Public Shared ReadOnly TypeName As PdfName = PdfName.OCG

        Private Shared ReadOnly MembershipName As PdfName = New PdfName("D-OCMD")

#End Region

#Region "Interface"
#Region "Public"

        Public Shared Shadows Function Wrap(ByVal baseObject As PdfDirectObject) As Layer
            If (baseObject IsNot Nothing) Then
                Return New Layer(baseObject)
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document, ByVal title As String)
            MyBase.New(context, PdfName.OCG)
            Me.Title = title
            '  // Add Me layer to the global collection!
            '/*
            '  NOTE: Every layer MUST be included In the Global collection [PDF:1.7:4.10.3].
            '*/
            Dim definition As LayerDefinition = context.Layer
            If (definition Is Nothing) Then
                definition = New LayerDefinition(context)
                context.Layer = definition
            End If
            definition.AllLayersObject.Add(BaseObject)
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the name Of the type Of content controlled by the group.</summary>
        '*/
        Public Property ContentType As String
            Get
                Return CStr(PdfSimpleObject(Of Object).GetValue(GetUsageEntry(PdfName.CreatorInfo)(PdfName.Subtype)))
            End Get
            Set(ByVal value As String)
                GetUsageEntry(PdfName.CreatorInfo)(PdfName.Subtype) = PdfName.Get(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the name Of the application that created Me layer.</summary>
        '*/
        Public Property Creator As String
            Get
                'Return CStr(PdfSimpleObject(Of Object).GetValue(GetUsageEntry(PdfName.CreatorInfo)(PdfName.Creator))
                Return CStr(PdfSimpleObject(Of Object).GetValue(GetUsageEntry(PdfName.CreatorInfo)(PdfName.Creator)))
            End Get
            Set(ByVal value As String)
                GetUsageEntry(PdfName.CreatorInfo)(PdfName.Creator) = PdfTextString.Get(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets whether Me layer Is visible When the document Is saved by a viewer
        '  application to a format that does Not support optional content.</summary>
        '*/
        Public Property Exportable As Boolean
            Get
                Return StateEnumExtension.Get(CType(GetUsageEntry(PdfName.Export)(PdfName.ExportState), PdfName)).IsEnabled()
            End Get
            Set(ByVal value As Boolean)
                GetUsageEntry(PdfName.Export)(PdfName.ExportState) = StateEnumExtension.Get(value).GetName()
                DefaultConfiguration.SetUsageApplication(PdfName.Export, PdfName.Export, Me, True)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the language (And possibly locale) Of the content controlled by Me layer.
        '  </summary>
        '*/
        Public Property Language As String
            Get
                Return CStr(PdfSimpleObject(Of Object).GetValue(GetUsageEntry(PdfName.Language)(PdfName.Lang)))
            End Get
            Set(ByVal value As String)
                GetUsageEntry(PdfName.Language)(PdfName.Lang) = PdfTextString.Get(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the sublayers.</summary>
        '*/
        Public Property Layers As Layers Implements ILayerNode.Layers
            Get
                Dim location As LayersLocation = FindLayersLocation()
                Return Layers.Wrap(location.ParentLayersObject.Get(Of PdfArray)(location.Index))
            End Get
            Set(ByVal value As Layers)
                Dim location As LayersLocation = FindLayersLocation()
                If (location.Index = location.ParentLayersObject.Count) Then
                    location.ParentLayersObject.Add(value.BaseObject) ' Appends New sublayers.
                ElseIf (TypeOf (location.ParentLayersObject.Resolve(location.Index)) Is PdfArray) Then
                    location.ParentLayersObject(location.Index) = value.BaseObject ' // Substitutes old sublayers.
                Else
                    location.ParentLayersObject.Insert(location.Index, value.BaseObject) ' Inserts New sublayers.
                End If
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets whether the Default visibility Of Me layer cannot be changed through the
        '  user Interface of a viewer application.</summary>
        '*/
        Public Property Locked As Boolean
            Get
                Return DefaultConfiguration.BaseDataObject.Resolve(Of PdfArray)(PdfName.Locked).Contains(BaseObject)
            End Get
            Set(ByVal value As Boolean)
                Dim lockedArrayObject As PdfArray = DefaultConfiguration.BaseDataObject.Resolve(Of PdfArray)(PdfName.Locked)
                If (Not lockedArrayObject.Contains(BaseObject)) Then
                    lockedArrayObject.Add(BaseObject)
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property Membership As LayerMembership
            Get
                Dim _membership As LayerMembership
                Dim membershipObject As PdfDirectObject = Me.BaseDataObject(MembershipName)
                If (membershipObject Is Nothing) Then
                    _membership = New LayerMembership(Document)
                    _membership.VisibilityPolicy = VisibilityPolicyEnum.AllOn ' NOTE: Forces visibility To depend On all the ascendant layers.
                    membershipObject = _membership.BaseObject
                    BaseDataObject(MembershipName) = membershipObject
                Else
                    _membership = LayerMembership.Wrap(membershipObject)
                End If
                If (_membership.VisibilityLayers.Count = 0) Then
                    _membership.VisibilityLayers.Add(Me)
                    Dim location As LayersLocation = FindLayersLocation()
                    If (location.ParentLayerObject IsNot Nothing) Then
                        _membership.VisibilityLayers.Add(New Layer(location.ParentLayerObject))
                    End If
                    For Each level As Object() In location.Levels
                        Dim layerObject As PdfDirectObject = CType(level(2), PdfDirectObject)
                        If (layerObject IsNot Nothing) Then
                            _membership.VisibilityLayers.Add(New Layer(layerObject))
                        End If
                    Next
                End If
                Return _membership
            End Get
        End Property

        '/**
        '  <summary> Gets/Sets whether Me layer Is visible When the document Is printed from a viewer
        '  application.</summary>
        '*/
        Public Property Printable As Boolean
            Get
                Return StateEnumExtension.Get(CType(GetUsageEntry(PdfName.Print)(PdfName.PrintState), PdfName)).IsEnabled()
            End Get
            Set(ByVal value As Boolean)
                GetUsageEntry(PdfName.Print)(PdfName.PrintState) = StateEnumExtension.Get(value).GetName()
                DefaultConfiguration.SetUsageApplication(PdfName.Print, PdfName.Print, Me, True)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.Title
        End Function

        '/**
        '  <summary> Gets/Sets whether Me layer Is visible When the document Is opened In a viewer
        '  application.</summary>
        '*/
        Public Property Viewable As Boolean
            Get
                Return StateEnumExtension.Get(CType(GetUsageEntry(PdfName.View)(PdfName.ViewState), PdfName)).IsEnabled()
            End Get
            Set(ByVal value As Boolean)
                GetUsageEntry(PdfName.View)(PdfName.ViewState) = StateEnumExtension.Get(value).GetName()
                DefaultConfiguration.SetUsageApplication(PdfName.View, PdfName.View, Me, True)
            End Set
        End Property

        Public Overrides ReadOnly Property VisibilityLayers As IList(Of Layer)
            Get
                Return Me.Membership.VisibilityLayers
            End Get
        End Property

        Public Overrides Property VisibilityPolicy As VisibilityPolicyEnum
            Get
                Return Me.Membership.VisibilityPolicy
            End Get
            Set(ByVal value As VisibilityPolicyEnum)
                If (Not value.Equals(Me.Membership.VisibilityPolicy)) Then Throw New NotSupportedException("Single layers cannot manage custom state policies; use LayerMembership instead.")
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets whether Me layer Is initially visible.</summary>
        '*/
        Public Property Visible As Boolean
            Get
                Return DefaultConfiguration.IsVisible(Me)
            End Get
            Set(ByVal value As Boolean)
                DefaultConfiguration.SetVisible(Me, value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the range Of magnifications at which the content In Me layer Is best
        '  viewed.</summary>
        '*/
        Public Property ZoomRange As Interval(Of Double)
            Get
                Dim zoomDictionary As PdfDictionary = GetUsageEntry(PdfName.Zoom)
                Dim minObject As IPdfNumber = CType(zoomDictionary.Resolve(PdfName.min), IPdfNumber)
                Dim maxObject As IPdfNumber = CType(zoomDictionary.Resolve(PdfName.max), IPdfNumber)
                Dim i1 As Double = 0 : If (minObject IsNot Nothing) Then i1 = minObject.RawValue
                Dim i2 As Double = Double.PositiveInfinity : If (maxObject IsNot Nothing) Then i1 = maxObject.RawValue

                Return New Interval(Of Double)(i1, i2)
            End Get
            Set(ByVal value As Interval(Of Double))
                If (value IsNot Nothing) Then
                    Dim zoomDictionary As PdfDictionary = GetUsageEntry(PdfName.Zoom)
                    If (value.Low <> 0) Then
                        zoomDictionary(PdfName.min) = PdfReal.Get(value.Low)
                    Else
                        zoomDictionary(PdfName.min) = Nothing
                    End If
                    If (value.High <> Double.PositiveInfinity) Then
                        zoomDictionary(PdfName.max) = PdfReal.Get(value.High)
                    Else
                        zoomDictionary(PdfName.max) = Nothing
                    End If
                Else
                    Usage.Remove(PdfName.Zoom)
                End If
                DefaultConfiguration.SetUsageApplication(PdfName.View, PdfName.Zoom, Me, value IsNot Nothing)
            End Set
        End Property

#Region "ILayerNode"

        Public Property Title As String Implements ILayerNode.Title
            Get
                Return CStr(CType(BaseDataObject(PdfName.Name), PdfTextString).Value)
            End Get
            Set(ByVal value As String)
                BaseDataObject(PdfName.Name) = New PdfTextString(value)
            End Set
        End Property

#End Region
#End Region

#Region "Private"

        Private ReadOnly Property DefaultConfiguration As LayerConfiguration
            Get
                Return Document.Layer.DefaultConfiguration
            End Get
        End Property

        '/**
        '  <summary> Finds the location Of the sublayers Object In the Default configuration; In Case no
        '  sublayers object Is associated to Me object, its virtual position Is indicated.</summary>
        '*/
        Private Function FindLayersLocation() As LayersLocation
            Dim location As LayersLocation = FindLayersLocation(DefaultConfiguration)
            If (location Is Nothing) Then
                '/*
                '  NOTE: In case the layer Is outside the default structure, it's appended to the root
                '  collection.
                '*/
                '/*
                '  TODO: anytime a layer Is inserted into a collection, the layer tree must be checked To
                '  avoid duplicate :    in case the layer Is already in the tree, it must be moved to the New
                '  position along With its sublayers.
                '*/
                Dim rootLayers As Layers = DefaultConfiguration.Layers
                rootLayers.Add(Me)
                location = New LayersLocation(Nothing, rootLayers.BaseDataObject, rootLayers.Count, New Stack(Of Object()))
            End If
            Return location
        End Function

        '/**
        '  <summary> Finds the location Of the sublayers Object In the specified configuration; In Case no
        '  sublayers object Is associated to Me object, its virtual position Is indicated.</summary>
        '  <param name = "configuration" > configuration context.</param>
        '  <returns> <code> Nothing</code>, If Me layer Is outside the specified configuration.</returns>
        '*/
        Private Function FindLayersLocation(ByVal configuration As LayerConfiguration) As LayersLocation
            '/*
            '  NOTE: As layers are only weakly tied to configurations, their sublayers have to be sought
            '  through the configuration Structure tree.
            '*/
            Dim levelLayerObject As PdfDirectObject = Nothing
            Dim levelObject As PdfArray = configuration.Layers.BaseDataObject
            Dim levelIterator As IEnumerator(Of PdfDirectObject) = levelObject.GetEnumerator()
            Dim levelIterators As Stack(Of Object()) = New Stack(Of Object())()
            Dim thisObject As PdfDirectObject = BaseObject
            Dim currentLayerObject As PdfDirectObject = Nothing
            While (True)
                If (Not levelIterator.MoveNext()) Then
                    If (levelIterators.Count = 0) Then Exit While
                    Dim levelItems As Object() = levelIterators.Pop()
                    levelObject = CType(levelItems(0), PdfArray)
                    levelIterator = CType(levelItems(1), IEnumerator(Of PdfDirectObject))
                    levelLayerObject = CType(levelItems(2), PdfDirectObject)
                    currentLayerObject = Nothing
                Else
                    Dim nodeObject As PdfDirectObject = levelIterator.Current
                    Dim nodeDataObject As PdfDataObject = PdfObject.Resolve(nodeObject)
                    If (TypeOf (nodeDataObject) Is PdfDictionary) Then
                        '                                /*
                        '  NOTE:           Sublayers are expressed As an array immediately following the parent layer node.
                        '*/
                        If (nodeObject.Equals(thisObject)) Then Return New LayersLocation(levelLayerObject, levelObject, levelObject.IndexOf(thisObject) + 1, levelIterators)
                        currentLayerObject = nodeObject
                    ElseIf (TypeOf (nodeDataObject) Is PdfArray) Then
                        levelIterators.Push(New Object() {levelObject, levelIterator, levelLayerObject})
                        levelObject = CType(nodeDataObject, PdfArray)
                        levelIterator = levelObject.GetEnumerator()
                        levelLayerObject = currentLayerObject
                        currentLayerObject = Nothing
                    End If
                End If
            End While
            Return Nothing
        End Function

        Private Function GetUsageEntry(ByVal key As PdfName) As PdfDictionary
            Return Usage.Resolve(Of PdfDictionary)(key)
        End Function

        Private ReadOnly Property Usage As PdfDictionary
            Get
                Return BaseDataObject.Resolve(Of PdfDictionary)(PdfName.Usage)
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

    Module StateEnumExtension

        Private ReadOnly codes As BiDictionary(Of Layer.StateEnum, PdfName)

        Sub New()
            codes = New BiDictionary(Of Layer.StateEnum, PdfName)
            codes(Layer.StateEnum.On) = PdfName.ON
            codes(Layer.StateEnum.Off) = PdfName.OFF
        End Sub

        Public Function [Get](ByVal name As PdfName) As Layer.StateEnum
            If (name Is Nothing) Then Return Layer.StateEnum.On
            Dim state As Layer.StateEnum? = codes.GetKey(name)
            If (Not state.HasValue) Then Throw New NotSupportedException("State unknown: " & name.ToString)
            Return state.Value
        End Function

        Public Function [Get](ByVal enabled As Boolean?) As Layer.StateEnum
            Return IIF(Not enabled.HasValue OrElse enabled.Value, Layer.StateEnum.On, Layer.StateEnum.Off)
        End Function

        <Extension>
        Public Function GetName(ByVal state As Layer.StateEnum) As PdfName
            Return codes(state)
        End Function

        <Extension>
        Public Function IsEnabled(ByVal state As Layer.StateEnum) As Boolean
            Return state = Layer.StateEnum.On
        End Function

    End Module

End Namespace