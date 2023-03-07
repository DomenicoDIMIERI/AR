Namespace CallManagers

    <Flags> _
    Public Enum DebugFlags
        Off = 0
        system = 1
        [call] = 2
        log = 4
        verbose = 8
        command = 16
        agent = 32
        user = 64
        reporting = 128
        [On] = 1024
    End Enum

End Namespace