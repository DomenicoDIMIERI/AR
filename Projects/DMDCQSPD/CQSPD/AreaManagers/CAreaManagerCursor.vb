Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Class CAreaManagerCursor
        Inherits CTeamManagersCursor

        Public Sub New()
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.AreaManagers.Module 'modAreaManager
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_AreaManagers"
        End Function

        'Protected Overrides Function GetWherePartLimit() As String
        '    Dim wherePart As String = MyBase.GetWherePartLimit()
        '    Dim tmpSQL1 As String
        '    If Me.Module.UserCanDoAction("list_assigned") Then
        '        tmpSQL1 = "("
        '        tmpSQL1 = tmpSQL1 & "( ResidenteA_Provincia In ("
        '        tmpSQL1 = tmpSQL1 & "SELECT T.Provincia FROM ( "
        '        tmpSQL1 = tmpSQL1 & "SELECT T.Provincia, Max(T.CanList) AS MaxDiCanList "
        '        tmpSQL1 = tmpSQL1 & "FROM (SELECT Provincia, (Allow And Not Negate) As CanList FROM tbl_ProvXGroup WHERE (Allow<>Negate) AND Gruppo In (SELECT [Group] FROM tbl_UsersXGroup WHERE [User]=" & Users.CurrentUser.ID & ") "
        '        tmpSQL1 = tmpSQL1 & "UNION SELECT Provincia, (Allow And Not Negate) As CanList FROM tbl_ProvXUsers WHERE (Allow<>Negate) AND Utente=" & Users.CurrentUser.ID & " "
        '        tmpSQL1 = tmpSQL1 & ") AS T "
        '        tmpSQL1 = tmpSQL1 & "GROUP BY T.Provincia "
        '        tmpSQL1 = tmpSQL1 & ") WHERE MaxDiCanList <> 0 "
        '        tmpSQL1 = tmpSQL1 & "))"
        '        tmpSQL1 = tmpSQL1 & ")"
        '        wherePart = Strings.Combine(wherePart, tmpSQL1, " OR ")
        '    End If
        '    Return wherePart
        'End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CAreaManager
        End Function

    End Class

End Class
