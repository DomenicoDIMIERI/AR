Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class AlgorithmIdentifier

        Private _dERObjectIdentifier As DERObjectIdentifier
        Private _derobject As DERObject

        Sub New(dERObjectIdentifier As DERObjectIdentifier, derobject As DERObject)
            DMD.DMDObject.IncreaseCounter(Me)
            _dERObjectIdentifier = dERObjectIdentifier
            _derobject = derobject
        End Sub

        Function getObjectId() As Object
            Throw New NotImplementedException
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace