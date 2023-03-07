'/*
'  Copyright 2006 - 2012 Stefano Chizzolini. http: //www.pdfclown.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  This file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  This Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With Me
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  Me list Of conditions.
'*/

Imports DMD.org.dmdpdf
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.layers
Imports DMD.org.dmdpdf.documents.interaction.forms
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.documents.interchange.metadata
Imports DMD.org.dmdpdf.documents.interaction.viewer
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports drawing = System.Drawing
Imports System.Drawing.Printing
Imports System.Runtime.CompilerServices

Namespace DMD.org.dmdpdf.documents

    '/**
    '  <summary> PDF document [PDF:1.6:3.6.1].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Document
        Inherits PdfObjectWrapper(Of PdfDictionary)

#Region "types"
        '/**
        '  <summary> Document configuration.</summary>
        '*/
        Public NotInheritable Class ConfigurationImpl

            '      /**
            '  <summary> Version compatibility mode.</summary>
            '*/
            Public Enum CompatibilityModeEnum
                '/**
                '  <summary> Document's conformance version is ignored;
                '  any feature Is accepted without checking its compatibility.</summary>
                '*/
                Passthrough
                '/**
                '  <summary> Document 's conformance version is automatically updated
                '  to support used features.</summary>
                '*/
                Loose
                '/**
                '  <summary> Document's conformance version is mandatory;
                '  any unsupported feature Is forbidden And causes an exception
                '  to be thrown in case of attempted use.</summary>
                '*/
                Strict
            End Enum

            '/**
            '  <summary> Cross-reference mode [PDF:1.6:3.4].</summary>
            '*/
            Public Enum XRefModeEnum
                '/**
                '  <summary> Cross-reference table [PDF:1.6:3.4.3].</summary>
                '*/
                <PDF(VersionEnum.PDF10)>
                Plain
                '/**
                '  <summary> Cross-reference stream [PDF:1.6:3.4.7].</summary>
                '*/
                <PDF(VersionEnum.PDF15)>
                Compressed
            End Enum

            Private _compatibilityMode As CompatibilityModeEnum = CompatibilityModeEnum.Loose
            Private _xrefMode As XRefModeEnum = XRefModeEnum.Plain

            Private _document As Document

            Friend Sub New(ByVal document As Document)
                Me._document = document
            End Sub

            '/**
            '  <summary> Gets the document's version compatibility mode.</summary>
            '*/
            Public Property CompatibilityMode As CompatibilityModeEnum
                Get
                    Return _compatibilityMode
                End Get
                Set(ByVal value As CompatibilityModeEnum)
                    _compatibilityMode = value
                End Set
            End Property

            '/**
            '  <summary> Gets the document associated With Me configuration.</summary>
            '*/
            Public ReadOnly Property Document As Document
                Get
                    Return _document
                End Get
            End Property

            '/**
            '  <summary> Gets the document's cross-reference mode.</summary>
            '*/
            Public Property XrefMode As XRefModeEnum
                Get
                    Return _xrefMode
                End Get
                Set(ByVal value As XRefModeEnum)
                    _xrefMode = value
                    _document.CheckCompatibility(_xrefMode)
                End Set
            End Property
        End Class

        '/**
        '  <summary> Page layout To be used When the document Is opened [PDF:1.6:3.6.1].</summary>
        '*/
        Public Enum PageLayoutEnum
            '/**
            '  <summary> Displays one page at a time.</summary>
            '*/
            SinglePage
            '/**
            '  <summary> Displays the pages In one column.</summary>
            '*/
            OneColumn
            '/**
            '  <summary> Displays the pages In two columns, With odd-numbered pages On the left.</summary>
            '*/
            TwoColumnLeft
            '/**
            '  <summary> Displays the pages In two columns, With odd-numbered pages On the right.</summary>
            '*/
            TwoColumnRight
            '/**
            '  <summary> Displays the pages two at a time, With odd-numbered pages On the left.</summary>
            '*/
            <PDF(VersionEnum.PDF15)>
            TwoPageLeft
            '/**
            '  <summary> Displays the pages two at a time, With odd-numbered pages On the right.</summary>
            '*/
            <PDF(VersionEnum.PDF15)>
            TwoPageRight
        End Enum

        '/**
        '  <summary> Page mode specifying how the document should be displayed When opened [PDF:1.6:3.6.1].
        '  </summary>
        '*/
        Public Enum PageModeEnum
            '/**
            '  <summary> Neither document outline nor thumbnail images visible.</summary>
            '*/
            Simple
            '/**
            '  <summary> Document outline visible.</summary>
            '*/
            Bookmarks
            '/**
            '  <summary> Thumbnail images visible.</summary>
            '*/
            Thumbnails
            '/**
            '  <summary> Full-screen mode, With no menu bar, window controls, Or any other window visible.
            '  </summary>
            '*/
            FullScreen
            '/**
            '  <summary>Optional content group panel visible.</summary>
            '*/
            <PDF(VersionEnum.PDF15)>
            Layers
            '/**
            '  <summary> Attachments panel visible.</summary>
            '*/
            <PDF(VersionEnum.PDF16)>
            Attachments
        End Enum

#End Region

