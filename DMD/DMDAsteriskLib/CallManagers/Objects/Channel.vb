Namespace CallManagers

    Public Class Channel
        Inherits IDObject

        Private m_State As String
        Private m_Channel As String
        Private m_CallerID As String
        Private m_CallerIDNum As String
        Private m_CallerIDName As String

        Public Sub New()
            'DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal e As Events.Newchannel)
            Me.New
            Me.m_State = e.State
            Me.m_Channel = e.Channel
            Me.m_CallerID = e.CallerID
            Me.m_CallerIDNum = e.CallerIDNum
            Me.m_CallerIDName = e.CallerIDName
            Me.UniqueID = e.UniqueID
        End Sub

        Public Property State As String
            Get
                Return Me.m_State
            End Get
            Set(value As String)
                Me.m_State = Trim(value)
            End Set
        End Property

        Public Property Channel As String
            Get
                Return Me.m_Channel
            End Get
            Set(value As String)
                Me.m_Channel = Trim(value)
            End Set
        End Property

        Public Property CallerID As String
            Get
                Return Me.m_CallerID
            End Get
            Set(value As String)
                Me.m_CallerID = Trim(value)
            End Set
        End Property

        Public Property CallerIDNum As String
            Get
                Return Me.m_CallerIDNum
            End Get
            Set(value As String)
                Me.m_CallerIDNum = Trim(value)
            End Set
        End Property

        Public Property CallerIDName As String
            Get
                Return Me.m_CallerIDName
            End Get
            Set(value As String)
                Me.m_CallerIDName = Trim(value)
            End Set
        End Property

        Public Sub Hangup()
            Debug.Print("Canale chiuso: " & Me.Channel)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            ' DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace