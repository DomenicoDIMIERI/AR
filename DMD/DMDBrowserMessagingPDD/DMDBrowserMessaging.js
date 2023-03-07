 
var Arrays = new Object();
Arrays.RemoveAt = function (items, position) {
    var numItems = (arguments.length == 3) ? arguments[2] : 1;
    //items.splice(position, numItems);
    for (var i = position; i < items.length - numItems; i++) {
        items[i] = items[i + numItems];
    }
    items.length = Math.max(0, items.length - numItems);
    return items;
}



//*** 
function DMDWHPPChatMessage() {
    this.div = null;
    this.direction = "";
    this.message = "";
    this.time = null;
    this.img = null;
    this.imgUrl = "";
    this.Chat = null;
}
DMDWHPPChatMessage.prototype.toString = function () {
    return this.time + " : " + this.message + " / " + this.imgUrl;
}
DMDWHPPChatMessage.prototype.parseDiv = function (div) {
    function parseDate(value) {
        const allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ÁÀÈÉÌÍÒÓÙÚ/:. ";
        value = value.toUpperCase().trim();
        var text = "";
        for (var i = 0; i < value.length; i++) {
            var ch = value.charAt(i);
            if (allowed.indexOf(ch) >= 0) text += ch;
        }

        //var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
        console.log(text);
        var d = new Date();
        switch (text.trim()) {
            case "OGGI": return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            case "IERI":
                d.setDate(d.getDate() - 1);
                return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            case "LUNEDÍ": case "LUNEDÌ": case "LUNED":
                while (d.getDay() != 1) {
                    d.setDate(d.getDate() - 1);
                }
                return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            case "MARTEDÍ": case "MARTEDÌ":case "MARTED":
                while (d.getDay() != 2) {
                    d.setDate(d.getDate() - 1);
                }
                return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            case "MERCOLEDÍ": case "MERCOLEDÌ": case "MERCOLED":
                while (d.getDay() != 3) {
                    d.setDate(d.getDate() - 1);
                }
                return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            case "GIOVEDÍ": case "GIOVEDÌ":case "GIOVED":
                while (d.getDay() != 4) {
                    d.setDate(d.getDate() - 1);
                }
                return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            case "VENERDÍ": case "VENERDÌ":case "VENERD":
                while (d.getDay() != 5) {
                    d.setDate(d.getDate() - 1);
                }
                return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            case "SABATO": 
                while (d.getDay() != 6) {
                    d.setDate(d.getDate() - 1);
                }
                return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            case "DOMENICA":
                while (d.getDay() != 0) {
                    d.setDate(d.getDate() - 1);
                }
                return d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
            default:
                return text;
        }

    }
    this.div = div;
    var classes = div.className.split(" ");
    if (classes.indexOf("msg-system")>=0) {
        //si tratta di un messaggio di quelli in cui compare la data oppure le scritta "OGGI" o "IERI"
        console.log(this.Chat.Title + " SYS : " + div.innerText);
        if (div.innerText != "I MESSAGGI CHE INVII IN QUESTA CHAT E LE CHIAMATE SONO PROTETTI CON LA CRITTOGRAFIA ENDTOEND") this.Chat.ChatTime = parseDate(div.innerText);
        console.log(this.Chat.ChatTime);
        return false;
    } else { //if (classes.indexOf("msg-continuation") >= 0) {
        //Si tratta di un vero messaggio
        this.direction = (div.getElementsByClassName("message-out").length > 0) ? "out" : "in";

        var divImg = div.getElementsByClassName("image-thumb")[0];
        if (divImg != null && typeof (divImg) != "undefined") {
            
            this.img = divImg.getElementsByTagName("img")[0];
            if (this.img != null && typeof (this.img) != "undefined") {
                this.imgUrl = this.img.src;
            }

        } else {
            var divText = div.getElementsByClassName("selectable-text")[0];
            if (divText != null && typeof (divText) != "undefined") {
                this.message = divText.innerText;

            }

        }

        var spanTime = div.getElementsByClassName("message-datetime")[0];
        if (spanTime != null && typeof (spanTime) != "undefined") {
            this.time = this.Chat.ChatTime + " " + spanTime.innerText;

        }

        console.log(this.Chat.Title + " MSG : " + this.direction + ", " + this.time + " " + this.message + " / " + this.imgUrl);

        return true;
    }

}
DMDWHPPChatMessage.prototype.XMLSerialize = function (writer) {
    writer.WriteAttribute("direction", this.direction);
    writer.WriteAttribute("message", this.message);
    writer.WriteAttribute("time", this.time);
    writer.WriteAttribute("imgUrl", this.imgUrl);
}

