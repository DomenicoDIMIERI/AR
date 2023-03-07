'/*
'  Copyright 2011-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text

Namespace DMD.org.dmdpdf.documents.contents.fonts

    '/**
    '  <summary>CFF file format parser [CFF:1.0].</summary>
    '*/
    Friend NotInheritable Class CffParser

#Region "types"
        '/**
        '  <summary>Dictionary [CFF:1.0:4].</summary>
        '*/
        Private NotInheritable Class Dict
            Implements IDictionary(Of Integer, IList(Of Object))

            Public Enum OperatorEnum
                Charset = 15
                CharStrings = 17
                CharstringType = 6 + OperatorValueEscape
                Encoding = 16
            End Enum

            Private Const OperatorValueEscape As Integer = 12 << 8

            Public Shared Function GetOperatorName(ByVal value As OperatorEnum) As String
                Select Case (value)
                    Case OperatorEnum.Charset : Return "charset"
                    Case Else : Return value.ToString()
                End Select
            End Function

            Public Shared Function Parse(ByVal data As Byte()) As Dict
                Return Parse(New bytes.Buffer(data))
            End Function

            Public Shared Function Parse(ByVal stream As IInputStream) As Dict
                Dim entries As IDictionary(Of Integer, IList(Of Object)) = New Dictionary(Of Integer, IList(Of Object))()
                Dim operands As IList(Of Object) = Nothing
                While (True)
                    Dim b0 As Integer = stream.ReadByte()
                    If (b0 = -1) Then Exit While

                    If (b0 >= 0 AndAlso b0 <= 21) Then ' Operator.
                        Dim operator_ As Integer = b0
                        If (b0 = 12) Then ' 2 - Byte Then Operator.
                            operator_ = operator_ << 8 + stream.ReadByte()
                        End If

                        '/*
                        '  NOTE: In order to resiliently support unknown operators on parsing, parsed operators
                        '  are Not directly mapped to OperatorEnum.
                        '*/
                        entries(operator_) = operands
                        operands = Nothing
                    Else ' Operand.
                        If (operands Is Nothing) Then
                            operands = New List(Of Object)()
                        End If


                        If (b0 = 28) Then ' 3 - Byte Then Integer.
                            operands.Add(stream.ReadByte() << 8 + stream.ReadByte())
                        ElseIf (b0 = 29) Then ' 5 - Byte Then Integer.
                            operands.Add(stream.ReadByte() << 24 + stream.ReadByte() << 16 + stream.ReadByte() << 8 + stream.ReadByte())
                        ElseIf (b0 = 30) Then ' Variable - length Then real.
                            Dim operandBuilder As StringBuilder = New StringBuilder()
                            Dim ended As Boolean = False
                            Do
                                Dim b As Integer = stream.ReadByte()
                                Dim nibbles As Integer() = {(b >> 4) And &HF, b And &HF}
                                For Each nibble As Integer In nibbles
                                    Select Case (nibble)
                                        Case &H0, &H1, &H2, &H3, &H4, &H5, &H6, &H7, &H8, &H9
                                            operandBuilder.Append(nibble)
                                            'break;
                                        Case &HA ' Decimal point.
                                            operandBuilder.Append(".")
                                            'break;
                                        Case &HB ' Positive exponent.
                                            operandBuilder.Append("E")
                                            'break;
                                        Case &HC ' Negative exponent.
                                            operandBuilder.Append("E-")
                                            'break;
                                        Case &HD ' Reserved.
                                            'break;
                                        Case &HE ' Minus.
                                            operandBuilder.Append("-")
                                            'break;
                                        Case &HF ' End of number.
                                            ended = True
                                            'break;
                                    End Select
                                Next
                            Loop While (Not ended)
                            operands.Add(Double.Parse(operandBuilder.ToString(), NumberStyles.Float))
                        ElseIf (b0 >= 32 AndAlso b0 <= 246) Then ' 1 - Byte Then Integer.
                            operands.Add(b0 - 139)
                        ElseIf (b0 >= 247 AndAlso b0 <= 250) Then ' 2 - Byte Then positive Integer.
                            operands.Add((b0 - 247) << 8 + stream.ReadByte() + 108)
                        ElseIf (b0 >= 251 AndAlso b0 <= 254) Then ' 2 - Byte Then negative Integer.
                            operands.Add(-(b0 - 251) << 8 - stream.ReadByte() - 108)
                        Else ' Reserved.
                            '/* NOOP */ }
                        End If
                    End If
                End While
                Return New Dict(entries)
            End Function

            Private ReadOnly entries As IDictionary(Of Integer, IList(Of Object))

            Private Sub New(ByVal entries As IDictionary(Of Integer, IList(Of Object)))
                Me.entries = entries
            End Sub

            Public Sub Add(ByVal key As Integer, ByVal value As IList(Of Object)) Implements Generic.IDictionary(Of Integer, IList(Of Object)).Add
                Throw New NotSupportedException()
            End Sub

            Public Function ContainsKey(ByVal key As Integer) As Boolean Implements Generic.IDictionary(Of Integer, IList(Of Object)).ContainsKey
                Return entries.ContainsKey(key)
            End Function


            Public ReadOnly Property Keys As ICollection(Of Integer) Implements Generic.IDictionary(Of Integer, IList(Of Object)).Keys
                Get
                    Return entries.Keys
                End Get
            End Property

            Public Function Remove(ByVal key As Integer) As Boolean Implements Generic.IDictionary(Of Integer, IList(Of Object)).Remove
                Throw New NotSupportedException()
            End Function

            Default Public Property Item(ByVal key As Integer) As IList(Of Object) Implements Generic.IDictionary(Of Integer, IList(Of Object)).Item
                Get
                    Dim value As IList(Of Object) = Nothing
                    entries.TryGetValue(key, value)
                    Return value
                End Get
                Set(ByVal value As IList(Of Object))
                    Throw New NotSupportedException()
                End Set
            End Property

            Public Function TryGetValue(ByVal key As Integer, ByRef value As IList(Of Object)) As Boolean Implements Generic.IDictionary(Of Integer, IList(Of Object)).TryGetValue
                Return entries.TryGetValue(key, value)
            End Function

            Public ReadOnly Property Values As ICollection(Of IList(Of Object)) Implements Generic.IDictionary(Of Integer, IList(Of Object)).Values
                Get
                    Return entries.Values
                End Get
            End Property

            Private Sub _Add(ByVal keyValuePair As KeyValuePair(Of Integer, IList(Of Object))) Implements ICollection(Of KeyValuePair(Of Integer, IList(Of Object))).Add
                Throw New NotSupportedException
            End Sub

            Public Sub Clear() Implements ICollection(Of KeyValuePair(Of Integer, IList(Of Object))).Clear
                Throw New NotSupportedException()
            End Sub

            Private Function _Contains(ByVal keyValuePair As KeyValuePair(Of Integer, IList(Of Object))) As Boolean Implements ICollection(Of KeyValuePair(Of Integer, IList(Of Object))).Contains
                Return entries.Contains(keyValuePair)
            End Function


            Public Sub CopyTo(ByVal keyValuePairs As KeyValuePair(Of Integer, IList(Of Object))(), ByVal Index As Integer) Implements ICollection(Of KeyValuePair(Of Integer, IList(Of Object))).CopyTo
                Throw New NotImplementedException()
            End Sub

            Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of Integer, IList(Of Object))).Count
                Get
                    Return entries.Count
                End Get
            End Property

            Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of Integer, IList(Of Object))).IsReadOnly
                Get
                    Return True
                End Get
            End Property

            Public Function Remove(ByVal KeyValuePair As KeyValuePair(Of Integer, IList(Of Object))) As Boolean Implements ICollection(Of KeyValuePair(Of Integer, IList(Of Object))).Remove
                Throw New NotSupportedException()
            End Function


            Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of Integer, IList(Of Object))) Implements IEnumerable(Of KeyValuePair(Of Integer, IList(Of Object))).GetEnumerator
                Return Me.entries.GetEnumerator()
            End Function

            Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return Me.GetEnumerator
            End Function


            Public Function [Get](ByVal operator_ As OperatorEnum, ByVal operandIndex As Integer) As Object
                Return [Get](operator_, operandIndex, Nothing)
            End Function

            Public Function [Get](ByVal operator_ As OperatorEnum, ByVal operandIndex As Integer, ByVal defaultValue As Integer?) As Object
                Dim operands As IList(Of Object) = Me(CInt(operator_))
                If (operands IsNot Nothing) Then
                    Return operands(operandIndex)
                Else
                    Return defaultValue
                End If
            End Function

        End Class


        '/**
        '  <summary>Array of variable-sized objects [CFF:1.0:5].</summary>
        '*/
        Private NotInheritable Class Index
            Implements IList(Of Byte())

            Public Shared Function Parse(ByVal data As Byte()) As Index
                Return Parse(New bytes.Buffer(data))
            End Function

            Public Shared Function Parse(ByVal stream As IInputStream) As Index
                Dim data As Byte()() = New Byte(stream.ReadUnsignedShort() - 1)() {}
                Dim offsets As Integer() = New Integer(data.Length + 1 - 1) {}
                Dim offSize As Integer = stream.ReadByte()
                Dim count As Integer = offsets.Length
                For index As Integer = 0 To count - 1
                    offsets(index) = stream.ReadInt(offSize)
                Next
                count = data.Length
                For index As Integer = 0 To count - 1
                    data(index) = New Byte(offsets(index + 1) - offsets(index) - 1) {}
                    stream.Read(data(index))
                Next
                Return New Index(data)
            End Function

            Public Shared Function Parse(ByVal stream As IInputStream, ByVal offset As Integer) As Index
                stream.Position = offset
                Return Parse(stream)
            End Function

            Private ReadOnly data As Byte()()

            Private Sub New(ByVal data As Byte()())
                Me.data = data
            End Sub


            Public Function IndexOf(ByVal item As Byte()) As Integer Implements IList(Of Byte()).IndexOf
                Throw New NotImplementedException()
            End Function

            Public Sub Insert(ByVal Index As Integer, ByVal item As Byte()) Implements IList(Of Byte()).Insert
                Throw New NotSupportedException()
            End Sub

            Public Sub RemoveAt(ByVal Index As Integer) Implements IList(Of Byte()).RemoveAt
                Throw New NotSupportedException()
            End Sub


            Default Public Property Item(ByVal Index As Integer) As Byte() Implements IList(Of Byte()).Item
                Get
                    Return Me.data(Index)
                End Get
                Set(ByVal value As Byte())
                    Throw New NotSupportedException()
                End Set
            End Property

            Public Sub Add(ByVal item As Byte()) Implements ICollection(Of Byte()).Add
                Throw New NotSupportedException()
            End Sub

            Public Sub Clear() Implements ICollection(Of Byte()).Clear
                Throw New NotSupportedException()
            End Sub

            Public Function Contains(ByVal item As Byte()) As Boolean Implements ICollection(Of Byte()).Contains
                Throw New NotImplementedException()
            End Function

            Public Sub CopyTo(ByVal items As Byte()(), ByVal Index As Integer) Implements ICollection(Of Byte()).CopyTo
                Throw New NotImplementedException()
            End Sub

            Public ReadOnly Property Count As Integer Implements ICollection(Of Byte()).Count
                Get
                    Return data.Length
                End Get
            End Property

            Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of Byte()).IsReadOnly
                Get
                    Return True
                End Get
            End Property

            Public Function Remove(ByVal item As Byte()) As Boolean Implements ICollection(Of Byte()).Remove
                Throw New NotSupportedException()
            End Function


            Public Function GetEnumerator() As IEnumerator(Of Byte()) Implements IEnumerable(Of Byte()).GetEnumerator
                '          For (int index = 0, length = Count; index < length; index++)
                '  {yield return Me[index];}
                'Next
                Return New mEnumerator(Of Byte())(Me)
            End Function

            Private Function _GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return Me.GetEnumerator
            End Function

        End Class

        '/**
        '  <summary>Predefined charsets [CFF:1.0:12,C].</summary>
        '*/
        Private Enum StandardCharsetEnum
            ISOAdobe = 0
            Expert = 1
            ExpertSubset = 2
        End Enum

