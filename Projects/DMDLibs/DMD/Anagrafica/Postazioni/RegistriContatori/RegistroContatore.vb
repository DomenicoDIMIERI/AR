Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.XML

Partial Public Class Anagrafica

    ''' <summary>
    ''' Definisce un contatore valido per una postazione (es. il numero di copie di una stampante)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class RegistroContatore
        Implements DMD.XML.IDMDXMLSerializable

        Public Nome As String
        Public LimiteMin As Double?
        Public LimiteMax As Double?
        Public Decimali As Integer
        Public Flags As Integer

        Public Sub New()
            Me.Nome = ""
            Me.LimiteMin = Nothing
            Me.LimiteMax = Nothing
            Me.Decimali = 0
            Me.Flags = 0
        End Sub

        Public Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Nome", Me.Nome)
            writer.WriteAttribute("LimiteMin", Me.LimiteMin)
            writer.WriteAttribute("LimiteMax", Me.LimiteMax)
            writer.WriteAttribute("Decimali", Me.Decimali)
            writer.WriteAttribute("Flags", Me.Flags)
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Nome" : Me.Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "LimiteMin" : Me.LimiteMin = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LimiteMax" : Me.LimiteMax = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Decimali" : Me.Decimali = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Nome
        End Function




    End Class


End Class