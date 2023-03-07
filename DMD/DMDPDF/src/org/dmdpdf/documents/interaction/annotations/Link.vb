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
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Link annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It represents either a hypertext link to a destination elsewhere in the document
    '  or an action to be performed.</remarks>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Link
        Inherits Annotation
        Implements ILink

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal text As String, ByVal target As PdfObjectWrapper)
            MyBase.New(page, PdfName.Link, box, text)
            Me.Target = target
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Property Action As actions.Action
            Get
                Return MyBase.Action
            End Get
            Set(ByVal value As actions.Action)
                '/*
                '  NOTE: This entry Is Not permitted in link annotations if a 'Dest' entry is present.
                '*/
                If (BaseDataObject.ContainsKey(PdfName.Dest) AndAlso value IsNot Nothing) Then
                    BaseDataObject.Remove(PdfName.Dest)
                End If

                MyBase.Action = value
            End Set
        End Property

#Region "ILink"

        Public Property Target As PdfObjectWrapper Implements ILink.Target
            Get
                If (BaseDataObject.ContainsKey(PdfName.Dest)) Then
                    Return Me.Destination
                ElseIf (BaseDataObject.ContainsKey(PdfName.A)) Then
                    Return Me.Action
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As PdfObjectWrapper)
                If (TypeOf (value) Is Destination) Then
                    Me.Destination = CType(value, Destination)
                ElseIf (TypeOf (value) Is actions.Action) Then
                    Me.Action = CType(value, actions.Action)
                Else
                    Throw New ArgumentException("It MUST be either a Destination or an Action.")
                End If
            End Set
        End Property

#End Region
#End Region

#Region "Private"

        Private Property Destination As Destination
            Get
                Dim destinationObject As PdfDirectObject = BaseDataObject(PdfName.Dest)
                If (destinationObject IsNot Nothing) Then
                    Return Document.ResolveName(Of Destination)(destinationObject)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Destination)
                If (value Is Nothing) Then
                    BaseDataObject.Remove(PdfName.Dest)
                Else
                    '/*
                    '  NOTE: This entry is not permitted in link annotations if an 'A' entry is present.
                    '*/
                    If (BaseDataObject.ContainsKey(PdfName.A)) Then
                        BaseDataObject.Remove(PdfName.A)
                    End If

                    BaseDataObject(PdfName.Dest) = value.NamedBaseObject
                End If
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace