Imports DMD
Imports DMD.Sistema

Public Class IntervalloLavoro
    Implements DMD.XML.IDMDXMLSerializable

    Public Attivo As Boolean                'Vero se l'intervallo è abilitato
    Public Nome As String                   'Nome dell'intervallo di lavoro
    Public OraInizio As Date?               'Ora di inizio dell'intervallo di lavoro
    Public OraFine As Date?                 'Ora di fine dell'intervallo di lavoro
    Public Consenti As Boolean              'Vero se l'intervallo di lavoro è consentito
    Public Nega As Boolean                  'Vero se l'intervallo di lavoro è negato
    Public Tolleranza As Integer            'Tolleranza in minuti rispetto agli orari di ingresso/uscita
    Public AzioniInizio As String()         'Elenco delle azioni da eseguire quando l'intervallo diventa attivo
    Public AzioniFine As String()           'Elenco delle azioni da eseguire quando l'intervallo scade

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.Attivo = True
        Me.Nome = ""
        Me.OraInizio = Nothing
        Me.OraFine = Nothing
        Me.Consenti = True
        Me.Nega = False
        Me.Tolleranza = 15
        Me.AzioniInizio = {}
        Me.AzioniFine = {"Soft Shutdown"}
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Nome
    End Function

    Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Attivo" : Me.Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue).Value
            Case "Nome" : Me.Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "OraInizio" : Me.OraInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "OraFine" : Me.OraFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
            Case "Consenti" : Me.Consenti = XML.Utils.Serializer.DeserializeBoolean(fieldValue).Value
            Case "Nega" : Me.Nega = XML.Utils.Serializer.DeserializeBoolean(fieldValue).Value
            Case "Tolleranza" : Me.Tolleranza = XML.Utils.Serializer.DeserializeInteger(fieldValue).Value
            Case "AzioniInizio"
                If (TypeOf (fieldValue) Is Array) Then
                    Me.AzioniInizio = CType(fieldValue, String())
                Else
                    Dim str As String = Trim(CStr(fieldValue))
                    If (str <> "") Then Me.AzioniInizio = {str}
                End If
            Case "AzioniFine"
                If (TypeOf (fieldValue) Is Array) Then
                    Me.AzioniFine = CType(fieldValue, String())
                Else
                    Dim str As String = Trim(CStr(fieldValue))
                    If (str <> "") Then Me.AzioniFine = {str}
                End If
        End Select
    End Sub

    Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Attivo", Me.Attivo)
        writer.WriteAttribute("Nome", Me.Nome)
        writer.WriteAttribute("OraInizio", Me.OraInizio)
        writer.WriteAttribute("OraFine", Me.OraFine)
        writer.WriteAttribute("Consenti", Me.Consenti)
        writer.WriteAttribute("Nega", Me.Nega)
        writer.WriteAttribute("Tolleranza", Me.Tolleranza)
        writer.WriteTag("AzioniInizio", Me.AzioniInizio)
        writer.WriteTag("AzioniFine", Me.AzioniFine)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
