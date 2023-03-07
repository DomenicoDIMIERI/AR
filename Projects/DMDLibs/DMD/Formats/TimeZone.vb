''' <summary>
''' public abstract class TimeZone
''' extends Object
''' Implements Serializable, Cloneable
''' TimeZone represents a time zone offset, and also figures out daylight savings.
''' Typically, you get a TimeZone using getDefault which creates a TimeZone based on the time zone where the program is running. For example, for a program running in Japan, getDefault creates a TimeZone object based on Japanese Standard Time.
''' You can also get a TimeZone using getTimeZone along with a time zone ID. For instance, the time zone ID for the U.S. Pacific Time zone is "America/Los_Angeles". So, you can get a U.S. Pacific Time TimeZone object with:
'''      TimeZone tz = TimeZone.getTimeZone("America/Los_Angeles");
''' You can use the getAvailableIDs method to iterate through all the supported time zone IDs. You can then choose a supported ID to get a TimeZone. If the time zone you want is not represented by one of the supported IDs, then a custom time zone ID can be specified to produce a TimeZone. The syntax of a custom time zone ID is:
'''      CustomID:
'''             GMT Sign Hours : Minutes
'''              GMT Sign Hours Minutes
'''              GMT Sign Hours
'''      Sign: one of
'''              + -
'''      Hours:
'''              Digit
'''              Digit Digit
'''      Minutes:
'''              Digit Digit
'''      Digit: one of
'''              0 1 2 3 4 5 6 7 8 9
''' Hours must be between 0 to 23 and Minutes must be between 00 to 59. For example, "GMT+10" and "GMT+0010" mean ten hours and ten minutes ahead of GMT, respectively.
''' The format is locale independent and digits must be taken from the Basic Latin block of the Unicode standard. No daylight saving time transition schedule can be specified with a custom time zone ID. If the specified string doesn't match the syntax, "GMT" is used.
''' When creating a TimeZone, the specified custom time zone ID is normalized in the following syntax:
'''      NormalizedCustomID:
'''              GMT Sign TwoDigitHours : Minutes
'''      Sign: one of
'''              + -
'''      TwoDigitHours:
'''              Digit Digit
'''      Minutes:
'''              Digit Digit
'''      Digit: one of
'''              0 1 2 3 4 5 6 7 8 9
''' For example, TimeZone.getTimeZone("GMT-8").getID() returns "GMT-08:00". 
''' </summary>
''' <remarks></remarks>
Public MustInherit Class TimeZone
    Inherits System.TimeZone

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Overrides ReadOnly Property DaylightName As String
        Get
            Return vbNullString
        End Get
    End Property

    Public Overrides Function GetDaylightChanges(year As Integer) As Globalization.DaylightTime
        Return Nothing
    End Function

    Public Overrides Function GetUtcOffset(time As Date) As TimeSpan
        Return Nothing
    End Function

    Public Overrides ReadOnly Property StandardName As String
        Get
            Return vbNullString
        End Get
    End Property

    Shared Function getTimeZone(tzText As String) As SimpleTimeZone
        Throw New NotImplementedException
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class