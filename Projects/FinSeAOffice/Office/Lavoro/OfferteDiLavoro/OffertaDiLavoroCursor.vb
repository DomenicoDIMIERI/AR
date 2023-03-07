Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella delle offerte di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OffertaDiLavoroCursor
        Inherits DBObjectCursorPO(Of OffertaDiLavoro)

        Private m_OnlyValid As Boolean
        Private m_DataInserzione As New CCursorField(Of Date)("DataInserzione")
        Private m_ValidaDal As New CCursorField(Of Date)("ValidaDal")
        Private m_ValidaAl As New CCursorField(Of Date)("ValidaAl")
        Private m_Attiva As New CCursorField(Of Boolean)("Attiva")
        Private m_NomeOfferta As New CCursorFieldObj(Of String)("NomeOfferta")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

        Public ReadOnly Property DataInserzione As CCursorField(Of Date)
            Get
                Return Me.m_DataInserzione
            End Get
        End Property

        Public ReadOnly Property ValidaDal As CCursorField(Of Date)
            Get
                Return Me.m_ValidaDal
            End Get
        End Property

        Public ReadOnly Property ValidaAl As CCursorField(Of Date)
            Get
                Return Me.m_ValidaAl
            End Get
        End Property

        Public ReadOnly Property Attiva As CCursorField(Of Boolean)
            Get
                Return Me.m_Attiva
            End Get
        End Property

        Public ReadOnly Property NomeOfferta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOfferta
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property



        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOfferteLavoro"
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



        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.OfferteDiLavoro.Module
        End Function
    End Class


End Class