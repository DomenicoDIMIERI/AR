Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.Internals


Namespace Internals

    Public Class CMailApplications
        Inherits CGeneralClass(Of MailApplication)

        Public Sub New()
            MyBase.New("modMailApps", GetType(MailApplicationCursor), -1)
        End Sub

        Public Function GetItemByUser(ByVal user As CUser) As MailApplication
            Dim cursor As MailApplicationCursor = Nothing
            Try
                If (user Is Nothing) Then Throw New ArgumentNullException("user")
                If (GetID(user) = 0) Then Return Nothing

                cursor = New MailApplicationCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.UserID.Value = GetID(user)

                Return cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        ''' <summary>
        ''' Questa funzione cerca l'applicazione associata all'utente e se non la trova la crea.
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        Public Function GetUserApp(ByVal user As CUser) As MailApplication
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            If (GetID(user) = 0) Then Throw New ArgumentNullException("userid")
            Dim app As MailApplication = Me.GetItemByUser(user)
            If (app Is Nothing) Then
                app = New MailApplication
                app.User = user
                app.Stato = ObjectStatus.OBJECT_VALID
                app.Save()
            End If
            Return app
        End Function

    End Class

End Namespace
