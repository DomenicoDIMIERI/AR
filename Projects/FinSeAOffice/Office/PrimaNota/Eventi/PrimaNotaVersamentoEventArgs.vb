Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office

 
        Public Class PrimaNotaVersamentoEventArgs
            Inherits System.EventArgs

            Private m_Registrazione

        Public Sub New(ByVal registrazione As RigaPrimaNota)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Registrazione = registrazione
        End Sub

        Public ReadOnly Property Registrazione As RigaPrimaNota
                Get
                    Return Me.m_Registrazione
                End Get
            End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class