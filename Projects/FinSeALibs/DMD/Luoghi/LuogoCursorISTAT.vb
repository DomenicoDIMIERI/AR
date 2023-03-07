Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica


    Public MustInherit Class LuogoCursorISTAT(Of T As LuogoISTAT)
        Inherits LuogoCursor(Of T)

        Private m_CodiceISTAT As New CCursorFieldObj(Of String)("Codice_ISTAT")
        Private m_CodiceCatasto As New CCursorFieldObj(Of String)("Codice_Catasto")

        Public Sub New()
        End Sub

        Public ReadOnly Property CodiceISTAT As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceISTAT
            End Get
        End Property

        Public ReadOnly Property CodiceCatasto As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceCatasto
            End Get
        End Property

    End Class


End Class