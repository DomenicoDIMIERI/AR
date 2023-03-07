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

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.forms

    '/**
    '  <summary>Check box field [PDF:1.6:8.6.3].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public NotInheritable Class CheckBox
        Inherits ButtonField

#Region "dynamic"
#Region "constructors"
        '/**
        '  <summary>Creates a new checkbox within the given document context.</summary>
        '*/
        Public Sub New(ByVal name As String, ByVal widget As Widget, ByVal checked_ As Boolean)
            MyBase.New(name, widget)
            Me.Checked = checked_
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Property Checked As Boolean
            Get
                Dim value As PdfName = CType(BaseDataObject(PdfName.V), PdfName)
                Return Not (value Is Nothing OrElse value.Equals(PdfName.Off_))
            End Get
            Set(ByVal value As Boolean)
                Dim widgetDictionary As PdfDictionary = Me.Widgets(0).BaseDataObject
                '/*
                '  NOTE: The appearance For the off state Is Optional but, If present, MUST be stored in the
                '  appearance Dictionary under the name Off. The recommended (but Not required) name for the
                '  On state Is Yes.
                '*/
                Dim baseValue As PdfName = Nothing
                If (value) Then
                    Dim appearanceDictionary As PdfDictionary = CType(widgetDictionary.Resolve(PdfName.AP), PdfDictionary)
                    If (appearanceDictionary IsNot Nothing) Then
                        For Each appearanceKey As PdfName In CType(appearanceDictionary.Resolve(PdfName.N), PdfDictionary).Keys
                            If (Not appearanceKey.Equals(PdfName.Off_)) Then
                                baseValue = appearanceKey
                                Exit For
                            End If
                        Next
                    Else
                        baseValue = PdfName.Yes
                    End If
                Else
                    baseValue = PdfName.Off_
                End If
                BaseDataObject(PdfName.V) = baseValue
                widgetDictionary(PdfName.AS) = baseValue
            End Set
        End Property

        Public Overrides Property Value As Object
            Get
                Return MyBase.Value
            End Get
            Set(ByVal value As Object)
                Me.Checked = Not (value Is Nothing OrElse value.Equals(String.Empty) OrElse value.Equals(PdfName.Off_.Value))
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace