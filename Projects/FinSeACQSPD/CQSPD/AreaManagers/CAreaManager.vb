Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Serializable> _
    Public Class CAreaManager
        Inherits CTeamManager

        <NonSerialized> _
        Private m_Pratiche As CAMPraticheCollection
        <NonSerialized> _
        Private m_TeamManagers As CAMTeamManagersCollection

        Public Sub New()
            Me.m_Pratiche = Nothing
            Me.m_TeamManagers = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.AreaManagers.Module
        End Function

        Public ReadOnly Property Pratiche As CAMPraticheCollection
            Get
                If (Me.m_Pratiche Is Nothing) Then
                    Me.m_Pratiche = New CAMPraticheCollection
                    Me.m_Pratiche.Initialize(Me)
                End If
                Return Me.m_Pratiche
            End Get
        End Property

        Public ReadOnly Property TeamManagers As CAMTeamManagersCollection
            Get
                If (Me.m_TeamManagers Is Nothing) Then
                    Me.m_TeamManagers = New CAMTeamManagersCollection
                    Me.m_TeamManagers.Initialize(Me)
                End If
                Return Me.m_TeamManagers
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_AreaManagers"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.AreaManagers.UpdateCached(Me)
        End Sub

    End Class

End Class
