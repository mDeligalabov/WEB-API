using HBProducts.Chat.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HBProducts.Chat.API.Controllers
{
    public class ChatController : ApiController
    {
        private DBConnection db = new DBConnection();
        
        //Method for getting the id of a Chat Session
        // GET: api/Chat/GetChatId/{uEmail}/{uName}
        [Route("api/Chat/GetChatId/{uEmail}/{uName}")]
        [HttpGet]
        public int GetChatId(string uEmail, string uName)
        {
            return db.GetChatId(uEmail, uName);
        }

        // Metthod for getting the Session by a given id
        // GET: api/Chat/GetSession/{sessionID:int}
        [Route("api/Chat/GetSession/{sessionID:int}")]
        [HttpGet]
        public string GetSession(int sessionId)
        {
            return JsonConvert.SerializeObject(db.GetSession(sessionId), Formatting.Indented);
        }

        // Method for closing an opened Session by a given id 
        // GET: api/Chat/GetChatId/{uEmail:string}
        [Route("api/Chat/CloseChatSession/{sessionId:int}")]
        [HttpGet]
        public int CloseChatSession(int sessionId)
        {
            return db.CloseChatSession(sessionId);
        }

        // Method for assigning a Session to to an Employee 
        // GET: api/Chat/TakeSession/{empId:int}/{sessionId:int}
        [Route("api/Chat/TakeSession/{empId:int}/{sessionId:int}")]
        [HttpGet]
        public int TakeSession(int empId, int sessionId)
        {
            return db.TakeUnansweredSession(empId, sessionId);
        }

        // Method for sending a message to a Session
        // Post:api/Chat/SendMessage/{sessionID:int}
        [Route("api/Chat/SendMessage/{sessionID:int}")]
        [HttpPost]
        public int SendMessage([FromBody]string mess, int sessionID)
        {
            return db.SendMessage(sessionID, mess);
        }

        //Method for getting the Employee messages from a Session
        // that are after(">") a given ID
        //Get: api/Chat/GetEmpMessages/5
        [Route("api/Chat/GetEmpMessages/{sessionId:int}/{messageId:int}")]
        [HttpGet]
        public string GetEmpMessages(int sessionId, int messageId)
        {
            return JsonConvert.SerializeObject(db.GetMessages(sessionId, messageId, true), Formatting.Indented);
        }

        //Method for getting the Customer messages from a Session
        // that are after(">") a given ID
        //Get: api/Chat/GetCustMessages/5
        [Route("api/Chat/GetCustMessages/{sessionId:int}/{messageId:int}")]
        [HttpGet]
        public string GetCustMessages(int sessionId, int messageId)
        {
            return JsonConvert.SerializeObject(db.GetMessages(sessionId, messageId, false), Formatting.Indented);
        }

        //Method for getting the list of untake Sessions 
        //Get: api/Chat/GetUnansweredSessions
        [Route("api/Chat/GetUnansweredSessions")]
        [HttpGet]
        public string GetUnansweredSessions()
        {
            return JsonConvert.SerializeObject(db.GetUnanswerdSessions(-1), Formatting.Indented);
        }

        // Method for getting a list of Sessions that an Employee took
        // given the Employee ID
        //Get: api/Chat/GetEmpSessions/2
        [Route("api/Chat/GetEmpSessions/{empID:int}")]
        [HttpGet]
        public string GetEmpSessions(int empID)
        {
            return JsonConvert.SerializeObject(db.GetUnanswerdSessions(empID), Formatting.Indented);
        }

        //Method for getting the Employee ID 
        // given a name
        //Get: api/Chat/GetEmpID/Martin
        [Route("api/Chat/GetEmpID/{empName}")]
        [HttpGet]
        public int GetEmpID(string empName)
        {
            return db.GetEmpID(empName);
        }
    }
}
