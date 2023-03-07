Imports DMD
Imports DMD.Anagrafica
Imports DMD.Sistema

Namespace Forms

    Public Class MergePersoneModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return New CMergePersonaCursor
        End Function



        Public Function GetGroups(ByVal renderer As Object) As String
            If Not Anagrafica.Persone.Module.UserCanDoAction("merge") Then Throw New PermissionDeniedException(Anagrafica.Persone.Module, "merge")
            Dim filter As CRMFinlterI = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))

            Dim col As CCollection(Of CPersonaInfo) = Anagrafica.Persone.Find(filter)
            col.Sort()

            For Each item As CPersonaInfo In col
                Me.MakeNotes(item)
            Next

            Dim gruppi As CCollection(Of MergeModuleGruppoPersona) = Me.MakeGruppi(col)

            Return XML.Utils.Serializer.Serialize(gruppi)
        End Function

        Public Function UnMerge(ByVal renderer As Object) As String
            If Not Anagrafica.Persone.Module.UserCanDoAction("merge") Then Throw New PermissionDeniedException(Anagrafica.Persone.Module, "merge")
            Dim item As Integer = RPC.n2int(GetParameter(renderer, "item", ""))
            Dim persona As CPersona = Anagrafica.Persone.GetItemById(item)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Return XML.Utils.Serializer.Serialize(persona.UndoMerge)
        End Function

        Public Function MegeItems(ByVal renderer As Object) As String
            If Not Anagrafica.Persone.Module.UserCanDoAction("merge") Then Throw New PermissionDeniedException(Anagrafica.Persone.Module, "merge")
            Dim items() As String = Split(RPC.n2str(GetParameter(renderer, "items", "")), ",")
            Dim idsList As New System.Collections.ArrayList
            For Each value As String In items
                If (Formats.ToInteger(value) <> 0) Then idsList.Add(Formats.ToInteger(value))
            Next
            Dim ids() As Integer = idsList.ToArray(GetType(Integer))
            If (UBound(ids) > 0) Then
                Dim persona As CPersona = Anagrafica.Persone.GetItemById(ids(0))
                For i As Integer = 1 To UBound(ids)
                    persona.MergeWith(Anagrafica.Persone.GetItemById(ids(i)))
                Next
            End If

            Return ""
        End Function

        Public Function Rename(ByVal renderer As Object) As String
            If Not Anagrafica.Persone.Module.UserCanDoAction("merge") Then Throw New PermissionDeniedException(Anagrafica.Persone.Module, "merge")
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))

            Dim nome As String = Strings.Trim(RPC.n2str(GetParameter(renderer, "n", "")))
            Dim cognome As String = Strings.Trim(RPC.n2str(GetParameter(renderer, "c", "")))

            If (nome & cognome = "") Then Throw New ArgumentNullException("Non puoi rinominare con nome vuoto")

            Dim persona As CPersona = Anagrafica.Persone.GetItemById(id)
            If (persona Is Nothing) Then Throw New ArgumentNullException("Persona")

            If (TypeOf (persona) Is CPersonaFisica) Then
                With DirectCast(persona, CPersonaFisica)
                    .Cognome = cognome
                    .Nome = nome
                    .MergeWith(persona)
                End With
            Else
                With DirectCast(persona, CAzienda)
                    .RagioneSociale = cognome & nome
                    .MergeWith(persona)
                End With
            End If

            Return ""
        End Function

        Private Function MakeNotes(ByVal info As CPersonaInfo) As String
            Dim p As CPersona = info.Persona

            Dim sessoAO As String = IIf(p.Sesso = "F", "a", "o")

            If (p.TipoPersona = TipoPersona.PERSONA_FISICA) Then
                With DirectCast(p, CPersonaFisica)
                    Dim strImpiego As String = ""
                    If (.ImpiegoPrincipale IsNot Nothing) Then
                        If (.ImpiegoPrincipale.NomeAzienda <> "" OrElse .ImpiegoPrincipale.Posizione <> "" OrElse .ImpiegoPrincipale.TipoRapporto <> "") Then
                            If (.ImpiegoPrincipale.TipoRapporto = "H") Then
                                strImpiego = "Pensionat" & sessoAO
                            Else
                                strImpiego = "Lavora"
                            End If

                            If (.ImpiegoPrincipale.NomeAzienda <> "") Then strImpiego &= " presso " & .ImpiegoPrincipale.NomeAzienda
                            If (.ImpiegoPrincipale.Posizione <> "") Then strImpiego &= " come " & .ImpiegoPrincipale.Posizione
                            If (.ImpiegoPrincipale.TipoRapporto <> "") Then strImpiego &= " (" & .ImpiegoPrincipale.TipoRapporto & ")"
                        End If
                        If strImpiego <> "" Then info.Notes = Strings.Combine(info.Notes, strImpiego, ", ")
                    End If
                End With
            End If

            Dim strContatti As String = ""
            'For i As Integer = 0 To p.Contatti.Count - 1
            '    Dim c As CContatto = p.Contatti(i)
            '    If (c.Valore <> "") Then strContatti = Strings.Combine(strContatti, c.Nome & ": " & Formats.FormatPhoneNumber(c.Valore), "; ")
            'Next
            'For i As Integer = 0 To p.ContattiWeb.Count - 1
            '    Dim c As CContatto = p.ContattiWeb(i)
            '    If (c.Valore <> "") Then strContatti = Strings.Combine(strContatti, c.Nome & ": " & c.Valore, "; ")
            'Next

            For i As Integer = 0 To p.Recapiti.Count - 1
                Dim c As CContatto = p.Recapiti(i)
                If (c.Valore <> "") Then strContatti = Strings.Combine(strContatti, c.Nome & ": " & Formats.FormatPhoneNumber(c.Valore), "; ")
            Next

            If (strContatti <> "") Then info.Notes &= ", Contatti: " & strContatti

            Return info.Notes
        End Function


        Private Function MakeGruppi(ByVal items As CCollection(Of CPersonaInfo)) As CCollection(Of MergeModuleGruppoPersona)
            Dim ret As New CCollection(Of MergeModuleGruppoPersona)
            If (items.Count = 0) Then Return ret

            Dim current As New MergeModuleGruppoPersona
            Dim i As Integer = 0
            While (i < items.Count)
                current = New MergeModuleGruppoPersona
                current.items.Add(items(i))
                ret.Add(current)
                Dim j As Integer = i + 1
                While (j < items.Count)
                    Dim p As CPersonaInfo = items(j)
                    If (current.IsSameGroup(p)) Then
                        current.items.Add(p)
                        items.RemoveAt(j)
                    Else
                        j += 1
                    End If
                End While
                i += 1
            End While

            Return ret
        End Function
    End Class



    Public Class MergeModuleGruppoPersona
        Implements DMD.XML.IDMDXMLSerializable

        Public items As New CCollection(Of CPersonaInfo)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Function IsSameGroup(ByVal p2 As CPersonaInfo) As Boolean
            If (Me.items.Count = 0) Then Return True
            Dim p1 As CPersonaInfo = Me.items(0)
            If (p1.Persona.TipoPersona <> p2.Persona.TipoPersona) Then Return False
            If (
                (p1.Persona.Nominativo = p2.Persona.Nominativo) AndAlso
                (p1.Persona.DataNascita.HasValue AndAlso p2.Persona.DataNascita.HasValue AndAlso DateUtils.Compare(p1.Persona.DataNascita, p2.Persona.DataNascita) = 0)
                ) Then Return True
            For Each c1 As CContatto In p1.Persona.Recapiti
                If (c1.Valore <> "") Then 'AndAlso (c1.Validated .HasValue =False OrElse c1.Validated = True ) Then
                    For Each c2 As CContatto In p2.Persona.Recapiti
                        If (c2.Valore = c1.Valore) Then Return True
                    Next
                End If
            Next
            Return False
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "items" : Me.items.Clear() : Me.items.AddRange(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("items", Me.items)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace