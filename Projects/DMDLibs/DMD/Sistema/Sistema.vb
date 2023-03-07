Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports System.Runtime.Serialization.Formatters.Binary
Imports DMD
Imports DMD.Databases

Public NotInheritable Class Sistema
    Private Shared m_Module As CModule

    'Public Const PWDMINLEN As Integer = 6     'Lunghezza minima di una password
    Public Const DEFAULT_PASSWORD_INTERVAL = 60 '60 days
    Public Const SESSIONTIMEOUT As Integer = 120

    Private Sub New()
    End Sub


    Private Shared m_ApplicationContext As IApplicationContext

    Public Shared ReadOnly Property ApplicationContext As IApplicationContext
        Get
            Return m_ApplicationContext
        End Get
    End Property

    Public Shared Sub SetApplicationContext(ByVal value As IApplicationContext)
        m_ApplicationContext = value
    End Sub



    ''' <summary>
    ''' Inizializza la libreria
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub Initialize()
        If Not Groups.KnownGroups.Administrators.Members.Contains(Users.KnownUsers.GlobalAdmin) Then
            Groups.KnownGroups.Administrators.Members.Add(Users.KnownUsers.GlobalAdmin)
        End If
        If Not Groups.KnownGroups.Guests.Members.Contains(Users.KnownUsers.GuestUser) Then
            Groups.KnownGroups.Guests.Members.Add(Users.KnownUsers.GuestUser)
        End If

        Sistema.Types.Initialize

    End Sub


#Region "Settings"

    Public NotInheritable Class Settings



        'Minimum eight characters, at least one letter And one number:

        '"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"

        'Minimum eight characters, at least one letter, one number And one special character:

        '"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"

        'Minimum eight characters, at least one uppercase letter, one lowercase letter And one number:

        '"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$"

        'Minimum eight characters, at least one uppercase letter, one lowercase letter, one number And one special character:

        '"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}"

        'Minimum eight And maximum 10 characters, at least one uppercase letter, one lowercase letter, one number And one special character:

        '"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,10}"



        Private Shared m_PWDPATTERN As String = "^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
        Private Shared m_PWDMINLEN As Integer = 8

        Private Sub New()

        End Sub

        Public Shared Property PWDMINLEN As Integer
            Get
                Return m_PWDMINLEN
            End Get
            Set(value As Integer)
                If (value = m_PWDMINLEN) Then Exit Property
                m_PWDMINLEN = value
            End Set
        End Property

        Public Shared Property PWDPATTERN As String
            Get
                Return m_PWDPATTERN
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                If (m_PWDPATTERN = value) Then Return
                m_PWDPATTERN = value
            End Set
        End Property

    End Class

#End Region

    'Public Class CRunningStack
    '    Inherits CStack

    '    ''' <summary>
    '    ''' Rimuove e restituisce l'oggetto in cima allo stack
    '    ''' </summary>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Shadows Function Pop() As CStackObject
    '        Dim ret As CStackObject
    '        Dim i As Integer
    '        Dim s As CDebugFunStats
    '        Dim d As Double
    '        ret = Me.Top
    '        If Debug.Enabled Then
    '            i = System.Debug.FunStats.IndexOfKey(TypeName(ret.Caller) & "." & ret.MethodName)
    '            s = System.Debug.FunStats.Item(i)
    '            d = Timer - ret.EnterTime
    '            s.DoExit(d)
    '        End If
    '        MyBase.Pop()
    '        'If Not (Me.Top Is Nothing) Then Me.Top.EnterTime = Me.Top.EnterTime + d        
    '        Return ret
    '    End Function

    '    ''' <summary>
    '    ''' Inserisce un oggetto nello stack
    '    ''' </summary>
    '    ''' <param name="obj"></param>
    '    ''' <param name="methodName"></param>
    '    ''' <remarks></remarks>
    '    Public Shadows Sub Push(ByVal obj As Object, ByVal methodName As String)
    '        Dim item As New CStackObject
    '        Dim i As Integer
    '        Dim s As CDebugFunStats
    '        item.Caller = obj
    '        item.MethodName = methodName
    '        item.EnterTime = Timer
    '        Call MyBase.Push(item)
    '        If Debug.Enabled Then
    '            i = System.Debug.FunStats.IndexOfKey(TypeName(obj) & "." & methodName)
    '            If (i < 0) Then
    '                s = New CDebugFunStats
    '                s.FunName = TypeName(obj) & "." & methodName
    '                System.Debug.FunStats.Add(TypeName(obj) & "." & methodName, s)
    '            Else
    '                s = System.Debug.FunStats.Item(i)
    '            End If
    '        End If
    '    End Sub

    'End Class

    'Public Class CDebugFunStats
    '    Public FunName As String 'Nome della funzione
    '    Public Count As Integer 'Numero di volte in cui è stata chiamata la funzione
    '    Public InitTime As Nullable(Of Date) 'Data ed ora del primo utilizzo
    '    Public LastTime As Nullable(Of Date) 'Data ed ora dell'ultimo utilizzo
    '    Public MinTime As Double 'Tempo minimo di esecuzione
    '    Public MaxTime As Double 'Tempo massimo di esecuzione
    '    Public AveTime As Double 'Tempo medio di esecuzione
    '    Public TotalTime As Double 'Tempo totale

    '    Public Sub New()
    '        FunName = ""
    '        Count = 0
    '        InitTime = NULL
    '        LastTime = NULL
    '        MinTime = 0
    '        MaxTime = 0
    '        AveTime = 0
    '        TotalTime = 0
    '    End Sub

    '    Public Sub DoExit(ByVal d As Double)
    '        Me.LastTime = Now
    '        If (InitTime.HasValue = False) Then Me.InitTime = Me.LastTime
    '        Me.Count = Me.Count + 1
    '        If (Me.Count = 1) Then
    '            Me.MinTime = d
    '            Me.MaxTime = d
    '            Me.AveTime = d
    '            Me.TotalTime = d
    '        Else
    '            If (d < Me.MinTime) Then Me.MinTime = d
    '            If (d > Me.MaxTime) Then Me.MaxTime = d
    '            Me.AveTime = Me.AveTime * (Me.Count - 1)
    '            Me.AveTime = (Me.AveTime + d) / Me.Count
    '            Me.TotalTime = Me.TotalTime + d
    '        End If
    '    End Sub

    '    Public Overrides Function ToString() As String
    '        Dim ret As String
    '        ret = vbCrLf
    '        ret &= Me.FunName & vbCrLf
    '        ret &= "Count: " & Me.Count & vbCrLf
    '        ret &= "InitTime: " & Me.InitTime & vbCrLf
    '        ret &= "LastTime: " & Me.LastTime & vbCrLf
    '        ret &= "MinTime: " & Me.MinTime & vbCrLf
    '        ret &= "MaxTime: " & Me.MaxTime & vbCrLf
    '        ret &= "AveTime: " & Me.AveTime & vbCrLf
    '        ret &= "TotaleTime: " & Me.TotalTime & vbCrLf
    '        ToString = ret
    '    End Function

    'End Class

    'Public Class CFunStatsTimeComparer
    '    Implements IComparer

    '    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
    '        Dim a As CDebugFunStats = x
    '        Dim b As CDebugFunStats = y
    '        If (a.TotalTime > b.TotalTime) Then Return 1
    '        If (a.TotalTime < b.TotalTime) Then Return -1
    '        Return 0
    '    End Function

    'End Class


    Public Shared ReadOnly Property [Module] As CModule
        Get
            If (m_Module Is Nothing) Then m_Module = Sistema.Modules.GetItemByName("Sistema")
            Return m_Module
        End Get
    End Property


    Public Shared Function TestFlag(ByVal value As Integer, ByVal fieldValue As Integer) As Boolean
        Return (value And fieldValue) = fieldValue
    End Function

    Public Shared Function SetFlag(ByVal value As Integer, ByVal fieldValue As Integer, ByVal cond As Boolean) As Integer
        If (cond = False) Then
            Return value And Not fieldValue
        Else
            Return value Or fieldValue
        End If
    End Function

    Public Shared Function vbTypeName(ByVal obj As Object) As String
        Return TypeName(obj)
    End Function

    Private Shared m_Formatter As New BinaryFormatter

    Public Shared Sub BinarySerialize(ByVal obj As Object, ByVal stream As System.IO.Stream)
        m_Formatter.Serialize(stream, obj)
    End Sub

    Public Shared Function BinaryDeserialize(ByVal stream As System.IO.Stream) As Object
        Dim formatter As New BinaryFormatter()
        Return m_Formatter.Deserialize(stream)
    End Function

End Class

