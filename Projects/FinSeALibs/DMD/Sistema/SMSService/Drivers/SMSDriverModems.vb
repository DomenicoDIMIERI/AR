Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Drivers
Imports DMD.Anagrafica

Partial Public Class Sistema
     
    ''' <summary>
    ''' Collezione dei modems definiti per un driver
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class SMSDriverModems
        Inherits CCollection(Of SMSDriverModem)

        <NonSerialized> Private m_Owner As BasicSMSDriver

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal owner As BasicSMSDriver)
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.m_Owner = owner
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, SMSDriverModem).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, SMSDriverModem).SetOwner(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Overridable Sub SetOwner(ByVal value As BasicSMSDriver)
            Me.m_Owner = value
            For Each m As SMSDriverModem In Me
                m.SetOwner(value)
            Next
        End Sub


    End Class


End Class
