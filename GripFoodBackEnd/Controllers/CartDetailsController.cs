using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GripFoodBackEnd.Entities;
using GripFoodBackEnd.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace GripFoodBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CartDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDetail>>> GetCartDetails()
        {
          if (_context.CartDetails == null)
          {
              return NotFound();
          }
            return await _context.CartDetails.ToListAsync();
        }

        // GET: api/CartDetails/5
        [HttpGet("{id}", Name = "GetCartDetail")]
        public async Task<ActionResult<CartDetailModel>> GetCartDetail(string id)
        {
          if (_context.CartDetails == null)
          {
              return NotFound();
          }

            var cartDetail = await _context.CartDetails
                  .AsNoTracking()
                  .Where(Q => Q.Id == id)
                  .Select(Q => new CartDetailModel
                  {
                      Id = Q.Id,
                      FoodItemsId = Q.FoodItemId,
                      FoodItemsPrice = Q.FoodItem.Price,
                      Qty = Q.Qty
                  }).FirstOrDefaultAsync();

            if (cartDetail == null)
            {
                return NotFound();
            }

            return cartDetail;
        }

        // PUT: api/CartDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartDetail(string id, CartDetail cartDetail)
        {
            if (id != cartDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CartDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name = "AddToCart")]
        public async Task<ActionResult<bool>> PostCartDetail(AddToCartModel model)
        {
          if (_context.CartDetails == null)
          {
              return Problem("Entity set 'ApplicationDbContext.CartDetails'  is null.");
          }

            var existing = await _context.CartDetails
                .Where(Q => Q.FoodItemId == model.FoodItemsId)
                .FirstOrDefaultAsync();

            // jangan lupa validasi qty jangan sampe over buy

            if (existing != null)
            {
                existing.Qty += model.Qty;
            }
            else
            {
                _context.CartDetails.Add(new CartDetail
                {
                    FoodItemId = model.FoodItemsId,
                    Qty = model.Qty,
                    Id = Ulid.NewUlid().ToString()
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE: api/CartDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartDetail(string id)
        {
            if (_context.CartDetails == null)
            {
                return NotFound();
            }
            var cartDetail = await _context.CartDetails.FindAsync(id);
            if (cartDetail == null)
            {
                return NotFound();
            }

            _context.CartDetails.Remove(cartDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartDetailExists(string id)
        {
            return (_context.CartDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
