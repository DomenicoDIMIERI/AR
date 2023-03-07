Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public NotInheritable Class CCollaboratoriClass
        Inherits CGeneralClass(Of CCollaboratore)


        Friend Sub New()
            MyBase.New("Collaboratori", GetType(CCollaboratoriCursor), -1)
        End Sub
         
        Public Function FormatStatoProduttore(ByVal value As StatoProduttore) As String
            Select Case value
                Case CQSPD.StatoProduttore.STATO_ATTIVO : Return "Attivo"
                Case CQSPD.StatoProduttore.STATO_DISABILITATO : Return "Disabilitato"
                Case CQSPD.StatoProduttore.STATO_ELIMINATO : Return "Eliminato"
                Case CQSPD.StatoProduttore.STATO_INATTIVAZIONE : Return "In Attivazione"
                Case CQSPD.StatoProduttore.STATO_SOSPESO : Return "Sospeso"
                    'Case CQSPD.StatoProduttore.STATO_INVALID : Return "Non Valido"
                Case Else : Return "Sconosciuto"
            End Select
        End Function

        Public Function ParseStatoProduttore(ByVal value As String) As StatoProduttore
            Select Case LCase(Trim(value))
                Case "attivo" : Return StatoProduttore.STATO_ATTIVO
                Case "disabilitato" : Return StatoProduttore.STATO_DISABILITATO
                Case "eliminato" : Return StatoProduttore.STATO_ELIMINATO
                Case "in attivazione" : Return StatoProduttore.STATO_INATTIVAZIONE
                Case "sospeso" : Return StatoProduttore.STATO_SOSPESO
                    'Case "non valido" : Return StatoProduttore.STATO_INVALID
                Case Else : Return 0
            End Select
        End Function

        'Public  Function GetItemByCodiceFiscale(ByVal valore As String) As CCollaboratore
        '    Dim cursor As New CCollaboratoriCursor
        '    Dim ret As CCollaboratore
        '    cursor.PageSize = 1
        '    cursor.IgnoreRights = True
        '    cursor.CodiceFiscale.Value = Formats.ParseCodiceFiscale(valore)
        '    ret = cursor.Item
        '    cursor.Reset()
        '    Return ret
        'End Function

        'Public  Function GetItemByPartitaIVA(ByVal valore As String) As CCollaboratore
        '    Dim cursor As New CCollaboratoriCursor
        '    Dim ret As CCollaboratore
        '    cursor.PageSize = 1
        '    cursor.IgnoreRights = True
        '    'cursor.PartitaIVA.Value = Formats.ParsePartitaIVA(valore)
        '    ret = cursor.Item
        '    cursor.Reset()
        '    Return ret
        'End Function

        'Public  Function GetItemByEMail(ByVal valore As String) As CCollaboratore
        '    Dim cursor As New CCollaboratoriCursor
        '    Dim ret As CCollaboratore
        '    cursor.PageSize = 1
        '    cursor.IgnoreRights = True
        '    cursor.eMail.Value = valore
        '    ret = cursor.Item
        '    cursor.Reset()
        '    Return ret
        'End Function

        'Public Function GetItemByUIF(ByVal valore As String) As CCollaboratore
        '    Dim cursor As New CCollaboratoriCursor
        '    Dim ret As CCollaboratore
        '    For   ret In Me.CachedItems
        '        If ret.NumeroIsci Then
        '    Next
        '    cursor.PageSize = 1
        '    cursor.IgnoreRights = True
        '    cursor.NumeroIscrizioneUIF.Value = valore
        '    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '    ret = cursor.Item
        '    cursor.Reset()
        '    Return ret
        'End Function

        Public Function GetItemByRUI(ByVal valore As String) As CCollaboratore
            valore = Trim(valore)
            If (valore = "") Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If Strings.Compare(item.NumeroIscrizioneRUI, valore) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByISVAP(ByVal valore As String) As CCollaboratore
            valore = Trim(valore)
            If (valore = "") Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If Strings.Compare(item.NumeroIscrizioneISVAP, valore) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByPersona(ByVal personID As Integer) As CCollaboratore
            If (personID = 0) Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If item.IDPersona = personID Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByUser(ByVal userId As Integer) As CCollaboratore
            If (userId = 0) Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If item.UserID = userId Then Return item
            Next
            Return Nothing
        End Function
         

        Public Function GetItemByName(ByVal value As String) As CCollaboratore
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each item As CCollaboratore In Me.LoadAll
                If Strings.Compare(item.NomePersona, value) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function CalcolaPremio(ByVal value As Decimal) As Decimal
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            Dim ret, somma, termine, finoA As Decimal
            Dim perc As Double
            dbSQL = "SELECT * FROM [tbl_CollaboratoriPremi] ORDER BY [FinoA] ASC"
            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            somma = value
            ret = 0
            While dbRis.Read And (somma > 0)
                finoA = Formats.ToValuta(dbRis("FinoA"))
                perc = Formats.ToDouble(dbRis("Percentuale"))
                If "" & finoA = "" Then finoA = 0
                If "" & perc = "" Then perc = 0
                termine = IIf(somma <= finoA, somma, finoA)
                somma = somma - termine
                termine = termine * perc / 100
                ret = ret + termine
            End While
            dbRis.Dispose()
            dbRis = Nothing
            Return ret
        End Function

    End Class

    Private Shared m_Collaboratori As CCollaboratoriClass = Nothing

    Public Shared ReadOnly Property Collaboratori As CCollaboratoriClass
        Get
            If (m_Collaboratori Is Nothing) Then m_Collaboratori = New CCollaboratoriClass
            Return m_Collaboratori
        End Get
    End Property

End Class