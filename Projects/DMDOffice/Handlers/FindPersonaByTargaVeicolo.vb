Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica



Partial Public Class Office


    Public Class FindPersonaByTargaVeicolo
        Inherits FindPersonaHandler

        Public Sub New()
        End Sub

        Private Function IsNuovaTarga(ByVal targa As String) As Boolean
            If (Len(targa) <> 7) Then Return False
            If Char.IsLetter(Mid(targa, 1, 1)) AndAlso Char.IsLetter(Mid(targa, 2, 1)) AndAlso Char.IsLetter(Mid(targa, 6, 1)) AndAlso Char.IsLetter(Mid(targa, 7, 1)) Then
                For i As Integer = 3 To 5
                    If Not Char.IsNumber(Mid(targa, i, 1)) Then Return False
                Next
                Return True
            End If
            Return False
        End Function

        Public Overrides Function CanHandle(targa As String, filter As CRMFinlterI) As Boolean
            targa = Office.Veicoli.ParseTarga(targa)
            Return Me.IsNuovaTarga(targa)
        End Function

        Public Overrides Sub Find(ByVal targa As String, filter As CRMFinlterI, ret As CCollection(Of CPersonaInfo))
            Dim cursor1 As VeicoliCursor = Nothing
            Dim cursor As CPersonaFisicaCursor = Nothing
            Dim list As New System.Collections.ArrayList
            Dim arr() As Integer = Nothing

            Try
                targa = Office.Veicoli.ParseTarga(targa)
                If (Len(Trim(targa)) < 3) Then Exit Sub

                cursor1 = New VeicoliCursor
                cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor1.Targa.Value = Strings.JoinW(targa, "%")
                cursor1.Targa.Operator = OP.OP_LIKE
                While Not cursor1.EOF
                    Dim veicolo As Veicolo = cursor1.Item
                    If (veicolo.IDProprietario <> 0) Then List.Add(veicolo.IDProprietario)

                    cursor1.MoveNext()
                End While
                cursor1.Dispose()

                If (list.Count = 0) Then Exit Sub

                cursor = New CPersonaFisicaCursor

                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = filter.ignoreRights
                arr = list.ToArray(GetType(Integer))
                cursor.ID.ValueIn(arr)
                If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
                If (filter.flags.HasValue) Then
                    cursor.PFlags.Value = filter.flags.Value
                    cursor.PFlags.Operator = OP.OP_ALLBITAND
                End If
                Dim cnt As Integer = 0
                While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                    ret.Add(New CPersonaInfo(cursor.Item))
                    cursor.MoveNext()
                    cnt += 1
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (arr IsNot Nothing) Then Erase arr : arr = Nothing
                If (cursor1 IsNot Nothing) Then cursor1.Dispose() : cursor1 = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Sub

        Public Overrides Function GetHandledCommand() As String
            Return "Targa Veicolo"
        End Function
    End Class


End Class
