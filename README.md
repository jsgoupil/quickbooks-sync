# QuickBooks Sync [![Build Status](https://travis-ci.org/jsgoupil/quickbooks-sync.svg?branch=master)](https://travis-ci.org/jsgoupil/quickbooks-sync) #

QuickBooks Sync regroups multiple NuGet packages to sync data from QuickBooks Desktop using QbXml. Make requests to QuickBooks Desktop, analyze the returned values, etc.

**This project is actively maintained and is in its early alpha stage. Many breaks will be introduced until stability is reached.**

## QbXml ##

QbXml is the language used by QuickBooks desktop to exchange back and forth data between an application and the QuickBooks database.

Here is a couple of ideas how you can make some requests and parse responses.

*Create a request XML with QbXml*
```C#
public class CustomerRequest
{
  public CustomerRequest()
  {
    var request = new QbXmlRequest();
    var innerRequest = new CustomerQueryRqType();

    // Add some filters here
    innerRequest.MaxReturned = "100";
    innerRequest.FromModifiedDate = new DATETIMETYPE(DateTime.Now);

    request.AddToSingle(innerRequest);

    // Get the XML
    var xml = request.GetRequest();
  }
}
```

*Receive a response from QuickBooks and parse the QbXml*
```C#
public class CustomerResponse
{
  public void LoadResponse(string xml)
  {
    var response = new QbXmlResponse();
    var rs = response.GetSingleItemFromResponse<CustomerQueryRsType>(xml);

    // Receive customer objects, corresponding to the xml
    var customers = rs.CustomerRet;
  }
}
```

## Web Connector ##

Version 1.0.0 supports .NET Standard 2.0. We follow the dependency injection standard to load the services.
We abstracted the SOAP protocol so you only have to implement necessary services in order to make your queries to QuickBooks.

