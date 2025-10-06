#define Primary
#if Primary
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendAPI.User.API.Models;
using BackendAPI.User.API.Data;

#region UserController
namespace BackendAPI.User.API.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        #endregion

        public UserController(ApplicationDbContext context)
        {
            _context = context;

            if (_context.UserItems.Count() == 0)
            {
                // Create a new UserItem if collection is empty,
                // which means you can't delete all UserItems.
                _context.UserItems.Add(new UserItem { Username = "Item1" });
                _context.SaveChanges();
            }
        }

        #region snippet_GetAll
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItem>>> GetUserItems()
        {
            return await _context.UserItems.ToListAsync();
        }

        #region snippet_GetByID
        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserItem>> GetUserItem(long id)
        {
            var UserItem = await _context.UserItems.FindAsync(id);

            if (UserItem == null)
            {
                return NotFound();
            }

            return UserItem;
        }
        #endregion
        #endregion

        #region snippet_Create
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserItem>> PostUserItem(UserItem item)
        {
            _context.UserItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserItem), new { id = item.Id }, item);
            //return CreatedAtAction(nameof(GetUserItem), new { id = item.Id }, item);
        }
        #endregion

        #region snippet_Update
        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem(string id, UserItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region snippet_Delete
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserItem(long id)
        {
            var UserItem = await _context.UserItems.FindAsync(id);

            if (UserItem == null)
            {
                return NotFound();
            }

            _context.UserItems.Remove(UserItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
#endif
