using CRUD_ALUMNOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD_ALUMNOS.Controllers
{
    public class AlumnoController : Controller
    {
        // GET: Alumno
        public ActionResult Index()
        {
            try
            {
                string sql = @"
                          select a.Id, a.Nombres, a.Apellidos, a.Edad, a.Sexo, a.FechaRegistro, c.Nombre as NombreCiudad
                        from Alumno a
                      join Ciudad c
                        on a.CodCiudad=c.Id";

                using (var db = new AlumnosContext())
                {
                //    var data = from a in db.Alumno
                //               join c in db.Ciudad on a.CodCiudad equals c.Id
                //               select new AlumnoCE()
                //               {
                //                   Id = a.Id,
                //                   Nombres = a.Nombres,
                //                   Apellidos = a.Apellidos,
                //                   Edad = a.Edad,
                //                   Sexo = a.Sexo,
                //                   NombreCiudad = c.Nombre,
                //                   FechaRegistro = a.FechaRegistro
                //               };
                //List<Alumno> lista = db.Alumno.Where(a => a.Edad > 18).ToList();
                //return View(data.ToList());

                    return View(db.Database.SqlQuery<AlumnoCE>(sql).ToList());
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public ActionResult Agregar()
        {
            return View();    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Alumno a)
        {   
            if(!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new AlumnosContext())
                {   
                    a.FechaRegistro = DateTime.Now;

                    db.Alumno.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Error al registrar al Alumno - " + ex.Message);
                return View();
            }
                
        }

        public ActionResult Editar(int id)
        {
            try
            {
                using (var db = new AlumnosContext())
                {
                    //Alumno al = db.Alumno.Where(a => a.Id == id).FirstOrDefault(); //Tablas con llave compuesta
                    Alumno alu = db.Alumno.Find(id); //--> Se usa cuando solo haya una llave primaria
                    return View(alu);
                }
            }
            catch (Exception ex)
            {

                throw;
            }   
        }

        public ActionResult Agregar2()
        {
            return View();
        }

        public ActionResult ListaCiudades()
        {
            using(var db = new AlumnosContext())
            {
                return PartialView(db.Ciudad.ToList());
            }   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Alumno a)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AlumnosContext())
                {
                    Alumno al = db.Alumno.Find(a.Id);
                    al.Nombres = a.Nombres;
                    al.Apellidos = a.Apellidos;
                    al.Edad = a.Edad;
                    al.Sexo = a.Sexo;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Detallesalumno(int id)
        {
            using (var db = new AlumnosContext())
            {
                Alumno alu = db.Alumno.Find(id); //--> Se usa cuando solo haya una llave primaria
                return View(alu);
            }
        }

        public ActionResult EliminarAlumno(int id)
        {
            using (var db = new AlumnosContext())
            {
                Alumno alu = db.Alumno.Find(id); //--> Se usa cuando solo haya una llave primaria
                db.Alumno.Remove(alu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public static string NombreCiudad(int CodCiudad)
        {
            using(var db = new AlumnosContext()) 
            { 
                return db.Ciudad.Find(CodCiudad).Nombre;
            }
        }


    }



}