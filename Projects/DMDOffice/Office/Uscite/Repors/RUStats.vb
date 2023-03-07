Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office

    Public Class RUStats
        Implements DMD.XML.IDMDXMLSerializable

        Public items As CKeyCollection(Of RUStatItem)
        Public Commissioni As CKeyCollection(Of Commissione)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.items = New CKeyCollection(Of RUStatItem)
            Me.Commissioni = New CKeyCollection(Of Commissione)
        End Sub


        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Dim tmp As CKeyCollection
            Select Case fieldName
                Case "items"
                    Me.items.Clear()
                    tmp = XML.Utils.Serializer.ToObject(fieldValue)
                    For Each item As RUStatItem In tmp
                        Me.items.Add(item.NomeOperatore, item)
                    Next
                Case "Commissioni"
                    Me.Commissioni.Clear()
                    tmp = XML.Utils.Serializer.ToObject(fieldValue)
                    For Each key As String In tmp.Keys
                        Me.Commissioni.Add(key, tmp(key))
                    Next
                    Me.SincronizzaCommissioni()
            End Select
        End Sub

        Private Sub SincronizzaCommissioni()
            For Each item As RUStatItem In Me.items
                For Each u As Uscita In item.Uscite
                    For Each cxu As CommissionePerUscita In u.Commissioni
                        cxu.SetCommissione(Me.Commissioni.GetItemByKey("C" & cxu.IDCommissione))
                    Next
                Next
            Next
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            Me.Commissioni.Clear()
            For Each item As RUStatItem In Me.Items
                For Each u As Uscita In item.Uscite
                    For Each cxu As CommissionePerUscita In u.Commissioni
                        If Not Me.Commissioni.ContainsKey("C" & cxu.IDCommissione) Then
                            Me.Commissioni.Add("C" & cxu.IDCommissione, cxu.Commissione)
                        End If
                    Next
                Next
            Next
            writer.WriteTag("items", Me.items)
            writer.WriteTag("Commissioni", Me.Commissioni)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Class