Imports DMD
Imports DMD.Databases

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class CQSPD

    ''' <summary>
    ''' Filtro applicabile alle statistiche sul liquidato
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LiquidatoFilter
        Implements DMD.XML.IDMDXMLSerializable

        Public PuntiOperativi As CCollection(Of Integer)
        Public Anni As CCollection(Of Integer)
        Public ChartWidth As Integer
        Public ChartHeight As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.PuntiOperativi = New CCollection(Of Integer)
            Me.Anni = New CCollection(Of Integer)
            Me.ChartWidth = 0
            Me.ChartHeight = 0
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "PO" : Me.PuntiOperativi.Clear() : Me.PuntiOperativi.AddRange(fieldValue)
                Case "Anni" : Me.Anni.Clear() : Me.Anni.AddRange(fieldValue)
                Case "ChartWidth" : Me.ChartWidth = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ChartHeight" : Me.ChartHeight = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("ChartWidth", Me.ChartWidth)
            writer.WriteAttribute("ChartHeight", Me.ChartWidth)
            writer.WriteTag("PO", Me.PuntiOperativi)
            writer.WriteTag("Anni", Me.Anni)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class