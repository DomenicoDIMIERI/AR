Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica
Imports DMD.XML

Partial Public Class CustomerCalls

    Public Enum TipoCampagnaCRM As Integer
        Normale = 0
        Autochiamata = 1
        Predictive = 2
    End Enum



    <Flags>
    Public Enum CampagnaCRMFlag As Integer
        None = 0
        Disattiva = 1
        RichiediApprovazionePause = 2
    End Enum

    <Serializable>
    Public Class CCampagnaCRM
        Inherits DBObjectPO


        Private m_Nome As String
        Private m_Inizio As Date?
        Private m_Fine As Date?
        Private m_TipoAssegnazione As TipoCampagnaCRM
        Private m_TipoCampagna As String
        Private m_Flags As CampagnaCRMFlag
        Private m_Attributi As CKeyCollection
        Private m_Gruppi As CCollection(Of CCampagnaXGroupAllowNegate)
        Private m_Utenti As CCollection(Of CCampagnaXUserAllowNegate)

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Inizio = Nothing
            Me.m_Fine = Nothing
            Me.m_TipoCampagna = ""
            Me.m_TipoAssegnazione = TipoCampagnaCRM.Normale
            Me.m_Flags = CampagnaCRMFlag.None
            Me.m_Attributi = Nothing
            Me.m_Gruppi = Nothing
            Me.m_Utenti = Nothing
        End Sub

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property


        Public Property Inizio As Date?
            Get
                Return Me.m_Inizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_Inizio
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_Inizio = value
                Me.DoChanged("Inizio", value, oldValue)
            End Set
        End Property

        Public Property Fine As Date?
            Get
                Return Me.m_Fine
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_Fine
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_Fine = value
                Me.DoChanged("Fine", value, oldValue)
            End Set
        End Property

        Public Property TipoAssegnazione As TipoCampagnaCRM
            Get
                Return Me.m_TipoAssegnazione
            End Get
            Set(value As TipoCampagnaCRM)
                Dim oldValue As TipoCampagnaCRM = Me.m_TipoAssegnazione
                If (oldValue = value) Then Return
                Me.m_TipoAssegnazione = value
                Me.DoChanged("TipoAssegnazione", value, oldValue)
            End Set
        End Property

        Public Property TipoCampagna As String
            Get
                Return Me.m_TipoCampagna
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoCampagna
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_TipoCampagna = value
                Me.DoChanged("TipoCampagna", value, oldValue)
            End Set
        End Property

        Public Property Flags As CampagnaCRMFlag
            Get
                Return Me.m_Flags
            End Get
            Set(value As CampagnaCRMFlag)
                Dim oldValue As CampagnaCRMFlag = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property Attiva As Boolean
            Get
                Return Not TestFlag(Me.m_Flags, CampagnaCRMFlag.Disattiva)
            End Get
            Set(value As Boolean)
                If Me.Attiva = value Then Return
                Me.m_Flags = SetFlag(Me.m_Flags, CampagnaCRMFlag.Disattiva, Not value)
                Me.DoChanged("Attiva", value, Not value)
            End Set
        End Property

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        Public ReadOnly Property Gruppi As CCollection(Of CCampagnaXGroupAllowNegate)
            Get
                If (Me.m_Gruppi Is Nothing) Then Me.m_Gruppi = New CCollection(Of CCampagnaXGroupAllowNegate)
                Return Me.m_Gruppi
            End Get
        End Property

        Public ReadOnly Property Utenti As CCollection(Of CCampagnaXUserAllowNegate)
            Get
                If (Me.m_Utenti Is Nothing) Then Me.m_Utenti = New CCollection(Of CCampagnaXUserAllowNegate)
                Return Me.m_Utenti
            End Get
        End Property


        Public Function GetGroupAllowNegate(ByVal group As CGroup) As CCampagnaXGroupAllowNegate
            If (group Is Nothing) Then Throw New ArgumentNullException("group")
            For Each item As CCampagnaXGroupAllowNegate In Me.Gruppi
                If (item.GroupID = GetID(group)) Then Return item
            Next
            Return Nothing
        End Function

        Public Function SetGroupAllowNegate(ByVal group As CGroup, ByVal allow As Boolean, ByVal negate As Boolean) As CCampagnaXGroupAllowNegate
            If (group Is Nothing) Then Throw New ArgumentNullException("group")
            Dim item As CCampagnaXGroupAllowNegate = Me.GetGroupAllowNegate(group)
            If (allow = negate) Then
                If (item IsNot Nothing) Then
                    Me.Gruppi.Remove(item)
                    Me.SetChanged("Grouppi")
                End If
            Else
                If (item Is Nothing) Then
                    item = New CCampagnaXGroupAllowNegate
                    item.Item = Me
                    item.Group = group
                    Me.Gruppi.Add(item)
                End If
                item.Allow = allow
                item.Negate = negate
                Me.SetChanged("Grouppi")
            End If
            Return item
        End Function

        Public Function GetUserAllowNegate(ByVal user As CUser) As CCampagnaXUserAllowNegate
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            For Each item As CCampagnaXUserAllowNegate In Me.Utenti
                If (item.UserID = GetID(user)) Then Return item
            Next
            Return Nothing
        End Function

        Public Function SetUserAllowNegate(ByVal User As CUser, ByVal allow As Boolean, ByVal negate As Boolean) As CCampagnaXUserAllowNegate
            If (User Is Nothing) Then Throw New ArgumentNullException("User")
            Dim item As CCampagnaXUserAllowNegate = Me.GetUserAllowNegate(User)
            If (allow = negate) Then
                If (item IsNot Nothing) Then
                    Me.Utenti.Remove(item)
                    Me.SetChanged("Utenti")
                End If
            Else
                If (item Is Nothing) Then
                    item = New CCampagnaXUserAllowNegate
                    item.Item = Me
                    item.User = User
                    Me.Utenti.Add(item)
                End If
                item.Allow = allow
                item.Negate = negate
                Me.SetChanged("Utenti")
            End If
            Return item
        End Function

        Public Function IsGroupAllowed(ByVal grp As CGroup) As Boolean
            Dim item As CCampagnaXGroupAllowNegate = Me.GetGroupAllowNegate(grp)
            If (item Is Nothing) Then Return False
            Return item.Allow AndAlso Not item.Negate
        End Function

        Public Function IsUserAllowed(ByVal user As CUser) As Boolean
            Dim allow As Boolean = False, negate As Boolean = False
            Dim u As CCampagnaXUserAllowNegate = Me.GetUserAllowNegate(user)
            If (u IsNot Nothing) Then
                allow = u.Allow
                negate = u.Negate
            End If
            If (Not negate) Then
                For Each grp As CGroup In user.Groups
                    If grp.Stato = ObjectStatus.OBJECT_VALID Then
                        Dim g As CCampagnaXGroupAllowNegate = Me.GetGroupAllowNegate(grp)
                        If (g IsNot Nothing) Then
                            allow = allow Or g.Allow
                            negate = negate Or g.Negate
                            If negate Then Exit For
                        End If
                    End If
                Next
            End If

            Return allow AndAlso Not negate
        End Function


        Public Overrides Function GetModule() As CModule
            Return CustomerCalls.CampagneCRM.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CustomerCalls.CRM.StatsDB
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CampagnaCRM"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Inizio = reader.Read("Inizio", Me.m_Inizio)
            Me.m_Fine = reader.Read("Fine", Me.m_Fine)
            Me.m_TipoAssegnazione = reader.Read("TipoAssegnazione", Me.m_TipoAssegnazione)
            Me.m_TipoCampagna = reader.Read("TipoCampagna", Me.m_TipoCampagna)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Dim tmp As String
            tmp = reader.Read("Attributi", "")
            If (tmp <> "") Then Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)

            tmp = reader.Read("Gruppi", "")
            If (tmp <> "") Then
                Me.m_Gruppi = New CCollection(Of CCampagnaXGroupAllowNegate)
                Me.m_Gruppi.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            End If

            tmp = reader.Read("Utenti", "")
            If (tmp <> "") Then
                Me.m_Utenti = New CCollection(Of CCampagnaXUserAllowNegate)
                Me.m_Utenti.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            End If

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Inizio", Me.m_Inizio)
            writer.Write("Fine", Me.m_Fine)
            writer.Write("TipoAssegnazione", Me.m_TipoAssegnazione)
            writer.Write("TipoCampagna", Me.m_TipoCampagna)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("Gruppi", XML.Utils.Serializer.Serialize(Me.Gruppi))
            writer.Write("Utenti", XML.Utils.Serializer.Serialize(Me.Utenti))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Inizio", Me.m_Inizio)
            writer.WriteAttribute("Fine", Me.m_Fine)
            writer.WriteAttribute("TipoAssegnazione", Me.m_TipoAssegnazione)
            writer.WriteAttribute("TipoCampagna", Me.m_TipoCampagna)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("Gruppi", Me.Gruppi)
            writer.WriteTag("Utenti", Me.Utenti)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Inizio" : Me.m_Inizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Fine" : Me.m_Fine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoAssegnazione" : Me.m_TipoAssegnazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoCampagna" : Me.m_TipoCampagna = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case "Gruppi" : Me.m_Gruppi = New CCollection(Of CCampagnaXGroupAllowNegate) : Me.m_Gruppi.AddRange(fieldValue)
                Case "Utenti" : Me.m_Utenti = New CCollection(Of CCampagnaXUserAllowNegate) : Me.m_Utenti.AddRange(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            CustomerCalls.CampagneCRM.UpdateCached(Me)
        End Sub

    End Class



End Class