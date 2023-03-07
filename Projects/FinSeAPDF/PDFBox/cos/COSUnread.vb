Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.pdfparser

Namespace org.apache.pdfbox.cos


    Public Class COSUnread
        Inherits COSBase

        Private objectNumber As Integer
        Private generation As Integer
        Private parser As ConformingPDFParser

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal objectNumber As Integer, ByVal generation As Integer)
            MyBase.New()
            Me.objectNumber = objectNumber
            Me.generation = generation
        End Sub

        Public Sub New(ByVal objectNumber As Integer, ByVal generation As Integer, ByVal parser As ConformingPDFParser)
            Me.New(objectNumber, generation)
            Me.parser = parser
        End Sub

        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object ' throws COSVisitorException {
            ' TODO: read the object using the parser (if available) and visit that object
            Throw New NotImplementedException("COSUnread can not be written/visited.")
        End Function

        Public Overrides Function toString() As String
            Return "COSUnread{" & Me.objectNumber & "," & Me.generation & "}"
        End Function

        '/**
        ' * @return the objectNumber
        ' */
        Public Function getObjectNumber() As Integer
            Return Me.objectNumber
        End Function

        '/**
        ' * @param objectNumber the objectNumber to set
        ' */
        Public Sub setObjectNumber(ByVal objectNumber As Integer)
            Me.objectNumber = objectNumber
        End Sub

        '/**
        ' * @return the generation
        ' */
        Public Function getGeneration() As Integer
            Return Me.generation
        End Function

        '/**
        ' * @param generation the generation to set
        ' */
        Public Sub setGeneration(ByVal generation As Integer)
            Me.generation = generation
        End Sub

        '/**
        ' * @return the parser
        ' */
        Public Function getParser() As ConformingPDFParser
            Return Me.parser
        End Function

        '/**
        ' * @param parser the parser to set
        ' */
        Public Sub setParser(ByVal parser As ConformingPDFParser)
            Me.parser = parser
        End Sub

    End Class


End Namespace