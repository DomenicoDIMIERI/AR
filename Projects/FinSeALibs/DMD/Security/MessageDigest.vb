Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class MessageDigest

        'public abstract class MessageDigest
        'extends MessageDigestSpi

        'This MessageDigest class provides applications the functionality of a message digest algorithm, such as SHA-1 or SHA-256. Message digests are secure one-way hash functions that take arbitrary-sized data and output a fixed-length hash value.

        'A MessageDigest object starts out initialized. The data is processed through it using the update methods. At any point reset can be called to reset the digest. Once all the data to be updated has been updated, one of the digest methods should be called to complete the hash computation.

        'The digest method can be called once for a given number of updates. After digest has been called, the MessageDigest object is reset to its initialized state.

        'Implementations are free to implement the Cloneable interface. Client applications can test cloneability by attempting cloning and catching the CloneNotSupportedException:

        ' MessageDigest md = MessageDigest.getInstance("SHA");

        ' try {
        '     md.update(toChapter1);
        '     MessageDigest tc1 = md.clone();
        '     byte[] toChapter1Digest = tc1.digest();
        '     md.update(toChapter2);
        '     ...etc.
        ' } catch (CloneNotSupportedException cnse) {
        '     throw new DigestException("couldn't make digest of partial content");
        ' }


        'Note that if a given implementation is not cloneable, it is still possible to compute intermediate digests by instantiating several instances, if the number of digests is known in advance.

        'Note that this class is abstract and extends from MessageDigestSpi for historical reasons. Application developers should only take notice of the methods defined in this MessageDigest class; all the methods in the superclass are intended for cryptographic service providers who wish to supply their own implementations of message digest algorithms.

        'Every implementation of the Java platform is required to support the following standard MessageDigest algorithms:
        '    MD5
        '    SHA-1
        '    SHA-256
        'These algorithms are described in the MessageDigest section of the Java Cryptography Architecture Standard Algorithm Name Documentation. Consult the release documentation for your implementation to see if any other algorithms are supported.

        'See Also:
        '    DigestInputStream, DigestOutputStream

        '    Constructor Summary
        '    Constructors  Modifier 	Constructor and Description
        '    protected 	MessageDigest(String algorithm)
        '    Creates a message digest with the specified algorithm name.
        '    Method Summary
        '    Methods  Modifier and Type 	Method and Description
        '    Object 	clone()
        '    Returns a clone if the implementation is cloneable.
        '    byte[] 	digest()
        '    Completes the hash computation by performing final operations such as padding.
        '    byte[] 	digest(byte[] input)
        '    Performs a final update on the digest using the specified array of bytes, then completes the digest computation.
        '    int 	digest(byte[] buf, int offset, int len)
        '    Completes the hash computation by performing final operations such as padding.
        '    String 	getAlgorithm()
        '    Returns a string that identifies the algorithm, independent of implementation details.
        '    int 	getDigestLength()
        '    Returns the length of the digest in bytes, or 0 if this operation is not supported by the provider and the implementation is not cloneable.
        '    static MessageDigest 	getInstance(String algorithm)
        '    Returns a MessageDigest object that implements the specified digest algorithm.
        '    static MessageDigest 	getInstance(String algorithm, Provider provider)
        '    Returns a MessageDigest object that implements the specified digest algorithm.
        '    static MessageDigest 	getInstance(String algorithm, String provider)
        '    Returns a MessageDigest object that implements the specified digest algorithm.
        '    Provider 	getProvider()
        '    Returns the provider of this message digest object.
        '    static boolean 	isEqual(byte[] digesta, byte[] digestb)
        '    Compares two digests for equality.
        '    void 	reset()
        '    Resets the digest for further use.
        '    String 	toString()
        '    Returns a string representation of this message digest object.
        '    void 	update(byte input)
        '    Updates the digest using the specified byte.
        '    void 	update(byte[] input)
        '    Updates the digest using the specified array of bytes.
        '    void 	update(byte[] input, int offset, int len)
        '    Updates the digest using the specified array of bytes, starting at the specified offset.
        '    void 	update(ByteBuffer input)
        '    Update the digest using the specified ByteBuffer.
        '        Methods inherited from class java.security.MessageDigestSpi
        '        engineDigest, engineDigest, engineGetDigestLength, engineReset, engineUpdate, engineUpdate, engineUpdate
        '        Methods inherited from class java.lang.Object
        '        equals, finalize, getClass, hashCode, notify, notifyAll, wait, wait, wait

        '    Constructor Detail
        '        MessageDigest

        '        protected MessageDigest(String algorithm)

        '        Creates a message digest with the specified algorithm name.

        '        Parameters:
        '            algorithm - the standard name of the digest algorithm. See the MessageDigest section in the Java Cryptography Architecture Standard Algorithm Name Documentation for information about standard algorithm names.

        '    Method Detail
        '        getInstance

        '        public static MessageDigest getInstance(String algorithm)
        '                                         throws NoSuchAlgorithmException

        '        Returns a MessageDigest object that implements the specified digest algorithm.

        '        This method traverses the list of registered security Providers, starting with the most preferred Provider. A new MessageDigest object encapsulating the MessageDigestSpi implementation from the first Provider that supports the specified algorithm is returned.

        '        Note that the list of registered providers may be retrieved via the Security.getProviders() method.

        '        Parameters:
        '            algorithm - the name of the algorithm requested. See the MessageDigest section in the Java Cryptography Architecture Standard Algorithm Name Documentation for information about standard algorithm names.
        '        Returns:
        '            a Message Digest object that implements the specified algorithm.
        '        Throws:
        '            NoSuchAlgorithmException - if no Provider supports a MessageDigestSpi implementation for the specified algorithm.
        '        See Also:
        '            Provider

        '        getInstance

        '        public static MessageDigest getInstance(String algorithm,
        '                                String provider)
        '                                         throws NoSuchAlgorithmException,
        '                                                NoSuchProviderException

        '        Returns a MessageDigest object that implements the specified digest algorithm.

        '        A new MessageDigest object encapsulating the MessageDigestSpi implementation from the specified provider is returned. The specified provider must be registered in the security provider list.

        '        Note that the list of registered providers may be retrieved via the Security.getProviders() method.

        '        Parameters:
        '            algorithm - the name of the algorithm requested. See the MessageDigest section in the Java Cryptography Architecture Standard Algorithm Name Documentation for information about standard algorithm names.
        '            provider - the name of the provider.
        '        Returns:
        '            a MessageDigest object that implements the specified algorithm.
        '        Throws:
        '            NoSuchAlgorithmException - if a MessageDigestSpi implementation for the specified algorithm is not available from the specified provider.
        '            NoSuchProviderException - if the specified provider is not registered in the security provider list.
        '            IllegalArgumentException - if the provider name is null or empty.
        '        See Also:
        '            Provider

        '        getInstance

        '        public static MessageDigest getInstance(String algorithm,
        '                                Provider provider)
        '                                         throws NoSuchAlgorithmException

        '        Returns a MessageDigest object that implements the specified digest algorithm.

        '        A new MessageDigest object encapsulating the MessageDigestSpi implementation from the specified Provider object is returned. Note that the specified Provider object does not have to be registered in the provider list.

        '        Parameters:
        '            algorithm - the name of the algorithm requested. See the MessageDigest section in the Java Cryptography Architecture Standard Algorithm Name Documentation for information about standard algorithm names.
        '            provider - the provider.
        '        Returns:
        '            a MessageDigest object that implements the specified algorithm.
        '        Throws:
        '            NoSuchAlgorithmException - if a MessageDigestSpi implementation for the specified algorithm is not available from the specified Provider object.
        '            IllegalArgumentException - if the specified provider is null.
        '        Since:
        '            1.4
        '        See Also:
        '            Provider

        '        getProvider

        '        public final Provider getProvider()

        '        Returns the provider of this message digest object.

        '        Returns:
        '            the provider of this message digest object

        '        update

        '        public void update(byte input)

        '        Updates the digest using the specified byte.

        '        Parameters:
        '            input - the byte with which to update the digest.

        '        update

        '        public void update(byte[] input,
        '                  int offset,
        '                  int len)

        '        Updates the digest using the specified array of bytes, starting at the specified offset.

        '        Parameters:
        '            input - the array of bytes.
        '            offset - the offset to start from in the array of bytes.
        '            len - the number of bytes to use, starting at offset.

        '        update

        '        public void update(byte[] input)

        '        Updates the digest using the specified array of bytes.

        '        Parameters:
        '            input - the array of bytes.

        '        update

        '        public final void update(ByteBuffer input)

        '        Update the digest using the specified ByteBuffer. The digest is updated using the input.remaining() bytes starting at input.position(). Upon return, the buffer's position will be equal to its limit; its limit will not have changed.

        '        Parameters:
        '            input - the ByteBuffer
        '        Since:
        '            1.5

        '        digest

        '        public byte[] digest()

        '        Completes the hash computation by performing final operations such as padding. The digest is reset after this call is made.

        '        Returns:
        '            the array of bytes for the resulting hash value.

        '        digest

        '        public int digest(byte[] buf,
        '                 int offset,
        '                 int len)
        '                   throws DigestException

        '        Completes the hash computation by performing final operations such as padding. The digest is reset after this call is made.

        '        Parameters:
        '            buf - output buffer for the computed digest
        '            offset - offset into the output buffer to begin storing the digest
        '            len - number of bytes within buf allotted for the digest
        '        Returns:
        '            the number of bytes placed into buf
        '        Throws:
        '            DigestException - if an error occurs.

        '        digest

        '        public byte[] digest(byte[] input)

        '        Performs a final update on the digest using the specified array of bytes, then completes the digest computation. That is, this method first calls update(input), passing the input array to the update method, then calls digest().

        '        Parameters:
        '            input - the input to be updated before the digest is completed.
        '        Returns:
        '            the array of bytes for the resulting hash value.

        '        toString

        '        public String toString()

        '        Returns a string representation of this message digest object.

        '        Overrides:
        '            toString in class Object
        '        Returns:
        '            a string representation of the object.

        '        isEqual

        '        public static boolean isEqual(byte[] digesta,
        '                      byte[] digestb)

        '        Compares two digests for equality. Does a simple byte compare.

        '        Parameters:
        '            digesta - one of the digests to compare.
        '            digestb - the other digest to compare.
        '        Returns:
        '            true if the digests are equal, false otherwise.

        '        reset

        '        public void reset()

        '        Resets the digest for further use.
        '        getAlgorithm

        '        public final String getAlgorithm()

        '        Returns a string that identifies the algorithm, independent of implementation details. The name should be a standard Java Security name (such as "SHA", "MD5", and so on). See the MessageDigest section in the Java Cryptography Architecture Standard Algorithm Name Documentation for information about standard algorithm names.

        '        Returns:
        '            the name of the algorithm

        '        getDigestLength

        '        public final int getDigestLength()

        '        Returns the length of the digest in bytes, or 0 if this operation is not supported by the provider and the implementation is not cloneable.

        '        Returns:
        '            the digest length in bytes, or 0 if this operation is not supported by the provider and the implementation is not cloneable.
        '        Since:
        '            1.2

        '        clone

        '        public Object clone()
        '                     throws CloneNotSupportedException

        '        Returns a clone if the implementation is cloneable.

        '        Overrides:
        '            clone in class MessageDigestSpi
        '        Returns:
        '            a clone if the implementation is cloneable.
        '        Throws:
        '            CloneNotSupportedException - if this is called on an implementation that does not support Cloneable.
        '        See Also:
        '            Cloneable

        Shared Function getInstance(p1 As String) As MessageDigest
            Throw New NotImplementedException
        End Function

        Sub update(p1 As Byte())
            Throw New NotImplementedException
        End Sub

        Function digest() As Byte()
            Throw New NotImplementedException
        End Function

        Function digest(p1 As Byte()) As Byte()
            Throw New NotImplementedException
        End Function

        Function reset() As Integer
            Throw New NotImplementedException
        End Function

        Sub update(zero As Byte)
            Throw New NotImplementedException
        End Sub

        Function update(digest As Byte(), p2 As Integer, length As Integer) As Integer
            Throw New NotImplementedException
        End Function


    End Class

End Namespace