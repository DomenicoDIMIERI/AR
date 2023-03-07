Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Public NotInheritable Class Office

    Private Shared m_Module As CModule = Nothing
    Private Shared m_Initialized As Boolean = False
    Private Shared m_Database As CDBConnection = Nothing

    Shared Sub New()

        'Initialize()
    End Sub

    Public Shared Property Database As CDBConnection
        Get
            If (m_Database Is Nothing) Then Return Databases.APPConn
            Return m_Database
        End Get
        Set(value As CDBConnection)
            Terminate()
            m_Database = value
            Initialize()
        End Set
    End Property

    Private Shared Sub HandlePersonaMerged(ByVal e As MergePersonaEventArgs)
        SyncLock DMD.Anagrafica.lock
            Dim mi As CMergePersona = e.MI
            Dim persona As CPersona = mi.Persona1
            Dim persona1 As CPersona = mi.Persona2

            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim rec As CMergePersonaRecord

            If TypeOf (persona) Is CPersonaFisica Then
                Try
                    'Tabella tbl_OfficeCommissioni 
                    dbSQL = "SELECT [ID], [NomePersona] FROM [tbl_OfficeCommissioni] WHERE [IDPersona]=" & GetID(persona1)
                    dbRis = Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_OfficeCommissioni"
                        rec.FieldName = "IDPersona"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.params.SetItemByKey("NomePersona", Formats.ToString(dbRis("NomePersona")))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_OfficeCommissioni] SET [IDPersona]=", GetID(persona), ", [NomePersona]=", DBUtils.DBString(persona.Nominativo), " WHERE [IDPersona]=", GetID(persona1)))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

                Try
                    'Tabella tbl_OfficeEstrattiC 
                    dbSQL = "SELECT [ID], [NomeCliente] FROM [tbl_OfficeEstrattiC] WHERE [IDCliente]=" & GetID(persona1)
                    dbRis = Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_OfficeEstrattiC"
                        rec.FieldName = "IDCliente"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.params.SetItemByKey("NomeCliente", Formats.ToString(dbRis("NomeCliente")))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_OfficeEstrattiC] SET [IDCliente]=", GetID(persona), ", [NomeCliente]=", DBUtils.DBString(persona.Nominativo), " WHERE [IDCliente]=", GetID(persona1)))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

                Try
                    'Tabella tbl_CQSPDRichCERQ 
                    dbSQL = "SELECT [ID], [NomeCliente] FROM [tbl_CQSPDRichCERQ] WHERE [IDCliente]=" & GetID(persona1)
                    dbRis = Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_CQSPDRichCERQ"
                        rec.FieldName = "IDCliente"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.params.SetItemByKey("NomeCliente", Formats.ToString(dbRis("NomeCliente")))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_CQSPDRichCERQ] SET [IDCliente]=", GetID(persona), ", [NomeCliente]=", DBUtils.DBString(persona.Nominativo), " WHERE [IDCliente]=", GetID(persona1)))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try
            Else
                Try
                    'Tabella tbl_OfficeCommissioni 
                    dbSQL = "SELECT [ID], [NomeAzienda] FROM [tbl_OfficeCommissioni] WHERE [IDAzienda]=" & GetID(persona1)
                    dbRis = Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_OfficeCommissioni"
                        rec.FieldName = "IDAzienda"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.params.SetItemByKey("NomeAzienda", Formats.ToString(dbRis("NomeAzienda")))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_OfficeCommissioni] SET [IDAzienda]=", GetID(persona), ", [NomeAzienda]=", DBUtils.DBString(persona.Nominativo), " WHERE [IDAzienda]=", GetID(persona1)))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

                Try
                    'Tabella tbl_OfficeEstrattiC 
                    dbSQL = "SELECT [ID], [NomeAmministrazione] FROM [tbl_OfficeEstrattiC] WHERE [IDAmministrazione]=" & GetID(persona1)
                    dbRis = Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_OfficeEstrattiC"
                        rec.FieldName = "IDAmministrazione"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.params.SetItemByKey("NomeAmministrazione", Formats.ToString(dbRis("NomeAmministrazione")))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_OfficeEstrattiC] SET [IDAmministrazione]=", GetID(persona), ", [NomeAmministrazione]=", DBUtils.DBString(persona.Nominativo), " WHERE [IDAmministrazione]=", GetID(persona1)))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

                Try
                    'Tabella tbl_CQSPDRichCERQ 
                    dbSQL = Strings.JoinW("SELECT [ID], [NomeAmministrazione] FROM [tbl_CQSPDRichCERQ] WHERE [IDAmministrazione]=", GetID(persona1))
                    dbRis = Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_CQSPDRichCERQ"
                        rec.FieldName = "IDAmministrazione"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.params.SetItemByKey("NomeAmministrazione", Formats.ToString(dbRis("NomeAmministrazione")))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_CQSPDRichCERQ] SET [IDAmministrazione]=", GetID(persona), ", [NomeAmministrazione]=", DBUtils.DBString(persona.Nominativo), " WHERE [IDAmministrazione]=", GetID(persona1)))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

            End If

            Try
                'Tabella tbl_OfficeCandidature 
                dbSQL = "SELECT [ID], [NomeCandidato] FROM [tbl_OfficeCandidature] WHERE [IDCandidato]=" & GetID(persona1)
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_OfficeCandidature"
                    rec.FieldName = "IDCandidato"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.params.SetItemByKey("NomeCandidato", Formats.ToString(dbRis("NomeCandidato")))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_OfficeCandidature] SET [IDCandidato]=", GetID(persona), ", [NomeCandidato]=", DBUtils.DBString(persona.Nominativo), " WHERE [IDCandidato]=", GetID(persona1)))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_OfficeCurricula 
                dbSQL = Strings.JoinW("SELECT [ID], [NomePersona] FROM [tbl_OfficeCurricula] WHERE [IDPersona]=", GetID(persona1))
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_OfficeCurricula"
                    rec.FieldName = "IDPersona"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.params.SetItemByKey("NomePersona", Formats.ToString(dbRis("NomePersona")))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_OfficeCurricula] SET [IDPersona]=", GetID(persona), ", [NomePersona]=", DBUtils.DBString(persona.Nominativo), " WHERE [IDPersona]=", GetID(persona1)))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_SupportTickets 
                dbSQL = "SELECT [ID], [NomeCliente] FROM [tbl_SupportTickets] WHERE [IDCliente]=" & GetID(persona1)
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_SupportTickets"
                    rec.FieldName = "IDCliente"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.params.SetItemByKey("NomeCliente", Formats.ToString(dbRis("NomeCliente")))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Database.ExecuteCommand("UPDATE [tbl_SupportTickets] SET [IDCliente]=" & GetID(persona) & ", [NomeCliente]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_OfficeSpedizioni 
                dbSQL = "SELECT [ID], [NomeMittente] FROM [tbl_OfficeSpedizioni] WHERE [IDMittente]=" & GetID(persona1)
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_OfficeSpedizioni"
                    rec.FieldName = "IDMittente"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.params.SetItemByKey("NomeMittente", Formats.ToString(dbRis("NomeMittente")))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Database.ExecuteCommand("UPDATE [tbl_OfficeSpedizioni] SET [IDMittente]=" & GetID(persona) & ", [NomeMittente]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDMittente]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_OfficeSpedizioni 
                dbSQL = "SELECT [ID], [NomeDestinatario] FROM [tbl_OfficeSpedizioni] WHERE [IDDestinatario]=" & GetID(persona1)
                dbRis = Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_OfficeSpedizioni"
                    rec.FieldName = "IDDestinatario"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.params.SetItemByKey("NomeDestinatario", Formats.ToString(dbRis("NomeDestinatario")))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Database.ExecuteCommand("UPDATE [tbl_OfficeSpedizioni] SET [IDDestinatario]=" & GetID(persona) & ", [NomeDestinatario]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDDestinatario]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

        End SyncLock
    End Sub

    Private Shared Function GetAffectedRecors(ByVal mi As CMergePersona, ByVal tableName As String, ByVal fieldName As String) As String
        Dim ret As New System.Text.StringBuilder
        For Each rec As CMergePersonaRecord In mi.TabelleModificate
            If (rec.NomeTabella = tableName AndAlso rec.RecordID <> 0 AndAlso rec.FieldName = fieldName) Then
                If (ret.Length > 0) Then ret.Append(",")
                ret.Append(DBUtils.DBNumber(rec.RecordID))
            End If
        Next
        Return ret.ToString
    End Function

    Private Shared Sub HandlePeronaUnMerged(ByVal e As MergePersonaEventArgs)
        SyncLock DMD.Anagrafica.lock
            Dim mi As CMergePersona = e.MI
            Dim persona As CPersona = mi.Persona1
            Dim persona1 As CPersona = mi.Persona2
            Dim items As String


            If TypeOf (persona) Is CPersonaFisica Then
                Try
                    'Tabella Login
                    items = GetAffectedRecors(mi, "tbl_OfficeCommissioni", "IDPersona")
                    If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_OfficeCommissioni] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

                Try
                    'Tabella tbl_OfficeEstrattiC 
                    items = GetAffectedRecors(mi, "tbl_OfficeEstrattiC", "IDCliente")
                    If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_OfficeEstrattiC] SET [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

                Try
                    'Tabella tbl_CQSPDRichCERQ 
                    items = GetAffectedRecors(mi, "tbl_CQSPDRichCERQ", "IDCliente")
                    If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] SET [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

            Else
                Try
                    'Tabella tbl_OfficeCommissioni 
                    items = GetAffectedRecors(mi, "tbl_OfficeCommissioni", "IDAzienda")
                    If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_OfficeCommissioni] SET [IDAzienda]=" & GetID(persona1) & ", [NomeAzienda]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

                Try
                    'Tabella tbl_OfficeEstrattiC 
                    items = GetAffectedRecors(mi, "tbl_OfficeEstrattiC", "IDAmministrazione")
                    If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_OfficeEstrattiC] SET [IDAmministrazione]=" & GetID(persona1) & ", [NomeAmministrazione]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

                Try
                    'Tabella tbl_CQSPDRichCERQ 
                    items = GetAffectedRecors(mi, "tbl_CQSPDRichCERQ", "IDAmministrazione")
                    If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_CQSPDRichCERQ] SET [IDAmministrazione]=" & GetID(persona1) & ", [NomeAmministrazione]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

            End If

            Try
                'Tabella tbl_OfficeCandidature 
                items = GetAffectedRecors(mi, "tbl_OfficeCandidature", "IDCandidato")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_OfficeCandidature] SET [IDCandidato]=" & GetID(persona1) & ", [NomeCandidato]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_OfficeCurricula 
                items = GetAffectedRecors(mi, "tbl_OfficeCurricula", "IDPersona")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_OfficeCurricula] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_SupportTickets 
                items = GetAffectedRecors(mi, "tbl_SupportTickets", "IDCliente")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_SupportTickets] SET [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_OfficeSpedizioni 
                items = GetAffectedRecors(mi, "tbl_OfficeSpedizioni", "IDMittente")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_OfficeSpedizioni] SET [IDMittente]=" & GetID(persona1) & ", [NomeMittente]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_OfficeSpedizioni 
                items = GetAffectedRecors(mi, "tbl_OfficeSpedizioni", "IDDestinatario")
                If (items <> "") Then Database.ExecuteCommand("UPDATE [tbl_OfficeSpedizioni] SET [IDDestinatario]=" & GetID(persona1) & ", [NomeDestinatario]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

        End SyncLock
    End Sub

    Private Shared Sub Initialize()
        If (m_Initialized) Then Exit Sub
        m_Initialized = True

        Sistema.Types.RegisteredTypeProviders.Add("Commissione", AddressOf Office.Commissioni.GetItemById)
        Sistema.Types.RegisteredTypeProviders.Add("Uscita", AddressOf Office.Uscite.GetItemById)
        Sistema.Types.RegisteredTypeProviders.Add("Utenza", AddressOf Office.Utenze.GetItemById)


        Office.Magazzini.Initialize()
        Office.CategorieArticoli.Initialize()
        Office.Listini.Initialize()
        Office.MarcheArticoli.Initialize()
        Office.Articoli.Initialize()
        Office.OggettiDaSpedire.Initialize()
        Office.Spedizioni.Initialize()

        AddHandler Anagrafica.PersonaMerged, AddressOf HandlePersonaMerged
        AddHandler Anagrafica.PersonaUnMerged, AddressOf HandlePeronaUnMerged

    End Sub

    Private Shared Sub Terminate()
        If (Not m_Initialized) Then Exit Sub

        RemoveHandler Anagrafica.PersonaMerged, AddressOf HandlePersonaMerged
        RemoveHandler Anagrafica.PersonaUnMerged, AddressOf HandlePeronaUnMerged

        Office.Magazzini.Terminate()
        Office.CategorieArticoli.Terminate()
        Office.Listini.Terminate()
        Office.MarcheArticoli.Terminate()
        Office.Articoli.Terminate()
        Office.OggettiDaSpedire.Terminate()
        Office.Spedizioni.Terminate()


        Sistema.Types.RegisteredTypeProviders.RemoveByKey("Commissione")
        Sistema.Types.RegisteredTypeProviders.RemoveByKey("Uscita")


        m_Initialized = False
    End Sub


    Public Shared ReadOnly Property [Module] As CModule
        Get
            If (m_Module) Is Nothing Then m_Module = InitModule()
            Return m_Module
        End Get
    End Property

    Private Shared Function InitModule() As CModule
        Dim ret As CModule = Sistema.Modules.GetItemByName("modOffice")
        If (ret Is Nothing) Then
            ret = New CModule("modOffice")
            ret.DisplayName = "Ufficio"
            ret.Description = "Ufficio"
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.Save()
        End If
        Return ret
    End Function



End Class


