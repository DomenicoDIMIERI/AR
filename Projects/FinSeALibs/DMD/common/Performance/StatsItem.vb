Imports DMD
Imports DMD.Databases
Imports System.Net
Imports DMD.Sistema

<Serializable> _
Public Class StatsItem
    Implements DMD.XML.IDMDXMLSerializable, IComparable

    Private Shared cntLock As New Object
    Private Shared counter As Integer = 0

    ''' <summary>
    ''' ID dell'oggetto loggato
    ''' </summary>
    ''' <remarks></remarks>
    Public ID As Integer


    ''' <summary>
    ''' Stringa che identifica l'oggetto (nome della funzione, query, ecc...)
    ''' </summary>
    ''' <remarks></remarks>
    Public Name As String

    ''' <summary>
    ''' Numero di volte che la funzione è stata chiamata
    ''' </summary>
    ''' <remarks></remarks>
    Public Count As Integer

    ''' <summary>
    ''' Nuemro di chiamate alla funzione attive 
    ''' </summary>
    ''' <remarks></remarks>
    Public ActiveCount As Integer

    ''' <summary>
    ''' ID dell'utente che ha eseguito la query, funzione, ecc..
    ''' </summary>
    ''' <remarks></remarks>
    Public UserID As Integer

    ''' <summary>
    ''' Nome dell'utente che ha eseguito la query, funzione, ecc
    ''' </summary>
    ''' <remarks></remarks>
    Public UserName As String

    ''' <summary>
    ''' Tempo di esecuzione
    ''' </summary>
    ''' <remarks></remarks>
    Public ExecTime As Single

    ''' <summary>
    ''' Tempo massimo di esecuzione
    ''' </summary>
    ''' <remarks></remarks>
    Public MaxExecTime As Single


    ''' <summary>
    ''' Data ed ora dell'ultima esecuzione
    ''' </summary>
    ''' <remarks></remarks>
    Public LastRun As Date

    Private m_User As CUser

    Public MemoryUsage As Long

    Public MaxMemoryUsage As Long

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.Name = ""
        Me.Count = 0
        Me.UserName = ""
        Me.UserID = 0
        Me.ExecTime = 0
        Me.MaxExecTime = 0
        Me.LastRun = Nothing
        Me.m_User = Nothing
        Me.MemoryUsage = 0
        Me.MaxMemoryUsage = 0
    End Sub

    Public Sub New(ByVal name As String, ByVal count As Integer)
        Me.New()
        Me.Name = name
        Me.Count = count
    End Sub

    Public Sub New(ByVal name As String, ByVal count As Integer, ByVal lastRun As Date, ByVal memory As Long)
        Me.New()
        Me.Name = name
        Me.Count = count
        Me.LastRun = lastRun
        Me.MemoryUsage = memory
        Me.MaxMemoryUsage = memory
    End Sub

    Public Sub Begin()
        SyncLock cntLock
            counter += 1
            Me.ID = counter
        End SyncLock
        'Me.UserID = GetID(Sistema.Users.CurrentUser)
        'Me.UserName = Sistema.Users.CurrentUser.UserName
        Me.ActiveCount += 1
        Me.Count += 1
        Me.LastRun = Calendar.Now
        'ApplicationContext.Log(Formats.FormatUserDateTime(Now) & " - " & Me.UserName & " - " & Me.ID & " Begin:" & Me.Name)
    End Sub

    Public Sub [End](ByVal beginInfo As StatsItem)
        Dim exeTime As Double = (Calendar.Now - beginInfo.LastRun).TotalMilliseconds
        Dim memory As Long = GC.GetTotalMemory(False) - beginInfo.MemoryUsage
        Me.Count += 1
        'Me.UserID = GetID(Sistema.Users.CurrentUser)
        'Me.UserName = Sistema.Users.CurrentUser.UserName
        If (exeTime > 0) Then
            Me.ExecTime += exeTime
            Me.MaxExecTime = Math.Max(Me.MaxExecTime, exeTime)
        End If
        'Me.ActiveCount -= 1
        If (memory > 0) Then
            Me.MemoryUsage += memory
            Me.MaxMemoryUsage = Math.Max(Me.MaxMemoryUsage, memory)
        End If

    End Sub

    Public Property User As CUser
        Get
            If m_User Is Nothing Then m_User = Sistema.Users.GetItemById(Me.UserID)
            Return Me.m_User
        End Get
        Set(value As CUser)
            Me.m_User = value
            Me.UserID = GetID(value)
            Me.UserName = ""
            If (value IsNot Nothing) Then Me.UserName = value.UserName
        End Set
    End Property


    Public Overrides Function ToString() As String
        Return Me.Name & ": " & Formats.FormatInteger(Me.Count)
    End Function

    Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "ID" : Me.ID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "Name" : Me.Name = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Count" : Me.Count = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "ActiveCount" : Me.ActiveCount = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "UserID" : Me.UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "UserName" : Me.UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "ExecTime" : Me.ExecTime = XML.Utils.Serializer.DeserializeFloat(fieldValue)
            Case "MaxExecTime" : Me.MaxExecTime = XML.Utils.Serializer.DeserializeFloat(fieldValue)
            Case "LastRun" : Me.LastRun = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "MemoryUsage" : Me.MemoryUsage = XML.Utils.Serializer.DeserializeLong(fieldValue)
            Case "MaxMemoryUsage" : Me.MaxMemoryUsage = XML.Utils.Serializer.DeserializeLong(fieldValue)
        End Select
    End Sub

    Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("ID", Me.ID)
        writer.WriteAttribute("Name", Me.Name)
        writer.WriteAttribute("Count", Me.Count)
        writer.WriteAttribute("ActiveCount", Me.ActiveCount)
        writer.WriteAttribute("UserID", Me.UserID)
        writer.WriteAttribute("UserName", Me.UserName)
        writer.WriteAttribute("ExecTime", Me.ExecTime)
        writer.WriteAttribute("MaxExecTime", Me.MaxExecTime)
        writer.WriteAttribute("LastRun", Me.LastRun)
        writer.WriteAttribute("MemoryUsage", Me.MemoryUsage)
        writer.WriteAttribute("MaxMemoryUsage", Me.MaxMemoryUsage)
    End Sub

    Private Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim other As StatsItem = obj
        Dim ret As Integer = other.ExecTime - Me.ExecTime
        If (ret = 0) Then ret = other.MaxExecTime - Me.MaxExecTime
        If (ret = 0) Then ret = other.Count - Me.Count
        If (ret = 0) Then ret = Strings.Compare(Me.Name, other.Name, CompareMethod.Text)
        Return ret
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
