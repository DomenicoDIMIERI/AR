Namespace Internals

    ''' <summary>
    ''' Rappresenta la collezione dei dispositivi Nokia connessi al PC
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CDevicesCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Public Sub New()
        End Sub

        Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As Nokia.NokiaDevice
            Get
                Return DirectCast(MyBase.InnerList.Item(index), Nokia.NokiaDevice)
            End Get
        End Property

        Public Function GetItemBySerialNumber(ByVal serial As String) As Nokia.NokiaDevice
            SyncLock Me
                For Each dev As Nokia.NokiaDevice In Me
                    If dev.SerialNumber = serial Then Return dev
                Next
                Return Nothing
            End SyncLock
        End Function

        Friend Sub Remove(ByVal dev As Nokia.NokiaDevice)
            MyBase.InnerList.Remove(dev)
        End Sub

        Friend Sub Add(ByVal dev As Nokia.NokiaDevice)
            MyBase.InnerList.Add(dev)
        End Sub
    End Class

End Namespace