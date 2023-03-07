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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Text

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF indirect object [PDF:1.6:3.2.9].</summary>
    '*/
    Public Class PdfIndirectObject
        Inherits PdfObject
        Implements IPdfIndirectObject

#Region "Static"
#Region "fields"

        Private Shared ReadOnly BeginIndirectObjectChunk As Byte() = tokens.Encoding.Pdf.Encode(Symbol.Space & Keyword.BeginIndirectObject & Symbol.LineFeed)
        Private Shared ReadOnly EndIndirectObjectChunk As Byte() = tokens.Encoding.Pdf.Encode(Symbol.LineFeed & Keyword.EndIndirectObject & Symbol.LineFeed)

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _dataObject As PdfDataObject
        Private _file As File
        Private _original As Boolean
        Private _reference As PdfReference
        Private _xrefEntry As XRefEntry

        Private _updateable As Boolean = True
        Private _updated As Boolean
        Private _virtual_ As Boolean

#End Region

#Region "constructors"
        '/**
        '  <param name="file">Associated file.</param>
        '  <param name="dataObject">
        '    <para>Data object associated to the indirect object. It MUST be</para>
        '    <list type="bullet">
        '      <item><code>null</code>, if the indirect object is original or free.</item>
        '      <item>NOT <code>null</code>, if the indirect object is new and in-use.</item>
        '    </list>
        '  </param>
        '  <param name="xrefEntry">Cross-reference entry associated to the indirect object. If the
        '    indirect object is new, its offset field MUST be set to 0.</param>
        '*/
        Friend Sub New(ByVal file As File, ByVal dataObject As PdfDataObject, ByVal xrefEntry As XRefEntry)
            Me._file = file
            Me._dataObject = Include(dataObject)
            Me._xrefEntry = xrefEntry
            Me._original = (xrefEntry.Offset >= 0)
            Me._reference = New PdfReference(Me)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal Visitor As IVisitor, ByVal data As Object) As PdfObject
            Return Visitor.Visit(Me, data)
        End Function

        '/**
        '  <summary>Adds the <see cref="DataObject">data Object</see> To the specified Object stream
        '  [PDF:1.6:3.4.6].</summary>
        '  <param name = "objectStream" > Target Object stream.</param>
        ' */
        Public Sub Compress(ByVal objectStream As ObjectStream)
            ' Remove from previous object stream!
            Uncompress()
            If (objectStream IsNot Nothing) Then
                ' Add to the object stream!
                objectStream(_xrefEntry.Number) = DataObject
                ' Update its xref entry!
                _xrefEntry.Usage = XRefEntry.UsageEnum.InUseCompressed
                _xrefEntry.StreamNumber = objectStream.Reference.ObjectNumber
                _xrefEntry.Offset = XRefEntry.UndefinedOffset ' Internal Object index unknown (To Set On Object stream serialization -- see ObjectStream).
            End If
        End Sub

        Public Overrides ReadOnly Property Container As PdfIndirectObject
            Get
                Return Me
            End Get
        End Property

        Public Overrides ReadOnly Property File As File
            Get
                Return Me._file
            End Get
        End Property

        Public Overrides Function GetHashCode() As Integer
            '/*
            '  NOTE:   Uniqueness should be achieved XORring the (local) reference hashcode with the (global)
            '  file hashcode.
            '  NOTE: Do Not directly invoke reference.GetHashCode() method here as, conversely relying on
            '  Me method, it would trigger an infinite loop.
            '*/
            Return _reference.Id.GetHashCode() Xor _file.GetHashCode()
        End Function

        '/**
        '  <summary>Gets whether Me Object Is compressed within an Object stream [PDF:1.6:3.4.6].
        '  </summary>
        '*/
        Public Function IsCompressed() As Boolean
            Return _xrefEntry.Usage = XRefEntry.UsageEnum.InUseCompressed
        End Function

        '/**
        '  <summary>Gets whether Me Object contains a data Object.</summary>
        '*/
        Public Function IsInUse() As Boolean
            Return (_xrefEntry.Usage = XRefEntry.UsageEnum.InUse)
        End Function

        '/**
        '  <summary>Gets whether Me Object comes intact from an existing file.</summary>
        '*/
        Public Function IsOriginal() As Boolean
            Return Me._original
        End Function

        Public Overrides ReadOnly Property Parent As PdfObject
            Get
                Return Nothing  ' NOTE: As indirect objects are root objects, no parent can be associated.
            End Get
        End Property

        Friend Overrides Sub SetParent(value As PdfObject)
            ' NOOP: As indirect objects are root objects, no parent can be associated. */}
        End Sub

        Public Overrides Function Swap(ByVal other As PdfObject) As PdfObject
            Dim otherObject As PdfIndirectObject = CType(other, PdfIndirectObject)
            Dim otherDataObject As PdfDataObject = otherObject._dataObject
            ' Update the other!
            otherObject.DataObject = _dataObject
            ' Update Me one!
            Me.DataObject = otherDataObject
            Return Me
        End Function

        '/**
        '  <summary> Removes the <see cref="DataObject">data Object</see> from its Object stream [PDF:1.6:3.4.6].</summary>
        '*/
        Public Sub Uncompress()
            If (Not IsCompressed()) Then Return

            ' Remove from its object stream!
            Dim oldObjectStream As ObjectStream = CType(_file.IndirectObjects(_xrefEntry.StreamNumber).DataObject, ObjectStream)
            oldObjectStream.Remove(_xrefEntry.Number)
            ' Update its xref entry!
            _xrefEntry.Usage = XRefEntry.UsageEnum.InUse
            _xrefEntry.StreamNumber = -1 ' No Object stream.
            _xrefEntry.Offset = XRefEntry.UndefinedOffset ' Offset unknown (To Set On file serialization -- see CompressedWriter).
        End Sub

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
            If (value AndAlso Me._original) Then
                '                        /*
                '  NOTE:           It 's expected that DropOriginal() is invoked by IndirectObjects indexer;
                '                  such an action Is delegated because clients may invoke directly the indexer skipping
                '  Me method.
                '*/
                _file.IndirectObjects.Update(Me)
            End If
            _updated = value
        End Sub


        Public Overrides Sub WriteTo(ByVal Stream As IOutputStream, ByVal context As File)
            ' Header.
            Stream.Write(_reference.Id)
            Stream.Write(BeginIndirectObjectChunk)
            ' Body.
            DataObject.WriteTo(Stream, context)
            ' Tail.
            Stream.Write(EndIndirectObjectChunk)
        End Sub

        Public ReadOnly Property XrefEntry As XRefEntry
            Get
                Return Me._xrefEntry
            End Get
        End Property

