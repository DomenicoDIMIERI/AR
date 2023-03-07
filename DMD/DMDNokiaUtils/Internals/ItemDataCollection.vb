Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Collezione di oggetti ItemData
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ItemDataCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal item As ItemData)
            MyBase.InnerList.Add(item)
        End Sub

        Public Function Add(ByVal tipo As String, ByVal valore As Object) As ItemData
            Dim item As New ItemData(tipo, valore)
            Me.Add(item)
            Return item
        End Function

        Public Function Add(ByVal tipo As String, ByVal sottotipo As String, ByVal valore As Object) As ItemData
            Dim item As New ItemData(tipo, sottotipo, valore)
            Me.Add(item)
            Return item
        End Function

        Default Public ReadOnly Property Item(ByVal index As Integer) As ItemData
            Get
                Return DirectCast(MyBase.InnerList.Item(index), ItemData)
            End Get
        End Property

    End Class

End Namespace