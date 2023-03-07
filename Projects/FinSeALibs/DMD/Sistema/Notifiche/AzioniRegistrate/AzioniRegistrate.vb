Imports DMD.Databases

Partial Public Class Sistema


    Partial Public Class CNotificheClass
         
        Private m_RegisteredActions As CCollection(Of AzioneRegistrata) = Nothing

        Public ReadOnly Property RegisteredActions As CCollection(Of AzioneRegistrata)
            Get
                SyncLock Me
                    If (m_RegisteredActions Is Nothing) Then
                        m_RegisteredActions = New CCollection(Of AzioneRegistrata)
                        Dim cursor As New AzioneRegistrataCursor
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                        cursor.IgnoreRights = True
                        While Not cursor.EOF
                            m_RegisteredActions.Add(cursor.Item)
                            cursor.MoveNext()
                        End While
                        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                    End If
                    Return m_RegisteredActions
                End SyncLock
            End Get
        End Property


        Public Function IsAzioneRegistrata(ByVal sourceName As String, ByVal azione As AzioneEseguibile) As Boolean
            SyncLock Me
                If (azione Is Nothing) Then Throw New ArgumentNullException("azione")
                sourceName = Trim(sourceName)
                For Each item As AzioneRegistrata In Me.RegisteredActions
                    If ((item.SourceName = "" OrElse item.SourceName = sourceName) AndAlso item.ActionType = TypeName(azione)) Then
                        Return True
                    End If
                Next
                Return False
            End SyncLock
        End Function

        ''' <summary>
        ''' Registra un'azione definita sul tipo di oggetti specificato
        ''' </summary>
        ''' <param name="sourceName">[in] Tipo di sorgente. Se NULL l'azione sarà disponibile su tutto</param>
        ''' <param name="azione">[in] Azione da registrare per il tipo di sorgente specificato</param>
        ''' <remarks></remarks>
        Public Function RegisterAzione(ByVal sourceName As String, ByVal azione As AzioneEseguibile) As AzioneRegistrata
            SyncLock Me
                If (azione Is Nothing) Then Throw New ArgumentNullException("azione")
                sourceName = Trim(sourceName)

                Dim item As AzioneRegistrata
                For Each item In RegisteredActions
                    If (item.SourceName = sourceName AndAlso item.ActionType = TypeName(azione)) Then
                        Throw New ArgumentException("Azione già registrata per il tipo specificato")
                    End If
                Next

                item = New AzioneRegistrata
                item.ActionType = azione.GetType.Name
                item.SourceName = sourceName
                item.Stato = ObjectStatus.OBJECT_VALID
                item.Save()

                RegisteredActions.Add(item)

                Return item
            End SyncLock
        End Function

        ''' <summary>
        ''' Rimuove la registrazione dell'azione
        ''' </summary>
        ''' <param name="sourceName">[in] Tipo di sorgente. Se NULL l'azione sarà rimossa solo se definita su tutto</param>
        ''' <param name="azione">[in] Azione da rimuovere</param>
        ''' <remarks></remarks>
        Public Function UnregisterAzione(ByVal sourceName As String, ByVal azione As AzioneEseguibile) As AzioneRegistrata
            SyncLock Me
                If (azione Is Nothing) Then Throw New ArgumentNullException("azione")
                sourceName = Trim(sourceName)
                For i As Integer = 0 To RegisteredActions.Count - 1
                    Dim item As AzioneRegistrata = RegisteredActions(i)
                    If (item.SourceName = sourceName AndAlso item.ActionType = TypeName(azione)) Then
                        RegisteredActions.RemoveAt(i)
                        Return item
                    End If
                Next

                Return Nothing
            End SyncLock
        End Function

        ''' <summary>
        ''' Restituisce una collezione contenente tutte le azioni registrate per la notifica
        ''' </summary>
        ''' <param name="sourceName">[in] Tipo di sorgente. Non può essere NULL</param>
        ''' <remarks></remarks>
        Public Function GetRegisteredActions(ByVal sourceName As String) As CCollection(Of AzioneRegistrata)
            SyncLock Me
                sourceName = Trim(sourceName)
                If (sourceName = vbNullString) Then Throw New ArgumentNullException("sourceType")

                Dim ret As New CCollection(Of AzioneRegistrata)
                For Each item As AzioneRegistrata In RegisteredActions
                    If (item.SourceName = "" OrElse item.SourceName = sourceName) Then
                        ret.Add(item)
                    End If
                Next

                Return ret
            End SyncLock
        End Function

        
    End Class


End Class