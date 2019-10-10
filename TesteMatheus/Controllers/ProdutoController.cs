using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TesteMatheus.Models;

namespace TesteMatheus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoContext _context;

        public ProdutoController(ProdutoContext context)
        {
            _context = context;

            if (_context.Produtos.Count() == 0)
            {
                _context.Produtos.Add(new Produto { Descricao = "Guitarra", Valor = 800 });
                _context.SaveChanges();
            }
        }

        // GET: api/Produto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        // GET: api/Produto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // POST: api/Produto
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto item)
        {
            //Buscando todos e veridicando se já existe um produto com esta descricao
            var produtos = await _context.Produtos.ToListAsync();
            foreach(var p in produtos)
            {
                if(p.Descricao == item.Descricao)
                {
                    return BadRequest();
                }
            }

            //Verificando se o valor é menor que zero ou maior que 999
            if(item.Valor > 999) return BadRequest();
            if(item.Valor < 0) return BadRequest();

            _context.Produtos.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = item.Id }, item);
        }

        // PUT: api/Produto/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            //Buscando todos e veridicando se já existe um produto com esta descricao
            var produtos = await _context.Produtos.ToListAsync();
            foreach (var p in produtos)
            {
                if (p.Descricao == item.Descricao)
                {
                    return BadRequest();
                }
            }

            //Verificando se o valor é menor que zero ou maior que 999
            if (item.Valor > 999) return BadRequest();
            if (item.Valor < 0) return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Produto/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
