using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.DTOs.Usuarios;
using ProyectoElectivaV.Excepciones;

using ProyectoElectivaV.Model.DbConfiguration;
using ProyectoElectivaV.Model.Entities;
using ProyectoElectivaV.Service.Email;
using System.Text.RegularExpressions;

namespace ProyectoElectivaV.Service
{
    public class UsuarioService
    {
        private readonly IMapper _mapeos;
        private readonly IMongoCollection<Usuario> _usuarioCollection;
        private readonly ICorreoService _correoService;
        private readonly UtilidadServicio _utilidadServicio;

        public UsuarioService(IMapper mapeos,
                            IProyectoElectivaVDBSettings proyectoElectivaVDBSettings,
                            ICorreoService correoService,
                            UtilidadServicio utilidadServicio)
        {
            _mapeos = mapeos;
            var cliente = new MongoClient(proyectoElectivaVDBSettings.ConnectionString);
            var database = cliente.GetDatabase(proyectoElectivaVDBSettings.DatabaseName);
            _usuarioCollection = database.GetCollection<Usuario>(proyectoElectivaVDBSettings.UsuariosCollectionname);
            _correoService = correoService;
            _utilidadServicio = utilidadServicio;
        }

        public async Task<List<UsuarioDTO>> ObtenerTodosLosUsuarios()
        {
            try
            {
                List<Usuario> usuarios = await _usuarioCollection.Find(s => true).ToListAsync();

                if (usuarios is null)
                {
                    throw new CustomeExceptions("No hay series disponibles", 203);
                }

                var result = _mapeos.Map<List<Usuario>, List<UsuarioDTO>>(usuarios);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UsuarioDTO> RegistrarUsuario(CrearUsuarioDTO dto)
        {
            try
            {
                Usuario user = await _usuarioCollection.Find(u => u.email == dto.email).FirstOrDefaultAsync();

                if (user is not null)
                {
                    throw new CustomeExceptions("El correo suministrado ya está asociado a una cuenta", 203);
                }

                var filtroVariacion = Builders<Usuario>.Filter.Regex(
                    u => u.email,
                    new BsonRegularExpression($"^{Regex.Escape(dto.email)}$", "i")
                );

                Usuario userVariacion = await _usuarioCollection.Find(filtroVariacion).FirstOrDefaultAsync();
                if (userVariacion is not null)
                {
                    throw new CustomeExceptions("Ya existe una cuenta con un correo similar (sin distinguir mayúsculas o minúsculas)", 203);
                }

                if (dto.contraseña != dto.confirmarContraseña)
                {
                    throw new CustomeExceptions("Las contraseñas no coinciden", 203);
                }

                Usuario usuario = _mapeos.Map<CrearUsuarioDTO, Usuario>(dto);
                usuario.contraseña = _utilidadServicio.ConvertirSHA246(dto.contraseña);
                usuario.restablecer = false;
                usuario.confirmado = false;
                await _usuarioCollection.InsertOneAsync(usuario);

                var filtro = Builders<Usuario>.Filter.Eq(u => u.id, usuario.id);
                var actualizacion = Builders<Usuario>.Update.Set(u => u.token, usuario.token);
                await _usuarioCollection.UpdateOneAsync(filtro, actualizacion);

                var correoVerificacion = new CorreoDTO
                {
                    Para = dto.email,
                    Asunto = "Verificación de Cuenta",
                    Contenido = $"<p>Por favor, verifica tu cuenta haciendo clic en el siguiente enlace:</p>" +
                                $"<p><a href='https://localhost:7198/usuarioController/ConfirmarCorreo?token={usuario.token}'>Verificar Cuenta</a></p>"
                };

                _correoService.EnviarEmail(correoVerificacion);

                return _mapeos.Map<UsuarioDTO>(usuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> ObtenerUsuario(string email)
        {
            try
            {
                Usuario exist = await _usuarioCollection.Find(s => s.email == email).FirstOrDefaultAsync();

                if (exist is null)
                {
                    throw new CustomeExceptions("El usuario solicitado no existe", 203);
                }

                var usuario = _mapeos.Map<Usuario, UsuarioDTO>(exist);
                usuario.contraseña = _utilidadServicio.ConvertirSHA246(usuario.contraseña);
                usuario.token = _utilidadServicio.GenerarToken(exist);

                return usuario;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UsuarioDTO> RestablecerContraseña(RestablecerContraseñaDTO dto)
        {
            try
            {
                Usuario exist = await _usuarioCollection.Find(s => s.token == dto.token).FirstOrDefaultAsync();

                if (exist is null)
                {
                    throw new CustomeExceptions("El token es inválido o ha expirado", 403);
                }

                if (dto.contraseña != dto.confirmarContraseña)
                {
                    throw new CustomeExceptions("Las contraseñas no coinciden", 400);
                }

                exist.restablecer = true;
                exist.contraseña = _utilidadServicio.ConvertirSHA246(dto.contraseña);
                exist.token = _utilidadServicio.GenerarToken(exist);
                await _usuarioCollection.ReplaceOneAsync(s => s.token == dto.token, exist);

                return _mapeos.Map<UsuarioDTO>(exist);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> LoginUsuario(LoginDTO dto)
        {
            try
            {
                Usuario exist = await _usuarioCollection.Find(s => s.email == dto.email &&
                                                                   s.contraseña == _utilidadServicio.ConvertirSHA246(dto.contraseña)).FirstOrDefaultAsync();
                
                if (exist is null)
                {
                    throw new CustomeExceptions("Estas credenciales, no están asociadas a una cuenta", 203);
                }
                //else if (exist.confirmado == false)
                //{
                //    throw new CustomeExceptions("No ha confirmado su cuenta", 203);
                //}
                else if (exist.email != dto.email && exist.contraseña == _utilidadServicio.ConvertirSHA246(dto.contraseña))
                {
                    throw new CustomeExceptions("Correo o contraseña incorrectas", 203);
                }
                else if (exist.email == dto.email && exist.contraseña != _utilidadServicio.ConvertirSHA246(dto.contraseña))
                {
                    throw new CustomeExceptions("Correo o contraseña incorrectas", 203);
                }

                var usuario = _mapeos.Map<Usuario, UsuarioDTO>(exist);
                usuario.token = _utilidadServicio.GenerarToken(exist);

                return usuario;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UsuarioDTO> ConfirmarCorreo(string token)
        {
            try
            {
                try
                {
                    Usuario exist = await _usuarioCollection.Find(s => s.token == token).FirstOrDefaultAsync();

                    if (exist is null)
                    {
                        throw new CustomeExceptions("El usuario solicitado no existe", 203);
                    }

                    exist.confirmado = true;
                    await _usuarioCollection.ReplaceOneAsync(s => s.token == token, exist);

                    return _mapeos.Map<UsuarioDTO>(exist);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
