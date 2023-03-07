Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Anagrafica
Imports DMD.WebSite
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Namespace Forms



    Public Class ControlPanelInfo
        Implements DMD.XML.IDMDXMLSerializable

        Public Uffici As CCollection(Of ControlPanelOfficeInfo)
        Public VisiteRicevute As CCollection(Of ContattoInAttesaInfo)
        Public Azioni As CCollection

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Carica tutte le informazioni iniziali
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Load(ByVal fromDate As Date)
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim t As Double = Timer

                Me.Uffici = New CCollection(Of ControlPanelOfficeInfo)
                Me.VisiteRicevute = New CCollection(Of ContattoInAttesaInfo)
                Me.Azioni = New CCollection

                Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                For Each ufficio As CUfficio In items
                    If (ufficio.IsValid) Then
                        Dim info As New ControlPanelOfficeInfo(ufficio)
                        info.Load(fromDate)
                        Me.Uffici.Add(info)
                    End If
                Next

                Debug.Print("CControlPanelOfficeInfo.Load1: " & (Timer - t))

                Dim dbSQL As String = "SELECT * FROM [tbl_Telefonate] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND ("
                dbSQL &= "([ClassName]='CTelefonata' AND ([Data]>=" & DBUtils.DBDate(fromDate) & " OR [StatoConversazione]=" & StatoConversazione.INCORSO & ")) OR "
                dbSQL &= "([ClassName]='CVisita' AND [Ricevuta]=True AND ([Data]>=" & DBUtils.DBDate(fromDate) & " OR [StatoConversazione]<>" & StatoConversazione.CONCLUSO & "))"
                dbSQL &= ")"

                dbRis = CustomerCalls.CRM.TelDB.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim c As CContattoUtente
                    If Formats.ToString(dbRis("ClassName")) = "CTelefonata" Then
                        c = New CTelefonata
                        CustomerCalls.CRM.TelDB.Load(c, dbRis)
                        Me.Azioni.Add(New ContattoInAttesaInfo(c))
                    Else
                        c = New CVisita
                        APPConn.Load(c, dbRis)
                        If (c.StatoConversazione = StatoConversazione.CONCLUSO) Then
                            Me.VisiteRicevute.Add(New ContattoInAttesaInfo(c))
                        Else
                            Me.Azioni.Add(New ContattoInAttesaInfo(c))
                        End If
                    End If

                End While
                dbRis.Dispose() : dbRis = Nothing

                Debug.Print("CControlPanelOfficeInfo.Load2: " & (Timer - t))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Public Sub Update(ByVal fromDate As Date)

        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Uffici"
                    Me.Uffici = New CCollection(Of ControlPanelOfficeInfo)
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.Uffici.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is ControlPanelOfficeInfo) Then
                        Me.Uffici.Add(fieldValue)
                    End If
                Case "VisiteRicevute"
                    Me.VisiteRicevute = New CCollection(Of ContattoInAttesaInfo)
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.VisiteRicevute.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is CVisita) Then
                        Me.VisiteRicevute.Add(fieldValue)
                    End If
                Case "Azioni"
                    Me.Azioni = New CCollection
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.Azioni.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is Object) Then
                        Me.Azioni.Add(fieldValue)
                    End If
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("Uffici", Me.Uffici)
            writer.WriteTag("VisiteRicevute", Me.VisiteRicevute)
            writer.WriteTag("Azioni", Me.Azioni)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace