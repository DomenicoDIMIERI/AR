Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils


Imports DMD.Web


Namespace Forms

    Public Class ListeRicontattiHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CListaRicontattiCursor
        End Function


        Public Function SaveLista(ByVal renderer As Object) As String
            Dim nome As String = Strings.Trim(RPC.n2str(GetParameter(renderer, "nml", "")))
            Dim sovrascrivi As Boolean = RPC.n2bool(GetParameter(renderer, "ovw", ""))
            Dim data As Date = RPC.n2date(GetParameter(renderer, "dt", ""))
            Dim note As String = Strings.Trim(RPC.n2str(GetParameter(renderer, "note", "")))
            Dim persone As CCollection = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "items", "")))

            If (nome = "") Then Throw New ArgumentNullException("nml", "Il nome della lista non può essere vuoto")
            Dim list As CListaRicontatti = Anagrafica.ListeRicontatto.GetItemByName(nome)
            If (list Is Nothing) Then
                list = New CListaRicontatti()
                list.Name = nome
                list.Descrizione = note
                list.DataInserimento = DateUtils.Now
            End If
            list.Stato = ObjectStatus.OBJECT_VALID
            list.Save()

            Dim ricontatti As New CCollection(Of ListaRicontattoItem)
            For Each p As CPersona In persone
                Dim ric As New ListaRicontattoItem()
                ric.Persona = p
                ric.DataPrevista = data
                ric.NomeLista = list.Name
                ric.Note = note
                'ric.setCategoria(list.getName());
                ric.Stato = ObjectStatus.OBJECT_VALID
                ric.StatoRicontatto = StatoRicontatto.PROGRAMMATO
                ric.Save()
                ricontatti.Add(ric)
            Next

            Return XML.Utils.Serializer.Serialize(ricontatti)
        End Function
    End Class



End Namespace