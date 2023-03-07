Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    ''' <summary>
    ''' Evento relativo ad un contatto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ContattoEventArgs
        Inherits System.EventArgs

        Private m_ContattoUtente As CContattoUtente

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal c As CContattoUtente)
            Me.New()
            If (c Is Nothing) Then Throw New ArgumentNullException("c")
            Me.m_ContattoUtente = c
        End Sub

        Public ReadOnly Property Contatto As CContattoUtente
            Get
                Return Me.m_ContattoUtente
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class