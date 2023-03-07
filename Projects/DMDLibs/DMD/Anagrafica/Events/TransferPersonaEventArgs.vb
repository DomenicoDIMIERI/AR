Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases



Partial Public Class Anagrafica
    

  
    Public Class TransferPersonaEventArgs
        Inherits PersonaEventArgs

        Private m_Ufficio As CUfficio
        Private m_Messaggio As String


        Public Sub New(ByVal p As CPersona, ByVal ufficio As CUfficio, ByVal messaggio As String)
            MyBase.New(p)
            Me.m_Ufficio = ufficio
            Me.m_Messaggio = messaggio
        End Sub

        Public ReadOnly Property Ufficio As CUfficio
            Get
                Return Me.m_Ufficio
            End Get
        End Property

        Public ReadOnly Property Messaggio As String
            Get
                Return Me.m_Messaggio
            End Get
        End Property

    End Class



End Class