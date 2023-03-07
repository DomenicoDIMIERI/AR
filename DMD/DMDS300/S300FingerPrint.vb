Imports DMD.S300.CKT_DLL
Imports DMD.Internals
Imports System.Runtime.InteropServices


Namespace S300

    ''' <summary>
    ''' Racchiude le informazioni su un'impronta digitale di un utente
    ''' </summary>
    Public Class S300FingerPrint

        Friend m_User As S300PersonInfo
        Friend m_FPID As Integer
        Friend m_Data() As Byte

        Public Sub New()
            Me.m_User = Nothing
            Me.m_FPID = 0
            Me.m_Data = {}
        End Sub

        ''' <summary>
        ''' Carica i dati dell'impronta dal file
        ''' </summary>
        ''' <param name="fileName"></param>
        Public Sub New(ByVal fileName As String)
            Me.New
            Me.LoadFromFile(fileName)
        End Sub

        ''' <summary>
        ''' Carica i dati dell'impronta dallo stream
        ''' </summary>
        ''' <param name="stream"></param>
        Public Sub New(ByVal stream As System.IO.Stream)
            Me.New
            Me.LoadFromStream(stream)
        End Sub

        ''' <summary>
        ''' Restituisce l'utente a cui appartiene questa impronta
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property User As S300PersonInfo
            Get
                Return Me.m_User
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'ID dell'impronta
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FPID As Integer
            Get
                Return Me.m_FPID
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un array contenente i dati dell'impronta
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDataAsArray() As Byte()
            Return Me.m_Data.Clone
        End Function

        ''' <summary>
        ''' Imposta i dati dell'imprtona 
        ''' </summary>
        ''' <param name="data"></param>
        Public Sub SetDataAsArray(ByVal data As Byte())
            If (data Is Nothing) Then data = {}
            Me.m_Data = data.Clone
        End Sub

        ''' <summary>
        ''' Salva i dati relativi all'impronta su un file
        ''' </summary>
        ''' <param name="fileName"></param>
        Public Sub SaveToFile(ByVal fileName As String)
            Dim fs As New System.IO.FileStream(fileName, IO.FileMode.Create)
            Me.SaveToStream(fs)
            fs.Dispose()
        End Sub

        ''' <summary>
        ''' Carica i dati relativi all'impronta da un file
        ''' </summary>
        ''' <param name="fileName"></param>
        Public Sub LoadFromFile(ByVal fileName As String)
            Dim fs As New System.IO.FileStream(fileName, IO.FileMode.Open)
            Me.LoadFromStream(fs)
            fs.Dispose()
        End Sub

        ''' <summary>
        ''' Salve i dati dell'impronta nello stream specificato
        ''' </summary>
        ''' <param name="stream"></param>
        Public Sub SaveToStream(ByVal stream As System.IO.Stream)
            stream.Write(Me.m_Data, 0, Me.m_Data.Length)
        End Sub

        ''' <summary>
        ''' Carica i dati dell'impronta dallo stream specificato
        ''' </summary>
        ''' <param name="stream"></param>
        Public Sub LoadFromStream(ByVal stream As System.IO.Stream)
            Dim len As Integer = stream.Length - stream.Position
            If (len = 0) Then
                Me.m_Data = {}
            Else
                ReDim Me.m_Data(len - 1)
                stream.Read(Me.m_Data, 0, len)
            End If
        End Sub




    End Class


End Namespace
