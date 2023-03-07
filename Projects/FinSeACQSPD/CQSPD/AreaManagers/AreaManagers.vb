Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

 
    Public NotInheritable Class CAreaManagersClass
        Inherits CGeneralClass(Of CAreaManager)

        Friend Sub New()
            MyBase.New("modAreaManager", GetType(CAreaManagerCursor), -1)
        End Sub
         

        Public Function GetItemByPersona(ByVal personID As Integer) As CAreaManager
            If (personID = 0) Then Return Nothing
            For Each item As CAreaManager In Me.LoadAll
                If (item.PersonaID = personID) Then Return item
            Next
            Return Nothing
        End Function
         
    End Class

    Private Shared m_AreaManagers As CAreaManagersClass = Nothing

    Public Shared ReadOnly Property AreaManagers As CAreaManagersClass
        Get
            If (m_AreaManagers Is Nothing) Then m_AreaManagers = New CAreaManagersClass
            Return m_AreaManagers
        End Get
    End Property


End Class
