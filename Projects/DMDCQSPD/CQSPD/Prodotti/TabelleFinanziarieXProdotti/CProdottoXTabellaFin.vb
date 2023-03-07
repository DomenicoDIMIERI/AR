Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

       
    ''' <summary>
    ''' Relazione Prodotto - Tabella Finanziaria
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CProdottoXTabellaFin
        Inherits DBObject

        Private m_ProdottoID As Integer    'ID del prodotto associato
        Private m_Prodotto As CCQSPDProdotto 'Oggetto Prodotto Associato
        Private m_TabellaID As Integer  'ID della tabella finanziaria associata
        Private m_Tabella As CTabellaFinanziaria 'Oggetto tabella finanziaria associata
        Private m_Vincoli As CVincoliProdottoTabellaFin    'Collezione di vincoli

        Public Sub New()
            Me.m_ProdottoID = 0
            Me.m_Prodotto = Nothing
            Me.m_TabellaID = 0
            Me.m_Tabella = Nothing
            Me.m_Vincoli = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        ''' <summary>
        ''' ID del prodotto associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDProdotto As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_ProdottoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProdotto
                If oldValue = value Then Exit Property
                Me.m_ProdottoID = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("IDProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Oggetto Prodotto Associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prodotto As CCQSPDProdotto
            Get
                If (Me.m_Prodotto Is Nothing) Then Me.m_Prodotto = CQSPD.Prodotti.GetItemById(Me.m_ProdottoID)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.Prodotto
                If (oldValue = value) Then Exit Property
                Me.m_Prodotto = value
                Me.m_ProdottoID = GetID(value)
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' ID della tabella finanziaria associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTabella As Integer
            Get
                Return GetID(Me.m_Tabella, Me.m_TabellaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabella
                If oldValue = value Then Exit Property
                Me.m_TabellaID = value
                Me.m_Tabella = Nothing
                Me.DoChanged("IDTabella", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Oggetto tabella finanziaria associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tabella As CTabellaFinanziaria
            Get
                If (Me.m_Tabella Is Nothing) Then Me.m_Tabella = CQSPD.TabelleFinanziarie.GetItemById(Me.m_TabellaID)
                Return Me.m_Tabella
            End Get
            Set(value As CTabellaFinanziaria)
                Dim oldValue As CTabellaFinanziaria = Me.Tabella
                If (oldValue = value) Then Exit Property
                Me.m_Tabella = value
                Me.m_TabellaID = GetID(value)
                Me.DoChanged("Tabella", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetTabella(ByVal value As CTabellaFinanziaria)
            Me.m_Tabella = value
            Me.m_TabellaID = GetID(value)
        End Sub

        Public ReadOnly Property Vincoli As CVincoliProdottoTabellaFin
            Get
                If (Me.m_Vincoli Is Nothing) Then
                    Me.m_Vincoli = New CVincoliProdottoTabellaFin
                    Me.m_Vincoli.Initialize(Me)
                End If
                Return Me.m_Vincoli
            End Get
        End Property

        ''' <summary>
        ''' Controlla che la relazione sia applicazione
        ''' </summary>
        ''' <param name="offerta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Check(ByVal offerta As COffertaCQS) As Boolean
            Return Me.Vincoli.Check(offerta)
        End Function

        Public Sub Calcola(ByVal offerta As COffertaCQS)
            If (offerta Is Nothing) Then Throw New ArgumentNullException("offerta")
            Dim pCoeffBase As Double? = Me.Tabella.CoefficienteBase(offerta.Durata)
            Dim pCommissBanc As Double? = Me.Tabella.CommissioniBancarie(offerta.Durata)
            'Calcoliamo le commissioni bancarie
            If (pCommissBanc.HasValue) Then
                offerta.CommissioniBancarie = offerta.MontanteLordo * pCommissBanc.Value / 100
            Else
                offerta.CommissioniBancarie = 0
            End If
            'Calcoliamo gli interessi come differenza rispetto al coefficiente base 
            If (pCoeffBase.HasValue) Then
                offerta.Interessi = (offerta.MontanteLordo * pCoeffBase.Value / 100) - offerta.CommissioniBancarie
            End If
            'Aggiungiamo lo spread base alle commissioni bancarie
            offerta.CommissioniBancarie += offerta.MontanteLordo * offerta.SpreadBase / 100
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_ProdXTabFin"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged()
            If (Not ret AndAlso Me.m_Vincoli IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Vincoli)
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If ret And (Not Me.m_Vincoli Is Nothing) Then Me.m_Vincoli.Save(force)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_ProdottoID = reader.Read("Prodotto", Me.m_ProdottoID)
            Me.m_TabellaID = reader.Read("Tabella", Me.m_TabellaID)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Prodotto", GetID(Me.m_Prodotto, Me.m_ProdottoID))
            writer.Write("Tabella", GetID(Me.m_Tabella, Me.m_TabellaID))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDProdotto", Me.IDProdotto)
            writer.WriteAttribute("IDTabella", Me.IDTabella)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Vincoli", Me.Vincoli.ToArray)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDProdotto" : Me.m_ProdottoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabella" : Me.m_TabellaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Vincoli"
                    Me.Vincoli.Clear()
                    If IsArray(fieldValue) Then
                        Me.Vincoli.AddRange(fieldValue)
                    ElseIf TypeOf (fieldValue) Is CProdTabFinConstraint Then
                        Me.Vincoli.Add(fieldValue)
                    End If
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Friend Overridable Sub SetProdotto(value As CCQSPDProdotto)
            Me.m_Prodotto = value
            Me.m_ProdottoID = GetID(value)
        End Sub

    End Class


End Class