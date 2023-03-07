Imports FinSeA.Databases

Partial Public Class Anagrafica1

    Partial Public Class Azioni

        ''' <summary>
        ''' Rappresenta un'azione che una persona o un'azienda può compiere.
        ''' Un'azione può essere gestita da uno o più handlers.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class AzioneDefinita
            Inherits DBObject

            Private m_Nome As String                'Nome dell'azione 
            Private m_Descrizione As String         'Descrizione estesa dell'azione
            Private m_Contesto As String            'Limita l'azione ad un contesto specifico
            Private m_Handlers As CKeyCollection(Of AzioneHandler) 'Collezione degli handlers definiti per l'azione

            Public Sub New()
                Me.m_Nome = vbNullString
                Me.m_Descrizione = vbNullString
                Me.m_Contesto = vbNullString
                Me.m_Handlers = Nothing
            End Sub

            ''' <summary>
            ''' Restituisce o imposta il nome dell'azione
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Nome As String
                Get
                    Return Me.m_Nome
                End Get
                Set(value As String)
                    value = Trim(value)
                    Dim oldValue As String = Me.m_Nome
                    If (oldValue = value) Then Exit Property
                    Me.m_Nome = value
                    Me.DoChanged("Nome", value, value)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta la descrizione estesa dell'azione
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Descrizione As String
                Get
                    Return Me.m_Descrizione
                End Get
                Set(value As String)
                    Dim oldValue As String = Me.m_Descrizione
                    If (oldValue = value) Then Exit Property
                    Me.m_Descrizione = value
                    Me.DoChanged("Descrizione", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta il contesto di validità dell'azione
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Contesto As String
                Get
                    Return Me.m_Contesto
                End Get
                Set(value As String)
                    value = Trim(value)
                    Dim oldValue As String = Me.m_Contesto
                    If (oldValue = value) Then Exit Property
                    Me.m_Contesto = value
                    Me.DoChanged("Contesto", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce una collezione enumerabile degli handlers definiti per l'azione
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Handlers As IEnumerable(Of AzioneHandler)
                Get
                    If (Me.m_Handlers Is Nothing) Then Me.m_Handlers = LoadHandlers()
                    Return Me.m_Handlers
                End Get
            End Property

            Private Function LoadHandlers() As CKeyCollection(Of AzioneHandler)
                Dim ret As New CKeyCollection(Of AzioneHandler)
                Dim dbRis As System.Data.IDataReader = Me.GetConnection.ExecuteReader("SELECT * FROM [tbl_SActionHandlers] WHERE [IDAzione]=" & GetID(Me) & " ORDER BY [Priority] ASC")
                While dbRis.Read
                    Dim className As String = Formats.ToString(dbRis("ClassName"))
                    Dim handler As AzioneHandler
                    Try
                        handler = FinSeA.Types.CreateInstance(className)
                        ret.Add(handler.GetType.FullName, handler)
                    Catch ex As Exception
                        Debug.Print("Errore nella creazione dell'Handler '" & className & "' per l'azione '" & Me.Nome & "'" & vbNewLine & ex.ToString)
                    End Try
                End While
                dbRis.Dispose()
                Return ret
            End Function

            ''' <summary>
            ''' Aggiunge l'handler all'azione
            ''' </summary>
            ''' <param name="handler"></param>
            ''' <remarks></remarks>
            Public Sub [AddHandler](ByVal handler As Object)
                If (handler Is Nothing) Then
                    Throw New ArgumentNullException("handler")
                End If

                Dim tmp As AzioneHandler = handler
                Dim key As String = handler.GetType.FullName

                'verifichiamo che l'handler non sia già presente
                If DirectCast(Me.Handlers, CKeyCollection(Of AzioneHandler)).ContainsKey(key) Then
                    Throw New Exception("L'handler '" & tmp.Nome & "' è già stato memorizzato per l'azione '" & Me.Nome & "'")
                End If

                'Memorizziamo l'azione nel DB
                Dim dbSQL As String = "UPDATE [tbl_SActionHandlers] ADD ([ClassName], [IDAzione], [Nome], [Priority]) VALUES (" & DBString(key) & "," & GetID(Me) & ", " & DBString(tmp.Nome) & ", " & DBNumber(tmp.Priority) & ")"
                Me.GetConnection.ExecuteCommand(dbSQL)

                'Aggiungiamo l'azione all'elenco
                Me.m_Handlers.Add(key, tmp)
            End Sub

            ''' <summary>
            ''' Rimuove l'handler
            ''' </summary>
            ''' <param name="handler"></param>
            ''' <remarks></remarks>
            Public Sub [RemoveHandler](ByVal handler As Object)
                If (handler Is Nothing) Then
                    Throw New ArgumentNullException("handler")
                End If

                Dim tmp As AzioneHandler = handler
                Dim key As String = handler.GetType.FullName

                'Memorizziamo l'azione nel DB
                Dim dbSQL As String = "DELETE * FROM [tbl_SActionHandlers] WHERE [ClassName]=" & DBString(handler.GetType.FullName) & " AND [IDAzione]=" & GetID(Me)
                Me.GetConnection.ExecuteCommand(dbSQL)

                'Aggiungiamo l'azione all'elenco
                If (Me.m_Handlers IsNot Nothing) Then
                    Me.m_Handlers.RemoveByKey(key)
                End If
            End Sub



            Protected Overrides Function GetConnection() As CDBConnection
                Return APPConn
            End Function

            Public Overrides Function GetModule() As Sistema.CModule
                Return Nothing
            End Function

            Protected Overrides Function GetTableName() As String
                Return "tbl_SAActionDef"
            End Function
        End Class

    End Class

End Class