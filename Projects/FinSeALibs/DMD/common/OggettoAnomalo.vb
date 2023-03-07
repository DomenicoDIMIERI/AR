Imports DMD.Sistema
Imports DMD.Anagrafica

Public Class OggettoAnomalo
    Implements IComparable, DMD.XML.IDMDXMLSerializable

    Public Oggetto As Object
    Public Gruppo As String
    Public Anomalie As New CCollection(Of Anomalia)

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Function AggiungiAnomalia(ByVal descrizione As String, ByVal importanza As Integer) As Anomalia
        Dim a As Anomalia

        For Each a In Me.Anomalie
            If (a.Descrizione = descrizione) Then
                a.Importanza = Math.Min(a.Importanza, importanza)
                Return a
            End If
        Next

        a = New Anomalia
        a.Oggetto = Me.Oggetto
        a.Descrizione = descrizione
        a.Importanza = importanza
        Me.Anomalie.Add(a)
        Return a
    End Function

    Public Overridable Function CompareTo(ByVal obj As OggettoAnomalo) As Integer
        Return Strings.Compare(Me.Gruppo, obj.Gruppo, CompareMethod.Text)
    End Function

    Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function

    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Gruppo" : Me.Gruppo = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Oggetto" : Me.Oggetto = XML.Utils.Serializer.ToObject(fieldValue)
            Case "Anomalie" : Me.Anomalie.Clear() : Me.Anomalie.AddRange(fieldValue)
        End Select
    End Sub

    Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Gruppo", Me.Gruppo)
        writer.WriteTag("Oggetto", Me.Oggetto)
        writer.WriteTag("Anomalie", Me.Anomalie)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
