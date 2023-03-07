Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    <Serializable> _
    Public Class CStatisticheOperazione
        Implements XML.IDMDXMLSerializable

        ''' <summary>
        ''' Numero delle chiamate
        ''' </summary>
        ''' <remarks></remarks>
        Public Numero As Integer

        Public Valore As Nullable(Of Decimal)

        ''' <summary>
        ''' Durata minima (in secondi)
        ''' </summary>
        ''' <remarks></remarks>
        Public MinLen As Double
        ''' <summary>
        ''' Durata massima (in secondi)
        ''' </summary>
        ''' <remarks></remarks>
        Public MaxLen As Double

        ''' <summary>
        ''' Somma delle durate (in secondi)
        ''' </summary>
        ''' <remarks></remarks>
        Public TotalLen As Double

        ''' <summary>
        ''' Somma delle attese (in secondi)
        ''' </summary>
        ''' <remarks></remarks>
        Public TotalWait As Double

        Public MinWait As Double

        Public MaxWait As Double

        ''' <summary>
        ''' Costo totale
        ''' </summary>
        ''' <remarks></remarks>
        Public TotalCost As Decimal


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Numero = 0
            Me.Valore = Nothing
            Me.MinLen = 0
            Me.MaxLen = 0
            Me.TotalLen = 0
            Me.TotalWait = 0
            Me.MinWait = 0
            Me.MaxWait = 0
            Me.TotalCost = 0
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Numero" : Me.Numero = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Valore" : Me.Valore = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MinLen" : Me.MinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MaxLen" : Me.MaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TotalLen" : Me.TotalLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TotalWait" : Me.TotalWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MinWait" : Me.MinWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MaxWait" : Me.MaxWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TotalCost" : Me.TotalCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Numero", Me.Numero)
            writer.WriteAttribute("Valore", Me.Valore)
            writer.WriteAttribute("MinLen", Me.MinLen)
            writer.WriteAttribute("MaxLen", Me.MaxLen)
            writer.WriteAttribute("TotalLen", Me.TotalLen)
            writer.WriteAttribute("TotalWait", Me.TotalWait)
            writer.WriteAttribute("MinWait", Me.MinWait)
            writer.WriteAttribute("MaxWait", Me.MaxWait)
            writer.WriteAttribute("TotalCost", Me.TotalCost)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Class