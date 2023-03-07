Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD


    ''' <summary>
    ''' Cursore sulla tabella dei cessionari
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCessionariCursor
        Inherits DBObjectCursor(Of CCQSPDCessionarioClass)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_ImagePath As New CCursorFieldObj(Of String)("ImagePath") 'Percorso dell'immagine
        Private m_Visibile As New CCursorField(Of Boolean)("Visibile") 'Se vero il cessionario viene mostrato negli elenchi 
        Private m_Preventivatore As New CCursorFieldObj(Of String)("Preventivatore") 'Percorso dello script di backdoor alla pagina di preventivazione
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Flags As New CCursorField(Of CessionarioFlags)("Flags")
        Private m_OnlyValid As Boolean = False

        Public Sub New()
        End Sub

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Public ReadOnly Property Flags As CCursorField(Of CessionarioFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property ImagePath As CCursorFieldObj(Of String)
            Get
                Return Me.m_ImagePath
            End Get
        End Property

        Public ReadOnly Property Visibile As CCursorField(Of Boolean)
            Get
                Return Me.m_Visibile
            End Get
        End Property

        Public ReadOnly Property Preventivatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_Preventivatore
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

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCQSPDCessionarioClass
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Cessionari"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Cessionari.Module
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_OnlyValid", Me.m_OnlyValid)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case (fieldName)
                Case "m_OnlyValid" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class


End Class