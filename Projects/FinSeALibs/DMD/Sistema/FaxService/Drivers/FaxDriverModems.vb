Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Drivers
Imports DMD.Anagrafica

Partial Public Class Sistema
     
    ''' <summary>
    ''' Definisce una linea FAX utilizzabile da un driver
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class FaxDriverModems
        Inherits CCollection(Of FaxDriverModem)

        <NonSerialized> Private m_Owner As BaseFaxDriver

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal owner As BaseFaxDriver)
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.m_Owner = owner
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, FaxDriverModem).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, FaxDriverModem).SetOwner(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Overridable Sub SetOwner(ByVal value As BaseFaxDriver)
            Me.m_Owner = value
            For Each m As FaxDriverModem In Me
                m.SetOwner(value)
            Next
        End Sub


    End Class


End Class
