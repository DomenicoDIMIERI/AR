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
Imports DMD.org.dmdpdf.documents.interaction
'Imports DMD.actions = org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Media play parameters [PDF:1.7:9.1.4].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class MediaPlayParameters
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"

        '/**
        '  <summary> Media player parameters viability.</summary>
        '*/
        Public Class Viability
            Inherits PdfObjectWrapper(Of PdfDictionary)

            Private Class DurationObject
                Inherits PdfObjectWrapper(Of PdfDictionary)

                Friend Sub New(ByVal value As Double)
                    MyBase.New(New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.MediaDuration}))
                    Me.Value = value
                End Sub

                Friend Sub New(ByVal baseObject As PdfDirectObject)
                    MyBase.New(baseObject)
                End Sub

                '        /**
                '  <summary> Gets/Sets the temporal duration.</summary>
                '  <returns>
                '        <List type = "bullet" >
                '      <item><code>Double.NEGATIVE_INFINITY</code>: intrinsic duration of the associated media;
                '      </item>
                '        <item> <code>Double.POSITIVE_INFINITY</code>: infinite duration;</item>
                '      <item> non-infinite positive: explicit duration.</item>
                '    </list>
                '  </returns>
                '*/
                Public Property Value As Double
                    Get
                        Dim durationSubtype As PdfName = CType(BaseDataObject(PdfName.S), PdfName)
                        If (PdfName.I.Equals(durationSubtype)) Then
                            Return Double.NegativeInfinity
                        ElseIf (PdfName.F.Equals(durationSubtype)) Then
                            Return Double.PositiveInfinity
                        ElseIf (PdfName.T.Equals(durationSubtype)) Then
                            Return New Timespan(BaseDataObject(PdfName.T)).Time
                        Else
                            Throw New NotSupportedException("Duration subtype '" & durationSubtype.ToString & "'")
                        End If
                    End Get
                    Set(ByVal value As Double)
                        If (Double.IsNegativeInfinity(value)) Then
                            BaseDataObject(PdfName.S) = PdfName.I
                            BaseDataObject.Remove(PdfName.T)
                        ElseIf (Double.IsPositiveInfinity(value)) Then
                            BaseDataObject(PdfName.S) = PdfName.F
                            BaseDataObject.Remove(PdfName.T)
                        Else
                            BaseDataObject(PdfName.S) = PdfName.T
                            Dim tmp As New Timespan(BaseDataObject.Get(Of PdfDictionary)(PdfName.T))
                            tmp.Time = value
                        End If
                    End Set
                End Property
            End Class

            Public Enum FitModeEnum As Integer
                '/**
                '  <summary> The media's width and height are scaled while preserving the aspect ratio so that
                '  the media And play rectangles have the greatest possible intersection While still
                '  displaying all media content. Same As <code>meet</code> value Of SMIL's fit attribute.
                '  </summary>
                '*/
                Meet
                '/**
                '  <summary> The media's width and height are scaled while preserving the aspect ratio so that
                '  the play rectangle Is entirely filled, And the amount Of media content that does Not fit
                '  within the play rectangle Is minimized. Same As <code>slice</code> value Of SMIL's fit
                '  attribute.</summary>
                '*/
                Slice
                '/**
                '  <summary> The media's width and height are scaled independently so that the media and play
                '  rectangles are the same; the aspect ratio Is Not necessarily preserved. Same As
                '  <code> fill</code> value Of SMIL's fit attribute.</summary>
                '*/
                Fill
                '/**
                '  <summary> The media Is Not scaled. A scrolling user Interface Is provided If the media
                '  rectangle Is wider Or taller than the play rectangle. Same as <code>scroll</code> value of
                '  SMIL's fit attribute.</summary>
                '*/
                Scroll
                '/**
                '  <summaryCThe media Is Not scaled. Only the portions Of the media rectangle that intersect
                '  the play rectangle are displayed. Same As <code>hidden</code> value Of SMIL's fit attribute.
                '  </summary>
                '*/
                Hidden
                '/**
                '  <summary> Use the player's default setting (author has no preference).</summary>
                '*/
                [Default]
            End Enum

            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

            '/**
            '  <summary> Gets/Sets whether the media should automatically play When activated.</summary>
            '*/
            Public Property Autoplay As Boolean
                Get
                    Return CBool(PdfBoolean.GetValue(BaseDataObject(PdfName.A), True))
                End Get
                Set(ByVal value As Boolean)
                    BaseDataObject(PdfName.A) = PdfBoolean.Get(value)
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the temporal duration, corresponding To the notion Of simple duration In
            '  SMIL.</summary>
            '  <returns>
            '          <List type = "bullet" >
            '      <item><code>Double.NEGATIVE_INFINITY</code>: intrinsic duration of the associated media;
            '      </item>
            '          <item> <code>Double.POSITIVE_INFINITY</code>: infinite duration;</item>
            '      <item> non-infinite positive: explicit duration.</item>
            '    </list>
            '  </returns>
            '*/
            Public Property Duration As Double
                Get
                    Dim durationObject As PdfDirectObject = BaseDataObject(PdfName.D)
                    If (durationObject IsNot Nothing) Then
                        Return New DurationObject(durationObject).Value
                    Else
                        Return Double.NegativeInfinity
                    End If
                End Get
                Set(ByVal value As Double)
                    BaseDataObject(PdfName.D) = New DurationObject(value).BaseObject
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the manner In which the player should treat a visual media type that does
            '  Not exactly fit the rectangle in which it plays.</summary>
            '*/
            Public Property FitMode As FitModeEnum?
                Get
                    Return FitModeEnumExtension.Get(CType(BaseDataObject(PdfName.F), PdfInteger))
                End Get
                Set(ByVal value As FitModeEnum?)
                    If (value.HasValue) Then
                        BaseDataObject(PdfName.F) = value.Value.GetCode()
                    Else
                        BaseDataObject(PdfName.F) = Nothing
                    End If
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets whether To display a player-specific controller user Interface (For
            '  example, play/pause/stop controls) when playing.</summary>
            '*/
            Public Property PlayerSpecificControl As Boolean
                Get
                    Return CBool(PdfBoolean.GetValue(BaseDataObject(PdfName.C), False))
                End Get
                Set(ByVal value As Boolean)
                    BaseDataObject(PdfName.C) = PdfBoolean.Get(value)
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the number Of iterations Of the duration To repeat; similar To SMIL's
            '  <code> repeatCount</code> attribute.</summary>
            '  <returns>
            '          <List type = "bullet" >
            '      <item><code>0</code>: repeat forever;</item>
            '                    </list>
            '  </returns>
            '*/
            Public Property RepeatCount As Double
                Get
                    Return CDbl(PdfReal.GetValue(BaseDataObject(PdfName.RC), 1D))
                End Get
                Set(ByVal value As Double)
                    BaseDataObject(PdfName.RC) = PdfReal.Get(value)
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the volume level As a percentage Of recorded volume level. A zero value
            '  Is equivalent to mute.</summary>
            '*/
            Public Property Volume As Integer
                Get
                    Return CInt(PdfInteger.GetValue(BaseDataObject(PdfName.V), 100))
                End Get
                Set(ByVal value As Integer)
                    If (value < 0) Then
                        value = 0
                    ElseIf (value > 100) Then
                        value = 100
                    End If
                    BaseDataObject(PdfName.V) = PdfInteger.Get(value)
                End Set
            End Property

        End Class


#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.MediaPlayParams}))
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

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
        '  <summary> Gets/Sets the preferred options the renderer should attempt To honor without affecting
        '  its viability.</summary>
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
        '  <summary> Gets/Sets the minimum requirements the renderer must honor In order To be considered
        '  viable.</summary>
        '*/
        Public Property Requirements As Viability
            Get
                Return New Viability(BaseDataObject.Get(Of PdfDictionary)(PdfName.MH))
            End Get
            Set(ByVal value As Viability)
                BaseDataObject(PdfName.MH) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

#End Region
#End Region
#End Region
    End Class


    Module FitModeEnumExtension 'Static 

        Private ReadOnly codes As BiDictionary(Of MediaPlayParameters.Viability.FitModeEnum, PdfInteger)

        Sub New()
            codes = New BiDictionary(Of MediaPlayParameters.Viability.FitModeEnum, PdfInteger)
            codes(MediaPlayParameters.Viability.FitModeEnum.Meet) = New PdfInteger(0)
            codes(MediaPlayParameters.Viability.FitModeEnum.Slice) = New PdfInteger(1)
            codes(MediaPlayParameters.Viability.FitModeEnum.Fill) = New PdfInteger(2)
            codes(MediaPlayParameters.Viability.FitModeEnum.Scroll) = New PdfInteger(3)
            codes(MediaPlayParameters.Viability.FitModeEnum.Hidden) = New PdfInteger(4)
            codes(MediaPlayParameters.Viability.FitModeEnum.Default) = New PdfInteger(5)
        End Sub

        Public Function [Get](ByVal code As PdfInteger) As MediaPlayParameters.Viability.FitModeEnum?
            If (code Is Nothing) Then Return MediaPlayParameters.Viability.FitModeEnum.Default
            Dim mode As MediaPlayParameters.Viability.FitModeEnum? = codes.GetKey(code)
            If (Not mode.HasValue) Then Throw New NotSupportedException("Mode unknown: " & code.ToString)
            Return mode
        End Function

        <Extension>
        Public Function GetCode(ByVal mode As MediaPlayParameters.Viability.FitModeEnum) As PdfInteger  'this 
            Return codes(mode)
        End Function

    End Module

End Namespace
