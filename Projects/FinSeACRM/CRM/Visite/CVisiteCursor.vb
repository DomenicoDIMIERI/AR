Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Cursore sulla tabella delle visite
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CVisiteCursor
        Inherits CCustomerCallsCursor

        'Private m_Indirizzo_Via As New CCursorFieldObj(Of String)("Indirizzo_Via")
        'Private m_Indirizzo_Civico As New CCursorFieldObj(Of String)("Indirizzo_Civico")
        'Private m_Indirizzo_CAP As New CCursorFieldObj(Of String)("Indirizzo_CAP")
        'Private m_Indirizzo_Citta As New CCursorFieldObj(Of String)("Indirizzo_Citta")
        'Private m_Indirizzo_Provincia As New CCursorFieldObj(Of String)("Indirizzo_Provincia")
        'Private m_Indirizzo_Nome As New CCursorFieldObj(Of String)("Indirizzo_Nome")

        Public Sub New()
            MyBase.ClassName.Value = "CVisita"
        End Sub

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CVisita
        End Function

        'Public ReadOnly Property Indirizzo_Via As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Indirizzo_Via
        '    End Get
        'End Property

        'Public ReadOnly Property Indirizzo_Civico As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Indirizzo_Civico
        '    End Get
        'End Property

        'Public ReadOnly Property Indirizzo_CAP As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Indirizzo_CAP
        '    End Get
        'End Property

        'Public ReadOnly Property Indirizzo_Citta As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Indirizzo_Citta
        '    End Get
        'End Property

        'Public ReadOnly Property Indirizzo_Provincia As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Indirizzo_Provincia
        '    End Get
        'End Property

        'Public ReadOnly Property Indirizzo_Nome As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Indirizzo_Nome
        '    End Get
        'End Property




    End Class


End Class

