Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office

Namespace Internals



    Public Class DriversRilevatoriPresenze
        Inherits CCollection(Of DriverRilevatorePresenze)

        Public Sub New()
        End Sub

        Public Function Find(ByVal rilevatore As RilevatorePresenze) As DriverRilevatorePresenze
            For Each driver As DriverRilevatorePresenze In Me
                If driver.Supports(rilevatore) Then Return driver
            Next
            Return Nothing
        End Function

    End Class


End Namespace