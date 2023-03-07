Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite

Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils
Imports DMD.XML

Namespace Forms

    Public Class CComuniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CComuniCursor
            ret.Nome.SortOrder = SortEnum.SORT_ASC
            Return ret
        End Function


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Luoghi.Comuni.GetItemById(id)
        End Function



        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Provincia", "Provincia", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("CodiceISTAT", "Codice ISTAT", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("CodiceCatasto", "Codice Catasto", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("PuntoOperativo", "Punto Operativo", TypeCode.String, True))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(ByVal renderer As Object, item As Object, key As String) As Object
            Select Case key
                Case "PuntoOperativo" : Return DirectCast(item, CComune).NomePuntoOperativo
                Case Else : Return MyBase.GetColumnValue(renderer, item, key)
            End Select
        End Function

        Protected Overrides Sub SetColumnValue(ByVal renderer As Object, item As Object, key As String, ByVal value As Object)
            Select Case key
                Case "PuntoOperativo" : DirectCast(item, CComune).PuntoOperativo = Anagrafica.Uffici.GetItemByName(Formats.ToString(value))
                Case Else : MyBase.SetColumnValue(renderer, item, key, value)
            End Select
        End Sub

        Public Function GetItemByName(ByVal renderer As Object) As String
            Dim value As String = Trim(RPC.n2str(GetParameter(renderer, "name", "")))
            If (value = "") Then Return ""
            Dim item As CComune = Luoghi.Comuni.GetItemByName(value)
            If (item Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function getNomeESiglaByCAP(ByVal renderer As Object) As String
            Dim cap As String = Trim(RPC.n2str(GetParameter(renderer, "cap", "")))
            If (cap = "") Then Return ""
            For Each c As CComune In Luoghi.Comuni.LoadAll
                If (Strings.Compare(c.CAP, cap) = 0) Then Return XML.Utils.Serializer.SerializeString(c.CittaEProvincia)
            Next
            Return ""
        End Function

        Public Function getCAPByNomeESigla(ByVal renderer As Object) As String
            Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "text", "")))
            If (text = "") Then Return ""
            Dim nc As String = Luoghi.GetComune(text)
            Dim np As String = Luoghi.GetProvincia(text)
            If (nc = "" AndAlso np = "") Then Return ""
            For Each c As CComune In Luoghi.Comuni.LoadAll
                If Strings.Compare(nc, c.Nome) = 0 AndAlso (np = "" OrElse Strings.Compare(np, c.Provincia) = 0 OrElse Strings.Compare(np, c.Sigla) = 0) Then
                    Return XML.Utils.Serializer.SerializeString(c.CAP)
                End If
            Next
            Return ""
        End Function

        Public Function GetComuniByPrefisso(ByVal renderer As Object) As String
            Dim items As CCollection(Of CComune) = Anagrafica.Luoghi.Comuni.LoadAll
            Dim prefisso As String = RPC.n2str(Me.GetParameter(renderer, "prefisso", ""))
            Dim ret As New CCollection(Of CComune)

            For Each c As CComune In items
                If (c.Prefisso = prefisso) Then ret.Add(c)
            Next

            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function getCodiceCatastoByNomeESigla(ByVal renderer As Object) As String
            Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "text", "")))
            If (text = "") Then Return ""
            Dim nc As String = Luoghi.GetComune(text)
            Dim np As String = Luoghi.GetProvincia(text)
            If (nc = "" AndAlso np = "") Then Return ""
            For Each c As CComune In Luoghi.Comuni.LoadAll
                If Strings.Compare(nc, c.Nome) = 0 AndAlso (np = "" OrElse Strings.Compare(np, c.Sigla) = 0 OrElse Strings.Compare(np, c.Provincia) = 0) Then
                    Return XML.Utils.Serializer.SerializeString(c.CodiceCatasto)
                End If
            Next
            For Each n As CNazione In Luoghi.Nazioni.LoadAll
                If Strings.Compare(nc, n.Nome) = 0 Then
                    Return XML.Utils.Serializer.SerializeString(n.CodiceCatasto)
                End If
            Next
            Return ""
            'Dim cursor As New CComuniCursor()
            'cursor.Stato().Value = ObjectStatus.OBJECT_VALID
            'cursor.Nome.Value = nc
            'cursor.IgnoreRights = True
            'If (np <> "") Then cursor.WhereClauses.Add("[Sigla]=" & DBString(np) & " OR [Provincia]=" & DBString(np) & "")
            'Dim ret As CComune = cursor.Item
            'cursor.Reset()
            'If (ret Is Nothing) Then
            '    Return ""
            'Else
            '    Return XML.Utils.Serializer.SerializeString(ret.CodiceCatasto)
            'End If
        End Function

        Public Function GetNomeByCodCatasto(ByVal renderer As Object) As String
            Dim cod As String = Trim(RPC.n2str(GetParameter(renderer, "cod", "")))
            If (cod = "") Then Return ""
            For Each c As CComune In Luoghi.Comuni.LoadAll
                If Strings.Compare(c.CodiceCatasto, cod) = 0 Then
                    Return XML.Utils.Serializer.SerializeString(c.CittaEProvincia)
                End If
            Next
            Return ""
            'Dim cursor As New CComuniCursor()
            'cursor.Stato().Value = ObjectStatus.OBJECT_VALID
            'cursor.CodiceCatasto.Value = cod
            'Dim ret As CComune = cursor.Item
            'cursor.Reset()
            'If (ret Is Nothing) Then
            '    Return ""
            'Else
            '    Return XML.Utils.Serializer.SerializeString(ret.CittaEProvincia)
            'End If
        End Function

        Public Function GetComuniByCAP(ByVal renderer As Object) As String
            Dim cap As String = Trim(RPC.n2str(GetParameter(renderer, "cap", "")))
            Dim items As CCollection(Of CComune) = Luoghi.Comuni.GetComuniByCAP(cap)
            Return XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
        End Function

    End Class


End Namespace