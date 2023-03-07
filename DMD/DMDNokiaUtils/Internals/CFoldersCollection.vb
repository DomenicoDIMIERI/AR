Namespace Internals

    ''' <summary>
    ''' Rappresenta la collezione delle cartelle SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CFoldersCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_Parent As CBaseFolder
        Private m_Device As Nokia.NokiaDevice

        Public Sub New()
            Me.m_Device = Nothing
            Me.m_Parent = Nothing
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device
        End Sub

        Friend Sub New(ByVal parent As CBaseFolder)
            Me.New()
            If (parent Is Nothing) Then Throw New ArgumentNullException("Parent")
            Me.SetDevice(parent.Device)
            Me.SetParent(parent)
        End Sub

        Public ReadOnly Property Device As Nokia.NokiaDevice
            Get
                If (Me.m_Parent IsNot Nothing) Then Return Me.m_Parent.Device
                Return Me.m_Device
            End Get
        End Property

        Public ReadOnly Property Parent As CBaseFolder
            Get
                Return Me.m_Parent
            End Get
        End Property

        Protected Friend Sub SetParent(ByVal parent As CBaseFolder)
            Me.m_Parent = parent
        End Sub

        Protected Friend Sub SetDevice(ByVal device As Nokia.NokiaDevice)
            Me.m_Device = device
        End Sub

        Friend Sub InternalAdd(ByVal item As CBaseFolder)
            Me.InnerList.Add(item)
        End Sub

        Friend Sub Remove(ByVal item As CBaseItem)
            Me.InnerList.Remove(item)
        End Sub

    End Class

End Namespace