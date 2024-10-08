﻿using JavaHateBE.exceptions;
using JavaHateBE.model;
using JavaHateBE.model.DTOs;
using JavaHateBE.service;
using Microsoft.AspNetCore.Mvc;

namespace JavaHateBE.controller
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly UserService _userService;

        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<User>> GetUserById([FromRoute] Guid id)
        {
            try
            {
                User user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername([FromRoute] string username)
        {
            try
            {
                User user = await _userService.GetUserByUsername(username);
                return Ok(user);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail([FromRoute] string email)
        {
            try
            {
                User user = await _userService.GetUserByEmail(email);
                return Ok(user);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateInput user)
        {
            try
            {
                User newUser = await _userService.CreateUser(user);
                return Ok(newUser);
            }
            catch (IllegalArgumentException e)
            {
                _logger.LogWarning(e, "Failed to add question.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message }, { "field", e.Argument } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser([FromBody] User user)
        {
            try
            {
                User updatedUser = await _userService.UpdateUser(user);
                return Ok(updatedUser);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (IllegalArgumentException e)
            {
                _logger.LogWarning(e, "Failed to add question.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message }, { "field", e.Argument } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpDelete("id/{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] UserLogin credentials)
        {
            try
            {
                User user = await _userService.Login(credentials.Password, credentials.Username);
                return Ok(user);
            }
            catch (ObjectNotFoundException e)
            {
                _logger.LogWarning(e, "Failed to get all questions.");
                return NotFound(new Dictionary<string, string> { { "message", e.Message }, { "entity", e.Object } });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all questions.");
                return BadRequest(new Dictionary<string, string> { { "message", e.Message } });
            }
        }
    }

    
}
