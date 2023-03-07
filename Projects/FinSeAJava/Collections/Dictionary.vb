Namespace java
    ''' <summary>
    ''' public abstract class Dictionary
    ''' extends Object
    ''' The Dictionary class is the abstract parent of any class, such as Hashtable, which maps keys to values. Every key and every value is an object. In any one Dictionary object, every key is associated with at most one value. Given a Dictionary and a key, the associated element can be looked up. Any non-null object can be used as a key and as a value.
    ''' As a rule, the equals method should be used by implementations of this class to decide if two keys are the same.
    ''' NOTE: This class is obsolete. New implementations should implement the Map interface, rather than extending this class.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Dictionary
        ''' <summary>
        ''' Returns an enumeration of the values in this dictionary.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function elements() As Global.System.Collections.IEnumerable

        ''' <summary>
        ''' Returns the value to which the key is mapped in this dictionary.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function [get](ByVal key As Object) As Object

        ''' <summary>
        ''' Tests if this dictionary maps no keys to value.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function isEmpty() As Boolean

        ''' <summary>
        ''' Returns an enumeration of the keys in this dictionary.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function keys() As Global.System.Collections.IEnumerable

        ''' <summary>
        ''' Maps the specified key to the specified value in this dictionary.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function put(ByVal key As Object, ByVal value As Object) As Object

        ''' <summary>
        ''' Removes the key (and its corresponding value) from this dictionary.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function remove(ByVal key As Object) As Object

        ''' <summary>
        ''' Returns the number of entries (distinct keys) in this dictionary.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function size() As Integer

    End Class

End Namespace