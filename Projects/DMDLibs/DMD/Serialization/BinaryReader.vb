Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net

Namespace Serializable

    Public Class BinaryReader
        Inherits System.IO.BinaryReader

        Private m_Settings As CKeyCollection

        Public Sub New(ByVal stream As System.IO.Stream)
            MyBase.New(stream)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Settings = Nothing
        End Sub

        Public Sub New(ByVal stream As System.IO.Stream, ByVal encoding As System.Text.Encoding)
            MyBase.New(stream, encoding)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Settings = Nothing
        End Sub

        Public ReadOnly Property Settings As CKeyCollection
            Get
                If (Me.m_Settings Is Nothing) Then Me.m_Settings = New CKeyCollection
                Return Me.m_Settings
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Namespace