//***
function DMDWHPPChatItem() {
    this.parser = null;
    this.div = null;
    this.img = null;

    this.iconURL = "";
    this.Title = "";
    this.Messages = [];
    this.innerHTML = "";
    this.ChatTime = "";
    //this.Status = "";
    //this.LastUpdated = null;
}
DMDWHPPChatItem.prototype.Activate = function () {
    var o = this.div.getElementsByClassName("chat-body")[0];
    if (o !== null) {
        o.focus();

        var pos = getElementAbsPos(o);
        eventFire(o, "mousedown", pos.x + 10, pos.y + 10, pos.x + 10, pos.x + 10);
    }
}
DMDWHPPChatItem.prototype.Unactivate = function () { }
DMDWHPPChatItem.prototype.toString = function () {
    return this.Title + ", " + this.iconURL.substring(0, 32);
}
DMDWHPPChatItem.prototype.parseDiv = function (div) {
    this.div = div;
    this.img = this.div.getElementsByTagName("img")[0];
    this.iconURL = (this.img == null) ? "" : this.img.src;

    var o = this.div.getElementsByClassName("chat-title")[0];
    this.Title = (o == null) ? "" : o.innerText;

    this.innerHTML = div.innerHTML;
}
DMDWHPPChatItem.prototype.PrepareMessages = function () {
    this.Activate();
    this.ParseMessages();
}
DMDWHPPChatItem.prototype.ParseMessages = function () {
    var divList = document.body.getElementsByClassName("message-list")[0];
    if (divList == null || typeof (divList) == "undefined") {
        console.log("   divList is NULL");
        return;
    }
    var divMessages = divList.getElementsByClassName("msg");
    for (var i = 0; i < divMessages.length; i++) {
        var message = new DMDWHPPChatMessage();
        message.Chat = this;
        if (message.parseDiv(divMessages[i])) this.Messages[this.Messages.length] = message;
    }
}
DMDWHPPChatItem.prototype.XMLSerialize = function (writer) {
    writer.WriteAttribute("iconURL", this.iconURL);
    writer.WriteAttribute("Title", this.Title);
    writer.WriteAttribute("ChatTime", this.ChatTime);
    writer.WriteTag("Messages", this.Messages);
}

//********
function DMDWHParser() {
    this.chats = [];
    this.parsingIndex = 0;
}
DMDWHParser.prototype.Parse = function () {
    var divs = document.body.getElementsByClassName("chat"); // domDic.getElementsByClassName("chat");
    this.chats = [];
    for (var i = 0; i < divs.length; i++) {
        var chat = new DMDWHPPChatItem();
        chat.parser = this;
        chat.parseDiv(divs[i]);
        this.chats[this.chats.length] = chat;
    }
    this.parsingIndex = 0;
    while (this.parsingIndex < this.chats.length) {
        this.ParseCurrent();
        this.parsingIndex += 1;
    }
}
DMDWHParser.prototype.ParseNext = function () {
    this.parsingIndex += 1;
    if (this.parsingIndex < this.chats.length) {
        this.chats[this.parsingIndex].PrepareMessages();        
    }
}
DMDWHParser.prototype.ParseCurrent = function () {
    this.chats[this.parsingIndex].PrepareMessages();
}
DMDWHParser.prototype.XMLSerialize = function (writer) {
    writer.WriteTag("Chats", this.chats);
}

DMDWHParser.Instance = null;


  

