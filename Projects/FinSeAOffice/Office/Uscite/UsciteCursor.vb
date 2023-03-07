Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella delle commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UsciteCursor
        Inherits DBObjectCursorPO(Of Uscita)

        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_OraUscita As New CCursorField(Of Date)("OraUscita")
        Private m_OraRientro As New CCursorField(Of Date)("OraRientro")
        Private m_DistanzaPercorsa As New CCursorField(Of Double)("DistanzaPercorsa")
        Private m_IDVeicoloUsato As New CCursorField(Of Integer)("IDVeicoloUsato")
        Private m_NomeVeicoloUsato As New CCursorFieldObj(Of String)("NomeVeicoloUsato")
        Private m_LitriCarburante As New CCursorField(Of Single)("LitriCarburante")
        Private m_Rimborso As New CCursorField(Of Decimal)("Rimborso")
        Private m_Indirizzo_Via As New CCursorFieldObj(Of String)("Indirizzo_Via")
        Private m_Indirizzo_Civico As New CCursorFieldObj(Of String)("Indirizzo_Civico")
        Private m_Indirizzo_Citta As New CCursorFieldObj(Of String)("Indirizzo_Citta")
        Private m_Indirizzo_Provincia As New CCursorFieldObj(Of String)("Indirizzo_Provincia")
        Private m_Indirizzo_CAP As New CCursorFieldObj(Of String)("Indirizzo_CAP")
        Private m_Lat As New CCursorField(Of Double)("Lat")
        Private m_Lng As New CCursorField(Of Double)("Lng")
        Private m_Alt As New CCursorField(Of Double)("Alt")

        Public ReadOnly Property Indirizzo_Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Via
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Civico As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Civico
            End Get
        End Property
        Public ReadOnly Property Indirizzo_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Citta
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Provincia
            End Get
        End Property

        Public ReadOnly Property Indirizzo_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_CAP
            End Get
        End Property

        Public ReadOnly Property Lat As CCursorField(Of Double)
            Get
                Return Me.m_Lat
            End Get
        End Property

        Public ReadOnly Property Lng As CCursorField(Of Double)
            Get
                Return Me.m_Lng
            End Get
        End Property

        Public ReadOnly Property Alt As CCursorField(Of Double)
            Get
                Return Me.m_Alt
            End Get
        End Property

        Public ReadOnly Property IDVeicoloUsato As CCursorField(Of Integer)
            Get
                Return Me.m_IDVeicoloUsato
            End Get
        End Property

        Public ReadOnly Property NomeVeicoloUsato As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeVeicoloUsato
            End Get
        End Property

        Public ReadOnly Property LitriCarburante As CCursorField(Of Single)
            Get
                Return Me.m_LitriCarburante
            End Get
        End Property

        Public ReadOnly Property Rimborso As CCursorField(Of Decimal)
            Get
                Return Me.m_Rimborso
            End Get
        End Property

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property


        Public ReadOnly Property OraUscita As CCursorField(Of Date)
            Get
                Return Me.m_OraUscita
            End Get
        End Property

        Public ReadOnly Property OraRientro As CCursorField(Of Date)
            Get
                Return Me.m_OraRientro
            End Get
        End Property


        Public ReadOnly Property DistanzaPercorsa As CCursorField(Of Double)
            Get
                Return Me.m_DistanzaPercorsa
            End Get
        End Property

        Public Overrides Function GetWherePartLimit() As String
            Dim ret As String = MyBase.GetWherePartLimit()
            If (ret <> vbNullString) AndAlso Me.Module.UserCanDoAction("list_assigned") Then
                Dim wherePart As String = "[IDOperatore] = " & GetID(Users.CurrentUser)
                ret = Strings.Combine(ret, wherePart, " OR ")
            End If
            Return ret
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Uscite.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeUscite"
        End Function

        Public Overrides Function Add() As Object
            Dim ret As Uscita = MyBase.Add()
            ret.Operatore = Users.CurrentUser
            'ret.OraUscita = Now
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class