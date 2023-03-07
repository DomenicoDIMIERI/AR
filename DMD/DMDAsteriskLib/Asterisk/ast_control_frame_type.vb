Namespace Asterisk

    <Flags> _
    Public Enum ast_control_frame_type
        OTHER_END_HAS_HANGUP = 1
        LOCAL_RING = 2
        REMOTE_END_IS_RINGING = 3
        REMOTE_END_HAS_ANSWERED = 4
        REMOTE_END_IS_BUSY = 5
        MAKE_IT_GO_OFF_HOOK = 6
        LINE_IS_OFF_HOOK = 7
        CONGESTION = 8 'circuits busy
    End Enum

End Namespace