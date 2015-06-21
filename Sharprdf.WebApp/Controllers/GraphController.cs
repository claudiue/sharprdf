using Sharprdf.Core;
using Sharprdf.WebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sharprdf.WebApp.Controllers
{
    public class GraphController : Controller
    {
        private Engine _engine;

        public GraphController()
        {

        }

        public ActionResult Index()
        {
            var model = new SourceCodeFileViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SourceCodeFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //return View(model);
            }

            byte[] uploadedFile = new byte[model.File.InputStream.Length];
            //model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

            StreamReader streamReader = new StreamReader(model.File.InputStream);
            var sourceCode = streamReader.ReadToEnd();

            // now you could pass the byte array to your model and store wherever 
            // you intended to store it
            var ontology = new Ontology(name: "C# Ontology", prefix: "cscro", fileName: Server.MapPath(@"~/App_Data/cscro.v2.owl"));
            _engine = new Engine(ontology);

            var filedata = _engine.CreateRdf(model.File.FileName, sourceCode);
            var contentType = "application/rdf+xml";

            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = model.File.FileName + ".rdf",
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
    }
}