﻿namespace PipelineApp2._0.Domain
{
    public class WeekDay
    {
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public string Name { get; set; }
        public bool IsWorkDay { get; set; }
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 0;
    }
}
