Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Anagrafica

 
    Public Class CBancheCursor
        Inherits DBObjectCursorPO(Of CBanca)

        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Filiale As New CCursorFieldObj(Of String)("Filiale")
        Private m_Indirizzo_Citta As New CCursorFieldObj(Of String)("Indirizzo_Citta")
        Private m_Indirizzo_Provincia As New CCursorFieldObj(Of String)("Indirizzo_Provincia")
        Private m_Indirizzo_Via As New CCursorFieldObj(Of String)("Indirizzo_Via")
        Private m_Indirizzo_CAP As New CCursorFieldObj(Of String)("Indirizzo_CAP")
        Private m_ABI As New CCursorFieldObj(Of String)("ABI")
        Private m_CAB As New CCursorFieldObj(Of String)("CAB")
        Private m_SWIFT As New CCursorFieldObj(Of String)("SWIFT")
        Private m_DataApertura As New CCursorField(Of Date)("DataApertura")
        Private m_DataChiusura As New CCursorField(Of Date)("DataChiusura")
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")
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

        Public ReadOnly Property Attiva As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Citta
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Provincia
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Via
            End Get
        End Property

        Public ReadOnly Property Indirizzo_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_CAP
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Filiale As CCursorFieldObj(Of String)
            Get
                Return Me.m_Filiale
            End Get
        End Property

        'Public ReadOnly Property Indirizzo As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Indirizzo
        '    End Get
        'End Property

        Public ReadOnly Property ABI As CCursorFieldObj(Of String)
            Get
                Return Me.m_ABI
            End Get
        End Property

        Public ReadOnly Property CAB As CCursorFieldObj(Of String)
            Get
                Return Me.m_CAB
            End Get
        End Property

        Public ReadOnly Property SWIFT As CCursorFieldObj(Of String)
            Get
                Return Me.m_SWIFT
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Banche.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Banche"
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
                wherePart = Strings.Combine(wherePart, "(([DataApertura] Is Null) Or ([DataApertura]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataChiusura] Is Null) Or ([DataChiusura]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

    End Class


End Class