using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Company;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Configuration;

namespace RESTWebService
{
    public class Service: IHttpHandler
    {
        #region Private Members

        private Employee emp;
        private DataAccess dal;
        private string connString;
        private ErrorHandler errHandler;

        #endregion

        public Service(string connStr)
        {
            dal = new DataAccess(connStr);
        }
        bool IHttpHandler.IsReusable
        {
            get { throw new NotImplementedException(); }
        }
        public void ProcessRequest(HttpContext context)
        {
            string url = Convert.ToString(context.Request.Url);
            connString = ConfigurationManager.ConnectionStrings["Properties.Settings.Default.ConnectionString"].ConnectionString;
            dal = new DataAccess(connString);
            errHandler = new ErrorHandler();

            switch (context.Request.HttpMethod)
            {
                case "GET":
                    READ(context);
                    break;
                case "POST":
                    CREATE(context);
                    break;
                case "PUT":
                    UPDATE(context);
                    break;
                case "DELETE":
                    DELETE(context);
                    break;
                default:
                    break;
            }
        }

        private void DELETE(HttpContext context)
        {
           //DELETE
            try
            {
                int EmpCode = Convert.ToInt16(context.Request["id"]);
                dal.DeleteEmployee(EmpCode);
                WriteResponse("Employee Deleted Successfully");
            }
            catch (Exception ex)
            {

                WriteResponse("Error in CREATE");
                errHandler.ErrorMessage = dal.GetException();
                errHandler.ErrorMessage = ex.Message.ToString();
            }
        }

        private void UPDATE(HttpContext context)
        {
            try
            {
                //PUT
                byte[] PUTRequestByte = context.Request.BinaryRead(context.Request.ContentLength);
                context.Response.Write(PUTRequestByte);

                // Deserialize Employee
                Company.Employee emp = Deserialize(PUTRequestByte);
                dal.UpdateEmployee(emp);
                //context.Response.Write("Employee Updtated Sucessfully");
                WriteResponse("Employee Updtated Sucessfully");
            }
            catch (Exception ex)
            {
                WriteResponse("Error in UPDATE");
                errHandler.ErrorMessage = dal.GetException();
                errHandler.ErrorMessage = ex.Message.ToString();
            }
        }

        private void CREATE(HttpContext context)
        {
            try
            {
                //POST
                byte[] PostData = context.Request.BinaryRead(context.Request.ContentLength);
                //Convert the bytes to string using Encoding class
                string str = Encoding.UTF8.GetString(PostData);
                // deserialize xml into employee class
                Company.Employee emp = Deserialize(PostData);
                // Insert data in database
                dal.AddEmployee(emp);             
            }
            catch (Exception ex)
            {                
                WriteResponse("Error in CREATE");
                errHandler.ErrorMessage = dal.GetException();
                errHandler.ErrorMessage = ex.Message.ToString();                
            }
        }

        private void READ(HttpContext context)
        {
            int employeeCode = Convert.ToInt16(context.Request["id"]);

            Employee emp = dal.GetEmployee(employeeCode);
            if (emp == null)
                context.Response.Write(employeeCode + "No Employee Found");

            string serializedEmployee = Serialize(emp);
            context.Response.ContentType = "text/xml";
            WriteResponse(serializedEmployee);
        }

        private void WriteResponse(string responseStr)
        {
            HttpContext.Current.Response.Write(responseStr);
        }

        private string Serialize(Employee emp)
        {
            try
            {
                string xmlString;
                XmlSerializer xs = new XmlSerializer(typeof(Employee));
                MemoryStream memoryStream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(writer, emp);

                memoryStream = (MemoryStream)writer.BaseStream;
                //Convert to array
                xmlString = UTF8ByteArrayToString(memoryStream.ToArray());
                return xmlString;
            }
            catch (Exception ex)
            {
                errHandler.ErrorMessage = ex.Message.ToString();
                throw;
            }
        }

        private string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private Company.Employee Deserialize(byte[] xmlByteData)
        {
            try
            {
                XmlSerializer ds = new XmlSerializer(typeof(Company.Employee));
                MemoryStream memoryStream = new MemoryStream(xmlByteData);
                Company.Employee emp = new Company.Employee();
                emp = (Company.Employee)ds.Deserialize(memoryStream);
                return emp;
            }
            catch (Exception ex)
            {

                errHandler.ErrorMessage = dal.GetException();
                errHandler.ErrorMessage = ex.Message.ToString();
                throw;
            }
        }

        
    }
}
