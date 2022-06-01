namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EventScheduler")]
    public partial class EventScheduler
    {
        public long ID { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool IsAllDay { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [StringLength(100)]
        public string StartTimezone { get; set; }

        [StringLength(100)]
        public string EndTimezone { get; set; }

        public long? RecurrenceID { get; set; }

        [StringLength(50)]
        public string RecurrenceRule { get; set; }

        [StringLength(50)]
        public string RecurrenceException { get; set; }

        [StringLength(20)]
        public string ThemColor { get; set; }
    }
}
