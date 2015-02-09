using Moq;
using QBSync.WebConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBSync.WebConnector.Tests
{
    class MockStepQueryResponse1 : StepQueryResponse
    {
        public MockStepQueryResponse1(AuthenticatedTicket authenticatedTicket)
        {
            AuthenticatedTicket = authenticatedTicket;
        }

        public AuthenticatedTicket AuthenticatedTicket { get; set; }

        public string SendXML()
        {
            throw new NotImplementedException();
        }

        public int ReceiveXML(string response, string hresult, string message)
        {
            throw new NotImplementedException();
        }

        public string LastError()
        {
            throw new NotImplementedException();
        }

        public bool GotoNextStep()
        {
            throw new NotImplementedException();
        }
    }
    class MockStepQueryResponse2 : StepQueryResponse
    {
        public MockStepQueryResponse2(AuthenticatedTicket authenticatedTicket)
        {
            AuthenticatedTicket = authenticatedTicket;
        }

        public AuthenticatedTicket AuthenticatedTicket { get; set; }

        public string SendXML()
        {
            throw new NotImplementedException();
        }

        public int ReceiveXML(string response, string hresult, string message)
        {
            throw new NotImplementedException();
        }

        public string LastError()
        {
            throw new NotImplementedException();
        }

        public bool GotoNextStep()
        {
            throw new NotImplementedException();
        }
    }
}
