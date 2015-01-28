using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SandBox_WebAPI.Models;
using System.Reflection;
using SandBox_WebAPI.Utilities;

namespace SandBox_WebAPI.Controllers
{
    public class DoneOnDueOnsController : ApiController
    {
        private SandBoxContext db = new SandBoxContext();

        // GET: api/DoneOnDueOns
        public WebApiResponseList<DoneOnDueOn> GetDoneOnDueOns()
        {

            WebApiResponseList<DoneOnDueOn> response = new WebApiResponseList<DoneOnDueOn>();
            try
            {
                response.RequestUrl = Request.RequestUri.ToString();
                response.Version = WebApi.Version;
                response.Exception = null;
                response.StatusCode = "200";
                response.List = db.DoneOnDueOns.ToList();
            }
            catch(Exception e)
            {
                response.Exception = e;
                response.StatusCode = "500";
            }
            return response;
            //return db.DoneOnDueOns.Include("Checklist");
            
        }

        // GET: api/DoneOnDueOns/5
        [ResponseType(typeof(WebApiResponse<DoneOnDueOn>))]
        public async Task<IHttpActionResult> GetDoneOnDueOn(int id)
        {
            WebApiResponse<DoneOnDueOn> response = new WebApiResponse<DoneOnDueOn>();
            try
            {                
                response.RequestUrl = Request.RequestUri.ToString();
                response.Version = WebApi.Version;
                response.Data = await db.DoneOnDueOns.FindAsync(id);
                response.Exception = null;
                response.StatusCode = "200";
                if (response.Data == null)
                {
                    return NotFound();
                }

                response.Includes = new Dictionary<string, string>();
                foreach (var property in typeof(DoneOnDueOn).GetProperties())
                {
                    Type propertyType = property.PropertyType;
                    if (!(propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(DateTime)))
                    {                      
                        response.Includes.Add(property.Name, response.RequestUrl + "/" + property.Name);
                    }

                }
            }
            catch(Exception e)
            {
                response.Exception = e;
                response.StatusCode = "500";
            }
            return Ok(response);
        }

        public async Task<IHttpActionResult> GetSafetyInstruction(int id, string property)
        {
            WebApiResponseList<Object> response = new WebApiResponseList<Object>();
            try
            {
                DoneOnDueOn doneOnDueOn = await db.DoneOnDueOns.Include(property).FirstOrDefaultAsync(d => d.ID == id);

                if (doneOnDueOn == null)
                {
                    return NotFound();
                }

                response.RequestUrl = Request.RequestUri.ToString();
                response.Version = WebApi.Version;
                response.Exception = null;
                response.StatusCode = "200";
                response.List = doneOnDueOn.GetType().GetProperty(property).GetValue(doneOnDueOn,null) as List<Object>;

                if (response.List == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                response.Exception = e;
                response.StatusCode = "500";
            }
            return Ok(response);


        }

        // PUT: api/DoneOnDueOns/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDoneOnDueOn(int id, DoneOnDueOn doneOnDueOn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doneOnDueOn.ID)
            {
                return BadRequest();
            }

            db.Entry(doneOnDueOn).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoneOnDueOnExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DoneOnDueOns
        [ResponseType(typeof(DoneOnDueOn))]
        public async Task<IHttpActionResult> PostDoneOnDueOn(DoneOnDueOnViewModel doneOnDueOnView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DoneOnDueOn doneOnDueOn = new DoneOnDueOn();
            doneOnDueOn.Checklist = await db.MaintenanceTasks.FindAsync(doneOnDueOnView.TaskId);
            doneOnDueOn.DoneOn = DateTime.Today;
            doneOnDueOn.DueOn = DateTime.Today.AddDays(doneOnDueOn.Checklist.Period);
            db.DoneOnDueOns.Add(doneOnDueOn);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = doneOnDueOn.ID }, doneOnDueOn);
        }

        // DELETE: api/DoneOnDueOns/5
        [ResponseType(typeof(DoneOnDueOn))]
        public async Task<IHttpActionResult> DeleteDoneOnDueOn(int id)
        {
            DoneOnDueOn doneOnDueOn = await db.DoneOnDueOns.FindAsync(id);
            if (doneOnDueOn == null)
            {
                return NotFound();
            }

            db.DoneOnDueOns.Remove(doneOnDueOn);
            await db.SaveChangesAsync();

            return Ok(doneOnDueOn);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoneOnDueOnExists(int id)
        {
            return db.DoneOnDueOns.Count(e => e.ID == id) > 0;
        }
    }
}