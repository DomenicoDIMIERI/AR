'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.files
'Imports DMD.actions = org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Media clip data [PDF:1.7:9.1.3].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class MediaClipData
        Inherits MediaClip

#Region "types"
        '/**
        '  <summary>Circumstance under which it is acceptable to write a temporary file in order to play
        '  a media clip.</summary>
        '*/
        Public Enum TempFilePermissionEnum As Integer

            '/**
            '  <summary>Never allowed.</summary>
            '*/
            Never
            '/**
            '  <summary>Allowed only if the document permissions allow content extraction.</summary>
            '*/
            ContentExtraction
            '/**
            '  <summary>Allowed only if the document permissions allow content extraction, including for
            '  accessibility purposes.</summary>
            '*/
            Accessibility
            '/**
            '  <summary>Always allowed.</summary>
            '*/
            Always
        End Enum

        '/**
        '  <summary>Media clip data viability.</summary>
        '*/
        Public Class Viability
            Inherits PdfObjectWrapper(Of PdfDictionary)

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

            '/**
            '  <summary> Gets the absolute URL To be used As the base URL In resolving any relative URLs
            '  found within the media data.</summary>
            '*/
            Public Property BaseURL As Uri
                Get
                    Dim baseURLObject As PdfString = CType(BaseDataObject(PdfName.BU), PdfString)
                    If (baseURLObject IsNot Nothing) Then
                        Return New Uri(baseURLObject.StringValue)
                    Else
                        Return Nothing
                    End If
                End Get
                Set(ByVal value As Uri)
                    If (value IsNot Nothing) Then
                        BaseDataObject(PdfName.BU) = New PdfString(value.ToString())
                    Else
                        BaseDataObject(PdfName.BU) = Nothing
                    End If
                End Set
            End Property
        End Class

#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal data As PdfObjectWrapper, ByVal mimeType As String)
            MyBase.New(data.Document, PdfName.MCD)
            Me.Data = data
            Me.MimeType = mimeType
            Me.TempFilePermission = TempFilePermissionEnum.Always
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Property Data As PdfObjectWrapper
            Get
                Dim dataObject As PdfDirectObject = BaseDataObject(PdfName.D)
                If (dataObject Is Nothing) Then Return Nothing
                If (TypeOf (dataObject.Resolve()) Is PdfStream) Then
                    Return FormXObject.Wrap(dataObject)
                Else
                    Return FileSpecification.Wrap(dataObject)
                End If
            End Get
            Set(ByVal value As PdfObjectWrapper)
                BaseDataObject(PdfName.D) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the MIME type Of data [RFC 2045].</summary>
        '*/
        Public Property MimeType As String
            Get
                Return CStr(PdfString.GetValue(BaseDataObject(PdfName.CT)))
            End Get
            Set(ByVal value As String)
                If (value IsNot Nothing) Then
                    BaseDataObject(PdfName.CT) = New PdfString(value)
                Else
                    BaseDataObject(PdfName.CT) = Nothing
                End If
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the player rules For playing this media.</summary>
        '*/
        Public Property Players As MediaPlayers
            Get
                Return MediaPlayers.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.PL))
            End Get
            Set(ByVal value As MediaPlayers)
                BaseDataObject(PdfName.PL) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the preferred options the renderer should attempt To honor without affecting its
        '  viability.</summary>
        '*/
        Public Property Preferences As Viability
            Get
                Return New Viability(BaseDataObject.Get(Of PdfDictionary)(PdfName.BE))
            End Get
            Set(ByVal value As Viability)
                BaseDataObject(PdfName.BE) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the minimum requirements the renderer must honor In order To be considered viable.
        '  </summary>
        '*/
        Public Property Requirements As Viability
            Get
                Return New Viability(BaseDataObject.Get(Of PdfDictionary)(PdfName.MH))
            End Get
            Set(ByVal value As Viability)
                BaseDataObject(PdfName.MH) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the circumstance under which it Is acceptable To write a temporary file In order
        '  to play this media clip.</summary>
        '*/
        Public Property TempFilePermission As TempFilePermissionEnum?
            Get
                Return TempFilePermissionEnumExtension.Get(CType(BaseDataObject.Resolve(Of PdfDictionary)(PdfName.P)(PdfName.TF), PdfString))
            End Get
            Set(ByVal value As TempFilePermissionEnum?)
                If (value.HasValue) Then
                    BaseDataObject.Resolve(Of PdfDictionary)(PdfName.P)(PdfName.TF) = value.Value.GetCode()
                Else
                    BaseDataObject.Resolve(Of PdfDictionary)(PdfName.P)(PdfName.TF) = Nothing
                End If
            End Set
        End Property

#End Region
#End Region
#End Region
    End Class


    Module TempFilePermissionEnumExtension 'Static 

        Private ReadOnly codes As BiDictionary(Of MediaClipData.TempFilePermissionEnum, PdfString)

        Sub New()
            codes = New BiDictionary(Of MediaClipData.TempFilePermissionEnum, PdfString)
            codes(MediaClipData.TempFilePermissionEnum.Never) = New PdfString("TEMPNEVER")
            codes(MediaClipData.TempFilePermissionEnum.ContentExtraction) = New PdfString("TEMPEXTRACT")
            codes(MediaClipData.TempFilePermissionEnum.Accessibility) = New PdfString("TEMPACCESS")
            codes(MediaClipData.TempFilePermissionEnum.Always) = New PdfString("TEMPALWAYS")
        End Sub

        Public Function [Get](ByVal code As PdfString) As MediaClipData.TempFilePermissionEnum?
            If (code Is Nothing) Then Return Nothing
            Dim tempFilePermission As MediaClipData.TempFilePermissionEnum? = codes.GetKey(code)
            If (Not tempFilePermission.HasValue) Then Throw New NotSupportedException("Operation unknown: " & code.ToString)
            Return tempFilePermission
        End Function

        <Extension>
        Public Function GetCode(ByVal tempFilePermission As MediaClipData.TempFilePermissionEnum) As PdfString  'this 
            Return codes(tempFilePermission)
        End Function

    End Module

End Namespace
