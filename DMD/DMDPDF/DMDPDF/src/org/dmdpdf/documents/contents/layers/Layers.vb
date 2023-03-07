'/*
'  Copyright 2011-2013 Stefano Chizzolini. http://www.dmdpdf.org

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
    '  <summary>Optional content group collection.</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class Layers
        Inherits Array(Of ILayerNode)
        Implements ILayerNode

#Region "types"

        Private Delegate Function EvaluateNode(ByVal currentNodeIndex As Integer, ByVal currentBaseIndex As Integer) As Integer

        Private Class ItemWrapper
            Implements IWrapper(Of ILayerNode)

            Public Function Wrap(ByVal baseObject As PdfDirectObject) As ILayerNode Implements IWrapper(Of ILayerNode).Wrap
                Return LayerNode.Wrap(baseObject)
            End Function

        End Class

#End Region

#Region "Static"
#Region "fields"

        Private Shared ReadOnly Wrapper As ItemWrapper = New ItemWrapper()

#End Region

#Region "Interface"
#Region "Public"

        Public Shared Shadows Function Wrap(ByVal baseObject As PdfDirectObject) As Layers
            If (baseObject IsNot Nothing) Then
                Return New Layers(baseObject)
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
            Me.New(context, Nothing)
        End Sub

        Public Sub New(ByVal context As Document, ByVal title As String)
            MyBase.New(context, Wrapper)
            Me.Title = title
        End Sub

        Private Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(Wrapper, baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Private Function countEvaluate(ByVal currentNodeIndex As Integer, ByVal currentBaseIndex As Integer) As Integer
            If (currentBaseIndex = -1) Then
                Return currentNodeIndex
            Else
                Return -1
            End If
        End Function

        Public Overrides ReadOnly Property Count As Integer
            Get
                Return Evaluate(AddressOf countEvaluate) + 1
            End Get
        End Property

        Public Overrides Function IndexOf(ByVal Item As ILayerNode) As Integer
            Return GetNodeIndex(MyBase.IndexOf(Item))
        End Function

        Public Overrides Sub Insert(ByVal index As Integer, ByVal item As ILayerNode)
            MyBase.Insert(GetBaseIndex(index), item)
        End Sub

        Public Overrides Sub RemoveAt(ByVal Index As Integer)
            Dim baseIndex As Integer = GetBaseIndex(Index)
            Dim removedItem As ILayerNode = MyBase.Item(baseIndex)
            MyBase.RemoveAt(baseIndex)
            If (TypeOf (removedItem) Is Layer AndAlso
                baseIndex < MyBase.Count) Then
                '/*
                '  NOTE:     Sublayers MUST be removed As well.
                '*/
                If (TypeOf (BaseDataObject.Resolve(baseIndex)) Is PdfArray) Then
                    BaseDataObject.RemoveAt(baseIndex)
                End If
            End If
        End Sub

        Default Public Overrides Property Item(ByVal Index As Integer) As ILayerNode
            Get
                Return MyBase.Item(GetBaseIndex(Index))
            End Get
            Set(ByVal value As ILayerNode)
                MyBase.Item(GetBaseIndex(Index)) = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.Title
        End Function

#Region "ILayerNode"

        ReadOnly Property Layers As Layers Implements ILayerNode.Layers
            Get
                Return Me
            End Get
        End Property

        Public Property Title As String Implements ILayerNode.Title
            Get
                If (BaseDataObject.Count = 0) Then Return Nothing
                Dim firstObject As PdfDirectObject = BaseDataObject(0)
                If (TypeOf (firstObject) Is PdfTextString) Then
                    Return CStr(CType(firstObject, PdfTextString).Value)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                Dim titleObject As PdfTextString = PdfTextString.Get(value)
                Dim BaseDataObject As PdfArray = Me.BaseDataObject
                Dim firstObject As PdfDirectObject
                If (BaseDataObject.Count = 0) Then
                    firstObject = Nothing
                Else
                    firstObject = BaseDataObject(0)
                End If
                If (TypeOf (firstObject) Is PdfTextString) Then
                    If (titleObject IsNot Nothing) Then
                        BaseDataObject(0) = titleObject
                    Else
                        BaseDataObject.RemoveAt(0)
                    End If
                ElseIf (titleObject IsNot Nothing) Then
                    BaseDataObject.Insert(0, titleObject)
                End If
            End Set
        End Property

#End Region
#End Region

#Region "private"

        '    /**
        '  <summary> Gets the positional information resulting from the collection evaluation.</summary>
        '  <param name = "evaluator" > Expression used To evaluate the positional matching.</param>
        '*/
        Private Function Evaluate(ByVal evaluateNode As EvaluateNode) As Integer
            '/*
            '  NOTE: Layer hierarchies are represented through a somewhat flatten Structure which needs
            '  to be evaluated in order to match nodes in their actual place.
            '*/
            Dim baseDataObject As PdfArray = Me.BaseDataObject
            Dim nodeIndex As Integer = -1
            Dim groupAllowed As Boolean = True
            Dim baseLength As Integer = MyBase.Count
            For baseIndex As Integer = 0 To baseLength - 1
                Dim itemDataObject As PdfDataObject = baseDataObject.Resolve(baseIndex)
                If (TypeOf (itemDataObject) Is PdfDictionary OrElse
                     (TypeOf (itemDataObject) Is PdfArray AndAlso groupAllowed)) Then
                    nodeIndex += 1
                    Dim evaluation As Integer = evaluateNode(nodeIndex, baseIndex)
                    If (evaluation > -1) Then Return evaluation
                End If
                groupAllowed = Not (TypeOf (itemDataObject) Is PdfDictionary)
            Next
            Return evaluateNode(nodeIndex, -1)
        End Function

        Private Class baseIndexEvaluator
            Public o As Layers
            Public nodeIndex As Integer

            Public Sub New(ByVal o As Layers, ByVal nodeIndex As Integer)
                Me.o = o
                Me.nodeIndex = nodeIndex
            End Sub

            Public Function Evaluate(ByVal currentNodeIndex As Integer, ByVal currentBaseIndex As Integer) As Integer
                If (currentNodeIndex = nodeIndex) Then
                    Return currentBaseIndex
                Else
                    Return -1
                End If
            End Function


        End Class



        Private Function GetBaseIndex(ByVal nodeIndex As Integer) As Integer
            Dim o As New baseIndexEvaluator(Me, nodeIndex)
            Return Evaluate(AddressOf o.Evaluate)
        End Function

        Private Class getNodeIndexEvaluator
            Public o As Layers
            Public baseIndex As Integer

            Public Sub New(ByVal o As Layers, ByVal baseIndex As Integer)
                Me.o = o
                Me.baseIndex = baseIndex
            End Sub

            Public Function Evaluate(ByVal currentNodeIndex As Integer, ByVal currentBaseIndex As Integer) As Integer
                If (currentBaseIndex = baseIndex) Then
                    Return currentNodeIndex
                Else
                    Return -1
                End If
            End Function


        End Class

        Private Function GetNodeIndex(ByVal baseIndex As Integer) As Integer
            Dim o As New getNodeIndexEvaluator(Me, baseIndex)
            Return Evaluate(AddressOf o.Evaluate)

        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace