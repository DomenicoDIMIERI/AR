Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    Public Enum StatoBUI As Integer
        ''' <summary>
        ''' La BUI non è stata ancora proposta
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONPROPOSTO = 0

        ''' <summary>
        ''' La BUI 			
        ''' </summary>
        ''' <remarks></remarks>
        STATO_PROPOSTA = 1

        ''' <summary>
        ''' L'operatore ha proposto il prodotto e si sta attendendo l'esito della valutazione  			
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ATTESAAPPROVAZIONE = 3

        ''' <summary>
        ''' Il supervisore ha approvato le richieste del produttore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_APPROVATO = 5

        ''' <summary>
        ''' Il supervisore non ha approvato le richieste del produttore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONAPPROVATO = 7

        ''' <summary>
        ''' Il produttore ha accettato l'offerta così come è stata approvata dal supervisore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ACCETTATO = 9

        ''' <summary>
        ''' Il produttore non ha accettato l'ultima offerta
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONACCETTATO = 11
    End Enum


    Public Enum StatoTrattativa As Integer
        ''' <summary>
        ''' 'L'operatore non ha proposto il prodotto
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONPROPOSTO = 0

        ''' <summary>
        ''' L'operatore ha proposto il prodotto ed il produttore ha formulato le sue richieste			
        ''' </summary>
        ''' <remarks></remarks>
        STATO_PROPOSTA = 1

        ''' <summary>
        ''' L'operatore ha proposto il prodotto e si sta attendendo l'esito della valutazione  			
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ATTESAAPPROVAZIONE = 3

        ''' <summary>
        ''' Il supervisore ha approvato le richieste del produttore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_APPROVATO = 5

        ''' <summary>
        ''' Il supervisore non ha approvato le richieste del produttore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONAPPROVATO = 7

        ''' <summary>
        ''' Il produttore ha accettato l'offerta così come è stata approvata dal supervisore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ACCETTATO = 9

        ''' <summary>
        ''' Il produttore non ha accettato l'ultima offerta
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONACCETTATO = 11
    End Enum

    <Serializable> _
    Public Class CTrattativaCollaboratore
        Inherits DBObject

        Private m_Richiesto As Boolean
        Private m_Collaboratore As CCollaboratore
        Private m_CollaboratoreID As Integer
        Private m_CessionarioID As Integer
        Private m_Cessionario As CCQSPDCessionarioClass
        Private m_NomeCessionario As String
        Private m_ProdottoID As Integer
        Private m_Prodotto As CCQSPDProdotto
        Private m_NomeProdotto As String
        Private m_StatoTrattativa As StatoTrattativa
        Private m_PropostoDa As CUser
        Private m_PropostoDaID As Integer
        Private m_PropostoIl As Nullable(Of Date)
        Private m_SpreadProposto As Double
        Private m_SpreadRichiesto As Double
        Private m_SpreadApprovato As Double
        Private m_ApprovatoDaID As Integer
        Private m_ApprovatoDa As CUser
        Private m_ApprovatoIl As Nullable(Of Date)
        Private m_Note As String

        Public Sub New()
            Me.m_Richiesto = False
            Me.m_CollaboratoreID = 0
            Me.m_CessionarioID = 0
            Me.m_ProdottoID = 0
            Me.m_ApprovatoDaID = 0
            Me.m_PropostoDaID = 0
            Me.m_PropostoDa = Nothing
            Me.m_Cessionario = Nothing
            Me.m_Collaboratore = Nothing
            Me.m_Prodotto = Nothing
            Me.m_ApprovatoDa = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.TrattativeCollaboratore.Module
        End Function

        Public Property Richiesto As Boolean
            Get
                Return Me.m_Richiesto
            End Get
            Set(value As Boolean)
                If (Me.m_Richiesto = value) Then Exit Property
                Me.m_Richiesto = value
                Me.DoChanged("Richiesto", value, Not value)
            End Set
        End Property

        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then Me.m_Collaboratore = CQSPD.Collaboratori.GetItemById(Me.m_CollaboratoreID)
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.Collaboratore
                If (oldValue = value) Then Exit Property
                Me.m_Collaboratore = value
                Me.m_CollaboratoreID = GetID(value)
                Me.DoChanged("Collaboratore", value, oldValue)
            End Set
        End Property

        Public Property ProdottoID As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_ProdottoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ProdottoID
                If oldValue = value Then Exit Property
                Me.m_ProdottoID = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("ProdottoID", value, oldValue)
            End Set
        End Property

        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

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
                If (value IsNot Nothing) Then Me.m_NomeProdotto = value.Descrizione
            End Set
        End Property

        Public Property NomeProdotto As String
            Get
                Return Me.m_NomeProdotto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeProdotto
                If (oldValue = value) Then Exit Property
                Me.m_NomeProdotto = value
                Me.DoChanged("NomeProdotto", value, oldValue)
            End Set
        End Property

        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = CQSPD.Cessionari.GetItemById(Me.m_CessionarioID)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Cessionario
                If (oldValue = value) Then Exit Property
                Me.m_Cessionario = value
                Me.m_CessionarioID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        Public Property NomeCessionario As String
            Get
                Return Me.m_NomeCessionario
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCessionario
                If (oldValue = value) Then Exit Property
                Me.m_NomeCessionario = value
                Me.DoChanged("NomeCessionario", value, oldValue)
            End Set
        End Property

        Public Property PropostoIl As Nullable(Of Date)
            Get
                Return Me.m_PropostoIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_PropostoIl
                If (oldValue = value) Then Exit Property
                Me.m_PropostoIl = value
                Me.DoChanged("PropostoIl", value, oldValue)
            End Set
        End Property

        Public Property PropostoDaID As Integer
            Get
                Return GetID(Me.m_PropostoDa, Me.m_PropostoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.PropostoDaID
                If oldValue = value Then Exit Property
                Me.m_PropostoDa = Nothing
                Me.m_PropostoDaID = value
                Me.DoChanged("PropostoDaID", value, oldValue)
            End Set
        End Property

        Public Property PropostoDa As CUser
            Get
                If (Me.m_PropostoDa Is Nothing) Then Me.m_PropostoDa = Sistema.Users.GetItemById(Me.m_PropostoDaID)
                Return Me.m_PropostoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.PropostoDa
                If (oldValue = value) Then Exit Property
                Me.m_PropostoDa = value
                Me.m_PropostoDaID = GetID(value)
                Me.DoChanged("PropostoDa", value, oldValue)
            End Set
        End Property

        Public Function PropostoDaUtente() As String
            If Me.PropostoDa Is Nothing Then Return "UserID: " & Me.PropostoDaID
            Return Me.PropostoDa.Nominativo
        End Function

        Public Property StatoTrattativa As StatoTrattativa
            Get
                Return Me.m_StatoTrattativa
            End Get
            Set(value As StatoTrattativa)
                Dim oldValue As StatoTrattativa = Me.m_StatoTrattativa
                If (oldValue = value) Then Exit Property
                Me.m_StatoTrattativa = value
                Me.DoChanged("StatoTrattativa", value, oldValue)
            End Set
        End Property

        Public Property StatoTrattativaEx As String
            Get
                Return CQSPD.TrattativeCollaboratore.FormatStato(Me.StatoTrattativa)
            End Get
            Set(value As String)
                Me.StatoTrattativa = CQSPD.TrattativeCollaboratore.ParseStato(value)
            End Set
        End Property

        Public Property SpreadProposto As Double
            Get
                Return Me.m_SpreadProposto
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_SpreadProposto
                If (oldValue = value) Then Exit Property
                Me.m_SpreadProposto = value
                Me.DoChanged("SpreadProposto", value, oldValue)
            End Set
        End Property

        Public Property SpreadRichiesto As Double
            Get
                Return Me.m_SpreadRichiesto
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_SpreadRichiesto
                If (oldValue = value) Then Exit Property
                Me.m_SpreadRichiesto = value
                Me.DoChanged("SpreadRichiesto", value, oldValue)
            End Set
        End Property

        Public Property SpreadApprovato As Double
            Get
                Return Me.m_SpreadApprovato
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_SpreadApprovato
                If (oldValue = value) Then Exit Property
                Me.m_SpreadApprovato = value
                Me.DoChanged("SpreadApprovato", value, oldValue)
            End Set
        End Property

        Public Property ApprovatoDa As CUser
            Get
                If (Me.m_ApprovatoDa Is Nothing) Then Me.m_ApprovatoDa = Sistema.Users.GetItemById(Me.m_ApprovatoDaID)
                Return Me.m_ApprovatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.ApprovatoDa
                If (oldValue = value) Then Exit Property
                Me.m_ApprovatoDa = value
                Me.m_ApprovatoDaID = GetID(value)
                Me.DoChanged("ApprovatoDa", value, oldValue)
            End Set
        End Property

        Public Property ApprovatoIl As Nullable(Of Date)
            Get
                Return Me.m_ApprovatoIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ApprovatoIl
                If (oldValue = value) Then Exit Property
                Me.m_ApprovatoIl = value
                Me.DoChanged("ApprovatoIl", value, oldValue)
            End Set
        End Property

        Public Function ApprovatoDaUtente() As String
            If Me.ApprovatoDa Is Nothing Then Return "UserID: " & Me.m_ApprovatoDaID
            Return Me.ApprovatoDa.Nominativo
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CollaboratoriTrattative"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            'reader.Read("Richiesto", Me.m_Richiesto)
            'reader.Read("Collaboratore", Me.m_CollaboratoreID)
            'm_CessionarioID = Formats.ToInteger(dbRis("Cessionario"))
            'm_NomeCessionario = Formats.ToString(dbRis("NomeCessionario"))
            'm_ProdottoID = Formats.ToInteger(dbRis("Prodotto"))
            'm_NomeProdotto = Formats.ToString(dbRis("NomeProdotto"))
            'm_PropostoIl = Formats.ParseDate(dbRis("PropostoIl"))
            'm_PropostoDaID = Formats.ToInteger(dbRis("PropostoDa"))
            'm_StatoTrattativa = Formats.ToInteger(dbRis("StatoTrattativa"))
            'm_SpreadProposto = Formats.ToDouble(dbRis("SpreadProposto"))
            'm_SpreadRichiesto = Formats.ToDouble(dbRis("SpreadRichiesto"))
            'm_SpreadApprovato = Formats.ToDouble(dbRis("SpreadApprovato"))
            'm_ApprovatoDaID = Formats.ToInteger(dbRis("ApprovatoDa"))
            'm_ApprovatoIl = Formats.ParseDate(dbRis("ApprovatoIl"))
            'm_Note = Formats.ToString(dbRis("Note"))
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Richiesto", Me.m_Richiesto)
            writer.Write("Collaboratore", GetID(Me.m_Collaboratore, Me.m_CollaboratoreID))
            writer.Write("Cessionario", GetID(Me.m_Cessionario, Me.m_CessionarioID))
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("Prodotto", GetID(Me.m_Prodotto, Me.m_ProdottoID))
            writer.Write("NomeProdotto", Me.m_NomeProdotto)
            writer.Write("PropostoIl", Me.m_PropostoIl)
            writer.Write("PropostoDa", GetID(Me.m_PropostoDa, Me.m_PropostoDaID))
            writer.Write("StatoTrattativa", Me.m_StatoTrattativa)
            writer.Write("SpreadProposto", Me.m_SpreadProposto)
            writer.Write("SpreadRichiesto", Me.m_SpreadRichiesto)
            writer.Write("SpreadApprovato", Me.m_SpreadApprovato)
            writer.Write("ApprovatoDa", GetID(Me.m_ApprovatoDa, Me.m_ApprovatoDaID))
            writer.Write("ApprovatoIl", Me.m_ApprovatoIl)
            writer.Write("Note", Me.m_Note)
            Return MyBase.SaveToRecordset(writer)
        End Function

    End Class

End Class
