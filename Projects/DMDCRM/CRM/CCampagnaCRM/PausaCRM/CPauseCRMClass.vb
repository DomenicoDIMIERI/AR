Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CPauseCRMClass
        Inherits CGeneralClass(Of CSessioneCRM)


        Friend Sub New()
            MyBase.New("modCRMPause", GetType(CPausaCRMCursor), 0)
        End Sub

        Protected Overrides Function InitializeModule() As CModule
            Return MyBase.InitializeModule()
        End Function

    End Class

End Namespace

Partial Public Class CustomerCalls

    Private Shared m_PauseCRM As CPauseCRMClass = Nothing

    Public Shared ReadOnly Property PauseCRM As CPauseCRMClass
        Get
            If (m_PauseCRM Is Nothing) Then m_PauseCRM = New CPauseCRMClass
            Return m_PauseCRM
        End Get
    End Property


End Class