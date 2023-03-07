Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Vincoli su una tabella assicurativa
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CProdTabAssConstraint
        Inherits CTableConstraint

        Private m_OwnerID As Integer
        Private m_Owner As CProdottoXTabellaAss

        Public Sub New()
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property OwnerID As Integer
            Get
                Return GetID(Me.m_Owner, Me.m_OwnerID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.OwnerID
                If oldValue = value Then Exit Property
                Me.m_Owner = Nothing
                Me.m_OwnerID = value
                Me.DoChanged("OwnerID", value, oldValue)
            End Set
        End Property

        Public Property Owner As CProdottoXTabellaAss
            Get
                If (Me.m_Owner Is Nothing) Then Me.m_Owner = CQSPD.TabelleAssicurative.GetTabellaXProdottoByID(Me.m_OwnerID)
                Return Me.m_Owner
            End Get
            Set(value As CProdottoXTabellaAss)
                Dim oldValue As CProdottoXTabellaAss = Me.Owner
                If (oldValue = value) Then Exit Property
                Me.SetOwner(value)
                Me.DoChanged("Owner", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetOwner(ByVal value As CProdottoXTabellaAss)
            Me.m_Owner = value
            Me.m_OwnerID = GetID(value)
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_ProdXTabAssConstr"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Owner", Me.m_OwnerID)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Owner", GetID(Me.m_Owner, Me.m_OwnerID))
            Return MyBase.SaveToRecordset(writer)
        End Function

    End Class


End Class