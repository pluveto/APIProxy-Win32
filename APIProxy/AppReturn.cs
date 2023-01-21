using System.Runtime.Serialization;

enum AppReturnType
{
    Success,
    ApplicationError,
    HandlerError,

}
[DataContract]

internal class AppReturn<T> where T : class
{
    [DataMember]

    public AppReturnType Type { get; private set; }
    [DataMember]

    public string HMsg { get; private set; } // Handler message, such as handler's error name key
    [DataMember]

    public string Msg { get; private set; }
    [DataMember]

    public T Data { get; private set; }

    public AppReturn(AppReturnType type, string message = "", T data = null, string hmsg = "")
    {
        Type = type;
        Msg = message;
        Data = data;
        HMsg = hmsg;
    }

    public static AppReturn<T> ofAppError(string message) => new AppReturn<T>(AppReturnType.ApplicationError, message);
    public static AppReturn<T> ofHandlerError(string message, string hmsg) => new AppReturn<T>(AppReturnType.HandlerError, message, hmsg: hmsg);
    public static AppReturn<T> ofSuccess(T data) => new AppReturn<T>(AppReturnType.Success, "", data);
}