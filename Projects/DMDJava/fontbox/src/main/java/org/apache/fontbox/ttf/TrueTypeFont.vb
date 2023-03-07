Imports System.IO
Imports FinSeA.Io

Namespace org.apache.fontbox.ttf

    '/**
    ' * A class to hold true type font information.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class TrueTypeFont

        Private version As Single

        Private tables As Map(Of String, TTFTable) = New HashMap(Of String, TTFTable)()

        Private data As TTFDataStream

        '/**
        ' * Constructor.  Clients should use the TTFParser to create a new TrueTypeFont object.
        ' * 
        ' * @param fontData The font data.
        ' */
        Public Sub New(ByVal fontData As TTFDataStream)
            data = fontData
        End Sub

        '/**
        ' * Close the underlying resources.
        ' * 
        ' * @throws IOException If there is an error closing the resources.
        ' */
        Public Sub close()
            data.close()
        End Sub

        '/**
        ' * @return Returns the version.
        ' */
        Public Function getVersion() As Single
            Return version
        End Function

        '/**
        ' * @param versionValue The version to set.
        ' */
        Public Sub setVersion(ByVal versionValue As Single)
            version = versionValue
        End Sub

        '/**
        ' * Add a table definition.
        ' * 
        ' * @param table The table to add.
        ' */
        Public Sub addTable(ByVal table As TTFTable)
            tables.put(table.getTag(), table)
        End Sub

        '/**
        ' * Get all of the tables.
        ' * 
        ' * @return All of the tables.
        ' */
        Public Function getTables() As ICollection(Of TTFTable)
            Return tables.Values()
        End Function

        '/**
        ' * This will get the naming table for the true type font.
        ' * 
        ' * @return The naming table.
        ' */
        Public Function getNaming() As NamingTable
            Return tables.get(NamingTable.TAG)
        End Function

        '/**
        ' * Get the postscript table for Me TTF.
        ' * 
        ' * @return The postscript table.
        ' */
        Public Function getPostScript() As PostScriptTable
            Return tables.get(PostScriptTable.TAG)
        End Function

        '/**
        ' * Get the OS/2 table for Me TTF.
        ' * 
        ' * @return The OS/2 table.
        ' */
        Public Function getOS2Windows() As OS2WindowsMetricsTable
            Return tables.get(OS2WindowsMetricsTable.TAG)
        End Function

        '/**
        ' * Get the maxp table for Me TTF.
        ' * 
        ' * @return The maxp table.
        ' */
        Public Function getMaximumProfile() As MaximumProfileTable
            Return tables.get(MaximumProfileTable.TAG)
        End Function

        '/**
        ' * Get the head table for Me TTF.
        ' * 
        ' * @return The head table.
        ' */
        Public Function getHeader() As HeaderTable
            Return tables.get(HeaderTable.TAG)
        End Function

        '/**
        ' * Get the hhea table for Me TTF.
        ' * 
        ' * @return The hhea table.
        ' */
        Public Function getHorizontalHeader() As HorizontalHeaderTable
            Return tables.get(HorizontalHeaderTable.TAG)
        End Function

        '/**
        ' * Get the hmtx table for Me TTF.
        ' * 
        ' * @return The hmtx table.
        ' */
        Public Function getHorizontalMetrics() As HorizontalMetricsTable
            Return tables.get(HorizontalMetricsTable.TAG)
        End Function

        '/**
        ' * Get the loca table for Me TTF.
        ' * 
        ' * @return The loca table.
        ' */
        Public Function getIndexToLocation() As IndexToLocationTable
            Return tables.get(IndexToLocationTable.TAG)
        End Function

        '/**
        ' * Get the glyf table for Me TTF.
        ' * 
        ' * @return The glyf table.
        ' */
        Public Function getGlyph() As GlyphTable
            Return tables.get(GlyphTable.TAG)
        End Function

        '/**
        ' * Get the cmap table for Me TTF.
        ' * 
        ' * @return The cmap table.
        ' */
        Public Function getCMAP() As CMAPTable
            Return tables.get(CMAPTable.TAG)
        End Function

        '/**
        ' * This permit to get the data of the True Type Font
        ' * program representing the stream used to build Me 
        ' * object (normally from the TTFParser object).
        ' * 
        ' * @return COSStream True type font program stream
        ' * 
        ' * @throws IOException If there is an error getting the font data.
        ' */
        Public Function getOriginalData() As InputStream
            Return data.getOriginalData()
        End Function

    End Class

End Namespace
