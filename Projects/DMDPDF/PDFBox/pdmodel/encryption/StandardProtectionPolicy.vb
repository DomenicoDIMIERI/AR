Namespace org.apache.pdfbox.pdmodel.encryption

    '/**
    ' * This class represents the protection policy to add to a document
    ' * for password-based protection.
    ' *
    ' * The following example shows how to protect a PDF document with password.
    ' * In this example, the document will be protected so that someone opening
    ' * the document with the user password <code>user_pwd</code> will not be
    ' * able to modify the document.
    ' *
    ' * <pre>
    ' * AccessPermission ap = new AccessPermission();
    ' * ap.setCanModify(false);
    ' * StandardProtectionPolicy policy = new StandardProtectionPolicy(owner_pwd, user_pwd, ap);
    ' * doc.protect(policy);
    ' * </pre>
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' * @version $Revision: 1.3 $
    ' */
    Public Class StandardProtectionPolicy
        Inherits ProtectionPolicy

        Private permissions As AccessPermission

        Private ownerPassword As String = ""

        Private userPassword As String = ""


        '/**
        ' * Creates an new instance of the standard protection policy
        ' * in order to protect a PDF document with passwords.
        ' *
        ' * @param ownerPass The owner's password.
        ' * @param userPass The users's password.
        ' * @param perms The access permissions given to the user.
        ' */
        Public Sub New(ByVal ownerPass As String, ByVal userPass As String, ByVal perms As AccessPermission)
            Me.permissions = perms
            Me.userPassword = userPass
            Me.ownerPassword = ownerPass
        End Sub

        '/**
        ' * Getter of the property <tt>permissions</tt>.
        ' *
        ' * @return Returns the permissions.
        ' */
        Public Function getPermissions() As AccessPermission
            Return permissions
        End Function

        '/**
        ' * Setter of the property <tt>permissions</tt>.
        ' *
        ' * @param perms The permissions to set.
        ' */
        Public Sub setPermissions(ByVal perms As AccessPermission)
            Me.permissions = perms
        End Sub

        '/**
        ' * Getter of the property <tt>ownerPassword</tt>.
        ' *
        ' * @return Returns the ownerPassword.
        ' */
        Public Function getOwnerPassword() As String
            Return ownerPassword
        End Function

        '/**
        ' * Setter of the property <tt>ownerPassword</tt>.
        ' *
        ' * @param ownerPass The ownerPassword to set.
        ' */
        Public Sub setOwnerPassword(ByVal ownerPass As String)
            Me.ownerPassword = ownerPass
        End Sub

        '/**
        ' * Getter of the property <tt>userPassword</tt>.
        ' *
        ' * @return Returns the userPassword.
        ' */
        Public Function getUserPassword() As String
            Return userPassword
        End Function

        '/**
        ' * Setter of the property <tt>userPassword</tt>.
        ' *
        ' * @param userPass The userPassword to set.
        ' */
        Public Sub setUserPassword(ByVal userPass As String)
            Me.userPassword = userPass
        End Sub

    End Class

End Namespace
