Partial Public Class Sistema

    Public Class CImportedNameSpaces
        'Inherits System.Collections.Specialized.NameValueCollection '(Of String)
        Private keys() As String = {}


        Public Sub New()

        End Sub

        Public Sub Add(ByVal ns As String)
            SyncLock Me
                ns = Trim(ns)
                If (ns = "") Then Throw New ArgumentNullException("ns")
                If (Me.ContainsKey(ns) = False) Then
                    'MyBase.Add(ns, ns)
                    Dim i As Integer = Arrays.GetInsertPosition(Me.keys, ns, 0, Me.keys.Length)
                    Me.keys = Arrays.Insert(Me.keys, ns, i)
                End If
            End SyncLock
        End Sub

        Public Sub Remove(ByVal ns As String)
            SyncLock Me
                ns = Trim(ns)
                Dim i As Integer = Array.BinarySearch(Me.keys, ns)
                If (i > 0) Then Me.keys = Arrays.RemoveAt(Me.keys, i)
            End SyncLock
        End Sub

        Public Function ContainsKey(ByVal ns As String) As Boolean
            Return Me.IndexOfKey(ns) >= 0
        End Function

        Public Function IndexOfKey(ByVal ns As String) As Boolean
            SyncLock Me
                ns = Trim(ns)
                Return Array.BinarySearch(Me.keys, ns)
            End SyncLock
        End Function

        Public Function Count() As Integer
            SyncLock Me
                Return Me.keys.Length
            End SyncLock
        End Function

        Default Public ReadOnly Property Item(ByVal index As Integer) As String
            Get
                Return Me.keys(index)
            End Get
        End Property


    End Class


End Class
