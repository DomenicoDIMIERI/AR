Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
 
    ''' <summary>
    ''' Cursore sulla tabella delle fonti
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CFontiCursor
        Inherits DBObjectCursor(Of CFonte)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Attiva As New CCursorField(Of Boolean)("Attiva")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_IDCampagna As New CCursorFieldObj(Of String)("IDCampagna")
        Private m_IDAnnuncio As New CCursorFieldObj(Of String)("IDAnnuncio")
        Private m_IDKeyWord As New CCursorFieldObj(Of String)("IDKeyWord")
        Private m_Siti As New CCursorFieldObj(Of String)("Siti")

        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
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

        Public ReadOnly Property Attiva As CCursorField(Of Boolean)
            Get
                Return Me.m_Attiva
            End Get
        End Property

        Public ReadOnly Property IDCampagna As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDCampagna
            End Get
        End Property

        Public ReadOnly Property IDAnnuncio As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDAnnuncio
            End Get
        End Property

        Public ReadOnly Property IDKeyWord As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDKeyWord
            End Get
        End Property

        Public ReadOnly Property Siti As CCursorFieldObj(Of String)
            Get
                Return Me.m_Siti
            End Get
        End Property

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If (Me.OnlyValid) Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Fonti.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FontiContatto"
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
    End Class


End Class