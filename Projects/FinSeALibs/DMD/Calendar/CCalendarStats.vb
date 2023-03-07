Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema



    Public Class CCalendarStats
        Implements XML.IDMDXMLSerializable

        Public Previste As Integer
        Public Effettuate As Integer
        Public Ricevute As Integer
        'Public PersoneAttive As CCollection

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Previste = 0
            Me.Effettuate = 0
            Me.Ricevute = 0
            ' Me.PersoneAttive = New CCollection
        End Sub

        Protected Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements DMD.XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Previste", Me.Previste)
            writer.WriteAttribute("Effettuate", Me.Effettuate)
            writer.WriteAttribute("Ricevute", Me.Ricevute)
            'writer.BeginTag("PersoneAttive")
            'If Me.PersoneAttive.Count > 0 Then
            'writer.Write(Me.PersoneAttive.ToArray, "CActivePerson")
            'End If
            ' writer.EndTag()
        End Sub

        Protected Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Previste" : Me.Previste = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Effettuate" : Me.Effettuate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Ricevute" : Me.Ricevute = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Case "PersoneAttive"
                    '    Me.PersoneAttive.Clear()
                    '  If TypeName(fieldValue) <> "string" Then
                    'If Not IsArray(fieldValue) Then fieldValue = {fieldValue}
                    ' Call Me.PersoneAttive.AddRange(fieldValue)
                    '  End If
            End Select
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class

