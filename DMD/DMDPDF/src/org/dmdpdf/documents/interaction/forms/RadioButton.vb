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
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.forms

    '/**
    '  <summary>Radio button field [PDF:1.6:8.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class RadioButton
        Inherits ButtonField

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new radiobutton within the given document context.</summary>
        '*/
        Public Sub New(ByVal name As String, ByVal widgets As DualWidget(), ByVal value As String)
            MyBase.new(name, widgets(0))
            Me.Flags = EnumUtils.Mask(EnumUtils.Mask(Flags, FlagsEnum.Radio, True), FlagsEnum.NoToggleToOff, True)
            Dim fieldWidgets As FieldWidgets = Me.Widgets
            Dim length As Integer = widgets.Length
            For index As Integer = 1 To length - 1
                fieldWidgets.Add(widgets(index))
            Next
            Me.Value = value
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets whether all the field buttons can be deselected at the same time.</summary>
        '*/
        Public Property Toggleable As Boolean
            Get
                Return (Me.Flags And FlagsEnum.NoToggleToOff) <> FlagsEnum.NoToggleToOff
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Flags, FlagsEnum.NoToggleToOff, Not value)
            End Set
        End Property

        Public Overrides Property Value As Object
            Get
                Return MyBase.Value
            End Get
            Set(ByVal value As Object)
                '/*
                '  NOTE: The parent field's V entry holds a name object corresponding to the appearance state
                '  of whichever child field is currently in the on state; the default value for this entry is
                '  Off.
                '*/
                Dim selectedWidgetName As PdfName = New PdfName(CStr(value))
                Dim selected As Boolean = False
                ' Selecting the current appearance state for each widget...
                For Each widget As Widget In Me.Widgets
                    Dim currentState As PdfName
                    If (CType(widget, DualWidget).WidgetName.Equals(value)) Then ' Selected state.
                        selected = True
                        currentState = selectedWidgetName
                    Else ' Unselected state.
                        currentState = PdfName.Off_
                    End If
                    widget.BaseDataObject(PdfName.AS) = currentState
                Next
                ' Select the current widget!
                If (selected) Then
                    BaseDataObject(PdfName.V) = selectedWidgetName
                Else
                    BaseDataObject(PdfName.V) = Nothing
                End If
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace