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
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Dual-state widget annotation.</summary>
    '  <remarks>As its name implies, it has two states: on and off.</remarks>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class DualWidget
        Inherits Widget

#Region "dynamic"
#Region "constructors"

        '/**
        '  <param name="widgetName">Widget name. It corresponds to the on-state name.</param>
        '*/
        Public Sub New(ByVal page As Page, ByVal box As RectangleF, ByVal widgetName As String)
            MyBase.New(page, box)
            '      // Initialize the on-state appearance!
            '/*
            '  NOTE: This is necessary to keep the reference to the on-state name.
            '*/
            Dim appearance As Appearance = New Appearance(page.Document)
            Me.Appearance = appearance
            Dim normalAppearance As AppearanceStates = appearance.Normal
            normalAppearance(New PdfName(widgetName)) = New FormXObject(page.Document, box.Size)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public ReadOnly Property WidgetName As String
            Get
                For Each normalAppearanceEntry As KeyValuePair(Of PdfName, FormXObject) In Appearance.Normal
                    Dim key As PdfName = normalAppearanceEntry.Key
                    If (Not key.Equals(PdfName.Off_)) Then 'On' state.
                        Return CStr(key.Value)
                    End If
                Next
                Return Nothing ' // NOTE: It MUST NOT happen (on-state should always be defined).
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace