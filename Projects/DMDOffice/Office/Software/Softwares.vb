Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals

    ''' <summary>
    ''' Gestione dei software installati su un dispositivo
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CSotfwaresClass
        Inherits CGeneralClass(Of Software)

        Friend Sub New()
            MyBase.New("modOfficeSoftware", GetType(SoftwareCursor), -1)
        End Sub


        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeSoftware")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeSoftware")
                ret.Description = "Software"
                ret.DisplayName = "Software"
                ret.Parent = DMD.Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If

            Return ret
        End Function





        Public Function GetItemByNameAndVersione(ByVal name As String, ByVal version As String) As Software
            name = Strings.Trim(name)
            version = Strings.Trim(version)
            If (name = vbNullString) Then Return Nothing
            Dim ret As Software
            For Each ret In Me.LoadAll
                If (Strings.Compare(ret.Nome, name) = 0) AndAlso (Strings.Compare(ret.Versione, version) = 0) Then Return ret
            Next
            Return Nothing
        End Function

    End Class


End Namespace

Partial Class Office



    Private Shared m_Software As CSotfwaresClass = Nothing

    Public Shared ReadOnly Property Softwares As CSotfwaresClass
        Get
            If (m_Software Is Nothing) Then m_Software = New CSotfwaresClass
            Return m_Software
        End Get
    End Property


End Class