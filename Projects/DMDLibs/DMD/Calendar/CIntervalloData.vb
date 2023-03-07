Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema

    Public Class CIntervalloData
        Implements XML.IDMDXMLSerializable, ISupportInitializeFrom

        Public Tipo As String = ""
        Public Inizio As Nullable(Of Date) = Nothing
        Public Fine As Nullable(Of Date) = Nothing

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal tipo As String, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date))
            Me.New
            Me.Tipo = Trim(tipo)
            Me.Inizio = fromDate
            Me.Fine = toDate
        End Sub

        Public Overridable Function IsSet() As Boolean
            Return (Types.IsNull(Me.Inizio) = False) Or (Types.IsNull(Me.Fine) = False) Or (Me.Tipo <> "")
        End Function

        Protected Overridable Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements DMD.XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("m_Tipo", Me.Tipo)
            writer.WriteAttribute("m_Inizio", Me.Inizio)
            writer.WriteAttribute("m_Fine", Me.Fine)
        End Sub

        Protected Overridable Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements DMD.XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "m_Tipo" : Me.Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Inizio" : Me.Inizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "m_Fine" : Me.Fine = XML.Utils.Serializer.DeserializeDate(fieldValue)
            End Select
        End Sub


        Public Overridable Sub CopyFrom(value As Object) Implements ISupportInitializeFrom.InitializeFrom
            With DirectCast(value, CIntervalloData)
                Me.Fine = .Fine
                Me.Inizio = .Inizio
                Me.Tipo = .Tipo
            End With
        End Sub

        Public Overridable Sub Clear()
            Me.Fine = Nothing
            Me.Inizio = Nothing
            Me.Tipo = ""
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class