Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination

Namespace org.apache.pdfbox.pdmodel.interactive.action.type

    '/**
    ' * This represents a go-to action that can be executed in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDActionGoTo
        Inherits PDAction

        ''' <summary>
        ''' This type of action this object represents.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "GoTo"

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            MyBase.New()
            setSubType(SUB_TYPE)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param a The action dictionary.
        ' */
        Public Sub New(ByVal a As COSDictionary)
            MyBase.New(a)
        End Sub

        '/**
        ' * This will get the destination to jump to.
        ' *
        ' * @return The D entry of the specific go-to action dictionary.
        ' *
        ' * @throws IOException If there is an error creating the destination.
        ' */
        Public Function getDestination() As PDDestination 'throws IOException
            Return PDDestination.create(getCOSDictionary().getDictionaryObject("D"))
        End Function

        '/**
        ' * This will set the destination to jump to.
        ' *
        ' * @param d The destination.
        ' */
        Public Sub setDestination(ByVal d As PDDestination)
            getCOSDictionary().setItem("D", d)
        End Sub

    End Class

End Namespace