//*** FUNZIONI DI UTILITA'
function isArray(obj) { return (Object.prototype.toString.call(obj) === '[object Array]'); }
function isDate(val) { return (typeof (val) !== "undefined") && (val !== null) && (val.constructor == Date); }
function vbTypeName(obj) {
    if (obj == null) return "Nothing";
    if (typeof (obj) == "object") {
        var tmp = (obj).constructor.toString();
        var i = tmp.indexOf("(");
        if (i >= 0) tmp = tmp.substring(0, i);
        i = tmp.indexOf(" ");
        if (i >= 0) tmp = tmp.substring(i);

        return tmp.trim();
    } else {
        var ret = typeof (obj);
        switch (ret) {
            case "boolean": return "Boolean";
            case "number": return "Double";
            case "string": return "String";
            default:
                if (isDate(obj)) return "DateTime";
                return ret;
        }
    }
}

//*** FUNZIONI PER IL DOM
function eventFire(el, etype) {
    var evObj;
    switch (etype) {
        case "click":
            if (el.fireEvent) {
                el.fireEvent('on' + etype);
            } else {
                evObj = document.createEvent('MouseEvents');
                var x = (arguments.length > 2) ? arguments[2] : 0;
                var y = (arguments.length > 3) ? arguments[3] : 0;
                var xc = (arguments.length > 4) ? arguments[4] : 0;
                var yc = (arguments.length > 5) ? arguments[5] : 0;
                //console.log("initMouseEvent (" + x + ", " + y + ")");
                //initMouseEvent( 'type', bubbles, cancelable, windowObject, detail, screenX, screenY, clientX, clientY, ctrlKey, altKey, shiftKey, metaKey, button, relatedTarget )
                evObj.initMouseEvent(etype, true, true, window, 1, x, y, xc, yc, false, false, false, false, 0, null);
                el.dispatchEvent(evObj);
            }
            break;
        default:
            if (el.fireEvent) {
                el.fireEvent('on' + etype);
            } else {
                evObj = document.createEvent('Events');
                evObj.initEvent(etype, true, false);
                el.dispatchEvent(evObj);
            }
    }

}
//Setta un attributo classe
function setClassAttribute(elem, classValue) {
    if (typeof (elem) == "string") elem = document.getElementById(elem);
    //if (isIE) elem.className = classValue;
    //else elem.class = classValue;
    elem.className = classValue;
    //if (typeof (elem.className) != "undefined") elem.className = classValue;
    //else elem.class = classValue;
}
function getClassAttribute(elem) {
    if (typeof (elem) == "string") elem = document.getElementById(elem);
    return elem.className;
}
function addClassAttribute(elem, value) {
    if (typeof (elem) == "string") elem = document.getElementById(elem);
    value = value.trim();
    var str = elem.className;
    var items = str.split(" ");
    for (var i = 0; i < items.length; i++) if (items[i] == value) return;
    elem.className = (value + " " + str).trim();
}
function removeClassAttribute(elem, value) {
    if (typeof (elem) == "string") elem = document.getElementById(elem);
    value = value.trim();
    var str = elem.className;
    var items = str.split(" ");
    for (var i = 0; i < items.length; i++) {
        if (items[i] == value) {
            items = Arrays.RemoveAt(items, i);
            elem.className = Array.join(items, " ");
            return;
        }
    }
}

//****************
function getElementAbsPos(elem) {
    if (typeof (elem) == "string") elem = document.getElementById(elem);
    var x = 0, y = 0;
    while (elem !== null) {
        var scroll = getElementScrollXY(elem);
        x += elem.offsetLeft - scroll.x;
        y += elem.offsetTop - scroll.y;
        elem = elem.offsetParent;
    }
    return { 'x': x, 'y': y };
}

function getElementScrollXY(elem) {
    var e = (typeof (elem) == "string") ? document.getElementById(elem) : elem;
    var scrOfX = 0, scrOfY = 0;
    if (typeof (e.scrollLeft) == "number") { //DOM compliant
        scrOfY = e.scrollTop;
        scrOfX = e.scrollLeft;
    } else if (document.documentElement && (e.scrollLeft || e.scrollTop)) { //IE6 standards compliant mode
        scrOfY = e.scrollTop;
        scrOfX = e.scrollLeft;
    } else {
        scrOfY = e.pageYOffset;
        scrOfX = e.pageXOffset;
    }
    return { 'x': scrOfX, 'y': scrOfY };
}

