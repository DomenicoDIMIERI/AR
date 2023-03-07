Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    ''' <summary>
    ''' Descrizione un evento generato da un cursore
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DBCursorEventArgs
        Inherits System.EventArgs

        Private m_Cursor As DBObjectCursorBase

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal cursor As DBObjectCursorBase)
            Me.New
            If (cursor Is Nothing) Then Throw New ArgumentNullException("cursor")
            Me.m_Cursor = cursor
        End Sub

        Public ReadOnly Property Cursor As DBObjectCursorBase
            Get
                Return Me.m_Cursor
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class