Thanks to the Web Connector, you can communicate with QuickBooks Desktop. Users must download it at the following address: [http://marketplace.intuit.com/webconnector/](http://marketplace.intuit.com/webconnector/ "http://marketplace.intuit.com/webconnector/")

The Web Connector uses the SOAP protocol to talk with your website, the NuGet package takes care of the heavy lifting to talk with the QuickBooks Desktop. However, you must implement some services in order to get everything working according to your needs. The Web Connector will come periodically to your website asking if you have any requests to do to its database.
With the nature of SOAP in mind, there are no protocols keeping the connection state between QuickBooks and your server. For this reason, your server needs to keep track of sessions with a database.

### How does it work? ###
Once the Web Connector downloaded, your user must get a QWC file that will connect the Web Connector to your website. To generate a QWC file, load the appropriate service then pass in your model:

```C#
public MyController(IWebConnectorQwc webConnectorQwc)
{
    this.webConnectorQwc = webConnectorQwc;
}

// ...

var data = webConnectorQwc.GetQwcFile(new QbSync.WebConnector.Models.WebConnectorQwcModel
{
    AppName = "My App",
    AppDescription = "Sync QuickBooks with My Website",
    AppSupport = $"{url}/support",
    AppURL = $"{url}/QBConnectorAsync.asmx",
    FileID = Guid.NewGuid(), // Don't generate a new guid all the time, save it somewhere
    OwnerID = Guid.NewGuid(), // Don't generate a new guid all the time, save it somewhere
    UserName = "jsgoupil",
    RunEvery = new TimeSpan(0, 30, 0),
    QBType = QbSync.WebConnector.Models.QBType.QBFS
});
```

### Register the services ###

In your Startup.cs, registers the services as follow:

```C#
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddWebConnector(options =>
        {
            options
                .AddAuthenticator<Authenticator>()

                //.WithMessageValidator<MyMessageValidator>()
                //.WithWebConnectorHandler<MyWebConnectorHandler>()

                // Register steps; the order matters.
                .WithStep<CustomerQuery.Request, CustomerQuery.Response>()
                .WithStep<InvoiceQuery.Request, InvoiceQuery.Response>();
        });
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // Before UseMvc()
    app
        .UseWebConnector(options =>
        {
            options.SoapPath = "/QBConnectorAsync.asmx";
        });

    app.UseMvc();
}
```

### Implement an Authenticator ###

An authenticator will keep the state in your database of what is happening with the Web Connector session.

```C#
public interface IAuthenticator
{
    Task<IAuthenticatedTicket> GetAuthenticationFromLoginAsync(string login, string password);
    Task<IAuthenticatedTicket> GetAuthenticationFromTicketAsync(string ticket);
    Task SaveTicketAsync(IAuthenticatedTicket ticket);
}
```

1. `GetAuthenticationFromLoginAsync` - Authenticates a user from its login/password combination.
2. `GetAuthenticationFromTicketAsync` - Authenticates a ticket previously given from a GetAuthenticationFromLogin call.
3. `SaveTicketAsync` - Saves the ticket to the database.

The IAuthenticatedTicket contains 3 mandatory properties:

```C#
public interface IAuthenticatedTicket
{
  string Ticket { get; set; }
  string CurrentStep { get; set; }
  bool Authenticated { get; set; }
}
```

1. `Ticket` - Exchanged with the Web Connector. It acts as a session identifier.
2. `CurrentStep` - State indicating at which step the Web Connector is currently at.
3. `Authenticated` - Simple boolean indicating if the ticket is authenticated.

If a user is not authenticated, make sure to return a ticket value, but set the Authenticated to `false`.
You may want to attach more properties to the interface, such as your `UserId`, `TimeZone`, etc.


### Implement a step ###

By registering a step such as `CustomerQuery`, you can get customers from the QuickBooks database.
Since all steps will require a database manipulation on your side, you have to implement it yourself. But don't worry, it is pretty simple.
The classes request and response are split because it is important to understand that they do not share fields: the HTTP requests coming from the Web Connector are made separately.

Don't forget to register them in your startup. The order matters, the steps will be executed in the order you provided.

After the step has executed, the next step in order will run.

Here is an example:
```C#
public class CustomerQuery
{
    public const string NAME = "CustomerQuery";

    public class Request : StepQueryRequestBase<CustomerQueryRqType>
    {
        public override string Name => NAME;

        protected override Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, CustomerQueryRqType request)
        {
            // Do some operations on the customerRequest to get only specific ones
            request.FromModifiedDate = new DateTimeType(DateTime.Now);

            return base.ExecuteRequestAsync(authenticatedTicket, request);
        }
    }

    public class Response : StepQueryResponseBase<CustomerQueryRsType>
    {
        public override string Name => NAME;

        protected override Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, CustomerQueryRsType response)
        {
            // Execute some operations with your database.

            return base.ExecuteResponseAsync(authenticatedTicket, response);
        }
    }
}
```

The 2 classes CustomerQueryRqType/CustomerQueryRsType are provided by the QbXml NuGet package. You associate the request and the response. They implement `QbRequest` and `QbResponse`.
To find the correct request and response pair, visit https://developer-static.intuit.com/qbSDK-current/Common/newOSR/index.html


### Implement step with an iterator ###

When you make a request to the QuickBooks database, you might receive hundreds of objects back. Your server or the database won't be able to handle that many; you have to break the query into batches. We have everything handled for you, but we need to save another state to the database. Instead of deriving from `StepQueryResponseBase`, you have to derive from `StepQueryWithIterator` and implement 2 methods.

* In the request *
```C#
protected abstract Task<string> RetrieveMessageAsync(IAuthenticatedTicket ticket, string key);
```

* In the response *
```C#
protected abstract void SaveMessageAsync(IAuthenticatedTicket ticket, string key, string value);
```

Save the message to the database based on its ticket, `CurrentStep` and `key`. Then retrieve it from the same keys.

By default, if you derive from the iterator, the query is batched with 100 objects.

The requests and responses that support an iterator implements `QbIteratorRequest` and `QbIteratorResponse`.


### Implement a step with multiple requests ###

If you wish to send more than one request at once to QuickBooks, inherit from `GroupStepQueryRequestBase` and `GroupStepQueryResponseBase` and send as many objects you want to QuickBooks. Keep in mind that you should keep the final result
under a certain size to allow your server to be able to parse it.

Look at this example which make a CustomerAdd and a CustomerQuery in one step.

```C#
public class CustomerGroupAddQuery
{
    public const string NAME = "CustomerGroupAddQuery";

    public class Request : GroupStepQueryRequestBase
    {
        public override string Name => NAME;

        private readonly ApplicationDbContext dbContext;

        public Request(
            ApplicationDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        protected override Task<IEnumerable<IQbRequest>> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket)
        {
            var list = new List<IQbRequest>
            {
                new CustomerAddRqType
                {
                    CustomerAdd = new QbSync.QbXml.Objects.CustomerAdd
                    {
                        Name = "Unique Name" + Guid.NewGuid().ToString("D"),
                        FirstName = "User " + authenticatedTicket.GetUserId().ToString()
                    }
                },
                new CustomerQueryRqType
                {
                    ActiveStatus = ActiveStatus.All
                }
            };

            return Task.FromResult(list as IEnumerable<IQbRequest>);
        }

        protected override Task<QBXMLMsgsRqOnError> GetOnErrorAttributeAsync(IAuthenticatedTicket authenticatedTicket)
        {
            // This is the default behavior, use this overriden method to change it to stopOnError
            // QuickBooks does not support rollbackOnError
            return Task.FromResult(QBXMLMsgsRqOnError.continueOnError);
        }
    }

    public class Response : GroupStepQueryResponseBase
    {
        public override string Name => NAME;

        private readonly ApplicationDbContext dbContext;

        public Response(
            ApplicationDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        protected override Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, IEnumerable<IQbResponse> responses)
        {
            foreach (var item in responses)
            {
                switch (item)
                {
                    case CustomerQueryRsType customerQueryRsType:
                        // Do something with the CustomerQuery data
                        break;
                    case CustomerAddRsType customerAddRsType:
                        // Do something with the CustomerAdd data
                        break;
                }
            }

            return base.ExecuteResponseAsync(authenticatedTicket, responses);
        }
    }
}
```


### Changing the step order at runtime ###

If you want to change the step order at runtime, you may implement the following methods:

```C#
public interface IStepQueryResponse
{
    Task<string> GotoStepAsync();
    Task<bool> GotoNextStepAsync();
}
```

1. `GotoStepAsync` - Indicates the exact step name you would like to go. If you return null, the `GotoNextStep` will be called.
2. `GotoNextStepAsync` - Indicates if you should go to the next step or not. If you return `false`, the same exact step will be executed.


### Implement a MessageValidator ###

QuickBooks supports multiple versions. However, this package supports only version 13.0 and above. In order to validate a request, you must provide a `IMessageValidator`.
The reason this package cannot validate the version is because of the nature of the Web Connector: it takes 2 calls from the WebConnector to validate the version then warn the user.

1. The first call sends a version to your server. You can validate the version and must save the ticket for reference in the second call.
2. The second call, you need to tell the WebConnector the version was wrong based on the ticket saved in step 1.

Since this is done with two requests, the first request must persist that the version is wrong based on the ticket.
With `IsValidTicket`, simply check if the ticket has been saved in your database (as invalid). If you find the ticket in your database, you can safely remove it from it as this method will not be called again with the same ticket.

This step is optional. If you don't implement a MessageValidator, we assume that the version is valid.

The MessageValidator can also be used to get the company file path that QuickBooks sends you.


### Implement a WebConnectorHandler ###

This step is optional but highly recommended, will receive some calls from the Web Connector that you can take further actions.
One particular action that you should handle is the QuickBooks TimeZone bug.

QbXml doesn't handle Daylight Saving Time properly when it comes to DateTime.
When the Daylight Saving Time is activated, the times returned by QuickBooks are off by the delta DST (typically 1h).

To overcome this problem, if you provide a TimeZoneInfo to the `GetOptionsAsync`, QbXml package will fix the times that are not properly set.

If you do not wish to implement all the methods, you can override `WebConnectorHandlerNoop`.

```C#
public interface IWebConnectorHandler
{
    Task ProcessClientInformationAsync(IAuthenticatedTicket authenticatedTicket, string response);
    Task<QbXmlResponseOptions> GetOptionsAsync(IAuthenticatedTicket authenticatedTicket);
    Task OnExceptionAsync(IAuthenticatedTicket authenticatedTicket, Exception exception);
    Task<int> GetWaitTimeAsync(IAuthenticatedTicket authenticatedTicket);
    Task<string> GetCompanyFileAsync(IAuthenticatedTicket authenticatedTicket);
    Task CloseConnectionAsync(IAuthenticatedTicket authenticatedTicket);
}
```

1. `ProcessClientInformationAsync` - Returns the configuration QuickBooks is in. This method is called once per session.
2. `GetOptionsAsync` - Returns the options to configure QbXml. Useful to fix the timezone QbXml bug. The timezone info must be based off the user who is connecting to your server. Match it with their ticket.
3. `OnExceptionAsync` - Called when any types of exception occur on the server.
4. `GetWaitTimeAsync` - Tells the Web Connector to come back later after X seconds. Returning 0 means to do the work immediately.
5. `GetCompanyFileAsync`- Uses the company file path. Return an empty string to use the file that is currently opened.
6. `CloseConnectionAsync` - The connection is closing, the Web Connector will not come back with this ticket.


## Internally how it works ##

If you are not contributing to this project, you most likely don't need to read this section.

The Web Connector executes the following tasks:

1. Authenticate - Sends the login/password that you must verify. You also return a session ticket that will be used for the rest of messages that are exchanged back and forth.
2. SendRequestXML - The Web Connector expects that you return an XML command that will execute on the database.
3. ReceiveRequestXML - Response regarding the previous step.
4. GOTO Step 2 - Until you return an empty string, indicating that you are done.
5. CloseConnection - Connection is done.


### QbManager ###

The QbManager can be overriden in order to handle the communication at a lower level. You most likely don't need to do this.

1. `SaveChanges` - Called before returning any data to the Web Connector. It's time to save data to your database.
2. `LogMessage` - Data going in or out goes through this method, you can save it to a database in order to better debug.
3. `GetWaitTime` - Tells the Web Connector to come back in X seconds.
4. `Authenticate` - Verifies if the login/password is correct. Returns appropriate message to the Web Connector in order to continue further communication.
5. `ServerVersion` - Returns the server version.
6. `ClientVersion` - Indicates which version is the Web Connector. Returns W:<message> to return a warning; E:<message> to return an error. Empty string if everything is fine.
7. `SendRequestXML` - The Web Connector is asking what has to be done to its database. Return an QbXml command.
8. `ReceiveRequestXML` - Response from the Web Connector based on the previous command sent.
9. `GetLastError` - Gets the last error that happened. This method is called only if an error is found.
10. `ConnectionError` - An error happened with the Web Connector.
11. `CloseConnection` - Closing the connection. Return a string to show to the user in the Web Connector.
12. `OnException` - Called if any of your steps throw an exception. It would be a great time to log this exception for future debugging.
13. `ProcessClientInformation` - Called when the WebConnector first connect to the service. It contains the information about the QuickBooks database.
14. `GetOptions` - Returns QbXml options. Used for TimeZone bug. See below.
15. `GetCompanyFile` - Indicates which company file to use on the client. By default, it uses the one currently opened.


### XSD Generator ###
The XSD generator that Microsoft provides does not embed enough information in the resulting C#.
For this reason, this project has its own code generator which enhanced a lot of types.
For instance, we order properly the items in the XML. We add some length restriction. We add some interfaces.

We marked some properties as deprecated as we found out QuickBooks was emitting a warning when using them. If you find more properties, let us know.

We use a modified version of the XSD provided from QuickBooks; after working on this project, we found that the XSD are not up to date with the latest information.

## Contributing

Contributions are welcome. Code or documentation!

1. Fork this project
2. Create a feature/bug fix branch
3. Push your branch up to your fork
4. Submit a pull request


## License

QuickBooksSync is under the MIT license.
