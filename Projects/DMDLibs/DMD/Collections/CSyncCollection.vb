Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net
Imports DMD.Sistema

Public Class CSyncCollection
    Inherits CCollection

    Private lockObject As New Object

    Public Sub New()
    End Sub

    Public Overrides ReadOnly Property IsSynchronized As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property SyncRoot As Object
        Get
            Return Me.lockObject
        End Get
    End Property
 
End Class
