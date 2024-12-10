using System;

namespace Shared.Core.Entities 
{
    public class EntityReference(string entity)
    {
        public void Increment()
        {
            LastUpdateOn = DateTime.Now;
            Count++;
        }

        public int Id { get; private set; }

        public string Entity { get; private set; } = entity;

        public string MonthYearString { get; private set; } = DateTime.Now.ToString("MMyy");

        public int Count { get; private set; } = 1;

        public DateTime LastUpdateOn { get; private set; } = DateTime.Now;
    }
}