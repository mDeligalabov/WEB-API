using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HBProducts.Chat.API.Models
{
    public class DBConnection
    {
        // Connection string 
        static readonly string connstr = String.Format("Server={0};Port={1};" +
           "User Id={2};Password={3};Database={4};Ssl Mode={5};",
           "hbp1234.postgres.database.azure.com", "5432", "hbp123@hbp1234",
           "123456789@Hb", "HBP", "Require");

        private readonly NpgsqlConnection conn = new NpgsqlConnection(connstr);
        private NpgsqlCommand command;
        private NpgsqlDataReader dataReader;
        private readonly NpgsqlConnection conn2 = new NpgsqlConnection(connstr);
        private NpgsqlCommand command2;
        private NpgsqlDataReader dataReader2;

        //Method for getting the id of a Chat Session
        public int GetChatId(string email, string name)
        {
            try
            {
                conn.Open();
                command = new NpgsqlCommand("Select GET_USER_ID(:uEmail, :uName)", conn);
                command.Parameters.Add(new NpgsqlParameter("uEmail", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("uName", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Prepare();
                command.Parameters[0].Value = email;
                command.Parameters[1].Value = name;
                dataReader = command.ExecuteReader();
                dataReader.Read();
                int user_ex = -1;
                user_ex = (int)dataReader[0];
                conn.Close();
                if (user_ex != -1)
                {
                    conn2.Open();
                    command2 = new NpgsqlCommand("Select NEW_SESSION(:uuEmail)", conn2);
                    command2.Parameters.Add(new NpgsqlParameter("uuEmail", NpgsqlTypes.NpgsqlDbType.Varchar));
                    command2.Prepare();
                    command2.Parameters[0].Value = email;
                    dataReader2 = command2.ExecuteReader();
                    dataReader2.Read();
                    int session_id = -1;
                    session_id = (int) dataReader2[0];
                    conn2.Close();
                    if (session_id != -1)
                    {
                        return session_id;
                    }
                }
                return -12;
            }
            catch(Exception e)
            {
                conn.Close();
                conn2.Close();
                return -123;
            }
            
        }

        // Method for closing an opened Session by a given id 
        public int CloseChatSession(int sessionId)
        {
            try
            {
                conn.Open();
                command = new NpgsqlCommand("Select CLOSE_CHAT_SESSION(:sessID)", conn);
                command.Parameters.Add(new NpgsqlParameter("sessID", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Prepare();
                command.Parameters[0].Value = sessionId;
                dataReader = command.ExecuteReader();
                dataReader.Read();
                int sess_ID = (int)dataReader[0];
                conn.Close();

                return sess_ID;
            }
            catch(Exception ex)
            {
                conn.Close();
                return -123;
            }
        }

        // Method for assigning a Session to to an Employee 
        public int TakeUnansweredSession(int emp_id, int session_id)
        {
            try
            {
                conn.Open();
                command = new NpgsqlCommand("Select TAKE_UNANSWERED_SESSION(:empID, :sessionID)", conn);
                command.Parameters.Add(new NpgsqlParameter("empID", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("sessionID", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Prepare();
                command.Parameters[0].Value = emp_id;
                command.Parameters[1].Value = session_id;
                dataReader = command.ExecuteReader();
                dataReader.Read();
                int sessionsID = (int)dataReader[0];
                conn.Close();
                return sessionsID;
            }
            catch(Exception ex)
            {
                conn.Close();
                return -123;
            }

            
        }

        // Method for sending a message to a Session
        public int SendMessage(int sessionID, string mess)
        {
            try
            {
                //string textMessage = JsonConvert.DeserializeObject<string>(mess);
                Message message = JsonConvert.DeserializeObject<Message>(mess);
                conn.Open();
                command = new NpgsqlCommand("SELECT NEW_MESSAGE(:mText, :sessionId, :isEMP)", conn);
                command.Parameters.Add(new NpgsqlParameter("mText", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("sessionId", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("isEMP", NpgsqlTypes.NpgsqlDbType.Boolean));
                command.Prepare();
                command.Parameters[0].Value = message.Text;
                command.Parameters[1].Value = sessionID;
                command.Parameters[2].Value = message.IsEmployee;
                dataReader = command.ExecuteReader();
                dataReader.Read();
                int messageID = (int)dataReader[0];
                conn.Close();

                return messageID;
            }
            catch(Exception e)
            {
                conn.Close();
                return -123;
            }
        }

        // Metthod for getting the Session by a given id
        public Session GetSession(int sessionID)
        {
            try
            {
                conn.Open();
                command = new NpgsqlCommand("SELECT USER_ID, EMPLOYEE_ID, TIMESTARTED FROM SESSIONS WHERE ID = :sessionID", conn);
                command.Parameters.Add(new NpgsqlParameter("sessionID", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Prepare();
                command.Parameters[0].Value = sessionID;
                dataReader = command.ExecuteReader();
                dataReader.Read();
                int userID = (int)dataReader[0];
                int empID = (int)dataReader[1];
                DateTime obj = (DateTime)dataReader[2];
                string timeStarted = obj.ToString();
                conn.Close();
                conn2.Open();
                command2 = new NpgsqlCommand("SELECT NAME, EMAIL FROM USERS WHERE ID = :userID", conn2);
                command2.Parameters.Add(new NpgsqlParameter("userID", NpgsqlTypes.NpgsqlDbType.Integer));
                command2.Prepare();
                command2.Parameters[0].Value = userID;
                dataReader2 = command2.ExecuteReader();
                dataReader2.Read();
                Customer customer = new Customer((string)dataReader2[0], "Company", (string)dataReader2[1], "+45 12 34 56 78", "Country");
                conn2.Close();
                conn.Open();
                command = new NpgsqlCommand("select name from employees where id = :empID", conn);
                command.Parameters.Add(new NpgsqlParameter("empID", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Prepare();
                command.Parameters[0].Value = empID;
                dataReader = command.ExecuteReader();
                dataReader.Read();
                Employee employee = new Employee((string)dataReader[0]);
                conn.Close();
                conn2.Open();
                command2 = new NpgsqlCommand("SELECT IS_EMP, M_TEXT, DATE_SENT, ID FROM MESSAGES WHERE SESSION_ID = :sessID ORDER BY DATE_SENT ASC; ", conn2);
                command2.Parameters.Add(new NpgsqlParameter("sessID", NpgsqlTypes.NpgsqlDbType.Integer));
                command2.Prepare();
                command2.Parameters[0].Value = sessionID;
                dataReader2 = command2.ExecuteReader();
                Message message;
                List<Message> mList = new List<Message>();
                while (dataReader2.Read())
                {
                    DateTime dateConvert = (DateTime)dataReader2[2];
                    message = new Message((bool)dataReader2[0], (string)dataReader2[1], dateConvert.ToString(), (int)dataReader2[3]);
                    mList.Add(message);
                }
                conn2.Close();
                return new Session(timeStarted, mList, customer, employee, sessionID);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                conn2.Close();
                conn.Close();
                return new Session("", null, null, null, -1);
            }
        }

        //Method for getting the messages of the Customer
        // or the Employee for a Session after(">") a given ID
        public List<Message> GetMessages(int sessionID, int lastMessage, bool emp)
        {
            try
            {
                conn.Open();
                command = new NpgsqlCommand("SELECT ID, M_TEXT, DATE_SENT, IS_EMP FROM MESSAGES " +
                                             "WHERE SESSION_ID = :sessNum AND ID > :messNum AND IS_EMP = :Emp " +
                                             "ORDER BY date_sent asc", conn);
                command.Parameters.Add(new NpgsqlParameter("sessNum", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("messNum", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Parameters.Add(new NpgsqlParameter("Emp", NpgsqlTypes.NpgsqlDbType.Boolean));
                command.Prepare();
                command.Parameters[0].Value = sessionID;
                command.Parameters[1].Value = lastMessage;
                command.Parameters[2].Value = emp;
                dataReader = command.ExecuteReader();
                List<Message> list = new List<Message>();
                Message message;
                DateTime dateConvert;
                while (dataReader.Read())
                {
                    dateConvert = (DateTime)dataReader[2];
                    message = new Message((bool)dataReader[3], (string)dataReader[1], (string)dateConvert.ToString(), (int)dataReader[0]);
                    list.Add(message);
                }
                conn.Close();

                return list;
            }
            catch (Exception ex) {
                conn.Close();
                return null;
            }
        }

        //Method for getting the list of the Sessions 
        // for the given Employee ID 
        public List<Session> GetUnanswerdSessions(int empID)
        {
            List<Session> list = new List<Session>();
            try
            {
                conn.Open();
                command = new NpgsqlCommand("select s.id, s.timestarted, u.name, u.email from sessions s"
                                                + " join users u on s.user_id = u.id"
                                                + " where employee_id = :empID and s.isClosed = false", conn);
                command.Parameters.Add(new NpgsqlParameter("empID", NpgsqlTypes.NpgsqlDbType.Integer));
                command.Prepare();
                command.Parameters[0].Value = empID;
                dataReader = command.ExecuteReader();
                Session sessionTemp;
                DateTime dateConvert;
                while (dataReader.Read())
                {
                    dateConvert = (DateTime)dataReader[1];
                    sessionTemp = new Session(dateConvert.ToString(), new List<Message>(), new Customer((string)dataReader[2], "Company X", 
                                                (string)dataReader[3], "+45 11 22 33 44", "Bulgaria"), new Employee("NULL"), (int)dataReader[0]);
                    list.Add(sessionTemp);
                }
                conn.Close();

                return list;
            }
            catch(Exception ex)
            {
                list.Add(new Session(ex.StackTrace, null, null, null, 0));
                conn.Close();
                return list;
            }
        }

        //Method for getting the Employee ID 
        // given a name
        public int GetEmpID(string name)
        {
            conn.Open();
            command = new NpgsqlCommand("select id from employees where name = :empName", conn);
            command.Parameters.Add(new NpgsqlParameter("empName", NpgsqlTypes.NpgsqlDbType.Varchar));
            command.Prepare();
            command.Parameters[0].Value = name;
            dataReader = command.ExecuteReader();
            dataReader.Read();
            int empID = (int)dataReader[0];
            conn.Close();

            return empID;
        } 
    }
}