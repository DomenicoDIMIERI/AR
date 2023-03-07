Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS
Imports DMD.Internals

Partial Class Nokia

    ''' <summary>
    ''' Rappresenta un indirizzo di tipo postale
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CPostalAddress
        Inherits CBaseItem

        Private m_Tipo As String
        Private pstrCity As String
        Private pstrCountry As String
        Private pstrExtendedData As String
        Private pstrPOBox As String
        Private pstrPostalCode As String
        Private pstrState As String
        Private pstrStreet As String
        Private m_Contact As CContactItem

        Public Sub New()
            Me.m_Tipo = ""
            Me.pstrCity = ""
            Me.pstrCountry = ""
            Me.pstrExtendedData = ""
            Me.pstrPOBox = ""
            Me.pstrPostalCode = ""
            Me.pstrState = ""
            Me.pstrStreet = ""
            Me.m_Contact = Nothing
        End Sub

        Public Sub New(ByVal contact As CContactItem)
            Me.New()
            Me.SetDevice(contact.Device)
            Me.m_Contact = contact
        End Sub

        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                Me.m_Tipo = value
            End Set
        End Property

        Public ReadOnly Property Owner As CContactItem
            Get
                Return Me.m_Contact
            End Get
        End Property


        Public Property POBox As String
            Get
                Return Me.pstrPOBox
            End Get
            Set(value As String)
                Me.pstrPOBox = value
            End Set
        End Property


        Public Property Street As String
            Get
                Return Me.pstrStreet
            End Get
            Set(value As String)
                Me.pstrStreet = value
            End Set
        End Property


        Public Property PostalCode As String
            Get
                Return Me.pstrPostalCode
            End Get
            Set(value As String)
                Me.pstrPostalCode = value
            End Set
        End Property

        Public Property City As String
            Get
                Return Me.pstrCity
            End Get
            Set(value As String)
                Me.pstrCity = value
            End Set
        End Property

        Public Property State As String
            Get
                Return Me.pstrState
            End Get
            Set(value As String)
                Me.pstrState = value
            End Set
        End Property

        Public Property Country As String
            Get
                Return Me.pstrCountry
            End Get
            Set(value As String)
                Me.pstrCountry = value
            End Set
        End Property

        Public Property ExtendedData As String
            Get
                Return Me.pstrExtendedData
            End Get
            Set(value As String)
                Me.pstrExtendedData = value
            End Set
        End Property

        Friend Sub FromInfo(ByVal info As CA_DATA_POSTAL_ADDRESS)
            Me.pstrCity = info.pstrCity
            Me.pstrCountry = info.pstrCountry
            Me.pstrExtendedData = info.pstrExtendedData
            Me.pstrPOBox = info.pstrPOBox
            Me.pstrPostalCode = info.pstrPostalCode
            Me.pstrState = info.pstrState
            Me.pstrStreet = info.pstrStreet
        End Sub

        Protected Overrides Sub InternalDelete()
            Throw New NotImplementedException
        End Sub
    End Class

End Class