Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Internals.Office

Namespace Internals.Office

    Public Class COggettiInventariatiClass
        Inherits CGeneralClass(Of OggettoInventariato)

        Public Sub New()
            MyBase.New("modOfficeOggettiInventariati", GetType(OggettoInventariatoCursor), 0)
        End Sub

        Protected Overrides Function InitializeModule() As CModule
            Dim m As CModule = MyBase.InitializeModule()
            m.Parent = DMD.Office.Module
            m.Visible = True
            m.Save()
            Return m
        End Function

        Public Function FormatStatoAttuale(ByVal value As StatoOggettoInventariato) As String
            Select value
                Case StatoOggettoInventariato.Sconosciuto : Return "Sconosciuto"
                Case StatoOggettoInventariato.Funzionante : Return "Funzionante"
                Case StatoOggettoInventariato.ConQualcheDifetto : Return "Con qualche difetto"
                Case StatoOggettoInventariato.DaRiparare : Return "Da Riparare"
                Case StatoOggettoInventariato.ValutazioneRiparazione : Return "Valutare Riparazione"
                Case StatoOggettoInventariato.NonRiparabile : Return "Non riparabile"
                Case StatoOggettoInventariato.Dismesso : Return "Dismesso"
                Case Else : Return "[" & value & "]"
            End Select
        End Function

        Public Function Find(ByVal text As String) As CCollection(Of OggettoInventariato)
            Dim ret As New CCollection(Of OggettoInventariato)
            text = Trim(text)
            If (text = "") Then Return ret
            Dim cursor As New OggettoInventariatoCursor
            'cerciahimo prima in base al codice
            cursor.WhereClauses.Add("[Seriale] Like '" & Replace(text, "'", "''") & "%' OR [CodiceRFID] Like '" & Replace(text, "'", "''") & "%' OR [Codice] Like '" & Replace(text, "'", "''") & "%'")
            While (Not cursor.EOF)
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Reset1()
            cursor.WhereClauses.Clear()

            If (ret.Count = 0) Then
                'Se non abbiamo risultati cerchiamo in base al nome
                cursor.Clear()
                cursor.Nome.Value = text & "%"
                cursor.Nome.Operator = OP.OP_LIKE
                While (Not cursor.EOF)
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            End If
            cursor.Reset1()

            If (cursor.Count = 0) Then
                'Se non abbiamo risultati cerchiamo il base al nome articolo
                cursor.Clear()
                cursor.NomeArticolo.Value = text & "%"
                cursor.NomeArticolo.Operator = OP.OP_LIKE
                While (Not cursor.EOF)
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            End If

            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Public Function GetItemByName(ByVal name As String) As OggettoInventariato
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            Dim cursor As New OggettoInventariatoCursor
            Dim ret As OggettoInventariato = Nothing
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            cursor.Nome.Value = name
            cursor.StatoAttuale.Value = StatoOggettoInventariato.Funzionante
            ret = cursor.Item
            cursor.Reset1()
            If (ret Is Nothing) Then
                cursor.StatoAttuale.Clear()
                cursor.StatoAttuale.SortOrder = SortEnum.SORT_ASC
                ret = cursor.Item
            End If
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

    End Class


End Namespace

Partial Public Class Office
     
    Private Shared m_OggettiInventariati As COggettiInventariatiClass = Nothing

    Public Shared ReadOnly Property OggettiInventariati As COggettiInventariatiClass
        Get
            If (m_OggettiInventariati Is Nothing) Then m_OggettiInventariati = New COggettiInventariatiClass
            Return m_OggettiInventariati
        End Get
    End Property




   
End Class


