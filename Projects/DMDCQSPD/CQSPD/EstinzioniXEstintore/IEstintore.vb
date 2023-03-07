Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Interfaccia implementata dagli oggetti che possono estinguere un finanziamento in corso
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IEstintore

        ReadOnly Property Estinzioni As CEstinzioniXEstintoreCollection

        Property DataDecorrenza As Date?

        Property Cliente As CPersonaFisica

    End Interface

End Class
