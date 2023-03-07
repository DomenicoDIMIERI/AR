Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office

    Public NotInheritable Class CommissioniPerUscite
        Private Shared m_Module As CModule = Nothing

        Private Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Shared ReadOnly Property [Module] As CModule
            Get
                If (m_Module) Is Nothing Then m_Module = Sistema.Modules.GetItemByName("modOfficeCommissXUscita")
                Return m_Module
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Shared Function GetItemById(ByVal id As Integer) As CommissionePerUscita
            If (id = 0) Then Return Nothing
            Dim cursor As New CommissioniPerUscitaCursor
            cursor.PageSize = 1
            cursor.IgnoreRights = True
            cursor.ID.Value = id
            Dim ret As CommissionePerUscita = cursor.Item
            cursor.Dispose()
            Return ret
        End Function

    End Class


End Class