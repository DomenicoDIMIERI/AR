'Autore: Domenico DI MIERI <domenico.dimieri@hotmail.it>
'Data: 22/01/2018
'Copyright: DMD All rights reserved
'Note:	Metodi di utilità per lavorare con date e calendario
'Modifiche: 22/01/2018 Corretta la funzione GetWeekDay (restituiva Strings.ToPropercase del vaore invece che il valore integer direttamente)


Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Internals



Namespace Internals


    Public NotInheritable Class CCalendarClass


        Private m_Module As CModule
        Private m_FirstDayOfWeek As Microsoft.VisualBasic.FirstDayOfWeek = FirstDayOfWeek.Monday
        Private m_Providers As CCollection(Of ICalendarProvider)
        Private m_DefaultComparer As IComparer
        Private m_GiorniFestivi As CKeyCollection(Of Date)

        Public GiorniFeriali As CKeyCollection(Of Integer)

        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_FirstDayOfWeek = 0
            Me.m_Providers = Nothing
            Me.m_DefaultComparer = Nothing
            Me.m_GiorniFestivi = New CKeyCollection(Of Date)
            Me.m_GiorniFestivi.Add("Capodanno", #1/1/1900#)
            Me.m_GiorniFestivi.Add("Befana", #1/6/1900#)
            Me.m_GiorniFestivi.Add("Festa della liberazione", #4/25/1900#)
            Me.m_GiorniFestivi.Add("Festa del lavoro", #5/1/1900#)
            Me.m_GiorniFestivi.Add("Festa della Repubblica", #6/2/1900#)
            Me.m_GiorniFestivi.Add("Ognissanti", #11/1/1900#)
            Me.m_GiorniFestivi.Add("Ferragosto", #8/15/1900#)
            Me.m_GiorniFestivi.Add("Festa dell'Immacolata", #12/8/1900#)
            Me.m_GiorniFestivi.Add("Natale", #12/25/1900#)
            Me.m_GiorniFestivi.Add("Santo Stefano", #12/26/1900#)
            Me.GiorniFeriali = New CKeyCollection(Of Integer)
            Me.GiorniFeriali.Add("Lunedì", 1)
            Me.GiorniFeriali.Add("Mertedì", 2)
            Me.GiorniFeriali.Add("Mercoledì", 3)
            Me.GiorniFeriali.Add("Giovedì", 4)
            Me.GiorniFeriali.Add("Venerdì", 5)
            Me.GiorniFeriali.Add("Sabato", 6)

            Me.AddProvider(New CCalendarActivitiesProvider)
        End Sub



        ''' <summary>
        ''' Restituisce vero se la data corrisponde ad un giorno feriale
        ''' </summary>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsFeriale(ByVal d As Date) As Boolean
            Return Not IsFestivo(d) And Me.GiorniFeriali.Contains(GetWeekDay(d))
        End Function

        ''' <summary>
        ''' Restituisce vero se la data corrisponde ad un giorno festivo
        ''' </summary>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsFestivo(ByVal d As Date) As Boolean
            For Each d1 As Date In Me.m_GiorniFestivi
                If d1.Day = d.Day AndAlso d1.Month = d.Month Then Return True
            Next
            Dim p As Date = GetEsterDate(d.Year)
            If (p.Day = d.Day And p.Month = d.Month) Then Return True
            p = DateAdd(DateInterval.Day, 1, p)
            If (p.Day = d.Day And p.Month = d.Month) Then Return True
            Return False
        End Function

        Public Function IsLeapYear(ByVal year As Integer) As Boolean
            Return (((year Mod 4 = 0) AndAlso (year Mod 100 <> 0)) OrElse (year Mod 400 = 0))
        End Function

        Public Function GetDaysInMonth(ByVal year As Integer, ByVal month As Integer) As Integer
            Dim items() As Integer = {31, IIf(IsLeapYear(year), 29, 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
            Return items(month - 1)
        End Function

        Public Function GetDaysInMonth(ByVal [date] As Date) As Integer
            Return GetDaysInMonth([date].Year, [date].Month)
        End Function


        ''' <summary>
        ''' Restituisce la collezione delle festività predefinite per l'anno
        ''' </summary>
        ''' <param name="anno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetGiorniFestivi(ByVal anno As Integer) As CKeyCollection(Of Date)
            Dim ret As New CKeyCollection(Of Date)
            Dim d As Date
            For Each k As String In m_GiorniFestivi.Keys
                d = m_GiorniFestivi(k)
                ret.Add(k, New Date(anno, d.Month, d.Day))
            Next
            d = GetEsterDate(anno)
            ret.Add("Pasqua", d)
            ret.Add("Lunedì dell'angelo", DateAdd(DateInterval.Day, 1, d))
            ret.Sort()
            Return ret
        End Function

        Public Function GetEsterDate(ByVal year As Integer) As Date
            If (year < 1583 Or year > 4099) Then Throw New ArgumentOutOfRangeException("Year deve essere compreso tra 1583 e 4099")
            Dim d, m As Integer
            EasterDate(d, m, year)
            Return New Date(year, m, d)
        End Function

        'EASTER DATE CALCULATION FOR YEARS 1583 TO 4099
        Private Sub EasterDate(ByRef d As Integer, ByRef m As Integer, ByVal y As Integer)
            ' EASTER DATE CALCULATION FOR YEARS 1583 TO 4099

            ' y is a 4 digit year 1583 to 4099
            ' d returns the day of the month of Easter
            ' m returns the month of Easter

            ' Easter Sunday is the Sunday following the Paschal Full Moon
            ' (PFM) date for the year

            ' This algorithm is an arithmetic interpretation of the 3 step
            ' Easter Dating Method developed by Ron Mallen 1985, as a vast
            ' improvement on the method described in the Common Prayer Book

            ' Because this algorithm is a direct translation of the
            ' official tables, it can be easily proved to be 100% correct

            ' This algorithm derives values by sequential inter-dependent
            ' calculations, so ... DO NOT MODIFY THE ORDER OF CALCULATIONS!

            ' The \ operator may be unfamiliar - it means integer division
            ' for example, 30 \ 7 = 4 (the remainder is ignored)

            ' All variables are integer data types

            ' It's free!  Please do not modify code or comments!
            ' ==========================================================
            Dim FirstDig, Remain19, temp As Integer   'intermediate results
            Dim tA, tB, tC, tD, tE As Integer 'table A to E results

            FirstDig = y \ 100              'first 2 digits of year
            Remain19 = y Mod 19             'remainder of year / 19

            ' calculate PFM date
            temp = (FirstDig - 15) \ 2 + 202 - 11 * Remain19

            Select Case FirstDig
                Case 21, 24, 25, 27 To 32, 34, 35, 38
                    temp = temp - 1
                Case 33, 36, 37, 39, 40
                    temp = temp - 2
            End Select
            temp = temp Mod 30

            tA = temp + 21
            If temp = 29 Then tA = tA - 1
            If (temp = 28 And Remain19 > 10) Then tA = tA - 1

            'find the next Sunday
            tB = (tA - 19) Mod 7

            tC = (40 - FirstDig) Mod 4
            If tC = 3 Then tC = tC + 1
            If tC > 1 Then tC = tC + 1

            temp = y Mod 100
            tD = (temp + temp \ 4) Mod 7

            tE = ((20 - tB - tC - tD) Mod 7) + 1
            d = tA + tE

            'return the date
            If d > 31 Then
                d = d - 31
                m = 4
            Else
                m = 3
            End If
        End Sub

        Public Function CalcolaEta(ByVal dal As Nullable(Of Date), ByVal al As Nullable(Of Date)) As Nullable(Of Single)
            If (dal.HasValue = False Or al.HasValue = False) Then Return Nothing
            Dim t1 As Date = Me.GetDatePart(dal)
            Dim t2 As Date = Me.GetDatePart(al)
            Dim ret As Single = Me.Year(t2) - Me.Year(t1)
            If (ret > 0) Then
                If (Me.Month(t2) < Me.Month(t1)) Then Return ret - 1
                If (Me.Month(t2) = Me.Month(t1)) Then
                    If (Me.Day(t2) < Me.Day(t1)) Then Return ret - 1
                End If
            End If
            Return ret
        End Function

        Public Function Year(ByVal data As Object) As Integer
            Return Microsoft.VisualBasic.Year(data)
        End Function

        Public Function Month(ByVal data As Object) As Integer
            Return Microsoft.VisualBasic.Month(data)
        End Function

        Public Function Day(ByVal data As Object) As Integer
            Return Microsoft.VisualBasic.Day(data)
        End Function

        Public Function DateAdd(ByVal interval As String, ByVal number As Double, ByVal data As Object) As Nullable(Of Date)
            Return Microsoft.VisualBasic.DateAdd(interval, number, data)
        End Function

        Public Function DateDiff(ByVal interval As String, ByVal d1 As Date, ByVal d2 As Date) As Long
            Return Microsoft.VisualBasic.DateDiff(interval, d1, d2)
        End Function

        Public Function DateDiff(ByVal interval As Microsoft.VisualBasic.DateInterval, ByVal d1 As Date, ByVal d2 As Date) As Long
            Return Microsoft.VisualBasic.DateDiff(interval, d1, d2)
        End Function

        ''' <summary>
        ''' Aggiunge il valore specificato alla data
        ''' </summary>
        ''' <param name="interval"></param>
        ''' <param name="number"></param>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DateAdd(ByVal interval As Microsoft.VisualBasic.DateInterval, ByVal number As Double, ByVal data As Object) As Date
            Dim intPart As Integer = Math.Floor(number)
            Dim decPart As Double = number - intPart
            Dim ret As Date

            ret = Microsoft.VisualBasic.DateAdd(interval, intPart, data)
            While (decPart <> 0)
                Select Case interval
                    Case DateInterval.Year
                        decPart *= 365
                        interval = DateInterval.Day
                        'Case DateInterval.WeekOfYear
                        'Case DateInterval.Weekday
                    Case DateInterval.Quarter
                        decPart *= 365 / 4
                        interval = DateInterval.Day
                    Case DateInterval.Month
                        decPart *= 30
                        interval = DateInterval.Minute
                    Case DateInterval.Day
                        decPart *= 24
                        interval = DateInterval.Day
                    Case DateInterval.Hour
                        decPart *= 60
                        interval = DateInterval.Minute
                    Case DateInterval.Minute
                        decPart *= 60
                        interval = DateInterval.Second
                    Case DateInterval.Second
                        decPart *= 60
                        intPart = Math.Floor(decPart)
                        decPart = decPart - intPart
                        Return Microsoft.VisualBasic.DateAdd(interval, intPart, ret)
                    Case Else
                        Throw New NotSupportedException("interval")
                End Select

                intPart = Math.Floor(decPart)
                decPart = decPart - intPart
                ret = Microsoft.VisualBasic.DateAdd(interval, intPart, ret)
            End While

            Return ret
        End Function

        ''' <summary>
        ''' Restituisce un numero che indica il giorno della settimana
        ''' </summary>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetWeekDay(ByVal d As Date) As Integer
            Return Weekday(d, FirstDayOfWeek.Monday)
        End Function

        Public ReadOnly Property [Module] As CModule
            Get
                If (m_Module Is Nothing) Then m_Module = Sistema.Modules.GetItemByName("Calendar")
                Return m_Module
            End Get
        End Property




        ''' <summary>
        ''' Restituisce un oggetto CPersonActivitiesInfo che racchiude le inform
        ''' </summary>
        ''' <param name="personID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPersonInfo(ByVal personID As Integer) As CCalendarActivityInfo
            Dim ret As New CCalendarActivityInfo
            ret.Initialize(personID)
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce il numero di mesi trascorsi tra le due date
        ''' </summary>
        ''' <param name="today"></param>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalcolaMesiTrascorsi(ByVal today As Date, ByVal data As Date) As Integer
            Dim today_year, today_month, today_day, data_year, data_month, data_day, ret As Integer
            today_year = Year(today)
            today_month = Month(today)
            today_day = Day(today)
            data_year = Year(data)
            data_month = Month(data)
            data_day = Day(data)
            If (today_year = data_year) Then
                ret = (data_month - today_month) + CInt((data_day - today_day) / 30)
            ElseIf (today_year < data_year) Then
                ret = (data_year - (today_year + 1)) * 12 + (data_month + 12 - today_month) + CInt((data_day - today_day) / 30)
            Else
                ret = -((today_year - (data_year + 1)) * 12 + (today_month + 12 - data_month) + CInt((today_day - data_day) / 30))
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce il numero di giorni lavorativi compresi tra le due date (incluse)
        ''' </summary>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ContaFeriali(ByVal fromDate As Date, ByVal toDate As Date, Optional ByVal escludiSabati As Boolean = False) As Integer
            Dim c As Date = fromDate
            Dim cnt As Integer = 0
            While (c < toDate)
                If (Me.IsFeriale(c) AndAlso (Not escludiSabati OrElse Me.GetWeekDay(c) <> 6)) Then cnt += 1
                c = Me.DateAdd(DateInterval.Day, 1, c)
            End While
            Return cnt
        End Function

        ''' <summary>
        ''' Restituisce il numero di anni trascorsi tra le due date
        ''' </summary>
        ''' <param name="today"></param>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalcolaAnniTrascorsi(ByVal today As Date, ByVal data As Date) As Integer
            'Dim n
            'n = DateDiff("y", today, data)
            'CalcolaAnniTrascorsi = Fix(n / 355.25)
            Dim i As Integer
            If data > today Then
                i = 0
                While DateAdd("yyyy", i, today) < data
                    i = i + 1
                End While
                i = i - 1
            Else
                i = 0
                While DateAdd("yyyy", i, today) < data
                    i = i - 1
                End While
                i = i + 1
            End If
            Return i
        End Function

        ''' <summary>
        ''' Restituisce il primo del mese corrente alla data specificata
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFirstMonthDay(ByVal data As Date) As Date
            Return DateSerial(Year(data), Month(data), 1)
        End Function

        ''' <summary>
        ''' Restituisce l'ultimo del mese corrente alla data specificata
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetLastMonthDay(ByVal data As Date) As Date
            Return DateAdd("d", -1, GetNextMonthFirstDay(data))
        End Function

        ''' <summary>
        ''' Restituisce le date (al secondo) che rappresentano i limiti (destro escluso) del periodo specificato
        ''' </summary>
        ''' <param name="periodo"></param>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <remarks></remarks>
        Public Function PeriodoToDates(ByVal periodo As String, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CIntervalloData
            Select Case LCase(Trim(periodo))
                Case "il mese prossimo", "il prossimo mese"
                    fromDate = Me.GetNextMonthFirstDay(Me.ToDay)
                    toDate = Me.GetLastMonthDay(fromDate)
                    toDate = DateAdd("s", 24 * 3600 - 1, toDate)
                Case "la settimana prossima", "la prossima settimana" : fromDate = Me.GetWeekFirstDay(Me.DateAdd(DateInterval.Day, 7, Me.ToDay)) : toDate = DateAdd("s", 7 * 24 * 3600 - 1, fromDate)
                Case "domani" : fromDate = Me.ToMorrow : toDate = DateAdd("s", 24 * 3600 - 1, fromDate)
                Case "oggi" : fromDate = Me.ToDay : toDate = DateAdd("s", 24 * 3600 - 1, fromDate)
                Case "ieri" : fromDate = Me.YesterDay : toDate = DateAdd("s", 24 * 3600 - 1, fromDate)
                Case "questa settimana" : fromDate = Me.GetWeekFirstDay(Me.ToDay) : toDate = DateAdd("s", 7 * 24 * 3600 - 1, fromDate)
                Case "la settimana scorsa", "la scorsa settimana" : fromDate = Me.GetWeekFirstDay(Me.DateAdd("d", -7, Me.ToDay)) : toDate = DateAdd("s", 7 * 24 * 3600 - 1, fromDate)
                Case "questo mese"
                    fromDate = Me.GetMonthFirstDay(Me.ToDay)
                    toDate = DateAdd("s", 24 * 3600 - 1, Me.GetLastMonthDay(Me.ToDay))
                Case "il mese scorso", "lo scorso mese"
                    fromDate = Me.GetPrevMonthFirstDay(Me.ToDay)
                    toDate = Me.GetLastMonthDay(fromDate)
                    toDate = DateAdd("s", 24 * 3600 - 1, toDate)
                Case "quest'anno", "questo anno"
                    fromDate = Me.MakeDate(Me.ToDay.Year, 1, 1)
                    toDate = Me.MakeDate(Me.ToDay.Year, 12, 31)
                    toDate = DateAdd("s", 24 * 3600 - 1, toDate)
                Case "l'anno scorso"
                    fromDate = Me.MakeDate(Me.ToDay.Year - 1, 1, 1)
                    toDate = Me.MakeDate(Me.ToDay.Year - 1, 12, 31)
                    toDate = DateAdd("s", 24 * 3600 - 1, toDate)
                Case "tra"
                    If (fromDate.HasValue AndAlso toDate.HasValue AndAlso fromDate = toDate) Then
                        toDate = DateAdd("s", 24 * 3600 - 1, fromDate)
                    End If
                Case "tutto" : fromDate = Nothing : toDate = Nothing
                Case Else
                    Try
                        Dim d As Date = Formats.ParseDate(periodo)
                        fromDate = d
                        toDate = DateAdd("s", 24 * 3600 - 1, fromDate)
                    Catch ex As Exception

                    End Try
            End Select

            Return New CIntervalloData(periodo, fromDate, toDate)
        End Function

        ''' <summary>
        ''' Restituisce l'array dei periodi supportati dalla funzione PeriodoToDates
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSupportedPeriods() As String()
            Return {"Oggi", "Ieri", "Questa settimana", "Questo mese", "Il mese scorso", "Quest'anno", "l'Anno scorso", "Tra", "Tutto"}
        End Function

        ''' <summary>
        ''' Restituisce un valore booleano che indica se la data è compresa nell'intervallo (estremi inclusi). Se uno o entrambi gli estremi sono Empty il confronto non viene effettuato
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="dataInizio"></param>
        ''' <param name="dataFine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckBetween(ByVal value As Date?, ByVal dataInizio As Nullable(Of Date), ByVal dataFine As Nullable(Of Date)) As Boolean
            If value.HasValue = False Then Return True
            If Not dataInizio.HasValue Then
                If Not dataFine.HasValue Then
                    Return True
                Else
                    Return (value <= dataFine.Value)
                End If
            Else
                If Not dataFine.HasValue Then
                    Return (value >= dataInizio.Value)
                Else
                    Return (value >= dataInizio.Value) And (value <= dataFine.Value)
                End If
            End If
        End Function


        ''' <summary>
        ''' Restituisce un valore booleano che indica se la data è compresa nell'intervallo (estremi inclusi). Se uno o entrambi gli estremi sono Empty il confronto non viene effettuato
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="dataInizio"></param>
        ''' <param name="dataFine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckBetween(ByVal value As Date?, ByVal dataInizio As Date?, ByVal dataFine As Date?, ByVal toleranceInSeconds As Integer) As Boolean
            If value.HasValue = False Then Return True
            If (dataInizio.HasValue) Then dataInizio = Me.DateAdd(DateInterval.Second, -toleranceInSeconds, dataInizio)
            If (dataFine.HasValue) Then dataFine = Me.DateAdd(DateInterval.Second, toleranceInSeconds, dataFine)
            Return Me.CheckBetween(value, dataInizio, dataFine)
        End Function

        ''' <summary>
        ''' Restituisce la data e l'ora corrente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Now() As Date
            Return Date.Now
        End Function

        ''' <summary>
        ''' Restituisce la mezzanotte di oggi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToDay() As Date
            Return Date.Today
        End Function

        ''' <summary>
        ''' Restituisce la mezzanotte di domani
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToMorrow() As Date
            Return DateAdd(DateInterval.Day, 1, ToDay)
        End Function

        ''' <summary>
        ''' Restituisce la mezzanotte di ieri
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function YesterDay() As Date
            Return DateAdd(DateInterval.Day, -1, ToDay)
        End Function


        Public Function NotifyPlannedActivity(ByVal activity As CCalendarActivity, ByVal user As CUser) As Integer
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Pianifica l'attività per l'utente corrente
        ''' </summary>
        ''' <param name="description"></param>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PlanActivity(ByVal description As String, ByVal fromDate As Date, ByVal toDate As Date) As CCalendarActivity
            Dim act As New CCalendarActivity
            act.Descrizione = description
            act.DataInizio = fromDate
            act.DataFine = toDate
            act.Stato = ObjectStatus.OBJECT_VALID
            act.Operatore = Sistema.Users.CurrentUser
            act.AssegnatoA = Users.CurrentUser
            act.Save()
            Return act
        End Function

        ''' <summary>
        ''' Pianifica l'attività per un utente specifico
        ''' </summary>
        ''' <param name="user"></param>
        ''' <param name="description"></param>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PlanActivityForUser(ByVal user As CUser, ByVal description As String, ByVal fromDate As Date, ByVal toDate As Date) As CCalendarActivity
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Dim act As New CCalendarActivity
            act.Descrizione = description
            act.DataInizio = fromDate
            act.DataFine = toDate
            act.Stato = ObjectStatus.OBJECT_VALID
            act.Operatore = Sistema.Users.CurrentUser
            act.AssegnatoA = user
            act.Save()
            Return act
        End Function

        ''' <summary>
        ''' Restituisce vero se gli impegni dell'utente (attività pianificate) intersecano l'intervallo di tempo specificato
        ''' </summary>
        ''' <param name="user"></param>
        ''' <param name="fromTime"></param>
        ''' <param name="toTime"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckIfBusy(ByVal user As CUser, ByVal fromTime As Date, ByVal toTime As Date) As Integer
            Return CheckIfBusy(GetID(user), fromTime, toTime)
        End Function

        ''' <summary>
        ''' Restituisce vero se gli impegni dell'utente (attività pianificate) intersecano l'intervallo di tempo specificato
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="fromTime"></param>
        ''' <param name="toTime"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckIfBusy(ByVal userID As Integer, ByVal fromTime As Date, ByVal toTime As Date) As Integer
            Dim items As CCollection(Of ICalendarActivity)
            Dim i As Integer
            Dim cnt As Integer
            items = GetPendingActivities()
            cnt = 0
            For i = 0 To items.Count - 1
                If items(i).IDOperatore = userID Then
                    If (items(i).DataInizio >= fromTime And items(i).DataInizio <= toTime) Or
                       (items(i).DataFine >= fromTime And items(i).DataFine <= toTime) Then
                        cnt += 1
                    End If
                End If
            Next
            Return cnt
        End Function

        ''' <summary>
        ''' Restituisce un array contenente gli operatori predefiniti
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAvailableOperators() As IEnumerable
            Return Sistema.Users.LoadAll
        End Function

        Public Function GetAvailableGroupOperators(ByVal group As CGroup) As IEnumerable
            Return group.Members
        End Function

        Public Function GetAvailableGroupOperators(ByVal groupID As Integer) As IEnumerable
            Return Me.GetAvailableGroupOperators(Sistema.Groups.GetItemById(groupID))
        End Function

        ''' <summary>
        ''' Restituisce un array contenente i gruppi disponibili
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAvailableGroups() As IEnumerable
            Return Sistema.Groups.LoadAll
        End Function

        ''' <summary>
        ''' Restituisce un array contenente i nomi dei tipi di classe che definiscono gli eventi che l'utente specificato può creare
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAvailableEventTypes() As System.Type()
            'Dim i As Integer
            'Dim prov As ICalendarProvider
            'Dim localActions As System.Type()
            'Dim actions As System.Type()
            'For i = 0 To Me.Providers.Count - 1
            '    prov = Me.Providers(i)
            '    localActions = prov.GetSupportedTypes
            '    'actions.Merge(localActions)
            'Next
            'Return actions
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Restituisce il nome abbreviato del giorno della settimana specificato (da 0 a 6)
        ''' </summary>
        ''' <param name="dayOfWeek"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetShortWeekDayName(ByVal dayOfWeek As Integer) As String
            If (dayOfWeek = 0) Then dayOfWeek = 7
            Return WeekdayName(dayOfWeek, True, Me.m_FirstDayOfWeek)
        End Function

        ''' <summary>
        ''' Restituisce il nome del giorno della settimana corrispondente alla data specificata
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetShortWeekDayName(ByVal data As Date) As String
            Return GetShortWeekDayName(data.DayOfWeek)
        End Function

        ''' <summary>
        ''' Restituisce il nome esteso del giorno della settimana specificato (da 0 a 6)
        ''' </summary>
        ''' <param name="dayOfWeek"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetWeekDayName(ByVal dayOfWeek As Integer) As String
            If (dayOfWeek = 0) Then dayOfWeek = 7
            Return WeekdayName(dayOfWeek, False, m_FirstDayOfWeek)
        End Function

        ''' <summary>
        ''' Restituisce il nome del giorno corrispondente alla data specificata
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetWeekDayName(ByVal data As Date) As String
            Return GetWeekDayName(data.DayOfWeek)
        End Function

        ''' <summary>
        ''' Restituisce la data corrispondente all'inizio della settimana specifica dell'anno
        ''' </summary>
        ''' <param name="year"></param>
        ''' <param name="weekNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFirstWeekDay(ByVal year As Integer, ByVal weekNumber As Integer) As Date
            Dim d As Date = Me.MakeDate(year, 1, 1)
            d = Microsoft.VisualBasic.DateAdd(DateInterval.WeekOfYear, weekNumber - 1, d)
            Dim wd As Integer = d.DayOfWeek - 1
            d = Microsoft.VisualBasic.DateAdd(DateInterval.Day, -wd, d)
            Return d
        End Function

        ''' <summary>
        ''' Restituisce il numero del giorno della settimana (1 è il primo giorno es: lun o dom)
        ''' </summary>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetWeekDayNumber(ByVal d As Date) As Integer
            Return Weekday(d, m_FirstDayOfWeek)
        End Function

        ''' <summary>
        ''' Restituisce la data che rappresenta le 0:00 del giorno value
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDatePart(ByVal value As Nullable(Of Date)) As Nullable(Of Date)
            If Not value.HasValue Then
                Return Nothing
            Else
                Return value.Value.Date ' DateSerial(Year(value), Month(value), Day(value))
            End If
        End Function

        ''' <summary>
        ''' Restituisce il nome esteso del mese a partire dal numero (1 - 12)
        ''' </summary>
        ''' <param name="monthNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMonthName(ByVal monthNumber As Integer) As String
            Return Strings.ToProperCase(MonthName(monthNumber, False))
        End Function

        Public Function GetMonthName(ByVal data As Date) As String
            Return GetMonthName(data.Month)
        End Function

        ''' <summary>
        ''' Restituisce il nome corto del mese a partire dal numero (1 - 12)
        ''' </summary>
        ''' <param name="monthNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetShortMonthName(ByVal monthNumber As Integer) As String
            Return Strings.ToProperCase(MonthName(monthNumber, True))
        End Function

        Public Function GetShortMonthName(ByVal data As Date) As String
            Return GetShortMonthName(data.Month)
        End Function

        ''' <summary>
        ''' Restituisce il primo giorno della settimana a cui appartiene la data specificata
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetWeekFirstDay(ByVal value As Date) As Date
            Return DateAdd(DateInterval.Day, -DatePart("w", value, 0), GetDatePart(value))
        End Function

        ''' <summary>
        ''' Restituisce il primo giorno del mese a cui appartiene la data specificata
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMonthFirstDay(ByVal value As Date) As Date
            Return DateSerial(Year(value), Month(value), 1)
        End Function

        ''' <summary>
        ''' Restituisce il primo giorno dell'anno a cui appartiene la data specificata
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetYearFirstDay(ByVal value As Date) As Date
            Return DateSerial(Year(value), 1, 1)
        End Function

        ''' <summary>
        ''' Restituisce il primo giorno dell'anno a cui appartiene la data specificata
        ''' </summary>
        ''' <param name="year">[in] Anno</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetYearFirstDay(ByVal year As Integer) As Date
            Return DateSerial(year, 1, 1)
        End Function

        ''' <summary>
        ''' Restituisce l'ultimo giorno dell'anno a cui appartiene la data specificata
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetYearLastDay(ByVal value As Date) As Date
            Return DateSerial(Year(value), 12, 31)
        End Function

        ''' <summary>
        ''' Restituisce il primo giorno del mese successivo alla data specificata
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNextMonthFirstDay(ByVal value As Date) As Date
            Dim y, m As Integer
            y = Year(value)
            m = Month(value) + 1
            If (m > 12) Then
                m = 1
                y = y + 1
            End If
            Return DateSerial(y, m, 1)
        End Function

        Public Function GetPrevMonthFirstDay(ByVal value As Date) As Date
            Dim y, m As Integer
            y = Year(value)
            m = Month(value) - 1
            If (m < 1) Then
                m = 12
                y = y - 1
            End If
            Return DateSerial(y, m, 1)
        End Function

        Public Function MakeDate(ByVal year As Integer, Optional ByVal month As Integer = 1, Optional ByVal day As Integer = 1, Optional ByVal hours As Integer = 0, Optional ByVal minutes As Integer = 0, Optional ByVal seconds As Integer = 0) As Date
            Return New Date(year, month, day, hours, minutes, seconds)
        End Function


        '--------------------------------------------------------
        ' PROVIDERS

        ''' <summary>
        ''' Restituisce una collezione di oggetti che implementano l'interfaccia ICalendarProvider
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Providers As CCollection(Of ICalendarProvider)
            Get
                SyncLock Me
                    If (m_Providers Is Nothing) Then Me.m_Providers = New CCollection(Of ICalendarProvider)
                    Return New CCollection(Of ICalendarProvider)(Me.m_Providers)
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce una attività in base al suo ID
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetActivityByID(ByVal value As Integer) As CCalendarActivity

            If (value = 0) Then Return Nothing
            Dim dbSQL As String = "SELECT * FROM [tbl_CalendarActivities] WHERE [ID]=" & value
            Dim reader As New DBReader(APPConn.Tables("tbl_CalendarActivities"), dbSQL)
            Dim item As CCalendarActivity = Nothing
#If Not DEBUG Then
            Try
#End If
            If reader.Read Then
                item = New CCalendarActivity
                If Not APPConn.Load(item, reader) Then item = Nothing
            End If
#If Not DEBUG Then
            Catch ex As Exception
                Throw
            Finally
#End If
            reader.Dispose()
#If Not DEBUG Then
            End Try
#End If

            Return item
        End Function

        Public ReadOnly Property DefaultComparer As Object
            Get
                If m_DefaultComparer Is Nothing Then m_DefaultComparer = New CCalendarItemsComparer
                Return m_DefaultComparer
            End Get
        End Property

        Public Function ShouldExec(ByVal intervalType As DateInterval, ByVal interval As Integer, ByVal lastRun As Date?, ByVal fromTime As Double?, ByVal toTime As Double?) As Boolean
            Dim f1 As Date? = Nothing
            Dim f2 As Date? = Nothing
            If (fromTime.HasValue) Then f1 = Me.DateAdd(DateInterval.Hour, fromTime.Value, Me.ToDay)
            If (toTime.HasValue) Then f2 = Me.DateAdd(DateInterval.Hour, toTime.Value, Me.ToDay)
            Return ShouldExec(intervalType, interval, lastRun, f1, f2)
        End Function

        Public Function GetTimePart(ByVal d As Date?) As Date?
            If (d.HasValue = False) Then Return Nothing
            Return Me.MakeDate(Date.MinValue.Year, Date.MinValue.Month, Date.MinValue.Day, d.Value.Hour, d.Value.Minute, d.Value.Second)
        End Function

        Public Function ShouldExec(ByVal intervalType As DateInterval, ByVal interval As Integer, ByVal lastRun As Date?, ByVal fromTime As Date?, ByVal toTime As Date?) As Boolean
            If (Me.CheckBetween(GetTimePart(Me.Now), GetTimePart(fromTime), GetTimePart(toTime))) Then
                If (lastRun.HasValue) Then
                    Return Math.Abs(Me.DateDiff(intervalType, Me.Now, lastRun.Value)) >= interval
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Restituisce una collezione ordinata di oggetti CCalendarActivity che rappresentano le scadenze nell'intervallo specificato
        ''' </summary>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetScadenze(ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CCollection(Of ICalendarActivity)
            Dim ret As New CCollection(Of ICalendarActivity)
            Dim col As CCollection(Of ICalendarActivity)
            ret.Comparer = Me.DefaultComparer
            ret.Sorted = True
            Dim providers As CCollection(Of ICalendarProvider) = Me.Providers
            For i As Integer = 0 To providers.Count - 1
                Dim provider As ICalendarProvider = providers(i)
                col = provider.GetScadenze(fromDate, toDate)
                col.Comparer = Me.DefaultComparer
                col.Sorted = True
                ret.Merge(col)
            Next
            Return ret
        End Function

        Public Function GetPendingActivities() As CCollection(Of ICalendarActivity)
            Dim allItems As CCollection(Of ICalendarActivity)
            Dim items As CCollection(Of ICalendarActivity)
            Dim calComparer As New CCalendarItemsComparer
            allItems = New CCollection(Of ICalendarActivity)
            allItems.Comparer = calComparer
            allItems.Sorted = True
            Dim providers As CCollection(Of ICalendarProvider) = Me.Providers
            For i As Integer = 0 To providers.Count - 1
                Dim provider As ICalendarProvider = providers(i)
                items = provider.GetPendingActivities()
                items.Comparer = calComparer
                items.Sorted = True
                allItems.Merge(items)
            Next
            Return allItems
        End Function

        ''' <summary>
        ''' Restituisce una collezione di oggetti CActivePerson
        ''' </summary>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <param name="ufficio"></param>
        ''' <param name="operatore"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetActivePersons(ByVal nomeLista As String, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date), Optional ByVal ufficio As Integer = 0, Optional ByVal operatore As Integer = 0) As CCollection(Of CActivePerson)
            Dim ret As New CCollection(Of CActivePerson)
            Dim tmp As CCollection(Of CActivePerson)
            ret.Comparer = Arrays.DefaultComparer
            ret.Sorted = True
            Dim providers As CCollection(Of ICalendarProvider) = Me.Providers
            For i As Integer = 0 To providers.Count - 1
                Dim provider As ICalendarProvider = providers(i)
                tmp = provider.GetActivePersons(nomeLista, fromDate, toDate, ufficio, operatore)
                tmp.Comparer = ret.Comparer
                tmp.Sorted = True
                ret.Merge(tmp)
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce il numero della settimana a partire dall'inizio dell'anno relatvo alla data specificata
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetWeekOfYear(ByVal value As Date) As Integer
            Return Microsoft.VisualBasic.DatePart(DateInterval.WeekOfYear, value)
        End Function


        ''' <summary>
        ''' Restituisce il giorno del mese indicata dalla data specificata
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDay(ByVal value As Date) As Integer
            Return Day(value)
        End Function

        Public Function GetYear(ByVal value As Date) As Integer
            Return Year(value)
        End Function

        Public Function GetMonth(ByVal value As Date) As Integer
            Return Month(value)
        End Function

        Public Function GetHour(ByVal value As Date) As Integer
            Return Hour(value)
        End Function

        Public Function GetMinute(ByVal value As Date) As Integer
            Return Minute(value)
        End Function

        Public Function GetSecond(ByVal value As Date) As Integer
            Return Second(value)
        End Function

        Public Function Format(ByVal value As Date, ByVal formatString As String) As String
            Const separators As String = " /|\""'()[]{}+*°<>!_-.:,;"
            Dim i, status As Integer
            Dim ch, ret, token As String
            i = 1
            status = 0
            ret = ""
            token = ""
            While (i < Strings.Len(formatString))
                ch = Strings.Mid(formatString, i, 1)
                If (Strings.InStr(separators, ch) > 0) Then
                    Select Case (token)
                        Case "d" : token = Me.GetDay(value)
                        Case "dd" : token = Strings.Right("00" & Me.GetDay(value), 2)
                        Case "ddd" : token = Me.GetShortWeekDayName(value)
                        Case "dddd" : token = Me.GetWeekDayName(value)
                        Case "M" : token = Me.GetMonth(value)
                        Case "MM" : token = Strings.Right("00" & Me.GetMonth(value), 2)
                        Case "MMM" : token = Me.GetShortMonthName(value)
                        Case "MMMM" : token = Me.GetMonthName(value)
                        Case "yy" : token = Strings.Right("00" & Me.GetYear(value), 2)
                        Case "yyy" : token = Me.GetYear(value)
                        Case "yyyy" : token = Me.GetYear(value)
                        Case "h" : token = Me.GetHour(value)
                        Case "HH", "hh" : token = Strings.Right("00" & Me.GetHour(value), 2)
                        Case "m" : token = Me.GetMinute(value)
                        Case "mm" : token = Strings.Right("00" & Me.GetMinute(value), 2)
                        Case "s" : token = Me.GetSecond(value)
                        Case "ss" : token = Strings.Right("00" & Me.GetSecond(value), 2)
                    End Select
                    ret &= token
                    ret &= ch
                    token = ""
                Else
                    token = token & ch
                End If
                i = i + 1
            End While
            ch = ""

            Select Case (token)
                Case "d" : token = Me.GetDay(value)
                Case "dd" : token = Strings.Right("00" & Me.GetDay(value), 2)
                Case "ddd" : token = Me.GetShortWeekDayName(value)
                Case "dddd" : token = Me.GetWeekDayName(value)
                Case "M" : token = Me.GetMonth(value)
                Case "MM" : token = Strings.Right("00" & Me.GetMonth(value), 2)
                Case "MMM" : token = Me.GetShortMonthName(value)
                Case "MMMM" : token = Me.GetMonthName(value)
                Case "yy" : token = Strings.Right("00" & Me.GetYear(value), 2)
                Case "yyy" : token = Me.GetYear(value)
                Case "yyyy" : token = Me.GetYear(value)
                Case "h" : token = Me.GetHour(value)
                Case "hh" : token = Strings.Right("00" & Me.GetHour(value), 2)
                Case "m" : token = Me.GetMinute(value)
                Case "mm" : token = Strings.Right("00" & Me.GetMinute(value), 2)
                Case "s" : token = Me.GetSecond(value)
                Case "ss" : token = Strings.Right("00" & Me.GetSecond(value), 2)
            End Select
            ret &= token
            ret &= ch
            token = ""
            Return ret
        End Function


        Public Function GetCompleanniPerMese(ByVal year As Integer, ByVal month As Integer) As CCollection(Of CPersonaInfo)()
            Dim cursor As New CPersonaFisicaCursor
            Dim ret As CCollection(Of CPersonaInfo)()

            Try
                Dim mLen As Integer = Me.GetDaysInMonth(year, month)
                ReDim ret(mLen - 1)

                Dim arr As New System.Collections.ArrayList
                For Each po As CUfficio In Anagrafica.Uffici.GetPuntiOperativiConsentiti
                    arr.Add(GetID(po))
                Next

                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Deceduto.Value = False
                cursor.WhereClauses.Add("(Day([DataNascita]) Between 1 And " & mLen & ") And (Month([DataNascita])=" & month & ")")
                cursor.IgnoreRights = True
                cursor.IDPuntoOperativo.ValueIn(arr.ToArray)

                For i As Integer = 0 To UBound(ret)
                    ret(i) = New CCollection(Of CPersonaInfo)
                Next

                While Not cursor.EOF
                    ret(Me.GetDay(cursor.Item.DataNascita) - 1).Add(New CPersonaInfo(cursor.Item))
                    cursor.MoveNext()
                End While

                For i As Integer = 0 To UBound(ret)
                    ret(i).Sort()
                Next


            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            Return ret
        End Function

        Public Function GetCompleanniPerGiorno(ByVal d As Date) As CPersonaInfo()
            Dim ret As New CCollection(Of CPersonaInfo)
            Dim cursor As New CPersonaFisicaCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Deceduto.Value = False
                cursor.WhereClauses.Add("(Day([DataNascita])=" & GetDay(d) & ") And (Month([DataNascita])=" & GetMonth(d) & ")")
                cursor.IgnoreRights = True
                Dim arr As New System.Collections.ArrayList
                For Each po As CUfficio In Anagrafica.Uffici.GetPuntiOperativiConsentiti
                    arr.Add(GetID(po))
                Next
                cursor.IDPuntoOperativo.ValueIn(arr.ToArray)

                While Not cursor.EOF
                    ret.Add(New CPersonaInfo(cursor.Item))
                    cursor.MoveNext()
                End While

                ret.Sort()
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            Return ret.ToArray
        End Function




        Public Function MergeDateTime(Data As Date?, Ora As Date?) As Date
            If (Data.HasValue) Then
                If (Ora.HasValue) Then
                    Return MakeDate(Year(Data), Month(Data), Day(Data), Hour(Ora), Minute(Ora), Second(Ora))
                Else
                    Return Data
                End If
            Else
                Return Ora
            End If
        End Function

        Function Compare(ByVal d1 As Date?, ByVal d2 As Date?) As Integer
            If (d1.HasValue) Then
                If (d2.HasValue) Then
                    If (d1.Value < d2.Value) Then Return -1
                    If (d1.Value > d2.Value) Then Return 1
                    Return 0
                Else
                    Return 1
                End If
            Else
                If (d2.HasValue) Then
                    Return -1
                Else
                    Return 0
                End If
            End If
        End Function

        ''' <summary>
        ''' Restituisce la data corrispondente ad un secondo alla mezzanotte del giorno specificato
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetLastSecond(ByVal value As Date) As Date
            Return Me.DateAdd(DateInterval.Second, 24 * 3600 - 1, Me.GetDatePart(value))
        End Function

        Public Function Max(ByVal d1 As Date?, ByVal d2 As Date?) As Date?
            If (d1.HasValue) Then
                If (d2.HasValue) Then
                    Dim c As Integer = Me.Compare(d1, d2)
                    If (c <= 0) Then
                        Return d2
                    ElseIf (c > 0) Then
                        Return d1
                    End If
                Else
                    Return d1
                End If
            Else
                Return d2
            End If
        End Function

        Public Function Min(ByVal d1 As Date?, ByVal d2 As Date?) As Date?
            If (d1.HasValue) Then
                If (d2.HasValue) Then
                    Dim c As Integer = -Me.Compare(d1, d2)
                    If (c <= 0) Then
                        Return d2
                    ElseIf (c > 0) Then
                        Return d1
                    End If
                Else
                    Return d1
                End If
            Else
                Return d2
            End If
        End Function

        Public Function GetToDoList(ByVal user As CUser) As CCollection(Of ICalendarActivity)
            If (user Is Nothing) Then Throw New ArgumentNullException("user")

            Dim ret As New CCollection(Of ICalendarActivity)

            Dim providers As CCollection(Of ICalendarProvider) = Me.Providers
            For Each p As ICalendarProvider In providers
                ret.AddRange(p.GetToDoList(user))
            Next

            ret.Sort()

            Return ret
        End Function

        Public Function GetProviderByName(ByVal name As String) As ICalendarProvider
            Dim providers As CCollection(Of ICalendarProvider) = Me.Providers
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            For Each p As ICalendarProvider In providers
                If p.UniqueName = name Then Return p
            Next
            Return Nothing
        End Function

        Public Sub AddProvider(ByVal p As ICalendarProvider)
            SyncLock Me
                If (Me.GetProviderByName(p.UniqueName) IsNot Nothing) Then Throw New ArgumentException("Provider già registrato")
                Me.m_Providers.Add(p)
            End SyncLock
        End Sub

        Public Sub RemoveProvider(ByVal p As ICalendarProvider)
            SyncLock Me
                p = Me.GetProviderByName(p.UniqueName)
                If (p Is Nothing) Then Throw New KeyNotFoundException("Provider non trovato")
                Me.m_Providers.Remove(p)
            End SyncLock
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace

Partial Class Sistema

    Public Enum vbFirstDayOfWeekEnum As Integer
        ''' <summary>
        ''' Viene utilizzata l'impostazione API National Language Support (NLS).
        ''' </summary>
        ''' <remarks></remarks>
        vbUseSystem = 0
        ''' <summary>
        ''' Domenica (impostazione predefinita)
        ''' </summary>
        ''' <remarks></remarks>
        vbSunday = 1
        ''' <summary>
        ''' Lunedì
        ''' </summary>
        ''' <remarks></remarks>
        vbMonday = 2
        ''' <summary>
        ''' Martedì
        ''' </summary>
        ''' <remarks></remarks>
        vbTuesday = 3
        ''' <summary>
        ''' Mercoledì
        ''' </summary>
        ''' <remarks></remarks>
        vbWednesday = 4
        ''' <summary>
        ''' Giovedì
        ''' </summary>
        ''' <remarks></remarks>
        vbThursday = 5
        ''' <summary>
        ''' Venerdì
        ''' </summary>
        ''' <remarks></remarks>
        vbFriday = 6
        ''' <summary>
        ''' Sabato
        ''' </summary>
        ''' <remarks></remarks>
        vbSaturday = 7
    End Enum


    Private Shared m_Calendar As CCalendarClass = Nothing

    Public Shared ReadOnly Property Calendar As CCalendarClass
        Get
            If (m_Calendar Is Nothing) Then m_Calendar = New CCalendarClass
            Return m_Calendar
        End Get
    End Property



End Class