#End Region

#Region "static"
#Region "fields"

        '/**
        '  <summary> Standard charset maps.</summary>
        '*/
        Private Shared ReadOnly StandardCharsets As IDictionary(Of StandardCharsetEnum, IDictionary(Of Integer, Integer))

        '/**
        '  <summary> Standard Strings [CFF:1.0:10] represent commonly occurring strings allocated to
        '  predefined SIDs.</summary>
        '*/
        Private Shared ReadOnly StandardStrings As IList(Of String)

#End Region

#Region "constructors"

        Shared Sub New()
            Dim stream As StreamReader
            StandardCharsets = New Dictionary(Of StandardCharsetEnum, IDictionary(Of Integer, Integer))()
            For Each charset As StandardCharsetEnum In [Enum].GetValues(GetType(StandardCharsetEnum))
                Dim charsetMap As IDictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)()
                '{
                stream = Nothing
                Try
                    ' Open the resource!
                    'stream = New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("fonts.cff." & charset.ToString() & "Charset"))
                    stream = New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(charset.ToString() & "Charset"))
                    ' Parsing the resource...
                    Dim line As String
                    line = stream.ReadLine()
                    While (line IsNot Nothing)
                        Dim lineItems As String() = line.Split(","c)
                        charsetMap(Int32.Parse(lineItems(0))) = GlyphMapping.NameToCode(lineItems(1)).Value
                        line = stream.ReadLine()
                    End While
                Catch e As System.Exception
                    Throw e
                Finally
                    If (stream IsNot Nothing) Then stream.Close()
                End Try
            Next
            '}

            StandardStrings = New List(Of String)
            '{
            stream = Nothing
            Try
                ' Open the resource!
                'stream = New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("fonts.cff.StandardStrings"))
                stream = New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("StandardStrings"))
                ' Parsing the resource...
                Dim line As String
                line = Stream.ReadLine()
                While (line IsNot Nothing)
                    StandardStrings.Add(line)
                    line = Stream.ReadLine()
                End While
            Catch e As System.Exception
                Throw e
            Finally
                If (Stream IsNot Nothing) Then Stream.Close()
            End Try
            '}
        End Sub

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary> Gets the charset corresponding To the given value.</summary>
        '*/
        Private Shared Function GetStandardCharset(ByVal value As Integer?) As StandardCharsetEnum?
            If (Not value.HasValue) Then
                Return StandardCharsetEnum.ISOAdobe
            ElseIf (Not [Enum].IsDefined(GetType(StandardCharsetEnum), value.Value)) Then
                Return Nothing
            Else
                Return CType(value.Value, StandardCharsetEnum)
            End If
        End Function

        Private Overloads Shared Function ToString(ByVal data As Byte()) As String
            Return Charset.ISO88591.GetString(data)
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Public glyphIndexes As IDictionary(Of Integer, Integer)

        Private ReadOnly _fontData As IInputStream
        Private _stringIndex As Index

