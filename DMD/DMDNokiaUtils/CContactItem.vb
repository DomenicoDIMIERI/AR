Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS
Imports DMD.Internals

Partial Class Nokia

    ''' <summary>
    ''' Rappresenta la collezione dei contatti memorizzati sul dispositivo Nokia
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CContactItem
        Inherits CBaseItem

        Private m_ContactName As String
        Private m_FirstName As String
        Private m_Company As String
        Private m_MiddleName As String
        Private m_Picture As System.Drawing.Image
        Private m_BirthDay As Date?
        Private m_LastName As String
        Private m_Suffix As String
        Private m_Title As String
        Private m_JobTitle As String
        Private m_NickName As String
        Private m_FormalName As String
        Private m_NamePronunciation As String
        Private m_FamilyNamePronunciation As String
        Private m_CompanyNamePronunciation As String
        Private m_CustomData As System.Collections.ArrayList
        Private m_Numbers As ItemDataCollection
        Private m_GeneralInformations As ItemDataCollection
        Private m_Addresses As System.Collections.ArrayList

        Public Sub New()
            Me.m_ContactName = ""
            Me.m_FirstName = ""
            Me.m_Company = ""
            Me.m_MiddleName = ""
            Me.m_BirthDay = Nothing
            Me.m_LastName = ""
            Me.m_Title = ""
            Me.m_Suffix = ""
            Me.m_JobTitle = ""
            Me.m_NickName = ""
            Me.m_FormalName = ""
            Me.m_NamePronunciation = ""
            Me.m_FamilyNamePronunciation = ""
            Me.m_CompanyNamePronunciation = ""
            Me.m_CustomData = New System.Collections.ArrayList
            Me.m_Numbers = New ItemDataCollection
            Me.m_GeneralInformations = New ItemDataCollection
            Me.m_Addresses = New System.Collections.ArrayList
        End Sub

        Friend Sub New(ByVal folder As CContactsFolder)
            Me.New()
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")
            Me.SetDevice(folder.Device)
            Me.SetParentFolder(folder)
        End Sub

        Public Property ContactName As String
            Get
                Return Me.m_ContactName
            End Get
            Set(value As String)
                Me.m_ContactName = value
            End Set
        End Property

        Public Property FirstName As String
            Get
                Return Me.m_FirstName
            End Get
            Set(value As String)
                Me.m_FirstName = value
            End Set
        End Property

        Public Property Company As String
            Get
                Return Me.m_Company
            End Get
            Set(value As String)
                Me.m_Company = value
            End Set
        End Property

        Public Property MiddleName As String
            Get
                Return Me.m_MiddleName
            End Get
            Set(value As String)
                Me.m_MiddleName = value
            End Set
        End Property

        Public Property LastName As String
            Get
                Return Me.m_LastName
            End Get
            Set(value As String)
                Me.m_LastName = value
            End Set
        End Property

        Public Property Suffix As String
            Get
                Return Me.m_Suffix
            End Get
            Set(value As String)
                Me.m_Suffix = value
            End Set
        End Property

        Public Property JobTitle As String
            Get
                Return Me.m_JobTitle
            End Get
            Set(value As String)
                Me.m_JobTitle = value
            End Set
        End Property

        Public Property NickName As String
            Get
                Return Me.m_NickName
            End Get
            Set(value As String)
                Me.m_NickName = value
            End Set
        End Property

        Public Property FormalName As String
            Get
                Return Me.m_FormalName
            End Get
            Set(value As String)
                Me.m_FormalName = value
            End Set
        End Property

        Public Property NamePronunciation As String
            Get
                Return Me.m_NamePronunciation
            End Get
            Set(value As String)
                Me.m_NamePronunciation = value
            End Set
        End Property

        Public Property FamilyNamePronunciation As String
            Get
                Return Me.m_FamilyNamePronunciation
            End Get
            Set(value As String)
                Me.m_FamilyNamePronunciation = value
            End Set
        End Property

        Public Property CompanyNamePronunciation As String
            Get
                Return Me.m_CompanyNamePronunciation
            End Get
            Set(value As String)
                Me.m_CompanyNamePronunciation = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'immagine associata al contatto sul dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Picture As System.Drawing.Image
            Get
                Return Me.m_Picture
            End Get
            Set(value As System.Drawing.Image)
                Me.m_Picture = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di nascita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BirthDate As Date?
            Get
                Return Me.m_BirthDay
            End Get
            Set(value As Date?)
                Me.m_BirthDay = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'elenco dei numeri
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Numbers As ItemDataCollection
            Get
                Return Me.m_Numbers
            End Get
        End Property

        Public Property Title As String
            Get
                Return Me.m_Title
            End Get
            Set(value As String)
                Me.m_Title = value
            End Set
        End Property

        Public ReadOnly Property GeneralInformations As ItemDataCollection
            Get
                Return Me.m_GeneralInformations
            End Get
        End Property

        'Friend Sub SetInfo(ByVal value As CA_DATA_CONTACT)
        '    Me.dataContact = value
        '    If dataContact.bPICount > 0 Then
        '        Dim itemData As CA_DATA_ITEM
        '        itemData = Marshal.PtrToStructure(dataContact.pPIFields, GetType(CA_DATA_ITEM))
        '        'itemZ.Text = Marshal.PtrToStringUni(itemData.pCustomData)
        '    End If

        'End Sub

        Public ReadOnly Property Addresses As System.Collections.ArrayList
            Get
                Return Me.m_Addresses
            End Get
        End Property


        '===================================================================
        ' GetContactDetails
        '
        ' Read selected contact from phone and show details in list view.
        '
        '===================================================================
        Private Sub GetContactDetails(ByVal dataContact As CA_DATA_CONTACT) 'ByVal UID As CA_ITEM_ID)
            ' Read contact item data from device
            ' Personal information
            Dim i As Integer
            For i = 0 To dataContact.bPICount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'i'
                Dim iPtr As Int64 = dataContact.pPIFields.ToInt64 + i * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Dim itemData As CA_DATA_ITEM
                itemData = CType(Marshal.PtrToStructure(ptr, GetType(CA_DATA_ITEM)), CA_DATA_ITEM)

                ' Add item to list view
                Select Case itemData.iFieldSubType
                    Case CA_FIELD_SUB_TYPE_NAME ' Name field			|   pstrText
                        Me.m_ContactName = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_FN ' First name			|	pstrText
                        Me.m_FirstName = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_MN ' Midle name			|	pstrText
                        Me.m_MiddleName = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_LN ' Last name			|	pstrText
                        Me.m_LastName = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_TITLE ' Title				|	pstrText
                        Me.m_Title = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_SUFFIX ' Suffix				|	pstrText
                        Me.m_Suffix = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_COMPANY ' Company				|	pstrText
                        Me.m_Company = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_JOB_TITLE ' Job title			|	pstrText
                        Me.m_JobTitle = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_BIRTHDAY ' Birthday				|	pCustomData as CA_DATA_DATE
                        Dim bDate As CA_DATA_DATE = New CA_DATA_DATE
                        bDate = CType(Marshal.PtrToStructure(itemData.pCustomData, GetType(CA_DATA_DATE)), CA_DATA_DATE)
                        Me.m_BirthDay = New Date(bDate.wYear, bDate.bMonth, bDate.bDay)
                    Case CA_FIELD_SUB_TYPE_PICTURE ' Picture				|	pCustomData as CA_DATA_PICTURE
                        Dim dataPicture As CA_DATA_PICTURE
                        dataPicture = CType(Marshal.PtrToStructure(itemData.pCustomData, GetType(CA_DATA_PICTURE)), CA_DATA_PICTURE)
                        Dim bytes(dataPicture.iDataLen - 1) As Byte
                        Marshal.Copy(dataPicture.pbData, bytes, 0, dataPicture.iDataLen)
                        ' Write image data to temporary file
                        Dim stream As New System.IO.MemoryStream
                        stream.Write(bytes, 0, bytes.Length)
                        stream.Position = 0
                        Me.m_Picture = New System.Drawing.Bitmap(stream)
                        'stream.Dispose()
                    Case CA_FIELD_SUB_TYPE_NICKNAME ' Nickname				|	pstrText
                        Me.m_NickName = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_FORMAL_NAME ' Formal name			|	pstrText
                        Me.m_FormalName = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_GIVEN_NAME_PRONUNCIATION 'Pronunciation field	|	pstrText
                        Me.m_NamePronunciation = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_FAMILY_NAME_PRONUNCIATION ' Pronunciation field	|	pstrText
                        Me.m_FamilyNamePronunciation = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case CA_FIELD_SUB_TYPE_COMPANY_NAME_PRONUNCIATION ' Pronunciation field	|	pstrText
                        Me.m_CompanyNamePronunciation = Marshal.PtrToStringUni(itemData.pCustomData)
                    Case Else
                        Me.m_CustomData.Add(Marshal.PtrToStringUni(itemData.pCustomData))
                End Select
            Next

            ' Numbers
            For i = 0 To dataContact.bNumberCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'i'
                Dim iPtr As Int64 = dataContact.pNumberFields.ToInt64 + i * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Dim itemData As CA_DATA_ITEM
                itemData = CType(Marshal.PtrToStructure(ptr, GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                ' Add item to list view
                Me.m_Numbers.Add(PIMFieldType2String(itemData), Marshal.PtrToStringUni(itemData.pCustomData))
            Next

            ' Addresses
            For i = 0 To dataContact.bAddressCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'i'
                Dim iPtr As Int64 = dataContact.pAddressFields.ToInt64 + i * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Dim itemData As CA_DATA_ITEM
                itemData = CType(Marshal.PtrToStructure(ptr, GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                ' Add item to list view

                If itemData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL Or itemData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL_BUSINESS Or itemData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL_PRIVATE Then
                    Dim address As New CPostalAddress(Me)
                    address.Tipo = PIMFieldType2String(itemData)
                    address.FromInfo(CType(Marshal.PtrToStructure(itemData.pCustomData, GetType(CA_DATA_POSTAL_ADDRESS)), CA_DATA_POSTAL_ADDRESS))
                    Me.m_Addresses.Add(address)
                Else
                    ' itemX.SubItems.Add(Marshal.PtrToStringUni(itemData.pCustomData))
                End If
            Next

            ' General information
            For i = 0 To dataContact.bGeneralCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'i'
                Dim iPtr As Int64 = dataContact.pGeneralFields.ToInt64 + i * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Dim itemData As CA_DATA_ITEM = CType(Marshal.PtrToStructure(ptr, GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                ' Add item to list view
                Me.m_GeneralInformations.Add(PIMFieldType2String(itemData), Marshal.PtrToStringUni(itemData.pCustomData))
            Next
            ' FreeContactData()
        End Sub


        '===================================================================
        ' PIMFieldType2String
        ' 
        ' Converts PIM field type values to string
        ' 
        '===================================================================
        Private Function PIMFieldType2String(ByVal pimData As CA_DATA_ITEM) As String
            If pimData.iFieldType = CA_FIELD_TYPE_CONTACT_PI Then
                PIMFieldType2String = "PI" 'Personal information
            ElseIf pimData.iFieldType = CA_FIELD_TYPE_CONTACT_NUMBER Then
                PIMFieldType2String = "Number"
            ElseIf pimData.iFieldType = CA_FIELD_TYPE_CONTACT_ADDRESS Then
                PIMFieldType2String = "Address"
            ElseIf pimData.iFieldType = CA_FIELD_TYPE_CONTACT_GENERAL Then
                PIMFieldType2String = "" ' General information
            ElseIf pimData.iFieldType = CA_FIELD_TYPE_CALENDAR Then
                PIMFieldType2String = ""  ' Calendar item
            Else
                PIMFieldType2String = "Unknown field type" ' shouldn't occur
            End If
            If PIMFieldType2String.Length > 0 Then
                PIMFieldType2String += ", "
            End If
            ' Personal information
            If pimData.iFieldSubType = CA_FIELD_SUB_TYPE_NAME Then
                PIMFieldType2String += "name"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_FN Then
                PIMFieldType2String += "first name"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_MN Then
                PIMFieldType2String += "middle name"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_LN Then
                PIMFieldType2String += "last name"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TITLE Then
                PIMFieldType2String += "title"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_SUFFIX Then
                PIMFieldType2String += "suffix"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_COMPANY Then
                PIMFieldType2String += "company"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_JOB_TITLE Then
                PIMFieldType2String += "job title"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_BIRTHDAY Then
                PIMFieldType2String += "birthday"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_PICTURE Then
                PIMFieldType2String += "picture"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_NICKNAME Then
                PIMFieldType2String += "nickname"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_FORMAL_NAME Then
                PIMFieldType2String += "formal name"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_GIVEN_NAME_PRONUNCIATION Then
                PIMFieldType2String += "given name pronunciation"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_FAMILY_NAME_PRONUNCIATION Then
                PIMFieldType2String += "family name pronunciation"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_COMPANY_NAME_PRONUNCIATION Then
                PIMFieldType2String += "company name pronunciation"
                ' Numbers
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TEL Then
                PIMFieldType2String += "telephone"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TEL_HOME Then
                PIMFieldType2String += "home"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TEL_WORK Then
                PIMFieldType2String += "work"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TEL_PREF Then
                PIMFieldType2String += "preferred"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TEL_CAR Then
                PIMFieldType2String += "car"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TEL_DATA Then
                PIMFieldType2String += "data"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_MOBILE Then
                PIMFieldType2String += "mobile"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_MOBILE_HOME Then
                PIMFieldType2String += "mobile (home)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_MOBILE_WORK Then
                PIMFieldType2String += "mobile (work)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_PAGER Then
                PIMFieldType2String += "pager"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_FAX Then
                PIMFieldType2String += "fax"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_FAX_HOME Then
                PIMFieldType2String += "fax (home)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_FAX_WORK Then
                PIMFieldType2String += "fax (work)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_VIDEO Then
                PIMFieldType2String += "video"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_VIDEO_HOME Then
                PIMFieldType2String += "video (home)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_VIDEO_WORK Then
                PIMFieldType2String += "work (work)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_VOIP Then
                PIMFieldType2String += "voip"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_VOIP_HOME Then
                PIMFieldType2String += "voip (home)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_VOIP_WORK Then
                PIMFieldType2String += "voip (work)"
                ' Addresses
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL Then
                PIMFieldType2String += "postal"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL_BUSINESS Then
                PIMFieldType2String += "business"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL_PRIVATE Then
                PIMFieldType2String += "private"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_EMAIL Then
                PIMFieldType2String += "email"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_EMAIL_HOME Then
                PIMFieldType2String += "email (home)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_EMAIL_WORK Then
                PIMFieldType2String += "email (work)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_WEB Then
                PIMFieldType2String += "web"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_WEB_HOME Then
                PIMFieldType2String += "web (home)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_WEB_WORK Then
                PIMFieldType2String += "web (work)"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_PTT Then
                PIMFieldType2String += "PTT"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_SIP_FOR_VIDEO Then
                PIMFieldType2String += "SIP for video"
                ' General fields
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_NOTE Then
                PIMFieldType2String += "Note"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_DTMF Then
                PIMFieldType2String += "DTMF"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_UID Then
                PIMFieldType2String += "UID"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_WIRELESS_VILLAGE Then
                PIMFieldType2String += "wireless village"
                ' Calendar sub types
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_DESCRIPTION Then
                PIMFieldType2String += "Description"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_LOCATION Then
                PIMFieldType2String += "Location"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_ITEM_DATA Then
                PIMFieldType2String += "" ' "Generic Item data"
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TODO_PRIORITY Then
                PIMFieldType2String += "todo prior."
            ElseIf pimData.iFieldSubType = CA_FIELD_SUB_TYPE_TODO_STATUS Then
                PIMFieldType2String += "todo status"
            Else
                PIMFieldType2String += "unknown field sub type" ' shouldn't occur
            End If
        End Function


        '===================================================================
        ' ReadContact
        '
        ' Reads contact from device
        '
        '===================================================================
        Friend Sub ReadContact(ByVal hOperHandle As Integer, ByVal UID As CA_ITEM_ID) ', ByRef dataContact As CA_DATA_CONTACT) As Integer
            Me.UID = UID

            Dim dataContact As New CA_DATA_CONTACT
            dataContact.iSize = Marshal.SizeOf(dataContact)
            dataContact.bPICount = 0
            dataContact.pPIFields = IntPtr.Zero
            dataContact.bAddressCount = 0
            dataContact.pAddressFields = IntPtr.Zero
            dataContact.bNumberCount = 0
            dataContact.pNumberFields = IntPtr.Zero
            dataContact.bGeneralCount = 0
            dataContact.pGeneralFields = IntPtr.Zero


            Dim iRet As Integer = CAReadItem(hOperHandle, CType(Me.UID, CA_ITEM_ID), CA_OPTION_USE_CACHE, CA_DATA_FORMAT_STRUCT, dataContact)
            If iRet = CONA_OK Then
                Me.GetContactDetails(dataContact)
            Else
                ShowErrorMessage("CAReadItem", iRet)

            End If

            ' Free memory allocated by DA API
            Dim iResult As Integer = CAFreeItemData(Me.ParentFolder.GetConnectionHandle, CA_DATA_FORMAT_STRUCT, dataContact)
            If iResult <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iResult)

        End Sub


        Protected Overrides Sub InternalDelete()
            Throw New NotImplementedException
        End Sub

        Public Overrides Function ToString() As String
            If (Me.ContactName <> "") Then Return Me.ContactName
            Return String.Concat(Me.FirstName, " ", Me.LastName)
        End Function
    End Class

End Class