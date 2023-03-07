Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD


    ''' <summary>
    ''' Cursore sulla tabella delle spese
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTabellaSpeseCursor
        Inherits DBObjectCursor(Of CTabellaSpese)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_CessionarioID As New CCursorField(Of Integer)("Cessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Running As New CCursorField(Of Double)("Running")
        Private m_Visible As New CCursorField(Of Boolean)("Visible")
        Private m_Flags As New CCursorField(Of TabellaSpeseFlags)("Flags")
        Private m_OnlyValid As Boolean

        Public Sub New()
          
            Me.m_OnlyValid = False
        End Sub

        Public ReadOnly Property Flags As CCursorField(Of TabellaSpeseFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Visible As CCursorField(Of Boolean)
            Get
                Return Me.m_Visible
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property CessionarioID As CCursorField(Of Integer)
            Get
                Return Me.m_CessionarioID
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
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

        Public ReadOnly Property Running As CCursorField(Of Double)
            Get
                Return Me.m_Running
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CTabellaSpese
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TabellaSpese"
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart
            If m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([Flags] AND 1) = 1)", " AND ")
            End If
            Return wherePart
        End Function


        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.TabelleSpese.Module
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
