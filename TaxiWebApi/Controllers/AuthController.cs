﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaxiWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(SignInDto dto)
        {
            var token = await _userService.Authenticate(dto.Username, dto.Password);

            if (token == null)
                return BadRequest("Wrong username or password");

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try
            {
                var user = await _userService.CreateUser(dto);

                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
