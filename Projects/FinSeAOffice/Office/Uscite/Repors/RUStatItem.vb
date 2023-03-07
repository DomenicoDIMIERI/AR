Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports DMD.GPS

Partial Class Office

      Public Class RUStatItem
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Public UserID As Integer
        Public UserName As String
        Public NomeOperatore As String
        Public Uscite As CCollection(Of Uscita)

        Public Sub New(ByVal operatore As CUser)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.UserID = GetID(operatore)
            Me.UserName = operatore.UserName
            Me.NomeOperatore = operatore.Nominativo
            Me.Uscite = New CCollection(Of Uscita)
        End Sub


        Private Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            With DirectCast(obj, RUStatItem)
                Return Strings.Compare(Me.NomeOperatore, .NomeOperatore, CompareMethod.Text)
            End With
        End Function

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "UserID" : Me.UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserName" : Me.UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeOperatore" : Me.NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Uscite" : Me.Uscite = XML.Utils.Serializer.ToObject(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("UserID", Me.UserID)
            writer.WriteAttribute("UserName", Me.UserName)
            writer.WriteAttribute("NomeOperatore", Me.NomeOperatore)
            writer.WriteTag("Uscite", Me.Uscite)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Class