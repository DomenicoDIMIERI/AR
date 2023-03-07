Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms

 

    Public Class OfficeSituazionePersona
        Implements DMD.XML.IDMDXMLSerializable

        Private m_IDPersona As Integer = 0
        Public Commissioni As CCollection(Of Commissione)
        Public Richieste As CCollection(Of RichiestaCERQ)
        'Public Uscite As CCollection(Of Uscita)

        Public Sub New()
        End Sub

        Public Sub New(ByVal idPersona As Integer)
            Me.m_IDPersona = idPersona
            Me.Commissioni = Office.Commissioni.GetCommissioniByPersona(idPersona)
            Me.Richieste = Office.RichiesteCERQ.GetRichiesteByPersona(idPersona)
            'Me.Uscite = Office.RichiesteCERQ.GetRichiesteByPersona(idPersona)
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Commissioni"
                    Me.Commissioni = New CCollection(Of Commissione)
                    If (TypeOf (fieldValue) Is IEnumerable) Then Me.Commissioni.AddRange(fieldValue)
                Case "Richieste"
                    Me.Richieste = New CCollection(Of RichiestaCERQ)
                    If (TypeOf (fieldValue) Is IEnumerable) Then Me.Richieste.AddRange(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDPersona", Me.m_IDPersona)
            writer.WriteTag("Commissioni", Me.Commissioni)
            writer.WriteTag("Richieste", Me.Richieste)
        End Sub
    End Class


End Namespace