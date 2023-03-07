Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

#Const usa_tbl_RicontattiQuick = False


Public NotInheritable Class Anagrafica
    Private Shared m_Module As CModule


 
    ''' <summary>
    ''' Evento generato quando un oggetto CPersona viene salvato per la prima volta con lo stato Valid
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event PersonaCreated(ByVal e As PersonaEventArgs)

    ''' <summary>
    ''' Evento generato quando viene effettuato il salvataggio di un oggetto CPersona
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event PersonaModified(ByVal e As PersonaEventArgs)

    ''' <summary>
    ''' Evento generato quando un oggetto CPersona viene salvato per la prima volta con lo stato Deleted
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event PersonaDeleted(ByVal e As PersonaEventArgs)

    ''' <summary>
    ''' Evento generato quando due oggetti CPersona vengono unifificati attraverso il metodo Merge
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event PersonaMerged(ByVal e As MergePersonaEventArgs)

    ''' <summary>
    ''' Evento generato quando viene annullata l'unione di due anagrafiche
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event PersonaUnMerged(ByVal e As MergePersonaEventArgs)

    ''' <summary>
    ''' Evento generato quando viene chiamato il metodo Transfer su un oggetto CPersona per trasferire la competenza ad un ufficio diverso
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event PersonaTransferred(ByVal e As TransferPersonaEventArgs)

    ''' <summary>
    ''' Lock sul modulo Anagrafica
    ''' </summary>
    Public Shared ReadOnly lock As New Object


    Private Sub New()
    End Sub

    Shared Sub New()
        AddHandler Anagrafica.PersonaMerged, AddressOf HandlePeronaMerged
        AddHandler Anagrafica.PersonaUnMerged, AddressOf HandlePeronaUnMerged
        AddHandler Anagrafica.PersonaModified, AddressOf HandlePeronaModified
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Shared ReadOnly Property [Module] As CModule
        Get
            If m_Module Is Nothing Then m_Module = Sistema.Modules.GetItemByName("Anagrafica")
            Return m_Module
        End Get
    End Property


    Private Shared Sub HandlePeronaMerged(ByVal e As MergePersonaEventArgs)
        SyncLock lock
            Dim mi As CMergePersona = e.MI
            Dim persona As CPersona = mi.Persona1
            Dim persona1 As CPersona = mi.Persona2
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim rec As CMergePersonaRecord
            Dim dbSQL As String

            Try
                'Tabella Login
                dbSQL = Strings.JoinW("SELECT [ID] FROM [Login] WHERE [Persona]=", GetID(persona1))
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "Login"
                    rec.FieldName = "Persona"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand(Strings.JoinW("UPDATE [Login] SET [Persona]=", GetID(persona), " WHERE [Persona]=", GetID(persona1)))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_Annotazioni
                dbSQL = "SELECT [ID] FROM [tbl_Annotazioni] WHERE [OwnerType]='" & TypeName(persona) & "' And [OwnerID]=" & GetID(persona1)
                dbRis = Sistema.Annotazioni.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_Annotazioni"
                    rec.FieldName = "OwnerID"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Sistema.Annotazioni.Database.ExecuteCommand("UPDATE [tbl_Annotazioni] SET [OwnerID]=" & GetID(persona) & " WHERE [OwnerType]='" & TypeName(persona) & "' And [OwnerID]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_Attachments
                dbSQL = "SELECT [ID] FROM [tbl_Attachments] WHERE [OwnerType]='" & TypeName(persona) & "' And [OwnerID]=" & GetID(persona1)
                dbRis = Sistema.Attachments.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_Attachments"
                    rec.FieldName = "OwnerID"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Sistema.Attachments.Database.ExecuteCommand("UPDATE [tbl_Attachments] SET [OwnerID]=" & GetID(persona) & " WHERE [OwnerType]='" & TypeName(persona) & "' And [OwnerID]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_DocumentiIdentita
                dbSQL = "SELECT [ID] FROM [tbl_DocumentiIdentita] WHERE [Persona]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_DocumentiIdentita"
                    rec.FieldName = "Persona"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_DocumentiIdentita] SET [Persona]=" & GetID(persona) & " WHERE [Persona]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            If TypeOf (persona) Is CPersonaFisica Then
                Try
                    'Tabella tbl_Impiegati
                    dbSQL = "SELECT [ID] FROM [tbl_Impiegati] WHERE [Persona]=" & GetID(persona1)
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_Impiegati"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.FieldName = "Persona"
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    APPConn.ExecuteCommand("UPDATE [tbl_Impiegati] SET [Persona]=" & GetID(persona) & ", [NomePersona]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [Persona]=" & GetID(persona1))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try
            Else
                Try
                    'Tabella tbl_Impiegati (Azienda)
                    dbSQL = "SELECT [ID] FROM [tbl_Impiegati] WHERE [Azienda]=" & GetID(persona1)
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    While dbRis.Read
                        mi.Add("tbl_Impiegati", "Azienda", Formats.ToInteger(dbRis("ID")))
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    APPConn.ExecuteCommand("UPDATE [tbl_Impiegati] SET [Azienda]=" & GetID(persona) & ", [NomeAzienda]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [Azienda]=" & GetID(persona1))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

                Try
                    'Tabella tbl_Impiegati (Ente Pagante)
                    dbSQL = "SELECT [ID] FROM [tbl_Impiegati] WHERE [IDEntePagante]=" & GetID(persona1)
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_Impiegati"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.FieldName = "IDEntePagante"
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    APPConn.ExecuteCommand("UPDATE [tbl_Impiegati] SET [IDEntePagante]=" & GetID(persona) & ", [NomeEntePagante]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDEntePagante]=" & GetID(persona1))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

                Try
                    'Tabella tbl_Persone (Azienda)
                    dbSQL = "SELECT [ID] FROM [tbl_Persone] WHERE [IMP_IDAzienda]=" & GetID(persona1)
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_Persone"
                        rec.FieldName = "IMP_IDAzienda"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    APPConn.ExecuteCommand("UPDATE [tbl_Persone] SET [IMP_IDAzienda]=" & GetID(persona) & ", [IMP_NomeAzienda]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IMP_IDAzienda]=" & GetID(persona1))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

                Try
                    'Tabella tbl_Persone (EntePagante)
                    dbSQL = "SELECT [ID] FROM [tbl_Persone] WHERE [IMP_IDEntePagante]=" & GetID(persona1)
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    While dbRis.Read
                        rec = New CMergePersonaRecord
                        rec.NomeTabella = "tbl_Persone"
                        rec.RecordID = Formats.ToInteger(dbRis("ID"))
                        rec.FieldName = "IMP_IDEntePagante"
                        mi.TabelleModificate.Add(rec)
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                    APPConn.ExecuteCommand("UPDATE [tbl_Persone] SET [IMP_IDEntePagante]=" & GetID(persona) & ", [IMP_NomeEntePagante]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IMP_IDEntePagante]=" & GetID(persona1))
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try

            End If

            Try
                'Tabella tbl_Indirizzi 
                dbSQL = "SELECT [ID] FROM [tbl_Indirizzi] WHERE [Persona]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_Indirizzi"
                    rec.FieldName = "Persona"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_Indirizzi] SET [Persona]=" & GetID(persona) & " WHERE [Persona]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_Contatti 
                dbSQL = "SELECT [ID] FROM [tbl_Contatti] WHERE [Persona]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_Contatti"
                    rec.FieldName = "Persona"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_Contatti] SET [Persona]=" & GetID(persona) & " WHERE [Persona]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_Ricontatti 
                dbSQL = "SELECT [ID] FROM [tbl_Ricontatti] WHERE [IDPersona]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_Ricontatti"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.FieldName = "IDPersona"
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_Ricontatti] SET [IDPersona]=" & GetID(persona) & ", [NomePersona]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

#If usa_tbl_RicontattiQuick Then
            Try
                'Tabella tbl_RicontattiQuick 
                dbSQL = "SELECT [ID] FROM [tbl_RicontattiQuick] WHERE [IDPersona]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_RicontattiQuick"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.FieldName = "IDPersona"
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_RicontattiQuick] SET [IDPersona]=" & GetID(persona) & ", [NomePersona]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
#End If

            Try
                'Tabella tbl_ListeRicontattoItems 
                dbSQL = "SELECT [ID] FROM [tbl_ListeRicontattoItems] WHERE [IDPersona]=" & GetID(persona1)
                dbRis = Anagrafica.ListeRicontatto.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_ListeRicontattoItems"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.FieldName = "IDPersona"
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Anagrafica.ListeRicontatto.Database.ExecuteCommand("UPDATE [tbl_ListeRicontattoItems] SET [IDPersona]=" & GetID(persona) & ", [NomePersona]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_PersoneRelazioni (IDPersona1) 
                dbSQL = "SELECT [ID] FROM [tbl_PersoneRelazioni] WHERE [IDPersona1]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_PersoneRelazioni"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.FieldName = "IDPersona1"
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_PersoneRelazioni] SET [IDPersona1]=" & GetID(persona) & ", [NomePersona1]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona1]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_PersoneRelazioni (IDPersona2) 
                dbSQL = "SELECT [ID] FROM [tbl_PersoneRelazioni] WHERE [IDPersona2]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_PersoneRelazioni"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.FieldName = "IDPersona2"
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_PersoneRelazioni] SET [IDPersona2]=" & GetID(persona) & ", [NomePersona2]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona2]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_PersoneConsensi 
                dbSQL = "SELECT [ID] FROM [tbl_PersoneConsensi] WHERE [IDPersona]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_PersoneConsensi"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.FieldName = "IDPersona"
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_PersoneConsensi] SET [IDPersona]=" & GetID(persona) & ", [NomePersona]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_SecurityTokens 
                dbSQL = "SELECT [ID] FROM [tbl_SecurityTokens] WHERE [TokenSourceName]=" & DBUtils.DBString(TypeName(persona1)) & " AND [TokenSourceID]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = mi.Add("tbl_SecurityTokens", "TokenSourceID", Formats.ToInteger(dbRis("ID")))
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_SecurityTokens] SET [TokenSourceID]=" & GetID(persona) & " WHERE [TokenSourceName]=" & DBUtils.DBString(TypeName(persona1)) & " AND [TokenSourceID]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_ADVResults 
                dbSQL = "SELECT [ID] FROM [tbl_ADVResults] WHERE [IDDestinatario]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_ADVResults"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.FieldName = "IDDestinatario"
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_ADVResults] SET [IDDestinatario]=" & GetID(persona) & ", [NomeDestinatario]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDDestinatario]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_TaskLavorazione 
                dbSQL = "SELECT [ID] FROM [tbl_TaskLavorazione] WHERE [IDCliente]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = New CMergePersonaRecord
                    rec.NomeTabella = "tbl_TaskLavorazione"
                    rec.RecordID = Formats.ToInteger(dbRis("ID"))
                    rec.FieldName = "IDCliente"
                    mi.TabelleModificate.Add(rec)
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_TaskLavorazione] SET [IDCliente]=" & GetID(persona) & ", [NomeCliente]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDCliente]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_ContiCorrentiInt 
                dbSQL = "SELECT [ID] FROM [tbl_ContiCorrentiInt] WHERE [IDPersona]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = mi.Add("tbl_ContiCorrentiInt", "IDPersona", Formats.ToInteger(dbRis("ID")))
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_ContiCorrentiInt] SET [IDPersona]=" & GetID(persona) & ", [NomePersona]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            Try
                'Tabella tbl_MergePersone 
                dbSQL = "SELECT [ID] FROM [tbl_MergePersone] WHERE [IDPersona1]=" & GetID(persona1)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    rec = mi.Add("tbl_MergePersone", "IDPersona1", Formats.ToInteger(dbRis("ID")))
                End While
                dbRis.Dispose() : dbRis = Nothing
                APPConn.ExecuteCommand("UPDATE [tbl_MergePersone] SET [IDPersona1]=" & GetID(persona) & ", [NomePersona1]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona1]=" & GetID(persona1))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

            ''Tabella tbl_MergePersone 
            'dbSQL = "SELECT [ID] FROM [tbl_MergePersone] WHERE [IDPersona2]=" & GetID(persona1)
            'dbRis = APPConn.ExecuteReader(dbSQL)
            'While dbRis.Read
            '    rec = mi.Add("tbl_MergePersone", "IDPersona2", Formats.ToInteger(dbRis("ID")))
            'End While
            'dbRis.Dispose()
            'APPConn.ExecuteCommand("UPDATE [tbl_MergePersone] SET [IDPersona2]=" & GetID(persona) & ", [NomePersona2]=" & DBUtils.DBString(persona.Nominativo) & " WHERE [IDPersona2]=" & GetID(persona1))
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
        SyncLock lock
            Dim mi As CMergePersona = e.MI
            Dim persona As CPersona = mi.Persona1
            Dim persona1 As CPersona = mi.Persona2
            Dim items As String

            Try
                'Tabella Login
                items = GetAffectedRecors(mi, "Login", "Persona")
                If (items <> "") Then APPConn.ExecuteCommand(Strings.JoinW("UPDATE [Login] SET [Persona]=", GetID(persona1), " WHERE [ID] In (", items, ")"))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_Annotazioni
                items = GetAffectedRecors(mi, "tbl_Annotazioni", "OwnerID")
                If (items <> "") Then Sistema.Annotazioni.Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_Annotazioni] SET [OwnerID]=", GetID(persona1), " WHERE [ID] In (", items, ")"))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_Attachments
                items = GetAffectedRecors(mi, "tbl_Attachments", "OwnerID")
                If (items <> "") Then Sistema.Attachments.Database.ExecuteCommand(Strings.JoinW("UPDATE [tbl_Attachments] SET [OwnerID]=", GetID(persona1), " WHERE [ID] In (", items, ")"))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_DocumentiIdentita
                items = GetAffectedRecors(mi, "tbl_DocumentiIdentita", "Persona")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_DocumentiIdentita] SET [Persona]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            If TypeOf (persona) Is CPersonaFisica Then
                Try
                    'Tabella tbl_Impiegati
                    items = GetAffectedRecors(mi, "tbl_Impiegati", "Persona")
                    If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_Impiegati] SET [Persona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try
            Else
                Try
                    'Tabella tbl_Impiegati (Azienda)
                    items = GetAffectedRecors(mi, "tbl_Impiegati", "Azienda")
                    If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_Impiegati] SET [Azienda]=" & GetID(persona1) & ", [NomeAzienda]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

                Try
                    'Tabella tbl_Impiegati (Ente Pagante)
                    items = GetAffectedRecors(mi, "tbl_Impiegati", "IDEntePagante")
                    If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_Impiegati] SET [IDEntePagante]=" & GetID(persona1) & ", [NomeEntePagante]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

                Try
                    'Tabella tbl_Persone (Azienda)
                    items = GetAffectedRecors(mi, "tbl_Persone", "IMP_IDAzienda")
                    If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_Persone] SET [IMP_IDAzienda]=" & GetID(persona1) & ", [IMP_NomeAzienda]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

                Try
                    'Tabella tbl_Persone (EntePagante)
                    items = GetAffectedRecors(mi, "tbl_Persone", "IMP_IDEntePagante")
                    If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_Persone] SET [IMP_IDEntePagante]=" & GetID(persona1) & ", [IMP_NomeEntePagante]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try
            End If

            Try
                'Tabella tbl_Indirizzi 
                items = GetAffectedRecors(mi, "tbl_Indirizzi", "Persona")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_Indirizzi] SET [Persona]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_Contatti 
                items = GetAffectedRecors(mi, "tbl_Contatti", "Persona")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_Contatti] SET [Persona]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_Ricontatti 
                items = GetAffectedRecors(mi, "tbl_Ricontatti", "IDPersona")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_Ricontatti] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

#If usa_tbl_RicontattiQuick Then
            Try
                'Tabella tbl_Ricontatti 
                items = GetAffectedRecors(mi, "tbl_RicontattiQuick", "IDPersona")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_RicontattiQuick] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

#End If

            Try
                'Tabella tbl_ListeRicontattoItems 
                items = GetAffectedRecors(mi, "tbl_ListeRicontattoItems", "IDPersona")
                If (items <> "") Then Anagrafica.ListeRicontatto.Database.ExecuteCommand("UPDATE [tbl_ListeRicontattoItems] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_PersoneRelazioni (IDPersona1) 
                items = GetAffectedRecors(mi, "tbl_PersoneRelazioni", "IDPersona1")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_PersoneRelazioni] SET [IDPersona1]=" & GetID(persona1) & ", [NomePersona1]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_PersoneRelazioni (IDPersona2) 
                items = GetAffectedRecors(mi, "tbl_PersoneRelazioni", "IDPersona2")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_PersoneRelazioni] SET [IDPersona2]=" & GetID(persona1) & ", [NomePersona2]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_PersoneConsensi 
                items = GetAffectedRecors(mi, "tbl_PersoneConsensi", "IDPersona")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_PersoneConsensi] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_SecurityTokens 
                items = GetAffectedRecors(mi, "tbl_SecurityTokens", "TokenSourceName")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_SecurityTokens] SET [TokenSourceName]=" & GetID(persona1) & " WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_ADVResults 
                items = GetAffectedRecors(mi, "tbl_ADVResults", "IDDestinatario")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_ADVResults] SET [IDDestinatario]=" & GetID(persona1) & ", [NomeDestinatario]=" & DBUtils.DBString(persona1.Nominativo) & "  WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_TaskLavorazione 
                items = GetAffectedRecors(mi, "tbl_TaskLavorazione", "IDCliente")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_TaskLavorazione] SET [IDCliente]=" & GetID(persona1) & ", [NomeCliente]=" & DBUtils.DBString(persona1.Nominativo) & "  WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_ContiCorrentiInt 
                items = GetAffectedRecors(mi, "tbl_ContiCorrentiInt", "IDPersona")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_ContiCorrentiInt] SET [IDPersona]=" & GetID(persona1) & ", [NomePersona]=" & DBUtils.DBString(persona1.Nominativo) & "  WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                'Tabella tbl_MergePersone
                items = mi.GetAffectedRecors("tbl_MergePersone", "IDPersona1")
                If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_MergePersone] SET [IDPersona1]=" & GetID(persona1) & ", [NomePersona1]=" & DBUtils.DBString(persona1.Nominativo) & "  WHERE [ID] In (" & items & ")")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
            ''Tabella tbl_MergePersone
            'items = mi.GetAffectedRecors("tbl_MergePersone", "IDPersona2")
            'If (items <> "") Then APPConn.ExecuteCommand("UPDATE [tbl_MergePersone] SET [IDPersona2]=" & GetID(persona1) & ", [NomePersona2]=" & DBUtils.DBString(persona1.Nominativo) & "  WHERE [ID] In (" & items & ")")

        End SyncLock
    End Sub

    Friend Shared Sub OnPersonaTransferred(ByVal e As TransferPersonaEventArgs)
        RaiseEvent PersonaTransferred(e)
    End Sub

    Friend Shared Sub OnPersonaCreated(ByVal e As PersonaEventArgs)
        RaiseEvent PersonaCreated(e)
    End Sub

    Friend Shared Sub OnPersonaModified(ByVal e As PersonaEventArgs)
        RaiseEvent PersonaModified(e)
    End Sub

    Friend Shared Sub OnPersonaDeleted(ByVal e As PersonaEventArgs)
        RaiseEvent PersonaDeleted(e)
    End Sub

    Friend Shared Sub OnPersonaMerged(ByVal e As MergePersonaEventArgs)
        RaiseEvent PersonaMerged(e)
    End Sub

    Friend Shared Sub OnPersonaUnMerged(ByVal e As MergePersonaEventArgs)
        RaiseEvent PersonaUnMerged(e)
    End Sub

    Private Shared Sub HandlePeronaModified(e As PersonaEventArgs)
        SyncLock DMD.Anagrafica.lock
            Dim p As CPersona = e.Persona
            'If (p.PuntoOperativo IsNot Nothing) Then
            Try
                APPConn.ExecuteCommand(Strings.JoinW("UPDATE [tbl_Ricontatti] SET [IDPuntoOperativo]=", p.IDPuntoOperativo, ", [NomePuntoOperativo]=", DBUtils.DBString(p.NomePuntoOperativo) & " WHERE [IDPersona]=", GetID(p), " AND [Stato]=1 AND [StatoRicontatto]=1"))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
#If usa_tbl_RicontattiQuick Then
            Try
                APPConn.ExecuteCommand(Strings.JoinW("UPDATE [tbl_RicontattiQuick] SET [IDPuntoOperativo]=", p.IDPuntoOperativo, ", [NomePuntoOperativo]=", DBUtils.DBString(p.NomePuntoOperativo), " WHERE [IDPersona]=", GetID(p), " AND [Stato]=1 AND [StatoRicontatto]=1"))
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
#End If
            Try
                Anagrafica.ListeRicontatto.Database.ExecuteCommand("UPDATE [tbl_ListeRicontattoItems] SET [DettaglioStato]=" & DBUtils.DBString(p.DettaglioEsito) & ", [DettaglioStato1]=" & DBUtils.DBString(p.DettaglioEsito1) & " WHERE [IDPersona]=" & GetID(p) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [StatoRicontatto]=" & StatoRicontatto.PROGRAMMATO)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
            'End If
        End SyncLock
    End Sub



End Class