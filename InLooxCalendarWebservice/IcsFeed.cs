using System;
using System.Collections.Generic;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using InLoox.ODataClient.Data.BusinessObjects;

namespace InLooxCalendarWebservice
{
    public class IcsFeed
    {
        private const string INLOOX_DISPLAY_NAME = "InLoox";
        private const string TASK_ENTITY_DISPLAY_NAME = "Task"; // <- change this if you like
        private const string TIMEZONE_UTC = "UTC";
        private const string STANDARD_FREE_KEY = "FBTYPE";
        private const string MICROSOFT_FREE_KEY = "X-MICROSOFT-CDO-BUSYSTATUS";
        private const string FREE_VALUE = "FREE";
        private readonly TimeSpan _reminderTimespan = TimeSpan.FromMinutes(-30); // <- change this if you like

        private readonly IEnumerable<WorkPackageView> _items;

        public IcsFeed(IEnumerable<WorkPackageView> items)
        {
            _items = items;
        }

        public string Serialize()
        {
            
            // create calendar, Outlook will ignore the name
            var calendar = new Ical.Net.Calendar
            {
                Name = INLOOX_DISPLAY_NAME
            };

            foreach (var item in _items)
            {
                // check if task item has dates
                if (item.HasStartDate == true
                    && item.HasEndDate == true)
                {
                    // add basic info
                    var calendarItem = new CalendarEvent
                    {
                        Summary = item.Name,
                        Start = new CalDateTime(((DateTimeOffset)item.StartDateTime).UtcDateTime, TIMEZONE_UTC),
                        End = new CalDateTime(((DateTimeOffset)item.EndDateTime).UtcDateTime, TIMEZONE_UTC),
                        Categories = new List<string> { INLOOX_DISPLAY_NAME, TASK_ENTITY_DISPLAY_NAME, item.PlanningReservationId.ToString() },
                        Uid = item.PlanningReservationId.ToString(),
                        Description = string.Empty
                    };
                    
                    // add free/busy type
                    calendarItem.AddProperty(STANDARD_FREE_KEY, FREE_VALUE);
                    calendarItem.AddProperty(MICROSOFT_FREE_KEY, FREE_VALUE);

                    // add alarm, Outlook does not support this
                    var alarm = new Alarm()
                    {
                        Summary = item.Name,
                        Trigger = new Trigger(_reminderTimespan),
                        Action = AlarmAction.Display
                    };
                    calendarItem.Alarms.Add(alarm);

                    // add to calendar
                    calendar.Events.Add(calendarItem);
                }
            }
            
            // create ICS output
            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            return serializedCalendar;
        }
    }
}
