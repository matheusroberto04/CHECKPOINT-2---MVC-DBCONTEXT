using CHECKPOINT_2.DatContext;
using CHECKPOINT_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CHECKPOINT_2.Controllers

{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        //CREATE para salvar um cadastro
        public IActionResult Create(UserViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                   
                };
                DbContext.User.Add(newUser);
                DbContext.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar o usuário: {ex.Message}");
            }
        }

        public IActionResult Register([FromBody] User request)
        {
            try
            {
                var user = _dataContext.User.FirstOrDefault(x => x.UserEmail == request.UserEmail);
                if (user != null)
                {
                    return BadRequest("Usuário já existe");
                }

                User newUser = new User
                {
                    Id = request.Id,
                    UserEmail = request.UserEmail,
                    UserName = request.UserName,
                    UserPassword = request.UserPassword,
                    UserPhone = request.UserPhone,
                };

                _dataContext.User.Add(newUser);
                _dataContext.SaveChanges();
                return Ok("Usuário cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao cadastrar usuário: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Login([FromBody] User request)
        {
            try
            {
                var user = _dataContext.User.FirstOrDefault(x => x.UserEmail == request.UserEmail);
                if (user == null)
                {
                    return BadRequest("Usuário não encontrado");
                }

                if (user.UserPassword != request.UserPassword)
                {
                    return BadRequest("Senha incorreta");
                }

                return Ok("Login bem-sucedido");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao realizar login: " + ex.Message);
            }
        }
    }
}
