Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CQSPD
Imports DMD.Internals


Namespace Internals


    Public NotInheritable Class CImportExportSourcesClass
        Inherits CGeneralClass(Of CImportExportSource)


        Friend Sub New()
            MyBase.New("modCQSPDImportExportSrc", GetType(CImportExportSourceCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As CImportExportSource
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            Dim items As CCollection(Of CImportExportSource) = Me.LoadAll
            For Each src As CImportExportSource In items
                If src.Name = name Then Return src
            Next
            Return Nothing
        End Function

         
 

    End Class


End Namespace

Partial Class CQSPD

    Private Shared m_ImportExportSources As CImportExportSourcesClass = Nothing

    Public Shared ReadOnly Property ImportExportSources As CImportExportSourcesClass
        Get
            If (m_ImportExportSources Is Nothing) Then m_ImportExportSources = New CImportExportSourcesClass
            Return m_ImportExportSources
        End Get
    End Property


End Class