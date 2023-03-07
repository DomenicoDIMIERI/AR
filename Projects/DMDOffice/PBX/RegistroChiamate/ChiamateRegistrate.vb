Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals

    ''' <summary>
    ''' Gestione dei veicoli
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ChiamateRegistrate
        Inherits CGeneralClass(Of ChiamataRegistrata)


        Friend Sub New()
            MyBase.New("modOfficeRegistroChiamate", GetType(ChiamataRegistrataCursor), 0)
        End Sub




    End Class

End Namespace

Partial Class Office



    Private Shared m_ChiamateRegistrate As ChiamateRegistrate = Nothing

    Public Shared ReadOnly Property ChiamateRegistrate As ChiamateRegistrate
        Get
            If (m_ChiamateRegistrate Is Nothing) Then m_ChiamateRegistrate = New ChiamateRegistrate
            Return m_ChiamateRegistrate
        End Get
    End Property

End Class