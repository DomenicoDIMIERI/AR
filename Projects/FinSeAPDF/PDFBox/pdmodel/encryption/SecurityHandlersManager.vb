Imports FinSeA.Security ' org.bouncycastle.jce.provider.BouncyCastleProvider;

Namespace org.apache.pdfbox.pdmodel.encryption


    '/**
    ' * This class manages security handlers for the application. It follows the singleton pattern.
    ' * To be usable, security managers must be registered in it. Security managers are retrieved by
    ' * the application when necessary.
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' *
    ' * @version $Revision: 1.3 $
    ' *
    ' */
    Public Class SecurityHandlersManager


        ''' <summary>
        ''' The unique instance of this manager.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared instance As SecurityHandlersManager

        '/**
        ' * hashtable used to index handlers regarding their name.
        ' * Basically this will be used when opening an encrypted
        ' * document to find the appropriate security handler to handle
        ' * security features of the document.
        ' */
        Private handlerNames As Hashtable = Nothing

        '/**
        ' * Hashtable used to index handlers regarding the class of
        ' * protection policy they use.  Basically this will be used when
        ' * encrypting a document.
        ' */
        Private handlerPolicyClasses As Hashtable = Nothing

        '/**
        ' * private constructor.
        ' */
        Private Sub New()
            handlerNames = New Hashtable()
            handlerPolicyClasses = New Hashtable()
            Try
                Me.registerHandler(StandardSecurityHandler.FILTER, GetType(StandardSecurityHandler), GetType(StandardProtectionPolicy))
                Me.registerHandler(PublicKeySecurityHandler.FILTER, GetType(PublicKeySecurityHandler), GetType(PublicKeyProtectionPolicy))
            Catch e As Exception
                Debug.Print("SecurityHandlersManager strange error with builtin handlers: " & e.Message)
                Stop
            End Try
        End Sub

        '/**
        ' * register a security handler.
        ' *
        ' * If the security handler was already registered an exception is thrown.
        ' * If another handler was previously registered for the same filter name or
        ' * for the same policy name, an exception is thrown
        ' *
        ' * @param filterName The name of the filter.
        ' * @param securityHandlerClass Security Handler class to register.
        ' * @param protectionPolicyClass Protection Policy class to register.
        ' *
        ' * @throws BadSecurityHandlerException If there is an error registering the security handler.
        ' */
        Public Sub registerHandler(ByVal filterName As String, ByVal securityHandlerClass As System.Type, ByVal protectionPolicyClass As System.Type) 'throws BadSecurityHandlerException
            If (handlerNames.contains(securityHandlerClass) OrElse handlerPolicyClasses.contains(securityHandlerClass)) Then
                Throw New BadSecurityHandlerException("the following security handler was already registered: " & securityHandlerClass.Name)
            End If

            If (GetType(SecurityHandler).IsAssignableFrom(securityHandlerClass)) Then
                Try
                    If (handlerNames.containsKey(filterName)) Then
                        Throw New BadSecurityHandlerException("a security handler was already registered for the filter name " & filterName)
                    End If
                    If (handlerPolicyClasses.containsKey(protectionPolicyClass)) Then
                        Throw New BadSecurityHandlerException("a security handler was already registered for the policy class " & protectionPolicyClass.Name)
                    End If

                    handlerNames.put(filterName, securityHandlerClass)
                    handlerPolicyClasses.put(protectionPolicyClass, securityHandlerClass)
                Catch e As Exception
                    Throw New BadSecurityHandlerException(e)
                End Try
            Else
                Throw New BadSecurityHandlerException("The class is not a super class of SecurityHandler")
            End If
        End Sub


        '/**
        ' * Get the singleton instance.
        ' *
        ' * @return The SecurityHandlersManager.
        ' */
        Public Shared Function getInstance() As SecurityHandlersManager
            If (instance Is Nothing) Then
                instance = New SecurityHandlersManager()
                Security.Providers.addProvider(New BouncyCastleProvider())
            End If
            Return instance
        End Function

        '/**
        ' * Get the security handler for the protection policy.
        ' *
        ' * @param policy The policy to get the security handler for.
        ' *
        ' * @return The appropriate security handler.
        ' *
        ' * @throws BadSecurityHandlerException If it is unable to create a SecurityHandler.
        ' */
        Public Function getSecurityHandler(ByVal policy As ProtectionPolicy) As SecurityHandler 'throws BadSecurityHandlerException
            Dim found As Object = handlerPolicyClasses.get(policy.GetType())
            If (found Is Nothing) Then
                Throw New BadSecurityHandlerException("Cannot find an appropriate security handler for " & policy.GetType().Name)
            End If
            Dim handlerclass As System.Type = found
            Dim argsClasses() As System.Type = {policy.GetType()}
            Dim args() As Object = {policy}
            Try
                'Constructor c = handlerclass.getDeclaredConstructor(argsClasses);
                Dim handler As SecurityHandler = Activator.CreateInstance(handlerclass, args) ' '(SecurityHandler)c.newInstance(args);
                Return handler
            Catch e As Exception
                Debug.Print(e.ToString)
                Throw New BadSecurityHandlerException("problem while trying to instanciate the security handler " & handlerclass.Name & ": " & e.Message)
            End Try
        End Function



        '/**
        ' * Retrieve the appropriate SecurityHandler for a the given filter name.
        ' * The filter name is an entry of the encryption dictionary of an encrypted document.
        ' *
        ' * @param filterName The filter name.
        ' *
        ' * @return The appropriate SecurityHandler if it exists.
        ' *
        ' * @throws BadSecurityHandlerException If the security handler does not exist.
        ' */
        Public Function getSecurityHandler(ByVal filterName As String) As SecurityHandler 'throws BadSecurityHandlerException
            Dim found As Object = handlerNames.get(filterName)
            If (found Is Nothing) Then
                Throw New BadSecurityHandlerException("Cannot find an appropriate security handler for " & filterName)
            End If
            Dim handlerclass As System.Type = found
            Dim argsClasses As System.Type() = {}
            Dim args() As Object = {}
            Try
                'Constructor c = handlerclass.getDeclaredConstructor(argsClasses);
                Dim handler As SecurityHandler = Activator.CreateInstance(handlerclass, args) '(SecurityHandler)c.newInstance(args);
                Return handler
            Catch e As Exception
                Debug.Print(e.ToString)
                Throw New BadSecurityHandlerException("problem while trying to instanciate the security handler " & handlerclass.Name & ": " & e.Message)
            End Try
        End Function


    End Class

End Namespace
