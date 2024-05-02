using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moonpig.PostOffice.Api.Model;

namespace Moonpig.PostOffice.Api.Controllers
{
    [Route("api/[controller]")]
    public class DespatchDateController : Controller
    {
        private readonly IDespatchService _despatchService;

        public DespatchDateController(IDespatchService despatchService)
        {
            _despatchService = despatchService ?? throw new ArgumentNullException(nameof(despatchService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(DespatchDate), StatusCodes.Status200OK)]
        public ActionResult Get(List<int> productIds, DateTime orderDate)
        {
            var despatchDate = _despatchService.GetDespatchDates(productIds, orderDate);

            return Ok(despatchDate);
        }
    }
}