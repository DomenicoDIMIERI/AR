Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class KeyTransRecipientInfo

        Private _recipId As RecipientIdentifier
        Private _algorithmidentifier As AlgorithmIdentifier
        Private _deroctetstring As DEROctetString

        Sub New(recipId As RecipientIdentifier, algorithmidentifier As AlgorithmIdentifier, deroctetstring As DEROctetString)
            ' TODO: Complete member initialization 
            _recipId = recipId
            _algorithmidentifier = algorithmidentifier
            _deroctetstring = deroctetstring
        End Sub


    End Class

End Namespace