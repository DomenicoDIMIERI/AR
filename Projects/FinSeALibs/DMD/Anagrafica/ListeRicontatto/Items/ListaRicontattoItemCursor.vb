Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

 
  
    Public Class ListaRicontattoItemCursor
        Inherits CRicontattiCursor

        Private m_NomeLista As New CCursorFieldObj(Of String)("NomeLista")

        Public ReadOnly Property NomeLista As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeLista
            End Get
        End Property



        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New ListaRicontattoItem
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ListeRicontattoItems"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.ListeRicontatto.Items.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Anagrafica.ListeRicontatto.Database
        End Function

        'Public Overrides Function GetWherePartLimit() As String
        '    Dim tmpSQL As String = MyBase.GetWherePartLimit()
        '    If Not Me.Module.UserCanDoAction("list") Then
        '        If Me.Module.UserCanDoAction("list_own") Then
        '            tmpSQL = Strings.Combine(tmpSQL, "([IDAssegnatoA]=" & GetID(Users.CurrentUser) & ")", " OR ")
        '        End If
        '    End If
        '    Return tmpSQL
        'End Function


    End Class

   

End Class