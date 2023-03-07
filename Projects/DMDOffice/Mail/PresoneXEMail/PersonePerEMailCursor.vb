Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Net

Partial Class Office

    Public Class PersonePerEMailCursor
        Inherits DBObjectCursorBase(Of PersonaPerEMail)

        Private m_IDApplication As Integer

        Private m_IDMessaggio As New CCursorField(Of Integer)("IDMessaggio")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_Indirizzo As New CCursorFieldObj(Of String)("Indirizzo")
        Private m_DataMessaggio As New CCursorField(Of Date)("DataMessaggio")

        Public Sub New()
        End Sub


        Public ReadOnly Property IDMessaggio As CCursorField(Of Integer)
            Get
                Return Me.m_IDMessaggio
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property Indirizzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        Public ReadOnly Property DataMessaggio As CCursorField(Of Date)
            Get
                Return Me.m_DataMessaggio
            End Get
        End Property


         
        Public Overrides Function GetTableName() As String
            Return "tbl_PersoneXEMail"
        End Function

        Protected Overrides Function GetModule() As Sistema.CModule
            Return Nothing
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            '  writer.WriteAttribute("AppID", GetID(Me.Application))
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "AppID" : Me.m_IDApplication = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            'If (Me.Application Is Nothing) Then Return Nothing
            'Return Me.Application.Connection
            Return Office.Mails.Database
        End Function

    End Class

End Class