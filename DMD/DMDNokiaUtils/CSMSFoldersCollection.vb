Namespace Internals

    ''' <summary>
    ''' Rappresenta la collezione delle cartelle SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CSMSFoldersCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_Device As Nokia.NokiaDevice

        Public Sub New()
            Me.m_Device = Nothing
        End Sub

        Public Sub New(ByVal device As Nokia.NokiaDevice)
            Me.m_Device = device

        End Sub

         

    End Class

End Namespace