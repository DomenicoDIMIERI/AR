Public partial class Databases

    Public MustInherit Class DBObjectCursor
        Inherits DBObjectCursorBase

        Private m_CreatoDa As CCursorField(Of Integer)
        Private m_CreatoIl As CCursorField(Of Date)
        Private m_ModificatoDa As CCursorField(Of Integer)
        Private m_ModificatoIl As CCursorField(Of Date)
        Private m_Stato As CCursorField(Of ObjectStatus)

        Public Sub New()
            Me.m_CreatoDa = New CCursorField(Of Integer)("CreatoDa")
            Me.m_CreatoIl = New CCursorField(Of Date)("CreatoIl")
            Me.m_ModificatoDa = New CCursorField(Of Integer)("ModificatoDa")
            Me.m_ModificatoIl = New CCursorField(Of Date)("ModificatoIl")
            Me.m_Stato = New CCursorField(Of ObjectStatus)("Stato")
        End Sub

        Public Overrides Function GetWherePartLimit() As String
            Dim tmpSQL As String
            tmpSQL = ""
            If Not Me.Module.UserCanDoAction("list") Then
                tmpSQL = ""
                If Me.Module.UserCanDoAction("list_own") Then
                    tmpSQL = DMD.Sistema.Strings.Combine(tmpSQL, "([CreatoDa]=" & GetID(Sistema.Users.CurrentUser) & ")", " OR ")
                End If
                If tmpSQL = "" Then tmpSQL = "(0<>0)"
            End If
            Return tmpSQL
        End Function

        Public ReadOnly Property CreatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_CreatoDa
            End Get
        End Property

        Public ReadOnly Property CreatoIl As CCursorField(Of Date)
            Get
                Return Me.m_CreatoIl
            End Get
        End Property

        Public ReadOnly Property ModificatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_ModificatoDa
            End Get
        End Property

        Public ReadOnly Property ModificatoIl As CCursorField(Of Date)
            Get
                Return Me.m_ModificatoIl
            End Get
        End Property

        Public ReadOnly Property Stato As CCursorField(Of ObjectStatus)
            Get
                Return Me.m_Stato
            End Get
        End Property

    End Class


End Class