//*******
var XML = new Object();
XML.Encoding = "windows-1252"; //"utf-8"
XML.Serialize = function (obj) {
    var writer = new XMLWriter();

    writer.WriteRowData("<?xml version=\"1.0\" encoding=\"" + XML.Encoding + "\"?>\n");
    if (isArray(obj) ) {
        writer.BeginTag("ArrayOf" + vbTypeName(obj[0]));
        writer.WriteAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        writer.WriteAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
    } else {
        writer.BeginTag(vbTypeName(obj));
        writer.WriteAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        writer.WriteAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
    }
    if (typeof (obj) == "object" && typeof (obj["XMLSerialize"]) == "function") obj.XMLSerialize(writer);
    else writer.Write(obj);
    writer.EndTag(); // += "</ArrayOf" + vbTypeName(obj[0]) + ">\n";


    return writer.toString();
};
XML.SerializeString = function (value) {
    function HTMLEncoder(source, display, tabs) {
        this.source = "" + source;
        this.display = (arguments.length>1)? (arguments[1]==true) :  false;
        this.tabs = (arguments.length > 2) ? Math.floor(arguments[2]) : 4; if (this.tabs < 0) this.tabs = 4;
        this.i = 0;
        this.ch = "";
        this.peek = "";
        this.line = [];
        this.result = [];
        this.spaces = 0;
    }
    HTMLEncoder.prototype.next = function () { // Stash the next character and advance the pointer
        this.peek = this.source.charAt(this.i);
        this.i += 1;
    };
    HTMLEncoder.prototype.endline = function () { // Start a new "line" of output, to be joined later by <br />
        this.line = this.line.join('');
        if (this.display) { // If a line starts or ends with a space, it evaporates in html unless it's an nbsp.
            this.line = this.line.replace(/(^ )|( $)/g, '&nbsp;');
        }
        this.result.push(this.line);
        this.line = [];
    };
    HTMLEncoder.prototype.push = function () { // Push a character or its entity onto the current line
        if (this.ch < ' ' || this.ch > '~') {
            this.line.push('&#' + this.ch.charCodeAt(0) + ';');
        } else {
            this.line.push(this.ch);
        }
    };
    HTMLEncoder.prototype.encode = function () {
        this.result = [];
        this.line = [];
        this.i = 0;
        this.next();
        while (this.i <= this.source.length) { // less than or equal, because i is always one ahead
            this.ch = this.peek;
            this.next();
            // HTML special chars.
            switch (this.ch) {
                case '<': this.line.push('&lt;'); break;
                case '>': this.line.push('&gt;'); break;
                case '&': this.line.push('&amp;'); break;
                case '"': this.line.push('&quot;'); break;
                case "'": this.line.push('&#39;'); break;
                    //case "À": this.line.push("&Agrave;"); break;
                    //case "à": this.line.push("&agrave;"); break;
                    //case "Ì": this.line.push("&Igrave;"); break;
                    //case "ì": this.line.push("&igrave;"); break;
                    //case "Ò": this.line.push("&Ograve;"); break;
                    //case "ò": this.line.push("&ograve;"); break;
                    //case "Ù": this.line.push("&Ugrave;"); break;
                    //case "ù": this.line.push("&ugrave;"); break;
                    //case "È": this.line.push("&Egrave;"); break;
                    //case "è": this.line.push("&egrave;"); break;
                    //case "É": this.line.push("&Eacute;"); break;
                    //case "é": this.line.push("&eacute;"); break;
                default:
                    // If the output is intended for display,
                    // then end lines on newlines, and replace tabs with spaces.
                    if (this.display) {
                        switch (this.ch) {
                            case '\r':
                                // If this \r is the beginning of a \r\n, skip over the \n part.
                                if (this.peek === '\n') {
                                    this.next();
                                }
                                this.endline();
                                break;
                            case '\n':
                                this.endline();
                                break;
                            case '\t':
                                // expand tabs
                                this.spaces = this.tabs - (this.line.length % this.tabs);
                                for (var s = 0; s < this.spaces; s += 1) {
                                    this.line.push(' ');
                                }
                                break;
                            default:
                                // All other characters can be dealt with generically.
                                this.push();
                        }
                    } else {
                        // If the output is not for display,
                        // then none of the characters need special treatment.
                        this.push();
                    }
            }
        }
        this.endline();

        // If you can't beat 'em, join 'em.
        this.result = this.result.join('<br />');

        if (this.display) {
            // Break up contiguous blocks of spaces with non-breaking spaces
            this.result = this.result.replace(/ {2}/g, ' &nbsp;');
        }

        // tada!
        return this.result;
    };

    var encoder = new HTMLEncoder(value, false, 4);
    return encoder.encode();
}
//***

