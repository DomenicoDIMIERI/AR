Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
    

    Public Class PersonaEventArgs
        Inherits System.EventArgs

        Private m_Persona As CPersona

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal persona As CPersona)
            Me.New()
            Me.m_Persona = persona
        End Sub

        Public ReadOnly Property Persona As CPersona
            Get
                Return Me.m_Persona
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class
 

End Class