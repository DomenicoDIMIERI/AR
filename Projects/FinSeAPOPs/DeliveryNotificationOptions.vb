Namespace Net.Mail

    ''' <summary>
    ''' Describes the delivery notification options for e-mail.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DeliveryNotificationOptions As Integer
        ''' <summary>
        ''' No notification.
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' Notify if the delivery is successful.
        ''' </summary>
        ''' <remarks></remarks>
        OnSuccess = 1

        ''' <summary>
        ''' Notify if the delivery is unsuccessful.
        ''' </summary>
        ''' <remarks></remarks>
        OnFailure = 2

        ''' <summary>
        ''' Notify if the delivery is delayed
        ''' </summary>
        ''' <remarks></remarks>
        Delay = 4

        ''' <summary>
        ''' Never notify.
        ''' </summary>
        ''' <remarks></remarks>
        Never = 134217728
    End Enum

End Namespace