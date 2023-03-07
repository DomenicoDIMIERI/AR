Imports System.IO.Compression

Namespace PDF


    '    'function CreateJsObject(s){return eval('new '+s);}
    '    Public Class cfile
    '        Implements IDisposable

    '        Private m_FileName As String = vbNullString
    '        Private obj As System.IO.Stream = Nothing
    '        Private mode As String = ""
    '        Private isBinary As Boolean = False

    '        Public Sub New()
    '        End Sub

    '        Public Sub Open(ByVal stream As System.IO.Stream, Optional ByVal isBinary As Boolean = True)
    '            Me.obj = stream
    '            Me.mode = ""
    '            Me.isBinary = isBinary
    '        End Sub

    '        Public Sub Open(ByVal fileName As String, ByVal params As String)
    '            'Dim v As Integer
    '            'Dim c As Boolean
    '            'Dim p As String
    '            'Select Case (params.Chars(0))
    '            '    Case "r" : v = 1 : c = False : p = name
    '            '    Case "w" : v = 2 : c = True : p = name
    '            '    Case "a" : v = 8 : c = True : p = name
    '            'End Select
    '            Me.m_FileName = fileName
    '            Me.mode = params
    '            Me.obj = New System.IO.FileStream(fileName, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)
    '        End Sub

    '#Region "IDisposable Support"
    '        Private disposedValue As Boolean ' To detect redundant calls

    '        ' IDisposable
    '        Protected Overridable Sub Dispose(disposing As Boolean)
    '            If Not Me.disposedValue Then
    '                If disposing Then
    '                    ' TODO: dispose managed state (managed objects).
    '                End If

    '            End If
    '            If Me.obj IsNot Nothing Then Me.obj.Dispose()
    '            Me.obj = Nothing
    '            Me.disposedValue = True
    '        End Sub

    '        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    '        'Protected Overrides Sub Finalize()
    '        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '        '    Dispose(False)
    '        '    MyBase.Finalize()
    '        'End Sub

    '        ' This code added by Visual Basic to correctly implement the disposable pattern.
    '        Public Sub Dispose() Implements IDisposable.Dispose
    '            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '            Dispose(True)
    '            GC.SuppressFinalize(Me)
    '        End Sub
    '#End Region

    '    End Class

    Public NotInheritable Class [lib]
        'Me.fso = new ActiveXObject("Scripting.FileSystemObject");
        'Public Shared Function empty(ByVal s As Object) As Boolean
        '    Return TypeOf (s) Is DBNull Or s Is Nothing
        'End Function

        'Public Shared Function ord(ByVal ch As String) As Integer
        '    Return Asc(ch.Chars(0))
        'End Function

        'Public Shared Function count(ByVal ar As Array) As Integer
        '    Dim i As Integer = 0
        '    For Each k As Object In ar
        '        i += 1
        '    Next
        '    Return i
        'End Function

        'Public Shared Function strlen(ByVal s As String) As Integer
        '    Return Len(s) '{s1 = new String(s);return s1.length;}
        'End Function

        'Public Shared Function chr(ByVal value As Integer) As String
        '    Return Microsoft.VisualBasic.Chr(value) 'String.fromCharCode(value)}
        'End Function

        'Public Shared Sub die(ByVal s As String)
        '    Throw New Exception(s)
        'End Sub

        Public Shared Function basename(ByVal s As String) As String
            Dim i As Integer = s.LastIndexOf("/")
            If (i < 0) Then i = 0
            Return s.Substring(i, s.Length)
        End Function

        'Public Shared Function fopen(ByVal name As String, ByVal params As String) As cfile
        '    '           Dim f As New cfile(name)
        '    '           f.Open(params)
        '    '		f.mode=params.charAt(0);
        '    'if (params.length>1){if (params.charAt(1)=="b"){f.isBinary = true}};
        '    'return f;
        '    Dim f As New cfile
        '    f.Open(name, params)
        '    Return f
        'End Function

        Public Shared Function eregi(ByVal r As String, ByVal s As String) As Boolean
            Dim re As New System.Text.RegularExpressions.Regex(r, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Return re.IsMatch(s)
        End Function

        Public Shared Function explode(ByVal ch As String, ByVal s As String) As String()
            Return s.Split(ch)
        End Function

        Public Shared Function explode(ByVal ch As Char, ByVal s As String, ByVal limit As Integer) As String()
            Return s.Split(New Char() {ch}, limit)
        End Function

        'Public Shared Function ltrim(ByVal s As String) As String
        '    'return s.replace(/^\s+/,"")
        '    Return s.TrimStart
        'End Function

        'Public Shared Function trim(ByVal s As String) As String
        '    'return s.replace(/\s+$|^\s+/g,"")
        '    Return s.Trim
        'End Function

        'Public Shared Function rtrim(ByVal s As String) As String
        '    '   ns = new String(s);
        '    'return ns.replace(/\s+$/,"");
        '    Return s.TrimEnd
        'End Function

        '        Me.file = function(path) {
        '    var f;
        '    var ar = new Array();
        '    try {
        '        f = Me.fso.OpenTextFile(Server.MapPath(path), 1);
        '        while (!f.atEndOfStream) {
        '            ar[ar.length] = f.ReadLine();
        '        };
        '        f.close();
        '        return ar;
        '    } catch (e) {
        '        Me.die("Error, path not found : " + path);
        '    }
        '}

        'Me.fwrite=function fwrite(f,buffer){
        '    try { f.obj.write(buffer); }
        '	catch(e){return e.number;}
        '	return true;
        '}

        'Me.fread=function fread(f,nch){
        '    try { f.obj.read(nch); }
        '	catch(e){return e.number;}
        '	return true;
        '}

        'Me.fclose=function fclose(f){
        '    try { f.obj.close(); }
        '	catch(e){return e.number;}
        '	return true;
        '}

        'Me.substr=function substr(){
        '	var i;var s;
        '	s = new String(arguments(0))
        '	if (arguments.length==2){
        '		e=s.length;
        '		i=(arguments(1)<0?s.length+arguments(1):arguments(1))
        '		}
        '	else{
        '	    i = arguments(1);
        '		e=(arguments(2)<0?s.length+arguments(2):arguments(2))
        '		}

        '                                    Return s.substr(i, e)
        '}
        'Me.strrpos=function strrpos(s,ch){
        '    res = s.lastIndexOf(ch);
        '	if (res>0-1){return res}else{return false}
        '}
        'Me.strpos=function strpos(s,ch,start){
        '	if (arguments.length<3){start=0}
        '	res = s.indexOf(ch,start);
        '	if (res>-1){return res}else{return false}
        '}
        'Me.is_int=function is_int(v){
        '	try{
        '	    res = !isNaN(parseInt(v));
        '	}
        '    catch (e) { res = false; }
        '	return res;
        '}
        'Me.is_string=function is_string(s){
        '	try{
        '	res=isNaN(parseInt(s))}
        '	catch(e){res=false}
        '	return res;
        '}
        'Me.is_array=function is_array(o){
        '	try{t=(o.constructor==Array);}
        '	catch(e){t=false}
        '	finally{return t}
        '}
        Public Shared Function [date](ByVal s As String) As String
            Dim r As String = ""
            Dim d As Date = Now
            For i As Integer = 1 To s.Length
                Select Case Mid(s, i, 1)
                    Case "Y" : r &= d.Year()
                    Case "m" : r &= Right("00" & d.Month, 2)
                    Case "d" : r &= Right("00" & d.Day, 2)
                    Case "H" : r &= Right("00" & d.Hour, 2)
                    Case "i" : r &= Right("00" & d.Minute, 2)
                    Case "s" : r &= Right("00" & d.Second, 2)
                End Select
            Next
            Return r
        End Function
        'Me.str_replace=function str_replace(psearchText,preplaceText,poriginalString){

        '                                                                searchText = New String(psearchText)
        '                                                                replaceText = New String(preplaceText)
        '                                                                originalString = New String(poriginalString)

        '	var strLength = originalString.length;
        '	var txtLength = searchText.length;
        '	if ((strLength == 0) || (txtLength == 0))
        '	{ return originalString; }
        '	var i = originalString.indexOf(searchText);
        '	if ((!i) && (searchText != originalString.substring(0,txtLength)))
        '	{ return originalString; }
        '	if (i == -1)
        '	{ return originalString; }
        '	var newstr = originalString.substring(0,i) + replaceText;
        '	if (i+txtLength < strLength) { newstr += Me.str_replace(searchText,replaceText,originalString.substring(i+txtLength,strLength)); }
        '	return newstr;
        '}

        'Me.str_replace1=function str_replace1(psearchText,preplaceText,poriginalString){
        '                                                                                originalString = New String(poriginalString)
        '	s = 'new RegExp("' + psearchText + '","gi")'
        '	Response.Write(s);
        '	Response.End;
        '	re = eval(s);
        '                                                                                Return originalString.replace(re, preplaceText)
        '}
        'Me.substr_count=function substr_count(s,ch){
        '	ar = s.split(ch);
        '	return ar.length;
        '}
        'Me.isset=function isset(s){if(s){return true}else{return false}}
        'Me.function_exists=function function_exists(s){
        '	if(s="gzcompress"){return false};
        '}

        'Me.getimagesize=function getimagesize(){Response.Write("getimagesize");Response.End;}
        'Me.imagesx=function imagesx(){Response.Write("imagex");Response.End;}
        'Me.imagesy=function imagesy(){Response.Write("imagey");Response.End;}
        'Me.tempnam=function tempnam(){Response.Write("temname");Response.End;}
        'Me.imagejpeg=function imagejpeg(){Response.Write("imagjpg");Response.End;}
        'Me.scalar_array=function scalar_test(ar){
        '	var i;
        '	s='ar';tmp='';
        '	for(i=0;i<arguments.length;i++){
        '			if(i==0){s="ar";}
        '                                                                                        Else
        '			{
        '			tmp = ( typeof(arguments(i))=="number" ? arguments(i) : "\"" + arguments(i) +"\"");
        '			s +=  "[" + tmp + "]" ;
        '			}
        '			o=eval(s);
        '			if (!Me.is_array(o)){
        '				eval(s + "=new Array()");
        '			}
        '	}
        '	return;
        '}
        'Me.newArray=function newArray(){
        '	var i;
        '	var ar=new Array();
        '	for(i=0;i<arguments.length;i++){
        '		ar[arguments(i)]=arguments[i+1];i=i+1
        '	}
        '	return ar;
        '}
        'Me.file_exists = function (path) {
        '    res = Me.fso.FileExists(Server.MapPath(path));
        '    return res;
        '}

        'Me.readtextfile=function readtextfile(path){
        '    var f, res;
        '    if (Me.file_exists(path)) {
        '        f = Me.fso.OpenTextFile(Server.MapPath(path), 1);
        '        res = f.ReadAll();
        '        f.close();
        '    } else {
        '        die("Path Not Found : " + Server.MapPath(path));
        '    }
        '	return res;
        '}

        'Me.readbinfile=function readbinfile(path){
        '    var f, res;
        '    f = Server.CreateObject("ADODB.Stream");
        '    f.CharSet = "ISO-8859-1";
        '    f.Type = 2;
        '    f.Open();
        '    f.LoadFromFile(Server.MapPath(path));
        '    f.Position = 0;
        '    res = f.ReadText();
        '    f.Close();
        '    return res;
        '}

        'Me.filesize=function filesize(path){
        '    if (!Me.file_exists(path)) { return false; }
        '    return Me.fso.getFile(Server.MapPath(path)).size;
        '}

        'Me.printf = function printf(format) {
        '   document.write(_spr(format, arguments));
        '}


        Public Shared Function sprintf(ByVal format As String, ByVal ParamArray arguments() As Object) As String
            Dim i As Integer
            Dim ret As String = vbNullString
            Dim ch As String
            Dim status As Integer = 0
            Dim j As Integer = 0
            Dim token As String = vbNullString
            Dim spaces As Integer
            Dim subformat As String
            i = 1
            While i <= Len(format)
                Select Case status
                    Case 0
                        ch = Mid(format, i, 1)
                        i += 1
                        Select Case ch
                            Case "%"    'Si tratta di un codice di formattazione
                                status = 2
                                token = vbNullString
                            Case "\"    'Si tratta di un codice escape
                                status = 1
                            Case Else
                                ret &= ch
                        End Select
                    Case 1 'Do escape char \
                        ch = Mid(format, i, 1)
                        i += 1
                        Select Case ch
                            Case "n" : ret &= vbLf
                            Case "r" : ret &= vbCr
                            Case "t" : ret &= vbTab
                            Case Else
                                ret &= ch
                        End Select
                        status = 0
                    Case 2
                        ch = Mid(format, i, 1)
                        i += 1
                        Select Case ch
                            Case "l", "c", "d", "f", "o", "x", "#", "s", "*", ".", "+", "-"
                                token &= ch
                            Case Else
                                If (ch >= "0") And (ch <= "9") Then
                                    token &= ch
                                Else
                                    If (token <> vbNullString) Then
                                        i -= 1
                                        status = 3
                                    Else
                                        Throw New FormatException
                                    End If
                                End If
                        End Select
                    Case 3
                        ch = Left(token, 1)
                        spaces = 0
                        subformat = vbNullString
                        If (ch >= "1") And (ch <= "9") Then
                            subformat = Left(token, Len(token) - 1)
                            token = Right(token, 1)
                        ElseIf (ch = "0") Then
                            subformat = Mid(token, 2, Len(token) - 2)
                            token = Right(token, 1)
                        ElseIf ch = "+" Then
                            token = Right(token, 1)
                        ElseIf ch = "." Then
                            subformat = "0" & Left(token, Len(token) - 1)
                            token = Right(token, 1)
                        End If

                        Select Case token
                            Case "c"
                                If TypeOf (arguments(j)) Is String Then
                                    ret &= Left(arguments(j), 1)
                                Else
                                    ret &= Chr(CInt(arguments(j)))
                                End If
                                j += 1
                                status = 0
                            Case "d", "ld"
                                If (ch = "0") Then
                                    spaces = -CInt(subformat)
                                Else
                                    spaces = CInt(subformat)
                                End If
                                If spaces > 0 Then
                                    ret &= Right(Space(spaces) & CDec(arguments(j)), spaces)
                                ElseIf spaces < 0 Then
                                    ret &= Right(Sistema.Strings.NChars(-spaces, "0") & CDec(arguments(j)), -spaces)
                                Else
                                    ret &= CStr(CDec(arguments(j)))
                                End If
                                j += 1
                                status = 0
                            Case "s"
                                ret &= CStr(arguments(j))
                                j += 1
                                status = 0
                            Case "f"
                                Dim p As Integer = InStr(subformat, ".")
                                Dim lenNum, numDec As Integer
                                If (p > 0) Then
                                    lenNum = CInt(Left(subformat, p - 1))
                                    numDec = CInt(Mid(subformat, p + 1))
                                Else
                                    lenNum = CInt(subformat)
                                    numDec = 0
                                End If
                                If (lenNum > 0) Then
                                    ret &= Replace(Strings.Right(Space(lenNum), FormatNumber(CDbl(arguments(j)), numDec)), ",", ".")
                                Else
                                    ret &= Replace(FormatNumber(CDbl(arguments(j)), numDec), ",", ".")
                                End If
                                j += 1
                                status = 0
                            Case Else
                                Throw New FormatException
                        End Select
                    Case Else
                        Throw New InvalidOperationException
                End Select
            End While
            Select Case status
                Case 0
                Case 1 'Do escape char \
                    Throw New FormatException
                Case 2, 3
                    ch = Left(token, 1)
                    spaces = 0
                    subformat = vbNullString
                    If (ch >= "1") And (ch <= "9") Then
                        subformat = Left(token, Len(token) - 1)
                        token = Right(token, 1)
                    ElseIf (ch = "0") Then
                        subformat = Mid(token, 2, Len(token) - 2)
                        token = Right(token, 1)
                    ElseIf ch = "+" Then
                        token = Right(token, 1)
                    ElseIf ch = "." Then
                        subformat = "0" & Left(token, Len(token) - 1)
                        token = Right(token, 1)
                    End If

                    Select Case token
                        Case "c"
                            If TypeOf (arguments(j)) Is String Then
                                ret &= Left(arguments(j), 1)
                            Else
                                ret &= Chr(CInt(arguments(j)))
                            End If
                            j += 1
                            status = 0
                        Case "d", "ld"
                            If (ch = "0") Then
                                spaces = -CInt(subformat)
                            Else
                                spaces = CInt(subformat)
                            End If
                            If spaces > 0 Then
                                ret &= Right(Space(spaces) & CDec(arguments(j)), spaces)
                            ElseIf spaces < 0 Then
                                ret &= Right(Sistema.Strings.NChars(-spaces, "0") & CDec(arguments(j)), -spaces)
                            Else
                                ret &= CStr(CDec(arguments(j)))
                            End If
                            j += 1
                            status = 0
                        Case "s"
                            ret &= CStr(arguments(j))
                            j += 1
                            status = 0
                        Case "f"
                            Dim p As Integer = InStr(subformat, ".")
                            Dim lenNum, numDec As Integer
                            If (p > 0) Then
                                lenNum = CInt(Left(subformat, p - 1))
                                numDec = CInt(Mid(subformat, p + 1))
                            Else
                                lenNum = CInt(subformat)
                                numDec = 0
                            End If
                            If (lenNum > 0) Then
                                ret &= Replace(Strings.Right(Space(lenNum), FormatNumber(CDbl(arguments(j)), numDec)), ",", ".")
                            Else
                                ret &= Replace(FormatNumber(CDbl(arguments(j)), numDec), ",", ".")
                            End If
                            j += 1
                            status = 0
                        Case Else
                            Throw New FormatException
                    End Select
                Case Else
                    Throw New InvalidOperationException
            End Select
            Return ret
        End Function

        'Me.SaveToFile=function SaveToFile(filename,buffer){
        '	var f;
        '                                                                                                            f = Me.fso.OpenTextFile(Server.MapPath(filename), 2, True)
        '	f.write(buffer);
        '	f.close();
        '}

        'Me._spr=function _spr(format, args) {
        '   function isdigit(c) {
        '      return (c <= "9") && (c >= "0");
        '   }

        '   function rep(c, n) {
        '      var s = "";
        '            While (--n >= 0)
        '         s += c;
        '      return s;
        '   }

        '   var c;
        '   var i, ii, j = 1;
        '   var retstr = "";
        '   var space = "&nbsp;";


        '   for (i = 0; i < format.length; i++) {
        '      var buf = "";
        '      var segno = "";
        '      var expx = "";
        '      c = format.charAt(i);
        '      if (c == "\n") {
        '         c = "<br>";
        '      }
        '      if (c == "%") {
        '         i++;
        '         leftjust = false;
        '         if (format.charAt(i) == '-') {
        '            i++;
        '            leftjust = true;
        '         }
        '         padch = ((c = format.charAt(i)) == "0") ? "0" : space;
        '         if (c == "0")
        '            i++;
        '         field = 0;
        '         if (isdigit(c)) {
        '            field = parseInt(format.substring(i));
        '            i += String(field).length;
        '         }

        '         if ((c = format.charAt(i)) == '.') {
        '            digits = parseInt(format.substring(++i));
        '            i += String(digits).length;
        '            c = format.charAt(i);
        '         }
        '                                        Else
        '            digits = 0;

        '         switch (c.toLowerCase()) {
        '            case "x":
        '               buf = args[j++].toString(16);
        '               break;
        '            case "e":
        '               expx = -1;
        '            case "f":
        '            case "d":
        '               if (args[j] < 0) {
        '                  args[j] = -args[j];
        '                  segno = "-";
        '                  field--;
        '               }
        '               if (expx != "") {
        '                                                    With (Math)
        '                     expx = floor(log(args[j]) / LN10);
        '                  args[j] /= Number("1E" + expx);
        '                  field -= String(expx).length + 2;
        '               }
        '               var x = args[j++];
        '               for (ii=0; ii < digits && x - Math.floor(x); ii++)
        '                  x *= 10;

        '               x = String(Math.round(x));

        '               x = rep("0", ii - x.length + 1) + x;

        '               buf += x.substring(0, x.length - ii);

        '                                                            If (digits > 0) Then
        '                  buf += "." + x.substring(x.length - ii) + rep("0", digits - ii);
        '               if (expx != "") {
        '                  var expsign = (expx >= 0) ? "+" : "-";
        '                  expx = Math.abs(expx) + "";
        '                  buf += c + expsign + rep("0", 3 - expx.length) + expx;
        '               }
        '               break;
        '            case "o":
        '               buf = args[j++].toString(8);
        '               break;
        '            case "s":
        '               buf = args[j++];
        '               break;
        '            case "c":
        '               buf = args[j++].substring(0, 1);
        '               break;
        '            default:
        '               retstr += c;
        '         }
        '         field -= buf.length;
        '         if (!leftjust) {
        '            if (padch == space)
        '               retstr += rep(padch, field) + segno;
        '                                                                        Else
        '               retstr += segno + rep("0", field);
        '         }
        '         retstr += buf;
        '                                                                            If (leftjust) Then
        '            retstr += rep(space, field);
        '      }
        '                                                                            Else
        '         retstr += c;
        '   }
        '   return retstr;
        '}
        '}
        Public Shared Function gzcompress(ByVal buffer As String) As String
            Dim stream As New System.IO.MemoryStream()
            stream.Write(New Byte() {&H1F, &H8B, &H8, &H0, &H0, &H0, &H0, &H0}, 0, 8)

            Dim gzip As New GZipStream(stream, CompressionMode.Compress)

            Dim bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(buffer)
            gzip.Write(bytes, 0, 1 + UBound(bytes))
            gzip.Close()

            bytes = stream.ToArray

            stream.Dispose()

            Return System.Text.Encoding.UTF8.GetString(bytes)
        End Function


    End Class

   
End Namespace