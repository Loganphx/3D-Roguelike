namespace ECS.Core.Components
{
    public static class MessageExtensions
    {
        // 16 bytes
        // public static void AddQuaternion(this Message message, Quaternion quaternion)
        // {
        //     // message.AddFloat(quaternion.x);
        //     // message.AddFloat(quaternion.y);
        //     // message.AddFloat(quaternion.z);
        //     // message.AddFloat(quaternion.w);
        // }
        //
        // // 12 bytes
        // public static void AddVector3(this Message message, Vector3 vector3)
        // {
        //     message.AddFloat(vector3.x);
        //     message.AddFloat(vector3.y);
        //     message.AddFloat(vector3.z);
        // }
        //
        // public static Message Serialize<T>(this Message message, T value) where T : ISerializable
        // {
        //     value.Serialize(message);
        //
        //     return message;
        // }
        //
        // public static Quaternion GetQuaternion(this Message message)
        // {
        //     return new Quaternion(message.GetFloat(),
        //         message.GetFloat(),
        //         message.GetFloat(),
        //         message.GetFloat());
        // }
        //
        // // 12 bytes
        // public static Vector3 GetVector3(this Message message)
        // {
        //     return new Vector3(message.GetFloat(),
        //         message.GetFloat(),
        //         message.GetFloat());
        // }
    }
}