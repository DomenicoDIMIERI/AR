Imports FinSeA

Namespace FinSeA

    Public Class Formats

        Public Shared Function ParsePhoneNumber(ByVal value As String) As String
            Return Trim(value)
        End Function

        Public Shared Function GetTimeStamp() As String
            Dim d As Date = Date.Now
            Return Right("0000" & d.Year, 4) & Right("00" & d.Month, 2) & Right("00" & d.Day, 2) & _
                   Right("00" & d.Hour, 2) & Right("00" & d.Minute, 2) & Right("00" & d.Second, 2) & _
                   Right("000" & d.Millisecond, 3)
        End Function

    End Class

End Namespace