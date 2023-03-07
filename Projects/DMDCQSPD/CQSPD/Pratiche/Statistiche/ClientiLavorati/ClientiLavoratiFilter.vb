Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Partial Public Class CQSPD

     
    <Serializable> _
    Public Class ClientiLavoratiFilter
        Implements DMD.XML.IDMDXMLSerializable

        Public IDPuntoOperativo As Integer
        Public IDConsulente As Integer
        Public DataInizio As Date?
        Public DataFine As Date?
        Public TipoFonte As String
        Public IDFonte As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.IDPuntoOperativo = 0
            Me.IDConsulente = 0
            Me.DataInizio = Nothing
            Me.DataFine = Nothing
            Me.TipoFonte = ""
            Me.IDFonte = 0
        End Sub



        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDPuntoOperativo" : Me.IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDConsulente" : Me.IDConsulente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizio" : Me.DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoFonte" : Me.TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("IDConsulente", Me.IDConsulente)
            writer.WriteAttribute("DataInizio", Me.DataInizio)
            writer.WriteAttribute("DataFine", Me.DataFine)
            writer.WriteAttribute("TipoFonte", Me.TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Class
