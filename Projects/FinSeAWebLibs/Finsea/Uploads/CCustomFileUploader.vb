Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.Threading
Imports System.Web.UI.HtmlControls
Imports System.IO

Partial Class WebSite



    Public Class CCustomFileUploader
        Inherits CFileUploader

        Private Enum StatusFlag
            UPLOADER_CERCAFIRMA = 0 'Siamo ancora in attesa della firma
            UPLOADER_CERCAINFO = 1        'Siamo alla ricerca delle info sul file
            UPLOADER_CERCATIPO = 2        'Siamo alla ricerca del tipo di dati inviati
            UPLOADER_SCARICA = 3      'Stiamo scaricando i dati
        End Enum

        Private m_InSream As System.IO.Stream
        Private m_OutputStream As System.IO.Stream

        Private stato As StatusFlag
        Private strBuffer As String
        Private firma As String
        Private contentDisposition As String
        Private contentType As String
        Private m_FieldName As String

        Public Sub New()
            Me.m_InSream = Nothing
            Me.m_OutputStream = Nothing
            Me.stato = StatusFlag.UPLOADER_CERCAFIRMA
            Me.firma = vbNullString
            Me.contentDisposition = vbNullString
            Me.contentType = vbNullString
            Me.m_FieldName = vbNullString
        End Sub

        Public Overrides Sub Dispose()
            MyBase.Dispose()
            If (Me.m_InSream IsNot Nothing) Then Me.m_InSream = Nothing
            If (Me.m_OutputStream IsNot Nothing) Then Me.m_OutputStream.Dispose() : Me.m_OutputStream = Nothing
            Me.strBuffer = vbNullString
            Me.firma = vbNullString
            Me.contentDisposition = vbNullString
            Me.contentType = vbNullString
            Me.m_FieldName = vbNullString
        End Sub


        Public Property FieldName As String
            Get
                Return Me.m_FieldName
            End Get
            Set(value As String)
                Me.m_FieldName = value
            End Set
        End Property

        ''' <summary>
        ''' Avvia il caricamento
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetFields(ByVal destURL As String, ByVal destFile As String, ByVal async As Boolean)
            Me.DestinationUrl = destURL
            Me.TargetFileName = destFile
            Me.Async = async
        End Sub


        Protected Overrides Sub PrepareUpload()
            Me.m_InSream = WebSite.ASP_Request.InputStream
            Me.stato = StatusFlag.UPLOADER_CERCAFIRMA
            Me.strBuffer = ""
            Me.firma = ""
            Me.m_OutputStream = New System.IO.FileStream(Me.TargetFileName, FileMode.CreateNew)
        End Sub

        Protected Overrides Function DoReader(buffer() As Byte) As Integer
            'Carichiamo il buffer dal computer remoto
            Dim n As Integer = Me.TotalBytes - Me.UploadedBytes
            Dim tmp() As Byte
            If (n > Me.BufferSize) Then n = Me.BufferSize
            tmp = WebSite.ASP_Request.BinaryRead(Me.BufferSize)
            System.Array.Copy(tmp, buffer, n)
            Return n
        End Function

        Protected Overrides Sub DoConsumer(buffer() As Byte, nBytes As Integer)
            MyBase.DoConsumer(buffer, nBytes)

            Me.strBuffer &= System.Text.Encoding.Default.GetString(buffer, 0, nBytes)

            Dim p, j As Integer
            Dim items() As String

            Select Case stato
                Case StatusFlag.UPLOADER_CERCAFIRMA
                    'Cerchiamo l'inizio del file da uploadare
                    p = InStr(Me.strBuffer, vbCrLf)
                    If (p > 0) Then
                        firma = Left(Me.strBuffer, p - 1)
                        Me.strBuffer = Mid(Me.strBuffer, p + 2)
                        stato = StatusFlag.UPLOADER_CERCAINFO
                    End If
                Case StatusFlag.UPLOADER_CERCAINFO
                    'Cerchiamo l'inizio del file da uploadare
                    p = InStr(Me.strBuffer, vbCrLf)
                    If (p > 0) Then
                        items = Split(Left(Me.strBuffer, p - 1), ";")

                        For i = LBound(items) To UBound(items)
                            j = InStr(items(i), "=")
                            If (j < 1) Then j = InStr(items(i), ":")


                            If (j >= 1) Then
                                Select Case LCase(Trim(Left(items(i), j - 1)))
                                    Case "content-disposition" : contentDisposition = Mid(items(i), j + 1)
                                    Case "name" : Me.m_FieldName = Mid(items(i), j + 1)
                                    Case "filename"
                                        FileName = Mid(items(i), j + 1)
                                        If Left(FileName, 1) = Chr(34) Then
                                            FileName = Mid(FileName, 2)
                                            j = InStr(FileName, Chr(34))
                                            contentType = Mid(FileName, j + 1)
                                            FileName = Left(FileName, j - 1)
                                            j = InStr(contentType, ":")
                                            contentType = Trim(Mid(contentType, j + 1))
                                        End If
                                    Case Else
                                End Select
                            End If
                        Next


                        stato = StatusFlag.UPLOADER_CERCATIPO
                        Me.strBuffer = Mid(Me.strBuffer, p + 2)

                    End If
                Case StatusFlag.UPLOADER_CERCATIPO
                    'Cerchiamo l'inizio del file da uploadare
                    p = InStr(Me.strBuffer, vbCrLf)
                    If (p > 0) Then
                        items = Split(Left(Me.strBuffer, p - 1), ";")

                        For i = LBound(items) To UBound(items)
                            j = InStr(items(i), "=")
                            If (j < 1) Then j = InStr(items(i), ":")


                            If (j >= 1) Then
                                Select Case LCase(Trim(Left(items(i), j - 1)))
                                    Case "content-type" : contentType = Mid(items(i), j + 1)
                                    Case Else
                                End Select
                            End If
                        Next

                        stato = StatusFlag.UPLOADER_SCARICA
                        Me.strBuffer = Mid(Me.strBuffer, p + 4)

                        If (Len(Me.strBuffer) > 0) Then
                            buffer = System.Text.Encoding.Default.GetBytes(Me.strBuffer)
                            Me.m_OutputStream.Write(buffer, 0, nBytes)
                        End If

                    End If
                Case StatusFlag.UPLOADER_SCARICA
                    p = InStr(Me.strBuffer, firma)
                    If (p > 0) Then
                        'fStream.WriteText(Left(bigBuffer, p - 1))
                        'fStream.SaveToFile(lastUploadedFilePath, 2)
                        'fStream.Close()
                        'fStream = Nothing

                        'bigBuffer = Mid(bigBuffer, p + Len(firma) + 2)
                        'stato = UPLOADER_CERCAINFO
                        'FileName = ""
                        'FieldName = ""
                        'contentType = ""
                        'm_CurrentUpload.Percentage = 100
                        Debug.Print(firma)
                    Else
                        Me.m_OutputStream.Write(buffer, 0, nBytes)
                    End If

                Case Else
                    Throw New InvalidOperationException("Stato non valido")
            End Select
        End Sub

        Protected Overrides Sub FinalizeUpload()
            If (Me.m_OutputStream IsNot Nothing) Then Me.m_OutputStream.Dispose()
            'If (Me.m_InputStream IsNot Nothing) Then Me.m_InputStream.Dispose()
            Me.m_OutputStream = Nothing
            Me.m_InSream = Nothing
        End Sub

        Protected Overrides Function GetTotalBytesToReceive() As Long
            Return WebSite.ASP_Request.TotalBytes
        End Function

       
    End Class


End Class