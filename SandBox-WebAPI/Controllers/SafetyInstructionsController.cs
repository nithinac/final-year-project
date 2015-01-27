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
    public class SafetyInstructionsController : ApiController
    {
        private SandBoxContext db = new SandBoxContext();

        // GET: api/SafetyInstructions
        public IQueryable<SafetyInstruction> GetSafetyInstructions()
        {
            return db.SafetyInstructions.Include("Attachment").Include("Attachment.Content");
            
        }

        // GET: api/SafetyInstructions/5
        [ResponseType(typeof(SafetyInstruction))]
        public async Task<IHttpActionResult> GetSafetyInstruction(int id)
        {
            SafetyInstruction safetyInstruction = await db.SafetyInstructions.Include("Attachment").Include("Attachment.Content").FirstOrDefaultAsync(s=>s.Id==id);
            if (safetyInstruction == null)
            {
                return NotFound();
            }

            return Ok(safetyInstruction);
        }

        // PUT: api/SafetyInstructions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSafetyInstruction(int id, SafetyInstruction safetyInstruction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != safetyInstruction.Id)
            {
                return BadRequest();
            }

            db.Entry(safetyInstruction).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SafetyInstructionExists(id))
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

        // POST: api/SafetyInstructions
        [ResponseType(typeof(SafetyInstruction))]
        public async Task<IHttpActionResult> PostSafetyInstruction(SafetyInstructionViewModel safetyInstructionView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SafetyInstruction safetyInstruction = new SafetyInstruction();
            safetyInstruction.Description = safetyInstructionView.Description;
            //ContentType contentType = await db.ContentTypes.FirstOrDefaultAsync(x => x.SafetyInstruction.Id == safetyInstruction.Id);
            ContentType contentType = new ContentType();
            contentType.Type = safetyInstructionView.Type;
            contentType.Extension = safetyInstructionView.Attachment.Substring(safetyInstructionView.Attachment.LastIndexOf("."));            
            Attachment attachment = new Attachment();
            attachment.Path = safetyInstructionView.Attachment;
            attachment.Content = contentType;
            attachment.FileName=safetyInstructionView.Attachment.Substring(safetyInstructionView.Attachment.LastIndexOf("\\")+1);
            
            safetyInstruction.Attachment = attachment;
            db.SafetyInstructions.Add(safetyInstruction);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = safetyInstruction.Id }, safetyInstruction);
        }

        // DELETE: api/SafetyInstructions/5
        [ResponseType(typeof(SafetyInstruction))]
        public async Task<IHttpActionResult> DeleteSafetyInstruction(int id)
        {
            SafetyInstruction safetyInstruction = await db.SafetyInstructions.FindAsync(id);
            if (safetyInstruction == null)
            {
                return NotFound();
            }

            db.SafetyInstructions.Remove(safetyInstruction);
            await db.SaveChangesAsync();

            return Ok(safetyInstruction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SafetyInstructionExists(int id)
        {
            return db.SafetyInstructions.Count(e => e.Id == id) > 0;
        }
    }
}