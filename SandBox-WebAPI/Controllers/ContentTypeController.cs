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
    public class ContentTypeController : ApiController
    {
        private SandBoxContext db = new SandBoxContext();

        // GET: api/ContentType
        public IQueryable<ContentType> GetContentTypes()
        {
            return db.ContentTypes;
        }

        // GET: api/ContentType/5
        [ResponseType(typeof(ContentType))]
        public async Task<IHttpActionResult> GetContentType(int id)
        {
            ContentType contentType = await db.ContentTypes.FindAsync(id);
            if (contentType == null)
            {
                return NotFound();
            }

            return Ok(contentType);
        }

        // PUT: api/ContentType/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContentType(int id, ContentType contentType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contentType.ID)
            {
                return BadRequest();
            }

            db.Entry(contentType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContentTypeExists(id))
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

        // POST: api/ContentType
        [ResponseType(typeof(ContentType))]
        public async Task<IHttpActionResult> PostContentType(ContentTypeViewModel contentTypeView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ContentType contentType = new ContentType();
            contentType.Type = contentTypeView.Type;
            contentType.Extension = contentTypeView.Extension;
            //contentType.SafetyInstruction= await db.SafetyInstructions.FindAsync(contentTypeView.SafetyInstructionId);
            db.ContentTypes.Add(contentType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = contentType.ID }, contentType);
        }

        // DELETE: api/ContentType/5
        [ResponseType(typeof(ContentType))]
        public async Task<IHttpActionResult> DeleteContentType(int id)
        {
            ContentType contentType = await db.ContentTypes.FindAsync(id);
            if (contentType == null)
            {
                return NotFound();
            }

            db.ContentTypes.Remove(contentType);
            await db.SaveChangesAsync();

            return Ok(contentType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContentTypeExists(int id)
        {
            return db.ContentTypes.Count(e => e.ID == id) > 0;
        }
    }
}