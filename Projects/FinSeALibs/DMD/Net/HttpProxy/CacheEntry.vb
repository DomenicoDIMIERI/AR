Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net

Namespace Net.HTTPProxy

    Public Class CacheEntry
        Implements IComparable, IDisposable

        Public Key As CacheKey
        Public Expires As DateTime?
        Public DateStored As DateTime
        Public StatusCode As HttpStatusCode
        Public StatusDescription As String
        Public Headers As List(Of HttpHeader)
        Public FlagRemove As Boolean
        Public LastUsed As Date
        Public ContentLength As Integer
        Public Stream As System.IO.Stream

        Public Sub New()
            Me.DateStored = Now
            Me.LastUsed = Now
            Me.Stream = Nothing
            Me.ContentLength = 0
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim o As CacheEntry = obj
            Return -Me.LastUsed.CompareTo(o.LastUsed)
        End Function

        Public ReadOnly Property Size As Integer
            Get
                If (Me.Stream Is Nothing) Then
                    Return Me.ContentLength
                Else
                    Return Me.Stream.Length
                End If
            End Get
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' Per rilevare chiamate ridondanti

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    If (Me.Stream IsNot Nothing) Then Me.Stream.Dispose() : Me.Stream = Nothing
                End If

                ' TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di Finalize().
                ' TODO: impostare campi di grandi dimensioni su Null.
            End If
            disposedValue = True
        End Sub

        ' TODO: eseguire l'override di Finalize() solo se Dispose(disposing As Boolean) include il codice per liberare risorse non gestite.
        'Protected Overrides Sub Finalize()
        '    ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
            Dispose(True)
            ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

End Namespace
