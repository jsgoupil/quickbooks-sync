# Quickbooks Sync #

[![Build Status](https://travis-ci.org/jsgoupil/quickbooks-sync.svg?branch=master)](https://travis-ci.org/jsgoupil/quickbooks-sync)

Quickbooks Sync regroups multiple NuGet packages to sync data from Quickbooks Desktop using QbXml. Make requests to Quickbooks Desktop, analyze the returned values, etc.

## QbXml ##

```C#
*Create a request XML with QbXml*
public class CustomerRequest {
    public CustomerRequest() 
    {
        var customerRequest = new QBSync.QbXml.Messages.CustomerQueryRequest()

        // Add some filters here
        customerRequest.MaxReturned = 100;
        customerRequest.FromModifiedDate = new DateTimeType(DateTime.Now);

        // Get the XML
        var xml = customerRequest.GetRequest();
    }
}
```

*Receive a response from Quickbooks and parse the QbXml*
```C#
public class CustomerResponse
{
    public void LoadResponse() 
    {
            var customerResponse = new QBSync.QbXml.Messages.CustomerQueryResponse();

            // Receive a customer object, corresponding to the xml
            var customer = customerResponse.ParseResponse(xml);
    }
}
```