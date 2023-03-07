Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.XML.Utils

Namespace XML

    Public Class XMLWriter

        Private m_Encoding As String
        Private m_OpenedTags As New CCollection(Of String)
        'Private m_Value As String = vbNullString
        Private m_Buffer As New System.Text.StringBuilder(2048)
        'Private m_Buffer1 As New System.Text.StringBuilder(2048)
        Private m_Settings As DMD.Sistema.CSettings = Nothing
        Private m_Method As XMLSerializeMethod = XMLSerializeMethod.Document
        Private m_BaseElemType As String = vbNullString
        Private m_IsArray As Boolean = False
        Private m_IsInTag As Boolean = False
        ' Private m_Attributes As New CKeyCollection(Of String)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Encoding = XML.Utils.Serializer.Encoding
        End Sub

        Public Property Encoding As String
            Get
                Return Me.m_Encoding
            End Get
            Set(value As String)
                Me.m_Encoding = value
            End Set
        End Property

        Public ReadOnly Property Settings As DMD.Sistema.CSettings
            Get
                If (Me.m_Settings Is Nothing) Then Me.m_Settings = New Sistema.CSettings
                Return Me.m_Settings
            End Get
        End Property

        Public ReadOnly Property Length As Integer
            Get
                Return Me.m_Buffer.Length
            End Get
        End Property

        ''' <summary>
        ''' Aggiunge una stringa di testo senza formattazione
        ''' </summary>
        ''' <param name="text"></param>
        ''' <remarks></remarks>
        Public Sub WriteRowString(ByVal text As String)
            Me.m_Buffer.Append(text)
        End Sub

        ''' <summary>
        ''' Aggiunge una stringa di testo senza formattazione
        ''' </summary>
        ''' <param name="text"></param>
        ''' <remarks></remarks>
        Public Sub WriteRowData(ByVal text As String)
            Me.m_Buffer.Append(text)
        End Sub

        Public Sub BeginTag(ByVal tagName As String)
            Me.CheckInTag()
            Me.m_OpenedTags.Add(Trim(tagName))
            Me.m_Buffer.Append("<" & tagName)
            Me.m_IsInTag = True
            'Me.m_CurrTag = tagName
            'Me.m_Value = ""
        End Sub

        Public Sub WriteAttribute1(ByVal key As String, ByVal value As Object)
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            If (TypeOf (value) Is Byte) Then
                Me.WriteAttribute(key, DirectCast(value, Byte))
            ElseIf (TypeOf (value) Is Nullable(Of Byte)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Byte)))
            ElseIf (TypeOf (value) Is Short) Then
                Me.WriteAttribute(key, DirectCast(value, Short))
            ElseIf (TypeOf (value) Is Nullable(Of Short)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Short)))
            ElseIf (TypeOf (value) Is Integer) Then
                Me.WriteAttribute(key, DirectCast(value, Integer))
            ElseIf (TypeOf (value) Is Nullable(Of Integer)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Integer)))
            ElseIf (TypeOf (value) Is Single) Then
                Me.WriteAttribute(key, DirectCast(value, Single))
            ElseIf (TypeOf (value) Is Nullable(Of Single)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Single)))
            ElseIf (TypeOf (value) Is Double) Then
                Me.WriteAttribute(key, DirectCast(value, Double))
            ElseIf (TypeOf (value) Is Nullable(Of Double)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Double)))
            ElseIf (TypeOf (value) Is Long) Then
                Me.WriteAttribute(key, DirectCast(value, Long))
            ElseIf (TypeOf (value) Is Nullable(Of Long)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Long)))
            ElseIf (TypeOf (value) Is Decimal) Then
                Me.WriteAttribute(key, DirectCast(value, Decimal))
            ElseIf (TypeOf (value) Is Nullable(Of Decimal)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Decimal)))
            ElseIf (TypeOf (value) Is String) Then
                Me.WriteAttribute(key, DirectCast(value, String))
            ElseIf (TypeOf (value) Is Boolean) Then
                Me.WriteAttribute(key, DirectCast(value, Boolean))
            ElseIf (TypeOf (value) Is Nullable(Of Boolean)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Boolean)))
            ElseIf (TypeOf (value) Is Date) Then
                Me.WriteAttribute(key, DirectCast(value, Date))
            ElseIf (TypeOf (value) Is Nullable(Of Date)) Then
                Me.WriteAttribute(key, DirectCast(value, Nullable(Of Date)))
            Else
                Me.WriteAttribute(key, XML.Utils.Serializer.SerializeObject(value))
            End If
        End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As String)
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeString(value)
            text = XML.Utils.Serializer.SerializeString(text)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            'Me.m_Attributes.Add(key, text)
        End Sub


        'Public Sub WriteAttribute(ByVal key As String, ByVal value As Integer)
        '    If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
        '    Dim text As String = XML.Utils.Serializer.SerializeInteger(value)
        '    Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
        '    '  Me.m_Attributes.Add(key, text)
        'End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As Nullable(Of Integer))
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeInteger(value)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            '  Me.m_Attributes.Add(key, text)
        End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As Nullable(Of Double))
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeDouble(value)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            ' Me.m_Attributes.Add(key, text)
        End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As Nullable(Of Boolean))
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeBoolean(value)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            ' Me.m_Attributes.Add(key, text)
        End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As Nullable(Of Decimal))
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeDouble(value)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            ' Me.m_Attributes.Add(key, text)
        End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As Nullable(Of Date))
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeDate(value)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            ' Me.m_Attributes.Add(key, text)
        End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As Nullable(Of Long))
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeInteger(value)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            ' Me.m_Attributes.Add(key, text)
        End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As Nullable(Of Byte))
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeInteger(value)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            ' Me.m_Attributes.Add(key, text)
        End Sub

        Public Sub WriteAttribute(ByVal key As String, ByVal value As Nullable(Of Short))
            If Not Me.m_IsInTag Then Throw New InvalidOperationException("WriteAttribute non può essere usata dopo aver scritto il contenuto del nodo")
            Dim text As String = XML.Utils.Serializer.SerializeInteger(value)
            Me.m_Buffer.Append(" " & key & "=""" & text & """ ")
            ' Me.m_Attributes.Add(key, text)
        End Sub





        Public Overridable Function CanCloseTag(ByVal tag As String) As Boolean
            Select Case UCase(Trim(tag))
                Case "IMG", "INPUT", "METADATA", "BR", "HR" : Return True
                Case Else : Return False
            End Select
        End Function

        Public Sub EndTag()
            Me.CheckInTag()
            Dim currTag As String = Me.m_OpenedTags(Me.m_OpenedTags.Count - 1)
            Me.m_OpenedTags.RemoveAt(Me.m_OpenedTags.Count - 1)
            If (Me.CanCloseTag(currTag)) Then
                'Me.m_Buffer.Append("/>")
            Else
                Me.m_Buffer.Append("</" & currTag & ">")
            End If
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Boolean))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteArray(ByVal value As Array)
            Me.CheckInTag()
            'Me.m_Buffer.Append(XML.Utils.Serializer.SerializeArray(value))
            If (value IsNot Nothing) Then
                For i As Integer = 0 To UBound(value)
                    Dim o As Object = value.GetValue(i)
                    Me.CheckInTag()
                    Me.BeginTag(XMLTypeName(o))
                    If (TypeOf (o) Is Boolean OrElse TypeOf (o) Is Nullable(Of Boolean)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeBoolean(o))
                    ElseIf (TypeOf (o) Is Byte OrElse TypeOf (o) Is Nullable(Of Byte)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Short OrElse TypeOf (o) Is Nullable(Of Short)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Integer OrElse TypeOf (o) Is Nullable(Of Integer)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Long OrElse TypeOf (o) Is Nullable(Of Long)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Single OrElse TypeOf (o) Is Nullable(Of Single)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Double OrElse TypeOf (o) Is Nullable(Of Double)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Decimal OrElse TypeOf (o) Is Nullable(Of Decimal)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Date OrElse TypeOf (o) Is Nullable(Of Date)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDate(o))
                        'Me.CheckInTag()
                    ElseIf (TypeOf (o) Is String) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeString(o))
                    ElseIf (IsArray(o)) Then
                        'Throw New Exception("Errore: Tipo non previsto in WriteArray")
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeArray(o))
                    ElseIf TypeOf (o) Is DMD.XML.IDMDXMLSerializable Then
                        DirectCast(o, DMD.XML.IDMDXMLSerializable).XMLSerialize(Me)
                    ElseIf (o Is Nothing) OrElse TypeOf (o) Is DBNull Then
                        'no
                        Me.CheckInTag()
                        ' Me.m_Buffer.Append("<Nothing></Nothing>")
                    Else
                        Throw New ArgumentException("Tipo non serializzabile: " & TypeName(o))
                    End If
                    Me.EndTag()
                Next
            End If
        End Sub

        Public Sub Write(Of T)(ByVal value() As T, ByVal basicType As String)
            Me.CheckInTag()
            'Me.m_Buffer.Append(XML.Utils.Serializer.SerializeArray(value, basicType, Me))
            If (value IsNot Nothing) Then
                For i As Integer = 0 To UBound(value)
                    Dim o As Object = value(i)
                    Me.CheckInTag()
                    Me.BeginTag(basicType)
                    If (TypeOf (o) Is Boolean OrElse TypeOf (o) Is Nullable(Of Boolean)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeBoolean(o))
                    ElseIf (TypeOf (o) Is Byte OrElse TypeOf (o) Is Nullable(Of Byte)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Short OrElse TypeOf (o) Is Nullable(Of Short)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Integer OrElse TypeOf (o) Is Nullable(Of Integer)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Long OrElse TypeOf (o) Is Nullable(Of Long)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Single OrElse TypeOf (o) Is Nullable(Of Single)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Double OrElse TypeOf (o) Is Nullable(Of Double)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Decimal OrElse TypeOf (o) Is Nullable(Of Decimal)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Date OrElse TypeOf (o) Is Nullable(Of Date)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDate(o))
                    ElseIf (TypeOf (o) Is String) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeString(o))
                    ElseIf (IsArray(o)) Then
                        Debug.Print("x")
                    ElseIf TypeOf (o) Is DMD.XML.IDMDXMLSerializable Then
                        DirectCast(o, DMD.XML.IDMDXMLSerializable).XMLSerialize(Me)
                    ElseIf o Is Nothing OrElse TypeOf (o) Is DBNull Then

                    Else
                        Throw New ArgumentException("Tipo non serializzabile: " & TypeName(o))
                    End If
                    Me.EndTag()
                Next
            End If
        End Sub

        Public Sub WriteTag(Of T)(ByVal tagname As String, ByVal value() As T)
            Me.BeginTag(tagname)

            If (value IsNot Nothing) Then
                For i As Integer = 0 To UBound(value)
                    Dim o As Object = value(i)
                    Me.CheckInTag()
                    Me.BeginTag(XMLTypeName(o))
                    If (TypeOf (o) Is Boolean OrElse TypeOf (o) Is Nullable(Of Boolean)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeBoolean(o))
                    ElseIf (TypeOf (o) Is Byte OrElse TypeOf (o) Is Nullable(Of Byte)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Short OrElse TypeOf (o) Is Nullable(Of Short)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Integer OrElse TypeOf (o) Is Nullable(Of Integer)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Long OrElse TypeOf (o) Is Nullable(Of Long)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Single OrElse TypeOf (o) Is Nullable(Of Single)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Double OrElse TypeOf (o) Is Nullable(Of Double)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Decimal OrElse TypeOf (o) Is Nullable(Of Decimal)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Date OrElse TypeOf (o) Is Nullable(Of Date)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDate(o))
                    ElseIf (TypeOf (o) Is String) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeString(o))
                    ElseIf (IsArray(o)) Then
                        Debug.Print("x")
                    ElseIf TypeOf (o) Is DMD.XML.IDMDXMLSerializable Then
                        DirectCast(o, DMD.XML.IDMDXMLSerializable).XMLSerialize(Me)
                    ElseIf (o Is Nothing) OrElse TypeOf (o) Is DBNull Then
                        Me.CheckInTag()
                    Else
                        Throw New ArgumentException("Tipo non serializzabile: " & TypeName(o))
                    End If
                    Me.EndTag()
                Next
            End If
            Me.EndTag()
        End Sub

        Public Sub WriteTag(Of T)(ByVal tagname As String, ByVal value() As T, ByVal basicType As String)
            Me.BeginTag(tagname)
            If (value IsNot Nothing) Then
                For i As Integer = 0 To UBound(value)
                    Dim o As Object = value(i)
                    Me.CheckInTag()
                    Me.BeginTag(basicType)
                    If (TypeOf (o) Is Boolean OrElse TypeOf (o) Is Nullable(Of Boolean)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeBoolean(o))
                    ElseIf (TypeOf (o) Is Byte OrElse TypeOf (o) Is Nullable(Of Byte)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Short OrElse TypeOf (o) Is Nullable(Of Short)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Integer OrElse TypeOf (o) Is Nullable(Of Integer)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Long OrElse TypeOf (o) Is Nullable(Of Long)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Single OrElse TypeOf (o) Is Nullable(Of Single)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Double OrElse TypeOf (o) Is Nullable(Of Double)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Decimal OrElse TypeOf (o) Is Nullable(Of Decimal)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Date OrElse TypeOf (o) Is Nullable(Of Date)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDate(o))
                    ElseIf (TypeOf (o) Is String) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeString(o))
                    ElseIf (IsArray(o)) Then
                        Debug.Print("x")
                    ElseIf TypeOf (o) Is DMD.XML.IDMDXMLSerializable Then
                        DirectCast(o, DMD.XML.IDMDXMLSerializable).XMLSerialize(Me)
                    ElseIf (o Is Nothing) OrElse TypeOf (o) Is DBNull Then
                        Me.CheckInTag()
                    Else
                        Throw New ArgumentException("Tipo non serializzabile: " & TypeName(o))
                    End If
                    Me.EndTag()
                Next
            End If
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Object)
            If (TypeOf (value) Is Boolean OrElse TypeOf (value) Is Nullable(Of Boolean)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeBoolean(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is Byte OrElse TypeOf (value) Is Nullable(Of Byte)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is Short OrElse TypeOf (value) Is Nullable(Of Short)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is Integer OrElse TypeOf (value) Is Nullable(Of Integer)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is Long OrElse TypeOf (value) Is Nullable(Of Long)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is Single OrElse TypeOf (value) Is Nullable(Of Single)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is Double OrElse TypeOf (value) Is Nullable(Of Double)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is Decimal OrElse TypeOf (value) Is Nullable(Of Decimal)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is Date OrElse TypeOf (value) Is Nullable(Of Date)) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDate(value))
                Me.EndTag()
            ElseIf (TypeOf (value) Is String) Then
                Me.BeginTag(tagname)
                Me.CheckInTag()
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeString(value))
                Me.EndTag()
            ElseIf (IsArray(value)) Then
                Me.CheckInTag()
                Me.BeginTag(tagname)
                Dim arr As Array = value
                For i As Integer = 0 To arr.Length - 1
                    Dim o As Object = arr.GetValue(i)
                    Me.BeginTag(XMLTypeName(o))
                    If (TypeOf (o) Is Boolean OrElse TypeOf (o) Is Nullable(Of Boolean)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeBoolean(o))
                    ElseIf (TypeOf (o) Is Byte OrElse TypeOf (o) Is Nullable(Of Byte)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Short OrElse TypeOf (o) Is Nullable(Of Short)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Integer OrElse TypeOf (o) Is Nullable(Of Integer)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Long OrElse TypeOf (o) Is Nullable(Of Long)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(o))
                    ElseIf (TypeOf (o) Is Single OrElse TypeOf (o) Is Nullable(Of Single)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Double OrElse TypeOf (o) Is Nullable(Of Double)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Decimal OrElse TypeOf (o) Is Nullable(Of Decimal)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(o))
                    ElseIf (TypeOf (o) Is Date OrElse TypeOf (o) Is Nullable(Of Date)) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDate(o))
                    ElseIf (TypeOf (o) Is String) Then
                        Me.CheckInTag()
                        Me.m_Buffer.Append(XML.Utils.Serializer.SerializeString(o))
                    ElseIf (IsArray(o)) Then
                        Debug.Print("x")
                    ElseIf TypeOf (o) Is DMD.XML.IDMDXMLSerializable Then
                        DirectCast(o, DMD.XML.IDMDXMLSerializable).XMLSerialize(Me)
                    ElseIf (o Is Nothing) OrElse TypeOf (o) Is DBNull Then
                        Me.CheckInTag()
                    Else
                        Throw New ArgumentException("Tipo non serializzabile: " & TypeName(o))
                    End If
                    Me.EndTag()
                Next
                Me.EndTag()
            ElseIf TypeOf (value) Is DMD.XML.IDMDXMLSerializable Then
                Me.CheckInTag()
                Me.BeginTag(tagname)
                Me.BeginTag(XMLTypeName(value))
                DirectCast(value, DMD.XML.IDMDXMLSerializable).XMLSerialize(Me)
                Me.EndTag()
                Me.EndTag()
            ElseIf Sistema.Types.IsNull(value) Then
                Me.CheckInTag()
                Me.BeginTag(tagname)
                Me.EndTag()
            Else
                Throw New ArgumentException("Tipo non serializzabile: " & TypeName(value))
            End If
        End Sub


        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Byte))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Short))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Integer))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Long))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Single))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Double))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Decimal))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As Nullable(Of Date))
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        Public Sub WriteTag(ByVal tagname As String, ByVal value As String)
            Me.BeginTag(tagname)
            Me.Write(value)
            Me.EndTag()
        End Sub

        ''' <summary>
        ''' Controlla lo stato del writer. Se siamo all'interno di un tag (è stata aperta una parentesi angolata, ma non è ancora stata chiusa), chiude il tag
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CheckInTag()
            If (Me.m_IsInTag) Then
                If Me.CanCloseTag(Me.m_OpenedTags(Me.m_OpenedTags.Count - 1)) Then
                    Me.m_Buffer.Append("/>")
                Else
                    Me.m_Buffer.Append(">")
                End If
            End If
            Me.m_IsInTag = False
        End Sub

        Public Sub Write(ByVal value As Object)
            Me.CheckInTag()
            If (IsArray(value)) Then
                If (Sistema.Arrays.Len(value) > 0) Then
                    Me.WriteArray(value)
                End If
            ElseIf (TypeOf (value) Is Boolean OrElse TypeOf (value) Is Nullable(Of Boolean)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeBoolean(value))
            ElseIf (TypeOf (value) Is Byte OrElse TypeOf (value) Is Nullable(Of Byte)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
            ElseIf (TypeOf (value) Is Short OrElse TypeOf (value) Is Nullable(Of Short)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
            ElseIf (TypeOf (value) Is Integer OrElse TypeOf (value) Is Nullable(Of Integer)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
            ElseIf (TypeOf (value) Is Long OrElse TypeOf (value) Is Nullable(Of Long)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
            ElseIf (TypeOf (value) Is Single OrElse TypeOf (value) Is Nullable(Of Single)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
            ElseIf (TypeOf (value) Is Double OrElse TypeOf (value) Is Nullable(Of Double)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
            ElseIf (TypeOf (value) Is Decimal OrElse TypeOf (value) Is Nullable(Of Decimal)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
            ElseIf (TypeOf (value) Is Date OrElse TypeOf (value) Is Nullable(Of Date)) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDate(value))
            ElseIf (TypeOf (value) Is String) Then
                Me.m_Buffer.Append(XML.Utils.Serializer.SerializeString(value))
            ElseIf (TypeOf (value) Is DMD.XML.IDMDXMLSerializable) Then
                Me.BeginTag(XMLTypeName(value))
                DirectCast(value, DMD.XML.IDMDXMLSerializable).XMLSerialize(Me)
                Me.EndTag()
            End If
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Boolean))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeBoolean(value))
        End Sub

        Public Sub Write(ByVal value As String)
            Me.CheckInTag()
            Dim text As String = XML.Utils.Serializer.SerializeString(value)
            text = XML.Utils.Serializer.SerializeString(text)
            Me.m_Buffer.Append(text)
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Byte))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Short))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Integer))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Long))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeInteger(value))
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Single))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Double))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Date))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDate(value))
        End Sub

        Public Sub Write(ByVal value As Nullable(Of Decimal))
            Me.CheckInTag()
            Me.m_Buffer.Append(XML.Utils.Serializer.SerializeDouble(value))
        End Sub

   
        Public Overrides Function ToString() As String
            Dim ret As String = Me.m_Buffer.ToString
#If DEBUG Then
            If (InStr(ret, "<Nothing><Nothing></Nothing></Nothing>")) Then
                Debug.Print("test")
            End If
#End If
            Return ret
        End Function

        Public Sub BeginDocument(ByVal method As XMLSerializeMethod, ByVal obj As Object)
            Me.m_Method = method
            Select Case (method)
                Case XMLSerializeMethod.Document
                    Me.m_Buffer.Append("<?xml version=""1.0"" encoding=""" & Me.m_Encoding & """?>" & vbCrLf)
                    If (IsArray(obj)) Then
                        Dim arr As Array = obj
                        If (arr.Length > 0) Then
                            Me.m_BaseElemType = XMLTypeName(arr.GetValue(0))
                        Else
                            Me.m_BaseElemType = "Object"
                        End If
                        Me.m_IsArray = True
                        Me.m_Buffer.Append("<ArrayOf" & Me.m_BaseElemType & " xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" & vbCrLf)
                    Else
                        Me.m_IsArray = False
                        Me.m_BaseElemType = XMLTypeName(obj)
                        Me.m_Buffer.Append("<" & Me.m_BaseElemType & " xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" & vbCrLf)
                    End If
                Case Else

            End Select
        End Sub

        Public Sub EndDocument()
            Select Case (Me.m_Method)
                Case XMLSerializeMethod.Document
                    If (Me.m_IsArray) Then
                        Me.m_Buffer.Append("</ArrayOf" + Me.m_BaseElemType & ">")
                    Else
                        Me.m_Buffer.Append("</" + Me.m_BaseElemType + ">")
                    End If
            End Select
        End Sub

        Public Sub Clear()
            Me.m_OpenedTags.Clear()
            Me.m_Buffer.Clear()
            Me.m_BaseElemType = vbNullString
            Me.m_IsArray = False
            Me.m_IsInTag = False
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Namespace
