Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabelle delle convenzioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CQSPDConvenzioniCursor
        Inherits DBObjectCursor


        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_IDProdotto As New CCursorField(Of Integer)("IDProdotto")
        Private m_NomeProdotto As New CCursorFieldObj(Of String)("NomeProdotto")
        Private m_Attiva As New CCursorField(Of Boolean)("Attiva")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_MinimoCaricabile As New CCursorField(Of Double)("MinimoCaricabile")
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                If (Me.m_OnlyValid = value) Then Exit Property
                Me.m_OnlyValid = value
                Me.Reset1()
            End Set
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property IDProdotto As CCursorField(Of Integer)
            Get
                Return Me.m_IDProdotto
            End Get
        End Property

        Public ReadOnly Property NomeProdotto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProdotto
            End Get
        End Property

        Public ReadOnly Property Attiva As CCursorField(Of Boolean)
            Get
                Return Me.m_Attiva
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
            Return Convenzioni.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDConvenzioni"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("OnlyValid", Me.m_OnlyValid)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OnlyValid" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CQSPDConvenzione
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "([Attiva]=True)", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

    End Class


End Class
