﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Model.EF;

namespace DemoGridview.Controllers
{
    public class EventSchedulerViewModel: ISchedulerEvent
    {
        public EventSchedulerViewModel()
        {
        }

        public EventSchedulerViewModel(EventScheduler eventScheduler)
        {
                ID = eventScheduler.ID;
                Title = eventScheduler.Title;
                Start = DateTime.SpecifyKind(eventScheduler.Start, DateTimeKind.Utc);
                End = DateTime.SpecifyKind(eventScheduler.End, DateTimeKind.Utc);
                StartTimezone = eventScheduler.StartTimezone;
                EndTimezone = eventScheduler.EndTimezone;
                Description = eventScheduler.Description;
                IsAllDay = eventScheduler.IsAllDay;
                RecurrenceRule = eventScheduler.RecurrenceRule;
                RecurrenceException = eventScheduler.RecurrenceException;
                RecurrenceID = eventScheduler.RecurrenceID;
        }

        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        private DateTime start;
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        private DateTime end;
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }

        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }

        public string RecurrenceRule { get; set; }
        public long? RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }


        public EventScheduler ToEntity()
        {
            return new EventScheduler
            {
                ID = ID,
                Title = Title,
                Start = Start,
                End = End,
                StartTimezone = StartTimezone,
                EndTimezone = EndTimezone,
                Description = Description,
                IsAllDay = IsAllDay,
                RecurrenceRule = RecurrenceRule,
                RecurrenceException = RecurrenceException,
                RecurrenceID = RecurrenceID
            };
        }
    }
}
