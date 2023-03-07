Imports DMD.Sistema
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.XML.Utils
Imports System.Web

Namespace XML



    Public Class XMLSerializer
        Private m_Encoding As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Encoding = "utf-8" '"windows-1252""ISO-8859-1" '
        End Sub

        Public Property Encoding As String
            Get
                Return Me.m_Encoding
            End Get
            Set(value As String)
                Me.m_Encoding = value
            End Set
        End Property


        Private Function getNextElementSibling(ByVal elem As System.Xml.XmlNode) As System.Xml.XmlNode
            Dim n As System.Xml.XmlNode
            n = elem.NextSibling
            While Not (n Is Nothing)
                If (n.NodeType = 1) Then
                    Return n
                End If
                n = n.NextSibling
            End While
            Return n
        End Function

        Private Function getFirstElementChild(ByVal elem As System.Xml.XmlNode) As System.Xml.XmlNode
            Dim n As System.Xml.XmlNode
            n = elem.FirstChild
            While Not (n Is Nothing)
                If (n.NodeType = 1) Then
                    Return n
                End If
                n = n.NextSibling
            End While
            Return n
        End Function

        Private Function getNodeText(ByVal node As System.Xml.XmlNode) As Object
            If (node.InnerText = "") Then
                Return Nothing
            Else
                Return node.InnerText
            End If
        End Function

        Public Function GetInnerTAG(ByVal text As String, ByVal tagName As String) As String
            Dim i, j, k As Integer
            Dim ret As String
            ret = ""
            i = InStr(text, "<" & tagName)
            If (i > 0) Then
                j = InStr(i + Len(tagName), text, ">")
                If (j > i) Then
                    k = InStr(j + 1, text, "</" & tagName & ">")
                    If (k <= j) Then
                        ret = Mid(text, j + 1)
                    Else
                        ret = Mid(text, j + 1, k - j - 1)
                    End If
                End If
            End If
            Return ret
        End Function



        'Public Overridable Function Serialize1(ByVal obj As Object, ByVal method As XMLSerializeMethod) As String
        '    Dim writer As New DMD.XML.XMLWriter
        '    writer.BeginDocument(method, obj)
        '    writer.Write(obj)
        '    writer.EndDocument()
        '    Return writer.ToString
        'End Function

        Public Overridable Function Serialize(ByVal obj As Object) As String
            Return Serialize(obj, XMLSerializeMethod.Document)
        End Function

        Public Overridable Function Serialize(ByVal obj As Object, ByVal method As XMLSerializeMethod) As String
            Dim writer As New XMLWriter
            Dim ret As String = Serialize(obj, method, writer)
            'writer.Dispose()
            Return ret
        End Function

        Public Overridable Function Serialize(ByVal obj As Object, ByVal method As XMLSerializeMethod, ByVal writer As XMLWriter) As String
            If (method = XMLSerializeMethod.Document) Then
                writer.WriteRowData("<?xml version=""1.0"" encoding=""" & Me.m_Encoding & """?>" & vbNewLine)
                If (IsArray(obj) AndAlso Arrays.Len(obj) > 0) Then
                    Dim arr As Array = obj
                    writer.BeginTag("ArrayOf" & XMLTypeName(arr.GetValue(0)))
                    writer.WriteAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
                    writer.WriteAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema")
                Else
                    writer.BeginTag(XMLTypeName(obj))
                    writer.WriteAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
                    writer.WriteAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema")
                End If
                If (TypeOf (obj) Is DMD.XML.IDMDXMLSerializable) Then
                    DirectCast(obj, DMD.XML.IDMDXMLSerializable).XMLSerialize(writer)
                Else
                    writer.Write(obj)
                End If
                writer.EndTag() '+= "</ArrayOf" + vbTypeName(obj[0]) + ">\n";
            Else
                writer.Write(obj)
            End If

            Return writer.ToString()
        End Function



        Public Overridable Function Deserialize(ByVal text As String) As Object
            Dim ret As Object
            Dim xmlDoc As New System.Xml.XmlDocument
            Dim node As System.Xml.XmlNode
            Dim n As System.Xml.XmlNode
            xmlDoc.LoadXml(text)
            node = xmlDoc.DocumentElement
            If (Left(node.Name, 7) = "ArrayOf") Then
                Dim tmp As New CCollection
                n = getFirstElementChild(xmlDoc.DocumentElement)
                While Not (n Is Nothing)
                    Dim rItem As Object
                    Select Case n.Name
                        Case "String" : rItem = CStr(getNodeText(n))
                        Case "Int32", "Integer" : rItem = XML.Utils.Serializer.DeserializeInteger(getNodeText(n))
                        Case "Byte", "SByte" : rItem = XML.Utils.Serializer.DeserializeInteger(getNodeText(n))
                        Case "Int64", "Long" : rItem = XML.Utils.Serializer.DeserializeLong(getNodeText(n))
                        Case "Int16", "Short" : rItem = XML.Utils.Serializer.DeserializeInteger(getNodeText(n))
                        Case "Single", "Double" : rItem = XML.Utils.Serializer.DeserializeDouble(getNodeText(n))
                        Case "Decimal" : rItem = XML.Utils.Serializer.DeserializeDouble(getNodeText(n))
                        Case "Boolean" : rItem = XML.Utils.Serializer.DeserializeBoolean(getNodeText(n))
                        Case Else
                            rItem = Types.CreateInstance(n.Name)
                            XML.Utils.Serializer.DeserializeInline(n, rItem)
                    End Select
                    tmp.Add(rItem)
                    n = getNextElementSibling(n)
                End While
                ret = tmp.ToArray
            Else
                Dim tpName As String = xmlDoc.DocumentElement.Name
                ret = Types.CreateInstance(tpName)
                If (ret IsNot Nothing) Then XML.Utils.Serializer.DeserializeInline(xmlDoc.DocumentElement, ret)
            End If


            xmlDoc = Nothing
#If DEBUG Then
            If (TypeOf (ret) Is DBObjectCursorBase) Then
                Debug.Print("Cursore")
            End If
#End If

            Return ret
        End Function

        Public Overridable Function Deserialize(ByVal text As String, ByVal type As System.Type) As Object
            Dim x As New System.Xml.Serialization.XmlSerializer(type)
            Dim sr As New System.IO.StringReader(text)
            Return x.Deserialize(sr)
        End Function

        Public Overridable Function Deserialize(ByVal text As String, ByVal tpName As String) As Object
            'Return Deserialize(text, Sistema.GetType(tpName))
            Dim ret As Object
            Dim xmlDoc As New System.Xml.XmlDocument
            Dim node As System.Xml.XmlNode
            Dim n As System.Xml.XmlNode
            'xmlDoc.async= "false"
            xmlDoc.LoadXml(text)
            node = xmlDoc.DocumentElement
            If (node.Name = "ArrayOf" & tpName) Then
                Dim tmp As New CCollection
                n = getFirstElementChild(xmlDoc.DocumentElement)
                While Not (n Is Nothing)
                    Dim rItem As Object
                    rItem = Types.CreateInstance(n.Name)
                    XML.Utils.Serializer.DeserializeInline(n, rItem)
                    tmp.Add(rItem)
                    n = getNextElementSibling(n)
                End While
                ret = tmp.ToArray
            Else
                ret = Types.CreateInstance(tpName)
                If (ret IsNot Nothing) Then XML.Utils.Serializer.DeserializeInline(xmlDoc.DocumentElement, ret)
            End If
            xmlDoc = Nothing
#If DEBUG Then
            If (TypeOf (ret) Is DBObjectCursorBase) Then
                Debug.Print("Cursore")
            End If
#End If
            Return ret
        End Function

        Private Function DeserializeNode(ByVal cn As System.Xml.XmlNode) As Object
            Dim item As Object
            Select Case (cn.Name)
                Case "Nothing", "Null", "NULL"
                    If cn.FirstChild IsNot Nothing AndAlso cn.FirstChild.Name = cn.Name Then
                        Return New Object() {Nothing}
                    Else
                        item = Nothing
                    End If
                Case "String", "string" : item = Strings.CStr(getNodeText(cn))
                Case "Number", "number" : item = XML.Utils.Serializer.DeserializeDouble(getNodeText(cn))
                Case "Boolean", "boolean", "bool", "Bool" : item = XML.Utils.Serializer.DeserializeBoolean(getNodeText(cn))
                Case "Byte", "SByte", "int8", "uint8" : item = XML.Utils.Serializer.DeserializeInteger(getNodeText(cn))
                Case "Short", "Int16", "short" : item = XML.Utils.Serializer.DeserializeInteger(getNodeText(cn))
                Case "UShort", "UInt16", "unsigned short" : item = XML.Utils.Serializer.DeserializeInteger(getNodeText(cn))
                Case "Integer", "int", "Int32" : item = XML.Utils.Serializer.DeserializeInteger(getNodeText(cn))
                Case "UInteger", "unsigned int", "UInt32" : item = XML.Utils.Serializer.DeserializeInteger(getNodeText(cn))
                Case "Long", "long", "Ing64" : item = XML.Utils.Serializer.DeserializeLong(getNodeText(cn))
                Case "ULong", "unsigned long", "UIng64" : item = XML.Utils.Serializer.DeserializeInteger(getNodeText(cn))
                Case "Double", "Single", "Decimal" : item = XML.Utils.Serializer.DeserializeDouble(getNodeText(cn))
                Case "Date", "DateTime" : item = XML.Utils.Serializer.DeserializeDate(getNodeText(cn))
                Case Else
                    item = Types.CreateInstance(cn.Name)
                    XML.Utils.Serializer.DeserializeInline(cn, item)
            End Select
#If DEBUG Then
            If (TypeOf (item) Is DBObjectCursorBase) Then
                Debug.Print("Cursore")
            End If
#End If
            Return item
        End Function

        Public Overridable Sub DeserializeInline(ByRef currNode As System.Xml.XmlNode, ByRef targetObject As IDMDXMLSerializable)
            Dim node As System.Xml.XmlNode
            Dim cn As System.Xml.XmlNode
            Dim items As CCollection
            Dim item As Object


            For Each attr As System.Xml.XmlAttribute In currNode.Attributes
                targetObject.SetFieldInternal(attr.Name, attr.Value)
            Next

            node = getFirstElementChild(currNode)
            While Not (node Is Nothing)
                cn = getFirstElementChild(node)
                If (cn IsNot Nothing) Then
                    If (getNextElementSibling(cn) Is Nothing) Then
                        item = Me.DeserializeNode(cn)
                        targetObject.SetFieldInternal(node.Name, item)
                    Else
                        items = New CCollection
                        While Not (cn Is Nothing)
                            item = Me.DeserializeNode(cn)
                            items.Add(item)
                            cn = getNextElementSibling(cn)
                        End While
                        targetObject.SetFieldInternal(node.Name, items.ToArray)
                    End If
                Else
                    targetObject.SetFieldInternal(node.Name, getNodeText(node))
                End If
                node = getNextElementSibling(node)
            End While
        End Sub

        'Public  Function FieldSerialize(ByVal value As Object) As String
        '    If IsArray(value) Then
        '    ElseIf IsObject(value) Then
        '    ElseIf LCase(TypeName(value)) = "string" Then
        '        FieldSerialize = Server.HTMLEncode(value)
        '    ElseIf IsDate(value) Then
        '        FieldSerialize = Server.HTMLEncode(Year(value) & "/" & Month(value) & "/" & Day(value) & " " & Hour(value) & "." & Minute(value) & "." & Second(value))
        '    Else
        '        FieldSerialize = Server.HTMLEncode("" & value)
        '    End If
        'End Function

        Public Function DeserializeDate(ByVal value As Object) As Nullable(Of Date)
            Dim text As String = Trim(Strings.CStr(value))
            If text = "" Then Return Nothing
            Dim n() As String
            Dim d() As String
            Dim t() As String
            Dim ret As Date
            n = Split(text, " ")
            d = Split(n(0), "/")
            t = Split(n(1), ".")
            ret = Calendar.MakeDate(d(0), d(1), d(2), t(0), t(1), t(2))
            Return ret
        End Function

        Public Function DeserializeBoolean(ByVal value As Object) As Nullable(Of Boolean)
            Dim text As String = Trim(Strings.CStr(value))
            If (text = vbNullString) Then Return Nothing
            text = UCase(Left(Trim(text), 1))
            Return ((text = "T") Or (text = "1"))
        End Function

        Public Function DeserializeInteger(ByVal value As Object) As Nullable(Of Integer)
            Dim text As String = Trim(Strings.CStr(value))
            If (text = vbNullString) Then Return Nothing
            Return Formats.ToInteger(text)
        End Function

        Public Function DeserializeLong(ByVal text As String) As Nullable(Of Long)
            text = Trim(text)
            If (text = vbNullString) Then Return Nothing
            Return Formats.ToLong(text)
        End Function

        Public Function DeserializeDouble(ByVal text As String) As Nullable(Of Double)
            Return DeserializeFloat(text)
        End Function

        Public Function DeserializeNumber(ByVal text As String) As Nullable(Of Double)
            Return DeserializeFloat(text)
        End Function

        Public Function DeserializeFloat(ByVal text As String) As Nullable(Of Double)
            Return Formats.ParseDouble(Replace(text, ".", ","))
        End Function

        Private Function ISOtoken(ByVal token As String) As String
            Dim ret As String = ""
            Select Case token
                Case "&quot;", "&quot;" : ret = Chr(34)
                Case "&lt;" : ret = "<"
                Case "&gt;" : ret = ">"
                Case "&amp;" : ret = "&"
                Case "&apos;" : ret = "'"
                Case "&#192;" : ret = "À"
                Case "&#193;" : ret = "Á"
                Case "&#194;" : ret = "Â"
                Case "&#195;" : ret = "Ã"
                Case "&#196;" : ret = "Ä"
                Case "&#197;" : ret = "Å"
                Case "&#198;" : ret = "Æ"
                Case "&#199;" : ret = "Ç"
                Case "&#200;" : ret = "È"
                Case "&#201;" : ret = "É"
                Case "&#202;" : ret = "Ê"
                Case "&#203;" : ret = "Ë"
                Case "&#204;" : ret = "Ì"
                Case "&#205;" : ret = "Í"
                Case "&#206;" : ret = "Î"
                Case "&#207;" : ret = "Ï"
                Case "&#208;" : ret = "Ð"

                Case "&#209;" : ret = "Ñ"
                Case "&#210;" : ret = "Ò"
                Case "&#211;" : ret = "Ó"
                Case "&#212;" : ret = "Ô"
                Case "&#213;" : ret = "Õ"
                Case "&#214;" : ret = "Ö"
                Case "&#215;" : ret = "×"
                Case "&#216;" : ret = "Ø"
                Case "&#217;" : ret = "Ù"
                Case "&#218;" : ret = "Ú"
                Case "&#219;" : ret = "Û"
                Case "&#220;" : ret = "Ü"
                Case "&#221;" : ret = "Ý"
                Case "&#222;" : ret = "Þ"
                Case "&#223;" : ret = "ß"
                Case "&#224;" : ret = "à"
                Case "&#225;" : ret = "á"
                Case "&#226;" : ret = "â"
                Case "&#227;" : ret = "ã"
                Case "&#228;" : ret = "ä"
                Case "&#229;" : ret = "å"
                Case "&#230;" : ret = "æ"
                Case "&#231;" : ret = "ç"
                Case "&#232;" : ret = "è"
                Case "&#233;" : ret = "é"
                Case "&#234;" : ret = "ê"
                Case "&#235;" : ret = "ë"
                Case "&#236;" : ret = "ì"
                Case "&#237;" : ret = "í"
                Case "&#238;" : ret = "î"
                Case "&#239;" : ret = "ï"
                Case "&#240;" : ret = "ð"
                Case "&#241;" : ret = "ñ"
                Case "&#242;" : ret = "ò"
                Case "&#243;" : ret = "ó"
                Case "&#244;" : ret = "ô"
                Case "&#245;" : ret = "õ"
                Case "&#246;" : ret = "ö"
                Case "&#247;" : ret = "÷"
                Case "&#248;" : ret = "ø"
                Case "&#249;" : ret = "ù"
                Case "&#250;" : ret = "ú"
                Case "&#251;" : ret = "û"
                Case "&#252;" : ret = "ü"
                Case "&#253;" : ret = "ý"
                Case "&#254;" : ret = "þ"
                Case "&#255;" : ret = "ÿ"

                Case "&#161;" : ret = "¡"
                Case "&#162;" : ret = "¢"
                Case "&#163;" : ret = "£"
                Case "&#164;" : ret = "¤"
                Case "&#165;" : ret = "¥"
                Case "&#128;", "&euro;" : ret = "€"
                Case "&#166;" : ret = "¦"
                Case "&#167;" : ret = "§"
                Case "&#168;" : ret = "¨"
                Case "&#169;" : ret = "©"
                Case "&#170;" : ret = "ª"
                Case "&#171;" : ret = "«"
                Case "&#172;" : ret = "¬"
                'case "¡"  "&#173;" 
                Case "&#174;" : ret = "®"
                Case "&#175;" : ret = "¯"
                Case "&#176;" : ret = "°"
                Case "&#177;" : ret = "±"
                Case "&#178;" : ret = "²"
                Case "&#179;" : ret = "³"
                Case "&#180;" : ret = "´"
                Case "&#181;" : ret = "µ"
                Case "&#182;" : ret = "¶"
                Case "&#183;" : ret = "·"
                Case "&#184;" : ret = "¸"
                Case "&#185;" : ret = "¹"
                Case "&#186;" : ret = "º"
                Case "&#187;" : ret = "»"
                Case "&#188;" : ret = "¼"
                Case "&#189;" : ret = "½"
                Case "&#190;" : ret = "¾"
                Case "&#191;" : ret = "¿"

                Case "&#130;" : ret = "‚"
                Case "&#131;" : ret = "ƒ"
                Case "&#132;" : ret = "„"
                Case "&#133;" : ret = "…"
                Case "&#134;" : ret = "†"
                Case "&#135;" : ret = "‡"
                Case "&#136;" : ret = "ˆ"
                Case "&#137;" : ret = "‰"
                Case "&#138;" : ret = "Š"
                Case "&#139;" : ret = "‹"
                Case "&#140;" : ret = "Œ"
                Case "&#142;" : ret = "Ž"
                Case "&#145;" : ret = "‘"
                Case "&#146;" : ret = "’"
                Case "&#147;" : ret = Chr(34) ' "“"
                Case " &#148;" : ret = Chr(34) '   "”"
                Case "&#149;" : ret = "•"
                Case "&#150;" : ret = "–"
                Case "&#151;" : ret = "—"
                Case "&#152;" : ret = "˜"
                Case "&#153;" : ret = "™"
                Case "&#154;" : ret = "š"
                Case "&#155;" : ret = "›"
                Case "&#156;" : ret = "œ"
                Case "&#158;" : ret = "ž"
                Case "&#159;" : ret = "Ÿ"
                Case Else
                    If (token = "") Then
                        ret = ""
                    ElseIf (token.StartsWith("&#") AndAlso token.EndsWith(";")) Then
                        token = Mid(token, 3, Len(token) - 3) ' token.substring(2, token.length - 1);
                        ret = Chr(CInt(token)) 'String.fromCharCode(parseInt(token));
                    Else
                        ret = token
                    End If
            End Select
            Return ret
        End Function

        Public Function DeserializeString(ByVal text As Object) As String
            Return Strings.HtmlDecode(text)
            'Dim value As String = Strings.CStr(text)
            'Dim ret As New System.Text.StringBuilder(value.Length)
            'Dim token As String = ""
            'Dim stato As Integer = 0
            'For i As Integer = 0 To value.Length - 1
            '    Dim ch As Char = value.Chars(i)
            '    Select Case (stato)
            '        Case 0
            '            If (ch = "&") Then
            '                stato = 1
            '                If (token <> "") Then ret.Append(token)
            '                token = ""
            '            End If
            '            token &= ch
            '        Case 1
            '            If (ch = ";") Then
            '                token &= ch
            '                token = ISOtoken(token)
            '                ret.Append(token)
            '                token = ""
            '                stato = 0
            '            ElseIf (ch = "&") Then
            '                ret.Append(token)
            '                token = ""
            '                stato = 1
            '                token &= ch
            '            Else
            '                token &= ch
            '            End If
            '    End Select
            'Next

            'ret.Append(token)
            'Return ret.ToString

            'Return Strings.HTMLDecode(Strings.CStr(text))

            ''Dim ret As String = Strings.HTMLDecode(text)
            ''If (ret = vbNullString) Then ret = ""
            ''Return ret ' text
            'Return Strings.URLDecode(text)
        End Function

        Public Function SerializeString(ByVal text As String) As String
            Return Strings.HtmlEncode(text)
#If 0 Then

            
            '' Return Strings.HtmlEncode(text)

            '''text = Replace(text, "￿", "ì")
            '''text = Replace(text, vbNullChar, "& # 0")
            '''Return Strings.HtmlEncode(text)
            ''Return Strings.URLEncode(text)
            'If (String.IsNullOrWhiteSpace(text)) Then Return text

            'Dim ret As New System.Text.StringBuilder(text.Length)
            'For Each ch As Char In text
            '    Select Case ch
            '        Case Chr(34) : ret.Append("&quot;")
            '        Case "<" : ret.Append("&lt;")
            '        Case ">" : ret.Append("&gt;")
            '        Case "&" : ret.Append("&amp;")
            '        Case "'" : ret.Append("&apos;")

            '        Case "À" : ret.Append("&#192;") '"&Agrave;")
            '        Case "Á" : ret.Append("&#193;") '"&Aacute;")
            '        Case "Â" : ret.Append("&#194;") '"&Acirc;")
            '        Case "Ã" : ret.Append("&#195;") '"&Atilde;")
            '        Case "Ä" : ret.Append("&#196;") '"&Auml;")
            '        Case "Å" : ret.Append("&#197;") 'ret.Append("&Aring;")
            '        Case "Æ" : ret.Append("&#198;") '"&AElig;")
            '        Case "Ç" : ret.Append("&#199;") ' ret.Append("&Ccedil;")
            '        Case "È" : ret.Append("&#200;") ' ret.Append("&Egrave;")
            '        Case "É" : ret.Append("&#201;") ' : ret.Append("&Eacute;")
            '        Case "Ê" : ret.Append("&#202;") ' ret.Append("&Ecirc;")
            '        Case "Ë" : ret.Append("&#203;") ' ret.Append("&Euml;")
            '        Case "Ì" : ret.Append("&#204;") ' ret.Append("&Igrave;")
            '        Case "Í" : ret.Append("&#205;") ' ret.Append("&Iacute;")
            '        Case "Î" : ret.Append("&#206;") 'ret.Append("&Icirc;")
            '        Case "Ï" : ret.Append("&#207;") ' : ret.Append("&Iuml;")
            '        Case "Ð" : ret.Append("&#208;") ' : ret.Append("&ETH;")

            '        Case "Ñ" : ret.Append("&#209;") ' ret.Append("&Ntilde;")
            '        Case "Ò" : ret.Append("&#210;") ' ret.Append("&Ograve;")
            '        Case "Ó" : ret.Append("&#211;") ' ret.Append("&Oacute;")
            '        Case "Ô" : ret.Append("&#212;") ' ret.Append("&Ocirc;")
            '        Case "Õ" : ret.Append("&#213;") ' ret.Append("&Otilde;")
            '        Case "Ö" : ret.Append("&#214;") ' ret.Append("&Ouml;")
            '        Case "×" : ret.Append("&#215;") ' ret.Append("&times;")
            '        Case "Ø" : ret.Append("&#216;") ' ret.Append("&Oslash;")
            '        Case "Ù" : ret.Append("&#217;") ' ret.Append("&Ugrave;")
            '        Case "Ú" : ret.Append("&#218;") ' ret.Append("&Uacute;")
            '        Case "Û" : ret.Append("&#219;") ' ret.Append("&Ucirc;")
            '        Case "Ü" : ret.Append("&#220;") ' ret.Append("&Uuml;")
            '        Case "Ý" : ret.Append("&#221;") ' ret.Append("&Yacute;")
            '        Case "Þ" : ret.Append("&#222;") ' ret.Append("&THORN;")
            '        Case "ß" : ret.Append("&#223;") ' ret.Append("&szlig;")
            '        Case "à" : ret.Append("&#224;") ' ret.Append("&agrave;")
            '        Case "á" : ret.Append("&#225;") ' ret.Append("&aacute;")
            '        Case "â" : ret.Append("&#226;") ' ret.Append("&acirc;")
            '        Case "ã" : ret.Append("&#227;") ' ret.Append("&atilde;")
            '        Case "ä" : ret.Append("&#228;") ' ret.Append("&auml;")
            '        Case "å" : ret.Append("&#229;") ' ret.Append("&aring;")
            '        Case "æ" : ret.Append("&#230;") ' ret.Append("&aelig;")
            '        Case "ç" : ret.Append("&#231;") ' ret.Append("&ccedil;")
            '        Case "è" : ret.Append("&#232;") ' : ret.Append("&egrave;")
            '        Case "é" : ret.Append("&#233;") ' : ret.Append("&eacute;")
            '        Case "ê" : ret.Append("&#234;") ' ret.Append("&ecirc;")
            '        Case "ë" : ret.Append("&#235;") ' ret.Append("&euml;")
            '        Case "ì" : ret.Append("&#236;") ' ret.Append("&igrave;")
            '        Case "í" : ret.Append("&#237;") ' ret.Append("&iacute;")
            '        Case "î" : ret.Append("&#238;") ' ret.Append("&icirc;")
            '        Case "ï" : ret.Append("&#239;") ' ret.Append("&iuml;")
            '        Case "ð" : ret.Append("&#240;") ' ret.Append("&eth;")
            '        Case "ñ" : ret.Append("&#241;") ' ret.Append("&ntilde;")
            '        Case "ò" : ret.Append("&#242;") ' ret.Append("&ograve;")
            '        Case "ó" : ret.Append("&#243;") ' ret.Append("&oacute;")
            '        Case "ô" : ret.Append("&#244;") ' ret.Append("&ocirc;")
            '        Case "õ" : ret.Append("&#245;") ' ret.Append("&otilde;")
            '        Case "ö" : ret.Append("&#246;") ' ret.Append("&ouml;")
            '        Case "÷" : ret.Append("&#247;") ' ret.Append("&divide;")
            '        Case "ø" : ret.Append("&#248;") ' ret.Append("&oslash;")
            '        Case "ù" : ret.Append("&#249;") ' ret.Append("&ugrave;")
            '        Case "ú" : ret.Append("&#250;") ' : ret.Append("&uacute;")
            '        Case "û" : ret.Append("&#251;") ' ret.Append("&ucirc;")
            '        Case "ü" : ret.Append("&#252;") ' ret.Append("&uuml;")
            '        Case "ý" : ret.Append("&#253;") ' ret.Append("&yacute;")
            '        Case "þ" : ret.Append("&#254;") ' ret.Append("&thorn;")
            '        Case "ÿ" : ret.Append("&#255;") ' ret.Append("&yuml;")

            '        Case "¡" : ret.Append("&#161;") 'ret.Append("&iexcl;")
            '        Case "¢" : ret.Append("&#162;") ' ret.Append("&cent;")
            '        Case "£" : ret.Append("&#163;") ' ret.Append("&pound;")
            '        Case "¤" : ret.Append("&#164;") ' ret.Append("&curren;")
            '        Case "¥" : ret.Append("&#165;") ' ret.Append("&yen;")
            '        Case "€" : ret.Append("&#128;") ' ret.Append("&euro;")
            '        Case "¦" : ret.Append("&#166;") ' ret.Append("&brvbar;")
            '        Case "§" : ret.Append("&#167;") ' ret.Append("&sect;")
            '        Case "¨" : ret.Append("&#168;") ' ret.Append("&uml;")
            '        Case "©" : ret.Append("&#169;") ' ret.Append("&copy;")
            '        Case "ª" : ret.Append("&#170;") ' ret.Append("&ordf;")
            '        Case "«" : ret.Append("&#171;") ' ret.Append("&laquo;")
            '        Case "¬" : ret.Append("&#172;") ' ret.Append("&not;")
            '            'Case "¡" : ret.Append("&#173;") ' ret.Append("&shy;")
            '        Case "®" : ret.Append("&#174;") ' ret.Append("&reg;")
            '        Case "¯" : ret.Append("&#175;") ' ret.Append("&macr;")
            '        Case "°" : ret.Append("&#176;") ' ret.Append("&deg;")
            '        Case "±" : ret.Append("&#177;") ' ret.Append("&plusmn;")
            '        Case "²" : ret.Append("&#178;") ': ret.Append("&sup2;")
            '        Case "³" : ret.Append("&#179;") ' ret.Append("&sup3;")
            '        Case "´" : ret.Append("&#180;") ' ret.Append("&acute;")
            '        Case "µ" : ret.Append("&#181;") ' ret.Append("&micro;")
            '        Case "¶" : ret.Append("&#182;") ' ret.Append("&para;")
            '        Case "·" : ret.Append("&#183;") ' ret.Append("&middot;")
            '        Case "¸" : ret.Append("&#184;") ' ret.Append("&cedil;")
            '        Case "¹" : ret.Append("&#185;") ' ret.Append("&sup1;")
            '        Case "º" : ret.Append("&#186;") ' ret.Append("&ordm;")
            '        Case "»" : ret.Append("&#187;") ' ret.Append("&raquo;")
            '        Case "¼" : ret.Append("&#188;") ' ret.Append("&frac14;")
            '        Case "½" : ret.Append("&#189;") ' ret.Append("&frac12;")
            '        Case "¾" : ret.Append("&#190;") ' ret.Append("&frac34;")
            '        Case "¿" : ret.Append("&#191;") ' ret.Append("&iquest;")

            '        Case "‚" : ret.Append("&#130;") ' ret.Append("&sbquo;")
            '        Case "ƒ" : ret.Append("&#131;") 'ret.Append("&fnof;")
            '        Case "„" : ret.Append("&#132;") 'ret.Append("&bdquo;")
            '        Case "…" : ret.Append("&#133;") 'ret.Append("&hellip;")
            '        Case "†" : ret.Append("&#134;") 'ret.Append("&dagger;")
            '        Case "‡" : ret.Append("&#135;") 'ret.Append("&Dagger;")
            '        Case "ˆ" : ret.Append("&#136;") 'ret.Append("&circ;")
            '        Case "‰" : ret.Append("&#137;") 'ret.Append("&permil;")
            '        Case "Š" : ret.Append("&#138;") 'ret.Append("&Scaron;")
            '        Case "‹" : ret.Append("&#139;") 'ret.Append("&lsaquo;")
            '        Case "Œ" : ret.Append("&#140;") 'ret.Append("&OElig;")
            '        Case "Ž" : ret.Append("&#142;") 'ret.Append("&Zcaron;")
            '        Case "‘" : ret.Append("&#145;") 'ret.Append("&lsquo;")
            '        Case "’" : ret.Append("&#146;") 'ret.Append("&rsquo;")
            '        Case Trim("“" ") : ret.Append("&#147;") ' ret.Append("&ldquo;")
            '        Case Trim("”" ") : ret.Append("&#148;")  'ret.Append("&rdquo;")
            '        Case "•" : ret.Append("&#149;") 'ret.Append("&bull;")
            '        Case "–" : ret.Append("&#150;") 'ret.Append("&ndash;")
            '        Case "—" : ret.Append("&#151;") 'ret.Append("&mdash;")
            '        Case "˜" : ret.Append("&#152;") 'ret.Append("&tilde;")
            '        Case "™" : ret.Append("&#153;") 'ret.Append("&trade;")
            '        Case "š" : ret.Append("&#154;") 'ret.Append("&scaron;")
            '        Case "›" : ret.Append("&#155;") 'ret.Append("&rsaquo;")
            '        Case "œ" : ret.Append("&#156;") 'ret.Append("&oelig;")
            '        Case "ž" : ret.Append("&#158;") 'ret.Append("&zcaron;")
            '        Case "Ÿ" : ret.Append("&#159;") 'ret.Append("&Yuml;")

            '        Case Else
            '            Dim code As Integer = Asc(ch)

            '            If (code <= 13 OrElse code > 127) Then
            '                ret.Append("&#" & code & ";")
            '            Else
            '                ret.Append(ch)
            '            End If
            '    End Select
            'Next
            'Return ret.ToString

#End If
        End Function

        Public Function SerializeBoolean(ByVal value As Object) As String
            If (TypeOf (value) Is Nullable(Of Boolean) AndAlso DirectCast(value, Nullable(Of Boolean)).HasValue = False) Then Return ""
            If (value Is Nothing) Then Return ""
            Return Strings.HtmlEncode(IIf(CBool(value) = True, "True", "False"))
        End Function

        Public Function SerializeNumber(ByVal value As Object) As String
            If (TypeOf (value) Is Nullable(Of Byte) AndAlso DirectCast(value, Nullable(Of Byte)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of SByte) AndAlso DirectCast(value, Nullable(Of SByte)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Int16) AndAlso DirectCast(value, Nullable(Of Int16)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of UInt16) AndAlso DirectCast(value, Nullable(Of UInt16)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Int32) AndAlso DirectCast(value, Nullable(Of Int32)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of UInt32) AndAlso DirectCast(value, Nullable(Of UInt32)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Int64) AndAlso DirectCast(value, Nullable(Of Int64)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of UInt64) AndAlso DirectCast(value, Nullable(Of UInt64)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Single) AndAlso DirectCast(value, Nullable(Of Single)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Double) AndAlso DirectCast(value, Nullable(Of Double)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Decimal) AndAlso DirectCast(value, Nullable(Of Decimal)).HasValue = False) Then Return ""
            If (value Is Nothing) Then Return ""
            Return Strings.HtmlEncode(Formats.USA.FormatDouble(value))
        End Function

        Public Function SerializeDouble(ByVal value As Object) As String
            Return Me.SerializeNumber(value)
        End Function

        Public Function SerializeInteger(ByVal value As Object) As String
            If (TypeOf (value) Is Nullable(Of Byte) AndAlso DirectCast(value, Nullable(Of Byte)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of SByte) AndAlso DirectCast(value, Nullable(Of SByte)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Int16) AndAlso DirectCast(value, Nullable(Of Int16)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of UInt16) AndAlso DirectCast(value, Nullable(Of UInt16)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Int32) AndAlso DirectCast(value, Nullable(Of Int32)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of UInt32) AndAlso DirectCast(value, Nullable(Of UInt32)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Int64) AndAlso DirectCast(value, Nullable(Of Int64)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of UInt64) AndAlso DirectCast(value, Nullable(Of UInt64)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Single) AndAlso DirectCast(value, Nullable(Of Single)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Double) AndAlso DirectCast(value, Nullable(Of Double)).HasValue = False) Then Return ""
            If (TypeOf (value) Is Nullable(Of Decimal) AndAlso DirectCast(value, Nullable(Of Decimal)).HasValue = False) Then Return ""
            If (value Is Nothing) Then Return ""
            Return Strings.HtmlEncode(CLng(value).ToString)
        End Function

        Public Function SerializeDate(ByVal value As Object) As String
            If (TypeOf (value) Is Nullable(Of Date) AndAlso DirectCast(value, Nullable(Of Date)).HasValue = False) Then Return ""
            If (value Is Nothing) Then Return ""
            Dim d As Date = CDate(value)
            Return Strings.HtmlEncode(Year(d) & "/" & Month(d) & "/" & Day(d) & " " & Hour(d) & "." & Minute(d) & "." & Second(d))
        End Function

        Public Function SerializeArray(Of T)(ByVal items() As T) As String
            Return SerializeArray(items, "", New XMLWriter)
        End Function

        Public Function SerializeArray(Of T)(ByVal items() As T, ByVal baseType As String) As String
            Return SerializeArray(items, baseType, New XMLWriter)
        End Function

        Public Overridable Function SerializeArray(Of T)(ByVal items() As T, ByVal baseType As String, ByVal writer As XMLWriter) As String
            Dim i As Integer
            Dim ret As String = vbNullString
            If (UBound(items) >= 0) Then
                If baseType = vbNullString Then
                    For i = 0 To Arrays.Len(items) - 1
                        baseType = XMLTypeName(items(i))
                        ret &= "<" & baseType & ">" & XML.Utils.Serializer.Serialize(items(i), XMLSerializeMethod.None, writer) & "</" & baseType & ">"
                    Next
                Else
                    For i = 0 To Arrays.Len(items) - 1
                        ret &= "<" & baseType & ">" & XML.Utils.Serializer.Serialize(items(i), XMLSerializeMethod.None, writer) & "</" & baseType & ">"
                    Next
                End If
            End If
            Return ret
        End Function

#Region "Read"

        Public Function Read(ByRef target As Boolean, ByVal source As Object) As Boolean
            Return Formats.ToBool(source)
        End Function

        Public Sub Read(ByRef target As Nullable(Of Boolean), ByVal source As Object)
            target = Formats.ParseBool(source)
        End Sub

        Public Sub Read(ByRef target As Byte, ByVal source As Object)
            target = CByte(Formats.ToInteger(source))
        End Sub

        Public Sub Read(ByRef target As Nullable(Of Byte), ByVal source As Object)
            target = Formats.ParseInteger(source)
        End Sub

        Public Sub Read(ByRef target As SByte, ByVal source As Object)
            target = CSByte(Formats.ToInteger(source))
        End Sub

        Public Sub Read(ByRef target As Nullable(Of SByte), ByVal source As Object)
            target = Formats.ParseInteger(source)
        End Sub

        Public Sub Read(ByRef target As Short, ByVal source As Object)
            target = CShort(Formats.ToInteger(source))
        End Sub

        Public Sub Read(ByRef target As Nullable(Of Short), ByVal source As Object)
            target = Formats.ParseInteger(source)
        End Sub

        Public Sub Read(ByRef target As Integer, ByVal source As Object)
            target = CInt(Formats.ToInteger(source))
        End Sub

        Public Sub Read(ByRef target As Nullable(Of Integer), ByVal source As Object)
            target = Formats.ParseInteger(source)
        End Sub

        Public Sub Read(ByRef target As Long, ByVal source As Object)
            target = CLng(Formats.ToInteger(source))
        End Sub

        Public Sub Read(ByRef target As Nullable(Of Long), ByVal source As Object)
            target = Formats.ParseInteger(source)
        End Sub

        Public Sub Read(ByRef target As Single, ByVal source As Object)
            target = Formats.ToDouble(source)
        End Sub

        Public Sub Read(ByRef target As Nullable(Of Single), ByVal source As Object)
            target = Formats.ParseDouble(source)
        End Sub

        Public Sub Read(ByRef target As Double, ByVal source As Object)
            target = Formats.ToDouble(source)
        End Sub

        Public Sub Read(ByRef target As Nullable(Of Double), ByVal source As Object)
            target = Formats.ParseDouble(source)
        End Sub

        Public Sub Read(ByRef target As Decimal, ByVal source As Object)
            target = Formats.ToDouble(source)
        End Sub

        Public Sub Read(ByRef target As Nullable(Of Decimal), ByVal source As Object)
            target = Formats.ParseDouble(source)
        End Sub

        Public Sub Read(ByRef target As Date, ByVal source As Object)
            target = Formats.ToDate(source)
        End Sub

        Public Sub Read(ByRef target As Nullable(Of Date), ByVal source As Object)
            target = Formats.ParseDate(source)
        End Sub

        Public Sub Read(ByRef target As String, ByVal source As Object)
            target = Formats.ToString(source)
        End Sub

        'Public  Sub Read(Of T)(ByRef target As T, ByVal source As Object)
        '    target = Formats.ToInteger(source)
        'End Sub

        'Public  Sub Read(ByRef target As Nullable(Of [Enum]), ByVal source As Object)
        '    target = Formats.ToInteger(source)
        'End Sub


#End Region

        Public Overridable Function SerializeObject(ByVal obj As Object) As String
            If (obj Is Nothing) Then Return vbNullString
            Return "<" & XMLTypeName(obj) & ">" & Me.Serialize(obj, XMLSerializeMethod.None) & "</" & XMLTypeName(obj) & ">"
        End Function

        Public Overridable Function SerializeObject(ByVal obj As Object, ByVal writer As XMLWriter) As String
            If (obj Is Nothing) Then Return vbNullString
            Return "<" & XMLTypeName(obj) & ">" & Me.Serialize(obj, XMLSerializeMethod.None, writer) & "</" & XMLTypeName(obj) & ">"
        End Function

        Public Overridable Sub DeserializeField(ByRef field As Double, ByVal fieldValue As Object)
            field = Me.DeserializeDouble(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Integer, ByVal fieldValue As Object)
            field = Me.DeserializeInteger(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Short, ByVal fieldValue As Object)
            field = Me.DeserializeInteger(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Byte, ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeInteger(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Single, ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeDouble(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Date, ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeDate(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Boolean, ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As String, ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeString(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Nullable(Of Double), ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeDouble(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Nullable(Of Integer), ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeInteger(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Nullable(Of Short), ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeInteger(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Nullable(Of Byte), ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeInteger(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Nullable(Of Single), ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeDouble(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Nullable(Of Date), ByVal fieldValue As Object)
            field = XML.Utils.Serializer.DeserializeDate(fieldValue)
        End Sub

        Public Overridable Sub DeserializeField(ByRef field As Array, ByVal fieldValue As Object)
            field = fieldValue
        End Sub

        Function ToObject(ByVal value As Object) As Object
            If (TypeOf (value) Is String) Then Return Nothing
            Return value
        End Function

        Function ToArray(Of T)(fieldValue As Object) As Array
            Return Arrays.Convert(Of T)(fieldValue)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace