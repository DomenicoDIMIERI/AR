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
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.documents.multimedia
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>'Control the playing of multimedia content' action [PDF:1.6:8.5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class Render
        Inherits Action

#Region "types"
        Public Enum OperationEnum

            '/**
            '  <summary>Play the rendition on the screen, stopping any previous one.</summary>
            '*/
            Play
            '/**
            '  <summary>Stop any rendition being played on the screen.</summary>
            '*/
            [Stop]
            '/**
            '  <summary>Pause any rendition being played on the screen.</summary>
            '*/
            Pause
            '/**
            '  <summary>Resume any rendition being played on the screen.</summary>
            '*/
            [Resume]
            '/**
            '  <summary>Play the rendition on the screen, resuming any previous one.</summary>
            '*/
            PlayResume
        End Enum

#End Region

#Region "dynamic"
#Region "constructors"
        '/**
        '  <summary>Creates a new action within the given document context.</summary>
        '*/
        Public Sub New(ByVal screen As Screen, ByVal operation As OperationEnum, ByVal rendition As Rendition)
            MyBase.New(screen.Document, PdfName.Rendition)
            Me.Operation = operation
            Me.Screen = screen
            Me.Rendition = rendition
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets/Sets the operation to perform when the action is triggered.</summary>
        '*/
        Public Property Operation As OperationEnum?
            Get
                Return OperationEnumExtension.Get(CType(Me.BaseDataObject(PdfName.OP), PdfInteger))
            End Get
            Set(ByVal value As OperationEnum?)
                Dim BaseDataObject As PdfDictionary = Me.BaseDataObject
                If (value Is Nothing AndAlso BaseDataObject(PdfName.JS) Is Nothing) Then
                    Throw New ArgumentException("Operation MUST be defined.")
                End If
                If (value.HasValue) Then
                    BaseDataObject(PdfName.OP) = value.Value.GetCode()
                Else
                    BaseDataObject(PdfName.OP) = Nothing
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the rendition Object To render.</summary>
        '*/
        Public Property Rendition As Rendition
            Get
                Return Rendition.Wrap(Me.BaseDataObject(PdfName.R))
            End Get
            Set(ByVal value As Rendition)
                If (value Is Nothing) Then
                    Dim operation As OperationEnum? = Me.Operation
                    If (operation.HasValue) Then
                        Select Case (operation.Value)
                            Case OperationEnum.Play, OperationEnum.PlayResume
                                Throw New ArgumentException("Rendition MUST be defined.")
                        End Select
                    End If
                End If
                Me.BaseDataObject(PdfName.R) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the screen where To render the rendition Object.</summary>
        '*/
        Public Property Screen As Screen
            Get
                Return CType(Annotation.Wrap(BaseDataObject(PdfName.AN)), Screen)
            End Get
            Set(ByVal value As Screen)
                If (value Is Nothing) Then
                    Dim operation As OperationEnum? = Me.Operation
                    If (operation.HasValue) Then
                        Select Case (operation.Value)
                            Case OperationEnum.Play,
                                 OperationEnum.PlayResume,
                                 OperationEnum.Pause,
                                 OperationEnum.Resume,
                                 OperationEnum.Stop
                                Throw New ArgumentException("Screen MUST be defined.")
                        End Select
                    End If
                End If
                Me.BaseDataObject(PdfName.AN) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the JavaScript script To be executed When the action Is triggered.</summary>
        '*/
        Public Property Script As String
            Get
                Return JavaScript.GetScript(BaseDataObject, PdfName.JS)
            End Get
            Set(ByVal value As String)
                Dim baseDataObject As PdfDictionary = Me.BaseDataObject
                If (value Is Nothing AndAlso baseDataObject(PdfName.OP) Is Nothing) Then
                    Throw New ArgumentException("Script MUST be defined.")
                End If
                JavaScript.SetScript(baseDataObject, PdfName.JS, value)
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

    Module OperationEnumExtension

        Private ReadOnly codes As BiDictionary(Of Render.OperationEnum, PdfInteger)

        Sub New()
            codes = New BiDictionary(Of Render.OperationEnum, PdfInteger)
            codes(Render.OperationEnum.Play) = New PdfInteger(0)
            codes(Render.OperationEnum.Stop) = New PdfInteger(1)
            codes(Render.OperationEnum.Pause) = New PdfInteger(2)
            codes(Render.OperationEnum.Resume) = New PdfInteger(3)
            codes(Render.OperationEnum.PlayResume) = New PdfInteger(4)
        End Sub

        Public Function [Get](ByVal code As PdfInteger) As Render.OperationEnum?
            If (code Is Nothing) Then Return Nothing
            Dim operation As Render.OperationEnum? = codes.GetKey(code)
            If (Not operation.HasValue) Then Throw New NotSupportedException("Operation unknown: " & code.ToString)
            Return operation
        End Function

        <Extension>
        Public Function GetCode(ByVal operation As Render.OperationEnum) As PdfInteger
            Return codes(operation)
        End Function
    End Module

End Namespace