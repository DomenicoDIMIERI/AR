Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

  
    Public Class CAMPraticheCollection
        Inherits CPraticheCollection

        Private m_AreaManager As CAreaManager

        Public Sub New()
            Me.m_AreaManager = Nothing
        End Sub

        Protected Friend Function Initialize(ByVal value As CAreaManager) As Boolean
            MyBase.Clear()
            Me.m_AreaManager = value
            For i As Integer = 0 To Me.m_AreaManager.TeamManagers.Count - 1
                Dim tm As CTeamManager = Me.m_AreaManager.TeamManagers(i)
                Dim pratiche As New CPraticheCollection(tm)
                For j As Integer = 0 To pratiche.Count - 1
                    MyBase.Add(pratiche(j))
                Next
            Next
            Return True
        End Function

    End Class


End Class
