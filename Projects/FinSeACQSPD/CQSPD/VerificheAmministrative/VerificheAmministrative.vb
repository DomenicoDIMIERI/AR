Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals


    Public NotInheritable Class VerificheAmministrativeClass
        Inherits CGeneralClass(Of VerificaAmministrativa)

        Friend Sub New()
            MyBase.New("modCQSPDVerificheAmministrative", GetType(VerificheAmministrativeCursor))
        End Sub

        Protected Overrides Function InitializeModule() As CModule
            Dim m As CModule = MyBase.InitializeModule()
            m.DisplayName = "Verifiche Amministrative"
            m.ClassHandler = "VerificheAmministrativeHandler"
            m.Save()
            Return m
        End Function
      
    End Class

End Namespace


Partial Public Class CQSPD

    Private Shared m_VerificheAmministrative As VerificheAmministrativeClass = Nothing

    Public Shared ReadOnly Property VerificheAmministrative As VerificheAmministrativeClass
        Get
            If (m_VerificheAmministrative Is Nothing) Then m_VerificheAmministrative = New VerificheAmministrativeClass
            Return m_VerificheAmministrative
        End Get
    End Property


End Class