#Region "shared"
#Region "Interface"
#Region "Public"

        Public Shared Function Resolve(Of T As PdfObjectWrapper)(ByVal baseObject As PdfDirectObject) As T
            If (GetType(Destination).IsAssignableFrom(GetType(T))) Then
                Dim tmp As Object = Destination.Wrap(baseObject) 'As T
                Try
                    Return CType(tmp, T)
                Catch ex As Exception
                    Debug.Print(ex.Message)
                    Return Nothing
                End Try
            Else
                Throw New NotSupportedException("Type '" & GetType(T).Name & "' wrapping is not supported.")
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Friend _Cache As Dictionary(Of PdfReference, Object) = New Dictionary(Of PdfReference, Object)()

        Private _configuration As ConfigurationImpl

#End Region

#Region "constructors"

        Friend Sub New(ByVal context As File)
            MyBase.New(context, New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.Catalog}))
            _configuration = New ConfigurationImpl(Me)
            ' Attach the document catalog to the file trailer!
            context.Trailer(PdfName.Root) = BaseObject
            ' Pages collection.
            Me.Pages = New Pages(Me)

            ' Default page size.
            PageSize = PageFormat.GetSize()

            ' Default resources collection.
            Resources = New Resources(Me)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject) ' Catalog.
            MyBase.New(baseObject)
            _configuration = New ConfigurationImpl(Me)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets/Sets the document's behavior in response to trigger events.</summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property Actions As DocumentActions
            Get
                Return New DocumentActions(BaseDataObject.Get(Of PdfDictionary)(PdfName.AA))
            End Get
            Set(ByVal value As DocumentActions)
                BaseDataObject(PdfName.AA) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets the article threads.</summary>
        '*/
        <PDF(VersionEnum.PDF11)>
        Public Property Articles As Articles
            Get
                Return Articles.Wrap(BaseDataObject.Get(Of PdfArray)(PdfName.Threads, False))
            End Get
            Set(ByVal value As Articles)
                BaseDataObject(PdfName.Threads) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the bookmark collection.</summary>
        '*/
        Public Property Bookmarks As Bookmarks
            Get
                Return Bookmarks.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.Outlines, False))
            End Get
            Set(ByVal value As Bookmarks)
                BaseDataObject(PdfName.Outlines) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        Public Overrides Function Clone(ByVal context As Document) As Object
            Throw New NotImplementedException()
        End Function

        '/**
        '  <summary> Gets/Sets the configuration Of Me document.</summary>
        '*/
        Public Property Configuration As ConfigurationImpl
            Get
                Return _configuration
            End Get
            Set(ByVal value As ConfigurationImpl)
                _configuration = value
            End Set
        End Property

        '/**
        '  <summary> Deletes the Object from Me document context.</summary>
        '*/
        Public Sub Exclude(ByVal obj As PdfObjectWrapper)
            If (obj.File IsNot File) Then Return
            obj.Delete()
        End Sub

        '/**
        '  <summary> Deletes the objects from Me document context.</summary>
        '*/
        Public Sub Exclude(Of T As PdfObjectWrapper)(ByVal objs As ICollection(Of T))
            For Each obj As T In objs
                Exclude(obj)
            Next
        End Sub

        '/**
        '  <summary> Gets/Sets the interactive form (AcroForm).</summary>
        '*/
        <PDF(VersionEnum.PDF12)>
        Public Property Form As Form
            Get
                Return Form.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.AcroForm))
            End Get
            Set(ByVal value As Form)
                BaseDataObject(PdfName.AcroForm) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets the document size, that Is the maximum page dimensions across the whole document.
        '  </summary>
        '  <seealso cref = "PageSize" />
        '*/
        Public Function GetSize() As drawing.SizeF
            Dim height As Single = 0, width As Single = 0
            For Each page As Page In Pages
                Dim pageSize As drawing.SizeF = page.Size
                height = Math.Max(height, pageSize.Height)
                width = Math.Max(width, pageSize.Width)
            Next
            Return New drawing.SizeF(width, height)
        End Function

        '/**
        '  <summary> Clones the Object within Me document context.</summary>
        '*/
        Public Function Include(ByVal obj As PdfObjectWrapper) As PdfObjectWrapper
            If (obj.File Is File) Then Return obj
            Return CType(obj.Clone(Me), PdfObjectWrapper)
        End Function

        '/**
        '  <summary> Clones the collection objects within Me document context.</summary>
        '*/
        Public Function Include(Of T As PdfObjectWrapper)(ByVal objs As ICollection(Of T)) As ICollection(Of T)
            Dim includedObjects As List(Of T) = New List(Of T)(objs.Count)
            For Each obj As T In objs
                includedObjects.Add(CType(Include(obj), T))
            Next
            Return CType(includedObjects, ICollection(Of T))
        End Function

        '/**
        '  <summary> Gets/Sets common document metadata.</summary>
        '*/
        Public Property Information As Information
            Get
                Return Information.Wrap(File.Trailer.Get(Of PdfDictionary)(PdfName.Info, False))
            End Get
            Set(ByVal value As Information)
                File.Trailer(PdfName.Info) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the Optional content properties.</summary>
        '*/
        <PDF(VersionEnum.PDF15)>
        Public Property Layer As LayerDefinition
            Get
                Return LayerDefinition.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.OCProperties))
            End Get
            Set(ByVal value As LayerDefinition)
                CheckCompatibility("Layer")
                BaseDataObject(PdfName.OCProperties) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the name dictionary.</summary>
        '*/
        <PDF(VersionEnum.PDF12)>
        Public Property Names As Names
            Get
                Return New Names(BaseDataObject.Get(Of PdfDictionary)(PdfName.Names))
            End Get
            Set(ByVal value As Names)
                BaseDataObject(PdfName.Names) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the page label ranges.</summary>
        '*/
        <PDF(VersionEnum.PDF13)>
        Public Property PageLabels As PageLabels
            Get
                Return New PageLabels(BaseDataObject.Get(Of PdfDictionary)(PdfName.PageLabels))
            End Get
            Set(ByVal value As PageLabels)
                CheckCompatibility("PageLabels")
                BaseDataObject(PdfName.PageLabels) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the page layout To be used When the document Is opened.</summary>
        '*/
        Public Property PageLayout As PageLayoutEnum
            Get
                Return PageLayoutEnumExtension.Get(CType(BaseDataObject(PdfName.PageLayout), PdfName))
            End Get
            Set(ByVal value As PageLayoutEnum)
                BaseDataObject(PdfName.PageLayout) = value.GetName()
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the page mode, that Is how the document should be displayed When Is opened.</summary>
        '*/
        Public Property PageMode As PageModeEnum
            Get
                Return PageModeEnumExtension.Get(CType(BaseDataObject(PdfName.PageMode), PdfName))
            End Get
            Set(ByVal value As PageModeEnum)
                BaseDataObject(PdfName.PageMode) = value.GetName()
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the page collection.</summary>
        '*/
        Public Property Pages As Pages
            Get
                Return New Pages(BaseDataObject(PdfName.Pages))
            End Get
            Set(ByVal value As Pages)
                BaseDataObject(PdfName.Pages) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the Default page size [PDF:1.6:3.6.2].</summary>
        '*/
        Public Property PageSize As drawing.Size?
            Get
                Dim mediaBox As PdfArray = Me.MediaBox
                If (mediaBox IsNot Nothing) Then
                    Return New drawing.Size(
                    CInt(CType(mediaBox(2), IPdfNumber).RawValue),
                    CInt(CType(mediaBox(3), IPdfNumber).RawValue)
                    )
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As drawing.Size?)
                Dim mediaBox As PdfArray = Me.MediaBox
                If (mediaBox Is Nothing) Then
                    ' Create default media box!
                    mediaBox = New Rectangle(0, 0, 0, 0).BaseDataObject
                    ' Assign the media box to the document!
                    CType(BaseDataObject.Resolve(PdfName.Pages), PdfDictionary)(PdfName.MediaBox) = mediaBox
                End If
                mediaBox(2) = PdfReal.Get(value.Value.Width)
                mediaBox(3) = PdfReal.Get(value.Value.Height)
            End Set
        End Property

        '/**
        '  <summary> Forces a named base Object To be expressed As its corresponding high-level
        '  representation.</summary>
        '*/
        Public Function ResolveName(Of T As PdfObjectWrapper)(ByVal namedBaseObject As PdfDirectObject) As T
            If (TypeOf (namedBaseObject) Is PdfString) Then '// Named Object.
                Return CType(Names.Get(GetType(T), CType(namedBaseObject, PdfString)), T)
            Else ' Explicit Object.
                Return Resolve(Of T)(namedBaseObject)
            End If
        End Function

        '/**
        '  <summary> Gets/Sets the Default resource collection [PDF:1.6:3.6.2].</summary>
        '  <remarks> The Default resource collection Is used As last resort by every page that doesn't
        '  reference one explicitly (And doesn't reference an intermediate one implicitly).</remarks>
        '*/
        Public Property Resources As Resources
            Get
                Return Resources.Wrap(CType(BaseDataObject.Resolve(PdfName.Pages), PdfDictionary).Get(Of PdfDictionary)(PdfName.Resources))
            End Get
            Set(ByVal value As Resources)
                CType(BaseDataObject.Resolve(PdfName.Pages), PdfDictionary)(PdfName.Resources) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

        '/**
        '  <summary> Gets/Sets the version Of the PDF specification Me document conforms To.</summary>
        '*/
        <PDF(VersionEnum.PDF14)>
        Public Property Version As Version
            Get
                '/*
                '  NOTE: If the header specifies a later version, Or If Me entry Is absent, the document
                '  conforms to the version specified in the header.
                '*/
                Dim fileVersion As Version = File.Version

                Dim versionObject As PdfName = CType(BaseDataObject(PdfName.Version), PdfName)
                If (versionObject Is Nothing) Then Return fileVersion

                Dim _version As Version = Version.Get(versionObject)
                If (File.Reader Is Nothing) Then Return _version

                If (_version.CompareTo(fileVersion) > 0) Then
                    Return _version
                Else
                    Return fileVersion
                End If
            End Get
            Set(ByVal value As Version)
                BaseDataObject(PdfName.Version) = PdfName.Get(value)
            End Set
        End Property

        '/**
        '  <summary> Gets the way the document Is To be presented.</summary>
        '*/
        Public Property ViewerPreferences As ViewerPreferences
            Get
                Return ViewerPreferences.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.ViewerPreferences))
            End Get
            Set(ByVal value As ViewerPreferences)
                BaseDataObject(PdfName.ViewerPreferences) = PdfObjectWrapper.GetBaseObject(value)
            End Set
        End Property

#End Region

#Region "private"
        '/**
        '  <summary> Gets the Default media box.</summary>
        '*/
        Private ReadOnly Property MediaBox As PdfArray
            Get
                '/*
                '  NOTE: Document media box MUST be associated With the page-tree root node In order To be
                '  inheritable by all the pages.
                '*/
                Return CType(CType(BaseDataObject.Resolve(PdfName.Pages), PdfDictionary).Resolve(PdfName.MediaBox), PdfArray)
            End Get
        End Property

#End Region
#End Region
#End Region
    End Class

    Friend Module PageLayoutEnumExtension

        Private ReadOnly _codes As BiDictionary(Of Document.PageLayoutEnum, PdfName)

        Sub New()
            _codes = New BiDictionary(Of Document.PageLayoutEnum, PdfName)()
            _codes(Document.PageLayoutEnum.SinglePage) = PdfName.SinglePage
            _codes(Document.PageLayoutEnum.OneColumn) = PdfName.OneColumn
            _codes(Document.PageLayoutEnum.TwoColumnLeft) = PdfName.TwoColumnLeft
            _codes(Document.PageLayoutEnum.TwoColumnRight) = PdfName.TwoColumnRight
            _codes(Document.PageLayoutEnum.TwoPageLeft) = PdfName.TwoPageLeft
            _codes(Document.PageLayoutEnum.TwoPageRight) = PdfName.TwoPageRight
        End Sub

        Public Function [Get](ByVal name As PdfName) As Document.PageLayoutEnum
            If (name Is Nothing) Then Return Document.PageLayoutEnum.SinglePage

            Dim pageLayout As Document.PageLayoutEnum? = _codes.GetKey(name)
            If (Not pageLayout.HasValue) Then Throw New NotSupportedException("Page layout unknown: " & name.ToString)
            Return pageLayout.Value
        End Function

        <Extension>
        Public Function GetName(ByVal pageLayout As Document.PageLayoutEnum) As PdfName
            Return _codes(pageLayout)
        End Function

    End Module

    Friend Module PageModeEnumExtension

        Private ReadOnly _codes As BiDictionary(Of Document.PageModeEnum, PdfName)

        Sub New()
            _codes = New BiDictionary(Of Document.PageModeEnum, PdfName)()
            _codes(Document.PageModeEnum.Simple) = PdfName.UseNone
            _codes(Document.PageModeEnum.Bookmarks) = PdfName.UseOutlines
            _codes(Document.PageModeEnum.Thumbnails) = PdfName.UseThumbs
            _codes(Document.PageModeEnum.FullScreen) = PdfName.FullScreen
            _codes(Document.PageModeEnum.Layers) = PdfName.UseOC
            _codes(Document.PageModeEnum.Attachments) = PdfName.UseAttachments
        End Sub

        Public Function [Get](ByVal name As PdfName) As Document.PageModeEnum
            If (name Is Nothing) Then Return Document.PageModeEnum.Simple
            Dim pageMode As Document.PageModeEnum? = _codes.GetKey(name)
            If (Not pageMode.HasValue) Then Throw New NotSupportedException("Page mode unknown: " & name.ToString())
            Return pageMode.Value
        End Function

        <Extension>
        Public Function GetName(ByVal pageMode As Document.PageModeEnum) As PdfName
            Return _codes(pageMode)
        End Function
    End Module

End Namespace

''/*
''  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

''  Contributors:
''    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

''  This file should be part of the source code distribution of "PDF Clown library" (the
''  Program): see the accompanying README files for more info.

''  This Program is free software; you can redistribute it and/or modify it under the terms
''  of the GNU Lesser General Public License as published by the Free Software Foundation;
''  either version 3 of the License, or (at your option) any later version.

''  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
''  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
''  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

''  You should have received a copy of the GNU Lesser General Public License along with Me
''  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

''  Redistribution and use, with or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license and disclaimer, along with
''  Me list of conditions.
''*/

'Imports DMD.org.dmdpdf
'Imports DMD.org.dmdpdf.documents.contents
'Imports DMD.org.dmdpdf.documents.contents.layers
'Imports DMD.org.dmdpdf.documents.interaction.forms
'Imports DMD.org.dmdpdf.documents.interaction.navigation.document
'Imports DMD.org.dmdpdf.documents.interchange.metadata
'Imports DMD.org.dmdpdf.documents.interaction.viewer
'Imports DMD.org.dmdpdf.files
'Imports DMD.org.dmdpdf.objects
'Imports DMD.org.dmdpdf.tokens
'Imports DMD.org.dmdpdf.util

'Imports System
'Imports System.Collections.Generic
'Imports System.Drawing
'Imports System.Drawing.Printing
'Imports System.Runtime.CompilerServices

'Namespace DMD.org.dmdpdf.documents

'    '/**
'    '  <summary>PDF document [PDF:1.6::3.6.1].</summary>
'    '*/
'    <PDF(VersionEnum.PDF10)>
'    Public NotInheritable Class Document
'        Inherits PdfObjectWrapper(Of PdfDictionary)

'#Region "types"
'        '/**
'        '  <summary>Document configuration.</summary>
'        '*/
'        Public NotInheritable Class ConfigurationImpl

'            '      /**
'            '  <summary>Version compatibility mode.</summary>
'            '*/
'            Public Enum CompatibilityModeEnum As Integer

'                '/**
'                '  <summary>Document's conformance version is ignored;
'                '  any feature is accepted without checking its compatibility.</summary>
'                '*/
'                Passthrough
'                '/**
'                '  <summary>Document's conformance version is automatically updated
'                '  to support used features.</summary>
'                '*/
'                Loose
'                '/**
'                '  <summary>Document's conformance version is mandatory;
'                '  any unsupported feature is forbidden and causes an exception
'                '  to be thrown in case of attempted use.</summary>
'                '*/
'                Strict
'            End Enum

'            '/**
'            '  <summary>Cross-reference mode [PDF:1.6:3.4].</summary>
'            '*/
'            Public Enum XRefModeEnum As Integer

'                '    /**
'                '  <summary>Cross-reference table [PDF:1.6:3.4.3].</summary>
'                '*/
'                <PDF(VersionEnum.PDF10)>
'                Plain
'                '/**
'                '  <summary>Cross-reference stream [PDF:1.6:3.4.7].</summary>
'                '*/
'                <PDF(VersionEnum.PDF15)>
'                Compressed
'            End Enum

'            Private _compatibilityMode As CompatibilityModeEnum = CompatibilityModeEnum.Loose
'            Private _xrefMode As XRefModeEnum = XRefModeEnum.Plain

'            Private _document As Document

'            Friend Sub New(ByVal document As Document)
'                Me._document = document
'            End Sub

'            '/**
'            '  <summary>Gets the document's version compatibility mode.</summary>
'            '*/
'            Public Property CompatibilityMode As CompatibilityModeEnum
'                Get
'                    Return Me._compatibilityMode
'                End Get
'                Set(ByVal value As CompatibilityModeEnum)
'                    Me._compatibilityMode = value
'                End Set
'            End Property

'            '/**
'            '  <summary>Gets the document associated with Me configuration.</summary>
'            '*/
'            Public ReadOnly Property Document As Document
'                Get
'                    Return Me._document
'                End Get
'            End Property

'            '/**
'            '  <summary>Gets the document's cross-reference mode.</summary>
'            '*/
'            Public Property XrefMode As XRefModeEnum
'                Get
'                    Return Me._xrefMode
'                End Get
'                Set(ByVal value As XRefModeEnum)
'                    Me._xrefMode = value
'                    Me._document.CheckCompatibility(Me._xrefMode)
'                End Set
'            End Property

'        End Class

'        '/**
'        '  <summary>Page layout to be used when the document is opened [PDF:1.6:3.6.1].</summary>
'        '*/
'        Public Enum PageLayoutEnum As Integer
'            '/**
'            '  <summary>Displays one page at a time.</summary>
'            '*/
'            SinglePage
'            '/**
'            '  <summary>Displays the pages in one column.</summary>
'            '*/
'            OneColumn
'            '/**
'            '  <summary>Displays the pages in two columns, with odd-numbered pages on the left.</summary>
'            '*/
'            TwoColumnLeft
'            '/**
'            '  <summary>Displays the pages in two columns, with odd-numbered pages on the right.</summary>
'            '*/
'            TwoColumnRight
'            '/**
'            '  <summary>Displays the pages two at a time, with odd-numbered pages on the left.</summary>
'            '*/
'            <PDF(VersionEnum.PDF15)>
'            TwoPageLeft
'            '/**
'            '  <summary>Displays the pages two at a time, with odd-numbered pages on the right.</summary>
'            '*/
'            <PDF(VersionEnum.PDF15)>
'            TwoPageRight
'        End Enum

'        '/**
'        '  <summary>Page mode specifying how the document should be displayed when opened [PDF:1.6:3.6.1].
'        '  </summary>
'        '*/
'        Public Enum PageModeEnum As Integer
'            '/**
'            '  <summary>Neither document outline nor thumbnail images visible.</summary>
'            '*/
'            Simple
'            '/**
'            '  <summary>Document outline visible.</summary>
'            '*/
'            Bookmarks
'            '/**
'            '  <summary>Thumbnail images visible.</summary>
'            '*/
'            Thumbnails
'            '/**
'            '  <summary>Full-screen mode, with no menu bar, window controls, or any other window visible.
'            '  </summary>
'            '*/
'            FullScreen
'            '/**
'            '  <summary>Optional content group panel visible.</summary>
'            '*/
'            <PDF(VersionEnum.PDF15)>
'            Layers
'            '/**
'            '  <summary>Attachments panel visible.</summary>
'            '*/
'            <PDF(VersionEnum.PDF16)>
'            Attachments
'        End Enum
'#End Region

'#Region "shared"
'#Region "Interface"
'#Region "Public"

