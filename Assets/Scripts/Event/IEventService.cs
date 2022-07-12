namespace Event
{
    public interface IEventService
    {
        public void TrackEvent(string type, string data);

        public void TrackEvent(Event @event);
    }
}