#End Region

#Region "constructors"

        Friend Sub New(ByVal fontData As IInputStream)
            Me._fontData = fontData
            Load()
        End Sub

#End Region

#Region "interface"
#Region "private"

        '/**
        '  <summary>Loads the font data.</summary>
        '*/
        Private Sub Load()
            Try
                ParseHeader()
                Dim nameIndex As Index = Index.Parse(_fontData)
                Dim topDictIndex As Index = Index.Parse(_fontData)
                _stringIndex = Index.Parse(_fontData)
                '#pragma warning disable 0219
                Dim globalSubrIndex As Index = Index.Parse(_fontData)

                Dim fontName As String = ToString(nameIndex(0))
                '#pragma warning restore 0219
                Dim topDict As Dict = Dict.Parse(topDictIndex(0))

                '      int encodingOffset = topDict.get(Dict.OperatorEnum.Encoding, 0, 0).intValue();
                'TODO: Encoding

                '#pragma warning disable 0219
                Dim charstringType As Integer = CInt(topDict.Get(Dict.OperatorEnum.CharstringType, 0, 2))
                '#pragma warning restore 0219
                Dim charStringsOffset As Integer = CInt(topDict.Get(Dict.OperatorEnum.CharStrings, 0))
                Dim charStringsIndex As Index = Index.Parse(_fontData, charStringsOffset)

                Dim charsetOffset As Integer = CInt(topDict.Get(Dict.OperatorEnum.Charset, 0, 0))
                Dim charset As StandardCharsetEnum? = GetStandardCharset(charsetOffset)
                If (charset.HasValue) Then
                    glyphIndexes = New Dictionary(Of Integer, Integer)(StandardCharsets(charset.Value))
                Else
                    glyphIndexes = New Dictionary(Of Integer, Integer)()
                    _fontData.Position = charsetOffset
                    Dim charsetFormat As Integer = _fontData.ReadByte()
                    Dim count As Integer = charStringsIndex.Count
                    'for (int index = 1, count = charStringsIndex.Count; index <= count;)
                    Dim index As Integer = 1
                    While (index <= count)
                        Select Case (charsetFormat)
                            Case 0
                                glyphIndexes(index) = ToUnicode(_fontData.ReadUnsignedShort()) : index += 1
                                'break;
                            Case 1, 2
                                Dim first As Integer = _fontData.ReadUnsignedShort()
                                Dim nLeft As Integer
                                If (charsetFormat = 1) Then
                                    nLeft = _fontData.ReadByte()
                                Else
                                    nLeft = _fontData.ReadUnsignedShort()
                                End If
                                Dim rangeItemEndIndex As Integer = first + nLeft
                                For rangeItemIndex As Integer = first To rangeItemEndIndex
                                    glyphIndexes(index) = ToUnicode(rangeItemIndex) : index += 1
                                Next
                                'break;
                        End Select
                    End While
                End If
            Catch e As System.Exception
                Throw e
            End Try
        End Sub

        '/**
        '  <summary>Gets the String corresponding To the specified identifier.</summary>
        '  <param name = "id" > SID(String ID).</param>
        '*/
        Private Function GetString(ByVal id As Integer) As String
            If (id < StandardStrings.Count) Then
                Return StandardStrings(id)
            Else
                Return ToString(_stringIndex(id - StandardStrings.Count))
            End If
        End Function


        Private Sub ParseHeader()
            _fontData.Seek(2)
            Dim hdrSize As Integer = _fontData.ReadByte()
            ' Skip to the end of the header!
            _fontData.Seek(hdrSize)
        End Sub

        Private Function ToUnicode(ByVal sid As Integer) As Integer
            '/*
            ' * FIXME: avoid Unicode resolution at this stage -- names should be kept to allow subsequent
            ' * character substitution (see font differences) in case of custom (non-unicode) encodings.
            ' */
            Dim code As Integer? = GlyphMapping.NameToCode(GetString(sid))
            If (Not code.HasValue) Then
                'custom code
                code = sid ' really bad
            End If
            Return code.Value
        End Function

#End Region
#End Region
#End Region

    End Class
End Namespace