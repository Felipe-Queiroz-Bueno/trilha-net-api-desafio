using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext context;

        public TarefaController(OrganizadorContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {

            // DONE: Buscar o Id no banco utilizando o EF
            // DONE: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada
            var tarefa = this.context.Tarefas.Where(t => t.Id == id);

            if (tarefa is null)
            {
                return BadRequest("Tarefa não encontrada");
            }

            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // DONE: Buscar todas as tarefas no banco utilizando o EF
            return Ok(this.context.Tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // DONE: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefa = this.context.Tarefas.Where(t => t.Titulo == titulo);

            if (tarefa is null)
            {
                return BadRequest("Tarefa não encontrada");
            }

            return Ok(tarefa);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = this.context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // DONE: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefa = this.context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            // DONE: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            this.context.Add(tarefa);
            this.context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = context.Tarefas.Find(id);

            if (tarefaBanco == null)
            {
                return NotFound();
            }
                
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            // DONE: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // DONE: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            this.context.SaveChanges();
            
            return Ok(tarefa);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = this.context.Tarefas.Find(id);

            if (tarefaBanco == null)
            {
                return NotFound();
            }

            // DONE: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            this.context.SaveChanges();
            return NoContent();
        }
    }
}
