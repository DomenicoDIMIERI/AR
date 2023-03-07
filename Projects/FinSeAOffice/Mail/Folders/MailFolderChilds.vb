Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Sistema
Imports DMD.Databases

Partial Class Office

    <Serializable> _
    Public Class MailFolderChilds
        Inherits MailFoldersCollection

        Public Sub New()
        End Sub

        Public Sub New(ByVal parent As MailFolder)
            Me.New
            If (parent Is Nothing) Then Throw New ArgumentNullException("parent")
            Me.Load(parent)
        End Sub



    End Class

End Class