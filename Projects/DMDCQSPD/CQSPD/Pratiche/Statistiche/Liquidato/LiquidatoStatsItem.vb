Imports DMD
Imports DMD.Databases

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class CQSPD


    Public Class LiquidatoStatsItem
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Public Anno As Integer
        Public Mese As Integer

        Public CaricatoCnt As Integer
        Public CaricatoSum As Decimal
        Public CaricatoUpfront As Decimal
        Public CaricatoRunning As Decimal
        Public CaricatoSconto As Decimal

        Public RichiestaDeliberaCnt As Integer
        Public RichiestaDeliberaSum As Decimal
        Public RichiestaDeliberaUpfront As Decimal
        Public RichiestaDeliberaRunning As Decimal
        Public RichiestaDeliberaSconto As Decimal

        Public LiquidatoCnt As Integer
        Public LiquidatoSum As Decimal
        Public LiquidatoUpfront As Decimal
        Public LiquidatoRunning As Decimal
        Public LiquidatoSconto As Decimal

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim o As LiquidatoStatsItem = obj
            Dim ret As Integer = -Me.Anno + o.Anno
            If (ret = 0) Then ret = -Me.Mese + o.Mese
            Return ret
        End Function

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Anno" : Me.Anno = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Mese" : Me.Mese = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "CaricatoCnt" : Me.CaricatoCnt = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CaricatoSum" : Me.CaricatoSum = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CaricatoUpfront" : Me.CaricatoUpfront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CaricatoRunning" : Me.CaricatoRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CaricatoSconto" : Me.CaricatoSconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "RichiestaDeliberaCnt" : Me.RichiestaDeliberaCnt = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RichiestaDeliberaSum" : Me.RichiestaDeliberaSum = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RichiestaDeliberaUpfront" : Me.RichiestaDeliberaUpfront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RichiestaDeliberaRunning" : Me.RichiestaDeliberaRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RichiestaDeliberaSconto" : Me.RichiestaDeliberaSconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "LiquidatoCnt" : Me.LiquidatoCnt = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LiquidatoSum" : Me.LiquidatoSum = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LiquidatoUpfront" : Me.LiquidatoUpfront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LiquidatoRunning" : Me.LiquidatoRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LiquidatoSconto" : Me.LiquidatoSconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Anno", Me.Anno)
            writer.WriteAttribute("Mese", Me.Mese)

            writer.WriteAttribute("CaricatoCnt", Me.CaricatoCnt)
            writer.WriteAttribute("CaricatoSum", Me.CaricatoSum)
            writer.WriteAttribute("CaricatoUpfront", Me.CaricatoUpfront)
            writer.WriteAttribute("CaricatoRunning", Me.CaricatoRunning)
            writer.WriteAttribute("CaricatoSconto", Me.CaricatoSconto)

            writer.WriteAttribute("RichiestaDeliberaCnt", Me.RichiestaDeliberaCnt)
            writer.WriteAttribute("RichiestaDeliberaSum", Me.RichiestaDeliberaSum)
            writer.WriteAttribute("RichiestaDeliberaUpfront", Me.RichiestaDeliberaUpfront)
            writer.WriteAttribute("RichiestaDeliberaRunning", Me.RichiestaDeliberaRunning)
            writer.WriteAttribute("RichiestaDeliberaSconto", Me.RichiestaDeliberaSconto)

            writer.WriteAttribute("LiquidatoCnt", Me.LiquidatoCnt)
            writer.WriteAttribute("LiquidatoSum", Me.LiquidatoSum)
            writer.WriteAttribute("LiquidatoUpfront", Me.LiquidatoUpfront)
            writer.WriteAttribute("LiquidatoRunning", Me.LiquidatoRunning)
            writer.WriteAttribute("LiquidatoSconto", Me.LiquidatoSconto)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class