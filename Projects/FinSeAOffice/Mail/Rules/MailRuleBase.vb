Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Net.Mail

Partial Class Office

   

    <Serializable> _
    Public MustInherit Class MailRuleBase
        Implements DMD.XML.IDMDXMLSerializable, IComparable


        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la regola è attiva
        ''' </summary>
        ''' <remarks></remarks>
        Public Attiva As Boolean

        ''' <summary>
        ''' Restituisce o imposta l'ordine di applicazione della regola
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priorita As Integer


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Attiva = False
            Me.Priorita = 0
        End Sub

        Public Function CompareTo(obj As MailRuleBase) As Integer
            Return Me.Priorita.CompareTo(obj.Priorita)
        End Function

        Private Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        ''' <summary>
        ''' Restituisce vero se la regola si applica al messaaggio
        ''' </summary>
        ''' <param name="m"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function Check(ByVal m As MailMessage)

        ''' <summary>
        ''' Applica la regola al messaggio
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub Execute(ByVal m As MailMessage)



        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Priorita" : Me.Priorita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attiva" : Me.Attiva = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Attiva", Me.Attiva)
            writer.WriteAttribute("Priorita", Me.Priorita)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class