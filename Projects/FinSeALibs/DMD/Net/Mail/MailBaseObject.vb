Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases

Namespace Net.Mail

    ''' <summary>
    ''' Rappresenta l'oggetto base da cui sono derivati gli oggetti email
    ''' </summary>
    Public MustInherit Class MailBaseObject

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub


        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

    End Class

End Namespace
