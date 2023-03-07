Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.CQSPD
Imports DMD.CustomerCalls



Namespace Forms


    Partial Public Class CAnagraficaModuleHandler


        <Serializable> _
        Public Class CPopUpInfoPersona
            Implements DMD.XML.IDMDXMLSerializable

            Public Persona As CPersona
            Public Estinzioni As CCollection
            Public UltimaChiamata As CContattoUtente

            Public Sub New()
                DMD.DMDObject.IncreaseCounter(Me)
                Me.Persona = Nothing
                Me.Estinzioni = Nothing
                Me.UltimaChiamata = Nothing
            End Sub

            Public Sub New(ByVal persona As CPersona)
                If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
                Me.Persona = persona
                Me.UltimaChiamata = CRM.GetUltimoContatto(persona)
                Me.Estinzioni = New CCollection
                Me.Estinzioni.AddRange(CQSPD.Estinzioni.GetEstinzioniByPersona(persona))
            End Sub


            Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
                Select Case fieldName
                    Case "Persona" : Me.Persona = fieldValue
                    Case "Estinzioni" : Me.Estinzioni = fieldValue
                    Case "UltimaChiamata" : Me.UltimaChiamata = fieldValue
                End Select
            End Sub

            Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
                writer.WriteTag("Persona", Me.Persona)
                writer.WriteTag("UltimaChiamata", Me.UltimaChiamata)
                writer.WriteTag("Estinzioni", Me.Estinzioni)
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class


    End Class


End Namespace