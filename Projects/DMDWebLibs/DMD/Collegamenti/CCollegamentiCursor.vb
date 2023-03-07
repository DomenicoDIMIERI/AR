Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class WebSite

    Public Class CCollegamentiCursor
        Inherits DBObjectCursor(Of CCollegamento)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Link As New CCursorFieldObj(Of String)("Link")
        Private m_Gruppo As New CCursorFieldObj(Of String)("Gruppo")
        Private m_Target As New CCursorFieldObj(Of String)("Target")
        Private m_IDParent As New CCursorField(Of Integer)("IDParent")
        Private m_CallModule As New CCursorFieldObj(Of String)("CallModule")
        Private m_CallAction As New CCursorFieldObj(Of String)("CallAction")
        Private m_OnlyAssigned As Boolean
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")
        Private m_Posizione As New CCursorField(Of Integer)("Posizione")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_PostedData As New CCursorFieldObj(Of String)("PostedData")

        Public Sub New()
            Me.m_OnlyAssigned = False
        End Sub

        Public ReadOnly Property PostedData As CCursorFieldObj(Of String)
            Get
                Return Me.m_PostedData
            End Get
        End Property

        Public ReadOnly Property Attivo As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property

        Public ReadOnly Property Posizione As CCursorField(Of Integer)
            Get
                Return Me.m_Posizione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property CallModule As CCursorFieldObj(Of String)
            Get
                Return Me.m_CallModule
            End Get
        End Property

        Public ReadOnly Property CallAction As CCursorFieldObj(Of String)
            Get
                Return Me.m_CallAction
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Link As CCursorFieldObj(Of String)
            Get
                Return Me.m_Link
            End Get
        End Property

        Public ReadOnly Property IDParent As CCursorField(Of Integer)
            Get
                Return Me.m_IDParent
            End Get
        End Property

        Public ReadOnly Property Gruppo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Gruppo
            End Get
        End Property

        Public ReadOnly Property Target As CCursorFieldObj(Of String)
            Get
                Return Me.m_Target
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCollegamento
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_Collegamenti"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Collegamenti.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica che il cursore deve restituire solo i link assegnati all'utente o al gruppo di utenti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OnlyAssigned As Boolean
            Get
                Return Me.m_OnlyAssigned
            End Get
            Set(value As Boolean)
                If (Me.m_OnlyAssigned = value) Then Exit Property
                Me.m_OnlyAssigned = value
                Me.Reset1()
            End Set
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("OnlyAssigned", Me.m_OnlyAssigned)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OnlyAssigned" : Me.m_OnlyAssigned = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function GetWherePart() As String
            Dim ret As String = MyBase.GetWherePart()
            If (Me.m_OnlyAssigned) AndAlso Me.Module.UserCanDoAction("list_assigned") Then
                Dim u As CUser = Sistema.Users.CurrentUser

                Dim tmpSQL As New System.Text.StringBuilder
                tmpSQL.Append("( [ID] In ")
                tmpSQL.Append("( SELECT [Collegamento] FROM ")
                tmpSQL.Append("(SELECT [Collegamento] FROM [tbl_CollegamentiXGruppo] WHERE [Gruppo] In (0,NULL")
                'SELECT DISTINCT [Group] FROM [tbl_UsersXGroup] WHERE [User]=" & userID 
                SyncLock u
                    For Each uff As CUfficio In u.Uffici
                        tmpSQL.Append(",")
                        tmpSQL.Append(GetID(uff))
                    Next
                End SyncLock
                tmpSQL.Append(") ")
                tmpSQL.Append("UNION ")
                tmpSQL.Append("SELECT [Collegamento] FROM [tbl_CollegamentiXUtente] WHERE [Utente]=")
                tmpSQL.Append(GetID(u))
                tmpSQL.Append(") GROUP BY [Collegamento]")
                tmpSQL.Append(")")
                tmpSQL.Append(")")

                ret = Strings.Combine(ret, tmpSQL.ToString, " AND ")
            End If

            Return ret
        End Function

    End Class

End Class

