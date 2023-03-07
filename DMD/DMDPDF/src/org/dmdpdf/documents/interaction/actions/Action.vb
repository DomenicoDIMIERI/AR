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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>Action to be performed by the viewer application [PDF:1.6:8.5].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public Class Action
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary>Wraps an action base object into an action object.</summary>
        '  <param name="baseObject">Action base object.</param>
        '  <returns>Action object associated to the base object.</returns>
        '*/
        Public Shared Function Wrap(ByVal baseObject As PdfDirectObject) As Action
            If (baseObject Is Nothing) Then Return Nothing

            Dim dataObject As PdfDictionary = CType(baseObject.Resolve(), PdfDictionary)
            Dim actionType As PdfName = CType(dataObject(PdfName.S), PdfName)
            If (actionType Is Nothing OrElse
               (dataObject.ContainsKey(PdfName.Type) AndAlso Not dataObject(PdfName.Type).Equals(PdfName.Action))) Then
                Return Nothing
            End If

            If (actionType.Equals(PdfName.GoTo)) Then
                Return New GoToLocal(baseObject)
            ElseIf (actionType.Equals(PdfName.GoToR)) Then
                Return New GoToRemote(baseObject)
            ElseIf (actionType.Equals(PdfName.GoToE)) Then
                Return New GoToEmbedded(baseObject)
            ElseIf (actionType.Equals(PdfName.Launch)) Then
                Return New Launch(baseObject)
            ElseIf (actionType.Equals(PdfName.Thread)) Then
                Return New GoToThread(baseObject)
            ElseIf (actionType.Equals(PdfName.URI)) Then
                Return New GoToURI(baseObject)
            ElseIf (actionType.Equals(PdfName.Sound)) Then
                Return New PlaySound(baseObject)
            ElseIf (actionType.Equals(PdfName.Movie)) Then
                Return New PlayMovie(baseObject)
            ElseIf (actionType.Equals(PdfName.Hide)) Then
                Return New ToggleVisibility(baseObject)
            ElseIf (actionType.Equals(PdfName.Named)) Then
                Dim actionName As PdfName = CType(dataObject(PdfName.N), PdfName)
                If (actionName.Equals(PdfName.NextPage)) Then
                    Return New GoToNextPage(baseObject)
                ElseIf (actionName.Equals(PdfName.PrevPage)) Then
                    Return New GoToPreviousPage(baseObject)
                ElseIf (actionName.Equals(PdfName.FirstPage)) Then
                    Return New GoToFirstPage(baseObject)
                ElseIf (actionName.Equals(PdfName.LastPage)) Then
                    Return New GoToLastPage(baseObject)
                Else ' Custom named action.
                    Return New NamedAction(baseObject)
                End If
            ElseIf (actionType.Equals(PdfName.SubmitForm)) Then
                Return New SubmitForm(baseObject)
            ElseIf (actionType.Equals(PdfName.ResetForm)) Then
                Return New ResetForm(baseObject)
            ElseIf (actionType.Equals(PdfName.ImportData)) Then
                Return New ImportData(baseObject)
            ElseIf (actionType.Equals(PdfName.JavaScript)) Then
                Return New JavaScript(baseObject)
            ElseIf (actionType.Equals(PdfName.SetOCGState)) Then
                Return New SetLayerState(baseObject)
            ElseIf (actionType.Equals(PdfName.Rendition)) Then
                Return New Render(baseObject)
            ElseIf (actionType.Equals(PdfName.Trans)) Then
                Return New DoTransition(baseObject)
            ElseIf (actionType.Equals(PdfName.GoTo3DView)) Then
                Return New GoTo3dView(baseObject)
            Else ' Custom action.
                Return New Action(baseObject)
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a New action within the given document context.</summary>
        '*/
        Protected Sub New(ByVal context As Document, ByVal actionType As PdfName)
            MyBase.New(context, New PdfDictionary(
                                        New PdfName() {
                                                PdfName.Type,
                                                PdfName.S
                                                },
                                        New PdfDirectObject() {
                                                PdfName.Action,
                                                actionType
                                                }
                                            )
                            )
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the actions To be performed after the current one.</summary>
        '*/
        <PDF(VersionEnum.PDF12)>
        Public Property Actions As ChainedActions
            Get
                Dim nextObject As PdfDirectObject = BaseDataObject(PdfName.Next)
                If (nextObject IsNot Nothing) Then
                    Return New ChainedActions(nextObject, Me)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As ChainedActions)
                BaseDataObject(PdfName.Next) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace