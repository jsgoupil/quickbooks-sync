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
    var innerRequest = new CustomerQueryRequest();

    // Add some filters here
    innerRequest.MaxReturned = 100;
    innerRequest.FromModifiedDate = new DateTimeType(DateTime.Now);

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
    var rs = response.GetSingleItemFromResponse<CustomerQueryRsTypeWrapper>(xml);

    // Receive customer objects, corresponding to the xml
    var customers = rs.CustomerRet;
  }
}
```

## Web Connector ##
Thanks to the Web Connector, you can communicate with QuickBooks Desktop. Users must download it at the following address: [http://marketplace.intuit.com/webconnector/](http://marketplace.intuit.com/webconnector/ "http://marketplace.intuit.com/webconnector/")

### How does it work? ###
Once downloaded, they must get a QWC file that will connect the Web Connector to your website. To generate a QWC file, use the following method

```C#
/// TODO
```

The Web Connector uses the SOAP protocol to talk with your website, the NuGet package takes care of the heavy lifting to talk with the QuickBooks Desktop. However, you must implement some methods in order to get everything working according to your needs. The Web Connector will come periodically to your website asking if you have any requests to do to its database.
The Web Connector executes the following tasks:

1. Authenticate - Sends the login/password that you must verify. You also return a session ticket that will be used for the rest of messages that are exchanged back and forth.
2. SendRequestXML - The Web Connector expects that you return an XML command that will execute on the database.
3. ReceiveRequestXML - Response regarding the previous step.
4. GOTO Step 2 - Until you return an empty string, indicating that you are done.
5. CloseConnection - Connection is done.

### Database state ###
Multiple "synchronous" connections are done between the Web Connector and your website. There is a need to keep some states with a database. The NuGet package doesn't provide such thing because it doesn't know which database provider/schema you are using; this is where you have to start coding your own implementation.

### Step 1. Create an Authenticator ###
```C#
public interface IAuthenticator
{
  AuthenticatedTicket GetAuthenticationFromLogin(string login, string password);
  AuthenticatedTicket GetAuthenticationFromTicket(string ticket);
}
```

1. GetAuthenticationFromLogin - Authenticate a user from its login/password combination
2. GetAuthenticationFromTicket - Authenticate a ticket previously given from a GetAuthenticationFromLogin call

The AuthenticatedTicket contains 3 properties:

```C#
public class AuthenticatedTicket
{
  public virtual string Ticket { get; set; }
  public virtual string CurrentStep { get; set; }
  public virtual bool Authenticated { get; set; }
}
```

1. Ticket - Exchanged with the Web Connector. It acts as a session identifier.
2. CurrentStep - State indicating at which step the Web Connector is currently at.
3. Authenticated - Simple boolean indicating if the ticket is authenticated.

If a user is not authenticated, make sure to return a ticket value, but set the Authenticated to `false`.

### Step 2. Create a SyncManager ###
Extend the `QbSync.WebConnector.SyncManager` and override only the methods that you really need. You will most likely need a database context, make sure you get it from your constructor. From your constructor, register the steps you want to execute:

```C#
public SyncManager(ApplicationDbContext db_context, IOwinContext owinContext, IAuthenticator authenticator)
  : base(authenticator)
{
  this.db_context = db_context;
  this.owinContext = owinContext;

  RegisterStep(new CustomerQuery(this.db_context));
}
```

**Important methods to override:**

1. SaveChanges - Called before returning any data to the Web Connector. It's time to save data to your database.
2. LogMessage - Data going in or out goes through this method, you can save it to a database in order to better debug.
3. GetWaitTime - Tells the Web Connector to come back in X seconds.

**Other methods you might want to override, but they are handled by the package:**

1. Authenticate - Verifies if the login/password is correct. Returns appropriate message to the Web Connector in order to continue further communication.
2. ServerVersion - Returns the server version.
3. ClientVersion - Indicates which version is the Web Connector. Returns W:<message> to return a warning; E:<message> to return an error. Empty string if everything is fine.
4. SendRequestXML - The Web Connector is asking what has to be done to its database. Return an QbXml command.
5. ReceiveRequestXML - Response from the Web Connector based on the previous command sent.
6. GetLastError - Gets the last error that happened. This method is called only if an error is found.
7. ConnectionError - An error happened with the Web Connector.
8. CloseConnection - Closing the connection. Return a string to show to the user in the Web Connector.


### Step 3. Register your Step Manager with the ASMX ###
The registration allows you to create a StepManager with any dependencies that you would like. Here is an example:

```C#
QbSync.WebConnector.QBConnectorSync.SyncManager = (QbSync.WebConnector.QBConnectorSync qbConnectorSync) =>
{
  return new MyOwn.SyncManager(kernel.Get<ApplicationDbContext>(), qbConnectorSync.Context.GetOwinContext());
};
```

### Step 4. Creating a step ###
By registering a step such as `CustomerQuery`, you can get customers from the QuickBooks database.
Since all steps will require a database manipulation on your side, you have to implement it yourself. But don't worry, it is pretty simple.

Here is an example:
```C#
public class CustomerQuery : StepQueryResponseBase<CustomerQueryRequest, CustomerQueryRsTypeWrapper>
{
  private ApplicationDbContext db_context;

  public CustomerQuery(ApplicationDbContext db_context)
    : base()
  {
    this.db_context = db_context;
  }

  public string GetName()
  {
    // Step name
    return "CustomerQuery";
  }

  protected override bool ExecuteRequest(AuthenticatedTicketContext authenticatedTicket, CustomerQueryRequest request)
  {
    // Do some operations on the customerRequest to get only specific ones
    customerRequest.FromModifiedDate = new DateTimeType(DateTime.Now);

    // Return false if you want to prevent the request to execute and go to the next step.
    return base.ExecuteRequest(authenticatedTicket, request);
  }

  protected override void ExecuteResponse(AuthenticatedTicketContext authenticatedTicket, CustomerQueryRsTypeWrapper response)
  {
    base.ExecuteResponse(authenticatedTicket, response);

    // Execute some operations with your database.
  }
}
```

The 3 generic classes are provided by the QbXml NuGet package. You associate the request, response and the object that would be returned with a response.

### Step 4.1. Creating a step with an iterator ###
When you make a request to the QuickBooks database, you might receive hundreds of objects back. Your server or the database won't be able to handle that many; you have to break the query into batches. We have everything handled for you, but we need to save another state to the database. Instead of deriving from `StepQueryResponseBase`, you have to derive from `StepQueryWithIterator` and implement 2 methods.

```C#
protected abstract void SaveMessage(string ticket, string step, string key, string value);
protected abstract string RetrieveMessage(string ticket, string step, string key);
```

Save the message to the database based on its ticket, `step` and `key`. Then retrieve it from the same keys.

By default, if you derive from the iterator, the query is batched with 100 objects.

### Step X ###
You can register other steps such as `UpdateCustomer`, `InvoiceQuery`, etc. Just make your own!


## Contributing

Contributions are welcome. Code or documentation!

1. Fork this project
2. Create a feature/bug fix branch
3. Push your branch up to your fork
4. Submit a pull request

## License

QuickBooksSync is under the MIT license.
