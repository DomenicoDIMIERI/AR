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
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Drawing 'System.Drawing;
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents.multimedia

    '/**
    '  <summary>Media screen parameters [PDF:1.7:9.1.5].</summary>
    '*/
    <PDF(VersionEnum.PDF15)>
    Public NotInheritable Class MediaScreenParameters
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
        '/**
        '  <summary> Media screen parameters viability.</summary>
        '*/
        Public Class Viability
            Inherits PdfObjectWrapper(Of PdfDictionary)

            Public Class FloatingWindowParametersObject
                Inherits PdfObjectWrapper(Of PdfDictionary)

                Public Enum LocationEnum As Integer

                    '/**
                    '  <summary> Upper-left corner.</summary>
                    '*/
                    UpperLeft
                    '/**
                    '  <summary> Upper center.</summary>
                    '*/
                    UpperCenter
                    '/**
                    '  <summary> Upper-right corner.</summary>
                    '*/
                    UpperRight
                    '/**
                    '  <summary> Center left.</summary>
                    '*/
                    CenterLeft
                    '/**
                    '  <summary> Center.</summary>
                    '*/
                    Center
                    '/**
                    '  <summary> Center right.</summary>
                    '*/
                    CenterRight
                    '/**
                    '  <summary> Lower-left corner.</summary>
                    '*/
                    LowerLeft
                    '/**
                    '  <summary> Lower center.</summary>
                    '*/
                    LowerCenter
                    '/**
                    '  <summary> Lower-right corner.</summary>
                    '*/
                    LowerRight
                End Enum

                Public Enum OffscreenBehaviorEnum As Integer

                    '/**
                    '  <summary> Take no special action.</summary>
                    '*/
                    None
                    '/**
                    '  <summary> Move And/Or resize the window so that it Is On-screen.</summary>
                    '*/
                    Adapt
                    '/**
                    '  <summary> Consider the Object To be non-viable.</summary>
                    '*/
                    NonViable
                End Enum

                Public Enum RelatedWindowEnum As Integer

                    '/**
                    '  <summary> The document window.</summary>
                    '*/
                    Document
                    '/**
                    '  <summary> The application window.</summary>
                    '*/
                    Application
                    '/**
                    '  <summary> The full virtual desktop.</summary>
                    '*/
                    Desktop
                    '/**
                    '  <summary> The monitor specified by <see cref="MediaScreenParameters.Viability.MonitorSpecifier"/>.</summary>
                    '*/
                    Custom
                End Enum

                Public Enum ResizeBehaviorEnum As Integer
                    '/**
                    '  <summary>Not resizable.</summary>
                    '*/
                    None
                    '/**
                    '  <summary> Resizable preserving its aspect ratio.</summary>
                    '*/
                    AspectRatioLocked
                    '/**
                    '  <summary> Resizable without preserving its aspect ratio.</summary>
                    '*/
                    Free
                End Enum

                Public Sub New(ByVal size As Drawing.Size)
                    MyBase.New(New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.FWParams}))
                    Me.Size = size
                End Sub

                Friend Sub New(ByVal baseObject As PdfDirectObject)
                    MyBase.New(baseObject)
                End Sub

                '/**
                '  <summary> Gets/Sets the location where the floating window should be positioned relative To
                '  the related window.</summary>
                '*/
                Public Property Location As LocationEnum?
                    Get
                        Return LocationEnumExtension.Get(CType(BaseDataObject(PdfName.P), PdfInteger))
                    End Get
                    Set(ByVal value As LocationEnum?)
                        If (value.HasValue) Then
                            BaseDataObject(PdfName.P) = value.Value.GetCode()
                        Else
                            BaseDataObject(PdfName.P) = Nothing
                        End If
                    End Set
                End Property

                '/**
                '  <summary> Gets/Sets what should occur If the floating window Is positioned totally Or
                '  partially offscreen(that Is, Not visible on any physical monitor).</summary>
                '*/
                Public Property OffscreenBehavior As OffscreenBehaviorEnum?
                    Get
                        Return OffscreenBehaviorEnumExtension.Get(CType(BaseDataObject(PdfName.O), PdfInteger))
                    End Get
                    Set(ByVal value As OffscreenBehaviorEnum?)
                        If (value.HasValue) Then
                            BaseDataObject(PdfName.O) = value.Value.GetCode()
                        Else
                            BaseDataObject(PdfName.O) = Nothing
                        End If
                    End Set
                End Property

                '/**
                '  <summary> Gets/Sets the window relative To which the floating window should be positioned.
                '  </summary>
                '*/
                Public Property RelatedWindow As RelatedWindowEnum?
                    Get
                        Return RelatedWindowEnumExtension.Get(CType(BaseDataObject(PdfName.RT), PdfInteger))
                    End Get
                    Set(ByVal value As RelatedWindowEnum?)
                        If (value.HasValue) Then
                            BaseDataObject(PdfName.RT) = value.Value.GetCode()
                        Else
                            BaseDataObject(PdfName.RT) = Nothing
                        End If
                    End Set
                End Property

                '/**
                '  <summary> Gets/Sets how the floating window may be resized by a user.</summary>
                '*/
                Public Property ResizeBehavior As ResizeBehaviorEnum?
                    Get
                        Return ResizeBehaviorEnumExtension.Get(CType(BaseDataObject(PdfName.R), PdfInteger))
                    End Get
                    Set(ByVal value As ResizeBehaviorEnum?)
                        If (value.HasValue) Then
                            BaseDataObject(PdfName.R) = value.Value.GetCode()
                        Else
                            BaseDataObject(PdfName.R) = Nothing
                        End If
                    End Set
                End Property

                '/**
                '  <summary> Gets/Sets the floating window's width and height, in pixels.</summary>
                '  <remarks> These values correspond To the dimensions Of the rectangle In which the media
                '  will play, not including such items As title bar And resizing Handles.</remarks>
                '*/
                Public Property Size As Drawing.Size
                    Get
                        Dim sizeObject As PdfArray = CType(BaseDataObject(PdfName.D), PdfArray)
                        Return New System.Drawing.Size(CType(sizeObject(0), PdfInteger).IntValue, CType(sizeObject(1), PdfInteger).IntValue)
                    End Get
                    Set(ByVal value As Drawing.Size)
                        BaseDataObject(PdfName.D) = New PdfArray(PdfInteger.Get(value.Width), PdfInteger.Get(value.Height))
                    End Set
                End Property

                '/**
                '  <summary> Gets/Sets whether the floating window should include user Interface elements that
                '  allow a user To close it.</summary>
                '  <remarks> Meaningful only If <see cref="TitleBarVisible"/> Is True.</remarks>
                '*/
                Public Property Closeable As Boolean
                    Get
                        Return CBool(PdfBoolean.GetValue(BaseDataObject(PdfName.UC), True))
                    End Get
                    Set(ByVal value As Boolean)
                        BaseDataObject(PdfName.UC) = PdfBoolean.Get(value)
                    End Set
                End Property

                '/**
                '  <summary> Gets/Sets whether the floating window should have a title bar.</summary>
                '*/
                Public Property TitleBarVisible As Boolean
                    Get
                        Return CBool(PdfBoolean.GetValue(BaseDataObject(PdfName.T), True))
                    End Get
                    Set(ByVal value As Boolean)
                        BaseDataObject(PdfName.T) = PdfBoolean.Get(value)
                    End Set
                End Property

                'TODO: TT entry!
            End Class

            Public Enum WindowTypeEnum As Integer
                '/**
                '  <summary> A floating window.</summary>
                '*
                Floating
                '/**
                '  <summary> A full-screen window that obscures all other windows.</summary>
                '*/
                FullScreen
                '/**
                '  <summary> A hidden window.</summary>
                '*/
                Hidden
                '/**
                '  <summary> The rectangle occupied by the {@link Screen screen annotation} associated With
                '  the media rendition.</summary>
                '*/
                Annotation
            End Enum


            Friend Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub
            '/**
            '  <summary> Gets/Sets the background color For the rectangle In which the media Is being played.
            '  </summary>
            '  <remarks> This color Is used If the media Object does Not entirely cover the rectangle Or If
            '  it has transparent sections.</remarks>
            '*/
            Public Property BackgroundColoras As DeviceRGBColor
                Get
                    Return DeviceRGBColor.Get(CType(BaseDataObject(PdfName.B), PdfArray))
                End Get
                Set(ByVal value As DeviceRGBColor)
                    BaseDataObject(PdfName.B) = PdfObjectWrapper.GetBaseObject(value)
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the opacity Of the background color.</summary>
            '  <returns> A number In the range 0 To 1, where 0 means full transparency And 1 full opacity.
            '  </returns>
            '*/
            Public Property BackgroundOpacity As Double
                Get
                    Return CDbl(PdfReal.GetValue(BaseDataObject(PdfName.O), 1D))
                End Get
                Set(ByVal value As Double)
                    If (value < 0) Then
                        value = 0
                    ElseIf (value > 1) Then
                        value = 1
                    End If
                    BaseDataObject(PdfName.O) = PdfReal.Get(value)
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the options used In displaying floating windows.</summary>
            '*/
            Public Property FloatingWindowParameters As FloatingWindowParametersObject
                Get
                    Return New FloatingWindowParametersObject(BaseDataObject.Get(Of PdfDictionary)(PdfName.F))
                End Get
                Set(ByVal value As FloatingWindowParametersObject)
                    BaseDataObject(PdfName.F) = PdfObjectWrapper.GetBaseObject(value)
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets which monitor In a multi-monitor system a floating Or full-screen window
            '  should appear On.</summary>
            '*/
            Public Property MonitorSpecifier As MonitorSpecifierEnum?
                Get
                    Return MonitorSpecifierEnumExtension.Get(CType(BaseDataObject(PdfName.M), PdfInteger))
                End Get
                Set(ByVal value As MonitorSpecifierEnum?)
                    If (value.HasValue) Then
                        BaseDataObject(PdfName.M) = value.Value.GetCode()
                    Else
                        BaseDataObject(PdfName.M) = Nothing
                    End If
                End Set
            End Property

            '/**
            '  <summary> Gets/Sets the type Of window that the media Object should play In.</summary>
            '*/
            Public Property WindowType As WindowTypeEnum?
                Get
                    Return WindowTypeEnumExtension.Get(CType(BaseDataObject(PdfName.W), PdfInteger))
                End Get
                Set(ByVal value As WindowTypeEnum?)
                    If (value.HasValue) Then
                        BaseDataObject(PdfName.W) = value.Value.GetCode()
                    Else
                        BaseDataObject(PdfName.W) = Nothing
                    End If
                End Set
            End Property


        End Class

