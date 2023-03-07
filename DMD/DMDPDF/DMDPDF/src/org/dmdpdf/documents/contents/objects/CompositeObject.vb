'/*
'  Copyright 2007-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.bytes

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Composite object. It is made up of multiple content objects.</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public MustInherit Class CompositeObject
        Inherits ContentObject


#Region "dynamic"
#Region "fields"

        Protected _objects As IList(Of ContentObject)

#End Region

#Region "constructors"

        Protected Sub New()
            Me._objects = New List(Of ContentObject)()
        End Sub


        Protected Sub New(ByVal obj As ContentObject)
            Me.New
            Me._objects.Add(obj)
        End Sub

        Protected Sub New(ParamArray objects As ContentObject())
            Me.New
            For Each obj As ContentObject In objects
                Me._objects.Add(obj)
            Next
        End Sub

        Protected Sub New(ByVal objects As IList(Of ContentObject))
            Me._objects = objects
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the Object header.</summary>
        '*/
        Public Overridable Property Header As Operation
            Get
                Return Nothing
            End Get
            Set(ByVal value As Operation)
                Throw New NotSupportedException()
            End Set
        End Property

        '/**
        '  <summary> Gets the list Of inner objects.</summary>
        '*/
        Public ReadOnly Property Objects As IList(Of ContentObject)
            Get
                Return _objects
            End Get
        End Property

        Public Overrides Sub Scan(ByVal state As ContentScanner.GraphicsState)
            Dim childLevel As ContentScanner = state.Scanner.ChildLevel
            If (Not Render(state)) Then
                childLevel.MoveEnd() ' Forces the current object to its final graphics state.
            End If
            childLevel.State.CopyTo(state) ' Copies the current Object's final graphics state to the current level's.
        End Sub

        Public Overrides Function ToString() As String
            Return "{" & _objects.ToString() & "}"
        End Function

        Public Overrides Sub WriteTo(ByVal Stream As IOutputStream, ByVal context As Document)
            For Each obj As ContentObject In _objects
                obj.WriteTo(Stream, context)
            Next
        End Sub

#End Region

#Region "protected"

        '/**
        '  <summary> Creates the rendering Object corresponding To Me container.</summary>
        '*/
        Protected Overridable Function CreateRenderObject() As GraphicsPath
            Return Nothing
        End Function

        '/**
        '  <summary> Renders Me container.</summary>
        '  <param name = "state" > Graphics state.</param>
        '  <returns> Whether the rendering has been executed.</returns>
        ' */
        Protected Function Render(ByVal state As ContentScanner.GraphicsState) As Boolean
            Dim scanner As ContentScanner = state.Scanner
            Dim context As Graphics = scanner.RenderContext
            If (context Is Nothing) Then Return False
            ' Render the inner elements!
            scanner.ChildLevel.Render(
                                    context,
                                    scanner.CanvasSize,
                                    CreateRenderObject()
                                    )
            Return True
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace
