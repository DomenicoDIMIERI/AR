'/*
'  Copyright 2008 - 2012 Stefano Chizzolini. http: //www.pdfclown.org

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

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.composition
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.tools

    '/**
    '  <summary> Tool For page management.</summary>
    '*/
    Public NotInheritable Class PageManager

        '    /*
        '  NOTE: As you can read on the PDF Clown's User Guide, referential operations on high-level object such as pages
        '  can be done at two levels
        '    1. shallow, involving page references but Not their data within the document;
        '    2. deep, involving page data within the document.
        '  This means that, For example, If you remove a page reference (shallow level) from the pages collection,
        '  the data Of that page (deep level) are still within the document!
        '*/

#Region "shared"
#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets the data size Of the specified page expressed In bytes.</summary>
        '  <param name = "page" > page whose data size has To be calculated.</param>
        '*/
        Public Shared Function GetSize(ByVal page As Page) As Long
            Return GetSize(page, New HashSet(Of PdfReference)())
        End Function

        '/**
        '  <summary> Gets the data size Of the specified page expressed In bytes.</summary>
        '  <param name = "page" > Page whose data size has To be calculated.</param>
        '  <param name = "visitedReferences" > References To data objects excluded from calculation.
        '    This set Is useful, for example, to avoid recalculating the data size of shared resources.
        '    During the operation, Me Set Is populated With references To visited data objects.</param>
        '*/
        Public Shared Function GetSize(ByVal page As Page, ByVal visitedReferences As HashSet(Of PdfReference)) As Long
            Return GetSize(page.BaseObject, visitedReferences, True)
        End Function

#End Region

#Region "private"
        '/**
        '  <summary> Gets the data size Of the specified Object expressed In bytes.</summary>
        '             <param name = "object" > Data Object whose size has To be calculated.</param>
        '             <param name = "visitedReferences" > References To data objects excluded from calculation.
        '    This set Is useful, for example, to avoid recalculating the data size of shared resources.
        '    During the operation, Me Set Is populated With references To visited data objects.</param>
        '             <param name = "isRoot" > Whether Me data Object represents the page root.</param>
        '*/
        Private Shared Function GetSize(ByVal obj As PdfDirectObject, ByVal visitedReferences As HashSet(Of PdfReference), ByVal isRoot As Boolean) As Long
            Dim dataSize As Long = 0
            '{
            Dim dataObject As PdfDataObject = PdfObject.Resolve(obj)

            ' 1. Evaluating the current object...
            If (TypeOf (obj) Is PdfReference) Then
                '{
                Dim reference As PdfReference = CType(obj, PdfReference)
                If (visitedReferences.Contains(reference)) Then
                    Return 0 ' Avoids circular references.
                End If

                If (TypeOf (dataObject) Is PdfDictionary AndAlso
                    PdfName.Page.Equals((CType(dataObject, PdfDictionary))(PdfName.Type)) AndAlso
                    Not isRoot) Then
                    Return 0 ' Avoids references To other pages.
                End If

                visitedReferences.Add(reference)

                ' Calculate the data size of the current object!
                Dim buffer As IOutputStream = New Buffer()
                reference.IndirectObject.WriteTo(buffer, reference.File)
                dataSize += buffer.Length
            End If

            ' 2. Evaluating the current object's children...
            Dim values As ICollection(Of PdfDirectObject) = Nothing
            '{
            If (TypeOf (dataObject) Is PdfStream) Then
                dataObject = CType(dataObject, PdfStream).Header
            End If

            If (TypeOf (dataObject) Is PdfDictionary) Then
                values = CType(dataObject, PdfDictionary).Values
            ElseIf (TypeOf (dataObject) Is PdfArray) Then
                values = CType(dataObject, PdfArray)
            End If
            '}

            If (values IsNot Nothing) Then
                ' Calculate the data size of the current object's children!
                For Each value As PdfDirectObject In values
                    dataSize += GetSize(value, visitedReferences, False)
                Next
            End If
            '}
            Return dataSize
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _document As Document
        Private _pages As Pages

#End Region

