Imports DMD
Imports DMD.Sistema
Imports DMD.Databases




Partial Public Class Anagrafica

  
 
    Public Class RicontattoEventArgs
        Inherits System.EventArgs

        Private m_Ricontatto As CRicontatto

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Ricontatto = Nothing
        End Sub

        Public Sub New(ByVal ricontatto As CRicontatto)
            Me.New
            If (ricontatto Is Nothing) Then Throw New ArgumentNullException("ricontatto")
            Me.m_Ricontatto = ricontatto
        End Sub

        Public ReadOnly Property Ricontatto As CRicontatto
            Get
                Return Me.m_Ricontatto
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class
     

End Class