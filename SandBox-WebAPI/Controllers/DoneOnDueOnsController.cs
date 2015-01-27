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

namespace SandBox_WebAPI.Controllers
{
    public class DoneOnDueOnsController : ApiController
    {
        private SandBoxContext db = new SandBoxContext();

        // GET: api/DoneOnDueOns
        public IQueryable<DoneOnDueOn> GetDoneOnDueOns()
        {
            return db.DoneOnDueOns.Include("Checklist");
        }

        // GET: api/DoneOnDueOns/5
        [ResponseType(typeof(DoneOnDueOn))]
        public async Task<IHttpActionResult> GetDoneOnDueOn(int id)
        {
            DoneOnDueOn doneOnDueOn = await db.DoneOnDueOns.Include("Checklist").FirstOrDefaultAsync(d=>d.ID==id);
            if (doneOnDueOn == null)
            {
                return NotFound();
            }

            return Ok(doneOnDueOn);
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