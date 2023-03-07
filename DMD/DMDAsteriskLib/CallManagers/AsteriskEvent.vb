Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers

    ''' <summary>
    ''' Rappresenta un evento generico di Asterisk CLI
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AsteriskEvent
        Inherits System.EventArgs

        Private m_Data As Date
        Private m_Manager As AsteriskCallManager
        Private m_Attributes As System.Collections.Specialized.NameValueCollection '(Of String)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Data = Now
            Me.m_Attributes = New System.Collections.Specialized.NameValueCollection '(Of String)
        End Sub

        Public Sub New(ByVal rows() As String)
            Me.New
            Me.Parse(rows)
        End Sub

        Public Sub New(ByVal eventName As String)
            Me.New()
            Me.m_Manager = Nothing
            Me.SetAttribute("Event", Trim(eventName))
        End Sub

        Public Sub New(ByVal manager As AsteriskCallManager, ByVal eventName As String)
            Me.New()
            Me.m_Manager = manager
            Me.SetAttribute("Event", Trim(eventName))
        End Sub


        Public Sub New(ByVal manager As AsteriskCallManager, ByVal rows() As String)
            Me.New
            Me.m_Manager = manager
            Me.Parse(rows)
        End Sub

        Public Sub New(ByVal manager As AsteriskCallManager, ByVal e As AsteriskEvent)
            Me.New
            Me.m_Data = e.m_Data
            Me.m_Manager = manager
            Me.m_Attributes = e.m_Attributes
        End Sub

        Public ReadOnly Property Manager As AsteriskCallManager
            Get
                Return Me.m_Manager
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la data e l'ora dell'evento
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Data As Date
            Get
                Return Me.m_Data
            End Get
        End Property



        Public Function GetAttribute(ByVal attrName As String) As String
            attrName = UCase(Trim(attrName))
            Return Me.m_Attributes.Get(attrName) '.GetItemByKey(attrName)
        End Function

        Private Function ContainsKey(ByVal key As String) As Boolean
            If (Me.m_Attributes.Get(key) Is Nothing) Then Return Me.m_Attributes.AllKeys.Contains(key)
            Return True
        End Function

        Public Function GetAttribute(ByVal attrName As String, ByVal defValue As Integer) As Integer
            attrName = UCase(Trim(attrName))
            If Me.ContainsKey(attrName) = False Then Return defValue
            Return CInt(Me.m_Attributes.Get(attrName))
        End Function

        Public Function GetAttribute(ByVal attrName As String, ByVal defValue As Single) As Single
            attrName = UCase(Trim(attrName))
            If Me.ContainsKey(attrName) = False Then Return defValue
            Return CSng(Me.m_Attributes.Get(attrName))
        End Function

        Public Sub SetAttribute(ByVal attrName As String, ByVal attrValue As String)
            attrName = UCase(Trim(attrName))
            'If Not Me.AttrMe.m_Attributes.ContainsKey(attrName) Then
            'Me.m_Attributes(attrName) = attrValue
            'Else
            'Me.m_Attributes.Add(attrName, attrValue)
            'End If
            Me.m_Attributes.Set(attrName, attrValue)
        End Sub

        Public Sub RemoveAttribute(ByVal attrName As String)
            attrName = UCase(Trim(attrName))
            Me.m_Attributes.Remove(attrName)
            'Me.m_Attributes.RemoveByKey(attrName)
        End Sub

        ''' <summary>
        ''' Restituisce il nome dell'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EventName As String
            Get
                Return Me.GetAttribute("Event")
            End Get
        End Property

       
        ''' <summary>
        ''' Se presente l'evento è parte della risposta all'azione con l'ID specificato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ActionID As String
            Get
                Return Me.GetAttribute("ActionID")
            End Get
        End Property

        Protected Friend Sub Parse(ByVal rows() As String)
            For i As Integer = 0 To UBound(rows)
                Dim row As New Responses.RowEntry(rows(i))
                Me.ParseRow(row)
            Next
        End Sub

        Protected Overridable Sub ParseRow(ByVal row As Responses.RowEntry)
            'Select Case UCase(row.Command)
            '    Case "EVENT" : Me.m_EventName = row.Params
            '    Case "PRIVILEGE" : Me.m_Privilege = row.Params
            '    Case "ACTIONID" : Me.m_ActionID = row.Params
            '    Case Else : Debug.Print("Unsupported: " & row.Command)
            'End Select
            Me.SetAttribute(row.Command, row.Params)
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            For Each k As String In Me.m_Attributes.Keys
                ret.Append(k & ": " & Me.m_Attributes(k) & vbCrLf)
            Next
            Return ret.ToString
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace