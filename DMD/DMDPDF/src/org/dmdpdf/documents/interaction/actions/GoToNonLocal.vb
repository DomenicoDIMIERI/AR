'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>Abstract 'go to non-local destination' action.</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public MustInherit Class GotoNonLocal(Of T As Destination)
        Inherits GoToDestination(Of T)

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal context As Document, ByVal actionType As PdfName, ByVal destinationFile As FileSpecification, ByVal destination As T)
            MyBase.new(context, actionType, destination)
            Me.DestinationFile = destinationFile
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the file in which the destination is located.</summary>
        '*/
        Public Overridable Property DestinationFile As FileSpecification
            Get
                Return FileSpecification.Wrap(BaseDataObject(PdfName.F))
            End Get
            Set(ByVal value As FileSpecification)
                If (value IsNot Nothing) Then
                    BaseDataObject(PdfName.F) = value.BaseObject
                Else
                    BaseDataObject(PdfName.F) = Nothing
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the action options.</summary>
        '*/
        Public Property Options As OptionsEnum
            Get
                Dim _options As OptionsEnum = 0
                Dim optionsObject As PdfDirectObject = Me.BaseDataObject(PdfName.NewWindow)
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

#End Region
#End Region
#End Region

    End Class
End Namespace