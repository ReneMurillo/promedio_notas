using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PromNotas.Context;
using PromNotas.Models;

namespace PromNotas.Controllers
{
    public class NotasController : Controller
    {
        //Instancia del contexto para tener acceso a la tabla
        NotasContext db = new NotasContext();

        //Acción GET que mostrará el listado de todas las notas
        public ActionResult Index()
        {
            return View(db.Notas.ToList());
        }

        //Acción POST para aplicar los filtros en el listado
        [HttpPost]
        public ActionResult Index(string filtro, string valor)
        {
            //Se listan todas las notas
            var nota = (from n in db.Notas
                        select n).ToList();
            //Se crea una variable y se inicializa en 0
            decimal val = 0;
            //Verificamos si estamos recibiendo el valor en el formulario de búsqueda
            if (valor != "")
            {
                /*Si estamos recibiendo valor lo convertimos a decimal y se lo asignamos a la variable 
                Creada anteriormente */
                val = Decimal.Parse(valor);

                //Hacemos la búsqueda 
                nota = (from n in db.Notas
                       where n.Promedio.Equals(val)
                       select n).ToList();
            }
            
            //Evaluamos si recibimos una opción en el combobox del formulario
             
            if(filtro == "Aprobado")
            {
                //Si la opción es igual a Aprobado, entonces aplicamos ese filtro 
               nota = (from n in db.Notas
                       where n.Estado == "Aprobado"
                       select n).ToList();
            }
            else if(filtro == "Reprobado")
            {
                //Si la opción es igual a Reprobado, entonces aplicamos ese filtro 
                nota = (from n in db.Notas
                        where n.Estado == "Reprobado"
                        select n).ToList();
            }

            //Retornamos a la vista con los filtros aplicados
            return View(nota);
        }

        //Acción GET para mostrar la interfaz donde se ingresarán las notas
        public ActionResult Create()
        {
            return View();
        }

        //Acción POST para recibir las notas ingresadas y almacenarlas en la base de datos
        [HttpPost]
        public ActionResult Create(Nota nota)
        {
            //Condición que verifica si el modelo es válido y que no se ingresen notas mayores a 10
            if (ModelState.IsValid && (nota.Nota1 <= 10 && nota.Nota2 <=10 && nota.Nota3 <=10))
            {
                //Se hace el cálculo del promedio
                decimal promedio = (nota.Nota1 + nota.Nota2 + nota.Nota3) / 3;

                //Se asigna el promedio a la propiedad correspondiente
                nota.Promedio = Math.Round(promedio, 2);

                //Evaluamos el valor del promedio para asignar un estado
                if (promedio >= Convert.ToDecimal(6.0))
                {
                    //Si el promedio es mayor o igual a 6.0 entonces es aprobado
                    nota.Estado = "Aprobado";
                }
                else
                {
                    //Si no, es reprobado
                    nota.Estado = "Reprobado";
                }

                try
                {
                    //Intentamos guardar la nota
                    db.Notas.Add(nota);
                    db.SaveChanges();
                    //Si todo sale bien, redireccionamos a la acción Index
                    return RedirectToAction("Index");
                }
                catch
                {
                    //Si algo sale mal retornamos a la misma vista con los datos que ya había ingresado
                    return View(nota);
                }
            }
            /*Si el modelo no es válido y alguna de las notas es mayor a 10, 
            retornamos a la misma vista con lo que había ingresado*/
            return View(nota);

        }

        //Acción GET para recibbir el ID de la nota y buscarlo en la base de datos
        public ActionResult Edit(int? id)
        {
            //Si el id nos llega nulo, entonces retornamos una vista de BadRequest
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Hacemos la búsqueda de la nota
            Nota nota = db.Notas.Find(id);

            //Si es nulo, retornamos una vista de registro no encontrado
            if(nota == null)
            {
                return HttpNotFound();
            }

            //Retornamos a la vista con el modelo encontrado
            return View(nota);
        }

        //Acción POST para recibir las modificaciones en las notas y aplicar los cambios en la BD
        [HttpPost]
        public ActionResult Edit(Nota nota)
        {
            if (ModelState.IsValid && (nota.Nota1 <= 10 && nota.Nota2 <= 10 && nota.Nota3 <= 10))
            {

                decimal promedio = (nota.Nota1 + nota.Nota2 + nota.Nota3) / 3;
                nota.Promedio = Math.Round(promedio, 2);

                if (promedio >= Convert.ToDecimal(6.0))
                {
                    nota.Estado = "Aprobado";
                }
                else
                {
                    nota.Estado = "Reprobado";
                }
                try
                {
                    //Línea de código que realiza la modificación
                    db.Entry(nota).State = EntityState.Modified;
                    //Guardamos los cambios
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(nota);
                }
            }
            return View(nota);
        }

        // ACiión GET para recibir el ID de la nota que se desea eliminar
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Notas.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }

            return View(nota);
        }

        //Acción POST para aplicar la eliminación en la BD
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Hacemos la búsqueda de la nota
            Nota nota = db.Notas.Find(id);
            try
            {
                //La eliminamos de la tabla
                db.Notas.Remove(nota);
                //Guardamos los cambios
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Delete", nota);
            }
        }
    }
}
