Imports DMD.Sistema
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.XML.Utils


Namespace Serializable

    Public Class BinarySerializer
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace