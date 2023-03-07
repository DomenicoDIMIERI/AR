Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Oggetto base contenuto in una cartella
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class CBaseItem
        Inherits CBaseObject
        Implements IDisposable

        Friend m_ParentFolder As CBaseFolder
        Friend UID As CAItemID

        Public Sub New()
            Me.m_ParentFolder = Nothing
        End Sub

        Public Sub New(ByVal folder As CBaseFolder)
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")
            Me.SetDevice(folder.Device)
            Me.m_ParentFolder = folder
        End Sub

        ''' <summary>
        ''' Restituisce la cartella contenitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ParentFolder As CBaseFolder
            Get
                Return Me.m_ParentFolder
            End Get
        End Property

        Protected Friend Overridable Sub SetParentFolder(ByVal value As CBaseFolder)
            Me.m_ParentFolder = value
        End Sub

        Public Overrides ReadOnly Property Device As NokiaDevice
            Get
                If (Me.m_ParentFolder IsNot Nothing) Then Return Me.m_ParentFolder.Device
                Return MyBase.Device
            End Get
        End Property

        ''' <summary>
        ''' Metodo che elimina l'oggetto dal dispositivo
        ''' </summary>
        Protected MustOverride Sub InternalDelete()


        Public Sub Delete()
            Me.InternalDelete()
            Me.ParentFolder.NotifyDeleted(Me)
        End Sub


        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            Me.m_Device = Nothing
            Me.m_ParentFolder = Nothing
        End Sub


    End Class

End Namespace