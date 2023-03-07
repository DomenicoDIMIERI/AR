Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms



    Public Class CQSPDPDBInfo
        Implements XML.IDMDXMLSerializable

        Public IDPratica As Integer
        Public NominativoCliente As String
        Public NomeProdotto As String
        Public IDStatoPratica As Integer
        Public GiorniContatto As Integer
        Public GiorniStatoAttuale As Integer
        Public Netto As Decimal
        Public Durata As Integer
        Public Rata As Decimal
        Public Spread As Decimal
        Public UpFront As Decimal
        Public Attributi As New CKeyCollection

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.IDPratica = 0
            Me.NominativoCliente = ""
            Me.NomeProdotto = ""
            Me.IDStatoPratica = 0
            Me.GiorniContatto = 0
            Me.GiorniStatoAttuale = 0
            Me.Netto = 0
            Me.Durata = 0
            Me.Rata = 0
            Me.Spread = 0
            Me.UpFront = 0
        End Sub

        Protected Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("NominativoCliente", Me.NominativoCliente)
            writer.WriteAttribute("NomeProdotto", Me.NomeProdotto)
            writer.WriteAttribute("IDStatoPratica", Me.IDStatoPratica)
            writer.WriteAttribute("GiorniContatto", Me.GiorniContatto)
            writer.WriteAttribute("GiorniStatoAttuale", Me.GiorniStatoAttuale)
            writer.WriteAttribute("Netto", Me.Netto)
            writer.WriteAttribute("Durata", Me.Durata)
            writer.WriteAttribute("Rata", Me.Rata)
            writer.WriteAttribute("Spread", Me.Spread)
            writer.WriteAttribute("UpFront", Me.UpFront)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case (fieldName)
                Case "IDPratica" : IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NominativoCliente" : NominativoCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeProdotto" : NomeProdotto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDStatoPratica" : Me.IDStatoPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "GiorniContatto" : GiorniContatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "GiorniStatoAttuale" : Me.GiorniStatoAttuale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Netto" : Me.Netto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Durata" : Me.Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Rata" : Me.Rata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Spread" : Me.Spread = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "UpFront" : Me.UpFront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Attributi" : Me.Attributi = fieldValue
            End Select
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace