Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.Internals


Partial Class Office

    <Serializable>
    Public Class MailApplicationAccounts
        Inherits CCollection(Of MailAccount)

        <NonSerialized> Private m_Application As MailApplication

        Public Sub New()
            Me.m_Application = Nothing
        End Sub

        Public Sub New(ByVal app As MailApplication)
            Me.New
            If (app Is Nothing) Then Throw New ArgumentNullException("app")
            Me.Load(app)
        End Sub

        Protected Friend Sub Load(ByVal app As MailApplication)
            Dim cursor As MailAccountCursor = Nothing
            Try
                Me.Clear()
                Me.m_Application = app
                If (GetID(app) = 0) Then Return
                cursor = New MailAccountCursor
                cursor.ApplicationID.Value = GetID(app)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub


    End Class

End Class