#Region "IPdfIndirectObject"

        Public Overrides Function Clone(context As File) As PdfObject Implements IPdfIndirectObject.Clone
            Return MyBase.Clone(context)
        End Function

        Public Property DataObject As PdfDataObject Implements IPdfIndirectObject.DataObject
            Get
                If (_dataObject Is Nothing) Then
                    Select Case (_xrefEntry.Usage)
                        Case XRefEntry.UsageEnum.Free ' Free entry (no data object at all).
                                'break;
                        Case XRefEntry.UsageEnum.InUse    ' In-use entry (late-bound data object).
                            Dim parser As FileParser = _file.Reader.Parser
                            ' Retrieve the associated data object among the original objects!
                            parser.Seek(_xrefEntry.Offset)
                            ' Get the indirect data object!
                            _dataObject = Include(parser.ParsePdfObject(4)) ' NOTE: Skips the indirect-Object header.
                            'break;
                        Case XRefEntry.UsageEnum.InUseCompressed
                            ' Get the object stream where its data object Is stored!
                            Dim ObjectStream As ObjectStream = CType(_file.IndirectObjects(_xrefEntry.StreamNumber).DataObject, ObjectStream)
                            ' Get the indirect data object!
                            _dataObject = Include(ObjectStream(_xrefEntry.Number))
                            'break;
                    End Select
                End If
                Return _dataObject
            End Get
            Set(ByVal value As PdfDataObject)
                If (_xrefEntry.Generation = XRefEntry.GenerationUnreusable) Then Throw New Exception("Unreusable entry.")
                Exclude(_dataObject)
                _dataObject = Include(value)
                _xrefEntry.Usage = XRefEntry.UsageEnum.InUse
                Update()
            End Set
        End Property

        Public Sub Delete() Implements IPdfIndirectObject.Delete
            If (_file Is Nothing) Then Return
            '/*
            '  NOTE:                           It 's expected that DropFile() is invoked by IndirectObjects.Remove() method;
            '                                  such an action Is delegated because clients may invoke directly Remove() method,
            '  skipping Me method.
            '*/
            _file.IndirectObjects.RemoveAt(_xrefEntry.Number)
        End Sub

        Public Overrides ReadOnly Property IndirectObject As PdfIndirectObject Implements IPdfIndirectObject.IndirectObject
            Get
                Return Me
            End Get
        End Property

        Public Overrides ReadOnly Property Reference As PdfReference Implements IPdfIndirectObject.Reference
            Get
                Return Me._reference
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim Buffer As New StringBuilder()
            ' Header.
            Buffer.Append(_reference.Id).Append(" obj").Append(Symbol.LineFeed)
            ' Body.
            Buffer.Append(DataObject)

            Return Buffer.ToString()
        End Function

#End Region
#End Region

#Region "Protected"

        Protected Friend Overrides Property Virtual As Boolean
            Get
                Return Me._virtual_
            End Get
            Set(ByVal value As Boolean)
                If (_virtual_ AndAlso Not value) Then
                    '/*
                    '  NOTE: When a virtual indirect object becomes concrete it must be registered.
                    '*/
                    _file.IndirectObjects.AddVirtual(Me)
                    _virtual_ = False
                    Reference.Update()
                Else
                    _virtual_ = value
                End If
                _dataObject.Virtual = _virtual_
            End Set
        End Property

#End Region

#Region "internal"

        Friend Sub DropFile()
            Uncompress()
            _file = Nothing
        End Sub

        Friend Sub DropOriginal()
            _original = False
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace