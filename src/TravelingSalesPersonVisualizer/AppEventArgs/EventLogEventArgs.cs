namespace TravelingSalesPersonVisualizer.AppEventArgs
{
    public class EventLogEventArgs
    {
        public EventLogEventArgs(string logText)
        {
            LogText = logText;
        }

        public string LogText { get; }
    }
}