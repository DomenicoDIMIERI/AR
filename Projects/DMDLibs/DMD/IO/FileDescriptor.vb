Namespace Io

    Public Structure FileDescriptor
        Public handle As IntPtr

        Public Sub New(ByVal h As IntPtr)
            Me.handle = h
        End Sub

        Public Shared Widening Operator CType(ByVal h As IntPtr) As FileDescriptor
            Return New FileDescriptor(h)
        End Operator

        Public Shared Narrowing Operator CType(ByVal h As FileDescriptor) As IntPtr
            Return h.handle
        End Operator

    End Structure

End Namespace