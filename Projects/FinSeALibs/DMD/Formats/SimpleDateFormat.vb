Imports DMD.Text

Public Class SimpleDateFormat

    Private _p1 As String
    Private _fmt As String
    Private _locale As Locale

    Sub New(p1 As String)
        DMD.DMDObject.IncreaseCounter(Me)
        _p1 = p1
    End Sub

    Sub New(fmt As String, locale As Locale)
        DMD.DMDObject.IncreaseCounter(Me)
        _fmt = fmt
        _locale = locale
    End Sub

    Sub setTimeZone(simpleTimeZone As SimpleTimeZone)
        Throw New NotImplementedException
    End Sub

    Function format(p1 As Date) As String
        Throw New NotImplementedException
    End Function

    Sub setCalendar(retCal As NDate)
        Throw New NotImplementedException
    End Sub

    Function parse(text As String, where As ParsePosition) As Object
        Throw New NotImplementedException
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class