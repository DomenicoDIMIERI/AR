Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms



    Public Class CQSPDDashInfo
        Implements XML.IDMDXMLSerializable

        Public Count As Integer
        Public SommaMontanteLordo As Decimal
        Public SommaProvvigioneTotale As Decimal
        Public SommaSpread As Decimal
        Public SommaUpFront As Decimal
        Public SommaRunning As Decimal

        Public Pratiche As CCollection(Of CQSPDPDBInfo)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Count = 0
            Me.SommaMontanteLordo = 0
            Me.SommaProvvigioneTotale = 0
            Me.SommaSpread = 0
            Me.SommaUpFront = 0
            Me.SommaRunning = 0
            Me.Pratiche = New CCollection(Of CQSPDPDBInfo)
        End Sub

        Protected Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Count", Me.Count)
            writer.WriteAttribute("SommaMontanteLordo", Me.SommaMontanteLordo)
            writer.WriteAttribute("SommaProvvigioneTotale", Me.SommaProvvigioneTotale)
            writer.WriteAttribute("SommaSpread", Me.SommaSpread)
            writer.WriteAttribute("SommaUpFront", Me.SommaUpFront)
            writer.WriteAttribute("SommaRunning", Me.SommaRunning)
            writer.WriteTag("Pratiche", Me.Pratiche)
        End Sub

        Protected Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case (fieldName)
                Case "Pratiche" : Me.Pratiche = fieldValue
                Case "Count" : Me.Count = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SommaMontanteLordo" : Me.SommaMontanteLordo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SommaProvvigioneTotale" : Me.SommaProvvigioneTotale = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SommaRunning" : Me.SommaRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SommaSpread" : Me.SommaSpread = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SommaUpFront" : Me.SommaUpFront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace