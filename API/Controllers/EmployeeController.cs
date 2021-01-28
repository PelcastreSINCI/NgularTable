using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using API.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _cofiguration;
        private readonly IWebHostEnvironment _env;
         public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _cofiguration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
            select EmployeeId, EmployeName, Department,
            convert(varchar(10),DateOfJoining,120) as DateOfJoining
            ,PhotoFileName
            from dbo.Employee
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _cofiguration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon =new SqlConnection(sqlDataSource) )
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"
            insert into dbo.Employee 
            (EmployeName,Department,DateOfJoining,PhotoFileName)
            values
            (
                '"+emp.EmployeeName+@"'
                ,'"+emp.Department+@"'
                ,'"+emp.DateOfJoining+@"'
                ,'"+emp.PhotoFileName+@"'
                
                )
                ";
            DataTable table = new DataTable();
            string sqlDataSource = _cofiguration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon =new SqlConnection(sqlDataSource) )
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");


        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
            update dbo.Employee set 
            EmployeName = '"+emp.EmployeeName+@"'
           , Department = '"+emp.Department+@"'
           , DateOfJoining = '"+emp.DateOfJoining+@"'
            where EmployeeId = "+emp.EmployeId+ @"";
            DataTable table = new DataTable();
            string sqlDataSource = _cofiguration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon =new SqlConnection(sqlDataSource) )
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Update Successfully");


        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
            delete from dbo.Employee where EmployeeId = "+id +@"
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _cofiguration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon =new SqlConnection(sqlDataSource) )
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Delete Successfully");


        }


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath+"/Photos/" + filename;

                using(var stream = new FileStream(physicalPath,FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);

            }
            catch (Exception)
            {
                return new JsonResult("anonymus.jpg");
            }
        }

        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            string query = @"
            select DepartmentName from dbo.Department
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _cofiguration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon =new SqlConnection(sqlDataSource) )
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);

        }


    }
}