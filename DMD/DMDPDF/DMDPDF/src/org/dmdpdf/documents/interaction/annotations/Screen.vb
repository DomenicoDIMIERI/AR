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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.forms
Imports DMD.org.dmdpdf.documents.multimedia
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections.Generic
Imports System.Drawing

Namespace DMD.org.dmdpdf.documents.interaction.annotations

    '/**
    '  <summary>Screen annotation [PDF:1.6:8.4.5].</summary>
    '  <remarks>It specifies a region of a page upon which media clips may be played.</remarks>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class Screen
        Inherits Annotation

#Region "Static"
#Region "fields"

        Private Const _PlayerPlaceholder As String = "%player%"

        '    /**
        '  <summary>Script for preview and rendering control.</summary>
        '  <remarks>NOTE: PlayerPlaceholder MUST be replaced with the actual player instance symbol.
        '  </remarks>
        '*/
        Private Const _RenderScript As String = "if(" & _PlayerPlaceholder & "==undefined){" &
                          "var doc = Me;" &
                          "var settings={autoPlay:false,visible:false,volume:100,startAt:0};" &
                          "var events=new app.media.Events({" &
                            "afterFocus:function(event){try{if(event.target.isPlaying){event.target.pause();}else{event.target.play();}doc.getField('" & _PlayerPlaceholder & "').setFocus();}catch(e){}}," &
                            "afterReady:function(event){try{event.target.seek(event.target.settings.startAt);event.target.visible=true;}catch(e){}}" &
                            "});" &
                          "var " & _PlayerPlaceholder & "=app.media.openPlayer({settings:settings,events:events});" &
                          "}"
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal page As Page, ByVal box As System.Drawing.RectangleF, ByVal text As String, ByVal mediaPath As String, ByVal mimeType As String)
            Me.New(
                   page, box, text,
                   New MediaRendition(
                        New MediaClipData(
                                FileSpecification.Get(
                                        EmbeddedFile.Get(page.Document, mediaPath),
                                        System.IO.Path.GetFileName(mediaPath)
                                        ),
                                mimeType
                            )
                        )
                    )
        End Sub

        Public Sub New(ByVal page As Page, ByVal box As System.Drawing.RectangleF, ByVal text As String, ByVal rendition As Rendition)
            MyBase.New(page, PdfName.Screen, box, text)
            Dim render As Render = New Render(Me, render.OperationEnum.PlayResume, rendition)
            '{
            '// Adding preview And play/pause control...
            '/*
            '  NOTE: Mouse-related actions don't work when the player is active; therefore, in order to let
            '    the user control the rendering of the media clip (play/pause) just by mouse-clicking on the
            '  player, we can only rely on the player's focus event. Furthermore, as the player's focus can
            '    only be altered setting it On another widget, we have to define an ancillary field on the
            '  same page(so convoluted!).
            '*/
            Dim playerReference As String = "__player" & CType(render.BaseObject, PdfReference).ObjectNumber
            Document.Form.Fields.Add(New TextField(playerReference, New Widget(page, New System.Drawing.RectangleF(box.X, box.Y, 0, 0)), "")) ' Ancillary field.
            render.Script = _RenderScript.Replace(_PlayerPlaceholder, playerReference)
            '}
            Actions.OnPageOpen = render

            If (TypeOf (rendition) Is MediaRendition) Then
                Dim data As PdfObjectWrapper = CType(rendition, MediaRendition).Clip.Data
                If (TypeOf (data) Is FileSpecification) Then
                    '// Adding fallback annotation...
                    '/*
                    '  NOTE: In case of viewers which don't support video rendering, Me annotation gently
                    '  degrades to a file attachment that can be opened on the same location of the corresponding
                    '  screen annotation.
                    '*/
                    Dim attachment As FileAttachment = New FileAttachment(page, box, text, CType(data, FileSpecification))
                    BaseDataObject(PdfName.T) = PdfString.Get(CType(data, FileSpecification).Path)
                    ' Force empty appearance to ensure no default icon is drawn on the canvas!
                    attachment.BaseDataObject(PdfName.AP) = New PdfDictionary(New PdfName() {PdfName.D, PdfName.R, PdfName.N}, New PdfDirectObject() {New PdfDictionary(), New PdfDictionary(), New PdfDictionary()})
                End If
            End If
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region
#End Region

    End Class

End Namespace