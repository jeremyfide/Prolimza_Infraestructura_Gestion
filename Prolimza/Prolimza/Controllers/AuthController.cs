﻿using Microsoft.AspNetCore.Mvc;
using Prolimza.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Prolimza.Helpers;
using Microsoft.EntityFrameworkCore;


namespace Prolimza.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contrasena)
        {
            var usuario = _context.Usuarios.Include(u => u.Rol).FirstOrDefault(u => u.Correo == correo);

            if (usuario == null || !PasswordHelper.VerifyPassword(contrasena, usuario.contrasenaEncriptada))
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre),
                new Claim("IdUsuario", usuario.IdUsuario.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Registro(string emailSignIn, string username, string loginPassword)
		{
			if (await _context.Usuarios.AnyAsync(u => u.Correo == emailSignIn))
			{
				ViewBag.Error = "El correo ya está registrado.";
				return View();
			}

			Usuario usuario = new Usuario
			{
				Correo = emailSignIn,
				Nombre = username,
				contrasenaEncriptada = PasswordHelper.HashPassword(loginPassword),
				IdRol = 4 // rol por defecto
			};

			_context.Usuarios.Add(usuario);
			await _context.SaveChangesAsync();

			return RedirectToAction("Login");
		}

	}
}
