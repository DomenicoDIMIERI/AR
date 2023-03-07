Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    <Serializable>
    Public Class CRMStatsAggregation
        Implements DMD.XML.IDMDXMLSerializable

        Public IDOperatore As Integer
        Public IDPuntoOperativo As Integer
        Public Data As Date
        Public Effettuate As CStatisticheOperazione
        Public Ricevute As CStatisticheOperazione

        Public Sub New()
            Me.IDOperatore = 0
            Me.IDPuntoOperativo = 0
            Me.Data = Calendar.ToDay
            Me.Effettuate = New CStatisticheOperazione
            Me.Ricevute = New CStatisticheOperazione
        End Sub


        Protected Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPuntoOperativo" : Me.IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Data" : Me.Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Effettuate" : Me.Effettuate = CType(fieldValue, CStatisticheOperazione)
                Case "Ricevute" : Me.Ricevute = CType(fieldValue, CStatisticheOperazione)
            End Select
        End Sub

        Protected Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("Data", Me.Data)
            writer.WriteTag("Effettuate", Me.Effettuate)
            writer.WriteTag("Ricevute", Me.Ricevute)
        End Sub

    End Class



End Class