//************************
function XMLWriter() {
    this.m_Encoding = "windows-1252"; //"utf-8"
    this.m_OpenedTags = [];
    this.m_Buffer = "";
    this.m_BaseElemType = "";
    this.m_IsArray = false;
    this.m_IsInTag = false;
}
XMLWriter.prototype.getEncoding = function () { return this.m_Encoding; }
XMLWriter.prototype.setEncoding = function (value) { this.m_Encoding = value; }
XMLWriter.prototype.getLength = function () { return this.m_Buffer.length; }
XMLWriter.prototype.WriteRowString = function (text) { this.CheckInTag(); this.m_Buffer += text; }
XMLWriter.prototype.WriteRowData = function (text) { this.CheckInTag(); this.m_Buffer += text; }
XMLWriter.prototype.BeginTag = function (tagName) {
    this.CheckInTag();
    this.m_OpenedTags[this.m_OpenedTags.length] = tagName.trim();
    this.m_Buffer += "<" + tagName;
    this.m_IsInTag = true;
}
XMLWriter.prototype.WriteAttribute = function (key, value) {
    if (!this.m_IsInTag) throw "WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo";

    var str;
    if (typeof (value) == "boolean") str = XML.SerializeBoolean(value);
    else if (typeof (value) == "number") str = XML.SerializeNumber(value);
    else if (typeof (value) == "string") str = XML.SerializeString(value);
    else if (isDate(value)) str = XML.SerializeDate(value);
    else if (isArray(value)) {
        if (arguments.length == 2) str = XML.SerializeArray(value, arguments[1], this);
        else str = XML.SerializeArray(value, "", this);
    } else if (value == null) {
        str = "";
    } else {
        str = XML.SerializeObject(value);
    }
    this.m_Buffer += " " + key + "=\"" + str + "\" ";
}
XMLWriter.prototype.CanCloseTag = function (tag) {
    switch (tag.trim().toUpperCase()) {
        case "IMG": case "INPUT": case "METADATA": case "BR": case "HR": return true;
        default: return false;
    }
}
XMLWriter.prototype.EndTag = function () {
    this.CheckInTag();
    var currTag = this.m_OpenedTags[this.m_OpenedTags.length - 1];
    this.m_OpenedTags = Arrays.RemoveAt(this.m_OpenedTags, this.m_OpenedTags.length - 1);
    if (this.CanCloseTag(currTag)) {
        //
    } else {
        this.m_Buffer += "</" + currTag + ">";
    }
}
XMLWriter.prototype.CheckInTag = function () {
    if (this.m_IsInTag) {
        if (this.CanCloseTag(this.m_OpenedTags[this.m_OpenedTags.length - 1])) {
            this.m_Buffer += "/>";
        } else {
            this.m_Buffer += ">";
        }
    }
    this.m_IsInTag = false;
}
XMLWriter.prototype.toString = function () { return this.m_Buffer; }
XMLWriter.prototype.BeginDocument = function (method, obj) { throw "Not Implemented"; }
XMLWriter.prototype.EndDocument = function () { throw "Not Implemented"; }
XMLWriter.prototype.Clear = function () { this.Dispose(); }
XMLWriter.prototype.Dispose = function () {
    this.m_OpenedTags = [];
    this.m_Buffer = "";
    this.m_BaseElemType = "";
    this.m_IsArray = false;
    this.m_IsInTag = false;
}
XMLWriter.prototype.WriteTag = function (tagname, value) {
    var i;
    if (typeof (value) == "boolean") { this.BeginTag(tagname); this.CheckInTag(); this.m_Buffer += XML.SerializeBoolean(value); this.EndTag(); }
    else if (typeof (value) == "number") { this.BeginTag(tagname); this.CheckInTag(); this.m_Buffer += XML.SerializeNumber(value); this.EndTag(); }
    else if (typeof (value) == "string") { this.BeginTag(tagname); this.CheckInTag(); this.m_Buffer += XML.SerializeString(value); this.EndTag(); }
    else if (isDate(value)) { this.BeginTag(tagname); this.CheckInTag(); this.m_Buffer += XML.SerializeDate(value); this.EndTag(); }
    else if (isArray(value)) {
        this.CheckInTag();
        this.BeginTag(tagname);
        if (arguments.length == 3) {
            for (i = 0; i < value.length; i++) {
                this.CheckInTag();
                this.BeginTag(arguments[2]);
                if (typeof (value[i]) == "boolean") { this.CheckInTag(); this.m_Buffer += XML.SerializeBoolean(value[i]); }
                else if (typeof (value[i]) == "number") { this.CheckInTag(); this.m_Buffer += XML.SerializeNumber(value[i]); }
                else if (typeof (value[i]) == "string") { this.CheckInTag(); this.m_Buffer += XML.SerializeString(value[i]); }
                else if (isDate(value[i])) { this.CheckInTag(); this.m_Buffer += XML.SerializeDate(value[i]); }
                else if (value[i] == null) {
                     //
                } else {
                    value[i].XMLSerialize(this);
                }
                this.EndTag();
            }
        }
        else {
            for (i = 0; i < value.length; i++) {
                this.CheckInTag();
                this.BeginTag(vbTypeName(value[i]));
                if (typeof (value[i]) == "boolean") { this.CheckInTag(); this.m_Buffer += XML.SerializeBoolean(value[i]); }
                else if (typeof (value[i]) == "number") { this.CheckInTag(); this.m_Buffer += XML.SerializeNumber(value[i]); }
                else if (typeof (value[i]) == "string") { this.CheckInTag(); this.m_Buffer += XML.SerializeString(value[i]); }
                else if (isDate(value[i])) { this.CheckInTag(); this.m_Buffer += XML.SerializeDate(value[i]); }
                else if (value[i] == null) {
                    //
                } else {
                    value[i].XMLSerialize(this);
                }
                this.EndTag();
            }
        }
        this.EndTag();
    } else if (value == null) {
        this.CheckInTag();
        this.BeginTag(tagname);
        this.EndTag();
    } else {
        this.CheckInTag();
        this.BeginTag(tagname);
        this.BeginTag(vbTypeName(value));
        value.XMLSerialize(this);
        this.EndTag();
        this.EndTag();
    }
}
XMLWriter.prototype.Write = function (value) {
    this.CheckInTag();
    var i;
    if (typeof (value) == "boolean") this.m_Buffer += XML.SerializeBoolean(value);
    else if (typeof (value) == "number") this.m_Buffer += XML.SerializeNumber(value);
    else if (typeof (value) == "string") this.m_Buffer += XML.SerializeString(value);
    else if (isDate(value)) this.m_Buffer += XML.SerializeDate(value);
    else if (isArray(value)) {
        if (arguments.length == 2) for (i = 0; i < value.length; i++) this.WriteTag(arguments[1], value[i]);
        else {
            for (i = 0; i < value.length; i++) {
                if (value[i] == null) this.m_Buffer += "<Nothing></Nothing>";
                else if (typeof (value[i]) == "object") this.Write(value[i]);
                else {
                    this.WriteTag(vbTypeName(value[i]), value[i]);
                }
            }
        }
    } else if (value == null) {
        //do nothing
    } else {
        this.BeginTag(vbTypeName(value));
        value.XMLSerialize(this);
        this.EndTag();
    }
}

//*** INTEROPERABILITY
// Listen for messages
chrome.runtime.onMessage.addListener(
    function (msg, sender, sendResponse) {
        // If the received message has the expected format...
        if (msg.text === 'report_back') {
            DMDWHParser.Instance = new DMDWHParser();
            DMDWHParser.Instance.Parse();
            console.log(vbTypeName(DMDWHParser.Instance.chats[0]));
            var str = XML.Serialize(DMDWHParser.Instance);
            sendResponse(str);           
        }
    }
);

//function handleClick(e) {
//    alert("Click rilevato (" + e.y + ", " + e.y + ")\r\n" + e.target.outerHTML);
//}
//window.addEventListener("mousedown", handleClick);