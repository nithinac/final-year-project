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
    public class MaintenanceTasksController : ApiController
    {
        private SandBoxContext db = new SandBoxContext();

        // GET: api/MaintenanceTasks
        public IQueryable<MaintenanceTask> GetMaintenanceTasks()
        {
            return db.MaintenanceTasks;
        }

        // GET: api/MaintenanceTasks/5
        [ResponseType(typeof(MaintenanceTask))]
        public async Task<IHttpActionResult> GetMaintenanceTask(int id)
        {
            MaintenanceTask maintenanceTask = await db.MaintenanceTasks.FindAsync(id);
            if (maintenanceTask == null)
            {
                return NotFound();
            }

            return Ok(maintenanceTask);
        }

        // PUT: api/MaintenanceTasks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMaintenanceTask(int id, MaintenanceTask maintenanceTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != maintenanceTask.ID)
            {
                return BadRequest();
            }

            db.Entry(maintenanceTask).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceTaskExists(id))
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

        // POST: api/MaintenanceTasks
        [ResponseType(typeof(MaintenanceTask))]
        public async Task<IHttpActionResult> PostMaintenanceTask(MaintenanceTask maintenanceTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MaintenanceTasks.Add(maintenanceTask);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = maintenanceTask.ID }, maintenanceTask);
        }

        // DELETE: api/MaintenanceTasks/5
        [ResponseType(typeof(MaintenanceTask))]
        public async Task<IHttpActionResult> DeleteMaintenanceTask(int id)
        {
            MaintenanceTask maintenanceTask = await db.MaintenanceTasks.FindAsync(id);
            if (maintenanceTask == null)
            {
                return NotFound();
            }

            db.MaintenanceTasks.Remove(maintenanceTask);
            await db.SaveChangesAsync();

            return Ok(maintenanceTask);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MaintenanceTaskExists(int id)
        {
            return db.MaintenanceTasks.Count(e => e.ID == id) > 0;
        }
    }
}