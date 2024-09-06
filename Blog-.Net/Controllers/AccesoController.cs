using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Blog_.Net.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services.Description;

namespace Blog_.Net.Controllers
{
    public class AccesoController : Controller
    {

        static string cadena = "Data Source=(localdb)\\server;Initial Catalog=DB_Blog;User id=sa;Password=Rambo#12345";

        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registered()
        {
            return View();
        }

        // El HttpPost del Registro
        [HttpPost]
        public ActionResult Registered(InfoUser oInfoUser)
        {
            bool registered;
            string message;

            if (oInfoUser.Passcode == oInfoUser.ConfirmPasscode)
            {
                oInfoUser.Passcode = TransformSha256(oInfoUser.Passcode);

            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_RegisterUser", cn);
                cmd.Parameters.AddWithValue("Email", oInfoUser.Email);
                cmd.Parameters.AddWithValue("Passcode", oInfoUser.Passcode);
                cmd.Parameters.Add("Registered",SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Message", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registered = Convert.ToBoolean(cmd.Parameters["Registered"].Value);
                message = cmd.Parameters["Message"].Value.ToString();
            }

            ViewData["Message"] = message;

            if (registered)
            {
                return RedirectToAction("Login","Acceso");
            }
            else
            {
                return View();
            }

        }

        // El HttpPost del Login
        [HttpPost]
        public ActionResult Login(InfoUser oInfoUser)
        {
            oInfoUser.Passcode = TransformSha256(oInfoUser.Passcode);

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_VerifyUser", cn);
                cmd.Parameters.AddWithValue("Email", oInfoUser.Email);
                cmd.Parameters.AddWithValue("Passcode", oInfoUser.Passcode);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                oInfoUser.IdUser = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }

            if (oInfoUser.IdUser != 0)
            {
                Session["infouser"] = oInfoUser;
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["Message"] = "Usuario no encontrado";
                return View();
            }
        }

        public static string TransformSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}