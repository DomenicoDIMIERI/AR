Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    ''' <summary>
    ''' Cursore sulla tabella degli uffici
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CUfficiCursor
        Inherits DBObjectCursor(Of CUfficio)

        Private m_IDAzienda As New CCursorField(Of Integer)("IDAzienda")
        Private m_NomeAzienda As New CCursorFieldObj(Of String)("NomeAzienda")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")
        Private m_DataApertura As New CCursorField(Of Date)("DataApertura")
        Private m_DataChiusura As New CCursorField(Of Date)("DataChiusura")
        Private m_CodiceFiscale As New CCursorFieldObj(Of String)("CodiceFiscale")
        Private m_Flags As New CCursorField(Of UfficioFlags)("Flags")
        Private m_OnlyValid As Boolean = False

        Public Sub New()
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

        Public ReadOnly Property Flags As CCursorField(Of UfficioFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Attivo As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property

        Public ReadOnly Property DataApertura As CCursorField(Of Date)
            Get
                Return Me.m_DataApertura
            End Get
        End Property

        Public ReadOnly Property DataChiusura As CCursorField(Of Date)
            Get
                Return Me.m_DataChiusura
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property CodiceFiscale As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceFiscale
            End Get
        End Property

        Public ReadOnly Property IDAzienda As CCursorField(Of Integer)
            Get
                Return Me.m_IDAzienda
            End Get
        End Property

        Public ReadOnly Property NomeAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAzienda
            End Get
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("OnlyValid", Me.m_OnlyValid)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OnlyValid" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CUfficio
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_AziendaUffici"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Uffici.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataApertura] Is Null) Or ([DataApertura]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataChiusura] Is Null) Or ([DataChiusura]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([Attivo] Is Null) Or ([Attivo]=True))", " AND ")
            End If
            Return wherePart
        End Function

    End Class


End Class