#Region "constructors"

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal document As Document)
            Me.Document = document
        End Sub

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary> Appends a document To the End Of the document.</summary>
        '                     <param name = "document" > Document To be added.</param>
        '*/
        Public Sub Add(ByVal document As Document)
            Add(CType(document.Pages, ICollection(Of Page)))
        End Sub

        '/**
        '  <summary> Inserts a document at the specified position In the document.</summary>
        '                         <param name = "index" > Position at which the document has To be inserted.</param>
        '                         <param name = "document" > Document To be inserted.</param>
        '*/
        Public Sub Add(ByVal index As Integer, ByVal document As Document)
            Add(index, CType(document.Pages, ICollection(Of Page)))
        End Sub

        '/**
        '  <summary> Appends a collection Of pages To the End Of the document.</summary>
        '                             <param name = "pages" > Pages To be added.</param>
        '*/
        Public Sub Add(ByVal pages As ICollection(Of Page))
            ' Add the source pages to the document (deep level)!
            Dim importedPages As ICollection(Of Page) = CType(_document.Include(pages), ICollection(Of Page)) ' NOTE: Alien Pages MUST be contextualized (i.e. imported).

            ' Add the imported pages to the pages collection (shallow level)!
            Me._pages.AddAll(importedPages)
        End Sub

        '/**
        '  <summary> Inserts a collection Of pages at the specified position In the document.</summary>
        '                                         <param name = "index" > Position at which the pages have To be inserted.</param>
        '                                         <param name = "pages" > Pages To be inserted.</param>
        '*/
        Public Sub Add(ByVal index As Integer, ByVal pages As ICollection(Of Page))
            ' Add the source pages to the document (deep level)!
            Dim importedPages As ICollection(Of Page) = CType(_document.Include(pages), ICollection(Of Page)) ' NOTE: Alien Pages MUST be contextualized (i.e. imported).

            ' Add the imported pages to the pages collection (shallow level)!
            If (index >= Me._pages.Count) Then
                Me._pages.AddAll(importedPages)
            Else
                Me._pages.InsertAll(index, importedPages)
            End If
        End Sub

        '/**
        '  <summary> Gets/Sets the document being managed.</summary>
        '*/
        Public Property Document As Document
            Get
                Return _document
            End Get
            Set(ByVal value As Document)
                Me._document = value
                Me._pages = _document.Pages
            End Set
        End Property

        '/**
        '  <summary> Extracts a page range from the document.</summary>
        '                                                     <param name = "startIndex" > the beginning index, inclusive.</param>
        '                                                     <param name = "endIndex" > the ending index, exclusive.</param>
        '                                                     <returns> Extracted page range.</returns>
        '*/
        Public Function Extract(ByVal startIndex As Integer, ByVal endIndex As Integer) As Document
            Dim extractedDocument As Document = New File().Document
            '{
            '// Add the pages to the target file!
            '/*
            '  NOTE: To be added to an alien document,
            '  Pages MUST be contextualized within it first,
            '  then added to the target pages collection.
            '*/
            extractedDocument.Pages.AddAll(CType(extractedDocument.Include(_pages.GetSlice(startIndex, endIndex)), ICollection(Of Page)))
            '}
            Return extractedDocument
        End Function

        '/**
        '  <summary> Moves a page range To a target position within the document.</summary>
        '                                                         <param name = "startIndex" > the beginning index, inclusive.</param>
        '                                                         <param name = "endIndex" > the ending index, exclusive.</param>
        '                                                         <param name = "targetIndex" > the target index.</param>
        '*/
        Public Sub Move(ByVal startIndex As Integer, ByVal endIndex As Integer, ByVal targetIndex As Integer)
            Dim pageCount As Integer = _pages.Count

            Dim movingPages As IList(Of Page) = _pages.GetSlice(startIndex, endIndex)

            '// Temporarily remove the pages from the pages collection!
            '/*
            '  NOTE: Shallow removal(only page references are removed, as their data are kept In the document).
            '  */
            _pages.RemoveAll(movingPages)

            ' Adjust indexes!
            pageCount -= movingPages.Count
            If (targetIndex > startIndex) Then
                targetIndex -= movingPages.Count ' Adjusts the target position due to shifting for temporary page removal.
            End If

            '// Reinsert the pages at the target position!
            '/*
            '  NOTE:   Shallow addition(only page references are added, as their data are already In the document).
            '      */
            If (targetIndex >= pageCount) Then
                _pages.AddAll(movingPages)
            Else
                _pages.InsertAll(targetIndex, movingPages)
            End If
        End Sub

        '/**
        '  <summary> Removes a page range from the document.</summary>
        '                                                             <param name = "startIndex" > the beginning index, inclusive.</param>
        '                                                             <param name = "endIndex" > the ending index, exclusive.</param>
        '*/
        Public Sub Remove(ByVal startIndex As Integer, ByVal endIndex As Integer)
            Dim removingPages As IList(Of Page) = _pages.GetSlice(startIndex, endIndex)

            '// Remove the pages from the pages collection!
            '/* NOTE: Shallow removal. */
            _pages.RemoveAll(removingPages)

            '// Remove the pages from the document (decontextualize)!
            '/* NOTE: Deep removal. */
            _document.Exclude(removingPages)
        End Sub

        '/**
        '  <summary> Bursts the document into Single-page documents.</summary>
        '                                                                 <returns> Split subdocuments.</returns>
        '*/
        Public Function Split() As IList(Of Document)
            Dim documents As IList(Of Document) = New List(Of Document)()
            For Each page As Page In _pages
                Dim pageDocument As Document = New File().Document
                pageDocument.Pages.Add(CType(page.Clone(pageDocument), Page))
                documents.Add(pageDocument)
            Next
            Return documents
        End Function

        '/**
        '  <summary> Splits the document into multiple subdocuments delimited by the specified page indexes.</summary>
        '               <param name = "indexes" > Split page indexes.</param>
        '               <returns> Split subdocuments.</returns>
        '*/
        Public Function Split(ParamArray indexes As Integer()) As IList(Of Document)
            Dim documents As IList(Of Document) = New List(Of Document)()
            '{
            Dim startIndex As Integer = 0
            For Each index As Integer In indexes
                documents.Add(Extract(startIndex, index))
                startIndex = index
            Next
            documents.Add(Extract(startIndex, _pages.Count))
            '}
            Return documents
        End Function

        '/**
        '  <summary> Splits the document into multiple subdocuments On maximum file size.</summary>
        '                           <param name = "maxDataSize" > Maximum data size (expressed In bytes) Of target files.
        '    Note that resulting files may be a little bit larger than Me value, As file data include (along With actual page data)
        '    some extra structures such As cross reference tables.</param>
        '                                                                                         <returns> Split documents.</returns>
        '*/
        Public Function Split(ByVal maxDataSize As Long) As IList(Of Document)
            Dim documents As IList(Of Document) = New List(Of Document)()
            '{
            Dim startPageIndex As Integer = 0
            Dim incrementalDataSize As Long = 0
            Dim visitedReferences As HashSet(Of PdfReference) = New HashSet(Of PdfReference)()
            For Each page As Page In _pages
                Dim pageDifferentialDataSize As Long = GetSize(page, visitedReferences)
                incrementalDataSize += pageDifferentialDataSize
                If (incrementalDataSize > maxDataSize) Then ' DataThen size limit reached.
                    Dim endPageIndex As Integer = page.Index

                    ' Split the current document page range!
                    documents.Add(Extract(startPageIndex, endPageIndex))

                    startPageIndex = endPageIndex
                    visitedReferences = New HashSet(Of PdfReference)()
                    incrementalDataSize = GetSize(page, visitedReferences)
                End If
            Next
            ' Split the last document page range!
            documents.Add(Extract(startPageIndex, _pages.Count))
            '}
            Return documents
        End Function
