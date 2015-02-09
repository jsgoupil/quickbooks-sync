using QBSync.QbXml;
using QBSync.WebConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.WebConnector.Messages
{
    public abstract class StepQueryResponseBase<T, Y, YResult> : StepQueryResponse
        where T : QbXmlRequest, new()
        where Y : QbXmlResponse<YResult>, new()
    {
        protected int step;

        public StepQueryResponseBase(int step)
        {
            this.step = step;
        }

        public virtual string SendXML()
        {
            var request = CreateRequest();
            ExecuteRequest(request);
            return request.GetRequest();
        }

        public virtual int ReceiveXML(string response, string hresult, string message)
        {
            var responseObject = CreateResponse();
            var msgResponseObject = responseObject.ParseResponse(response);
            ExecuteResponse(msgResponseObject);

            return 0;
        }

        public virtual bool GotoNextStep()
        {
            return true;
        }

        public virtual string LastError()
        {
            return string.Empty;
        }

        protected virtual T CreateRequest()
        {
            return new T();
        }

        protected virtual Y CreateResponse()
        {
            return new Y();
        }

        protected virtual void ExecuteRequest(T request)
        {
        }

        protected virtual void ExecuteResponse(QbXmlMsgResponse<YResult> response)
        {
        }

        protected abstract internal void SaveMessage(int stepNumber, string key, string value);
        protected abstract internal string RetrieveMessage(int step, string key);
    }
}
