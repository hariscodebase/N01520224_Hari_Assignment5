using MySql.Data.MySqlClient;
using N01520224_Hari_Assignment5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace N01520224_Hari_Assignment5.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Fetches the list of Teachers based on search input or shows full list
        /// </summary>
        /// <param name="NameKey">String input parameter</param>
        /// <returns>returns a list of teacher object based on the input</returns>
        [HttpGet]
        [Route("api/teacherdata/listteachers/{NameKey?}")]
        public List<Teacher> ListTeachers(string NameKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            string query = "select * from teachers";

            //handling the null case
            if (NameKey != null)
            {
                cmd.CommandText = query + " where lower(teacherfname) = lower(@key)";
                cmd.Parameters.AddWithValue("key", NameKey);
                cmd.Prepare();
            }
            else
            {
                cmd.CommandText = query;
            }


            //SQl 

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string TeacherHireDate = ResultSet["hiredate"].ToString();
                string TeacherSalary = ResultSet["salary"].ToString();


                Teacher NewTeacher = new Teacher();
                NewTeacher.Id = TeacherId;
                NewTeacher.FName = TeacherFName;
                NewTeacher.LName = TeacherLName;
                NewTeacher.HireDate = TeacherHireDate;
                NewTeacher.Salary = TeacherSalary;

                //Add the Teacher Name to the List
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of Teacher names
            return Teachers;
        }


        /// <summary>
        /// Finds an Teacher in the system given an ID
        /// </summary>
        /// <param name="id">The Teacher primary key</param>
        /// <returns>A Teacher object</returns>
        [HttpGet]
        [Route("api/teacherdata/showdetails/{id}")]
        public Teacher ShowDetails(int? id)
        {

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT * FROM teachers WHERE teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Craete an instant of teacher
            Teacher teacher = new Teacher();

            while (ResultSet.Read())
            {

                //Access Column information by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string Salary = ResultSet["salary"].ToString();


                teacher.Id = TeacherId;
                teacher.FName = TeacherFname;
                teacher.LName = TeacherLname;
                teacher.Salary = Salary;

            }


            return teacher;
        }


        /// <summary>
        /// Adds a teacher
        /// </summary>
        /// <param name="teacher"></param>
        [HttpPost]
        public void AddTeacher(Teacher teacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherFName,@teacherLName,@employeeNumber, CURRENT_DATE(), @salary)";
            cmd.Parameters.AddWithValue("@teacherFName", teacher.FName);
            cmd.Parameters.AddWithValue("@teacherLName", teacher.LName);
            cmd.Parameters.AddWithValue("@employeeNumber", teacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@salary", teacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }


        /// <summary>
        /// Deletes the teacher based on input id
        /// </summary>
        /// <param name="TeacherId"></param>
        public void DeleteTeacher(int? TeacherId)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();


            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "delete from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", TeacherId);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Updates the teacher with teacker info based on teacher id input
        /// </summary>
        /// <param name="TeacherId">primary key to update the teacher</param>
        /// <param name="TeacherInfo">teacher object with fName, lName, salary</param>
        public void UpdateTeacher(int TeacherId, Teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update teachers set teacherfname = @fname, teacherlname = @lname, salary = @sal " +
                "where teacherid = @teacherid";
            cmd.Parameters.AddWithValue("@fname", TeacherInfo.FName);
            cmd.Parameters.AddWithValue("@lname", TeacherInfo.LName);
            cmd.Parameters.AddWithValue("@sal", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@teacherid", TeacherId);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }
    }
}
