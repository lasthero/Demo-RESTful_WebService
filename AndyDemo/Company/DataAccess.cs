using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Company
{
    public class DataAccess
    {
        private SqlConnection conn;
        private static string connString;
        private SqlCommand command;
        private static List<Employee> empList;
        private ErrorHandler err;

        public DataAccess(string _connString)
        {
            err = new ErrorHandler();
            connString = _connString;
        }

        public void AddEmployee(Employee emp)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand command = null;
                using (conn)
                {
                    //using parametirized query
                    string sqlInserString =
                       "INSERT INTO Employee (FirstName, LastName, ID, " +
                       "Designation) VALUES (@firstName, @lastName, @ID, @designation)";

                    command = new SqlCommand();
                    command.Connection = conn;
                    command.Connection.Open();
                    command.CommandText = sqlInserString;

                    SqlParameter firstNameparam = new SqlParameter("@firstName", emp.FirstName);
                    SqlParameter lastNameparam = new SqlParameter("@lastName", emp.LastName);
                    SqlParameter IDparam = new SqlParameter("@ID", emp.EmpCode);
                    SqlParameter designationParam = new SqlParameter("@designation", emp.Designation);

                    command.Parameters.AddRange(new SqlParameter[]{
                        firstNameparam,lastNameparam,IDparam,designationParam});
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                err.ErrorMessage = ex.Message.ToString();
                throw;
            }
        }

        public void UpdateEmployee(Employee emp)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand command = null;
                using (conn)
                {
                    //using parametirized query
                    string sqlInserString =
                       "UPDATE Employee SET FirstName=@firstName, LastName=@lastName, Designation=@designation) WHERE ID=@ID;";

                    command = new SqlCommand();
                    command.Connection = conn;
                    command.Connection.Open();
                    command.CommandText = sqlInserString;

                    SqlParameter firstNameparam = new SqlParameter("@firstName", emp.FirstName);
                    SqlParameter lastNameparam = new SqlParameter("@lastName", emp.LastName);
                    SqlParameter IDparam = new SqlParameter("@ID", emp.EmpCode);
                    SqlParameter designationParam = new SqlParameter("@designation", emp.Designation);

                    command.Parameters.AddRange(new SqlParameter[]{
                        firstNameparam,lastNameparam,IDparam,designationParam});
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                err.ErrorMessage = ex.Message.ToString();
                throw;
            }
        }

        public void DeleteEmployee(int empId)
        {
            try
            {
                using (conn)
                {
                    string sqlDeleteString =
                    "DELETE FROM Employee WHERE ID=@ID ";

                    conn = new SqlConnection(connString);

                    command = new SqlCommand();
                    command.Connection = conn;
                    command.Connection.Open();
                    command.CommandText = sqlDeleteString;

                    SqlParameter IDparam = new SqlParameter("@ID", empId);
                    command.Parameters.Add(IDparam);
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                err.ErrorMessage = ex.Message.ToString();
                throw;
            }
        }

        public Employee GetEmployee(int empId)
        {
            try
            {
                if (empList == null)
                {
                    empList = GetEmployees();
                }
                // enumerate through all employee list
                // and select the concerned employee
                foreach (Employee emp in empList)
                {
                    if (emp.EmpCode == empId)
                    {
                        return emp;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                err.ErrorMessage = ex.Message.ToString();
                throw;
            }
        }

        private List<Employee> GetEmployees()
        {
            try
            {
                using (conn)
                {
                    empList = new List<Employee>();
                    conn = new SqlConnection(connString);

                    string query = @"SELECT * FROM Employee";

                    command = new SqlCommand(query, conn);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read() != null)
                    {
                        Employee emp = new Employee();
                        emp.FirstName = reader[0].ToString();
                        emp.LastName = reader[1].ToString();
                        emp.EmpCode = Convert.ToInt16(reader[2]);
                        emp.Designation = reader[3].ToString();
                        empList.Add(emp);
                    }
                    command.Connection.Close();
                    return empList;
                }
            }
            catch (Exception ex)
            {
                err.ErrorMessage = ex.Message.ToString();
                throw;
            }
        }

        public string GetException()
        {
            return err.ErrorMessage.ToString();
        }
    }
}
