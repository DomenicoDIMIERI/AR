Imports FinSeA
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.interactive.digitalsignature

    '/**
    ' * This represents a pdf signature seed value dictionary.
    ' *
    ' * @author Thomas Chojecki
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDSeedValue
        Implements COSObjectable

        ''' <summary>
        ''' A Ff flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_FILTER = 1

        Public Const FLAG_SUBFILTER = 1 << 1

        Public Const FLAG_V = 1 << 2

        Public Const FLAG_REASON = 1 << 3

        Public Const FLAG_LEGAL_ATTESTATION = 1 << 4

        Public Const FLAG_ADD_REV_INFO = 1 << 5

        Public Const FLAG_DIGEST_METHOD = 1 << 6

        Private dictionary As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
            dictionary.setItem(COSName.TYPE, COSName.SV)
            dictionary.setDirect(True) ' the specification claim to use direct objects
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict The signature dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
            dictionary.setDirect(True) ' the specification claim to use direct objects
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return getDictionary()
        End Function

        '/**
        ' * Convert this standard java object to a COS dictionary.
        ' *
        ' * @return The COS dictionary that matches this Java object.
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' *
        ' * @return true if the Filter is required
        ' */
        Public Function isFilterRequired() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_FILTER)
        End Function

        '/**
        ' * set true if the filter shall be required.
        ' * 
        ' * @param flag if true, the specified Filter shall be used when signing.
        ' */
        Public Sub setFilterRequired(ByVal flag As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_FILTER, flag)
        End Sub

        '/**
        ' *
        ' * @return true if the SubFilter is required
        ' */
        Public Function isSubFilterRequired() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_SUBFILTER)
        End Function

        '/**
        ' * set true if the subfilter shall be required.
        ' * 
        ' * @param flag if true, the first supported SubFilter in the array shall be used when signing.
        ' */
        Public Sub setSubFilterRequired(ByVal flag As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_SUBFILTER, flag)
        End Sub

        '/**
        '*
        '* @return true if the DigestMethod is required
        '*/
        Public Function isDigestMethodRequired() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_DIGEST_METHOD)
        End Function

        '/**
        ' * set true if the DigestMethod shall be required.
        ' * 
        ' * @param flag if true, one digest from the array shall be used.
        ' */
        Public Sub setDigestMethodRequired(ByVal flag As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_DIGEST_METHOD, flag)
        End Sub

        '/**
        '*
        '* @return true if the V entry is required
        '*/
        Public Function isVRequired() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_V)
        End Function

        '/**
        ' * set true if the V entry shall be required.
        ' * 
        ' * @param flag if true, the V entry shall be used.
        ' */
        Public Sub setVRequired(ByVal flag As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_V, flag)
        End Sub

        '/**
        '*
        '* @return true if the Reason is required
        '*/
        Public Function isReasonRequired() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_REASON)
        End Function

        '/**
        ' * set true if the Reason shall be required.
        ' * 
        ' * @param flag if true, the Reason entry shall be used.
        ' */
        Public Sub setReasonRequired(ByVal flag As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_REASON, flag)
        End Sub

        '/**
        '*
        '* @return true if the LegalAttestation is required
        '*/
        Public Function isLegalAttestationRequired() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_LEGAL_ATTESTATION)
        End Function

        '/**
        ' * set true if the LegalAttestation shall be required.
        ' * 
        ' * @param flag if true, the LegalAttestation entry shall be used.
        ' */
        Public Sub setLegalAttestationRequired(ByVal flag As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_LEGAL_ATTESTATION, flag)
        End Sub

        '/**
        '*
        '* @return true if the AddRevInfo is required
        '*/
        Public Function isAddRevInfoRequired() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_ADD_REV_INFO)
        End Function

        '/**
        ' * set true if the AddRevInfo shall be required.
        ' * 
        ' * @param flag if true, the AddRevInfo shall be used.
        ' */
        Public Sub setAddRevInfoRequired(ByVal flag As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_ADD_REV_INFO, flag)
        End Sub

        '/**
        ' * If <b>Filter</b> is not null and the {@link #isFilterRequired()} indicates this entry is a
        ' * required constraint, then the signature handler specified by this entry shall be used when
        ' * signing; otherwise, signing shall not take place. If {@link #isFilterRequired()} indicates
        ' * that this is an optional constraint, this handler may be used if it is available. If it is
        ' * not available, a different handler may be used instead.
        ' *
        ' * @return the filter that shall be used by the signature handler
        ' */
        Public Function getFilter() As String
            Return dictionary.getNameAsString(COSName.FILTER)
        End Function

        '/**
        ' * (Optional) The signature handler that shall be used to sign the signature field.
        ' *
        ' * @param filter is the filter that shall be used by the signature handler
        ' */
        Public Sub setFilter(ByVal filter As COSName)
            dictionary.setItem(COSName.FILTER, filter)
        End Sub

        '/**
        ' * If <b>SubFilter</b> is not null and the {@link #isSubFilterRequired()} indicates this
        ' * entry is a required constraint, then the first matching encodings shall be used when
        ' * signing; otherwise, signing shall not take place. If {@link #isSubFilterRequired()}
        ' * indicates that this is an optional constraint, then the first matching encoding shall
        ' * be used if it is available. If it is not available, a different encoding may be used
        ' * instead.
        ' *
        ' * @return the subfilter that shall be used by the signature handler
        ' */
        Public Function getSubFilter() As List(Of String)
            Dim retval As List(Of String) = Nothing
            Dim fields As COSArray = dictionary.getDictionaryObject(COSName.SUBFILTER)

            If (fields IsNot Nothing) Then
                Dim actuals As List(Of String) = New ArrayList(Of String)()
                For i As Integer = 0 To fields.size() - 1
                    Dim element As String = fields.getName(i)
                    If (element <> "") Then
                        actuals.add(element)
                    End If
                Next
                retval = New COSArrayList(actuals, fields)
            End If
            Return retval
        End Function

        '/**
        ' * (Optional) An array of names indicating encodings to use when signing. The first name
        ' * in the array that matches an encoding supported by the signature handler shall be the
        ' * encoding that is actually used for signing.
        ' *
        ' * @param subfilter is the name that shall be used for encoding
        ' */
        Public Sub setSubFilter(ByVal subfilter As List(Of COSName))
            dictionary.setItem(COSName.SUBFILTER, COSArrayList.converterToCOSArray(subfilter))
        End Sub

        '/**
        ' * An array of names indicating acceptable digest algorithms to use when
        ' * signing. The value shall be one of <b>SHA1</b>, <b>SHA256</b>, <b>SHA384</b>,
        ' * <b>SHA512</b>, <b>RIPEMD160</b>. The default value is implementation-specific.
        ' *
        ' * @return the digest method that shall be used by the signature handler
        ' */
        Public Function getDigestMethod() As List(Of String)
            Dim retval As List(Of String) = Nothing
            Dim fields As COSArray = dictionary.getDictionaryObject(COSName.DIGEST_METHOD)

            If (fields IsNot Nothing) Then
                Dim actuals As List(Of String) = New ArrayList(Of String)()
                For i As Integer = 0 To fields.size() - 1
                    Dim element As String = fields.getName(i)
                    If (element <> "") Then
                        actuals.add(element)
                    End If
                Next
                retval = New COSArrayList(actuals, fields)
            End If
            Return retval
        End Function

        '/**
        ' * <p>(Optional, PDF 1.7) An array of names indicating acceptable digest
        ' * algorithms to use when signing. The value shall be one of <b>SHA1</b>,
        ' * <b>SHA256</b>, <b>SHA384</b>, <b>SHA512</b>, <b>RIPEMD160</b>. The default
        ' * value is implementation-specific.</p>
        ' *
        ' * <p>This property is only applicable if the digital credential signing contains RSA
        ' * public/privat keys</p>
        ' *
        ' * @param digestMethod is a list of possible names of the digests, that should be
        ' * used for signing.
        ' */
        Public Sub setDigestMethod(ByVal digestMethod As List(Of COSName))
            ' integrity check
            For Each cosName As COSName In digestMethod
                If (Not (cosName.equals(cosName.DIGEST_SHA1) OrElse cosName.equals(cosName.DIGEST_SHA256) OrElse cosName.equals(cosName.DIGEST_SHA384) OrElse cosName.equals(cosName.DIGEST_SHA512) OrElse cosName.equals(cosName.DIGEST_RIPEMD160))) Then
                    Throw New ArgumentException("Specified digest " & cosName.getName() & " isn't allowed.")
                End If
            Next
            dictionary.setItem(COSName.DIGEST_METHOD, COSArrayList.converterToCOSArray(digestMethod))
        End Sub

        '/**
        ' * The minimum required capability of the signature field seed value
        ' * dictionary parser. A value of 1 specifies that the parser shall be able to
        ' * recognize all seed value dictionary entries in a PDF 1.5 file. A value of 2
        ' * specifies that it shall be able to recognize all seed value dictionary entries
        ' * specified.
        ' *
        ' * @return the minimum required capability of the signature field seed value
        ' * dictionary parser
        ' */
        Public Function getV() As Single
            Return dictionary.getFloat(COSName.V)
        End Function

        '/**
        ' * (Optional) The minimum required capability of the signature field seed value
        ' * dictionary parser. A value of 1 specifies that the parser shall be able to
        ' * recognize all seed value dictionary entries in a PDF 1.5 file. A value of 2
        ' * specifies that it shall be able to recognize all seed value dictionary entries
        ' * specified.
        ' *
        ' * @param minimumRequiredCapability is the minimum required capability of the
        ' * signature field seed value dictionary parser
        ' */
        Public Sub setV(ByVal minimumRequiredCapability As Single)
            dictionary.setFloat(COSName.V, minimumRequiredCapability)
        End Sub

        '/**
        ' * If the Reasons array is provided and {@link #isReasonRequired()} indicates that
        ' * Reasons is a required constraint, one of the reasons in the array shall be used
        ' * for the signature dictionary; otherwise signing shall not take place. If the
        ' * {@link #isReasonRequired()} indicates Reasons is an optional constraint, one of
        ' * the reasons in the array may be chose or a custom reason can be provided.
        ' *
        ' * @return the reasons that should be used by the signature handler
        ' */
        Public Function getReasons() As List(Of String)
            Dim retval As List(Of String) = Nothing
            Dim fields As COSArray = dictionary.getDictionaryObject(COSName.REASONS)

            If (fields IsNot Nothing) Then
                Dim actuals As List(Of String) = New ArrayList(Of String)()
                For i As Integer = 0 To fields.size() - 1
                    Dim element As String = fields.getString(i)
                    If (element <> "") Then
                        actuals.add(element)
                    End If
                Next
                retval = New COSArrayList(actuals, fields)
            End If
            Return retval
        End Function

        '/**
        ' * (Optional) An array of text strings that specifying possible reasons for signing
        ' * a document. If specified, the reasons supplied in this entry replace those used
        ' * by conforming products.
        ' *
        ' * @param reasons is a list of possible text string that specifying possible reasons
        ' */
        Public Sub setReasonsd(ByVal reasons As List(Of String))
            dictionary.setItem(COSName.REASONS, COSArrayList.converterToCOSArray(reasons))
        End Sub

        '/**
        ' * <p>(Optional; PDF 1.6) A dictionary containing a single entry whose key is P
        ' * and whose value is an integer between 0 and 3. A value of 0 defines the
        ' * signatures as an author signature. The value 1 through 3 shall be used for
        ' * certification signatures and correspond to the value of P in a DocMDP transform
        ' * parameters dictionary.</p>
        ' *
        ' * <p>If this MDP key is not present or the MDP dictionary does not contain a P
        ' * entry, no rules shall be defined regarding the type of signature or its
        ' * permissions.</p>
        ' *
        ' * @return the mdp dictionary as PDSeedValueMDP
        ' */
        Public Function getMDP() As PDSeedValueMDP
            Dim dict As COSDictionary = dictionary.getDictionaryObject(COSName.MDP)
            Dim mdp As PDSeedValueMDP = Nothing
            If (dict IsNot Nothing) Then
                mdp = New PDSeedValueMDP(dict)
            End If
            Return mdp
        End Function

        '/**
        ' * <p>(Optional; PDF 1.6) A dictionary containing a single entry whose key is P
        ' * and whose value is an integer between 0 and 3. A value of 0 defines the
        ' * signatures as an author signature. The value 1 through 3 shall be used for
        ' * certification signatures and correspond to the value of P in a DocMDP transform
        ' * parameters dictionary.</p>
        ' *
        ' * <p>If this MDP key is not present or the MDP dictionary does not contain a P
        ' * entry, no rules shall be defined regarding the type of signature or its
        ' * permissions.</p>
        ' *
        ' * @param mdp dictionary
        ' */
        Public Sub setMPD(ByVal mdp As PDSeedValueMDP)
            If (mdp IsNot Nothing) Then
                dictionary.setItem(COSName.MDP, mdp.getCOSObject())
            End If
        End Sub

        '/**
        ' * <p>(Optional; PDF 1.6) A time stamp dictionary containing two entries. URL which
        ' * is a ASCII string specifying the URL to a rfc3161 conform timestamp server and Ff
        ' * to indicate if a timestamp is required or optional.</p>
        ' *
        ' * @return the timestamp dictionary as PDSeedValueTimeStamp
        ' */
        Public Function getTimeStamp() As PDSeedValueTimeStamp
            Dim dict As COSDictionary = dictionary.getDictionaryObject(COSName.TIME_STAMP)
            Dim timestamp As PDSeedValueTimeStamp = Nothing
            If (dict IsNot Nothing) Then
                timestamp = New PDSeedValueTimeStamp(dict)
            End If
            Return timestamp
        End Function

        '/**
        ' * <p>(Optional; PDF 1.6) A time stamp dictionary containing two entries. URL which
        ' * is a ASCII string specifying the URL to a rfc3161 conform timestamp server and Ff
        ' * to indicate if a timestamp is required or optional.</p>
        ' *
        ' * @param timestamp dictionary
        ' */
        Public Sub setTimeStamp(ByVal timestamp As PDSeedValueTimeStamp)
            If (timestamp IsNot Nothing) Then
                dictionary.setItem(COSName.TIME_STAMP, timestamp.getCOSObject())
            End If
        End Sub

        '/**
        ' * (Optional, PDF 1.6) An array of text strings that specifying possible legal
        ' * attestations.
        ' *
        ' * @return the reasons that should be used by the signature handler
        ' */
        Public Function getLegalAttestation() As List(Of String)
            Dim retval As List(Of String) = Nothing
            Dim fields As COSArray = dictionary.getDictionaryObject(COSName.LEGAL_ATTESTATION)

            If (fields IsNot Nothing) Then
                Dim actuals As List(Of String) = New ArrayList(Of String)()
                For i As Integer = 0 To fields.size() - 1
                    Dim element As String = fields.getString(i)
                    If (element <> "") Then
                        actuals.add(element)
                    End If
                Next
                retval = New COSArrayList(actuals, fields)
            End If
            Return retval
        End Function

        '/**
        ' * (Optional, PDF 1.6) An array of text strings that specifying possible legal
        ' * attestations.
        ' *
        ' * @param legalAttestation is a list of possible text string that specifying possible
        ' * legal attestations.
        ' */
        Public Sub setLegalAttestation(ByVal legalAttestation As List(Of String))
            dictionary.setItem(COSName.LEGAL_ATTESTATION, COSArrayList.converterToCOSArray(legalAttestation))
        End Sub

    End Class

End Namespace
