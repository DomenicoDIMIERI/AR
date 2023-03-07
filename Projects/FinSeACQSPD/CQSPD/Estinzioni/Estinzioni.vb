Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals


    ''' <summary>
    ''' Oggetto che consente di accedere alle estinzioni
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CEstinzioniClass
        Inherits CGeneralClass(Of CEstinzione)


        Friend Sub New()
            MyBase.New("modEstinzioni", GetType(CEstinzioniCursor))
        End Sub

        ''' <summary>
        ''' Restituisce un oggetto CEstinzioniPersona contenente tutte le estinzioni associate alla persona
        ''' </summary>
        ''' <param name="persona"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetEstinzioniByPersona(ByVal persona As CPersona) As CEstinzioniPersona
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Return New CEstinzioniPersona(persona)
        End Function

        ''' <summary>
        ''' Restituisce tutte le estinzioni per la pratica specificata
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetEstinzioniByPratica(ByVal pratica As CRapportino) As CCollection(Of CEstinzione)
            Dim cursor As CEstinzioniCursor = Nothing
            Try
                If (pratica Is Nothing) Then Throw New ArgumentNullException("pratica")

                Dim ret As New CCollection(Of CEstinzione)

                If (GetID(pratica) = 0) Then Return ret

                cursor = New CEstinzioniCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDPratica.Value = GetID(pratica)
                'cursor.Estinta.SortOrder = SortEnum.SORT_ASC
                cursor.IgnoreRights = True
                cursor.DataInizio.SortOrder = SortEnum.SORT_DESC

                While Not cursor.EOF
                    Dim est As CEstinzione = cursor.Item
                    est.SetPratica(pratica)
                    ret.Add(est)
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Function

        ''' <summary>
        ''' Restituisce tutte le estinzioni per la pratica specificata
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetEstinzioniByPratica(ByVal idPratica As Integer) As CCollection(Of CEstinzione)
            Return Me.GetEstinzioniByPratica(CQSPD.Pratiche.GetItemById(idPratica))
        End Function

        ''' <summary>
        ''' Restituisce un oggetto CEstinzioniPersona contenente tutte le estinzioni associate alla persona
        ''' </summary>
        ''' <param name="idPersona"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetEstinzioniByPersona(ByVal idPersona As Integer) As CEstinzioniPersona
            Return Me.GetEstinzioniByPersona(Anagrafica.Persone.GetItemById(idPersona))
        End Function

        ' ''' <summary>
        ' ''' Associa l'estinzione alla persona e restituisce l'ID dell'associazione
        ' ''' </summary>
        ' ''' <param name="estinzione"></param>
        ' ''' <param name="persona"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public  Function addEstinzioneAPersona(ByVal estinzione As CEstinzione, ByVal persona As CPersona) As CEstinzioneXPersona
        '    If (estinzione Is Nothing) Then Throw New ArgumentNullException("estinzione")
        '    If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
        '    Dim estXpers As CEstinzioneXPersona
        '    Dim cursor As New CEstinzioniXPersonaCursor
        '    cursor.IgnoreRights = True
        '    cursor.IDPersona.Value = GetID(persona)
        '    cursor.IDEstinzione.Value = GetID(estinzione)
        '    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '    estXpers = cursor.Item
        '    cursor.Dispose()
        '    If (estXpers Is Nothing) Then
        '        estXpers = New CEstinzioneXPersona
        '        With estXpers
        '            .Estinzione = estinzione
        '            .Persona = persona
        '            .Stato = ObjectStatus.OBJECT_VALID
        '        End With
        '        CQSPD.Database.Save(estXpers)
        '    End If
        '    Return estXpers
        'End Function

        'Function IsEstinzioneLinkedWithPersona(ByVal idEstinzione As Integer, ByVal idPersona As Integer) As Boolean
        '    If (idEstinzione = 0) Then Throw New ArgumentNullException("idEstinzione")
        '    If (idPersona = 0) Then Throw New ArgumentNullException("idPersona")
        '    Dim cursor As New CEstinzioniXPersonaCursor
        '    Dim ret As Boolean
        '    cursor.IgnoreRights = True
        '    cursor.IDPersona.Value = idPersona
        '    cursor.IDEstinzione.Value = idEstinzione
        '    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '    ret = Not cursor.EOF
        '    cursor.Dispose()
        '    Return ret
        'End Function

        ' ''' <summary>
        ' ''' Associa l'estinzione alla persona e restituisce l'ID dell'associazione
        ' ''' </summary>
        ' ''' <param name="idEstinzione"></param>
        ' ''' <param name="idPersona"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public  Function addEstinzioneAPersona(ByVal idEstinzione As Integer, ByVal idPersona As Integer) As CEstinzioneXPersona
        '    If (idEstinzione = 0) Then Throw New ArgumentNullException("idEstinzione")
        '    If (idPersona = 0) Then Throw New ArgumentNullException("idPersona")
        '    Dim estXpers As CEstinzioneXPersona
        '    Dim cursor As New CEstinzioniXPersonaCursor
        '    cursor.IgnoreRights = True
        '    cursor.IDPersona.Value = idPersona
        '    cursor.IDEstinzione.Value = idEstinzione
        '    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '    estXpers = cursor.Item
        '    cursor.Dispose()
        '    If (estXpers Is Nothing) Then
        '        estXpers = New CEstinzioneXPersona
        '        With estXpers
        '            .IDEstinzione = idEstinzione
        '            .IDPersona = idPersona
        '            .Stato = ObjectStatus.OBJECT_VALID
        '        End With
        '        CQSPD.Database.Save(estXpers)
        '    End If
        '    Return estXpers
        'End Function

        Public Function FormatTipo(ByVal tipo As TipoEstinzione) As String
            Select Case tipo
                Case TipoEstinzione.ESTINZIONE_NO : Return ""
                Case TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO : Return "CQS"
                Case TipoEstinzione.ESTINZIONE_CQP : Return "CQP"
                Case TipoEstinzione.ESTINZIONE_PRESTITODELEGA : Return "PD"
                Case TipoEstinzione.ESTINZIONE_PRESTITOPERSONALE : Return "Prestito Personale"
                Case TipoEstinzione.ESTINZIONE_PIGNORAMENTO : Return "Pignoramento"
                Case TipoEstinzione.ESTINZIONE_MUTUO : Return "Mutuo"
                Case TipoEstinzione.ESTINZIONE_PROTESTI : Return "Protesti"
                Case TipoEstinzione.ESTINZIONE_ASSICURAZIONE : Return "Assicurazione"
                Case TipoEstinzione.ESTINZIONE_ALIMENTI : Return "Alimenti"
                Case TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO : Return "Piccolo Prestito"
                Case Else : Return "invalid"
            End Select
        End Function

        Public Function GetEstinzioniXEstintore(ByVal estintore As IEstintore) As CCollection(Of EstinzioneXEstintore)
            If (estintore Is Nothing) Then Throw New ArgumentNullException("estintore")
            Return GetEstinzioniXEstintore(GetID(estintore), TypeName(estintore))
        End Function

        Public Function GetEstinzioniXEstintore(ByVal idEstintore As Integer, ByVal tipoEstintore As String) As CCollection(Of EstinzioneXEstintore)
            If (tipoEstintore = "") Then Throw New ArgumentNullException("tipoEstintore")
            Dim ret As New CCollection(Of EstinzioneXEstintore)
            If (idEstintore = 0) Then Return ret

            Dim cursor As New EstinzioneXEstintoreCursor
            Try
                cursor.IDEstintore.Value = idEstintore
                cursor.TipoEstintore.Value = tipoEstintore
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        ''' <summary>
        ''' Controlla tra tutti i prestiti in corso programmando le date di ricontatto secondo i parametri
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RielaboraRinnovabili()
            Dim cursor As CEstinzioniCursor = Nothing
            Try
                cursor = New CEstinzioniCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.IDEstintoDa.Value = 0
                cursor.IDEstintoDa.IncludeNulls = True
                cursor.Scadenza.Value = Calendar.ToDay
                cursor.Scadenza.Operator = OP.OP_GE
                'cursor.Scadenza.IncludeNulls = True
                While Not cursor.EOF
                    cursor.Item.Save(True)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Sub

        Public Function CanDelete(ByVal p As CEstinzione) As String
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Return Me.CanDelete(GetID(p))
        End Function

        Public Function CanDelete(ByVal id As Integer) As String
            Dim cursor As EstinzioneXEstintoreCursor = Nothing
            Try
                If (id = 0) Then Return vbNullString

                cursor = New EstinzioneXEstintoreCursor
                cursor.IgnoreRights = True
                cursor.IDEstintore.Value = id
                While Not cursor.EOF
                    Dim item As EstinzioneXEstintore = cursor.Item
                    Dim estintore As Object = item.Estintore
                    If TypeOf (estintore) Is CRapportino Then
                        If (DirectCast(estintore, CRapportino).Stato = ObjectStatus.OBJECT_VALID) Then
                            Return "Pratica N°" & DirectCast(estintore, CRapportino).NumeroPratica
                        End If
                    ElseIf TypeOf (estintore) Is CQSPDConsulenza Then
                        If (DirectCast(estintore, CQSPDConsulenza).Stato = ObjectStatus.OBJECT_VALID) Then
                            Return "Simulazione" & DirectCast(estintore, CQSPDConsulenza).ID
                        End If
                    ElseIf TypeOf (estintore) Is IDBObject Then
                        If (DirectCast(estintore, IDBObject).Stato = ObjectStatus.OBJECT_VALID) Then
                            Return TypeName(estintore) & GetID(estintore)
                        End If
                    Else
                        Return TypeName(estintore) & GetID(estintore)
                    End If
                    cursor.MoveNext()
                End While

                Return ""
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try


        End Function

        Public Function GetPrestitiAttivi(ByVal persona As CPersonaFisica, ByVal data As Date) As CCollection(Of CEstinzione)
            Dim items As CCollection(Of CEstinzione) = Me.GetEstinzioniByPersona(persona)
            Dim ret As New CCollection(Of CEstinzione)
            For Each e As CEstinzione In items
                If (e.IsInCorso(data)) Then ret.Add(e)
            Next
            Return ret
        End Function

        Public Function GetPrestitiAttivi(ByVal persona As CPersonaFisica) As CCollection(Of CEstinzione)
            Return Me.GetPrestitiAttivi(persona, Calendar.Now)
        End Function

        Protected Friend Shadows Sub doItemCreated(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

        Public Function getMeseRinnovo(ByVal durata As Integer?) As Integer?
            Dim m_Mesi As Integer() = {24, 36, 48, 60, 72, 84, 96, 108, 120}
            Dim m_Rinnovo As Integer() = {10, 15, 20, 24, 29, 34, 39, 44, 48}
            Dim i As Integer = Arrays.IndexOf(m_Mesi, durata)
            If (i < 0) Then Return Nothing 'throw "ArgumentException: La durata deve essere compresa tra 24 e 120 e multipla di 12";
            Return m_Rinnovo(i)
        End Function

        Public Overrides Sub Initialize()
            MyBase.Initialize()

            Dim table As CDBTable = CQSPD.Database.Tables.GetItemByKey("tbl_Estinzioni")
            If (table Is Nothing) Then
                Return
            End If
            Dim col As CDBEntityField = table.Fields.GetItemByKey("TAEG")
            If (col Is Nothing) Then
                col = table.Fields.Add("TAEG", GetType(Double))
                table.Update()
            End If
        End Sub

    End Class
End Namespace

Partial Public Class CQSPD



    Private Shared m_Estinzioni As CEstinzioniClass = Nothing

    Public Shared ReadOnly Property Estinzioni As CEstinzioniClass
        Get
            If (m_Estinzioni Is Nothing) Then m_Estinzioni = New CEstinzioniClass
            Return m_Estinzioni
        End Get
    End Property

End Class