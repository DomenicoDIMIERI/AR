Imports DMD.Internals

Partial Class Nokia

    ''' <summary>
    ''' Rappresenta una cartella che contiene SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CMMSFolder
        Private m_Name As String
        Private m_Device As Nokia.NokiaDevice
        Private m_Messages As CSMSMessagesCollection
        Private m_SubFolders As CSMSFoldersCollection
        Friend folderInfo As CA_FOLDER_INFO

        Public Sub New()
            Me.m_Name = ""
            Me.m_Device = Nothing
            Me.m_Messages = Nothing
        End Sub

        Public Sub New(ByVal device As Nokia.NokiaDevice, ByVal name As String)
            Me.New()
            Me.m_Name = name
            Me.m_Device = device
        End Sub

        ''' <summary>
        ''' Restituisce il device
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Device As Nokia.NokiaDevice
            Get
                Return Me.m_Device
            End Get
        End Property

        ''' <summary>
        ''' Restituisce i messaggi memorizzati nella cartella
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Messages As CSMSMessagesCollection
            Get
                SyncLock Me.m_Device
                    If (Me.m_Messages Is Nothing) Then Me.m_Messages = New CSMSMessagesCollection(Me)
                    Return Me.m_Messages
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'elenco delle sottocartelle
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SubFolders As CSMSFoldersCollection
            Get
                SyncLock Me.m_Device
                    If (Me.m_SubFolders Is Nothing) Then Me.m_SubFolders = New CSMSFoldersCollection(Me)
                    Return Me.m_SubFolders
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce i lnome della cartella
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name As String
            Get
                Return Me.m_Name
            End Get
        End Property

        Protected Overridable Sub GetSMSFolders()

        End Sub
    End Class

End Class