#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal context As Document)
            MyBase.New(context, New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.MediaScreenParams}))
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub



#End Region

#Region "interface"
#Region "public"

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


    Module LocationEnumExtension 'Static 

        Private ReadOnly _codes As BiDictionary(Of MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum, PdfInteger)

        Sub New()
            _codes = New BiDictionary(Of MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum, PdfInteger)()
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.UpperLeft) = New PdfInteger(0)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.UpperCenter) = New PdfInteger(1)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.UpperRight) = New PdfInteger(2)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.CenterLeft) = New PdfInteger(3)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.Center) = New PdfInteger(4)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.CenterRight) = New PdfInteger(5)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.LowerLeft) = New PdfInteger(6)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.LowerCenter) = New PdfInteger(7)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.LowerRight) = New PdfInteger(8)
        End Sub

        Public Function [Get](ByVal code As PdfInteger) As MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum?
            If (code Is Nothing) Then Return MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum.Center

            Dim location As MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum? = _codes.GetKey(code)
            If (Not location.HasValue) Then Throw New NotSupportedException("Location unknown: " & code.ToString)

            Return location
        End Function

        <Extension>
        Public Function GetCode(ByVal location As MediaScreenParameters.Viability.FloatingWindowParametersObject.LocationEnum) As PdfInteger ''this 
            Return _codes(location)
        End Function
    End Module

    Module OffscreenBehaviorEnumExtension 'Static 

        Private ReadOnly _codes As BiDictionary(Of MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum, PdfInteger)

        Sub New()
            _codes = New BiDictionary(Of MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum, PdfInteger)()
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum.None) = New PdfInteger(0)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum.Adapt) = New PdfInteger(1)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum.NonViable) = New PdfInteger(2)
        End Sub

        Public Function [Get](ByVal code As PdfInteger) As MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum?
            If (code Is Nothing) Then Return MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum.Adapt
            Dim offscreenBehavior As MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum? = _codes.GetKey(code)
            If (Not offscreenBehavior.HasValue) Then Throw New NotSupportedException("Offscreen behavior unknown: " & code.ToString)
            Return offscreenBehavior
        End Function

        <Extension>
        Public Function GetCode(ByVal offscreenBehavior As MediaScreenParameters.Viability.FloatingWindowParametersObject.OffscreenBehaviorEnum) As PdfInteger  'this 
            Return _codes(offscreenBehavior)
        End Function

    End Module

    Module RelatedWindowEnumExtension 'Static 

        Private ReadOnly _codes As BiDictionary(Of MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum, PdfInteger)

        Sub New()
            _codes = New BiDictionary(Of MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum, PdfInteger)()
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum.Document) = New PdfInteger(0)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum.Application) = New PdfInteger(1)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum.Desktop) = New PdfInteger(2)
            _codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum.Custom) = New PdfInteger(3)
        End Sub

        Public Function [Get](ByVal code As PdfInteger) As MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum?
            If (code Is Nothing) Then Return MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum.Document
            Dim relatedWindow As MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum? = _codes.GetKey(code)
            If (Not relatedWindow.HasValue) Then Throw New NotSupportedException("Related window unknown: " & code.ToString)
            Return relatedWindow
        End Function

        <Extension>
        Public Function GetCode(ByVal relatedWindow As MediaScreenParameters.Viability.FloatingWindowParametersObject.RelatedWindowEnum) As PdfInteger  'this 
            Return _codes(relatedWindow)
        End Function
    End Module


    Module ResizeBehaviorEnumExtension 'Static 

        Private ReadOnly codes As BiDictionary(Of MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum, PdfInteger)

        Sub New()
            codes = New BiDictionary(Of MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum, PdfInteger)
            codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum.None) = New PdfInteger(0)
            codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum.AspectRatioLocked) = New PdfInteger(1)
            codes(MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum.Free) = New PdfInteger(2)
        End Sub

        Public Function [Get](ByVal code As PdfInteger) As MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum?
            If (code Is Nothing) Then Return MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum.None
            Dim resizeBehavior As MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum? = codes.GetKey(code)
            If (Not resizeBehavior.HasValue) Then Throw New NotSupportedException("Resize behavior unknown: " & code.ToString)
            Return resizeBehavior
        End Function

        <Extension>
        Public Function GetCode(ByVal resizeBehavior As MediaScreenParameters.Viability.FloatingWindowParametersObject.ResizeBehaviorEnum) As PdfInteger  'this 
            Return codes(resizeBehavior)
        End Function

    End Module

    Module WindowTypeEnumExtension 'static

        Private ReadOnly codes As BiDictionary(Of MediaScreenParameters.Viability.WindowTypeEnum, PdfInteger)

        Sub New()
            codes = New BiDictionary(Of MediaScreenParameters.Viability.WindowTypeEnum, PdfInteger)
            codes(MediaScreenParameters.Viability.WindowTypeEnum.Floating) = New PdfInteger(0)
            codes(MediaScreenParameters.Viability.WindowTypeEnum.FullScreen) = New PdfInteger(1)
            codes(MediaScreenParameters.Viability.WindowTypeEnum.Hidden) = New PdfInteger(2)
            codes(MediaScreenParameters.Viability.WindowTypeEnum.Annotation) = New PdfInteger(3)
        End Sub

        Public Function [Get](ByVal code As PdfInteger) As MediaScreenParameters.Viability.WindowTypeEnum?
            If (code Is Nothing) Then Return MediaScreenParameters.Viability.WindowTypeEnum.Annotation
            Dim windowType As MediaScreenParameters.Viability.WindowTypeEnum? = codes.GetKey(code)
            If (Not windowType.HasValue) Then Throw New NotSupportedException("Window type unknown: " & code.ToString)
            Return windowType
        End Function


        <Extension>
        Public Function GetCode(ByVal windowType As MediaScreenParameters.Viability.WindowTypeEnum) As PdfInteger  'this 
            Return codes(windowType)
        End Function
    End Module


End Namespace
