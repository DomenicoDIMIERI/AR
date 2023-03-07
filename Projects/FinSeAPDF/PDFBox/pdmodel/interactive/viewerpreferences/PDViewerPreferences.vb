Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.viewerpreferences

    '/**
    ' * This is the document viewing preferences.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDViewerPreferences
        Implements COSObjectable

        '/**
        ' * From PDF Reference: "Neither document outline nor thumbnail images visible".
        ' * 
        ' * @deprecated use {@link NON_FULL_SCREEN_PAGE_MODE} instead
        ' */
        Public Const NON_FULL_SCREEN_PAGE_MODE_USE_NONE = "UseNone"
        '/**
        ' * From PDF Reference: "Document outline visible".
        ' * 
        ' * @deprecated use {@link NON_FULL_SCREEN_PAGE_MODE} instead
        ' */
        Public Const NON_FULL_SCREEN_PAGE_MODE_USE_OUTLINES = "UseOutlines"
        '/**
        ' * From PDF Reference: "Thumbnail images visible".
        ' * 
        ' * @deprecated use {@link NON_FULL_SCREEN_PAGE_MODE} instead
        ' */
        Public Const NON_FULL_SCREEN_PAGE_MODE_USE_THUMBS = "UseThumbs"
        '/**
        ' * From PDF Reference: "Optional content group panel visible".
        ' * 
        ' * @deprecated use {@link NON_FULL_SCREEN_PAGE_MODE} instead
        ' */
        Public Const NON_FULL_SCREEN_PAGE_MODE_USE_OPTIONAL_CONTENT = "UseOC"

        '/**
        ' * Enumeration containing all valid values for NonFullScreenPageMode.
        ' */
        Public Enum NON_FULL_SCREEN_PAGE_MODE
            '/**
            ' *  From PDF Reference: "Neither document outline nor thumbnail images visible".
            ' */
            UseNone
            '/**
            ' * From PDF Reference: "Document outline visible".
            ' */
            UseOutlines
            '/**
            ' * From PDF Reference: "Thumbnail images visible".
            ' */
            UseThumbs
            '/**
            ' * From PDF Reference: "Optional content group panel visible".
            ' */
            UseOC
        End Enum

        '/**
        ' * Reading direction.
        ' * 
        ' * @deprecated use {@link READING_DIRECTION} instead
        ' */
        Public Const READING_DIRECTION_L2R = "L2R"
        '/**
        ' * Reading direction.
        ' * 
        ' * @deprecated use {@link READING_DIRECTION} instead
        ' */
        Public Const READING_DIRECTION_R2L = "R2L"
        '/**
        ' * Enumeration containing all valid values for ReadingDirection.
        ' */
        Public Enum READING_DIRECTION
            '/**
            ' * left to right.
            ' */
            L2R
            '/**
            ' * right to left.
            ' */
            R2L
        End Enum

        '/**
        ' * Boundary constant.
        ' * 
        ' * @deprecated use {@link BOUNDARY} instead
        ' */
        <Obsolete("use {BOUNDARY} instead")> _
        Public Const BOUNDARY_MEDIA_BOX = "MediaBox"
        <Obsolete("use {BOUNDARY} instead")> _
        Public Const BOUNDARY_CROP_BOX = "CropBox"
        <Obsolete("use {BOUNDARY} instead")> _
        Public Const BOUNDARY_BLEED_BOX = "BleedBox"
        <Obsolete("use {BOUNDARY} instead")> _
        Public Const BOUNDARY_TRIM_BOX = "TrimBox"
        <Obsolete("use {BOUNDARY} instead")> _
        Public Const BOUNDARY_ART_BOX = "ArtBox"


        ''' <summary>
        '''  Enumeration containing all valid values for boundaries.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum BOUNDARY
            ''' <summary>
            ''' use media box as boundary.
            ''' </summary>
            ''' <remarks></remarks>
            MediaBox

            ''' <summary>
            ''' use crop box as boundary.
            ''' </summary>
            ''' <remarks></remarks>
            CropBox

            ''' <summary>
            ''' use bleed box as boundary.
            ''' </summary>
            ''' <remarks></remarks>
            BleedBox

            ''' <summary>
            ''' use trim box as boundary.
            ''' </summary>
            ''' <remarks></remarks>
            TrimBox

            ''' <summary>
            ''' use art box as boundary.
            ''' </summary>
            ''' <remarks></remarks>
            ArtBox
        End Enum

        ''' <summary>
        ''' Enumeration containing all valid values for duplex.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum DUPLEX
            ''' <summary>
            ''' simplex printing.
            ''' </summary>
            ''' <remarks></remarks>
            Simplex

            ''' <summary>
            ''' duplex printing, flip at short edge.
            ''' </summary>
            ''' <remarks></remarks>
            DuplexFlipShortEdge

            ''' <summary>
            ''' duplex printing, flip at long edge.
            ''' </summary>
            ''' <remarks></remarks>
            DuplexFlipLongEdge
        End Enum

        ''' <summary>
        ''' Enumeration containing all valid values for printscaling.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum PRINT_SCALING
            ''' <summary>
            ''' no scaling.
            ''' </summary>
            ''' <remarks></remarks>
            None

            ''' <summary>
            ''' use app default.
            ''' </summary>
            ''' <remarks></remarks>
            AppDefault
        End Enum

        Private prefs As COSDictionary

        '/**
        ' * Constructor that is used for a preexisting dictionary.
        ' *
        ' * @param dic The underlying dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            prefs = dic
        End Sub

        '/**
        ' * This will get the underlying dictionary that this object wraps.
        ' *
        ' * @return The underlying info dictionary.
        ' */
        Public Function getDictionary() As COSDictionary
            Return prefs
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return prefs
        End Function

        '/**
        ' * Get the toolbar preference.
        ' *
        ' * @return the toolbar preference.
        ' */
        Public Function hideToolbar() As Boolean
            Return prefs.getBoolean(COSName.HIDE_TOOLBAR, False)
        End Function

        '/**
        ' * Set the toolbar preference.
        ' *
        ' * @param value Set the toolbar preference.
        ' */
        Public Sub setHideToolbar(ByVal value As Boolean)
            prefs.setBoolean(COSName.HIDE_TOOLBAR, value)
        End Sub

        '/**
        ' * Get the menubar preference.
        ' *
        ' * @return the menubar preference.
        ' */
        Public Function hideMenubar() As Boolean
            Return prefs.getBoolean(COSName.HIDE_MENUBAR, False)
        End Function

        '/**
        ' * Set the menubar preference.
        ' *
        ' * @param value Set the menubar preference.
        ' */
        Public Sub setHideMenubar(ByVal value As Boolean)
            prefs.setBoolean(COSName.HIDE_MENUBAR, value)
        End Sub

        '/**
        ' * Get the window UI preference.
        ' *
        ' * @return the window UI preference.
        ' */
        Public Function hideWindowUI() As Boolean
            Return prefs.getBoolean(COSName.HIDE_WINDOWUI, False)
        End Function

        '/**
        ' * Set the window UI preference.
        ' *
        ' * @param value Set the window UI preference.
        ' */
        Public Sub setHideWindowUI(ByVal value As Boolean)
            prefs.setBoolean(COSName.HIDE_WINDOWUI, value)
        End Sub

        '/**
        ' * Get the fit window preference.
        ' *
        ' * @return the fit window preference.
        ' */
        Public Function fitWindow() As Boolean
            Return prefs.getBoolean(COSName.FIT_WINDOW, False)
        End Function

        '/**
        ' * Set the fit window preference.
        ' *
        ' * @param value Set the fit window preference.
        ' */
        Public Sub setFitWindow(ByVal value As Boolean)
            prefs.setBoolean(COSName.FIT_WINDOW, value)
        End Sub

        '/**
        ' * Get the center window preference.
        ' *
        ' * @return the center window preference.
        ' */
        Public Function centerWindow() As Boolean
            Return prefs.getBoolean(COSName.CENTER_WINDOW, False)
        End Function

        '/**
        ' * Set the center window preference.
        ' *
        ' * @param value Set the center window preference.
        ' */
        Public Sub setCenterWindow(ByVal value As Boolean)
            prefs.setBoolean(COSName.CENTER_WINDOW, value)
        End Sub

        '/**
        ' * Get the display doc title preference.
        ' *
        ' * @return the display doc title preference.
        ' */
        Public Function displayDocTitle() As Boolean
            Return prefs.getBoolean(COSName.DISPLAY_DOC_TITLE, False)
        End Function

        '/**
        ' * Set the display doc title preference.
        ' *
        ' * @param value Set the display doc title preference.
        ' */
        Public Sub setDisplayDocTitle(ByVal value As Boolean)
            prefs.setBoolean(COSName.DISPLAY_DOC_TITLE, value)
        End Sub

        '/**
        ' * Get the non full screen page mode preference.
        ' *
        ' * @return the non full screen page mode preference.
        ' */
        Public Function getNonFullScreenPageMode() As String
            Return prefs.getNameAsString(COSName.NON_FULL_SCREEN_PAGE_MODE, [Enum].GetName(GetType(NON_FULL_SCREEN_PAGE_MODE), NON_FULL_SCREEN_PAGE_MODE.UseNone))
        End Function

        '/**
        ' * Set the non full screen page mode preference.
        ' *
        ' * @param value Set the non full screen page mode preference.
        ' */
        Public Sub setNonFullScreenPageMode(ByVal value As NON_FULL_SCREEN_PAGE_MODE)
            prefs.setName(COSName.NON_FULL_SCREEN_PAGE_MODE, [Enum].GetName(GetType(NON_FULL_SCREEN_PAGE_MODE), value))
        End Sub

        '/**
        ' * Set the non full screen page mode preference.
        ' *
        ' * @param value Set the non full screen page mode preference.
        ' * 
        ' * @deprecated
        ' */
        <Obsolete> _
        Public Sub setNonFullScreenPageMode(ByVal value As String)
            prefs.setName(COSName.NON_FULL_SCREEN_PAGE_MODE, value)
        End Sub

        '/**
        ' * Get the reading direction preference.
        ' *
        ' * @return the reading direction preference.
        ' */
        Public Function getReadingDirection() As String
            Return prefs.getNameAsString(COSName.DIRECTION, [Enum].GetName(GetType(READING_DIRECTION), READING_DIRECTION.L2R))
        End Function

        '/**
        ' * Set the reading direction preference.
        ' *
        ' * @param value Set the reading direction preference.
        ' */
        Public Sub setReadingDirection(ByVal value As READING_DIRECTION)
            prefs.setName(COSName.DIRECTION, [Enum].GetName(GetType(READING_DIRECTION), value))
        End Sub

        '/**
        ' * Set the reading direction preference.
        ' *
        ' * @param value Set the reading direction preference.
        ' * 
        ' * @deprecated
        ' */
        <Obsolete> _
        Public Sub setReadingDirection(ByVal value As String)
            prefs.setName(COSName.DIRECTION, value)
        End Sub

        '/**
        ' * Get the ViewArea preference.  See BOUNDARY enumeration.
        ' *
        ' * @return the ViewArea preference.
        ' */
        Public Function getViewArea() As String
            Return prefs.getNameAsString(COSName.VIEW_AREA, [Enum].GetName(GetType(BOUNDARY), BOUNDARY.CropBox))
        End Function

        '/**
        ' * Set the ViewArea preference.  See BOUNDARY_XXX constants.
        ' *
        ' * @param value Set the ViewArea preference.
        ' * 
        ' * @deprecated
        ' */
        <Obsolete> _
        Public Sub setViewArea(ByVal value As String)
            prefs.setName(COSName.VIEW_AREA, value)
        End Sub

        '/**
        ' * Set the ViewArea preference.  See BOUNDARY enumeration.
        ' *
        ' * @param value Set the ViewArea preference.
        ' */
        Public Sub setViewArea(ByVal value As BOUNDARY)
            prefs.setName(COSName.VIEW_AREA, [Enum].GetName(GetType(BOUNDARY), value))
        End Sub

        '/**
        ' * Get the ViewClip preference.  See BOUNDARY enumeration.
        ' *
        ' * @return the ViewClip preference.
        ' */
        Public Function getViewClip() As String
            Return prefs.getNameAsString(COSName.VIEW_CLIP, [Enum].GetName(GetType(BOUNDARY), BOUNDARY.CropBox))
        End Function

        '/**
        ' * Set the ViewClip preference.  See BOUNDARY enumeration.
        ' *
        ' * @param value Set the ViewClip preference.
        ' */
        Public Sub setViewClip(ByVal value As BOUNDARY)
            prefs.setName(COSName.VIEW_CLIP, [Enum].GetName(GetType(BOUNDARY), value))
        End Sub

        '/**
        ' * Set the ViewClip preference.  See BOUNDARY_XXX constants.
        ' *
        ' * @param value Set the ViewClip preference.
        ' * 
        ' * @deprecated
        ' */
        <Obsolete> _
        Public Sub setViewClip(ByVal value As String)
            prefs.setName(COSName.VIEW_CLIP, value)
        End Sub

        '/**
        ' * Get the PrintArea preference.  See BOUNDARY enumeration.
        ' *
        ' * @return the PrintArea preference.
        ' */
        Public Function getPrintArea() As String
            Return prefs.getNameAsString(COSName.PRINT_AREA, [Enum].GetName(GetType(BOUNDARY), BOUNDARY.CropBox))
        End Function

        '/**
        ' * Set the PrintArea preference.  See BOUNDARY_XXX constants.
        ' *
        ' * @param value Set the PrintArea preference.
        ' * 
        ' * @deprecated
        ' */
        <Obsolete> _
        Public Sub setPrintArea(ByVal value As String)
            prefs.setName(COSName.PRINT_AREA, value)
        End Sub

        '/**
        ' * Set the PrintArea preference.  See BOUNDARY enumeration.
        ' *
        ' * @param value Set the PrintArea preference.
        ' */
        Public Sub setPrintArea(ByVal value As BOUNDARY)
            prefs.setName(COSName.PRINT_AREA, [Enum].GetName(GetType(BOUNDARY), value))
        End Sub

        '/**
        ' * Get the PrintClip preference.  See BOUNDARY enumeration.
        ' *
        ' * @return the PrintClip preference.
        ' */
        Public Function getPrintClip() As String
            Return prefs.getNameAsString(COSName.PRINT_CLIP, [Enum].GetName(GetType(BOUNDARY), BOUNDARY.CropBox))
        End Function

        '/**
        ' * Set the PrintClip preference.  See BOUNDARY_XXX constants.
        ' *
        ' * @param value Set the PrintClip preference.
        ' * 
        ' * @deprecated
        ' */
        <Obsolete> _
        Public Sub setPrintClip(ByVal value As String)
            prefs.setName(COSName.PRINT_CLIP, value)
        End Sub

        '/**
        ' * Set the PrintClip preference.  See BOUNDARY enumeration.
        ' *
        ' * @param value Set the PrintClip preference.
        ' */
        Public Sub setPrintClip(ByVal value As BOUNDARY)
            prefs.setName(COSName.PRINT_CLIP, [Enum].GetName(GetType(BOUNDARY), value))
        End Sub

        '/**
        ' * Get the Duplex preference.  See DUPLEX enumeration.
        ' *
        ' * @return the Duplex preference.
        ' */
        Public Function getDuplex() As String
            Return prefs.getNameAsString(COSName.DUPLEX)
        End Function

        '/**
        ' * Set the Duplex preference.  See DUPLEX enumeration.
        ' *
        ' * @param value Set the Duplex preference.
        ' */
        Public Sub setDuplex(ByVal value As DUPLEX)
            prefs.setName(COSName.DUPLEX, [Enum].GetName(GetType(DUPLEX), value))
        End Sub

        '/**
        ' * Get the PrintScaling preference.  See PRINT_SCALING enumeration.
        ' *
        ' * @return the PrintScaling preference.
        ' */
        Public Function getPrintScaling() As String
            Return prefs.getNameAsString(COSName.PRINT_SCALING, [Enum].GetName(GetType(PRINT_SCALING), PRINT_SCALING.AppDefault))
        End Function

        '/**
        ' * Set the PrintScaling preference.  See PRINT_SCALING enumeration.
        ' *
        ' * @param value Set the PrintScaling preference.
        ' */
        Public Sub setPrintScaling(ByVal value As PRINT_SCALING)
            prefs.setName(COSName.PRINT_SCALING, [Enum].GetName(GetType(PRINT_SCALING), value))
        End Sub

    End Class

End Namespace
