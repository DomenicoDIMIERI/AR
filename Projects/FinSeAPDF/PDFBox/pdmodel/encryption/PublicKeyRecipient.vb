Imports FinSeA.Security

Namespace org.apache.pdfbox.pdmodel.encryption


    '/**
    ' * Represents a recipient in the public key protection policy.
    ' *
    ' * @see PublicKeyProtectionPolicy
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' *
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PublicKeyRecipient

        Private x509 As X509Certificate

        Private permission As AccessPermission

        '/**
        ' * Returns the X509 certificate of the recipient.
        ' *
        ' * @return The X509 certificate
        ' */
        Public Function getX509() As X509Certificate
            Return x509
        End Function

        '/**
        ' * Set the X509 certificate of the recipient.
        ' *
        ' * @param aX509 The X509 certificate
        ' */
        Public Sub setX509(ByVal aX509 As X509Certificate)
            Me.x509 = aX509
        End Sub

        '/**
        ' * Returns the access permission granted to the recipient.
        ' *
        ' * @return The access permission object.
        ' */
        Public Function getPermission() As AccessPermission
            Return permission
        End Function

        '/**
        ' * Set the access permission granted to the recipient.
        ' *
        ' * @param permissions The permission to set.
        ' */
        Public Sub setPermission(ByVal permissions As AccessPermission)
            Me.permission = permissions
        End Sub
    End Class

End Namespace
