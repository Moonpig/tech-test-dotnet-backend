using System;
using System.Collections.Generic;
using Moonpig.PostOffice.Api.Model;

namespace Moonpig.PostOffice.Api.Interfaces
{
    public interface IDespatchService
    {
        DespatchDate GetDespatchDates(List<int> productIds, DateTime orderDate);
    }
}