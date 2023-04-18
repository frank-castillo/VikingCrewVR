public class SimpleTextMessage : IEventBusMessage
{
    public string Message { get; set; }
    public object Sender { get; set; }
    public SimpleTextMessage(object sender, string message)
    {
        Sender = sender;
        Message = message;
    }
}

//public class OnBeatInputMessage : IEventBusMessage
//{
//    public object Sender { get; set; }
//    public bool IsOnTime => _isOnTime;
//    private bool _isOnTime = false;

//    public OnBeatInputMessage(object sender, bool isOnTimeParam)
//    {
//        _isOnTime = isOnTimeParam;
//        Sender = sender;
//    }
//}