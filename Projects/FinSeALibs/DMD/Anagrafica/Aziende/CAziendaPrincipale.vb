Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable>
    Public Class CAziendaPrincipale
        Inherits CAzienda

        Public Sub New()
        End Sub

        Public Sub New(ByVal src As CAzienda)
            Me.New()
            Me.SetID(GetID(src))
            Me.InitializeFrom(src)
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            'MyBase.OnCreate(e)
        End Sub

    End Class

    


End Class