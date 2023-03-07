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

Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents

    '  /**
    '  <summary>Document actions [PDF:1.6:8.5.2].</summary>
    '*/
    <PDF(VersionEnum.PDF14)>
    Public NotInheritable Class DocumentActions
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary())
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets/Sets the action to be performed after printing the document.</summary>
        '*/
        Public Property AfterPrint As interaction.actions.Action
            Get
                Return interaction.actions.Action.Wrap(BaseDataObject(PdfName.DP))
            End Get
            Set(ByVal value As interaction.actions.Action)
                BaseDataObject(PdfName.DP) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the action to be performed after saving the document.</summary>
        '*/
        Public Property AfterSave As interaction.actions.Action
            Get
                Return interaction.actions.Action.Wrap(BaseDataObject(PdfName.DS))
            End Get
            Set(ByVal value As interaction.actions.Action)
                BaseDataObject(PdfName.DS) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the action to be performed before printing the document.</summary>
        '*/
        Public Property BeforePrint As interaction.actions.Action
            Get
                Return interaction.actions.Action.Wrap(BaseDataObject(PdfName.WP))
            End Get
            Set(ByVal value As interaction.actions.Action)
                BaseDataObject(PdfName.WP) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the action to be performed before saving the document.</summary>
        '*/
        Public Property BeforeSave As interaction.actions.Action
            Get
                Return interaction.actions.Action.Wrap(BaseDataObject(PdfName.WS))
            End Get
            Set(ByVal value As interaction.actions.Action)
                BaseDataObject(PdfName.WS) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the action to be performed before closing the document.</summary>
        '*/
        Public Property OnClose As interaction.actions.Action
            Get
                Return interaction.actions.Action.Wrap(BaseDataObject(PdfName.DC))
            End Get
            Set(ByVal value As interaction.actions.Action)
                BaseDataObject(PdfName.DC) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the destination to be displayed or the action to be performed
        '  after opening the document.</summary>
        '*/
        Public Property OnOpen As PdfObjectWrapper
            Get
                Dim onOpenObject As PdfDirectObject = Document.BaseDataObject(PdfName.OpenAction)
                If (TypeOf (onOpenObject) Is PdfDictionary) Then ' Action (dictionary).
                    Return interaction.actions.Action.Wrap(onOpenObject)
                Else 'Destination (array).
                    Return Destination.Wrap(onOpenObject)
                End If
            End Get
            Set(ByVal value As PdfObjectWrapper)
                If (Not (TypeOf (value) Is interaction.actions.Action OrElse TypeOf (value) Is LocalDestination)) Then
                    Throw New System.ArgumentException("Value MUST be either an Action or a LocalDestination.")
                End If
                Document.BaseDataObject(PdfName.OpenAction) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
