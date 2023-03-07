Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.XML

Namespace Forms


    Public Class CPersoneFisicheModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CPersonaCursor
            ret.Nominativo.SortOrder = SortEnum.SORT_ASC
            ret.TipoPersona.Value = TipoPersona.PERSONA_FISICA
            Return ret
        End Function


        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Nominativo", "Nominativo", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Cognome", "Cognome", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("CodiceFiscale", "Codice Fiscale", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("PartitaIVA", "Partita IVA", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("DataNascita", "Data di Nascita", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("LuogoNascita", "Luogo di Nascita", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Residenza", "Residenza", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Domicilio", "Domicilio", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("TelefonoCasa", "Telefono Casa", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("TelefonoUfficio", "Telefono Ufficio", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Cellulare", "Cellulare", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Fax", "Fax", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("eMail", "eMail", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("eMail1", "Altra eMail", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomeFonte", "Nome Fonte", TypeCode.String, True))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(ByVal renderer As Object, item As Object, key As String) As Object
            Dim p As CPersonaFisica = item
            Select Case key
                Case "LuogoNascita" : Return p.NatoA.ToString
                Case "Residenza" : Return p.ResidenteA.ToString
                Case "Domicilio" : Return p.DomiciliatoA.ToString
                    'Case "TelefonoCasa" : Return Formats.FormatPhoneNumber(p.TelefonoCasa(0).Valore)
                    'Case "TelefonoUfficio" : Return Formats.FormatPhoneNumber(p.TelefonoUfficio(0).Valore)
                    'Case "Cellulare" : Return Formats.FormatPhoneNumber(p.Cellulare(0).Valore)
                    'Case "Fax" : Return Formats.FormatPhoneNumber(p.Fax(0).Valore)
                    'Case "eMail" : Return p.eMail(0).Valore
                    'Case "eMail1" : Return p.eMail(1).Valore
                Case Else : Return MyBase.GetColumnValue(renderer, item, key)
            End Select
        End Function

        Protected Overrides Sub SetColumnValue(ByVal renderer As Object, item As Object, key As String, value As Object)
            Dim p As CPersonaFisica = item
            Select Case key
                Case "LuogoNascita" : p.NatoA.Parse(value)
                Case "Residenza" : p.ResidenteA.Parse(value)
                Case "Domicilio" : p.DomiciliatoA.Parse(value)
                    'Case "TelefonoCasa" : p.TelefonoCasa(0).Valore = Formats.ParsePhoneNumber(value)
                    'Case "TelefonoUfficio" : p.TelefonoUfficio(0).Valore = Formats.ParsePhoneNumber(value)
                    'Case "Cellulare" : p.Cellulare(0).Valore = Formats.ParsePhoneNumber(value)
                    'Case "Fax" : p.Fax(0).Valore = Formats.ParsePhoneNumber(value)
                    'Case "eMail" : p.eMail(0).Valore = Trim(value)
                    'Case "eMail1" : p.eMail(1).Valore = Trim(value)
                Case Else : MyBase.SetColumnValue(renderer, item, key, value)
            End Select

        End Sub


        Protected Overrides Function FindExportedItem(curs As DBObjectCursorBase, info As DBReader) As Object
            Dim cursor As CPersonaFisicaCursor = curs
            Dim ret As CPersona
            Dim id As Integer = info.GetValue(Of Integer)("ID", 0)
            If (id = 0) Then
                Dim cf As String = Formats.ParseCodiceFiscale(info.GetValue(Of String)("CodiceFiscale", ""))
                If (cf <> "") Then
                    cursor.Clear()
                    cursor.CodiceFiscale.Value = cf
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    ret = cursor.Item
                    cursor.Reset1()
                Else
                    Dim nome As String = Trim(info.GetValue(Of String)("Nome", ""))
                    Dim cognome As String = Trim(info.GetValue(Of String)("Cognome", ""))
                    Dim contatto As String
                    Dim contatti() As String = {"TelefonoCasa", "TelefonoUfficio", "Cellulare", "Fax"}
                    ret = Nothing
                    For Each nomeContatto As String In contatti
                        contatto = Formats.ParsePhoneNumber(info.GetValue(Of String)(nomeContatto, ""))
                        If (contatto <> "") Then
                            cursor.Clear()
                            cursor.Nome.Value = nome
                            cursor.Cognome.Value = cognome
                            cursor.Telefono.Value = contatto
                        End If
                        ret = cursor.Item
                        cursor.Reset1()
                        If (ret IsNot Nothing) Then Exit For
                    Next
                    If (ret Is Nothing) Then
                        contatti = {"eMail"}
                        For Each nomeContatto As String In contatti
                            contatto = Trim(info.GetValue(Of String)(nomeContatto, ""))
                            If (contatto <> "") Then
                                cursor.Clear()
                                cursor.Nome.Value = nome
                                cursor.Cognome.Value = cognome
                                cursor.eMail.Value = contatto
                            End If
                            ret = cursor.Item
                            cursor.Reset1()
                            If (ret IsNot Nothing) Then Exit For
                        Next
                    End If
                End If
            Else
                cursor.Clear()
                cursor.ID.Value = id
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                ret = cursor.Item
                cursor.Reset1()
            End If

            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Public Function getImpieghi(ByVal renderer As Object) As String
            Dim aid As Integer = RPC.n2int(Me.GetParameter(renderer, "aid", 0))
            Dim persona As CPersonaFisica = Anagrafica.Persone.GetItemById(aid)
            If (persona Is Nothing) Then Throw New ArgumentNullException("Persona")
            If (persona.Impieghi.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(persona.Impieghi.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

    End Class

End Namespace