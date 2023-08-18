using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Persistence;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private DatabaseContext _context { get; set; }

    public PersonController(DatabaseContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public ActionResult<List<Person>> Get()
    {
        var result = _context.Pessoas.Include(p => p.Contratos).ToList();

        if (!result.Any()) return NoContent();

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<Person> Post([FromBody] Person pessoa)
    {
        try
        {
            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            return BadRequest(new
            {
                msg = "Falha ao criar!",
                status = HttpStatusCode.BadRequest
            });
        }

        return Created("uri", (new
        {
            msg = "Criado com sucesso!",
            status = HttpStatusCode.Created
        }));
    }

    [HttpPut("{id}")]
    public ActionResult<Object> Update([FromRoute] int id, [FromBody] Person pessoa)
    {
        var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);

        if (result is null)
        {
            return NotFound(new
            {
                msg = "Registro não encontrado!",
                status = HttpStatusCode.NotFound
            });
        }

        try
        {
            _context.Pessoas.Update(pessoa);
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            return BadRequest(new
            {
                msg = $"Erro de solicitação de atualização do id {id}!",
                status = HttpStatusCode.BadRequest
            });
        }

        return new
        {
            msg = $"Dados do id {id} atualizados.",
            status = HttpStatusCode.OK
        };
    }

    [HttpDelete("{id}")]
    public ActionResult<Object> Delete([FromRoute] int id)
    {
        var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);

        if (result is null)
        {
            return BadRequest(new
            {
                msg = "Conteúdo inexistente, solicitação inválida!",
                status = HttpStatusCode.BadRequest
            });
        }

        _context.Pessoas.Remove(result);
        _context.SaveChanges();

        return Ok(new
        {
            msg = $"Id de número {id} foi deletado com sucesso!",
            status = HttpStatusCode.OK
        });
    }
}