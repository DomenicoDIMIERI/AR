Namespace Serializable

    Public Interface IBinarySerializable

        Sub Serialize(ByVal writer As BinaryWriter)
        Sub Deserialize(ByVal reader As BinaryReader)

    End Interface

End Namespace
