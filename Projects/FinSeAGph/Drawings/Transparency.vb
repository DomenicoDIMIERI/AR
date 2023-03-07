Namespace Drawings

   
    Public Interface Transparency

        Enum Mode
            ''' <summary>
            ''' Represents image data that is guaranteed to be either completely opaque, with an alpha value of 1.0, or completely transparent, with an alpha value of 0.0.
            ''' </summary>
            ''' <remarks></remarks>
            BITMASK

            ''' <summary>
            ''' Represents image data that is guaranteed to be completely opaque, meaning that all pixels have an alpha value of 1.0.
            ''' </summary>
            ''' <remarks></remarks>
            OPAQUE

            ''' <summary>
            ''' Represents image data that contains or might contain arbitrary alpha values between and including 0.0 and 1.0.
            ''' </summary>
            ''' <remarks></remarks>
            TRANSLUCENT
        End Enum

        ''' <summary>
        ''' Returns the type of this Transparency.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getTransparency() As Integer

    End Interface

End Namespace