'        Public Shared Function Resolve(Of T As PdfObjectWrapper)(ByVal baseObject As PdfDirectObject) As T
'            If (GetType(Destination).IsAssignableFrom(GetType(T))) Then
'                Dim ret As Object = Destination.Wrap(baseObject)
'                Return CType(ret, T)
'            Else
'                Throw New NotSupportedException("Type '" & GetType(T).Name & "' wrapping is not supported.")
'            End If
'        End Function

'#End Region
'#End Region
'#End Region

'#Region "dynamic"
'#Region "fields"

'        Friend _Cache As Dictionary(Of PdfReference, Object) = New Dictionary(Of PdfReference, Object)()

'        Private _configuration As ConfigurationImpl

'#End Region

'#Region "constructors"

'        Friend Sub New(ByVal context As File)
'            MyBase.New(context, New PdfDictionary(New PdfName() {PdfName.Type}, New PdfDirectObject() {PdfName.Catalog}))
'            Me._configuration = New ConfigurationImpl(Me)
'            'Attach the document catalog to the file trailer!
'            context.Trailer(PdfName.Root) = BaseObject
'            ' Pages collection.
'            Me.Pages = New Pages(Me)
'            ' Default page size.
'            PageSize = PageFormat.GetSize()
'            ' Default resources collection.
'            Resources = New Resources(Me)
'        End Sub

'        ''' <summary>
'        ''' 
'        ''' </summary>
'        ''' <param name="baseObject">Catalog</param>
'        Friend Sub New(ByVal baseObject As PdfDirectObject)
'            MyBase.New(baseObject)
'            Me._configuration = New ConfigurationImpl(Me)
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"
'        '/**
'        '  <summary>Gets/Sets the document's behavior in response to trigger events.</summary>
'        '*/
'        <PDF(VersionEnum.PDF14)>
'        Public Property Actions As DocumentActions
'            Get
'                Return New DocumentActions(BaseDataObject.Get(Of PdfDictionary)(PdfName.AA))
'            End Get
'            Set(ByVal value As DocumentActions)
'                BaseDataObject(PdfName.AA) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property


'        '/**
'        '  <summary>Gets the article threads.</summary>
'        '*/
'        <PDF(VersionEnum.PDF11)>
'        Public Property Articles As Articles
'            Get
'                Return Articles.Wrap(BaseDataObject.Get(Of PdfArray)(PdfName.Threads, False))
'            End Get
'            Set(ByVal value As Articles)
'                BaseDataObject(PdfName.Threads) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property


'        '/**
'        '  <summary>Gets/Sets the bookmark collection.</summary>
'        '*/
'        Public Property Bookmarks As Bookmarks
'            Get
'                Return Bookmarks.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.Outlines, False))
'            End Get
'            Set(ByVal value As Bookmarks)
'                BaseDataObject(PdfName.Outlines) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'        Public Overrides Function Clone(ByVal context As Document) As Object
'            Throw New NotImplementedException()
'        End Function

'        '/**
'        '  <summary>Gets/Sets the configuration Of Me document.</summary>
'        '*/
'        Public Property Configuration As ConfigurationImpl
'            Get
'                Return Me._configuration
'            End Get
'            Set(ByVal value As ConfigurationImpl)
'                Me._configuration = value
'            End Set
'        End Property

'        '/**
'        '  <summary>Deletes the Object from Me document context.</summary>
'        '*/
'        Public Sub Exclude(ByVal obj As PdfObjectWrapper)
'            If (obj.File IsNot Nothing) Then Return
'            obj.Delete()
'        End Sub

'        '/**
'        '  <summary>Deletes the objects from Me document context.</summary>
'        '*/
'        Public Sub Exclude(Of T As PdfObjectWrapper)(ByVal objs As ICollection(Of T))
'            For Each obj As T In objs
'                Exclude(obj)
'            Next
'        End Sub

'        '/**
'        '  <summary>Gets/Sets the interactive form (AcroForm).</summary>
'        '*/
'        <PDF(VersionEnum.PDF12)>
'        Public Property Form As Form
'            Get
'                Return Form.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.AcroForm))
'            End Get
'            Set(ByVal value As Form)
'                BaseDataObject(PdfName.AcroForm) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets the document size, that is the maximum page dimensions across the whole document.
'        '  </summary>
'        '                 <seealso cref="PageSize"/>
'        '*/
'        Public Function GetSize() As System.Drawing.SizeF
'            Dim height As Single = 0, width As Single = 0
'            For Each page As Page In Me.Pages
'                Dim pageSize As System.Drawing.SizeF = page.Size
'                height = Math.Max(height, pageSize.Height)
'                width = Math.Max(width, pageSize.Width)
'            Next
'            Return New System.Drawing.SizeF(width, height)
'        End Function

'        '/**
'        '  <summary>Clones the object within Me document context.</summary>
'        '*/
'        Public Function Include(ByVal obj As PdfObjectWrapper) As PdfObjectWrapper
'            If (obj.File Is File) Then Return obj
'            Return CType(obj.Clone(Me), PdfObjectWrapper)
'        End Function

