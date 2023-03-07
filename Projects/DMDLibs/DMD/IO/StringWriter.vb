Namespace Io

    Public Class StringWriter
        Inherits Writer
        Implements IDisposable


        Private m_Base As New System.IO.StringWriter

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub


        Public Overrides Sub close()
            Me.m_Base.Close()
        End Sub

        Public Overrides Sub flush()
            Me.m_Base.Flush()
        End Sub

        Public Overloads Overrides Sub write(cbuf() As Char, off As Integer, len As Integer)
            Me.m_Base.Write(cbuf, off, len)
        End Sub



        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            If (Me.m_Base IsNot Nothing) Then Me.m_Base.Dispose() : Me.m_Base = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace