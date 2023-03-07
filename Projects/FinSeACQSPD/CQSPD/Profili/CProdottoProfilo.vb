Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD


    ''' <summary>
    ''' Valori enumerativi utilizzati per definire la relazione tra un profilo ed un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum IncludeModes As Integer
        ''' <summary>
        ''' Eredita il prodotto dal genitore
        ''' </summary>
        ''' <remarks></remarks>
        Eredita = 0
        ''' <summary>
        ''' Forza l'inclusione del prodotto
        ''' </summary>
        ''' <remarks></remarks>
        Include = 1
        ''' <summary>
        ''' Forza l'esclusione del prodotto
        ''' </summary>
        ''' <remarks></remarks>
        Escludi = -1
    End Enum

    ''' <summary>
    ''' Rappresenta l'associazione tra un prodotto ed un listino
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CProdottoProfilo
        Inherits DBObject

        Private m_ProfiloID As Integer
        Private m_Profilo As CProfilo
        Private m_ProdottoID As Integer
        Private m_Prodotto As CCQSPDProdotto
        Private m_Azione As IncludeModes
        Private m_Spread As Double

        Public Sub New()
            Me.m_ProdottoID = 0
            Me.m_Profilo = Nothing
            Me.m_ProdottoID = 0
            Me.m_Prodotto = Nothing
            Me.m_Azione = IncludeModes.Include
            Me.m_Spread = 0
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'ID del profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDProfilo As Integer
            Get
                Return GetID(Me.m_Profilo, Me.m_ProfiloID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProfilo
                If (oldValue = value) Then Exit Property
                Me.m_ProfiloID = value
                Me.m_Profilo = Nothing
                Me.DoChanged("IDProfilo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto Profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Profilo As CProfilo
            Get
                If (Me.m_Profilo Is Nothing) Then Me.m_Profilo = DMD.CQSPD.Profili.GetItemById(Me.m_ProfiloID)
                Return Me.m_Profilo
            End Get
            Set(value As CProfilo)
                Dim oldValue As CProfilo = Me.m_Profilo
                If (oldValue Is value) Then Exit Property
                Me.m_Profilo = value
                Me.m_ProfiloID = GetID(value)
                Me.DoChanged("Profilo", value, oldValue)
            End Set
        End Property
        Protected Friend Sub SetProfilo(ByVal value As CProfilo)
            Me.m_Profilo = value
            Me.m_ProfiloID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDProdotto As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_ProdottoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProfilo
                If oldValue = value Then Exit Property
                Me.m_ProdottoID = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("IDProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto Prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prodotto As CCQSPDProdotto
            Get
                If (Me.m_Prodotto Is Nothing) Then Me.m_Prodotto = DMD.CQSPD.Prodotti.GetItemById(Me.m_ProdottoID)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.m_Prodotto
                If (oldValue Is value) Then Exit Property
                Me.m_Prodotto = value
                Me.m_ProdottoID = GetID(value)
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property
        Protected Friend Sub SetProdotto(ByVal value As CCQSPDProdotto)
            Me.m_Prodotto = value
            Me.m_ProdottoID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il tipo di relazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Azione As IncludeModes
            Get
                Return Me.m_Azione
            End Get
            Set(value As IncludeModes)
                Dim oldValue As IncludeModes = Me.m_Azione
                If (oldValue = value) Then Exit Property
                Me.m_Azione = value
                Me.DoChanged("Azione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo spread da aggiugnere al prodotto 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Spread As Double
            Get
                Return Me.m_Spread
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Spread
                If (oldValue = value) Then Exit Property
                Me.m_Spread = value
                Me.DoChanged("Spread", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property NomeProdotto As String
            Get
                If Me.Prodotto Is Nothing Then Return "IVALID: ID=" & Me.IDProdotto
                Return Me.Prodotto.Nome
            End Get
        End Property

        Public ReadOnly Property NomeProfilo As String
            Get
                If Me.Profilo Is Nothing Then Return "IVALID: ID=" & Me.IDProfilo
                Return Me.Profilo.Nome
            End Get
        End Property

        Public Overrides Function ToString() As String
            Select Case Me.Azione
                Case IncludeModes.Eredita : Return "Eredita " & Me.NomeProdotto & " (" & Me.Spread & ")"
                Case IncludeModes.Include : Return "Includi " & Me.NomeProdotto & " (" & Me.Spread & ")"
                Case IncludeModes.Escludi : Return "Escludi " & Me.NomeProdotto
                Case Else : Return "???"
            End Select
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PreventivatoriXProdotto"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_ProfiloID = reader.Read("Preventivatore", Me.m_ProfiloID)
            Me.m_ProdottoID = reader.Read("Prodotto", Me.m_ProdottoID)
            Me.m_Azione = reader.Read("Azione", Me.m_Azione)
            Me.m_Spread = reader.Read("Spread", Me.m_Spread)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Preventivatore", GetID(Me.m_Profilo, Me.m_ProfiloID))
            writer.Write("Prodotto", GetID(Me.m_Prodotto, Me.m_ProdottoID))
            writer.Write("Azione", Me.m_Azione)
            writer.Write("Spread", Me.m_Spread)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDProfilo", Me.IDProfilo)
            writer.WriteAttribute("IDProdotto", Me.IDProdotto)
            writer.WriteAttribute("Azione", Me.m_Azione)
            writer.WriteAttribute("Spread", Me.m_Spread)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDProfilo" : Me.m_ProfiloID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProdotto" : Me.m_ProdottoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Azione" : Me.m_Azione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Spread" : Me.m_Spread = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Dim p As CProfilo = Me.Profilo
            p.ProdottiXProfiloRelations.Update(Me)
        End Sub


    End Class


End Class
