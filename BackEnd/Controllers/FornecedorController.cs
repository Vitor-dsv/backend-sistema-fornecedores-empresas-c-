using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly FornecedorService _fornecedorService;

        public FornecedorController(AppDbContext context)
        {
            this._context = context;
            _fornecedorService = new FornecedorService();
        }

        // GET: api/Fornecedor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedores()
        {
            try
            {
                return await _context.Fornecedor
                    .Include(f => f.Empresa)
                    .ToListAsync();
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Falha ao listar os Fornecedores. Erro: {0}", erro.Message));
            }
        }

        // GET: api/Fornecedor/filtro
        [HttpGet("filtro/{nome}/{cpfCnpj}/{dataInicio}/{dataFim}")]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedoresFiltro(string nome = null, string cpfCnpj = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                var fornecedores = from f in _context.Fornecedor select f;

                if (!string.IsNullOrEmpty(nome))
                {
                    fornecedores = fornecedores.Where(f => f.Nome.Contains(nome));
                }

                if (!string.IsNullOrEmpty(cpfCnpj))
                {
                    fornecedores = fornecedores.Where(f => f.CpfOuCnpj == cpfCnpj);
                }

                if (DateTime.MinValue != dataInicio.Value)
                {
                    fornecedores = fornecedores.Where(f => f.DataEHoraDoCadastro >= dataInicio.Value);
                }

                if (DateTime.MinValue != dataFim.Value)
                {
                    fornecedores = fornecedores.Where(f => f.DataEHoraDoCadastro <= dataFim.Value);
                }

                return await fornecedores
                    .Include(f => f.Empresa)
                    .ToListAsync();
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Falha ao listar os Fornecedores filtrados. Erro: {0}", erro.Message));
            }
        }

        // GET: api/Fornecedor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> GetFornecedor(long id)
        {
            try
            {
                var fornecedor = await _context.Fornecedor.FindAsync(id);
                fornecedor.Empresa = await _context.Empresa.FindAsync(fornecedor.EmpresaFK);

                if (fornecedor == null)
                {
                    throw new Exception("Fornecedor inexistente no sistema.");
                }

                return fornecedor;
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Falha ao listar Fornecedor. Erro: {0}", erro.Message));
            }
        }

        // PUT: api/Fornecedor/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutFornecedorAsync(long id, Fornecedor fornecedor)
        {
            try
            {
                if (id != fornecedor.Id)
                {
                    throw new Exception("Os parâmetros de Id's não coincidiram.");
                }

                Empresa empresa = await _context.Empresa.FindAsync(fornecedor.EmpresaFK);

                if (empresa == null)
                {
                    throw new Exception("Empresa inexistente no sistema.");
                }

                _fornecedorService.ValidarFornecedor(fornecedor, empresa);

                _context.Entry(fornecedor).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception erro)
            {
                if (!FornecedorExiste(id))
                {
                    throw new Exception("Falha ao atualizar o cadastro do Fornecedor. Erro: Parâmetro ID não existente.");
                }

                throw new Exception(string.Format("Falha ao atualizar o cadastro do Fornecedor. Erro: \r\n{0}", erro.Message));
            }
        }

        // POST: api/Fornecedor
        [HttpPost]
        public async Task<ActionResult<Fornecedor>> PostFornecedorAsync(Fornecedor fornecedor)
        {
            try
            {
                Empresa empresa = await _context.Empresa.FindAsync(fornecedor.EmpresaFK);

                if (empresa == null)
                {
                    throw new Exception("Empresa inexistente no sistema.");
                }

                _fornecedorService.ValidarFornecedor(fornecedor, empresa);

                _context.Fornecedor.Add(fornecedor);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFornecedor", new { id = fornecedor.Id }, fornecedor);
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Falha ao cadastrar o Fornecedor. Erro: \r\n{0}", erro.Message));
            }
        }

        // DELETE: api/Fornecedor/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Fornecedor>> DeleteFornecedor(long id)
        {
            try
            {
                var fornecedor = await _context.Fornecedor.FindAsync(id);
                if (fornecedor == null)
                {
                    throw new Exception("Fornecedor inexistente no sistema.");
                }

                _context.Fornecedor.Remove(fornecedor);
                await _context.SaveChangesAsync();

                return fornecedor;
            }
            catch (Exception erro)
            {
                throw new Exception(string.Format("Falha ao excluir o Fornecedor. Erro: {0}", erro.Message));
            }
        }

        private bool FornecedorExiste(long id)
        {
            return _context.Fornecedor.Any(e => e.Id == id);
        }
    }
}
