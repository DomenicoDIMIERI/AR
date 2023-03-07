Imports System.IO
Imports FinSeA.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.Sistema

Namespace org.apache.pdfbox.util


    '/**
    ' * Date format is described in PDF Reference 1.7 section 3.8.2
    ' * (www.adobe.com/devnet/acrobat/pdfs/pdf_reference_1-7.pdf)
    ' * and also in PDF 32000-1:2008 
    ' * (http://www.adobe.com/devnet/acrobat/pdfs/PDF32000_2008.pdf))
    ' * although the latter inexplicably omits the trailing apostrophe.
    ' * 
    ' * The interpretation of dates without timezones is unclear. 
    ' * The code below assumes that such dates are in UTC+00 (aka GMT).
    ' * This is in keeping with the PDF Reference's assertion that:
    ' *      numerical fields default to zero values. 
    ' * However, the Reference does go on to make the cryptic remark:
    ' *      If no UT information is specified, the relationship of the specified  
    ' *      time to UT is considered to be unknown. Whether or not the time 
    ' *      zone is known, the rest of the date should be specified in local time.
    ' * I understand this to refer to _creating_ a pdf date value. That is, 
    ' * code that can get the wall clock time and cannot get the timezone 
    ' * should write the wall clock time with a time zone of zero.
    ' * When _parsing_ a PDF date, the statement talks about "the rest of the date"
    ' * being local time, thus explicitly excluding the use of the local time
    ' * for the time zone.
    '*/ 

    '/**
    ' * This class is used to convert dates to strings and back using the PDF
    ' * date standard in section 3.8.2 of PDF Reference 1.7.  
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author <a href="mailto:zweibieren@ahoo.com">Fred Hansen</a>
    ' * 
    ' * TODO Move members of this class elsewhere for shared use in pdfbox, xmpbox, and jempbox.
    ' */
    Public Class DateConverter

        ' milliseconds/1000 = seconds; seconds / 60 = minutes; minutes/60 = hours
        Private Const MINUTES_PER_HOUR As Integer = 60
        Private Const SECONDS_PER_MINUTE As Integer = 60
        Private Const MILLIS_PER_MINUTE = SECONDS_PER_MINUTE * 1000
        Private Const MILLIS_PER_HOUR As Integer = MINUTES_PER_HOUR * MILLIS_PER_MINUTE
        Private Const HALF_DAY As Integer = 12 * MINUTES_PER_HOUR * MILLIS_PER_MINUTE
        Private Const DAY As Integer = 2 * HALF_DAY

        '/**
        ' * Error value if date is invalid. Parsing is done with 
        ' * GregorianCalendar.setLenient(false), so every date field value
        ' * must be within bounds. If an attempt is made to parse an invalid date 
        ' * field, toCalendar(String, String[]) returns Jan 1 in year INVALID_YEAR.
        ' */
        Public Const INVALID_YEAR As Integer = 999


        '/**
        ' * The Date format is supposed to be the PDF_DATE_FORMAT, but other
        ' * forms appear. These lists offer alternatives to be tried 
        ' * if parseBigEndianDate fails.  
        ' * 
        ' * The time zone offset generally trails the date string, so it is processed
        ' * separately with parseTZoffset. (This does not preclude having time
        ' * zones in the elements below; one does.)
        ' * 
        ' * Alas, SimpleDateFormat is badly non-reentrant -- it modifies its 
        ' * calendar field (PDFBox-402), so these lists are strings to create
        ' * SimpleDate format as needed.
        ' * 
        ' * Some past entries have been elided because they duplicate existing 
        ' * entries. See the API for SimpleDateFormat, which says 
        ' *      "For parsing, the number of pattern letters is ignored 
        ' *      unless it's needed to separate two adjacent fields."
        ' * 
        ' * toCalendar(String, String[]) tests to see that the entire input text
        ' * has been consumed. Therefore the ordering of formats is important. 
        ' * If one format begins with the entirety of another, the longer
        ' * must precede the other in the list.
        ' * 
        ' * HH is for 0-23 hours and hh for 1-12 hours; an "a" field must follow "hh"
        ' * Where year is yy, four digit years are accepted 
        ' * and two digit years are converted to four digits in the range
        ' *      [thisyear-79...thisyear+20]
        ' */
        Private Shared ReadOnly ALPHA_START_FORMATS() As String = { _
                                                                "EEEE, dd MMM yy hh:mm:ss a", _
                                                                "EEEE, MMM dd, yy hh:mm:ss a", _
                                                                "EEEE, MMM dd, yy 'at' hh:mma", _
                                                                "EEEE, MMM dd, yy", _
                                                                "EEEE MMM dd, yy HH:mm:ss", _
                                                                "EEEE MMM dd HH:mm:ss z yy", _
                                                                "EEEE MMM dd HH:mm:ss yy" _
                                                                }

        Private Shared ReadOnly DIGIT_START_FORMATS() As String = { _
                                                                    "dd MMM yy HH:mm:ss", _
                                                                    "dd MMM yy HH:mm", _
                                                                    "yyyy MMM d", _
                                                                    "yyyymmddhh:mm:ss", _
                                                                    "H:m M/d/yy", _
                                                                    "M/d/yy HH:mm:ss", _
                                                                    "M/d/yy HH:mm", _
                                                                    "M/d/yy" _
                                                                    }
        '// proposed rule that is unreachable due to "dd MMM yy HH:mm:ss" 
        '//     "yyyy MMM d HH:mm:ss", 

        '// rules made unreachable by "M/d/yy HH:mm:ss" "M/d/yy HH:mm"  "M/d/yy",
        '// (incoming digit strings do not mark themselves as y, m, or d!)
        '    // "d/MM/yyyy HH:mm:ss", // PDFBOX-164 and PDFBOX-170 
        '    // "M/dd/yyyy hh:mm:ss",
        '    // "MM/d/yyyy hh:mm:ss",
        '    // "M/d/yyyy HH:mm:ss",
        '    // "M/dd/yyyy",
        '    // "MM/d/yyyy",
        '    // "M/d/yyyy",
        '    // "M/d/yyyy HH:mm:ss",
        '    // "M/d/yy HH:mm:ss",
        '// subsumed by big-endian parse
        '    // "yyyy-MM-dd'T'HH:mm:ss",
        '    // "yyyy-MM-dd'T'HH:mm:ss",
        '    // "yyyymmdd hh:mm:ss", 
        '    // "yyyymmdd", 
        '    // "yyyymmddX''00''",  // covers 24 cases 
        '    //    (orignally the above ended with '+00''00'''; 
        '    //      the first apostrophe quoted the plus, 
        '    //      '' mapped to a single ', and the ''' was invalid)


        Private Sub New()
            'utility class should not be constructed.
        End Sub

        '////////////////////////////////////////////
        '// C o n v e r t   t o   S t r i n g   Methods

        '/**
        ' * Get all know formats.
        ' * 
        ' * @return an array containig all known formats
        ' */
        Public Shared Function getFormats() As String()
            Dim val() As String
            ReDim val(ALPHA_START_FORMATS.Length + DIGIT_START_FORMATS.Length - 1)
            Array.Copy(ALPHA_START_FORMATS, 0, val, 0, ALPHA_START_FORMATS.Length)
            Array.Copy(DIGIT_START_FORMATS, 0, val, ALPHA_START_FORMATS.Length, DIGIT_START_FORMATS.Length)
            Return val
        End Function

        '/**
        ' * Converts a Calendar to a string formatted as:
        ' *     D:yyyyMMddHHmmss#hh'mm'  where # is Z, +, or -.
        ' * 
        ' * @param cal The date to convert to a string. May be null.
        ' * The DST_OFFSET is included when computing the output time zone.
        ' *
        ' * @return The date as a String to be used in a PDF document, 
        ' *      or null if the cal value is null
        ' */
        Public Shared Shadows Function toString(ByVal cal As NDate) As String
            If (cal.HasValue = False) Then Return ""
            Dim offset As String = formatTZoffset(cal.get(Calendar.ZONE_OFFSET) & cal.get(Calendar.DST_OFFSET), "'")
            Return String.Format("D:" & "%1$4tY%1$2tm%1$2td" & "%1$2tH%1$2tM%1$2tS" & "%2$s" & "'", cal, offset)

            '     String offset = formatTZoffset(cal.get(Calendar.ZONE_OFFSET)
            '        + cal.get(Calendar.DST_OFFSET), "'");
            'return String.format("D:"
            '        + "%1$4tY%1$2tm%1$2td"   // yyyyMMdd 
            '        + "%1$2tH%1$2tM%1$2tS"   // HHmmss 
            '        + "%2$s"                // time zone
            '        + "'",                  // trailing apostrophe
            '    cal, offset);      
        End Function

        '/**
        ' * Converts the date to ISO 8601 string format:
        ' *     yyyy-mm-ddThh:MM:ss#hh:mm    (where '#" is '+' or '-').
        ' *
        ' * @param cal The date to convert.  Must not be null.
        ' * The DST_OFFSET is included in the output value.
        ' * 
        ' * @return The date represented as an ISO 8601 string.
        ' */
        Public Shared Function toISO8601(ByVal cal As NDate) As String
            Dim offset As String = formatTZoffset(cal.get(Calendar.ZONE_OFFSET) & cal.get(Calendar.DST_OFFSET), ":")
            Return String.Format("%1$4tY" & "-%1$2tm" & "-%1$2td" & "T" & "%1$2tH:%1$2tM:%1$2tS" & "%2$s", cal, offset)

            'return String.format(
            '        "%1$4tY"   // yyyy
            '        + "-%1$2tm"   // -mm  (%tm adds one to cal month value)
            '        + "-%1$2td"  // -dd  (%tm adds one to cal month value)
            '        + "T"                             // T
            '        + "%1$2tH:%1$2tM:%1$2tS"   // HHmmss  
            '        + "%2$s",              // time zone
            '    cal, offset);      
        End Function

        '/**
        ' * Constrain a timezone offset to the range  [-11:59 thru +11:59].
        ' * @param proposedOffset A value intended to be a timezone offset.
        ' * @return The corresponding value reduced to the above noted range 
        ' * by adding or subtracting multiples of a full day.
        ' */
        Public Shared Function restrainTZoffset(ByVal proposedOffset As Integer) As Integer
            proposedOffset = ((proposedOffset + HALF_DAY) Mod DAY + DAY) Mod DAY
            ' 0 <= proposedOffset < DAY
            proposedOffset = (proposedOffset - HALF_DAY) Mod HALF_DAY
            ' -HALF_DAY < proposedOffset < HALF_DAY
            Return CInt(proposedOffset)
        End Function

        '/** 
        ' * Formats a time zone offset as #hh^mm
        ' * where # is + or -, hh is hours, ^ is a separator, and mm is minutes.
        ' * Any separator may be specified by the second argument;
        ' * the usual values are ":" (ISO 8601), "" (RFC 822), and "'" (PDF).
        ' * The returned value is constrained to the range -11:59 ... 11:59.
        ' * For offset of 0 millis, the String returned is "+00^00", never "Z".
        ' * To get a "general" offset in form GMT#hh:mm, write
        ' *      "GMT"+DateConverter.formatTZoffset(offset, ":");
        ' * <p>
        ' * Take thought in choosing the source for the millis value. 
        ' * It can come from calendarValue.getTimeZone() or from 
        ' * calendarValue.get(Calendar.ZONE_OFFSET).  If a TimeZone was created
        ' * from a valid time zone ID, then it may have a daylight savings rule.
        ' * (As of July 4, 2013, the data base at http://www.iana.org/time-zones 
        ' * recognized 629 time zone regions. But a TimeZone created as 
        ' *      new SimpleTimeZone(millisOffset, "ID"), 
        ' * will not have a daylight savings rule. (Not even if there is a
        ' * known time zone with the given ID. To get the TimeZone named "xDT"
        ' * with its DST rule, use an ID of EST5EDT, CST6CDT, MST7MDT, or PST8PDT.
        ' * <p>
        ' * When parsing PDF dates, the incoming values DOES NOT have a TIMEZONE value.
        ' * At most it has an OFFSET value like -04'00'. It is generally impossible to 
        ' * determine what TIMEZONE corresponds to a given OFFSET. If the date is
        ' * in the summer when daylight savings is in effect, an offset of -0400
        ' * might correspond to any one of the 38 regions (of 53) with standard time 
        ' * offset -0400 and no daylight saving. Or it might correspond to 
        ' * any one of the 31 regions (out of 43) that observe daylight savings 
        ' * and have standard time offset of -0500.
        ' * <p>
        ' * If a Calendar has not been assigned a TimeZone with setTimeZone(), 
        ' * it will have by default the local TIMEZONE, not just the OFFSET.  In the
        ' * USA, this TimeZone will have a daylight savings rule.
        ' * <p>
        ' * The offset assigned with calVal.set(Calendar.ZONE_OFFSET) differs
        ' * from the offset in the TimeZone set by Calendar.setTimeZone(). Example:
        ' * Suppose my local TimeZone is America/New_York. It has an offset of -05'00'.
        ' * And suppose I set a GregorianCalendar's ZONE_OFFSET to -07'00'
        ' *     calVal = new GregorianCalendar();   // TimeZone is the local default
        ' *     calVal.set(Calendar.ZONE_OFFSET, -7* MILLIS_PER_HOUR);
        ' * Four different offsets can be computed from calVal:
        ' *     calVal.get(Calendar.ZONE_OFFSET)  =>  -07:00
        ' *     calVal.get(Calendar.ZONE_OFFSET) + calVal.get(Calendar.DST_OFFSET) => -06:00
        ' *     calVal.getTimeZone().getRawOffset()  =>  -05:00
        ' *     calVal.getTimeZone().getOffset(calVal.getTimeInMillis())  =>  -04:00
        ' * <p>
        ' * Which is correct??? I dunno, though setTimeZone() does seem to affect
        ' * ZONE_OFFSET, and not vice versa.  One cannot even test whether TimeZone 
        ' * or ZONE_OFFSET has been set; both have been set by initialization code.
        ' * TimeZone is initialized to the local default time zone 
        ' * and ZONE_OFFSET is set from it.
        ' * 
        ' * My choice in this DateConverter class has been to set the 
        ' * initial TimeZone of a GregorianCalendar to GMT. Thereafter
        ' * the TimeZone is modified with {@link #adjustTimeZoneNicely}. 
        ' * 
        ' * @param millis a time zone offset expressed in milliseconds
        ' *      Any value is accepted; it is normalized to [-11:59 ... +11:59]
        ' * @param sep a String to insert between hh and mm. May be empty.
        ' * @return the formatted String for the offset
        ' */
        Public Shared Function formatTZoffset(ByVal millis As Integer, ByVal sep As String) As String
            Dim sdf As New SimpleDateFormat("Z") ' // #hhmm
            sdf.setTimeZone(New SimpleTimeZone(restrainTZoffset(millis), "unknown"))
            Dim tz As String = sdf.format(New Date())
            Return tz.Substring(0, 3) + sep + tz.Substring(3)
        End Function

        '//////////////////////////////////////////////
        '// P A R S E   Methods

        ' /**
        ' * Parses an integer from a string, starting at and advancing a ParsePosition.
        ' * 
        ' * @param text The string being parsed. If null, the remedy value is returned.
        ' * @param where The ParsePosition to start the search. This value 
        ' *      will be incremented by the number of digits found, but no 
        ' *      more than maxlen.  That is, the ParsePosition will 
        ' *      advance across at most maxlen initial digits in text.
        ' *      The error index is ignored and unchanged.
        ' * @param maxlen The maximum length of the integer to parse. 
        ' *      Usually 2, but 4 for year fields.
        ' *      If the field of length maxlen begins with a digit, 
        ' *      but contains a non-digit, no error is signaled 
        ' *      and the integer value is returned.
        ' * @param remedy Value to be assigned if no digit is found at the
        ' *      initial parse position; that is, if the field is empty.
        ' * @return The integer that was at the given parse position. Or
        ' *      the remedy value if no digits were found.
        ' */
        Public Shared Function parseTimeField(ByVal text As String, ByVal where As ParsePosition, ByVal maxlen As Integer, ByVal remedy As Integer) As Integer
            If (text = "") Then Return remedy
            ' (it would seem that DecimalFormat.parse() would be simpler;
            '     but that class blithely ignores setMaximumIntegerDigits)
            Dim retval As Integer = 0
            Dim index As Integer = where.getIndex()
            Dim limit As Integer = index + Math.Min(maxlen, text.Length() - index)
            While (index < limit)
                Dim cval As Integer = Convert.ToInt16(text.Chars(index)) - Asc("0") '  // convert digit to integer
                If (cval < 0 Or cval > 9) Then ' test to see if we got a digit
                    Exit While  '   // no digit at index
                End If
                retval = retval * 10 + cval '   // append the digit to the return value
                index += 1
            End While
            If (index = where.getIndex()) Then
                Return remedy
            End If
            where.setIndex(index)
            Return retval
        End Function

        '/**
        ' * Advances the ParsePosition past any and all the characters 
        ' *      that match those in the optionals list.
        ' *      In particular, a space will skip all spaces.
        ' * @param text The text to examine
        ' * @param where index to start looking. 
        ' *      The value is incremented by the number of optionals found.
        ' *      The error index is ignored and unchanged.
        ' * @param optionals A String listing all the optional characters 
        ' *      to be skipped.
        ' * @return The last non-space character passed over. 
        ' *      Returns a space if no non-space character was found 
        ' *      (even if space is not in the optionals list.)
        ' */
        Public Shared Function skipOptionals(ByVal text As String, ByVal where As ParsePosition, ByVal optionals As String) As Char
            Dim retval As Char = " "
            Dim currch As Char
            currch = text.Chars(where.getIndex())
            While (text <> "" AndAlso where.getIndex() < text.Length() AndAlso optionals.IndexOf(currch) >= 0)
                retval = IIf(currch <> " ", currch, retval)
                where.setIndex(where.getIndex() + 1)
                currch = text.Chars(where.getIndex())
            End While
            Return retval
        End Function

        '/**
        ' * If the victim string is at the given position in the text,
        ' * this method advances the position past that string. 
        ' * 
        ' * @param text The text to examine
        ' * @param victim The string to look for
        ' * @param where The initial position to look at. After return, this will
        ' *      have been incremented by the length of the victim if it was found.
        ' *      The error index is ignored and unchanged.
        ' * @return true if victim was found; otherwise false.
        ' */
        Public Shared Function skipString(ByVal text As String, ByVal victim As String, ByVal where As ParsePosition) As Boolean
            If (Text.StartsWith(victim, where.getIndex())) Then
                where.setIndex(where.getIndex() + victim.Length())
                Return True
            End If
            Return False
        End Function

        '/** 
        ' * Construct a new GregorianCalendar and set defaults.
        ' * Locale is ENGLISH.
        ' * TimeZone is "UTC" (zero offset and no DST).
        ' * Parsing is NOT lenient. Milliseconds are zero.
        ' * 
        ' * @return a new gregorian calendar
        ' */
        Public Shared Function newGreg() As NDate ' GregorianCalendar
            Dim retCal As NDate ' GregorianCalendar(Locale.ENGLISH)
            'retCal.Value .setTimeZone(New SimpleTimeZone(0, "UTC"))
            'retCal.setLenient(False)
            'retCal.setm set(Calendar.MILLISECOND, 0)
            Return retCal
        End Function

        '/**
        ' * Install a TimeZone on a GregorianCalendar without changing the 
        ' * hours value. A plain GregorianCalendat.setTimeZone() 
        ' * adjusts the Calendar.HOUR value to compensate. This is *BAD*
        ' * (not to say *EVIL*) when we have already set the time.
        ' * @param cal The GregorianCalendar whose TimeZone to change.
        ' * @param tz The new TimeZone.
        ' */
        Public Shared Sub adjustTimeZoneNicely(ByVal cal As NDate, ByVal tz As TimeZone)
            cal.setTimeZone(tz)
            Dim offset As Integer = (cal.get(Calendar.ZONE_OFFSET) + cal.get(Calendar.DST_OFFSET)) / MILLIS_PER_HOUR
            cal = Calendar.DateAdd(DateInterval.Hour, -offset, cal)
        End Sub

        '/**
        ' * Parses the end of a date string for a time zone and, if one is found,
        ' * sets the time zone of the GregorianCalendar. Otherwise the calendar 
        ' * time zone is unchanged.
        ' * 
        ' * The text is parsed as
        ' *      (Z|GMT|UTC)? [+- ]* h [': ]? m '?
        ' * where the leading String is optional, h is two digits by default, 
        ' * but may be a single digit if followed by one of space, apostrophe, 
        ' * colon, or the end of string. Similarly, m is one or two digits. 
        ' * This scheme accepts the format of PDF, RFC 822, and ISO8601. 
        ' * If none of these applies (as for a time zone name), we try
        ' * TimeZone.getTimeZone().
        ' * 
        ' * @param text The text expected to begin with a time zone value,
        ' * possibly with leading or trailing spaces.
        ' * @param cal The Calendar whose TimeZone to set. 
        ' * @param initialWhere where Scanning begins at where.index. After success, the returned
        ' *      index is that of the next character after the recognized string.
        ' *      The error index is ignored and unchanged.
        ' * @return true if parsed a time zone value; otherwise the 
        ' *      time zone is unchanged and the return value is false.
        ' */
        Public Shared Function parseTZoffset(ByVal text As String, ByVal cal As NDate, ByVal initialWhere As ParsePosition) As Boolean
            Dim where As New ParsePosition(initialWhere.getIndex())
            Dim tz As New SimpleTimeZone(0, "GMT")
            Dim tzHours, tzMin As Integer
            Dim sign As Char = skipOptionals(text, where, "Z+- ")
            Dim hadGMT As Boolean = (sign = "Z" OrElse skipString(text, "GMT", where) OrElse skipString(text, "UTC", where))
            sign = IIf(Not hadGMT, sign, skipOptionals(text, where, "+- "))

            tzHours = parseTimeField(text, where, 2, -999)
            skipOptionals(text, where, "\': ")
            tzMin = parseTimeField(text, where, 2, 0)
            skipOptionals(text, where, "\' ")

            If (tzHours <> -999) Then
                ' we parsed a time zone in default format
                Dim hrSign As Integer = IIf(sign = "-", -1, +1)
                tz.setRawOffset(restrainTZoffset(hrSign * (tzHours * MILLIS_PER_HOUR + tzMin * MILLIS_PER_MINUTE)))
                tz.setID("unknown")
            ElseIf (Not hadGMT) Then
                ' try to process as a name; "GMT" or "UTC" has already been processed
                Dim tzText As String = text.Substring(initialWhere.getIndex()).Trim()
                tz = TimeZone.getTimeZone(tzText)
                ' getTimeZone returns "GMT" for unknown ids
                If ("GMT".Equals(tz.getID())) Then
                    ' no timezone in text
                    ' cal amd initialWhere are unchanged
                    Return False
                Else
                    ' we got a tz by name; use it
                    where.setIndex(text.Length())
                End If
            End If
            adjustTimeZoneNicely(cal, tz)
            initialWhere.setIndex(where.getIndex())
            Return True
        End Function

        '/**
        ' * Parses a big-endian date: year month day hour min sec.
        ' * The year must be four digits. Other fields may be adjacent 
        ' * and delimited by length or they may follow appropriate delimiters.
        ' *     year [ -/]* month [ -/]* dayofmonth [ T]* hour [:] min [:] sec [.secFraction]
        ' * If any numeric field is omitted, all following fields must also be omitted.
        ' * No time zone is processed.
        ' * 
        ' * Ambiguous dates can produce unexpected results. For example:
        ' *      1970 12 23:08 will parse as 1970 December 23 00:08:00 
        ' * 
        ' * @param text The string to parse.
        ' * 
        ' * @param initialWhere Where to begin the parse. On return the index
        ' *      is advanced to just beyond the last character processed.
        ' *      The error index is ignored and unchanged.
        ' * 
        ' * @return a GregorianCalendar representing the parsed date. 
        ' *      Or null if the text did not begin with at least four digits.
        ' */
        Public Shared Function parseBigEndianDate(ByVal text As String, ByVal initialWhere As ParsePosition) As NDate ' GregorianCalendar
            Dim where As New ParsePosition(initialWhere.getIndex())
            Dim year As Integer = parseTimeField(text, where, 4, 0)
            If (where.getIndex() <> 4 + initialWhere.getIndex()) Then
                Return Nothing
            End If
            skipOptionals(text, where, "/- ")
            Dim month As Integer = parseTimeField(text, where, 2, 1) - 1 ' Calendar months are 0...11
            skipOptionals(text, where, "/- ")
            Dim day As Integer = parseTimeField(text, where, 2, 1)
            skipOptionals(text, where, " T")
            Dim hour As Integer = parseTimeField(text, where, 2, 0)
            skipOptionals(text, where, ": ")
            Dim minute As Integer = parseTimeField(text, where, 2, 0)
            skipOptionals(text, where, ": ")
            Dim second As Integer = parseTimeField(text, where, 2, 0)
            Dim nextC As Char = skipOptionals(text, where, ".")
            If (nextC = ".") Then
                ' fractions of a second: skip upto 19 digits
                parseTimeField(text, where, 19, 0)
            End If

            Dim dest As NDate ' GregorianCalendar = newGreg()
            Try
                dest = New Date(year, month + 1, day, hour, minute, second)
                'dest.getTimeInMillis() ' trigger limit tests
            Catch ill As ArgumentOutOfRangeException
                Return Nothing
            End Try
            initialWhere.setIndex(where.getIndex())
            skipOptionals(text, initialWhere, " ")
            Return dest ' dest has at least a year value
        End Function

        '/**
        ' * See if text can be parsed as a date according to any of a list of 
        ' * formats. The time zone may be included as part of the format, or
        ' * omitted in favor of later testing for a trailing time zone.
        ' * 
        ' * @param text The text to be parsed.
        ' * 
        ' * @param fmts A list of formats to be tried. The syntax is that for 
        ' *      {@link #java.text.SimpleDateFormat}
        ' * 
        ' * @param initialWhere At start this is the position to begin
        ' *      examining the text. Upon return it will have been
        ' *      incremented to refer to the next non-space character after the date.
        ' *      If no date was found, the value is unchanged.
        ' *      The error index is ignored and unchanged.
        ' * 
        ' * @return null for failure to find a date, or the GregorianCalendar
        ' *      for the date that was found. Unless a time zone was 
        ' *      part of the format, the time zone will be GMT+0
        ' */
        Public Shared Function parseSimpleDate(ByVal text As String, ByVal fmts() As String, ByVal initialWhere As ParsePosition) As NDate 'GregorianCalendar
            For Each fmt As String In fmts
                Dim where As New ParsePosition(initialWhere.getIndex())
                Dim sdf As New SimpleDateFormat(fmt, Locale.ENGLISH)
                Dim retCal As NDate = newGreg() 'GregorianCalendar
                sdf.setCalendar(retCal)
                If (sdf.parse(text, where) IsNot Nothing) Then
                    initialWhere.setIndex(where.getIndex())
                    skipOptionals(text, initialWhere, " ")
                    Return retCal
                End If
            Next
            Return Nothing
        End Function


        '/**
        ' * Parses a String to see if it begins with a date, and if so, 
        ' * returns that date. The date must be strictly correct--no 
        ' * field may exceed the appropriate limit.
        ' * (That is, the Calendar has setLenient(false).) 
        ' * Skips initial spaces, but does NOT check for "D:"
        ' * 
        ' * The scan first tries parseBigEndianDate and parseTZoffset
        ' * and then tries parseSimpleDate with appropriate formats, 
        ' * again followed by parseTZoffset. If at any stage the entire 
        ' * text is consumed, that date value is returned immediately. 
        ' * Otherwise the date that consumes the longest initial part
        ' * of the text is returned.
        ' * 
        ' * - PDF format dates are among those recognized by parseBigEndianDate.
        ' * - The formats tried are alphaStartFormats or digitStartFormat and
        ' * any listed in the value of moreFmts.
        ' * 
        ' * @param text The String that may begin with a date. Must not be null.
        ' *      Initial spaces and "D:" are skipped over.
        ' * @param moreFmts Additional formats to be tried after trying the
        ' *      built-in formats.
        ' * @param initialWhere where Parsing begins at the given position in text. If the
        ' *      parse succeeds, the index of where is advanced to point 
        ' *      to the first unrecognized character.
        ' *      The error index is ignored and unchanged.
        ' * @return A GregorianCalendar for the date. If no date is found, 
        ' *      returns null. The time zone will be GMT+0 unless parsing 
        ' *      succeeded with a format containing a time zone. (Only one
        ' *      builtin format contains a time zone.)
        ' * 
        ' */
        Public Shared Function parseDate(ByVal text As String, ByVal moreFmts() As String, ByVal initialWhere As ParsePosition) As NDate
            ' place to remember longestr date string
            Dim longestLen As Integer = -999999 ' theorem: this value will never be used
            ' proof: longestLen is only used if longestDate is not null
            Dim longestDate As NDate = Nothing ' null says no date found yet GregorianCalendar
            Dim whereLen As Integer ' tempcopy of where.getIndex()

            Dim where As New ParsePosition(initialWhere.getIndex())
            ' check for null (throws exception) and trim off surrounding spaces
            skipOptionals(text, where, " ")
            Dim startPosition As Integer = where.getIndex()

            ' try big-endian parse
            Dim retCal As NDate = parseBigEndianDate(text, where) 'GregorianCalendar
            ' check for success and a timezone
            If (retCal.HasValue AndAlso (where.getIndex() = text.Length() OrElse parseTZoffset(text, retCal, where))) Then
                ' if text is fully consumed, return the date
                ' else remember it and its length
                whereLen = where.getIndex()
                If (whereLen = text.Length()) Then
                    initialWhere.setIndex(whereLen)
                    Return retCal
                End If
                longestLen = whereLen
                longestDate = retCal
            End If

            ' try one of the sets of standard formats
            where.setIndex(startPosition)
            Dim formats() As String
            If NChar.isDigit(text.Chars(startPosition)) Then
                formats = DIGIT_START_FORMATS
            Else
                formats = ALPHA_START_FORMATS
            End If
            retCal = parseSimpleDate(text, formats, where)
            ' check for success and a timezone
            If (retCal.HasValue AndAlso (where.getIndex() = text.Length() OrElse parseTZoffset(text, retCal, where))) Then
                ' if text is fully consumed, return the date
                ' else remember it and its length
                whereLen = where.getIndex()
                If (whereLen = text.Length()) Then
                    initialWhere.setIndex(whereLen)
                    Return retCal
                End If
                If (whereLen > longestLen) Then
                    longestLen = whereLen
                    longestDate = retCal
                End If
            End If

            ' try the supplied formats
            If (moreFmts IsNot Nothing) Then
                where.setIndex(startPosition)
                retCal = parseSimpleDate(text, moreFmts, where)
                If (retCal.HasValue AndAlso (where.getIndex() = text.Length() OrElse parseTZoffset(text, retCal, where))) Then
                    whereLen = where.getIndex()
                    ' if text is fully consumed, return the date
                    'else remember it and its length
                    If (whereLen = text.Length() OrElse (longestDate.HasValue AndAlso whereLen > longestLen)) Then
                        initialWhere.setIndex(whereLen)
                        Return retCal
                    End If
                End If
            End If
            If (longestDate.HasValue) Then
                initialWhere.setIndex(longestLen)
                Return longestDate
            End If
            Return retCal
        End Function

        '/**
        ' * Converts a string to a Calendar by parsing the String for a date.
        ' * @see toCalendar(String).
        ' *
        ' * The returned value will have 0 for DST_OFFSET.
        ' * 
        ' * @param text The COSString representation of a date.
        ' * @return The Calendar that the text string represents. 
        ' *      Or null if text was null.
        ' * @throws IOException If the date string is not in the correct format.
        ' * @deprecated This method throws an IOException for failure. Replace
        ' *      calls to it with {@link #toCalendar(text.getString(), null)} 
        ' *      and test for failure with
        ' *          (value Is Nothing || value.get(Calendar.YEAR) == INVALID_YEAR)
        ' */
        Public Shared Function toCalendar(ByVal text As COSString) As NDate ' throws IOException
            If (text Is Nothing) Then Return Nothing
            Return toCalendar(text.getString())
        End Function

        '/**
        ' * Converts a string date to a Calendar date value; equivalent to 
        ' * {@link #toCalendar(String, null)}, 
        ' * but throws an IOException for failure.
        ' * 
        ' * The returned value will have 0 for DST_OFFSET.
        ' * 
        ' * @param text The string representation of the calendar.
        ' * @return The Calendar that this string represents 
        ' *      or null if the incoming text is null.
        ' * @throws IOException If the date string is non-null 
        ' *      and not a parseable date.
        ' * @deprecated This method throws an IOException for failure. Replace
        ' *      calls to it with {@link #toCalendar(text, null)} 
        ' *      and test for failure with
        ' *          (value Is Nothing || value.get(Calendar.YEAR) == INVALID_YEAR)
        ' */
        Public Shared Function toCalendar(ByVal text As String) As NDate ' throws IOException
            If (text = "") Then Return Nothing
            Dim val As NDate = toCalendar(text, Nothing)
            If (val.HasValue AndAlso Year(val) = INVALID_YEAR) Then
                Throw New ArgumentOutOfRangeException("Error converting date: " & text)
            End If
            Return val
        End Function

        '/**
        ' * Converts a string to a calendar. The entire string must be consumed.
        ' * The date must be strictly correct; that is, no field may exceed
        ' * the appropriate limit. Uses {@link #parseDate} to do the actual parsing.
        ' * 
        ' * The returned value will have 0 for DST_OFFSET.
        ' * 
        ' * @param text The text to parse. Initial spaces and "D:" are skipped over.
        ' * @param moreFmts An Array of formats (as Strings) to try 
        ' *      in addition to the standard list.
        ' * @return the Calendar value corresponding to the date text. 
        ' *      If text does not represent a valid date, 
        ' *      the value is January 1 on year INVALID_YEAR at 0:0:0 GMT.
        ' * 
        ' */
        Public Shared Function toCalendar(ByVal text As String, ByVal moreFmts() As String) As NDate
            Dim where As New ParsePosition(0)
            skipOptionals(text, where, " ")
            skipString(text, "D:", where)
            Dim retCal As NDate = parseDate(text, moreFmts, where) ' PARSE THE TEXT
            If (retCal.HasValue = False OrElse where.getIndex() <> text.Length()) Then
                ' the date string is invalid for all formats we tried,
                'retCal = newGreg()
                'retCal.set(INVALID_YEAR, 0, 1, 0, 0, 0)
                retCal = New Date(INVALID_YEAR, 1, 1)
            End If
            Return retCal
        End Function

    End Class

End Namespace
