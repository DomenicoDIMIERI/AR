Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals

    ''' <summary>
    ''' Gestione delle licenze software
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CLicenzeSoftwareClass
        Inherits CGeneralClass(Of LicenzaSoftware)

        Friend Sub New()
            MyBase.New("modOfficeLicenzeSoftware", GetType(LicenzeSoftwareCursor), -1)
        End Sub


        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeLicenzeSoftware")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeLicenzeSoftware")
                ret.Description = "Software"
                ret.DisplayName = "Software"
                ret.Parent = DMD.Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If

            Return ret
        End Function





        Public Function GetItemByCodice(ByVal codiceLicenza As String) As LicenzaSoftware
            codiceLicenza = Strings.Trim(codiceLicenza)
            If (codiceLicenza = vbNullString) Then Return Nothing
            Dim ret As LicenzaSoftware
            For Each ret In Me.LoadAll
                If (Strings.Compare(ret.CodiceLicenza, codiceLicenza) = 0) Then Return ret
            Next
            Return Nothing
        End Function

    End Class


End Namespace

Partial Class Office



    Private Shared m_LicenzeSoftware As CLicenzeSoftwareClass = Nothing

    Public Shared ReadOnly Property LicenzeSoftware As CLicenzeSoftwareClass
        Get
            If (m_LicenzeSoftware Is Nothing) Then m_LicenzeSoftware = New CLicenzeSoftwareClass
            Return m_LicenzeSoftware
        End Get
    End Property


End Class