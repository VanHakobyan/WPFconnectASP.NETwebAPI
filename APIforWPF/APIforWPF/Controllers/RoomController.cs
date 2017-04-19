using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIforWPF.Models;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace APIforWPF.Controllers
{
    public class RoomController : ApiController
    {

        //List<People> peoples = new List<People>
        //{
        //    new People { Name="Gayane",Age=125,Heigth=12},
        //new People{Name="Lusine",Age=215,Heigth=13}
        //};
        readonly string pathCustom = @"C:\TestDirectory";
        // GET: api/Room
        public IEnumerable<string> GetAllFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(pathCustom);
            var files = directory.GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).Select(f => f.Name).ToArray();
            return files;
        }


        //Bad prictice

        //public HttpResponseMessage Get(string name)
        //{
        //    string path = Path.Combine(pathCustom, name);
        //    StreamReader sr = File.OpenText(path);
        //    string textline = sr.ReadLine();
        //    sr.Close();
        //    return textline;
        //    HttpResponseMessage res = new HttpResponseMessage();
        //    res.Content = new StringContent(textline);
        //    return res;

        //}

        // GET: api/Room/5
        public string Get(string name)
        {
            string path = Path.Combine(pathCustom, name);
            StreamReader sr = File.OpenText(path);
            string textline = sr.ReadLine();
            sr.Close();
            return textline;
        }

        // POST: api/Room
        public void Post(string value)
        {
            if (Path.GetFileName(value) != value)
            {
                throw new Exception("'fileName' is invalid!");
            }
            string combined = Path.Combine(pathCustom, value);
            File.Create(combined);

        }

        // PUT: api/Room/5
        public void Put(string name,int id)
        {
            //TODO:

            //if (Path.GetFileName(name) != name)
            //{
            //    Path.ChangeExtension(Path.GetFileName(name), name);
            //}
            //else
            //{
            //    throw new Exception("'fileName' is invalid!");
            //}
           
        }

        // DELETE: api/Room/5
        public void Delete(string name)
        {
            string fileName = Path.Combine(pathCustom, name);

            string[] Files = Directory.GetFiles(pathCustom);

            foreach (string file in Files)
            {
                if (file.ToUpper().Contains(fileName.ToUpper()))
                {
                    File.Delete(file);
                    Console.WriteLine("Sucsesful");
                }
            }
        }
    }
}
