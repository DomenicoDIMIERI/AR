Imports System.IO

Namespace PDF


    Public Class cImage

        Public fileName As String = ""
        Private m_image As System.Drawing.Image
        Public width As Integer = -1
        Public height As Integer = -1
        Public mime As String = ""
        Public channels As Integer
        Public bits As Integer
        Private m_size As Integer = -1
        Public extension As String
        'Public id As Integer

        Public Sub New()
        End Sub

        Public Sub New(ByVal fileName As String)
            Me.New()
            Me.Open(fileName)
        End Sub

        Public Sub New(ByVal image As System.Drawing.Bitmap)
            Me.New()
            Me.Image = image
        End Sub

        Public Sub Open(ByVal pFileName As String)
            Me.m_image = New System.Drawing.Bitmap(pFileName)
            Me.fileName = pFileName
            Me.m_size = Sistema.FileSystem.GetFileSize(pFileName)
            Me.extension = Sistema.FileSystem.GetExtensionName(pFileName).ToLower
            Me.mime = Me.GetMimeType(Me.extension)
            Me.width = Me.Image.Width
            Me.height = Me.Image.Height
            Me.mime = Me.GetMimeType(Me.extension)
            Me.channels = 3
            Me.bits = 8
            ' Me.m_Buffer = New System.IO.FileStream(pFileName, IO.FileMode.Open, IO.FileAccess.Read)
        End Sub

        Public Property Image As System.Drawing.Image
            Get
                Return Me.m_image
            End Get
            Set(value As System.Drawing.Image)
                Me.m_image = New System.Drawing.Bitmap(value.Width, value.Height, Drawing.Imaging.PixelFormat.Format24bppRgb)
                Dim g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(Me.m_image)
                g.DrawImage(value, 0, 0)
                g.Dispose()
                'Me.m_Buffer = New System.IO.MemoryStream()
                'value.Save(Me.m_Buffer, System.Drawing.Imaging.ImageFormat.Jpeg)
                Me.m_size = -1
                Me.extension = "jpg"
                Me.mime = Me.GetMimeType(Me.extension)
                Me.width = Me.m_image.Width
                Me.height = Me.m_image.Height
                Me.channels = 3
                Me.bits = 8
            End Set
        End Property


        Private Function GetMimeType(ByVal ext As String) As String
            Select Case (ext)
                Case "jpg", "jpeg" : Return "image/jpeg"
                Case "png" : Return "image/png"
                Case Else
                    Throw New Exception("Tipo MIME non riconosciuto: " & ext)
            End Select
        End Function

        'Private Function toAscii(ByVal code As Integer) As Integer
        '    '//debug(code)
        '    Select Case (code)
        '        Case 8364 : code = 128
        '        Case 8218 : code = 130
        '        Case 402 : code = 131
        '        Case 8222 : code = 132
        '        Case 8230 : code = 133
        '        Case 8224 : code = 134
        '        Case 8225 : code = 135
        '        Case 710 : code = 136
        '        Case 8240 : code = 137
        '        Case 352 : code = 138
        '        Case 8249 : code = 139
        '        Case 338 : code = 140
        '        Case 381 : code = 142
        '        Case 8216 : code = 145
        '        Case 8217 : code = 146
        '        Case 8220 : code = 147
        '        Case 8221 : code = 148
        '        Case 8226 : code = 149
        '        Case 8211 : code = 150
        '        Case 8212 : code = 151
        '        Case 732 : code = 152
        '        Case 8482 : code = 153
        '        Case 353 : code = 154
        '        Case 8250 : code = 155
        '        Case 339 : code = 156
        '        Case 382 : code = 158
        '        Case 376 : code = 159
        '        Case Else
        '            Throw New Exception("Error ascii code : " & code)
        '    End Select
        '    Return code
        'End Function

        'Private Function Read(ByVal nB As Integer, Optional ByVal radix As Integer = 16) As Integer
        '    Dim res As String = ""
        '    'else if (radix=="string"){return Me.Buffer.ReadText(nB);}
        '    For i As Integer = 1 To nB
        '        Dim ch(0) As Byte
        '        Me.m_ = Me.Buffer.ReadText(1).charCodeAt(0)
        '    			if (ch>255)ch=toAscii(ch)
        '            ch = ch.toString(16)
        '    			if (ch.length==1)ch = "0" + ch
        '                res += ch
        '    		}
        '    	if (radix!=16){
        '    		res = res.toString(radix);
        '    		if (radix==10){res = parseInt(res,16)}
        '    		else if (radix==2){
        '    			if (res.length!=nB*8){
        '    				s = "";for (i=0;i<nB*8-res.length;i++){s+="0"}
        '    				res = s + res;
        '    			}
        '    		}
        '    	}
        '    	return res;
        '    }

        'Private Sub _parseJpeg()
        '    Const TEM = &H1 : Const SOF = &HC0 : Const DHT = &HC4 : Const JPGA = &HC8
        '    Const DAC = &HCC : Const RST = &HD0 : Const SOI = &HD8 : Const EOI = &HD9
        '    Const SOS = &HDA : Const DQT = &HDB : Const DNL = &HDC : Const DRI = &HDD
        '    Const DHP = &HDE : Const EXP = &HDF : Const APP = &HE0 : Const JPG = &HF0
        '    Const COM = &HFE
        '    Dim marker As Integer
        '    Dim length As Integer
        '    'Me.id = 2
        '    	if (Me.Read(1,10)==0xff){
        '    		while(!Me.Buffer.EOS){
        '            marker = Me.Read(1, 10)
        '    		while (marker==0xff){marker = Me.Read(1,10)}
        '    		switch(marker){
        '    			case DHP:case SOF+0:case SOF+1:case SOF+2:case SOF+3:case SOF+5:
        '    			case SOF+6:case SOF+7:case SOF+9:case SOF+10:case SOF+11:case SOF+13:case SOF+14:case SOF+15:
        '                length = Me.Read(2, 10)
        '                Me.bits = Me.Read(1, 10)
        '                Me.height = Me.Read(2, 10)
        '                Me.width = Me.Read(2, 10)
        '                Me.channels = Me.Read(1, 10)
        '    					return;
        '    			case APP+0:	case APP+1:	case APP+2:	case APP+3:case APP+4:	case APP+5:	case APP+6:	case APP+7:
        '    			case APP+8:	case APP+9:	case APP+10:case APP+11:case APP+12:case APP+13:case APP+14:case APP+15:
        '    			case DRI:case SOS:	case DHT:case DAC:case DNL:case EXP:
        '                h = Me.Read(2, 10) - 2
        '    				Me.Buffer.Position +=  h;
        '    				break;
        '    			}
        '    		}
        '    	}
        'End Sub

        'Me._parsePng = function _parsePng(){

        '}

        Public Function GetBuffer() As String
            Dim stream As System.IO.Stream = Nothing
            If (Me.fileName <> "") Then
                stream = New System.IO.FileStream(Me.fileName, FileMode.Open, FileAccess.Read)
            ElseIf (Me.m_image IsNot Nothing) Then
                stream = New System.IO.MemoryStream()
                Me.m_image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Dim reader As New System.IO.StreamReader(stream)
            Dim ret As String
            stream.Position = 0
            ret = reader.ReadToEnd
            reader.Dispose()
            stream.Dispose()
            Return ret
        End Function

        Public ReadOnly Property Size As Long
            Get
                If (Me.m_size < 0) Then
                    Dim stream As System.IO.Stream = Nothing
                    stream = New System.IO.MemoryStream()
                    Me.m_image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg)
                    Me.m_size = stream.Length
                    stream.Dispose()
                End If
                Return Me.m_size
            End Get
        End Property

    End Class


End Namespace