Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.Asterisk

Namespace CallManagers.Responses

    Public Class OriginateResponse
        Inherits ActionResponse

        Private m_Channel As String
        Private m_Context As String
        Private m_Exten As String
        Private m_Reason As String
        Private m_Uniqueid As String
        Private m_CallerIDNum As String
        Private m_CallerIDName As String

        Public Sub New()
            Me.m_Channel = ""
            Me.m_Context = ""
            Me.m_Exten = ""
            Me.m_Reason = ""
            Me.m_Uniqueid = ""
            Me.m_CallerIDNum = ""
            Me.m_CallerIDName = ""
        End Sub

        Public Sub New(ByVal action As Action)
            MyBase.New(action)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.m_Channel
            End Get
        End Property

        Public ReadOnly Property Context As String
            Get
                Return Me.m_Context
            End Get
        End Property

        Public ReadOnly Property Exten As String
            Get
                Return Me.m_Exten
            End Get
        End Property

        Public ReadOnly Property Reason As String
            Get
                Return Me.m_Reason
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.m_Uniqueid
            End Get
        End Property

        Public ReadOnly Property CallerIDNum As String
            Get
                Return Me.m_CallerIDNum
            End Get
        End Property

        Public ReadOnly Property CallerIDName As String
            Get
                Return Me.m_CallerIDName
            End Get
        End Property
            
        Protected Overrides Sub ParseRow(row As RowEntry)
            Select Case UCase(row.Command)
                Case "CHANNEL" : Me.m_Channel = row.Params
                Case "CONTEXT" : Me.m_Context = row.Params
                Case "EXTEN" : Me.m_Exten = row.Params
                Case "REASON" : Me.m_Reason = row.Params
                Case "UNIQUEID" : Me.m_Uniqueid = row.Params
                Case "CALLERIDNUM" : Me.m_CallerIDNum = row.Params
                Case "CALLERIDNAME" : Me.m_CallerIDName = row.Params
                Case Else : MyBase.ParseRow(row)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String = MyBase.ToString()
            ret &= "Channel: " & Me.Channel & vbCrLf
            ret &= "Context: " & Me.Context & vbCrLf
            ret &= "Exten: " & Me.Exten & vbCrLf
            ret &= "Reason: " & Me.Reason & vbCrLf
            ret &= "UniqueID: " & Me.UniqueID & vbCrLf
            ret &= "CallerIDNum: " & Me.CallerIDNum & vbCrLf
            ret &= "CallerIDName: " & Me.CallerIDName & vbCrLf
            Return ret
        End Function


    End Class

End Namespace