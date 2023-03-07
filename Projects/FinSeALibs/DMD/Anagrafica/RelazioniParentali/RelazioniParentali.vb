Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    ''' <summary>
    ''' Rappresenta le relazioni di parentela/affinità tra le persone
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CRelazioniParentaliClass
        Inherits CGeneralClass(Of CRelazioneParentale)

        Private ReadOnly SupportedNames As String() = {"Marito", "Moglie", "Genitore", "Figlio", "Figlia", "Padre", "Madre", "Nonno", "Nonna", "Zio", "Zia", "Nipote", "Cognato", "Cognata", "Genero", "Nuora", "Suocero", "Suocera", "Fratello", "Sorella", "Amico", "Amica", "Conoscente", "Ex Marito", "Ex Moglie", "Convivente", "Cugino", "Cugina"}

        Friend Sub New()
            MyBase.New("modAnaRelazioni", GetType(CRelazioneParentaleCursor))
        End Sub
          
        Public Function GetSupportedNames() As String()
            Return SupportedNames
        End Function

        Public Function GetInvertedRelations(ByVal relation As String) As String()
            Select Case LCase(Trim(relation))
                Case "marito" : Return {"Moglie"}
                Case "moglie" : Return {"Marito"}
                Case "figlio", "figlia" : Return {"Padre", "Madre", "Genitore"}
                Case "padre", "madre", "genitore" : Return {"Figlio", "Figlia"}
                Case "nonno", "nonna" : Return {"Nipote"}
                Case "zio", "zia" : Return {"Nipote"}
                Case "nipote" : Return {"Nonno", "Nonna", "Zio", "Zia"}
                Case "cognato", "cognata" : Return {"Cognato", "Cognata"}
                Case "genero", "nuora" : Return {"Suocero", "Suocera"}
                Case "suocero", "suocera" : Return {"Genero", "Nuora"}
                Case "fratello", "sorella" : Return {"Fratello", "Sorella"}
                Case "amico", "amica" : Return {"Amico", "Amica"}
                Case "conoscente" : Return {"Conoscente"}
                Case "ex marito" : Return {"Ex Moglie"}
                Case "ex moglie" : Return {"Ex Marito"}
                Case "convivente" : Return {"Convivente"}
                Case "cugino", "cugina" : Return {"Cugino", "Cugina"}
                Case Else : Return {""}
            End Select
        End Function

        Public Function GetRelazioni(ByVal personID As Integer) As CCollection(Of CRelazioneParentale)
            Dim conn As CDBConnection = APPConn
            Dim ret As New CCollection(Of CRelazioneParentale)
            If (personID = 0) Then Return ret

            If (conn.IsRemote) Then
                Dim tmp As String = RPC.InvokeMethod("/widgets/websvc/?_m=modAnaRelazioni&_a=GetRelazioni", "pid", RPC.int2n(personID))
                If (tmp <> "") Then ret.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            Else
                Dim cursor As New CRelazioneParentaleCursor
                Try
                    cursor.WhereClauses.Add("([IDPersona1]=" & personID & " OR [IDPersona2]=" & personID & ")")
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.NomeRelazione.SortOrder = SortEnum.SORT_ASC
                    cursor.IgnoreRights = True
                    While Not cursor.EOF
                        ret.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                Catch ex As Exception
                    Throw
                Finally
                    cursor.Dispose()
                End Try
            End If
            Return ret
        End Function

        Public Function GetRelazioni(ByVal persona As CPersonaFisica) As CCollection(Of CRelazioneParentale)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Return GetRelazioni(GetID(persona))
        End Function

    End Class

    Private Shared m_RelazioniParentali As CRelazioniParentaliClass = Nothing

    Public Shared ReadOnly Property RelazioniParentali As CRelazioniParentaliClass
        Get
            If (m_RelazioniParentali Is Nothing) Then m_RelazioniParentali = New CRelazioniParentaliClass
            Return m_RelazioniParentali
        End Get
    End Property
End Class