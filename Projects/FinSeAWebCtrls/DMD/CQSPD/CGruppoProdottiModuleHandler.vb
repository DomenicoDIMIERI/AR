Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
  
    Public Class CGruppoProdottiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.GruppiProdotto.GetItemById(id)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CGruppoProdottiCursor
            Return cursor
        End Function

        'Public Function addDocGrp() As String
        '    Dim IDdocIn As Integer
        '    Dim DocGrp As CDocumentoPerPratica
        '    Dim IDgrp As Integer
        '    Dim reqStates() As String
        '    Dim cboConsegnare
        '    Dim item As CDocumentoPerPraticaGrp
        '    Dim i As Integer
        '    IDdocIn = RPC.n2int(Me.GetParameter(renderer, "IDdocIn"))
        '    IDgrp = RPC.n2int(Me.GetParameter(renderer, "IDgrp"))
        '    reqStates = Split(RPC.n2str(Me.GetParameter(renderer, "reqStates")), ",")
        '    cboConsegnare = RPC.n2int(Me.GetParameter(renderer, "cboConsegnare"))
        '    item = New CDocumentoPerPraticaGrp
        '    DocGrp = CQSPD.DocsXPrat.GetDocGrpByID(IDdocIn)
        '    With item
        '        .DocPerPrat = DocGrp.Documento
        '        .IDGruppoProdotti = IDgrp
        '        For i = 0 To UBound(reqStates)
        '            Call .SetRequiredForState(Formats.ToInteger(reqStates(i)), True)
        '        Next
        '        .ConsegnareA = cboConsegnare
        '        .ConsegnareAAltro = ""
        '        .Stato = ObjectStatus.OBJECT_VALID
        '    End With
        '    If item.Save Then
        '        Return RPC.FormatID(GetID(item))
        '    Else
        '        Err.Raise(1, "CGruppoProdottiModuleHandler", "Errore nel salvataggio")
        '    End If
        'End Function


        Public Function GetDocumentiXGruppo(ByVal renderer As Object) As String
            Dim gid As Integer = RPC.n2int(GetParameter(renderer, "gid", 0))
            Dim gruppo As CGruppoProdotti = CQSPD.GruppiProdotto.GetItemById(gid)
            Return XML.Utils.Serializer.Serialize(gruppo.Documenti)
        End Function

        Public Function UnloadDocuments(ByVal renderer As Object) As String
            Dim gid As Integer = RPC.n2int(GetParameter(renderer, "gid", 0))
            Dim gruppo As CGruppoProdotti = CQSPD.GruppiProdotto.GetItemById(gid)
            If (gruppo IsNot Nothing) Then gruppo.Documenti.ReLoad()
            Return ""
        End Function

    End Class




End Namespace