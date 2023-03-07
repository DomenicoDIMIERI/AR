Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica

Partial Public Class Databases


    Public MustInherit Class DBObjectCursorPO(Of T As DBObjectPO)
        Inherits DBObjectCursor(Of T)

        Private m_IDPuntoOperativo As New CCursorField(Of Integer)("IDPuntoOperativo")
        Private m_NomePuntoOperativo As New CCursorFieldObj(Of String)("NomePuntoOperativo")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDPuntoOperativo As CCursorField(Of Integer)
            Get
                Return Me.m_IDPuntoOperativo
            End Get
        End Property

        Public ReadOnly Property NomePuntoOperativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePuntoOperativo
            End Get
        End Property

        Public Overrides Function GetWherePartLimit() As String
            Dim tmpSQL As New System.Text.StringBuilder
            If Not Me.Module.UserCanDoAction("list") Then
                If Me.Module.UserCanDoAction("list_office") Then
                    Dim items As CCollection(Of CUfficio) = Users.CurrentUser.Uffici
                    tmpSQL.Append("[IDPuntoOperativo] In (0, NULL")
                    For i As Integer = 0 To items.Count - 1
                        tmpSQL.Append(",")
                        tmpSQL.Append(DBUtils.DBNumber(GetID(items(i))))
                    Next
                    tmpSQL.Append(")")
                End If
                If Me.Module.UserCanDoAction("list_own") Then
                    If tmpSQL.Length > 0 Then tmpSQL.Append(" OR ")
                    tmpSQL.Append("([CreatoDa]=" & GetID(Users.CurrentUser) & ")")
                End If
                If tmpSQL.Length = 0 Then tmpSQL.Append("(0<>0)")
            End If
            Return tmpSQL.ToString
        End Function

    End Class



End Class

