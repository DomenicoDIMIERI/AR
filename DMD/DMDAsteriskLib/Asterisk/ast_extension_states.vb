Namespace Asterisk

    <Flags> _
    Public Enum ast_extension_states
        '''Extension removed 
        AST_EXTENSION_REMOVED = -2

        ''' <summary>
        ''' Extension hint removed 
        ''' </summary>
        ''' <remarks></remarks>
        AST_EXTENSION_DEACTIVATED = -1

        ''' <summary>
        ''' No device INUSE or BUSY
        ''' </summary>
        ''' <remarks></remarks>
        AST_EXTENSION_NOT_INUSE = 0

        ''' <summary>
        ''' One or more devices INUSE
        ''' </summary>
        ''' <remarks></remarks>
        AST_EXTENSION_INUSE = 1

        ''' <summary>
        ''' All devices BUSY
        ''' </summary>
        ''' <remarks></remarks>
        AST_EXTENSION_BUSY = 1 << 1

        ''' <summary>
        ''' All devices UNAVAILABLE/UNREGISTERED
        ''' </summary>
        ''' <remarks></remarks>
        AST_EXTENSION_UNAVAILABLE = 1 << 2

        ''' <summary>
        ''' All devices RINGING
        ''' </summary>
        ''' <remarks></remarks>
        AST_EXTENSION_RINGING = 1 << 3

        ''' <summary>
        ''' All devices ONHOLD
        ''' </summary>
        ''' <remarks></remarks>
        AST_EXTENSION_ONHOLD = 1 << 4
    End Enum

End Namespace