Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public Class CListaRicontattiCursor
        Inherits DBObjectCursorPO(Of CListaRicontatti)

        Private m_Name As New CCursorFieldObj(Of String)("Name")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_DataInserimento As New CCursorField(Of Date)("DataInserimento")
        Private m_IDProprietario As New CCursorField(Of Integer)("IDProprietario")
        Private m_NomeProprietario As New CCursorFieldObj(Of String)("NomeProprietario")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDProprietario As CCursorField(Of Integer)
            Get
                Return Me.m_IDProprietario
            End Get
        End Property

        Public ReadOnly Property NomeProprietario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProprietario
            End Get
        End Property


        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property DataInserimento As CCursorField(Of Date)
            Get
                Return Me.m_DataInserimento
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_ListeRicontatto"
        End Function


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Anagrafica.ListeRicontatto.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return ListeRicontatto.Module
        End Function

        Public Overrides Function GetWherePartLimit() As String
            Dim tmpSQL As String
            tmpSQL = ""
            If Not Me.Module.UserCanDoAction("list") Then
                tmpSQL = ""
                If Me.Module.UserCanDoAction("list_office") Then
                    'Dim array As New System.Text.StringBuilder
                    'array.Append("0")
                    'For Each ufficio As CUfficio In Users.CurrentUser.Uffici
                    '    array.Append(",")
                    '    array.Append(CStr(GetID(ufficio)))
                    'Next
                    'If (array.Length > 0) Then
                    '    tmpSQL = Strings.Combine(tmpSQL, "([IDPuntoOperativo] In (" & array.ToString() & "))", " OR ")
                    'End If
                    tmpSQL = "[IDPuntoOperativo] = 0"
                    Dim items As CCollection(Of CUfficio) = Users.CurrentUser.Uffici
                    For i As Integer = 0 To items.Count - 1
                        tmpSQL = Strings.Combine(tmpSQL, "[IDPuntoOperativo] = " & DBUtils.DBNumber(GetID(items(i))), " OR ")
                    Next
                    '    array.Append(",")
                    '    array.Append(CStr(GetID(ufficio)))
                    'Next

                End If
                If Me.Module.UserCanDoAction("list_own") Then
                    tmpSQL = Strings.Combine(tmpSQL, "([IDProprietario]=" & GetID(Users.CurrentUser) & ")", " OR ")
                End If
                If tmpSQL = "" Then tmpSQL = "(0<>0)"
            End If
            Return tmpSQL
        End Function
    End Class


End Class