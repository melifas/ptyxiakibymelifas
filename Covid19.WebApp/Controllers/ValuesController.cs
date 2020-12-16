using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Covid19.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public FeatureCollection Get()
        {
            List<Feature> features = new List<Feature>();

            //foreach (RequestViewModel request in userRequests)
            //{
            //    Point geometry = new Point(new Position(request.Latitude, request.Longitude));
            //    Dictionary<string, object> properties = request.GetType()
            //        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            //        .ToDictionary(p => p.Name, p => p.GetValue(request));

            //    features.Add(new Feature(geometry, properties));
            //}

            Point geometry = new Point(new Position(38.0459061985, 23.8579639893));
            Dictionary<string, object> properties = new Dictionary<string, object>();
            features.Add(new Feature(geometry, properties, "1"));

            FeatureCollection model = new FeatureCollection(features);

            return model;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
