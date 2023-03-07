Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Cursore sulle sottocartelle
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class FindFolderCursor
        Implements IDisposable, DMD.XML.IDMDXMLSerializable

        Private m_Pattern As String
        Private m_AttributesMask As FileAttribute
        Private m_IncludeSubDirs As Boolean
        Private m_Results As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        Private m_Index As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Pattern = ""
            Me.m_AttributesMask = FileAttribute.Normal
            Me.m_Results = Nothing
            Me.m_Index = 0
            Me.m_IncludeSubDirs = False
        End Sub

        Public Sub New(ByVal pattern As String)
            Me.New(pattern, FileAttribute.Normal, False)
        End Sub

        Public Sub New(ByVal pattern As String, ByVal attributeMask As FileAttribute)
            Me.New(pattern, attributeMask, False)
        End Sub

        Public Sub New(ByVal pattern As String, ByVal attributeMask As FileAttribute, ByVal includeSubDirs As Boolean)
            Me.New
            Me.m_Pattern = Trim(pattern)
            Me.m_AttributesMask = attributeMask
            Me.m_IncludeSubDirs = includeSubDirs
            Me.m_Results = Nothing
            Me.m_Index = -1
        End Sub

        ''' <summary>
        ''' Restituisce il filtro di ricerca
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Pattern As String
            Get
                Return Me.m_Pattern
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la maschera di ricerca
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AttributesMask As FileAttribute
            Get
                Return Me.m_AttributesMask
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un valore booleano che indica se la ricerca include le sottocartelle
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IncludeSubDirs As Boolean
            Get
                Return Me.m_IncludeSubDirs
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il numero di files e/o cartelle corrispondenti al filtro
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Count() As Integer
            Me.CheckInit()
            Return Me.m_Results.Count
        End Function

        Public ReadOnly Property EOF As Boolean
            Get
                Me.CheckInit()
                Return (Me.m_Index >= Me.Count)
            End Get
        End Property

        Public Function MoveFirst() As Boolean
            Me.CheckInit()
            Me.m_Index = 0
            Return (Me.m_Index < Me.Count)
        End Function

        Public Function MoveNext() As Boolean
            Me.CheckInit()
            Me.m_Index = Math.Min(Me.m_Index + 1, Me.Count)
            Return (Me.m_Index < Me.Count)
        End Function

        Public Function MovePrev() As Boolean
            Me.CheckInit()
            Me.m_Index = Math.Max(Me.m_Index - 1, 0)
            Return (Me.m_Index < Me.Count)
        End Function

        Public Function MoveLast() As Boolean
            Me.CheckInit()
            Me.m_Index = Math.Max(Me.Count - 1, 0)
            Return (Me.m_Index < Me.Count)
        End Function

        Public ReadOnly Property Item As String
            Get
                Me.CheckInit()
                If (Me.m_Index < Me.Count) Then
                    Return Me.m_Results(Me.m_Index)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Private Sub CheckInit()
            If (Me.m_Index = -1) Then Me.Init()
        End Sub

        Private Sub Init()
            Dim path As String
            Dim wildChars As String
            If (InStr(Me.m_Pattern, "*") > 0 OrElse InStr(Me.m_Pattern, "?") > 0) Then
                path = Sistema.FileSystem.NormalizePath(Sistema.FileSystem.GetFolderName(Me.m_Pattern))
                wildChars = Mid(Me.m_Pattern, Len(path) + 1)
            Else
                path = Sistema.FileSystem.NormalizePath(Me.m_Pattern)
                wildChars = "*.*"
            End If
            If (Me.m_IncludeSubDirs) Then
                Me.m_Results = My.Computer.FileSystem.GetDirectories(path, FileIO.SearchOption.SearchAllSubDirectories, wildChars)
            Else
                Me.m_Results = My.Computer.FileSystem.GetDirectories(path, FileIO.SearchOption.SearchTopLevelOnly, wildChars)
            End If
            Me.m_Index = 0
        End Sub

        Public Sub Reset()
            Me.m_Index = -1
            Me.m_Results = Nothing
        End Sub


        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Reset()
            Me.m_Pattern = vbNullString
            Me.m_Results = Nothing
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Pattern" : Me.m_Pattern = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AttributesMask" : Me.m_AttributesMask = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IncludeSubDirs" : Me.m_IncludeSubDirs = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Results"
                    If (fieldValue Is Nothing) Then
                        Me.m_Results = Nothing
                    ElseIf IsArray(fieldValue) Then
                        Me.m_Results = New System.Collections.ObjectModel.ReadOnlyCollection(Of String)(fieldValue)
                    Else
                        Me.m_Results = New System.Collections.ObjectModel.ReadOnlyCollection(Of String)({fieldValue})
                    End If
                Case "Index" : Me.m_Index = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("Pattern", Me.m_Pattern)
            writer.WriteTag("AttributesMask", Me.m_AttributesMask)
            writer.WriteTag("IncludeSubDirs", Me.m_IncludeSubDirs)
            writer.WriteTag("Results", Me.GetResultsAsArray)
            writer.WriteTag("Index", Me.m_Index)
        End Sub

        Public Function GetResultsAsArray() As String()
            Me.CheckInit()
            Dim ret() As String = Nothing
            If (Me.Count > 0) Then
                ReDim ret(Me.Count - 1)
                For i As Integer = 0 To Me.Count - 1
                    ret(i) = Me.m_Results(i)
                Next
            End If
            Return ret
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class


 