#End Region
#End Region
#End Region
    End Class

End Namespace

''/*
''  Copyright 2008 - 2012 Stefano Chizzolini. http: //www.dmdpdf.org

''  Contributors:
''    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

''  This file should be part Of the source code distribution Of "PDF Clown library" (the
''  Program): see the accompanying README files For more info.

''  This Program Is free software; you can redistribute it And/Or modify it under the terms
''  of the GNU Lesser General Public License as published by the Free Software Foundation;
''  either version 3 Of the License, Or (at your Option) any later version.

''  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
''  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
''  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

''  You should have received a copy Of the GNU Lesser General Public License along With Me
''  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

''  Redistribution And use, with Or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license And disclaimer, along With
''  Me list Of conditions.
''*/

'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.documents
'Imports DMD.org.dmdpdf.documents.contents
'Imports DMD.org.dmdpdf.documents.contents.composition
'Imports DMD.org.dmdpdf.documents.contents.objects
'Imports DMD.org.dmdpdf.files
'Imports DMD.org.dmdpdf.objects

'Imports System.Collections.Generic

'Namespace DMD.org.dmdpdf.tools
'    '{
'    '  /**
'    '    <summary> Tool For page management.</summary>
'    '  */
'    Public NotInheritable Class PageManager

'        '    /*
'        '  NOTE: As you can read on the PDF Clown's User Guide, referential operations on high-level object such as pages
'        '  can be done at two levels
'        '    1. shallow, involving page references but Not their data within the document;
'        '    2. deep, involving page data within the document.
'        '  This means that, For example, If you remove a page reference (shallow level) from the pages collection,
'        '  the data Of that page (deep level) are still within the document!
'        '*/

'#Region "shared"
'#Region "interface"
'#Region "public"
'        '/**
'        '  <summary> Gets the data size Of the specified page expressed In bytes.</summary>
'        '  <param name = "page" > page whose data size has To be calculated.</param>
'        '*/
'        Public Shared Function GetSize(ByVal page As Page) As Long
'            Return GetSize(page, New HashSet(Of PdfReference)())
'        End Function

'        '/**
'        '  <summary> Gets the data size Of the specified page expressed In bytes.</summary>
'        '  <param name = "page" > page whose data size has To be calculated.</param>
'        '  <param name = "visitedReferences" > References To data objects excluded from calculation.
'        '    This set Is useful, for example, to avoid recalculating the data size of shared resources.
'        '    During the operation, Me Set Is populated With references To visited data objects.</param>
'        '*/
'        Public Shared Function GetSize(ByVal page As Page, ByVal visitedReferences As HashSet(Of PdfReference)) As Long
'            Return GetSize(page.BaseObject, visitedReferences, True)
'        End Function

'#End Region

'#Region "private"

'        '    /**
'        '  <summary> Gets the data size Of the specified Object expressed In bytes.</summary>
'        '  <param name = "object" > Data Object whose size has To be calculated.</param>
'        '  <param name = "visitedReferences" > References To data objects excluded from calculation.
'        '    This set Is useful, for example, to avoid recalculating the data size of shared resources.
'        '    During the operation, Me Set Is populated With references To visited data objects.</param>
'        '  <param name = "isRoot" > Whether Me data Object represents the page root.</param>
'        '*/
'        Private Shared Function GetSize(ByVal obj As PdfDirectObject, ByVal visitedReferences As HashSet(Of PdfReference), ByVal isRoot As Boolean) As Long
'            Dim dataSize As Long = 0
'            Dim dataObject As PdfDataObject = PdfObject.Resolve(obj)

'            ' 1. Evaluating the current object...
'            If (TypeOf (obj) Is PdfReference) Then
'                Dim reference As PdfReference = CType(obj, PdfReference)
'                If (visitedReferences.Contains(reference)) Then
'                    Return 0 'Avoids circular references.
'                End If

'                If (TypeOf (dataObject) Is PdfDictionary AndAlso
'                    PdfName.Page.Equals(CType(dataObject, PdfDictionary)(PdfName.Type)) AndAlso
'                    Not isRoot) Then
'                    Return 0 ' Avoids references To other pages.
'                End If

'                visitedReferences.Add(reference)

'                ' Calculate the data size of the current object!
'                Dim buffer As IOutputStream = New Buffer()
'                reference.IndirectObject.WriteTo(buffer, reference.File)
'                dataSize += buffer.Length
'            End If

'            ' 2. Evaluating the current object's children...
'            Dim values As ICollection(Of PdfDirectObject) = Nothing
'            If (TypeOf (dataObject) Is PdfStream) Then
'                dataObject = CType(dataObject, PdfStream).Header
'            End If
'            If (TypeOf (dataObject) Is PdfDictionary) Then
'                values = CType(dataObject, PdfDictionary).Values
'            ElseIf (TypeOf (dataObject) Is PdfArray) Then
'                values = CType(dataObject, PdfArray)
'            End If

'            If (values IsNot Nothing) Then
'                ' Calculate the data size of the current object's children!
'                For Each value As PdfDirectObject In values
'                    dataSize += GetSize(value, visitedReferences, False)
'                Next
'            End If

'            Return dataSize
'        End Function

'#End Region
'#End Region
'#End Region

'#Region "dynamic"
'#Region "fields"

'        Private _document As Document
'        Private _pages As Pages

'#End Region

'#Region "constructors"

'        Public Sub New()
'            Me.New(Nothing)
'        End Sub

'        Public Sub New(ByVal document As Document)
'            Me.Document = document
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"

'        '    /**
'        '  <summary> Appends a document To the End Of the document.</summary>
'        '  <param name = "document" > Document To be added.</param>
'        '*/
'        Public Sub Add(ByVal document As Document)
'            Add(CType(document.Pages, ICollection(Of Page)))
'        End Sub

'        '/**
'        '  <summary> Inserts a document at the specified position In the document.</summary>
'        '  <param name = "index" > Position at which the document has To be inserted.</param>
'        '  <param name = "document" > Document To be inserted.</param>
'        '*/
'        Public Sub Add(ByVal index As Integer, ByVal document As Document)
'            Add(index, CType(document.Pages, ICollection(Of Page)))
'        End Sub

'        '/**
'        '  <summary> Appends a collection Of pages To the End Of the document.</summary>
'        '  <param name = "pages" > pages To be added.</param>
'        '*/
'        Public Sub Add(ByVal pages As ICollection(Of Page))
'            ' Add the source pages to the document (deep level)!
'            Dim importedPages As ICollection(Of Page) = CType(_document.Include(pages), ICollection(Of Page)) ' NOTE: Alien pages MUST be contextualized (i.e. imported).

'            ' Add the imported pages to the pages collection (shallow level)!
'            Me._pages.AddAll(importedPages)
'        End Sub

'        '/**
'        '  <summary> Inserts a collection Of pages at the specified position In the document.</summary>
'        '  <param name = "index" > Position at which the pages have To be inserted.</param>
'        '  <param name = "pages" > pages To be inserted.</param>
'        '*/
'        Public Sub Add(ByVal index As Integer, ByVal pages As ICollection(Of Page))
'            ' Add the source pages to the document (deep level)!
'            Dim importedPages As ICollection(Of Page) = CType(_document.Include(pages), ICollection(Of Page)) ' NOTE: Alien pages MUST be contextualized (i.e. imported).

'            ' Add the imported pages to the pages collection (shallow level)!
'            If (index >= Me._pages.Count) Then
'                Me._pages.AddAll(importedPages)
'            Else
'                Me._pages.InsertAll(index, importedPages)
'            End If
'        End Sub

'        '/**
'        '  <summary> Gets/Sets the document being managed.</summary>
'        '*/
'        Public Property Document As Document
'            Get
'                Return Me._document
'            End Get
'            Set(value As Document)
'                Me._document = value
'                Me._pages = _document.Pages
'            End Set
'        End Property

'        '/**
'        '  <summary> Extracts a page range from the document.</summary>
'        '  <param name = "startIndex" > the beginning index, inclusive.</param>
'        '  <param name = "endIndex" > the ending index, exclusive.</param>
'        '  <returns> Extracted page range.</returns>
'        '*/
'        Public Function Extract(ByVal startIndex As Integer, ByVal endIndex As Integer) As Document
'            Dim extractedDocument As Document = New File().Document
'            '// Add the pages to the target file!
'            '/*
'            '  NOTE: To be added to an alien document,
'            '  pages MUST be contextualized within it first,
'            '  then added to the target pages collection.
'            '*/
'            extractedDocument.Pages.AddAll(CType(extractedDocument.Include(_pages.GetSlice(startIndex, endIndex)), ICollection(Of Page)))
'            Return extractedDocument
'        End Function

'        '/**
'        '  <summary> Moves a page range To a target position within the document.</summary>
'        '  <param name = "startIndex" > the beginning index, inclusive.</param>
'        '  <param name = "endIndex" > the ending index, exclusive.</param>
'        '  <param name = "targetIndex" > the target index.</param>
'        '*/
'        Public Sub Move(ByVal startIndex As Integer, ByVal endIndex As Integer, ByVal targetIndex As Integer)
'            Dim pageCount As Integer = _pages.Count

'            Dim movingPages As IList(Of Page) = _pages.GetSlice(startIndex, endIndex)

'            '// Temporarily remove the pages from the pages collection!
'            '/*
'            '  NOTE: Shallow removal(only page references are removed, as their data are kept In the document).
'            '*/
'            _pages.RemoveAll(movingPages)

'            ' Adjust indexes!
'            pageCount -= movingPages.Count
'            If (targetIndex > startIndex) Then
'                targetIndex -= movingPages.Count ' Adjusts the target position due to shifting for temporary page removal.
'            End If

'            '// Reinsert the pages at the target position!
'            '/*
'            '  NOTE:   Shallow addition(only page references are added, as their data are already In the document).
'            '  */
'            If (targetIndex >= pageCount) Then
'                _pages.AddAll(movingPages)
'            Else
'                _pages.InsertAll(targetIndex, movingPages)
'            End If
'        End Sub


'        '/**
'        '  <summary> Removes a page range from the document.</summary>
'        '           <param name = "startIndex" > the beginning index, inclusive.</param>
'        '           <param name = "endIndex" > the ending index, exclusive.</param>
'        '*/
'        Public Sub Remove(ByVal startIndex As Integer, ByVal endIndex As Integer)
'            Dim removingPages As IList(Of Page) = _pages.GetSlice(startIndex, endIndex)

'            '// Remove the pages from the pages collection!
'            '/* NOTE: Shallow removal. */
'            _pages.RemoveAll(removingPages)

'            '// Remove the pages from the document (decontextualize)!
'            '/* NOTE: Deep removal. */
'            _document.Exclude(removingPages)
'        End Sub

'        '/**
'        '  <summary> Bursts the document into Single-page documents.</summary>
'        '               <returns> Split subdocuments.</returns>
'        '*/
'        Public Function Split() As IList(Of Document)
'            Dim documents As IList(Of Document) = New List(Of Document)()
'            For Each page As Page In _pages
'                Dim pageDocument As Document = New File().Document
'                pageDocument.Pages.Add(CType(page.Clone(pageDocument), Page))
'                documents.Add(pageDocument)
'            Next
'            Return documents
'        End Function

'        '/**
'        '  <summary> Splits the document into multiple subdocuments delimited by the specified page indexes.</summary>
'        '                     <param name = "indexes" > Split page indexes.</param>
'        '                     <returns> Split subdocuments.</returns>
'        '*/
'        Public Function Split(ParamArray indexes As Integer()) As IList(Of Document)
'            Dim documents As IList(Of Document) = New List(Of Document)()
'            Dim startIndex As Integer = 0
'            For Each index As Integer In indexes
'                documents.Add(Extract(startIndex, index))
'                startIndex = index
'            Next
'            documents.Add(Extract(startIndex, _pages.Count))

'            Return documents
'        End Function

'        '/**
'        '  <summary> Splits the document into multiple subdocuments On maximum file size.</summary>
'        '                                 <param name = "maxDataSize" > Maximum data size (expressed In bytes) Of target files.
'        '    Note that resulting files may be a little bit larger than Me value, As file data include (along With actual page data)
'        '    some extra structures such As cross reference tables.</param>
'        '                                 <returns> Split documents.</returns>
'        '*/
'        Public Function Split(ByVal maxDataSize As Long) As IList(Of Document)
'            Dim documents As IList(Of Document) = New List(Of Document)()
'            Dim startPageIndex As Integer = 0
'            Dim incrementalDataSize As Long = 0
'            Dim visitedReferences As HashSet(Of PdfReference) = New HashSet(Of PdfReference)()
'            For Each page As Page In _pages
'                Dim pageDifferentialDataSize As Long = GetSize(page, visitedReferences)
'                incrementalDataSize += pageDifferentialDataSize
'                If (incrementalDataSize > maxDataSize) Then ' Data Then size limit reached.
'                    Dim endPageIndex As Integer = page.index

'                    ' Split the current document page range!
'                    documents.Add(Extract(startPageIndex, endPageIndex))

'                    startPageIndex = endPageIndex
'                    visitedReferences = New HashSet(Of PdfReference)()
'                    incrementalDataSize = GetSize(page, visitedReferences)
'                End If
'            Next
'            ' Split the last document page range!
'            documents.Add(Extract(startPageIndex, _pages.Count))

'            Return documents
'        End Function

'#End Region
'#End Region
'#End Region

'    End Class

'End Namespace
