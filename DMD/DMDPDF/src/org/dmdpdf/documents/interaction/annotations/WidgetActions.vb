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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System '
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Widget actions [PDF:1.6:8.5.2].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class WidgetActions
        Inherits AnnotationActions

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal parent As Annotation)
            MyBase.new(parent)
        End Sub

        Friend Sub New(ByVal parent As Annotation, ByVal baseObject As PdfDirectObject)
            MyBase.new(parent, baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException ' TODO: verify parent reference.
        End Function

        '    /**
        '  <summary>Gets/Sets the action to be performed when the annotation loses the input focus.</summary>
        '*/
        Public Property OnBlur As actions.Action
            Get
                Return actions.Action.Wrap(BaseDataObject(PdfName.Bl))
            End Get
            Set(ByVal value As actions.Action)
                Me.BaseDataObject(PdfName.Bl) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the action to be performed when the annotation receives the input focus.</summary>
        '*/
        Public Property OnFocus As actions.Action
            Get
                Return actions.Action.Wrap(BaseDataObject(PdfName.Fo))
            End Get
            Set(ByVal value As actions.Action)
                Me.BaseDataObject(PdfName.Fo) = value.BaseObject
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace