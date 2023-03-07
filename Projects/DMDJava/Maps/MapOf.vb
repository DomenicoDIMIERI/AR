Namespace java
    ''' <summary>
    ''' public interface Map(K,V)
    ''' An object that maps keys to values. A map cannot contain duplicate keys; each key can map to at most one value.
    ''' This interface takes the place of the Dictionary class, which was a totally abstract class rather than an interface.
    ''' The Map interface provides three collection views, which allow a map's contents to be viewed as a set of keys, collection of values, or set of key-value mappings. The order of a map is defined as the order in which the iterators on the map's collection views return their elements. Some map implementations, like the TreeMap class, make specific guarantees as to their order; others, like the HashMap class, do not.
    ''' Note: great care must be exercised if mutable objects are used as map keys. The behavior of a map is not specified if the value of an object is changed in a manner that affects equals comparisons while the object is a key in the map. A special case of this prohibition is that it is not permissible for a map to contain itself as a key. While it is permissible for a map to contain itself as a value, extreme caution is advised: the equals and hashCode methods are no longer well defined on such a map.
    ''' All general-purpose map implementation classes should provide two "standard" constructors: a void (no arguments) constructor which creates an empty map, and a constructor with a single argument of type Map, which creates a new map with the same key-value mappings as its argument. In effect, the latter constructor allows the user to copy any map, producing an equivalent map of the desired class. There is no way to enforce this recommendation (as interfaces cannot contain constructors) but all of the general-purpose map implementations in the JDK comply.
    ''' The "destructive" methods contained in this interface, that is, the methods that modify the map on which they operate, are specified to throw UnsupportedOperationException if this map does not support the operation. If this is the case, these methods may, but are not required to, throw an UnsupportedOperationException if the invocation would have no effect on the map. For example, invoking the putAll(Map) method on an unmodifiable map may, but is not required to, throw the exception if the map whose mappings are to be "superimposed" is empty.
    ''' Some map implementations have restrictions on the keys and values they may contain. For example, some implementations prohibit null keys and values, and some have restrictions on the types of their keys. Attempting to insert an ineligible key or value throws an unchecked exception, typically NullPointerException or ClassCastException. Attempting to query the presence of an ineligible key or value may throw an exception, or it may simply return false; some implementations will exhibit the former behavior and some will exhibit the latter. More generally, attempting an operation on an ineligible key or value whose completion would not result in the insertion of an ineligible element into the map may throw an exception or it may succeed, at the option of the implementation. Such exceptions are marked as "optional" in the specification for this interface.
    ''' This interface is a member of the Java Collections Framework.
    ''' Many methods in Collections Framework interfaces are defined in terms of the equals method. For example, the specification for the containsKey(Object key) method says: "returns true if and only if this map contains a mapping for a key k such that (key==null ? k==null : key.equals(k))." This specification should not be construed to imply that invoking Map.containsKey with a non-null argument key will cause key.equals(k) to be invoked for any key k. Implementations are free to implement optimizations whereby the equals invocation is avoided, for example, by first comparing the hash codes of the two keys. (The Object.hashCode() specification guarantees that two objects with unequal hash codes cannot be equal.) More generally, implementations of the various Collections Framework interfaces are free to take advantage of the specified behavior of underlying Object methods wherever the implementor deems it appropriate.
    ''' </summary>
    ''' <typeparam name="TKey"></typeparam>
    ''' <typeparam name="TValue"></typeparam>
    ''' <remarks></remarks>
    Public Interface Map(Of TKey, TValue)
        Inherits Map

        ''' <summary>
        ''' Returns true if this map contains a mapping for the specified key.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function containsKey(ByVal key As TKey) As Boolean

        ''' <summary>
        ''' Returns true if this map maps one or more keys to the specified value.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function containsValue(ByVal value As TValue) As Boolean

        ''' <summary>
        ''' Returns a Set view of the mappings contained in this map.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function entrySet() As [Set](Of Map.Entry(Of TKey, TValue))


        ''' <summary>
        ''' Returns the value to which the specified key is mapped, or null if this map contains no mapping for the key.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function [get](ByVal v As TKey) As TValue


        ''' <summary>
        ''' Returns a Set view of the keys contained in this map.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function keySet() As [Set](Of TKey)

        ''' <summary>
        ''' Associates the specified value with the specified key in this map (optional operation).
        ''' </summary>
        ''' <param name="p1"></param>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Shadows Function put(p1 As TKey, value As TValue) As TValue

        ''' <summary>
        ''' Copies all of the mappings from the specified map to this map (optional operation).
        ''' </summary>
        ''' <param name="m"></param>
        ''' <remarks></remarks>
        Shadows Sub putAll(ByVal m As Map(Of TKey, TValue))

        ''' <summary>
        ''' Removes the mapping for a key from this map if it is present (optional operation).
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows Function remove(ByVal key As TKey) As TValue


        ''' <summary>
        ''' Returns a Collection view of the values contained in this map.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shadows ReadOnly Property Values() As ICollection(Of TValue)


    End Interface

End Namespace