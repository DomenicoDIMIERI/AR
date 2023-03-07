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
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>'Launch an application' action [PDF:1.6:8.5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class Launch
        Inherits Action

#Region "types"
        '/**
        '  <summary>Windows-specific launch parameters [PDF:1.6:8.5.3].</summary>
        '*/
        Public Class WinTarget
            Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
            '/**
            '  <summary>Operation [PDF:1.6:8.5.3].</summary>
            '*/
            Public Enum OperationEnum

                '    /**
                '  <summary>Open.</summary>
                '*/
                Open
                '/**
                '  <summary>Print.</summary>
                '*/
                Print
            End Enum
#End Region

#Region "Static"
#Region "fields"

            Private Shared ReadOnly _OperationEnumCodes As Dictionary(Of OperationEnum, PdfString)

#End Region

#Region "constructors"

            Shared Sub New()
                _OperationEnumCodes = New Dictionary(Of OperationEnum, PdfString)
                _OperationEnumCodes(OperationEnum.Open) = New PdfString("open")
                _OperationEnumCodes(OperationEnum.Print) = New PdfString("print")
            End Sub

#End Region

#Region "interface"
#Region "private"

            '/**
            '  <summary>Gets the code corresponding to the given value.</summary>
            '*/
            Private Shared Function ToCode(ByVal value As OperationEnum) As PdfString
                Return _OperationEnumCodes(value)
            End Function

            '/**
            '  <summary>Gets the operation corresponding to the given value.</summary>
            '*/
            Private Shared Function ToOperationEnum(ByVal value As PdfString) As OperationEnum
                For Each operation As KeyValuePair(Of OperationEnum, PdfString) In _OperationEnumCodes
                    If (operation.Value.Equals(value)) Then
                        Return operation.Key
                    End If
                Next
                Return OperationEnum.Open
            End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

            Public Sub New(ByVal context As Document, ByVal fileName As String)
                MyBase.New(context, New PdfDictionary())
                Me.FileName = fileName
            End Sub

            Public Sub New(ByVal context As Document, ByVal fileName As String, ByVal operation As OperationEnum)
                Me.New(context, fileName)
                Me.Operation = operation
            End Sub

            Public Sub New(ByVal context As Document, ByVal fileName As String, ByVal parameterString As String)
                Me.New(context, fileName)
                Me.ParameterString = parameterString
            End Sub

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

#End Region

#Region "Interface"
#Region "Public"

            Public Overrides Function Clone(ByVal context As Document) As Object
                Throw New NotImplementedException()
            End Function

            '/**
            '  <summary>Gets/Sets the default directory.</summary>
            '*/
            Public Property DefaultDirectory As String
                Get
                    Dim defaultDirectoryObject As PdfString = CType(BaseDataObject(PdfName.D), PdfString)
                    If (defaultDirectoryObject IsNot Nothing) Then
                        Return CStr(defaultDirectoryObject.Value)
                    Else
                        Return Nothing
                    End If
                End Get
                Set(ByVal value As String)
                    Me.BaseDataObject(PdfName.D) = New PdfString(value)
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the file name of the application to be launched
            '  or the document to be opened or printed.</summary>
            '*/
            Public Property FileName As String
                Get
                    Return CStr(CType(BaseDataObject(PdfName.F), PdfString).Value)
                End Get
                Set(ByVal value As String)
                    Me.BaseDataObject(PdfName.F) = New PdfString(value)
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the operation To perform.</summary>
            '*/
            Public Property Operation As OperationEnum
                Get
                    Return ToOperationEnum(CType(BaseDataObject(PdfName.O), PdfString))
                End Get
                Set(ByVal value As OperationEnum)
                    BaseDataObject(PdfName.O) = ToCode(value)
                End Set
            End Property

            '/**
            '  <summary>Gets/Sets the parameter String To be passed To the application.</summary>
            '*/
            Public Property ParameterString As String
                Get
                    Dim parameterStringObject As PdfString = CType(BaseDataObject(PdfName.P), PdfString)
                    If (parameterStringObject IsNot Nothing) Then
                        Return CStr(parameterStringObject.Value)
                    Else
                        Return Nothing
                    End If
                End Get
                Set(ByVal value As String)
                    Me.BaseDataObject(PdfName.P) = New PdfString(value)
                End Set
            End Property

#End Region
#End Region
#End Region

        End Class

#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a launcher.</summary>
        '  <param name = "context" > Document context.</param>
        '  <param name = "target" > Either a <see cref="FileSpecification"/> Or a <see cref="WinTarget"/>
        '  representing either an application Or a document.</param>
        '*/
        Public Sub New(ByVal context As Document, ByVal target As PdfObjectWrapper)
            MyBase.New(context, PdfName.Launch)
            Me.Target = target
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the action options.</summary>
        '*/
        Public Property Options As OptionsEnum
            Get
                Dim _options As OptionsEnum = 0
                Dim optionsObject As PdfDirectObject = BaseDataObject(PdfName.NewWindow)
                If (optionsObject IsNot Nothing AndAlso
                        CType(optionsObject, PdfBoolean).BooleanValue) Then
                    _options = _options Or OptionsEnum.NewWindow
                End If
                Return _options
            End Get
            Set(ByVal value As OptionsEnum)
                If ((value And OptionsEnum.NewWindow) = OptionsEnum.NewWindow) Then
                    BaseDataObject(PdfName.NewWindow) = PdfBoolean.True
                ElseIf ((value And OptionsEnum.SameWindow) = OptionsEnum.SameWindow) Then
                    BaseDataObject(PdfName.NewWindow) = PdfBoolean.False
                Else
                    BaseDataObject.Remove(PdfName.NewWindow) ' NOTE: Forcing the absence of this entry ensures that the viewer application should behave in accordance with the current user preference.
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the application To be launched Or the document To be opened Or printed.
        '  </summary>
        '*/
        Public Property Target As PdfObjectWrapper
            Get
                Dim targetObject As PdfDirectObject
                targetObject = BaseDataObject(PdfName.F)
                If (targetObject IsNot Nothing) Then Return FileSpecification.Wrap(targetObject)
                targetObject = BaseDataObject(PdfName.Win)
                If (targetObject IsNot Nothing) Then Return New WinTarget(targetObject)
                Return Nothing
            End Get
            Set(ByVal value As PdfObjectWrapper)
                If (TypeOf (value) Is FileSpecification) Then
                    BaseDataObject(PdfName.F) = CType(value, FileSpecification).BaseObject
                ElseIf (TypeOf (value) Is WinTarget) Then
                    BaseDataObject(PdfName.Win) = CType(value, WinTarget).BaseObject
                Else
                    Throw New ArgumentException("MUST be either FileSpecification or WinTarget")
                End If
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace