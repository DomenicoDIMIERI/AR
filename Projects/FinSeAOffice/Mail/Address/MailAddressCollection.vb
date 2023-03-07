Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.Internals
Imports DMD.XML

Partial Class Office

    <Serializable>
    Public Class MailAddressCollection
        Inherits CCollection(Of MailAddress)

        <NonSerialized> Private m_Message As MailMessage
        Private m_FieldName As String

        Public Sub New()
            Me.m_Message = Nothing
            Me.m_FieldName = ""
        End Sub

        Public Sub New(ByVal msg As MailMessage, ByVal fieldName As String)
            Me.New
            Me.m_Message = msg
            Me.m_FieldName = fieldName
            For Each m As MailAddress In msg.GetOriginalAdressies
                If (m.FieldName = fieldName) Then
                    Me.Add(m)
                End If
            Next
        End Sub

        Public ReadOnly Property Application As MailApplication
            Get
                If (Me.m_Message Is Nothing) Then Return Nothing
                Return Me.m_Message.Application
            End Get
        End Property


        Public ReadOnly Property Message As MailMessage
            Get
                Return Me.m_Message
            End Get
        End Property

        Protected Friend Sub SetMessage(ByVal value As MailMessage)
            Me.m_Message = value
            If (value IsNot Nothing) Then
                For Each m As MailAddress In Me
                    m.SetApplication(value.Application)
                    m.SetMessage(value)
                Next
            End If
        End Sub

        Protected Friend Sub SetFieldName(ByVal value As String)
            Me.m_FieldName = value
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Message IsNot Nothing) Then
                With DirectCast(value, MailAddress)
                    .SetApplication(Me.Application)
                    .SetMessage(Me.m_Message)
                    .FieldName = Me.m_FieldName
                End With
            End If
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Message IsNot Nothing) Then
                With DirectCast(newValue, MailAddress)
                    .SetApplication(Me.Application)
                    .SetMessage(Me.m_Message)
                    .FieldName = Me.m_FieldName
                End With
            End If
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            For Each a As MailAddress In Me
                If (ret.Length > 0) Then ret.Append(", ")
                ret.Append(a.ToString)
            Next
            Return ret.ToString
        End Function
    End Class

End Class
