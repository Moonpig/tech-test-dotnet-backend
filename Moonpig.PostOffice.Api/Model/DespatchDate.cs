namespace Moonpig.PostOffice.Api.Model
{
    using System;

    public class DespatchDate
    {
        public DespatchDate()
        {
        }

        public DespatchDate(DateTime date)
        {
            Date = DateOnly.FromDateTime(date);
        }

        public DateOnly Date { get; set; }
    }
}