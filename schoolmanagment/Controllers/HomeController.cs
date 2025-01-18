using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using schoolmanagment.Models;
namespace schoolmanagment.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]

        public JsonResult AddClass(Atul obj)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HostelManagement"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandText = "spclassinsert";
            cmd.Parameters.AddWithValue("@className", obj.className);
            cmd.Parameters.AddWithValue("@FirstName", obj.FirstName);
            cmd.Parameters.AddWithValue("@LastName", obj.LastName);
            cmd.Parameters.AddWithValue("@Email", obj.Email);
            cmd.Parameters.AddWithValue("@Mobile", obj.Mobile);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(ds);


            if (ds.Tables[0].Rows.Count > 0)
            {
                obj.msg = ds.Tables[0].Rows[0]["id"].ToString();
            }
            else
            {
                obj.msg = "0";
            }




            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]


        public JsonResult BindData()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HostelManagement"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandText = "bindproc";
            List<Atul> list = new List<Atul>();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(ds);


            if (ds.Tables[0].Rows.Count > 0)


            {

                foreach (DataRow dtrow in ds.Tables[0].Rows)
                {

                    Atul obj = new Atul();
                    obj.classId = dtrow["classId"].ToString();
                    obj.className = dtrow["className"].ToString();
                    obj.FirstName = dtrow["FirstName"].ToString();
                    obj.LastName = dtrow["LastName"].ToString();
                    obj.Email = dtrow["Email"].ToString();
                    obj.Mobile = dtrow["Mobile"].ToString();


                    list.Add(obj);

                }


            }
            else
            {

                list.Add(null);
            }


            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //UPDATE DATA//

        [HttpPost]

        public JsonResult UpdateClass(Atul obj)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HostelManagement"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.CommandText = "updateclassbyak";
            cmd.Parameters.AddWithValue("@className", obj.className);
            cmd.Parameters.AddWithValue("@FirstName", obj.FirstName);
            cmd.Parameters.AddWithValue("@LastName", obj.LastName);
            cmd.Parameters.AddWithValue("@Email", obj.Email);
            cmd.Parameters.AddWithValue("@Mobile", obj.Mobile);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(ds);


            if (ds.Tables[0].Rows.Count > 0)
            {
                obj.msg = ds.Tables[0].Rows[0]["id"].ToString();
            }
            else
            {
                obj.msg = "0";
            }




            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditClass(Atul obj)

        {
            SqlDataAdapter dd = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HostelManagement"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "editclass";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ClassId", obj.classId);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            dd.SelectCommand = cmd;
            dd.Fill(ds);
            Atul objlist = new Atul();
            if (ds.Tables[0].Rows.Count > 0)
            {
                objlist.classId = ds.Tables[0].Rows[0]["classId"].ToString();
                objlist.className = ds.Tables[0].Rows[0]["className"].ToString();
                objlist.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                objlist.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                objlist.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                objlist.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
            }
            else
            {
                objlist.classId = null;
            }
            return Json(objlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult deletedataclass(string deleteid, Atul obj)
        {
            SqlDataAdapter dd = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HostelManagement"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "[Sp_Delete]";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@deleteid", deleteid);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            dd.SelectCommand = cmd;
            dd.Fill(ds);
            Atul objlist = new Atul();
            if (ds.Tables[0].Rows.Count > 0)
            {
                obj.msg = ds.Tables[0].Rows[0]["id"].ToString();
            }
            else
            {

                obj.msg = "0";
            }

            return Json(objlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchFunction(string keyword)
        {
            List<Atul> list = new List<Atul>();

            // Use a more secure connection string handling approach in real scenarios

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HostelManagement"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Searchfunction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", keyword);

                    con.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        // Ensure the DataSet contains tables and rows
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dtrow in ds.Tables[0].Rows)
                            {
                                Atul student = new Atul
                                {
                                    
                                        classId = dtrow["classId"].ToString(),
                                        className = dtrow["className"].ToString(),
                                        FirstName = dtrow["FirstName"].ToString(),
                                        LastName = dtrow["LastName"].ToString(),
                                        Email = dtrow["Email"].ToString(),
                                        Mobile = dtrow["Mobile"].ToString()
                                };
                                list.Add(student);
                            }
                        }
                    }
                }
            }

            // Return the result as JSON
            return Json(list, JsonRequestBehavior.AllowGet);
        }


    }
}






    

