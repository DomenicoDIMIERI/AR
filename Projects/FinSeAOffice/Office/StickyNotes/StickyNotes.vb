Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.Office

Namespace Internals



    Public NotInheritable Class CStickyNotesClass
        Inherits CGeneralClass(Of StickyNote)

        Friend Sub New()
            MyBase.New("modOfficeStickyNotes", GetType(StickyNotesCursor))
        End Sub


    End Class

End Namespace

Partial Class Office


    Private Shared m_StickyNotes As CStickyNotesClass = Nothing

    Public Shared ReadOnly Property StickyNotes As CStickyNotesClass
        Get
            If (m_StickyNotes Is Nothing) Then m_StickyNotes = New CStickyNotesClass
            Return m_StickyNotes
        End Get
    End Property

End Class