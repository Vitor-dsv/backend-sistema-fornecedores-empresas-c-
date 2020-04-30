using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.Services;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmpresaService _empresaService;

        public EmpresaController(AppDbContext context)
        {
            this._context = context;
            _empresaService = new EmpresaService();
        }

        // GET: api/Empresa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            try
            {
                return await _context.Empresa.ToListAsync();
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Erro ao listar as Empresas. Erro: {0}", erro.Message));
            }
        }

        // GET: api/Empresa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(long id)
        {
            try
            {
                var empresa = await _context.Empresa.FindAsync(id);

                if (empresa == null)
                {
                    throw new Exception("Empresa inexistente no sistema.");
                }

                return empresa;
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Erro ao listar Empresa. Erro: {0}", erro.Message));
            }
        }

        // PUT: api/Empresa/5
        [HttpPut("{id}")]
        public IActionResult PutEmpresa(long id, Empresa empresa)
        {
            try
            {
                if (id != empresa.Id)
                {
                    throw new Exception("Os parâmetros de Id's não coincidiram.");
                }

                _empresaService.ValidarEmpresa(empresa);

                _context.Entry(empresa).State = EntityState.Modified;
                _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception erro)
            {
                if (!EmpresaExiste(id))
                {
                    throw new Exception("Falha ao atualizar o cadastro da Empresa. Erro: Parâmetro ID não existente.");
                }

                throw new Exception(string.Format("Falha ao atualizar o cadastro da Empresa. Erro: \r\n{0}", erro.Message));
            }
        }

        // POST: api/Empresa
        [HttpPost]
        public ActionResult<Empresa> PostEmpresa(Empresa empresa)
        {
            try
            {
                _empresaService.ValidarEmpresa(empresa);

                _context.Empresa.Add(empresa);
                _context.SaveChangesAsync();

                return CreatedAtAction("GetEmpresa", new { id = empresa.Id }, empresa);
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Falha ao atualizar o cadastro da Empresa. Erro: \r\n{0}", erro.Message));
            }
        }

        // DELETE: api/Empresa/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Empresa>> DeleteEmpresa(long id)
        {
            try
            {
                var empresa = await _context.Empresa.FindAsync(id);
                if (empresa == null)
                {
                    throw new Exception("Empresa inexistente no sistema.");
                }

                _context.Empresa.Remove(empresa);
                await _context.SaveChangesAsync();

                return empresa;
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Falha ao excluir Empresa. Erro: {0}", erro.Message));
            }
        }

        private bool EmpresaExiste(long id)
        {
            return _context.Empresa.Any(e => e.Id == id);
        }
    }
}
