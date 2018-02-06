namespace Moonpig.PostOffice.Domain
{
    using System;

    public interface IPostOffice
    {
        DateTime CalculateDespatchDate(Order order);
    }
}
