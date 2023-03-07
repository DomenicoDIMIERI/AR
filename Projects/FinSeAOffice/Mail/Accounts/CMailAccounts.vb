Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office

Namespace Internals
    ''' <summary>
    ''' Rappresenta la collezione degli account definiti per l'applicazione
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CMailAccounts
        Inherits CGeneralClass(Of MailAccount)

        Public Sub New()
            MyBase.New("modOfficeEMailsAcc", GetType(MailAccountCursor), 0)
        End Sub




        'Protected Friend Sub Load()
        '    Dim cursor As MailAccountCursor = Nothing
        '    Try
        '        Me.Clear()

        '        cursor = New MailAccountCursor()
        '        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '        cursor.DisplayName.SortOrder = SortEnum.SORT_ASC
        '        cursor.IgnoreRights = True
        '        While Not cursor.EOF
        '            Me.Add(cursor.Item)
        '            cursor.MoveNext()
        '        End While

        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '        Throw
        '    Finally
        '        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '    End Try
        'End Sub



    End Class

End Namespace