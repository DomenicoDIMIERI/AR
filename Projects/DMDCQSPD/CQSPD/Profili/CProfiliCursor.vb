Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD


    Public Class CProfiliCursor
        Inherits DBObjectCursor(Of CProfilo)

        Private m_CessionarioID As New CCursorField(Of Integer)("Cessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Profilo As New CCursorFieldObj(Of String)("Profilo")
        Private m_ProfiloVisibile As New CCursorFieldObj(Of String)("ProfiloVisibile")
        Private m_UserName As New CCursorFieldObj(Of String)("UserName")
        Private m_Visibile As New CCursorField(Of Boolean)("Visibile")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_ParentID As New CCursorField(Of Integer)("Padre")
        Private m_ShowInterni As Boolean
        Private m_ShowEsterni As Boolean
        Private m_OnlyValid As Boolean
        Private m_OnlyAssigned As Boolean

        Public Sub New()
            Me.m_ShowInterni = True
            Me.m_ShowEsterni = True
            Me.m_OnlyAssigned = False
            Me.m_OnlyValid = False
        End Sub

        Public Property OnlyAssigned As Boolean
            Get
                Return Me.m_OnlyAssigned
            End Get
            Set(value As Boolean)
                Me.m_OnlyAssigned = value
            End Set
        End Property

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Public Property ShowInterni As Boolean
            Get
                Return Me.m_ShowInterni
            End Get
            Set(value As Boolean)
                Me.m_ShowInterni = value
            End Set
        End Property

        Public Property ShowEsterni As Boolean
            Get
                Return Me.m_ShowEsterni
            End Get
            Set(value As Boolean)
                Me.m_ShowEsterni = value
            End Set
        End Property

        Public ReadOnly Property IDCessionario As CCursorField(Of Integer)
            Get
                Return Me.m_CessionarioID
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        Public ReadOnly Property Visibile As CCursorField(Of Boolean)
            Get
                Return Me.m_Visibile
            End Get
        End Property

        Public ReadOnly Property ParentID As CCursorField(Of Integer)
            Get
                Return Me.m_ParentID
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Profilo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Profilo
            End Get
        End Property

        Public ReadOnly Property ProfiloVisibile As CCursorFieldObj(Of String)
            Get
                Return Me.m_ProfiloVisibile
            End Get
        End Property

        Public ReadOnly Property UserName As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Profili.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Preventivatori"
        End Function

        Private Function GetTxtGruppi() As String
            Dim ret As New System.Text.StringBuilder
            SyncLock Sistema.Users.CurrentUser
                For Each grp As CGroup In Sistema.Users.CurrentUser.Groups
                    If (ret.Length > 0) Then ret.Append(",")
                    ret.Append(GetID(grp))
                Next
            End SyncLock
            Return ret.ToString
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String
            wherePart = MyBase.GetWherePart()
            If Me.m_ShowInterni = False Then wherePart = Strings.Combine(wherePart, "(([path]<>'') And Not ([path] Is Null))", " AND ")
            If Me.m_ShowEsterni = False Then wherePart = Strings.Combine(wherePart, "(([path]='') Or ([path] Is Null))", " AND ")
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

        Public Overrides Function GetWherePartLimit() As String
            Dim wherePart As String = MyBase.GetWherePartLimit()
            If Me.m_OnlyAssigned AndAlso Me.Module.UserCanDoAction("list_assigned") Then
                Dim userID As Integer = GetID(Users.CurrentUser)
                Dim tmpSQL As String
                Dim txtGruppi As String = Me.GetTxtGruppi
                If (txtGruppi = "") Then
                    tmpSQL = ""
                    tmpSQL &= "([ID] In (SELECT [Preventivatore] FROM [tbl_PreventivatoriXUser] WHERE [Utente]=" & userID & "))"
                Else
                    tmpSQL = ""
                    tmpSQL &= "("
                    tmpSQL &= "[ID] In ("
                    tmpSQL &= "SELECT [Preventivatore] FROM ("
                    tmpSQL &= "SELECT [Preventivatore] FROM [tbl_PreventivatoriXGroup] WHERE [Gruppo] In (" & txtGruppi & ") "
                    tmpSQL &= "UNION "
                    tmpSQL &= "SELECT [Preventivatore] FROM [tbl_PreventivatoriXUser] WHERE [Utente]=" & userID & " "
                    tmpSQL &= ") GROUP BY [Preventivatore]"
                    tmpSQL &= ")"
                    tmpSQL &= ")"
                End If
                wherePart = Strings.Combine(wherePart, tmpSQL, " AND ")
            End If
            Return wherePart
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CProfilo
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_ShowInterni", Me.m_ShowInterni)
            writer.WriteAttribute("m_ShowEsterni", Me.m_ShowEsterni)
            writer.WriteAttribute("m_OnlyAssigned", Me.m_OnlyAssigned)
            writer.WriteAttribute("m_OnlyValid", Me.m_OnlyValid)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case (fieldName)
                Case "m_ShowInterni" : Me.m_ShowInterni = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_ShowEsterni" : Me.m_ShowEsterni = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_OnlyAssigned" : Me.m_OnlyAssigned = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_OnlyValid" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class
