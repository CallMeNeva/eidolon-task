using System;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{
    public class EventService : MonoBehaviour, IEventService
    {
        private readonly string _serverUrl;
        private long _lastRequestUnixTimestampInSeconds;
        private readonly ISet<Event> _stashedEvents;

        public byte CooldownBeforeSendInSeconds { get; set; } = 120; // Default cooldown

        public EventService(string serverUrl)
        {
            _serverUrl = serverUrl;
            _lastRequestUnixTimestampInSeconds = 0;
            _stashedEvents = new HashSet<Event>();
        }

        public void TrackEvent(string type, string data)
        {
            var @event = new Event
            {
                Type = type,
                Data = data
            };

            TrackEvent(@event);
        }

        public void TrackEvent(Event @event)
        {
            // TODO: Make sure Event works with sets (in Java this implies a well-defined equals() method).
            _stashedEvents.Add(@event);

            if (CooldownHasElapsed())
            {
                // Send request. Only on 200 OK update timestamp and empty stash. Do neither otherwise.
            }
        }

        private class PostRequest
        {
            public ISet<Event> Events { get; set; }
        }

        private bool CooldownHasElapsed()
        {
            long currentUnixTimestampInSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long elapsedSeconds = currentUnixTimestampInSeconds - _lastRequestUnixTimestampInSeconds;

            return elapsedSeconds >= CooldownBeforeSendInSeconds;
        }
    }
}
