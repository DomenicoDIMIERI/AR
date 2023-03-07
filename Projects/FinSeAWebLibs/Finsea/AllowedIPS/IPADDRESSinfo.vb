Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.IO


Partial Public Class WebSite


    <Serializable> _
    Public Class IPADDRESSinfo
        Inherits DBObject
        Implements IComparable

        Private m_IP() As Byte
        Private m_NetMask() As Byte
        Private m_Descrizione As String
        Private m_Allow As Boolean
        Private m_Negate As Boolean
        Private m_Interno As Boolean
        Private m_AssociaUfficio As String

        Public Sub New()
            ReDim Me.m_IP(3)
            ReDim Me.m_NetMask(3)
            Me.m_Descrizione = ""
            Me.m_Allow = True
            Me.m_Negate = False
            Me.m_Interno = True
            Me.m_AssociaUfficio = ""
        End Sub

        Public Sub New(ByVal value As String)
            Dim p As Integer = InStr(value, "/")
            Dim ip, netMask As String
            If (p > 1) Then
                ip = Left(value, p - 1)
                netMask = Mid(value, p + 1)
            Else
                ip = value
                netMask = ""
            End If
            Me.m_IP = GetBytes(ip)
            If InStr(netMask, ".") > 0 Then
                Me.m_NetMask = GetBytes(netMask)
            Else
                Me.m_NetMask = GetMaskBytes32(CInt(netMask))
            End If
            Me.m_Allow = True
        End Sub

        Public Sub New(ByVal ip As String, ByVal mask As String)
            Me.m_IP = GetBytes(ip)
            Me.m_NetMask = GetBytes(mask)
            Me.m_Allow = True
        End Sub

        Public Property IP As Byte()
            Get
                Return Me.m_IP
            End Get
            Set(value As Byte())
                Me.m_IP = value
            End Set
        End Property

        Public Property NetMask As Byte()
            Get
                Return Me.m_NetMask
            End Get
            Set(value As Byte())
                Me.m_NetMask = value
            End Set
        End Property

        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Me.m_Descrizione = value
            End Set
        End Property

        Public Property Allow As Boolean
            Get
                Return Me.m_Allow
            End Get
            Set(value As Boolean)
                Me.m_Allow = value
            End Set
        End Property

        Public Property Negate As Boolean
            Get
                Return Me.m_Negate
            End Get
            Set(value As Boolean)
                Me.m_Negate = value
            End Set
        End Property

        Public Function Match(ByVal ip As String) As Boolean
            Dim bytes() As Byte = GetBytes(ip)
            For i As Integer = 0 To Math.Min(UBound(Me.m_IP), UBound(Me.m_NetMask))
                If (bytes(i) And Me.m_NetMask(i)) <> (Me.m_IP(i) And Me.m_NetMask(i)) Then Return False
            Next
            Return True
        End Function

        Public Function GetBytes(ByVal ip As String) As Byte()
            Dim items() As String
            Dim bytes() As Byte = Nothing
            If InStr(ip, "::") > 0 Then
                ReDim bytes(7)
            Else
                items = Split(ip, ".")
                If (UBound(items) = 3) Then
                    ReDim bytes(UBound(items))
                    For i As Integer = 0 To UBound(items)
                        bytes(i) = CByte(items(i))
                    Next
                Else
                    Throw New ArgumentException("L'indirizzo [" & ip & "] non sembra un indirizzo IPv4 o IPv6")
                End If
            End If
            Return bytes
        End Function

        Public Function GetMaskBytes32(ByVal bits As Integer) As Byte()
            Dim bytes() As Byte
            Dim i As Integer
            i = 0
            ReDim bytes(3)
            While (bits > 8)
                bytes(i) = GetHighBits(8)
                i = i + 1
                bits -= 8
            End While
            bytes(i) = GetHighBits(bits)
            Return bytes
        End Function

        Private Function GetHighBits(ByVal bits As Integer) As Byte
            Dim ret As Integer = 0
            While (bits > 0)
                bits -= 1
                ret = ret Or (2 ^ bits)
            End While
            Return CByte(ret)
        End Function

        Public Overrides Function GetModule() As CModule
            Return WebSite.Instance.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_AllowedIPs"
        End Function

        Private Function FromString(ByVal str As String) As Byte()
            Dim ret() As Byte
            Dim tmp() As String
            tmp = Split(Trim(str), ".")
            ReDim ret(UBound(tmp))
            For i As Integer = 0 To UBound(tmp)
                ret(i) = CByte(tmp(i))
            Next
            Return ret
        End Function

        Private Overloads Function ToString(ByVal items() As Byte) As String
            Dim tmp() As String
            ReDim tmp(UBound(items))
            For i As Integer = 0 To UBound(tmp)
                tmp(i) = CStr(items(i))
            Next
            Return Join(tmp, ".")
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se considerare l'indirizzo come interno all'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Interno As Boolean
            Get
                Return Me.m_Interno
            End Get
            Set(value As Boolean)
                If (Me.m_Interno = value) Then Exit Property
                Me.m_Interno = value
                Me.DoChanged("Interno", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'ufficio da associare al login in caso l'utnte effettui l'accesso da un IP consentito da questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AssociaUfficio As String
            Get
                Return Me.m_AssociaUfficio
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_AssociaUfficio
                If (oldValue = value) Then Exit Property
                Me.m_AssociaUfficio = value
                Me.DoChanged("AssociaUfficio", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.IP = Me.FromString(reader.GetValue("IP", vbNullString))
            Me.NetMask = Me.FromString(reader.GetValue("NetMask", vbNullString))
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Allow = reader.Read("Allow", Me.m_Allow)
            Me.m_Negate = reader.Read("Negate", Me.m_Negate)
            Me.m_Interno = reader.Read("Interno", Me.m_Interno)
            Me.m_AssociaUfficio = reader.Read("AssociaUfficio", Me.m_AssociaUfficio)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IP", Me.ToString(Me.IP))
            writer.Write("NetMask", Me.ToString(Me.NetMask))
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Allow", Me.m_Allow)
            writer.Write("Negate", Me.m_Negate)
            writer.Write("Interno", Me.m_Interno)
            writer.Write("AssociaUfficio", Me.m_AssociaUfficio)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IP", Me.ToString(Me.IP))
            writer.WriteAttribute("NetMask", Me.ToString(Me.NetMask))
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Allow", Me.m_Allow)
            writer.WriteAttribute("Negate", Me.m_Negate)
            writer.WriteAttribute("Interno", Me.m_Interno)
            writer.WriteAttribute("AssociaUfficio", Me.m_AssociaUfficio)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IP" : Me.m_IP = Me.FromString(fieldValue)
                Case "NetMask" : Me.m_NetMask = Me.FromString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Allow" : Me.m_Allow = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Negate" : Me.m_Negate = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Interno" : Me.m_Interno = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "AssociaUfficio" : Me.m_AssociaUfficio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.ToString(Me.m_IP) & "/" & Me.ToString(Me.m_NetMask)
        End Function

        Public Function CompareTo(obj As IPADDRESSinfo) As Integer
            If Me.Negate And Not obj.Negate Then
                Return -1
            ElseIf Not Me.Negate And obj.Negate Then
                Return 1
            Else
                Return Strings.Compare(Me.ToString, obj.ToString)
            End If
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class

End Class
