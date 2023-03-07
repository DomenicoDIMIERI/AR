Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

     
    <Serializable> _
    Public Class CQSFilter
        Implements DMD.XML.IDMDXMLSerializable

        Private m_IDCliente As Integer
        Private m_Cliente As CPersona
        Private m_IDRichiesta As Integer
        Private m_Richiesta As CRichiestaFinanziamento
        Private m_Consulenza As CQSPDConsulenza
        Private m_IDConsulenza As Integer
        Private m_Consulente As CConsulentePratica
        Private m_IDConsulente As Integer
        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_IDRichiesta = 0
            Me.m_Richiesta = Nothing
            Me.m_TipoFonte = ""
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_Consulenza = Nothing
            Me.m_IDConsulenza = 0
            Me.m_Consulente = Nothing
            Me.m_IDConsulente = 0
        End Sub

        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
            End Set
        End Property

        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
            End Set
        End Property

        Public Property IDRichiesta As Integer
            Get
                Return GetID(Me.m_Richiesta, Me.m_IDRichiesta)
            End Get
            Set(value As Integer)
                Me.m_IDRichiesta = value
                Me.m_Richiesta = Nothing
            End Set
        End Property

        Public Property Richiesta As CRichiestaFinanziamento
            Get
                If (Me.m_Richiesta Is Nothing) Then Me.m_Richiesta = CQSPD.RichiesteFinanziamento.GetItemById(Me.m_IDRichiesta)
                Return Me.m_Richiesta
            End Get
            Set(value As CRichiestaFinanziamento)
                Me.m_Richiesta = value
                Me.m_IDRichiesta = GetID(value)
            End Set
        End Property

        Public Property IDConsulente As Integer
            Get
                Return GetID(Me.m_Consulente, Me.m_IDConsulente)
            End Get
            Set(value As Integer)
                Me.m_IDConsulente = value
                Me.m_Consulente = Nothing
            End Set
        End Property

        Public Property Consulente As CConsulentePratica
            Get
                If (Me.m_Consulente Is Nothing) Then Me.m_Consulente = CQSPD.Consulenti.GetItemById(Me.m_IDConsulente)
                Return Me.m_Consulente
            End Get
            Set(value As CConsulentePratica)
                Me.m_Consulente = value
                Me.m_IDConsulente = GetID(value)
            End Set
        End Property

        Public Property IDConsulenza As Integer
            Get
                Return GetID(Me.m_Consulenza, Me.m_IDConsulenza)
            End Get
            Set(value As Integer)
                Me.m_IDConsulenza = value
                Me.m_Consulenza = Nothing
            End Set
        End Property

        Public Property Consulenza As CQSPDConsulenza
            Get
                If (Me.m_Consulenza Is Nothing) Then Me.m_Consulenza = CQSPD.Consulenze.GetItemById(Me.m_IDConsulenza)
                Return Me.m_Consulenza
            End Get
            Set(value As CQSPDConsulenza)
                Me.m_Consulenza = value
                Me.m_IDConsulenza = GetID(value)
            End Set
        End Property

        Public Property TipoFonte As String
            Get
                Return Me.m_TipoFonte
            End Get
            Set(value As String)
                Me.m_TipoFonte = value
                Me.m_Fonte = Nothing
            End Set
        End Property

        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
            End Set
        End Property

        Public Property Fonte As IFonte
            Get
                If (Me.m_Fonte Is Nothing AndAlso Me.m_TipoFonte <> "") Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_TipoFonte, Me.m_TipoFonte, Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                Me.m_Fonte = value
                Me.m_IDFonte = GetID(value)
            End Set
        End Property

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRichiesta" : Me.m_IDRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDConsulenza" : Me.m_IDConsulenza = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDConsulente" : Me.m_IDConsulente = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("IDRichiesta", Me.IDRichiesta)
            writer.WriteAttribute("TipoFonte", Me.TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("IDConsulenza", Me.IDConsulenza)
            writer.WriteAttribute("IDConsulente", Me.IDConsulente)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Class
