Imports System.Runtime.InteropServices
Imports System.Drawing.Text
Imports FinSeA.Sistema

Namespace Drawings

    Public Enum JFontFormat As Integer
        TRUETYPE_FONT = 1
        TYPE1_FONT = 2
    End Enum

    <Serializable> _
    Public Class JFont
        Implements IDisposable


        Private Shared fontCollection As New PrivateFontCollection

        Private m_Font As System.Drawing.Font

        Public Sub New(ByVal fontName As String, ByVal emSize As Single)
            Me.m_Font = New System.Drawing.Font(fontName, emSize)
        End Sub

        Public Sub New(ByVal family As FontFamily, ByVal emSize As Single)
            Me.m_Font = New System.Drawing.Font(family, emSize)
        End Sub

        Function getName() As String
            Return Me.m_Font.Name
        End Function

        Function canDisplayUpTo([string] As String) As Integer
            Throw New NotImplementedException
        End Function

        Function getFamily() As String
            Return Me.m_Font.FontFamily.Name
        End Function

        Function getPSName() As String
            Return Me.m_Font.OriginalFontName
        End Function

        Function IsBold() As Boolean
            Return Me.m_Font.Bold
        End Function

        Function createGlyphVector(frc As FontRenderContext, codePoints As Integer()) As GlyphVector
            Throw New NotImplementedException
        End Function

        Function createGlyphVector(frc As FontRenderContext, [string] As String) As GlyphVector
            Throw New NotImplementedException
        End Function

        Function isItalic() As Boolean
            Return Me.m_Font.Italic
        End Function

        ''' <summary>
        ''' Checks if this Font has a glyph for the specified character.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <remarks></remarks>
        Public Function canDisplay(ByVal c As Char) As Boolean
            Return True
        End Function

        ''' <summary>
        ''' Checks if this Font has a glyph for the specified character.
        ''' </summary>
        ''' <param name="codePoint"></param>
        ''' <remarks></remarks>
        Public Function canDisplay(ByVal codePoint As Integer) As Boolean
            Return True
        End Function

        ''' <summary>
        ''' Creates a new Font object by replicating the current Font object and applying a new size to it.
        ''' </summary>
        ''' <param name="newSize"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function deriveFont(ByVal newSize As Single) As JFont
            Return New JFont(Me.m_Font.Name, newSize)
        End Function

        ''' <summary>
        '''  Returns a new Font using the specified font type and input data.
        ''' </summary>
        ''' <param name="fontFormat"></param>
        ''' <param name="inputStream"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function createFont(fontFormat As JFontFormat, inputStream As Io.InputStream) As JFont
            Throw New NotImplementedException
        End Function


        Shared Function decode(p1 As Object) As JFont
            Throw New NotImplementedException
        End Function

        Public Shared Function LoadFontFamily(ByVal fileName As String) As FontFamily
            '   fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(fileName)
            Return fontCollection.Families(0)
        End Function

        Public Shared Function LoadFontFamily(ByVal buffer() As Byte) As FontFamily
            'pin array so we can get its address
            Dim handle As GCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
            Try
                Dim ptr As System.IntPtr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0)
                'fontCollection = New PrivateFontCollection()
                fontCollection.AddMemoryFont(ptr, buffer.Length)
                Return fontCollection.Families(0)
            Finally
                ' don't forget to unpin the array!
                handle.Free()
            End Try
        End Function

        Public Shared Function LoadFontFamily(ByVal stream As System.IO.Stream) As FontFamily
            Dim buffer() As Byte = Arrays.CreateInstance(Of Byte)(stream.Length)
            stream.Read(buffer, 0, buffer.Length)
            Dim ret As FontFamily = LoadFontFamily(buffer)
            Return ret
        End Function

        Public Shared Function GetAllFontsAsArray() As JFont()
            Dim installed_fonts As New InstalledFontCollection
            Dim ret As New System.Collections.ArrayList
            'Dim font_families() As FontFamily = installed_fonts.Families() '' Get an array of the system's font familiies.
            For Each f As FontFamily In installed_fonts.Families
                ret.Add(New JFont(f, 8))
            Next
            Return ret.ToArray(GetType(JFont))
        End Function


        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            If (Me.m_Font IsNot Nothing) Then Me.m_Font.Dispose() : Me.m_Font = Nothing
        End Sub


    End Class

End Namespace