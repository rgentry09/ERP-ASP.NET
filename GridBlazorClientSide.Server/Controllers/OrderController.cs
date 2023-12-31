﻿using GridBlazorClientSide.Server.Models;
using GridBlazorClientSide.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GridBlazorClientSide.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly NorthwindDbContext _context;

        public OrderController(NorthwindDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetAllOrders()
        {
            var repository = new OrdersRepository(_context);
            var orders = repository.GetAll().ToList();
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                if (order == null)
                {
                    return BadRequest();
                }

                var repository = new OrdersRepository(_context);
                try
                {
                    await repository.Insert(order);
                    repository.Save();

                    return Ok(order.OrderID);
                }
                catch (Exception e)
                {
                    return BadRequest(new
                    {
                        message = e.Message.Replace('{', '(').Replace('}', ')')
                    });
                }
            }
            return BadRequest(new
            {
                message = "ModelState is not valid"
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrder(int id)
        {
            var repository = new OrdersRepository(_context);
            Order order = await repository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                if (order == null || order.OrderID != id)
                {
                    return BadRequest();
                }

                var repository = new OrdersRepository(_context);
                try
                {
                    await repository.Update(order);
                    repository.Save();

                    return NoContent();
                }
                catch (Exception e)
                {
                    return BadRequest(new
                    {
                        message = e.Message.Replace('{', '(').Replace('}', ')')
                    });
                }
            }
            return BadRequest(new
            {
                message = "ModelState is not valid"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var repository = new OrdersRepository(_context);
            Order order = await repository.GetById(id);

            if (order == null)
            {
                return NotFound();
            }

            try
            {
                repository.Delete(order);
                repository.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = e.Message.Replace('{', '(').Replace('}', ')')
                });
            }
        }
    }
}
