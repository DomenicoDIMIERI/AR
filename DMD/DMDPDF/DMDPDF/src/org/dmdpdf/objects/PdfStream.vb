'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.bytes.filters
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF stream object [PDF:1.6:3.2.7].</summary>
    '*/
    Public Class PdfStream
        Inherits PdfDataObject
        Implements IFileResource

#Region "Static"
#Region "fields"
        Private Shared ReadOnly BeginStreamBodyChunk As Byte() = Encoding.Pdf.Encode(Symbol.LineFeed & Keyword.BeginStream & Symbol.LineFeed)
        Private Shared ReadOnly EndStreamBodyChunk As Byte() = Encoding.Pdf.Encode(Symbol.LineFeed & Keyword.EndStream)
#End Region
#End Region

#Region "dynamic"
#Region "fields"
        Friend _body As IBuffer
        Friend _header As PdfDictionary

        Private _parent As PdfObject
        Private _updateable As Boolean = True
        Private _updated As Boolean
        Private virtual_ As Boolean

        '/**
        '  <summary> Indicates whether {@link #_body} has already been resolved And therefore contains the
        '  actual stream data.</summary>
        '*/
        Private _bodyResolved As Boolean
#End Region

#Region "constructors"
        Public Sub New()
            Me.New(New PdfDictionary(), New bytes.Buffer())
        End Sub

        Public Sub New(ByVal _header As PdfDictionary)
            Me.New(_header, New bytes.Buffer())
        End Sub


        Public Sub New(ByVal _body As IBuffer)
            Me.New(New PdfDictionary(), _body)
        End Sub

        Public Sub New(ByVal header As PdfDictionary, ByVal body As IBuffer)
            Me._header = CType(Include(header), PdfDictionary)
            Me._body = body
            Me._body.Dirty = False
            AddHandler Me._body.OnChange, AddressOf handleOnChange
        End Sub

        Private Sub handleOnChange(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Update()
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
            Return visitor.Visit(Me, data)
        End Function

        '/**
        '  <summary> Gets the decoded stream _body.</summary>
        '*/
        Public ReadOnly Property Body As IBuffer
            Get
                '/*
                '  NOTE: Encoding filters are removed by Default because they belong To a lower layer (token
                '  layer), so that it's appropriate and consistent to transparently keep the object layer
                '  unaware of such a facility.
                '*/
                Return GetBody(True)
            End Get
        End Property

        Public Property Filter As PdfDirectObject
            Get
                If (Me._header(PdfName.F) Is Nothing) Then
                    Return CType(Me._header.Resolve(PdfName.Filter), PdfDirectObject)
                Else
                    Return CType(_header.Resolve(PdfName.FFilter), PdfDirectObject)
                End If
            End Get
            Protected Set(ByVal value As PdfDirectObject)
                If (_header(PdfName.F) Is Nothing) Then
                    Me._header(PdfName.Filter) = value
                Else
                    Me._header(PdfName.FFilter) = value
                End If
            End Set

        End Property


        '/**
        '  <summary> Gets the stream _body.</summary>
        '  <param name = "decode" > Defines whether the _body has To be decoded.</param>
        '*/
        Public Function GetBody(ByVal decode As Boolean) As IBuffer
            If (Not _bodyResolved) Then
                '/*
                '  NOTE: In case of stream data from external file, a copy to the local buffer has to be done.
                '*/
                Dim dataFile As FileSpecification = Me.dataFile
                If (dataFile IsNot Nothing) Then
                    Me.Updateable = False
                    Me._body.SetLength(0)
                    Me._body.Write(dataFile.GetInputStream())
                    Me._body.Dirty = False
                    Me.Updateable = True
                End If
                Me._bodyResolved = True
            End If
            If (decode) Then
                Dim filter As PdfDataObject = Me.Filter
                If (filter IsNot Nothing) Then ' Stream Then encoded.
                    Me._header.Updateable = False
                    Dim parameters As PdfDataObject = Me.parameters
                    If (TypeOf (filter) Is PdfName) Then ' Single Then Filter.
                        Me._body.Decode(bytes.filters.Filter.Get(CType(filter, PdfName)), CType(parameters, PdfDictionary))
                    Else ' Multiple filters.
                        Dim filterIterator As IEnumerator(Of PdfDirectObject) = CType(filter, PdfArray).GetEnumerator()
                        Dim parametersIterator As IEnumerator(Of PdfDirectObject)
                        If (parameters IsNot Nothing) Then
                            parametersIterator = CType(parameters, PdfArray).GetEnumerator()
                        Else
                            parametersIterator = Nothing
                        End If
                        While (filterIterator.MoveNext())
                            Dim filterParameters As PdfDictionary
                            If (parametersIterator Is Nothing) Then
                                filterParameters = Nothing
                            Else
                                parametersIterator.MoveNext()
                                filterParameters = CType(Resolve(parametersIterator.Current), PdfDictionary)
                            End If
                            Me._body.Decode(bytes.filters.Filter.Get(CType(Resolve(filterIterator.Current), PdfName)), filterParameters)
                        End While
                    End If
                    Me.Filter = Nothing ' The stream Is free from encodings.
                    Me._header.Updateable = True
                End If
            End If
            Return Me._body
        End Function

        '/**
        '  <summary> Gets the stream _header.</summary>
        '*/
        Public ReadOnly Property Header As PdfDictionary
            Get
                Return Me._header
            End Get
        End Property

        Public ReadOnly Property Parameters As PdfDirectObject
            Get
                If (Me._header(PdfName.F) Is Nothing) Then
                    Return CType(Me._header.Resolve(PdfName.DecodeParms), PdfDirectObject)
                Else
                    Return CType(Me._header.Resolve(PdfName.FDecodeParms), PdfDirectObject)
                End If
            End Get
        End Property

        Protected Sub SetParameters(ByVal value As PdfDirectObject)
            If (Me._header(PdfName.F) Is Nothing) Then
                Me._header(PdfName.DecodeParms) = value
            Else
                Me._header(PdfName.FDecodeParms) = value
            End If
        End Sub

        Public Overrides ReadOnly Property Parent As PdfObject
            Get
                Return Me._parent
            End Get
        End Property

        Friend Overrides Sub SetParent(value As PdfObject)
            Me._parent = value
        End Sub


        '/**
        '  <param name = "preserve" > Indicates whether the data from the old data source substitutes the
        '  New one. This way data can be imported to/exported from local Or preserved in case of external
        '  file location changed.</param>
        '  <seealso cref = "DataFile" />
        '*/
        Public Sub SetDataFile(ByVal value As FileSpecification, ByVal preserve As Boolean)
            '/*
            '  NOTE: If preserve argument Is Set To True, _body's dirtiness MUST be forced in order to ensure
            '  data serialization To the New external location.

            '  Old data source | New data source | preserve | Action
            '  ----------------------------------------------------------------------------------------------
            '  local           | Not null        | false     | A. Substitute local with New file.
            '  local           | Not null        | true      | B. Export local to New file.
            '  external        | Not null        | false     | C. Substitute old file with New file.
            '  external        | Not null        | true      | D. Copy old file data to New file.
            '  local           | null            | (any)     | E. No action.
            '  external        | null            | false     | F. Empty local.
            '  external        | null            | true      | G. Import old file to local.
            '  ----------------------------------------------------------------------------------------------
            '*/
            Dim oldDataFile As FileSpecification = Me.dataFile
            Dim dataFileObject As PdfDirectObject
            If (value IsNot Nothing) Then
                dataFileObject = value.BaseObject
            Else
                dataFileObject = Nothing
            End If

            If (value IsNot Nothing) Then
                If (preserve) Then
                    If (oldDataFile IsNot Nothing) Then ' Case D (copy old file data to New file).
                        If (Not Me._bodyResolved) Then
                            ' Transfer old file data to local!
                            Me.GetBody(False) ' Ensures that external data Is loaded As-Is into the local buffer.
                        End If
                    Else ' Case B (export local To New file).
                        ' Transfer local settings to file!
                        _header(PdfName.FFilter) = _header(PdfName.Filter) : _header.Remove(PdfName.Filter)
                        _header(PdfName.FDecodeParms) = _header(PdfName.DecodeParms) : _header.Remove(PdfName.DecodeParms)

                        ' Ensure local data represents actual data (otherwise it would be substituted by resolved file data)!
                        _bodyResolved = True
                    End If
                    ' Ensure local data has to be serialized to New file!
                    Me._body.Dirty = True
                Else ' Case A/C (substitute local/old file With New file).
                    ' Dismiss local/old file data!
                    Me._body.SetLength(0)
                    ' Dismiss local/old file settings!
                    Me.Filter = Nothing
                    Me.SetParameters(Nothing)
                    ' Ensure local data has to be loaded from New file!
                    Me._bodyResolved = False
                End If
            Else
                If (oldDataFile IsNot Nothing) Then
                    If (preserve) Then ' Case G (import old file to local).
                        ' Transfer old file data to local!
                        GetBody(False) ' Ensures that external data Is loaded As-Is into the local buffer.
                        ' Transfer old file settings to local!
                        Me._header(PdfName.Filter) = Me._header(PdfName.FFilter) : Me._header.Remove(PdfName.FFilter)
                        Me._header(PdfName.DecodeParms) = Me._header(PdfName.FDecodeParms) : Me._header.Remove(PdfName.FDecodeParms)
                    Else ' Case F (empty local).
                        ' Dismiss old file data!
                        _body.SetLength(0)
                        ' Dismiss old file settings!
                        Me.Filter = Nothing
                        Me.SetParameters(Nothing)
                        ' Ensure local data represents actual data (otherwise it would be substituted by resolved file data)!
                        Me._bodyResolved = True
                    End If
                Else ' E (no action).
                    'NOOP *
                End If
            End If
            Me._header(PdfName.F) = dataFileObject
        End Sub

        Public Overrides Function Swap(ByVal other As PdfObject) As PdfObject
            Dim otherStream As PdfStream = CType(other, PdfStream)
            Dim otherHeader As PdfDictionary = otherStream._header
            Dim otherBody As IBuffer = otherStream._body
            ' Update the other!
            otherStream._header = Me._header
            otherStream._body = Me._body
            otherStream.Update()
            ' Update this one!
            Me._header = otherHeader
            Me._body = otherBody
            Me.Update()
            Return Me
        End Function

        Public Overrides Property Updateable As Boolean
            Get
                Return Me._updateable
            End Get
            Set(ByVal value As Boolean)
                Me._updateable = value
            End Set
        End Property

        Public Overrides ReadOnly Property Updated As Boolean
            Get
                Return Me._updated
            End Get
        End Property

        Protected Friend Overrides Sub SetUpdated(value As Boolean)
            Me._updated = value
        End Sub

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File)
            '/*
            '  NOTE: The _header Is temporarily tweaked To accommodate serialization settings.
            '*/
            Me._header.Updateable = False
            Dim bodyData As Byte()
            Dim bodyUnencoded As Boolean
            Dim dataFile As FileSpecification = Me.dataFile
            '/*
            '  NOTE: In case of external file, the _body buffer has to be saved back only if the file was
            '  actually resolved(that Is brought into the _body buffer) And modified.
            '*/
            Dim encodeBody As Boolean = (dataFile Is Nothing OrElse (Me._bodyResolved AndAlso Me._body.Dirty))
            If (encodeBody) Then
                Dim filterObject As PdfDirectObject = Me.Filter
                If (filterObject Is Nothing) Then ' Unencoded Then _body.
                    '/*
                    '  NOTE: Header entries related To stream _body encoding are temporary, instrumental To
                    '  the current serialization process only.
                    '*/
                    bodyUnencoded = True

                    ' Set the filter to apply!
                    filterObject = PdfName.FlateDecode ' zlib/deflate filter.
                    ' Get encoded _body data applying the filter to the stream!
                    bodyData = Me._body.Encode(bytes.filters.Filter.Get(CType(filterObject, PdfName)), Nothing)
                    ' Set 'Filter' entry!
                    Me.Filter = filterObject
                Else ' Encoded _body.
                    bodyUnencoded = False
                    ' Get encoded _body data!
                    bodyData = Me._body.ToByteArray()
                End If

                If (dataFile IsNot Nothing) Then
                    '/*
                    '  NOTE: In case of external file, _body data has to be serialized there, leaving empty
                    '  its representation within this stream.
                    '*/
                    Try
                        Dim dataFileOutputStream As IOutputStream = dataFile.GetOutputStream()
                        dataFileOutputStream.Write(bodyData)
                        dataFileOutputStream.Dispose()
                    Catch e As System.Exception
                        Throw New Exception("Data writing into " & dataFile.Path & " failed.", e)
                    End Try
                    ' Local serialization Is empty!
                    bodyData = New Byte() {}
                End If
            Else
                bodyUnencoded = False
                bodyData = New Byte() {}
            End If

            ' Set the encoded data length!
            Me._header(PdfName.Length) = PdfInteger.Get(bodyData.Length)

            ' 1. Header.
            Me._header.WriteTo(stream, context)

            If (bodyUnencoded) Then
                ' Restore actual _header entries!
                Me._header(PdfName.Length) = PdfInteger.Get(CInt(Me._body.Length))
                Me.Filter = Nothing
            End If

            ' 2. Body.
            stream.Write(BeginStreamBodyChunk)
            stream.Write(bodyData)
            stream.Write(EndStreamBodyChunk)

            Me._header.Updateable = True
        End Sub

#Region "IFileResource"

        <PDF(VersionEnum.PDF12)>
        Public Property DataFile As FileSpecification Implements IFileResource.DataFile
            Get
                Return FileSpecification.Wrap(Me._header(PdfName.F))
            End Get
            Set(ByVal value As FileSpecification)
                Me.SetDataFile(value, False)
            End Set
        End Property

#End Region
#End Region

#Region "Protected"

        Protected Friend Overrides Property Virtual As Boolean
            Get
                Return Me.virtual_
            End Get
            Set(value As Boolean)
                Me.virtual_ = value
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace

