Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfparser
Imports FinSeA.org.apache.pdfbox.persistence.util

Namespace org.apache.pdfbox.pdmodel


    Public Class ConformingPDDocument
        Inherits PDDocument

        ''' <summary>
        ''' Maps ObjectKeys to a COSObject. Note that references to these objects are also stored in COSDictionary objects that map a name to a specific object.
        ''' </summary>
        ''' <remarks></remarks>
        Private objectPool As Map(Of COSObjectKey, COSBase) = New HashMap(Of COSObjectKey, COSBase)
        Private parser As ConformingPDFParser = Nothing

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal doc As COSDocument) ' throws IOException {
            MyBase.New(doc)
        End Sub

        '/**
        ' * This will load a document from an input stream.
        ' * @param input The File which contains the document.
        ' * @return The document that was loaded.
        ' * @throws IOException If there is an error reading from the stream.
        ' */
        Public Overloads Shared Function load(ByVal input As FinSeA.Io.File) As PDDocument 'throws IOException {
            Dim parser As ConformingPDFParser = New ConformingPDFParser(input)
            parser.parse()
            Dim ret As PDDocument = parser.getPDDocument()
            Return ret
        End Function

        '/**
        ' * This will get an object from the pool.
        ' * @param key The object key.
        ' * @return The object in the pool or a new one if it has not been parsed yet.
        ' * @throws IOException If there is an error getting the proxy object.
        ' */
        Public Function getObjectFromPool(ByVal key As COSObjectKey) As COSBase 'throws IOException {
            Return objectPool.get(key)
        End Function

        '/**
        ' * This will get an object from the pool.
        ' * @param key The object key.
        ' * @return The object in the pool or a new one if it has not been parsed yet.
        ' * @throws IOException If there is an error getting the proxy object.
        ' */
        Public Function getObjectKeysFromPool() As List(Of COSObjectKey)  'throws IOException {
            Dim keys As List(Of COSObjectKey) = New ArrayList(Of COSObjectKey)()
            For Each key As COSObjectKey In objectPool.keySet()
                keys.add(key)
            Next
            Return keys
        End Function

        '/**
        ' * This will get an object from the pool.
        ' * @param number the object number
        ' * @param generation the generation of this object you wish to load
        ' * @return The object in the pool
        ' * @throws IOException If there is an error getting the proxy object.
        ' */
        Public Function getObjectFromPool(ByVal number As Long, ByVal generation As Long) As COSBase  'throws IOException {
            Return objectPool.get(New COSObjectKey(number, generation))
        End Function

        Public Sub putObjectInPool(ByVal [object] As COSBase, ByVal number As Long, ByVal generation As Long)
            objectPool.put(New COSObjectKey(number, generation), [object])
        End Sub

        Public Function getParser() As ConformingPDFParser
            Return parser
        End Function

        Public Sub setParser(ByVal parser As ConformingPDFParser)
            Me.parser = parser
        End Sub

    End Class

End Namespace
