Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable>
    Public Class CAziendaPrincipale
        Inherits CAzienda

        Public Sub New()
        End Sub

        Public Sub New(ByVal azienda As CAzienda)
            Me.New()
            If (azienda Is Nothing) Then Throw New ArgumentNullException("azienda")
            Me.SetID(GetID(azienda))
            Me.InitializeFrom(azienda)
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            'MyBase.OnCreate(e)
        End Sub

    End Class

    


End Class