'        '/**
'        '  <summary>Clones the collection objects within Me document context.</summary>
'        '*/
'        Public Function Include(Of T As PdfObjectWrapper)(ByVal objs As ICollection(Of T)) As ICollection(Of T)
'            Dim includedObjects As List(Of T) = New List(Of T)(objs.Count)
'            For Each obj As T In objs
'                includedObjects.Add(CType(Include(obj), T))
'            Next
'            Return CType(includedObjects, ICollection(Of T))
'        End Function

'        '/**
'        '  <summary>Gets/Sets common document metadata.</summary>
'        '*/
'        Public Property Information As Information
'            Get
'                Return Information.Wrap(File.Trailer.Get(Of PdfDictionary)(PdfName.Info, False))
'            End Get
'            Set(ByVal value As Information)
'                File.Trailer(PdfName.Info) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the optional content properties.</summary>
'        '*/
'        <PDF(VersionEnum.PDF15)>
'        Public Property Layer As LayerDefinition
'            Get
'                Return LayerDefinition.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.OCProperties))
'            End Get
'            Set(ByVal value As LayerDefinition)
'                CheckCompatibility("Layer")
'                BaseDataObject(PdfName.OCProperties) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the name dictionary.</summary>
'        '*/
'        <PDF(VersionEnum.PDF12)>
'        Public Property Names As Names
'            Get
'                Return New Names(BaseDataObject.Get(Of PdfDictionary)(PdfName.Names))
'            End Get
'            Set(ByVal value As Names)
'                BaseDataObject(PdfName.Names) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the page label ranges.</summary>
'        '*/
'        <PDF(VersionEnum.PDF13)>
'        Public Property PageLabels As PageLabels
'            Get
'                Return New PageLabels(BaseDataObject.Get(Of PdfDictionary)(PdfName.PageLabels))
'            End Get
'            Set(ByVal value As PageLabels)
'                CheckCompatibility("PageLabels")
'                BaseDataObject(PdfName.PageLabels) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the page layout to be used when the document is opened.</summary>
'        '*/
'        Public Property PageLayout As PageLayoutEnum
'            Get
'                Return PageLayoutEnumExtension.Get(CType(BaseDataObject(PdfName.PageLayout), PdfName))
'            End Get
'            Set(ByVal value As PageLayoutEnum)
'                BaseDataObject(PdfName.PageLayout) = value.GetName()
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the page mode, that is how the document should be displayed when is opened.</summary>
'        '*/
'        Public Property PageMode As PageModeEnum
'            Get
'                Return PageModeEnumExtension.Get(CType(BaseDataObject(PdfName.PageMode), PdfName))
'            End Get
'            Set(ByVal value As PageModeEnum)
'                BaseDataObject(PdfName.PageMode) = value.GetName()
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the page collection.</summary>
'        '*/
'        Public Property Pages As Pages
'            Get
'                Return New Pages(BaseDataObject(PdfName.Pages))
'            End Get
'            Set(ByVal value As Pages)
'                BaseDataObject(PdfName.Pages) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets/Sets the default page size [PDF:1.6:3.6.2].</summary>
'        '*/
'        Public Property PageSize As System.Drawing.Size?
'            Get
'                Dim mediaBox As PdfArray = Me.MediaBox
'                If (mediaBox IsNot Nothing) Then
'                    Return New System.Drawing.Size(CInt(CType(mediaBox(2), IPdfNumber).RawValue), CInt(CType(mediaBox(3), IPdfNumber).RawValue))
'                Else
'                    Return Nothing
'                End If
'            End Get
'            Set(ByVal value As System.Drawing.Size?)
'                Dim mediaBox As PdfArray = Me.MediaBox
'                If (mediaBox Is Nothing) Then
'                    ' Create default media box!
'                    mediaBox = New dmdpdf.objects.Rectangle(0, 0, 0, 0).BaseDataObject
'                    ' Assign the media box to the document!
'                    CType(BaseDataObject.Resolve(PdfName.Pages), PdfDictionary)(PdfName.MediaBox) = mediaBox
'                End If
'                mediaBox(2) = PdfReal.Get(value.Value.Width)
'                mediaBox(3) = PdfReal.Get(value.Value.Height)
'            End Set
'        End Property

'        '/**
'        '  <summary>Forces a named base object to be expressed as its corresponding high-level
'        '  representation.</summary>
'        '*/
'        Public Function ResolveName(Of T As PdfObjectWrapper)(ByVal namedBaseObject As PdfDirectObject) As T
'            If (TypeOf (namedBaseObject) Is PdfString) Then ' Named object.
'                Return CType(Names.Get(GetType(T), CType(namedBaseObject, PdfString)), T)
'            Else ' Explicit object.
'                Return Resolve(Of T)(namedBaseObject)
'            End If
'        End Function

'        '/**
'        '  <summary>Gets/Sets the default resource collection [PDF:1.6:3.6.2].</summary>
'        '                                                 <remarks>The default resource collection is used as last resort by every page that doesn't
'        '  reference one explicitly (and doesn't reference an intermediate one implicitly).</remarks>
'        '*/
'        Public Property Resources As Resources
'            Get
'                Return Resources.Wrap(CType(BaseDataObject.Resolve(PdfName.Pages), PdfDictionary).Get(Of PdfDictionary)(PdfName.Resources))
'            End Get
'            Set(ByVal value As Resources)
'                CType(BaseDataObject.Resolve(PdfName.Pages), PdfDictionary)(PdfName.Resources) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property


