Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Net.Mime

namespace Net.Mime

    Public NotInheritable Class MediaTypes

        Public Shared ReadOnly Multipart As String = "multipart"
        Public Shared ReadOnly Mixed As String = "mixed"
        Public Shared ReadOnly Alternative As String = "alternative"
        Public Shared ReadOnly MultipartMixed As String = String.Concat(Multipart, "/", Mixed)
        Public Shared ReadOnly MultipartAlternative As String = String.Concat(Multipart, "/", Alternative)
        Public Shared ReadOnly TextPlain As String = MediaTypeNames.Text.Plain
        Public Shared ReadOnly TextHtml As String = MediaTypeNames.Text.Html
        Public Shared ReadOnly TextRich As String = MediaTypeNames.Text.RichText
        Public Shared ReadOnly TextXml As String = MediaTypeNames.Text.Xml
        Public Shared ReadOnly Message As String = "message"
        Public Shared ReadOnly Rfc822 As String = "rfc822"
        Public Shared ReadOnly MessageRfc822 As String = String.Concat(Message, "/", Rfc822)
        Public Shared ReadOnly Application As String

    End Class

End Namespace
