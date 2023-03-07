Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office

Namespace Internals

    Public Class CFoldersClass
        Inherits CGeneralClass(Of MailFolder)

        Public Sub New()
            MyBase.New("modOfficeEMailsFolders", GetType(MailFolderCursor), 0)
        End Sub


    End Class

End Namespace