'        '/**
'        '  <summary>Gets/Sets the version of the PDF specification Me document conforms to.</summary>
'        '*/
'        <PDF(VersionEnum.PDF14)>
'        Public Property Version As Version
'            Get
'                '/*
'                '  NOTE: If the header specifies a later version, or if Me entry is absent, the document
'                '  conforms to the version specified in the header.
'                '*/
'                Dim fileVersion As Version = File.Version

'                Dim versionObject As PdfName = CType(BaseDataObject(PdfName.Version), PdfName)
'                If (versionObject Is Nothing) Then Return fileVersion

'                Dim _version As Version = Version.Get(versionObject)
'                If (File.Reader Is Nothing) Then Return _version

'                If (_version.CompareTo(fileVersion) > 0) Then
'                    Return _version
'                Else
'                    Return fileVersion
'                End If
'            End Get
'            Set(ByVal value As Version)
'                BaseDataObject(PdfName.Version) = PdfName.Get(value)
'            End Set
'        End Property

'        '/**
'        '  <summary>Gets the way the document is to be presented.</summary>
'        '*/
'        Public Property ViewerPreferences As ViewerPreferences
'            Get
'                Return ViewerPreferences.Wrap(BaseDataObject.Get(Of PdfDictionary)(PdfName.ViewerPreferences))
'            End Get
'            Set(ByVal value As ViewerPreferences)
'                BaseDataObject(PdfName.ViewerPreferences) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'#End Region

'#Region "private"
'        '/**
'        '  <summary>Gets the default media box.</summary>
'        '*/
'        Private ReadOnly Property MediaBox As PdfArray
'            Get
'                '/*
'                '  NOTE: Document media box MUST be associated with the page-tree root node in order to be
'                '  inheritable by all the pages.
'                '*/
'                Return CType(CType(BaseDataObject.Resolve(PdfName.Pages), PdfDictionary).Resolve(PdfName.MediaBox), PdfArray)
'            End Get
'        End Property

'#End Region
'#End Region
'#End Region
'    End Class


'    Module PageLayoutEnumExtension 'shared 

'        Private ReadOnly _codes As BiDictionary(Of Document.PageLayoutEnum, PdfName)

'        Sub New()
'            _codes = New BiDictionary(Of Document.PageLayoutEnum, PdfName)()
'            _codes(Document.PageLayoutEnum.SinglePage) = PdfName.SinglePage
'            _codes(Document.PageLayoutEnum.OneColumn) = PdfName.OneColumn
'            _codes(Document.PageLayoutEnum.TwoColumnLeft) = PdfName.TwoColumnLeft
'            _codes(Document.PageLayoutEnum.TwoColumnRight) = PdfName.TwoColumnRight
'            _codes(Document.PageLayoutEnum.TwoPageLeft) = PdfName.TwoPageLeft
'            _codes(Document.PageLayoutEnum.TwoPageRight) = PdfName.TwoPageRight
'        End Sub

'        Public Function [Get](ByVal name As PdfName) As Document.PageLayoutEnum
'            If (name Is Nothing) Then Return Document.PageLayoutEnum.SinglePage
'            Dim pageLayout As Document.PageLayoutEnum? = _codes.GetKey(name)
'            If (Not pageLayout.HasValue) Then Throw New NotSupportedException("Page layout unknown: " & name.ToString)
'            Return pageLayout.Value
'        End Function

'        <Extension>
'        Public Function GetName(ByVal pageLayout As Document.PageLayoutEnum) As PdfName  'Me
'            Return _codes(pageLayout)
'        End Function
'    End Module


'    Module PageModeEnumExtension 'shared 

'        Private ReadOnly _codes As BiDictionary(Of Document.PageModeEnum, PdfName)

'        Sub New()
'            _codes = New BiDictionary(Of Document.PageModeEnum, PdfName)()
'            _codes(Document.PageModeEnum.Simple) = PdfName.UseNone
'            _codes(Document.PageModeEnum.Bookmarks) = PdfName.UseOutlines
'            _codes(Document.PageModeEnum.Thumbnails) = PdfName.UseThumbs
'            _codes(Document.PageModeEnum.FullScreen) = PdfName.FullScreen
'            _codes(Document.PageModeEnum.Layers) = PdfName.UseOC
'            _codes(Document.PageModeEnum.Attachments) = PdfName.UseAttachments
'        End Sub

'        Function [Get](ByVal name As PdfName) As Document.PageModeEnum
'            If (name Is Nothing) Then Return Document.PageModeEnum.Simple
'            Dim pageMode As Document.PageModeEnum? = _codes.GetKey(name)
'            If (Not pageMode.HasValue) Then Throw New NotSupportedException("Page mode unknown: " & name.ToString)
'            Return pageMode.Value
'        End Function

'        <Extension>
'        Function GetName(ByVal pageMode As Document.PageModeEnum) As PdfName  'Me
'            Return _codes(pageMode)
'        End Function

'    End Module

'End Namespace

