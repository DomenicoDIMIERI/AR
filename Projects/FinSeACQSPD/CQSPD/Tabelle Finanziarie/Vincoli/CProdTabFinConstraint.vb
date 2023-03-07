Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CProdTabFinConstraint
        Inherits CTableConstraint

        Private m_OwnerID As Integer 'ID dell'oggetto relazione prodotto X tabella finanziaria
        Private m_Owner As CProdottoXTabellaFin 'Oggetto relazione prodotto X tabella finanziaria

        Public Sub New()
            Me.m_OwnerID = 0
            Me.m_Owner = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        ''' <summary>
        ''' ID dell'oggetto relazione prodotto X tabella finanziaria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OwnerID As Integer
            Get
                Return GetID(Me.m_Owner, Me.m_OwnerID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.OwnerID
                If oldValue = value Then Exit Property
                Me.m_OwnerID = value
                Me.m_Owner = Nothing
                Me.DoChanged("OwnerID", value, oldValue)
            End Set
        End Property

        Public Property Owner As CProdottoXTabellaFin
            Get
                If (Me.m_Owner Is Nothing) Then Me.m_Owner = DMD.CQSPD.TabelleFinanziarie.GetTabellaXProdottoByID(Me.m_OwnerID)
                Return Me.m_Owner
            End Get
            Set(value As CProdottoXTabellaFin)
                Dim oldValue As CProdottoXTabellaFin = Me.Owner
                If (oldValue = value) Then Exit Property
                Me.SetOwner(value)
                Me.DoChanged("Owner", value, oldValue)
            End Set
        End Property
        Friend Sub SetOwner(ByVal value As CProdottoXTabellaFin)
            Me.m_Owner = value
            Me.m_OwnerID = GetID(value)
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_ProdXTabFinConstr"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_OwnerID = reader.Read("Owner", Me.m_OwnerID)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Owner", Me.OwnerID)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("OwnerID", Me.OwnerID)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OwnerID" : Me.m_OwnerID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class