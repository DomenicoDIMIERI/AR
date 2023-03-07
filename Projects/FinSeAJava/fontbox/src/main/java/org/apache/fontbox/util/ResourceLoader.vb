Imports FinSeA.Io

Namespace org.apache.fontbox.util

    '/**
    ' * This class will handle loading resource files(AFM/CMAP).  This was originally
    ' * written for PDFBox but FontBox uses it as well.  For now each project will
    ' * have their own version.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.1 $
    ' */
    Public Class ResourceLoader


        ''' <summary>
        ''' private constructor for utility class.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub

        '/**
        ' * This will attempt to load the resource given the resource name.
        ' *
        ' * @param resourceName The resource to try and load.
        ' *
        ' * @return The resource as a stream or null if it could not be found.
        ' * 
        ' * @throws IOException If there is an error while attempting to load the resource.
        ' */
        Public Shared Function loadResource(ByVal resourceName As String) As InputStream
            Dim loader As Reflection.Assembly = GetType(ResourceLoader).Assembly

            Dim [is] As InputStream = Nothing

            If (loader IsNot Nothing) Then
                [is] = loader.GetManifestResourceStream(resourceName)
            End If

            'see sourceforge bug 863053, Me is a fix for a user that
            'needed to have PDFBox loaded by the bootstrap classloader
            If ([is] Is Nothing) Then
                loader = Reflection.Assembly.GetEntryAssembly '.getSystemClassLoader()
                If (loader IsNot Nothing) Then
                    [is] = loader.GetManifestResourceStream(resourceName)
                End If
            End If

            If ([is] Is Nothing) Then
                Dim f As New FinSeA.Io.File(resourceName)
                If (f.exists()) Then
                    [is] = New FileInputStream(f)
                End If
            End If

            Return [is]
        End Function

        '/**
        ' * This will attempt to load the resource given the resource name.
        ' *
        ' * @param resourceName The resource to try and load.
        ' *
        ' * @return The resource as a stream or null if it could not be found.
        ' * 
        ' * @throws IOException If there is an error loading the properties.
        ' */
        Public Shared Function loadProperties(ByVal resourceName As String) As Properties
            Dim properties As Properties = Nothing
            Dim [is] As InputStream = Nothing
            Try
                [is] = loadResource(resourceName)
                If ([is] IsNot Nothing) Then
                    properties = New Properties()
                    properties.load([is])
                End If
            Finally
                If ([is] IsNot Nothing) Then
                    [is].Close()
                End If
            End Try
            Return properties
        End Function

        '/**
        ' * This will attempt to load the resource given the resource name.
        ' *
        ' * @param resourceName The resource to try and load.
        ' * @param defaults A stream of default properties.
        ' *
        ' * @return The resource as a stream or null if it could not be found.
        ' * 
        ' * @throws IOException If there is an error loading the properties.
        ' */
        Public Shared Function loadProperties(ByVal resourceName As String, ByVal defaults As Properties) As Properties
            Dim [is] As InputStream = Nothing
            Try
                [is] = loadResource(resourceName)
                If ([is] IsNot Nothing) Then
                    defaults.load([is])
                End If
            Finally
                If ([is] IsNot Nothing) Then
                    [is].Close()
                End If
            End Try
            Return defaults
        End Function

    End Class

End Namespace