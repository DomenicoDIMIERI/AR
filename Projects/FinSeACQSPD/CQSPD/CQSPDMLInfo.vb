Imports DMD
Imports DMD.Databases
Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Partial Class CQSPD



    ''' <summary>
    ''' Informazioni aggregate sulle pratiche che verificano un filtro
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CQSPDMLInfo
        Implements DMD.XML.IDMDXMLSerializable

        ''' <summary>
        ''' Numero di pratiche che verificano il filtro
        ''' </summary>
        ''' <remarks></remarks>
        Public Conteggio As Integer

        ''' <summary>
        ''' Somma del montante lordo
        ''' </summary>
        ''' <remarks></remarks>
        Public ML As Decimal

        ''' <summary>
        ''' Somma dello spread
        ''' </summary>
        ''' <remarks></remarks>
        Public Spread As Decimal

        ''' <summary>
        ''' Somma dell'UpFront
        ''' </summary>
        ''' <remarks></remarks>
        Public UpFront As Decimal

        ''' <summary>
        ''' Somma del Running
        ''' </summary>
        ''' <remarks></remarks>
        Public Running As Decimal

        ''' <summary>
        ''' Somma degli sconti applicati
        ''' </summary>
        ''' <remarks></remarks>
        Public Sconto As Decimal

        ''' <summary>
        ''' Somma del Rappel
        ''' </summary>
        ''' <remarks></remarks>
        Public Rappel As Decimal

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub


        Protected Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Conteggio" : Me.Conteggio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ML" : Me.ML = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Spread" : Me.Spread = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "UpFront" : Me.UpFront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Sconto" : Me.Sconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Running" : Me.Running = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Rappel" : Me.Rappel = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Protected Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Conteggio", Me.Conteggio)
            writer.WriteAttribute("ML", Me.ML)
            writer.WriteAttribute("Spread", Me.Spread)
            writer.WriteAttribute("UpFront", Me.UpFront)
            writer.WriteAttribute("Running", Me.Running)
            writer.WriteAttribute("Sconto", Me.Sconto)
            writer.WriteAttribute("Rappel", Me.Rappel)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Class