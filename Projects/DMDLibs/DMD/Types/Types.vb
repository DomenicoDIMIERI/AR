Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema
Imports DMD.Internals


Namespace Internals

    Public NotInheritable Class CTypesClass
        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Delegate Function GetItemByIDFun(ByVal id As Integer) As DBObjectBase

        Private m_Imports As New CImportedNameSpaces
        Private m_TypeHandlers As NewTypeHandlersCollection
        Private m_ReferencedAssemblies As CKeyCollection(Of System.Reflection.Assembly)
        Private lockObject As New Object
        Private m_LoadedTypes As New CKeyCollection(Of System.Type)
        Private m_RegisteredTypeProviders As CKeyCollection(Of GetItemByIDFun) = Nothing

        ''' <summary>
        ''' Restituisec la collezione dei gestori di tipi esterni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NewTypeHandlers As NewTypeHandlersCollection
            Get
                If (m_TypeHandlers Is Nothing) Then m_TypeHandlers = New NewTypeHandlersCollection
                Return m_TypeHandlers
            End Get
        End Property

        ''' <summary>
        ''' Restituisce vero se l'oggetto obj espone il membro pubblico methodName
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="methodName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MethodExists(ByVal obj As Object, ByVal methodName As String) As Boolean
            Dim m As System.Reflection.MethodInfo = obj.GetType.GetMethod(methodName, Reflection.BindingFlags.FlattenHierarchy Or Reflection.BindingFlags.InvokeMethod)
            Return m IsNot Nothing
        End Function

        Public Function PropertyExists(ByVal obj As Object, ByVal propName As String) As Boolean
            Dim p As System.Reflection.PropertyInfo = obj.GetType.GetProperty(propName, Reflection.BindingFlags.FlattenHierarchy Or Reflection.BindingFlags.InvokeMethod)
            Return p IsNot Nothing
        End Function

        Public Function TypeExits(ByVal typeName As String) As Boolean
            Dim t As System.Type = System.Reflection.Assembly.GetCallingAssembly.GetType(typeName)
            Return (t IsNot Nothing)
        End Function

        ''' <summary>
        ''' Copia tutti i campi di source in target (fields)
        ''' Gli oggetti vengono copiati per riferimento
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="target"></param>
        ''' <remarks></remarks>
        Public Sub Copy(ByVal source As Object, ByRef target As Object)
            Me.Copy(source, target, source.GetType)
        End Sub

        Private Sub Copy(ByVal source As Object, ByRef target As Object, ByVal t As System.Type)
            Dim m As System.Reflection.FieldInfo()
            m = t.GetFields(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            For Each p As System.Reflection.FieldInfo In m
                Dim value As Object = p.GetValue(source)
                p.SetValue(target, value)
            Next
            If (t.BaseType IsNot Nothing) Then Me.Copy(source, target, t.BaseType)
        End Sub


        ''' <summary>
        ''' Restituisce tutti i campi definiti per il tipo e per le sue superclassi
        ''' </summary>
        ''' <param name="t"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllFields(ByVal t As System.Type) As System.Reflection.FieldInfo()
            Return DMD.DMDObject.GetAllFields(t)
        End Function

        ''' <summary>
        ''' Compare tutti i campi di a con quelli di b.
        ''' Gli oggetti vengono comparati per riferimento.
        ''' </summary>
        ''' <param name="a"></param>
        ''' <param name="b"></param>
        ''' <remarks></remarks>
        Public Function BitwiseCompare(ByVal a As Object, ByVal b As Object) As Boolean
            Dim m As System.Reflection.FieldInfo() = Me.GetAllFields(a.GetType)
            Dim ret As Boolean
            ret = True
            For Each p As System.Reflection.FieldInfo In m
                Dim v1 As Object = p.GetValue(a)
                Dim v2 As Object = p.GetValue(b)
                If TypeOf (v1) Is Object AndAlso TypeOf (v2) Is Object Then
                    ret = v1 Is v2
                ElseIf Not (TypeOf (v1) Is Object AndAlso TypeOf (v2) Is Object) Then
                    ret = v1.Equals(v2)
                Else
                    ret = False
                End If
                If (ret = False) Then Return False
            Next
            Return True
        End Function

        Public Function GetTypeCode(ByVal value As Object) As TypeCode
            If (value Is Nothing) Then Return TypeCode.Empty
            Return Type.GetTypeCode(value.GetType)
        End Function

        Public Function GetTypeCode(ByVal value As System.Type) As TypeCode
            Return Type.GetTypeCode(value)
        End Function

        ''' <summary>
        ''' Aggiunge l'assembly agli elementi disponibili per la funzione GetType
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Public Sub AddReferencedAssembly(ByVal item As System.Reflection.Assembly)
            SyncLock Me.lockObject
                If Me.m_ReferencedAssemblies Is Nothing Then Me.InitReferencedAssemblies()
                If Me.m_ReferencedAssemblies.ContainsKey(item.FullName) Then Throw New DuplicateNameException("Assemblu già aggiunto")
                Me.m_ReferencedAssemblies.Add(item.FullName, item)
            End SyncLock
        End Sub

        ''' <summary>
        ''' Rimuove l'assembly dagli elementi disponibili per la funzione GetType
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Public Sub RemoveReferencedAssembly(ByVal item As System.Reflection.Assembly)
            SyncLock Me.lockObject
                Me.m_ReferencedAssemblies.Remove(item)
            End SyncLock
        End Sub

        ''' <summary>
        ''' Inizializza l'elenco degli assemblies utilizzati dall'applicazione
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitReferencedAssemblies()
            Me.m_ReferencedAssemblies = New CKeyCollection(Of System.Reflection.Assembly)
            Dim item As System.Reflection.Assembly = System.Reflection.Assembly.GetEntryAssembly
            If (item Is Nothing) Then item = Sistema.ApplicationContext.GetEntryAssembly
            AssRefAssRec(Me.m_ReferencedAssemblies, item)
        End Sub

        ''' <summary>
        ''' Restituisce una copia di tutti gli assemblies utilizzati dall'applicazione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllReferencedAssemblies() As CKeyCollection(Of System.Reflection.Assembly)
            SyncLock Me.lockObject
                If Me.m_ReferencedAssemblies Is Nothing Then Me.InitReferencedAssemblies()
                Return New CKeyCollection(Of System.Reflection.Assembly)(Me.m_ReferencedAssemblies)
            End SyncLock
        End Function

        ''' <summary>
        ''' Aggiunge l'assembly e tutti gli assembly a cui esso fa riferimento
        ''' </summary>
        ''' <param name="col"></param>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Private Sub AssRefAssRec(ByVal col As CKeyCollection(Of System.Reflection.Assembly), ByVal item As System.Reflection.Assembly)
            If col.ContainsKey(item.FullName) Then Exit Sub
            col.Add(item.FullName, item)
            For Each aN As System.Reflection.AssemblyName In item.GetReferencedAssemblies
                Try
                    Dim currItem As System.Reflection.Assembly = System.Reflection.Assembly.Load(aN)
                    AssRefAssRec(col, currItem)
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try
            Next
        End Sub

        ''' <summary>
        ''' Restituisce l'elenco dei namespace importati. I namespace importati consentono di instanziare una classe senza dover specificare il nome completo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property [Imports] As CImportedNameSpaces
            Get
                Return m_Imports
            End Get
        End Property



        Public Overloads Function [GetType](ByVal typeName As String) As System.Type
            typeName = Strings.Trim(typeName)
            If (typeName = "") Then Return Nothing
            SyncLock Me.lockObject
                Dim t As System.Type = m_LoadedTypes.GetItemByKey(typeName)
                If (t IsNot Nothing) Then Return t

                Dim i As Integer
                If (InStr(typeName, ".") > 0) OrElse (InStr(typeName, "+") > 0) Then
                    Dim items As CKeyCollection(Of Reflection.Assembly) = GetAllReferencedAssemblies()
                    i = 0
                    While (i < items.Count)
                        Dim item As System.Reflection.Assembly = items(i)
                        t = item.GetType(typeName)
                        If (t IsNot Nothing) Then Exit While
                        i += 1
                    End While
                Else
                    Select Case typeName
                        Case "String" : t = GetType(String)
                        Case "Boolean" : t = GetType(Boolean)
                        Case "Byte" : t = GetType(Byte)
                        Case "SByte" : t = GetType(SByte)
                        Case "Short", "Int16" : t = GetType(Int16)
                        Case "UShort", "UInt16" : t = GetType(UInt16)
                        Case "Integer", "Int32" : t = GetType(Int32)
                        Case "UInteger", "UInt32" : t = GetType(UInt32)
                        Case "Long", "Int64" : t = GetType(Int64)
                        Case "ULong", "UInt64" : t = GetType(UInt64)
                        Case "Single" : t = GetType(Single)
                        Case "Double" : t = GetType(Double)
                        Case "Decimal" : t = GetType(Decimal)
                        Case "Nothing" : t = GetType(DBNull)
                        Case "Object" : t = GetType(Object)
                        Case Else
                            i = 0
                            While (i < m_Imports.Count)
                                Dim ns As String = m_Imports(i)
                                t = [GetType](typeName, ns)
                                If (t IsNot Nothing) Then Exit While
                                i += 1
                            End While
                    End Select
                End If
                If (t Is Nothing) Then
                    i = 0
                    While (i < NewTypeHandlers.Count)
                        Dim h As NewTypeHandler = NewTypeHandlers(i)
                        t = h.FindType(typeName)
                        If (t IsNot Nothing) Then Exit While
                        i += 1
                    End While
                End If
                Me.m_LoadedTypes.Add(typeName, t)
                Return t
            End SyncLock
        End Function

        Private Overloads Function [GetType](ByVal typeName As String, ByVal [nameSpace] As String) As System.Type
            Dim t As System.Type = Nothing
            Dim i As Integer = 0
            Dim items As CKeyCollection(Of Reflection.Assembly) = GetAllReferencedAssemblies()
            While (i < items.Count)
                Dim item As System.Reflection.Assembly = items(i)
                If (item IsNot Nothing) Then
                    t = item.GetType([nameSpace] & "." & typeName)
                    If (t IsNot Nothing) Then Exit While
                    t = item.GetType([nameSpace] & "+" & typeName)
                    If (t IsNot Nothing) Then Exit While
                End If
                i += 1
            End While
            Return t
        End Function

        Public Function CreateInstance(ByVal typeName As String) As Object
            Select Case typeName
                Case "String" : Return New String("")
                Case "Boolean" : Return New Boolean
                Case "Byte" : Return New Byte
                Case "SByte" : Return New SByte
                Case "Short", "Int16" : Return New Int16
                Case "UShort", "UInt16" : Return New UInt16
                Case "Integer", "Int32" : Return New Int32
                Case "UInteger", "UInt32" : Return New UInt32
                Case "Long", "Int64" : Return New Int16
                Case "ULong", "UInt64" : Return New UInt64
                Case "Single" : Return New Single
                Case "Double" : Return New Double
                Case "Decimal" : Return New Decimal
                Case "Nothing" : Return Nothing
                Case "Date", "DateTime" : Return New DateTime
                Case "Array" : Return New Object() {}
                Case Else
                    Dim t As System.Type = [GetType](typeName)
                    If t Is Nothing Then
                        Throw New ArgumentException("Non riesco a trovare il tipo: " & typeName)
                        Return Nothing
                    End If
                    Try
                        Return Activator.CreateInstance(t) ' t.Assembly.CreateInstance(t.FullName)
                    Catch ex As Exception
                        Throw New ArgumentException("Impossibile creare un'istanza del tipo: " & typeName, ex)
                        Return Nothing
                    End Try
            End Select
        End Function

        Public Function CreateInstance(ByVal t As System.Type) As Object
            Return Activator.CreateInstance(t) ' t.Assembly.CreateInstance(t.FullName)
        End Function

        Public Function CastTo(ByVal value As Object, ByVal tipo As TypeCode) As Object
            Select Case tipo
                Case TypeCode.Boolean
                    If (TypeOf (value) Is String) Then
                        Select Case LCase(Trim(value))
                            Case "1", "t", "true" : Return True
                            Case "0", "f", "false" : Return False
                            Case Else
                                Return DBNull.Value
                        End Select
                    ElseIf TypeOf (value) Is Nullable(Of Boolean) Then
                        Return value
                    ElseIf IsNull(value) Then
                        Return DBNull.Value
                    Else
                        Return CType(value, Boolean)
                    End If
                Case TypeCode.Byte : Return CByte(Formats.ToInteger(value))
                Case TypeCode.Char : Return CChar(Formats.ToString(value))
                Case TypeCode.DateTime : Return Formats.ToDate(value)
                Case TypeCode.DBNull : Return DBNull.Value
                Case TypeCode.Decimal : Return Formats.ToValuta(value)
                Case TypeCode.Double : Return Formats.ToDouble(value)
                Case TypeCode.Empty : Return Nothing
                Case TypeCode.Int16 : Return CShort(Formats.ToInteger(value))
                Case TypeCode.Int32 : Return Formats.ToInteger(value)
                Case TypeCode.Int64 : Return CLng(Formats.ToInteger(value))
                Case TypeCode.Object : Return value
                Case TypeCode.SByte : Return CSByte(Formats.ToInteger(value))
                Case TypeCode.Single : Return CSng(Formats.ToDouble(value))
                Case TypeCode.String : Return Formats.ToString(value)
                Case TypeCode.UInt16 : Return CUShort(Formats.ToInteger(value))
                Case TypeCode.UInt32 : Return CUInt(Formats.ToInteger(value))
                Case TypeCode.UInt64 : Return CLng(Formats.ToInteger(value))
                Case Else
                    Throw New NotSupportedException
            End Select
        End Function

        Public Function CastTo(ByVal value As Object, ByVal tipo As System.Type) As Object
            If Nullable.GetUnderlyingType(tipo) Is Nothing Then
                Return CastTo(value, Type.GetTypeCode(tipo))
            Else
                If (value.Equals(Nothing)) Then
                    Return Nothing
                Else
                    Return CastTo(value, Type.GetTypeCode(Nullable.GetUnderlyingType(tipo)))
                End If
            End If
        End Function

        Public Function GetTypeFromCode(ByVal code As System.TypeCode) As System.Type
            Select Case code
                Case TypeCode.Boolean : Return GetType(Boolean)
                Case TypeCode.Byte : Return GetType(Byte)
                Case TypeCode.Char : Return GetType(Char)
                Case TypeCode.DateTime : Return GetType(Date)
                Case TypeCode.DBNull : Return GetType(DBNull)
                Case TypeCode.Decimal : Return GetType(Decimal)
                Case TypeCode.Double : Return GetType(Double)
                Case TypeCode.Empty : Return Nothing
                Case TypeCode.Int16 : Return GetType(Int16)
                Case TypeCode.Int32 : Return GetType(Int32)
                Case TypeCode.Int64 : Return GetType(Int64)
                Case TypeCode.Object : Return GetType(Object)
                Case TypeCode.SByte : Return GetType(SByte)
                Case TypeCode.Single : Return GetType(Single)
                Case TypeCode.String : Return GetType(String)
                Case TypeCode.UInt16 : Return GetType(UInt16)
                Case TypeCode.UInt32 : Return GetType(UInt32)
                Case TypeCode.UInt64 : Return GetType(UInt64)
                Case Else
                    Throw New NotSupportedException
            End Select
        End Function

        ''' <summary>
        ''' Restituisce il nome del tipo dell'oggetto eliminato eventuali parametrizzazioni
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function vbTypeName(ByVal obj As Object) As String
            Dim ret As String = TypeName(obj)
            Dim p As Integer = InStr(ret, "(")
            If (p > 0) Then
                ret = Left(ret, p - 1)
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce vero se il valore passato come argomento è un oggetto Nothing, un valore DBNull.Value oppure un NULLABLE senza valore
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsNull(ByVal value As Object) As Boolean
            If (TypeOf (value) Is DBNull) Then
                Return True
            ElseIf (value Is Nothing) Then
                Return True
            ElseIf IsNullableType(value.GetType) Then
                Return value.HasValue = False
            Else
                Return False
            End If
        End Function

        Public Function IsNullableType(ByVal myType As System.Type) As Boolean
            Return (myType.IsGenericType) AndAlso (myType.GetGenericTypeDefinition() Is GetType(Nullable(Of )))
        End Function

        Public Sub Initialize()

        End Sub


        Public ReadOnly Property RegisteredTypeProviders As CKeyCollection(Of GetItemByIDFun)
            Get
                SyncLock Me
                    If (Me.m_RegisteredTypeProviders Is Nothing) Then Me.m_RegisteredTypeProviders = New CKeyCollection(Of GetItemByIDFun)
                    Return Me.m_RegisteredTypeProviders
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto in base al tipo registrato ed al suo ID
        ''' </summary>
        ''' <param name="type">[in] Nome del provider che implementa la funzione GetItemByID</param>
        ''' <param name="id">[in] ID dell'oggetto da restituire</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByTypeAndId(ByVal type As String, ByVal id As Integer) As DBObjectBase
            type = Trim(type)
            If (type = "") Then Return Nothing
            Dim provider As GetItemByIDFun = Me.RegisteredTypeProviders.GetItemByKey(type)
            If (provider Is Nothing) Then Throw New ArgumentException("Nessun provider registrato per il tipo: " & type)
            Return provider(id)
        End Function






        Public Function CallMethod(ByVal obj As Object, ByVal methodName As String) As Object
            Dim m As System.Reflection.MemberInfo() = obj.GetType.GetMember(methodName)
            If m Is Nothing OrElse UBound(m) < 0 Then Throw New MissingMemberException(methodName)
            If TypeOf (m(0)) Is System.Reflection.MethodInfo Then
                Return DirectCast(m(0), System.Reflection.MethodInfo).Invoke(obj, Nothing)
            ElseIf TypeOf (m(0)) Is System.Reflection.PropertyInfo Then
                Return DirectCast(m(0), System.Reflection.PropertyInfo).GetValue(obj, Nothing)
            ElseIf TypeOf (m(0)) Is System.Reflection.FieldInfo Then
                Return DirectCast(m(0), System.Reflection.FieldInfo).GetValue(obj)
            Else
                Throw New EntryPointNotFoundException
            End If
        End Function

        Public Function Clone(ByVal obj As Object) As Object
            If (TypeOf (obj) Is ICloneable) Then
                Return DirectCast(obj, ICloneable).Clone
            Else
                Throw New ArgumentException("L'oggetto non implementa l'interfaccia ICloneable")
            End If
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace

Partial Class Sistema


    Private Shared m_Types As CTypesClass = Nothing

    Public Shared ReadOnly Property Types As CTypesClass
        Get
            If (m_Types Is Nothing) Then m_Types = New CTypesClass
            Return m_Types
        End Get
    End Property


End Class