Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading


Partial Public Class Keyboard

    <Serializable> _
    Public Class KeyboardEventArgs
        Inherits System.EventArgs

        Private m_Key As VirtualKeys
        Private m_scanCode As Integer
        Private m_Flags As Integer
        Private m_ExtraFlags As Integer
        Private m_Char As Char

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal key As VirtualKeys, ByVal scanCode As Integer, ByVal flags As Integer, ByVal extraFlags As Integer, ByVal [char] As Char)
            Me.New
            Me.m_Key = key
            Me.m_scanCode = scanCode
            Me.m_ExtraFlags = extraFlags
            Me.m_Flags = flags
            Me.m_Char = [char]
        End Sub

        Public ReadOnly Property Key As VirtualKeys
            Get
                Return Me.m_Key
            End Get
        End Property

        Public ReadOnly Property IsKeyUp As Boolean
            Get
                Return (Me.m_Flags And KBDLLFlags.LLKHF_UP) = KBDLLFlags.LLKHF_UP
            End Get
        End Property

        Public ReadOnly Property IsKeyDown As Boolean
            Get
                Return Not Me.IsKeyUp
            End Get
        End Property

        Public ReadOnly Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property ScanCode As Integer
            Get
                Return Me.m_scanCode
            End Get
        End Property

        Public ReadOnly Property ExtraFlags As Integer
            Get
                Return Me.m_ExtraFlags
            End Get
        End Property

        Public ReadOnly Property [Char] As Char '(ByVal code As VirtualKeys, ByVal scanCode As Integer, ByVal flags As Integer, ByVal modifiers As Integer) As String
            Get
                Return Me.m_Char
            End Get
        End Property

        Function IsPrintable() As Boolean
            Return Char.IsLetterOrDigit(Me.m_Char) OrElse Char.IsWhiteSpace(Me.m_Char) OrElse (Me.m_Char = vbCr) OrElse (Me.m_Char = vbLf) OrElse (Me.m_Char = vbTab)
        End Function

        Private Function GetKeyName() As String
            Try
                Return [Enum].GetName(GetType(VirtualKeys), Me.Key)
            Catch ex As Exception
                Return "?"
            End Try
        End Function

        Public Overrides Function ToString() As String
            If (Me.IsKeyDown) Then
                Return "KEY_DOWN: " & Right("0000" & Hex(Me.ScanCode), 4) & " : " & Me.Char & " - " & Me.GetKeyName()
            Else
                Return "KEY_UP  : " & Right("0000" & Hex(Me.ScanCode), 4) & " : " & Me.Char & " - " & Me.GetKeyName()
            End If
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class

