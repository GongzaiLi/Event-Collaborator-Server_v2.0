using System;
using System.Threading.Tasks;
using event_client_app.Services;
using event_client_app.View;
using Microsoft.AspNetCore.Mvc;
using event_client_app.Models;
using event_client_app.Services.IRepository;
using event_client_app.Services.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;

namespace event_client_app.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private HashAndVerify _hashAndVerify;
        private IUserRepository _userRepository;

        public UserController(DBAPPContext dbContext)
        {
            _hashAndVerify = new HashAndVerify();
            _userRepository = new UserRepository(dbContext);
        }


        [HttpPost("users/register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(PostUserRegisterObject userRegisterObject)
        {
            string hashPassword;
            using (_hashAndVerify)
            {
                hashPassword = _hashAndVerify.HashCode(userRegisterObject.password);
            }

            User newUser = new User
            {
                FirstName = userRegisterObject.firstName,
                LastName = userRegisterObject.lastName,
                password = hashPassword,
                Email = userRegisterObject.email
            };

            using (_userRepository)
            {
                User user = await _userRepository.FindUserByEmail(newUser.Email);
                if (user != null)
                {
                    return BadRequest("Email is already exist!");
                }

                _userRepository.InsertUser(newUser);
                _userRepository.Save();
            }

            return Created(new Uri("users/register", UriKind.Relative), new {userId = newUser.UserId});
        }

        [HttpPost("users/login")]
        public async Task<IActionResult> Login(PostUserLoginObject postUserLoginObject)
        {
            using (_userRepository)
            {
                User user = await _userRepository.FindUserByEmail(postUserLoginObject.email);
                if (user == null)
                {
                    return BadRequest("Email is not exist!");
                }

                using (_hashAndVerify)
                {
                    if (!_hashAndVerify.VerifyHashedCode(postUserLoginObject.password, user.password))
                    {
                        return BadRequest("Password error!");
                    }

                    user.AuthToken = _hashAndVerify.HashCode(user.Email);
                }

                _userRepository.Save();
                return Ok(new {userId = user.UserId, token = user.AuthToken});
            }

            // Request.Headers.TryGetValue("X-Authorization", out var token);
        }

        [HttpPost("users/logout")]
        public async Task<IActionResult> Logout()
        {
            using (_userRepository)
            {
                Request.Headers.TryGetValue("X-Authorization", out var token);
                User user = await _userRepository.FindUserByToken(token);
                if (user == null)
                {
                    return Unauthorized();
                }

                _userRepository.DeleteToken(user);
                _userRepository.Save();
                return Ok("Ok");
            }
        }

        [HttpGet("users/{id}")]
        public IActionResult GetUser(int id)
        {
            using (_userRepository)
            {
                User user = _userRepository.FindUserById(id);
                if (user == null)
                {
                    return NotFound();
                }

                Request.Headers.TryGetValue("X-Authorization", out var token);
                if (token.Equals(user.AuthToken) && token.Count > 0)
                {
                    return Ok(new {firstName = user.FirstName, lastName = user.LastName, email = user.Email});
                }

                return Ok(new {firstName = user.FirstName, lastName = user.LastName});
            }
        }

        [HttpPatch("users/{id}")]
        public async Task<IActionResult> EditUser(int id, PatchUserObject patchUserObject)
        {
            using (_userRepository)
            {
                Request.Headers.TryGetValue("X-Authorization", out var token);
                User user = await _userRepository.FindUserByToken(token);
                if (user == null || user.UserId != id)
                {
                    return Unauthorized();
                }

                if (!_userRepository.CheckEmailNotUseInOtherUsers(id, patchUserObject.email))
                {
                    return BadRequest("Email is already exist!");
                }

                user.Email = user.Email != patchUserObject.email ? patchUserObject.email : user.Email;
                user.FirstName = patchUserObject.FirstName;
                user.LastName = patchUserObject.LastName;

                if (patchUserObject.password == null ^ patchUserObject.currentPassword == null)
                {
                    return BadRequest("Password or Current Password can not be null!");
                }

                if (patchUserObject.password != null && patchUserObject.currentPassword != null)
                {
                    if (patchUserObject.password.Length == 0 || patchUserObject.currentPassword.Length == 0)
                    {
                        return BadRequest("Password or Current Password Length should be more then one!");
                    }

                    using (_hashAndVerify)
                    {
                        if (!_hashAndVerify.VerifyHashedCode(patchUserObject.currentPassword, user.password))
                        {
                            return BadRequest("Password error!");
                        }

                        user.password = _hashAndVerify.HashCode(patchUserObject.password);
                    }
                }

                _userRepository.Save();
                return Ok();
            }
        }


        // todo user image
    }
}