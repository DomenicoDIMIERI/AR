Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
  
    Public Class CDistributoriCursor
        Inherits DBObjectCursor(Of CDistributore)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")
        Private m_OnlyValid As Boolean

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
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

        Public ReadOnly Property Attivo As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property

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

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Distributori.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Distributori"
        End Function

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

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If (Me.OnlyValid) Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

    End Class


End Class