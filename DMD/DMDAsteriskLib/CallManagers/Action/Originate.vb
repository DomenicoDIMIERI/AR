Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class Originate
        Inherits AsyncAction

        Private m_Channel As String
        Private m_Context As String
        Private m_Exten As String
        Private m_Priority As Nullable(Of Integer)
        Private m_Timeout As Integer
        Private m_CallerID As String
        Private m_Variable As String
        Private m_Account As String
        Private m_Application As String
        Private m_Data As String
        Private m_Async As Boolean
        
        Public Sub New()
            MyBase.New("Originate")
            Me.m_Channel = ""
            Me.m_Context = ""
            Me.m_Exten = ""
            Me.m_Priority = Nothing
            Me.m_Timeout = 30000
            Me.m_CallerID = ""
            Me.m_Variable = ""
            Me.m_Account = ""
            Me.m_Application = ""
            Me.m_Data = ""
            Me.m_Async = False
        End Sub

        Public Sub New(ByVal channel As String, ByVal callerID As String, ByVal account As String, ByVal variable As String, ByVal context As String, ByVal exten As String, ByVal priority As Integer, ByVal application As String, ByVal data As String, ByVal actionID As String, Optional ByVal async As Boolean = False, Optional ByVal timeout As Integer = 30000)
            Me.New()
            Me.m_Channel = channel
            Me.m_Context = context
            Me.m_Priority = priority
            Me.m_Timeout = timeout
            Me.m_CallerID = callerID
            Me.m_Variable = variable
            Me.m_Account = account
            Me.m_Application = application
            Me.m_Data = data
            Me.m_Async = async
            Me.m_Exten = exten
        End Sub

        Public Sub New(ByVal channel As String, ByVal callerID As String, ByVal account As String, ByVal variable As String, ByVal application As String, ByVal data As String, ByVal actionID As String, Optional ByVal async As Boolean = False, Optional ByVal timeout As Integer = 30000)
            Me.New()
            Me.m_Channel = channel
            Me.m_Context = Context
            Me.m_Priority = Priority
            Me.m_Timeout = timeout
            Me.m_CallerID = callerID
            Me.m_Variable = variable
            Me.m_Account = account
            Me.m_Application = Application
            Me.m_Data = data
            Me.m_Async = async
        End Sub

        ''' <summary>
        ''' Channel on which to originate the call (The same as you specify in the Dial application command)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Channel As String
            Get
                Return Me.m_Channel
            End Get
            Set(value As String)
                Me.m_Channel = value
            End Set
        End Property

        ''' <summary>
        ''' Context to use on connect (must use Exten and Priority with it)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Context As String
            Get
                Return Me.m_Context
            End Get
            Set(value As String)
                Me.m_Context = value
            End Set
        End Property

        ''' <summary>
        ''' Priority to use on connect (must use Context and Exten with it)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priority As Nullable(Of Integer)
            Get
                Return Me.m_Priority
            End Get
            Set(value As Nullable(Of Integer))
                Me.m_Priority = value
            End Set
        End Property

        ''' <summary>
        ''' Timeout (in milliseconds) for the originating connection to happen(defaults to 30000 milliseconds)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Timeout As Integer
            Get
                Return Me.m_Timeout
            End Get
            Set(value As Integer)
                Me.m_Timeout = value
            End Set
        End Property

        ''' <summary>
        ''' CallerID to use for the call
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CallerID As String
            Get
                Return Me.m_CallerID
            End Get
            Set(value As String)
                Me.m_CallerID = value
            End Set
        End Property

        ''' <summary>
        ''' Channels variables to set (max 32). Variables will be set for both channels (local and connected).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Variable As String
            Get
                Return Me.m_Variable
            End Get
            Set(value As String)
                Me.m_Variable = value
            End Set
        End Property

        ''' <summary>
        ''' Account code for the call
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Account As String
            Get
                Return Me.m_Account
            End Get
            Set(value As String)
                Me.m_Account = value
            End Set
        End Property

        ''' <summary>
        ''' Application to use on connect (use Data for parameters)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Application As String
            Get
                Return Me.m_Application
            End Get
            Set(value As String)
                Me.m_Application = value
            End Set
        End Property

        ''' <summary>
        ''' Data if Application parameter is used
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As String
            Get
                Return Me.m_Data
            End Get
            Set(value As String)
                Me.m_Data = value
            End Set
        End Property


        ''' <summary>
        ''' For the origination to be asynchronous (allows multiple calls to be generated without waiting for a response)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Async As Boolean
            Get
                Return Me.m_Async
            End Get
            Set(value As Boolean)
                Me.m_Async = value
            End Set
        End Property

 
        ''' <summary>
        ''' Extension to use on connect (must use Context and Priority with it)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Exten As String
            Get
                Return Me.m_Exten
            End Get
            Set(value As String)
                Me.m_Exten = value
            End Set
        End Property

        Protected Overrides Function GetCommandText() As String
            Dim cmd As String = MyBase.GetCommandText()
            cmd &= "Channel: " & Me.Channel & vbCrLf
            If (Me.Context <> "" OrElse Me.Exten <> "" OrElse Me.Priority.HasValue) Then
                cmd &= "Context: " & Me.Context & vbCrLf
                cmd &= "Exten: " & Me.Exten & vbCrLf
                cmd &= "Priority: " & Me.Priority & vbCrLf
            End If
            cmd &= "Timeout: " & Me.Timeout & vbCrLf
            If (Me.CallerID <> "") Then cmd &= "Callerid: " & Me.CallerID & vbCrLf
            If (Me.Variable <> "") Then cmd &= "Variable: " & Me.Variable & vbCrLf
            If (Me.Account <> "") Then cmd &= "Account: " & Me.Account & vbCrLf
            If (Me.Application <> "" OrElse Me.Data <> "") Then
                cmd &= "Application: " & Me.Application & vbCrLf
                cmd &= "Data: " & Me.Data & vbCrLf
            End If
            If (Me.m_Async) Then cmd &= "Async: yes" & vbCrLf
            Return cmd
        End Function



        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New OriginateResponse(Me)
        End Function


    End Class

End Namespace