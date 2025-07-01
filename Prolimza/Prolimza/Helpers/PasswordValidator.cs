using System.Text.RegularExpressions;

namespace Prolimza.Helpers
{

    public static class PasswordValidator
    {
        public static bool EsSegura(string contrasena, out string mensajeError)
        {
            mensajeError = "";

            if (contrasena.Length < 12)
            {
                mensajeError = "La contraseña debe tener al menos 12 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(contrasena, "[A-Z]"))
            {
                mensajeError = "La contraseña debe contener al menos una letra mayúscula.";
                return false;
            }

            if (!Regex.IsMatch(contrasena, "[a-z]"))
            {
                mensajeError = "La contraseña debe contener al menos una letra minúscula.";
                return false;
            }

            if (!Regex.IsMatch(contrasena, "[0-9]"))
            {
                mensajeError = "La contraseña debe contener al menos un número.";
                return false;
            }

            if (!Regex.IsMatch(contrasena, "[^a-zA-Z0-9]"))
            {
                mensajeError = "La contraseña debe contener al menos un carácter especial.";
                return false;
            }

            // Aquí podrías agregar una lista de contraseñas prohibidas comunes

            return true;
        }
    }

}
