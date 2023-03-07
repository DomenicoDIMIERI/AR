Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
    

  
    Public Class MergePersonaEventArgs
        Inherits PersonaEventArgs


        Private m_Mi As CMergePersona


        Public Sub New(ByVal mi As CMergePersona)
            MyBase.New(mi.Persona1)
            Me.m_Mi = mi
        End Sub

        Public ReadOnly Property MI As CMergePersona
            Get
                Return Me.m_Mi
            End Get
        End Property
    End Class



End Class