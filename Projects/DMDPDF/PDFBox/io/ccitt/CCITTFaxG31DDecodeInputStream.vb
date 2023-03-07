Imports FinSeA.Exceptions
Imports System.IO
Imports FinSeA.org.apache.pdfbox.io.ccitt.CCITTFaxConstants

Namespace org.apache.pdfbox.io.ccitt

    '/**
    ' * This is a CCITT Group 3 1D decoder (ITU T.4).
    ' * @version $Revision$
    ' */
    Public Class CCITTFaxG31DDecodeInputStream
        Inherits FinSeA.Io.InputStream

        Private Shared ReadOnly CODE_WORD As Integer = 0
        Private Shared ReadOnly SIGNAL_EOD As Integer = -1
        Private Shared ReadOnly SIGNAL_EOL As Integer = -2

        Private source As Stream
        Private columns As Integer
        Private rows As Integer

        'for reading compressed bits
        Private bits As Integer
        Private bitPos As Integer = 8

        'a single decoded line (one line decoded at a time, then read byte by byte)
        Private decodedLine As PackedBitArray
        Private decodedWritePos As Integer 'write position in bits (used by the decoder algorithm)
        Private decodedReadPos As Integer 'read position in bytes (used by the actual InputStream reading)

        'state
        Private y As Integer = -1 'Current row/line
        Private accumulatedRunLength As Integer 'Used for make-up codes

        Private Shared WHITE_LOOKUP_TREE_ROOT As New NonLeafLookupTreeNode
        Private Shared BLACK_LOOKUP_TREE_ROOT As New NonLeafLookupTreeNode

        Shared Sub New()
            'WHITE_LOOKUP_TREE_ROOT = new NonLeafLookupTreeNode();
            'BLACK_LOOKUP_TREE_ROOT = new NonLeafLookupTreeNode();
            buildLookupTree()
        End Sub

        '/**
        ' * Creates a new decoder.
        ' * @param source the input stream containing the compressed data.
        ' * @param columns the number of columns
        ' * @param rows the number of rows (0 if undefined)
        ' */
        Public Sub New(ByVal source As Stream, ByVal columns As Integer, ByVal rows As Integer)
            Me.source = source
            Me.columns = columns
            Me.rows = rows
            Me.decodedLine = New PackedBitArray(columns)
            Me.decodedReadPos = Me.decodedLine.getByteCount()
        End Sub

        '/**
        ' * Creates a new decoder.
        ' * @param source the input stream containing the compressed data.
        ' * @param columns the number of columns
        ' */
        Public Sub New(ByVal source As Stream, ByVal columns As Integer)
            Me.New(source, columns, 0)
        End Sub

        '/** {@inheritDoc} */
        Public Overrides Function markSupported() As Boolean
            Return False
        End Function

        ' /** {@inheritDoc} */
        Public Overloads Function read() As Integer ' throws IOException
            If (Me.decodedReadPos >= Me.decodedLine.getByteCount()) Then
                Dim hasLine As Boolean = decodeLine()
                If (Not hasLine) Then
                    Return -1
                End If
            End If
            Dim data As Byte = Me.decodedLine.getData()(Me.decodedReadPos) : Me.decodedReadPos += 1

            'System.out.println("Returning " + PackedBitArray.visualizeByte(data));
            Return data And &HFF
        End Function

        'TODO Implement the other two read methods

        Private Function decodeLine() As Boolean ' throws IOException
            If (Me.bits < 0) Then
                Return False 'Shortcut after EOD
            End If
            Me.y += 1
            'System.out.println("decodeLine " + Me.y);
            Dim x As Integer = 0
            If (Me.rows > 0 AndAlso Me.y >= Me.rows) Then
                Return False 'All rows decoded, ignore further bits
            End If
            Me.decodedLine.clear()
            Me.decodedWritePos = 0
            Dim expectRTC As Integer = 6
            Dim white As Boolean = True
            While (x < Me.columns OrElse Me.accumulatedRunLength > 0)
                Dim code As CodeWord
                Dim root As LookupTreeNode
                If (white) Then
                    root = WHITE_LOOKUP_TREE_ROOT
                Else
                    root = BLACK_LOOKUP_TREE_ROOT
                End If
                code = root.getNextCodeWord(Me)
                If (code Is Nothing) Then
                    'no more code words (EOD)
                    If (x > 0) Then
                        'Have last line
                        Me.decodedReadPos = 0
                        Return True
                    Else
                        Return False
                    End If
                ElseIf (code.getType() = SIGNAL_EOL) Then
                    expectRTC -= 1
                    If (expectRTC = 0) Then
                        'System.out.println("Return to Control");
                        Return False 'Return to Control = End Of Data
                    End If
                    If (x = 0) Then
                        'System.out.println("Ignoring leading EOL");
                        Continue While
                    End If
                Else
                    expectRTC = -1
                    x += code.execute(Me)
                    If (Me.accumulatedRunLength = 0) Then
                        'Only switch if not using make-up codes
                        white = Not white
                    End If
                End If
            End While
            Me.decodedReadPos = 0
            Return True
        End Function

        Private Sub writeRun(ByVal bit As Integer, ByVal length As Integer)
            Me.accumulatedRunLength += length

            'System.out.println(" Run " + bit + " for " + Me.accumulatedRunLength + " at " + decodedWritePos);
            If (bit <> 0) Then
                Me.decodedLine.setBits(Me.decodedWritePos, Me.accumulatedRunLength)
            End If
            Me.decodedWritePos += Me.accumulatedRunLength
            Me.accumulatedRunLength = 0
        End Sub

        Private Sub writeNonTerminating(ByVal length As Integer)
            'System.out.println(" Make up code for " + length + " bits");
            Me.accumulatedRunLength += length
        End Sub

        Private Shared ReadOnly BIT_POS_MASKS() As Integer = {&H80, &H40, &H20, &H10, &H8, &H4, &H2, &H1}

        Private Function readBit() As Integer ' throws IOException
            If (Me.bitPos >= 8) Then
                readByte()
                If (Me.bits < 0) Then
                    Return SIGNAL_EOD
                End If
            End If
            Dim bit As Integer = IIf(Me.bits And BIT_POS_MASKS(Me.bitPos) = 0, 0, 1) : Me.bitPos += 1
            'System.out.print(bit);
            Return bit
        End Function

        Private Shadows Sub readByte()  ' throws IOException
            Me.bits = Me.source.ReadByte()
            Me.bitPos = 0
        End Sub

        Private Shared ReadOnly EOL_STARTER As Integer = &HB00

        Private Shared Sub buildLookupTree()
            buildUpTerminating(WHITE_TERMINATING, WHITE_LOOKUP_TREE_ROOT, True)
            buildUpTerminating(BLACK_TERMINATING, BLACK_LOOKUP_TREE_ROOT, False)
            buildUpMakeUp(WHITE_MAKE_UP, WHITE_LOOKUP_TREE_ROOT)
            buildUpMakeUp(BLACK_MAKE_UP, BLACK_LOOKUP_TREE_ROOT)
            buildUpMakeUpLong(LONG_MAKE_UP, WHITE_LOOKUP_TREE_ROOT)
            buildUpMakeUpLong(LONG_MAKE_UP, BLACK_LOOKUP_TREE_ROOT)
            Dim eolNode As New EndOfLineTreeNode()
            addLookupTreeNode(EOL_STARTER, WHITE_LOOKUP_TREE_ROOT, eolNode)
            addLookupTreeNode(EOL_STARTER, BLACK_LOOKUP_TREE_ROOT, eolNode)
        End Sub

        Private Shared Sub buildUpTerminating(ByVal codes() As Integer, ByVal root As NonLeafLookupTreeNode, ByVal white As Boolean)
            Dim c As Integer = codes.Length
            For len As Integer = 0 To c - 1
                Dim leaf As New RunLengthTreeNode(IIf(white, 0, 1), len)
                addLookupTreeNode(codes(len), root, leaf)
            Next
        End Sub

        Private Shared Sub buildUpMakeUp(ByVal codes() As Integer, ByVal root As NonLeafLookupTreeNode)
            Dim c As Integer = codes.Length
            For len As Integer = 0 To c - 1
                Dim leaf As New MakeUpTreeNode((len + 1) * 64)
                addLookupTreeNode(codes(len), root, leaf)
            Next
        End Sub

        Private Shared Sub buildUpMakeUpLong(ByVal codes() As Integer, ByVal root As NonLeafLookupTreeNode)
            Dim c As Integer = codes.Length
            For len As Integer = 0 To c - 1
                Dim leaf As New MakeUpTreeNode((len + 28) * 64)
                addLookupTreeNode(codes(len), root, leaf)
            Next
        End Sub

        Private Shared Sub addLookupTreeNode(ByVal code As Integer, ByVal root As NonLeafLookupTreeNode, ByVal leaf As LookupTreeNode)
            Dim codeLength As Integer = code >> 8
            Dim pattern As Integer = code And &HFF
            Dim node As NonLeafLookupTreeNode = root
            For p As Integer = codeLength - 1 To 1 Step -1
                Dim bit1 As Integer = (pattern >> p) And &H1
                Dim child As LookupTreeNode = node.get(bit1)
                If (child Is Nothing) Then
                    child = New NonLeafLookupTreeNode()
                    node.set(bit1, child)
                End If
                If (TypeOf (child) Is NonLeafLookupTreeNode) Then
                    node = child
                Else
                    Throw New IllegalStateException("NonLeafLookupTreeNode expected, was " & child.GetType().FullName)
                End If
            Next
            Dim bit As Integer = pattern And &H1
            If (node.get(bit) IsNot Nothing) Then
                Throw New IllegalStateException("Two codes conflicting in lookup tree")
            End If
            node.set(bit, leaf)
        End Sub

        ''' <summary>
        ''' Base class for all nodes in the lookup tree for code words.
        ''' </summary>
        ''' <remarks></remarks>
        Private MustInherit Class LookupTreeNode

            Public MustOverride Function getNextCodeWord(ByVal decoder As CCITTFaxG31DDecodeInputStream) As CodeWord 'throws IOException;

        End Class

        ''' <summary>
        ''' Interface for code words.
        ''' </summary>
        ''' <remarks></remarks>
        Private Interface CodeWord

            Function [getType]() As Integer
            Function execute(ByVal decoder As CCITTFaxG31DDecodeInputStream) As Integer ' throws IOException;

        End Interface

        ''' <summary>
        ''' Non-leaf nodes that hold a child node for both the 0 and 1 cases for the lookup tree.
        ''' </summary>
        ''' <remarks></remarks>
        Private Class NonLeafLookupTreeNode
            Inherits LookupTreeNode

            Private zero As LookupTreeNode
            Private one As LookupTreeNode

            Public Sub [set](ByVal bit As Integer, ByVal node As LookupTreeNode)
                If (bit = 0) Then
                    Me.zero = node
                Else
                    Me.one = node
                End If
            End Sub

            Public Function [get](ByVal bit As Integer) As LookupTreeNode
                Return IIf(bit = 0, Me.zero, Me.one)
            End Function

            Public Overrides Function getNextCodeWord(ByVal decoder As CCITTFaxG31DDecodeInputStream) As CodeWord 'throws IOException
                Dim bit As Integer = decoder.readBit()
                If (bit < 0) Then
                    Return Nothing
                End If
                Dim node As LookupTreeNode = [get](bit)
                If (node IsNot Nothing) Then
                    Return node.getNextCodeWord(decoder)
                End If
                Throw New IOException("Invalid code word encountered")
            End Function

        End Class

        ''' <summary>
        ''' This node represents a run length of either 0 or 1.
        ''' </summary>
        ''' <remarks></remarks>
        Private Class RunLengthTreeNode
            Inherits LookupTreeNode
            Implements CodeWord

            Private bit As Integer
            Private length As Integer

            Public Sub New(ByVal bit As Integer, ByVal length As Integer)
                Me.bit = bit
                Me.length = length
            End Sub

            Public Overrides Function getNextCodeWord(ByVal decoder As CCITTFaxG31DDecodeInputStream) As CodeWord 'throws IOException
                Return Me
            End Function

            Public Function execute(ByVal decoder As CCITTFaxG31DDecodeInputStream) As Integer Implements CodeWord.execute
                decoder.writeRun(Me.bit, Me.length)
                Return length
            End Function

            Public Shadows Function [getType]() As Integer Implements CodeWord.getType
                Return CODE_WORD
            End Function

            Public Overrides Function toString() As String
                Return "Run Length for " & length & " bits of " & IIf(bit = 0, "white", "black")
            End Function

        End Class

        ''' <summary>
        ''' Represents a make-up code word. 
        ''' </summary>
        ''' <remarks></remarks>
        Private Class MakeUpTreeNode
            Inherits LookupTreeNode
            Implements CodeWord

            Private length As Integer

            Public Sub New(ByVal length As Integer)
                Me.length = length
            End Sub

            Public Overrides Function getNextCodeWord(ByVal decoder As CCITTFaxG31DDecodeInputStream) As CodeWord 'throws IOException
                Return Me
            End Function

            Public Function execute(ByVal decoder As CCITTFaxG31DDecodeInputStream) As Integer Implements CodeWord.execute 'throws IOException
                decoder.writeNonTerminating(length)
                Return length
            End Function

            Public Function getNodeType() As Integer Implements CodeWord.[getType]
                Return CODE_WORD
            End Function

            Public Overrides Function toString() As String
                Return "Make up code for length " & length
            End Function

        End Class

        ''' <summary>
        ''' Represents an EOL code word. 
        ''' </summary>
        ''' <remarks></remarks>
        Private Class EndOfLineTreeNode
            Inherits LookupTreeNode
            Implements CodeWord

            Public Overrides Function getNextCodeWord(ByVal decoder As CCITTFaxG31DDecodeInputStream) As CodeWord ' throws IOException
                Dim bit As Integer
                Do
                    bit = decoder.readBit()
                Loop While (bit = 0) 'bit 1 finishes the EOL, any number of bit 0 allowed as fillers
                If (bit < 0) Then
                    Return Nothing
                End If
                Return Me
            End Function

            Public Function execute(ByVal decoder As CCITTFaxG31DDecodeInputStream) As Integer Implements CodeWord.execute ' throws IOException
                'nop
                Return 0
            End Function

            Public Function getCodeType() As Integer Implements CodeWord.getType
                Return SIGNAL_EOL
            End Function

            Public Overrides Function toString() As String
                Return "EOL"
            End Function

        End Class

        Public Overrides ReadOnly Property CanRead As Boolean
            Get
                Return Me.source.CanRead
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek As Boolean
            Get
                Return Me.source.CanSeek
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite As Boolean
            Get
                Return Me.source.CanWrite
            End Get
        End Property

        Public Overrides Sub Flush()
            Me.source.Flush()
        End Sub

        Public Overrides ReadOnly Property Length As Long
            Get
                Return Me.source.Length
            End Get
        End Property

        Public Overrides Property Position As Long
            Get
                Return Me.source.Position
            End Get
            Set(value As Long)
                Me.source.Position = value
            End Set
        End Property

        Public Overloads Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
            Return Me.source.Read(buffer, offset, count)
        End Function

        Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
            Return Me.source.Seek(offset, origin)
        End Function

        Public Overrides Sub SetLength(value As Long)
            Me.source.SetLength(value)
        End Sub

        Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
            Me.source.Write(buffer, offset, count)
        End Sub
    End Class

End Namespace
