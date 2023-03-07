Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica




Partial Public Class CQSPD

    ''' <summary>
    ''' Rappresenta dei dati aggiuntivi per la pratica (relazione 1 a 1)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CCorrezionePratica
        Inherits DMD.DMDObject
        Implements DMD.XML.IDMDXMLSerializable

        Public Data As Date
        Public IDOperatore As Integer
        Public NomeOperatore As String
        Public NomeCampo As String
        Public TipoValore As System.TypeCode
        Public VecchioValore As String
        Public NuovoValore As String
        Public Note As String

        Public Sub New()
            Me.Data = Calendar.Now
            Me.IDOperatore = 0
            Me.NomeOperatore = ""
            Me.NomeCampo = ""
            Me.TipoValore = TypeCode.Empty
            Me.VecchioValore = ""
            Me.NuovoValore = ""
            Me.Note = ""
        End Sub



        Protected Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements DMD.XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Data", Me.Data)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.NomeOperatore)
            writer.WriteAttribute("NomeCampo", Me.NomeCampo)
            writer.WriteAttribute("TipoValore", Me.TipoValore)
            writer.WriteAttribute("VecchioValore", Me.VecchioValore)
            writer.WriteAttribute("NuovoValore", Me.NuovoValore)
            writer.WriteAttribute("Note", Me.Note)
        End Sub

        Protected Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements DMD.XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Data" : Me.Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeCampo" : Me.NomeCampo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoValore" : Me.TipoValore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "VecchioValore" : Me.VecchioValore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NuovoValore" : Me.NuovoValore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else
            End Select
        End Sub

    End Class


End Class
