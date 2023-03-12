using AppRifasDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppRifasDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        readonly AppRifaDBContext db = new AppRifaDBContext();

        //Obtener listado de usuarios
        [HttpGet]
        public IEnumerable<Object> Get()
        {
            var listaUsuarios=(from usuarios in db.Usuarios
                                        select new
                                        {
                                            usuarios.NombreUsuario,
                                            usuarios.Correo
                                        }).ToList();

            return listaUsuarios;
        }

        //Obtener un unico usuario
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(string id)
        {
            var usuario = (from usarios in db.Usuarios
                           where usarios.NombreUsuario.Equals(id)
                           select new
                           {
                               usarios.NombreUsuario,
                               usarios.Correo
                           }).FirstOrDefault();

            if (usuario == null)
                return NotFound();
            else
                return Ok(usuario);
        }

        //crear nuevo usuario
        [HttpPost]
        public IActionResult Post(Usuario usuario)
        {

            if (usuario.Correo.Equals(""))
                return BadRequest("El correo es requerido");


            try
            {

                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return Ok("Guardado");
            }
            catch (Exception e)
            {
                return BadRequest(e);
                throw;
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(string id, Usuario usuario)
        {
            //? convierte a una variable en nullable (que puede recibir valores nulos)
            Usuario? usuarioDB = (from usuarios in db.Usuarios
                                 where usuarios.NombreUsuario.Equals(id)
                                 select usuarios).FirstOrDefault();

            if (usuarioDB == null)
                return NotFound();
            else
            {                
                usuarioDB.Password = usuario.Password;
                usuarioDB.Correo = usuario.Correo;

                db.SaveChanges();
                return Ok(usuarioDB);
            }
        }

        //Eliminando Usuario
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            Usuario usuario = new Usuario() { NombreUsuario = id };

            db.Usuarios.Remove(usuario);

            db.SaveChanges();

            return Ok();
        }
    }
}
