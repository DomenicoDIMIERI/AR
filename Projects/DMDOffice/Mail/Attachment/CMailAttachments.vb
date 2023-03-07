Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.Internals


Namespace Internals

    Public Class CMailAttachments
        Inherits CGeneralClass(Of MailAttachment)

        Public Sub New()
            MyBase.New("modOfficeEMailsAtt", GetType(MailAttachmentCursor), 0)
        End Sub

    End Class

End Namespace

