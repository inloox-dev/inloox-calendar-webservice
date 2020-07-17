# InLoox Calendar Webservice

a webserive that reads data from InLoox.ODataClient nuget package https://www.nuget.org/packages/InLoox.ODataClient/ and outputs an ICS calender feed through https://github.com/rianjs/ical.net

## Setup

1. Create an InLoox now! account here: https://app.inlooxnow.com/Account/CreateAndLogin
2. Create at least one task with a start date and an end date
3. Obtain a token, go to: https://app.inlooxnow.com/tests/oauth2
<space><space>Click the "Authorize" button.
<space><space>Copy the value of the "Access Token" field.
4. Run the code in debug mode in a local environment
5. Navigate to: https://localhost:5001/my-tasks/?access_token={INSERT_ACCESS_TOKEN_HERE}
   Replace ***{INSERT_ACCESS_TOKEN_HERE}*** by the access token you copied in step 3.
6. You should see an ICS feed in your browser

**Important note:** Sharing an access token is equivalent to sharing username and password for an account.

## Deployment

### Server Deploy

You can either deploy the application internally to an Internet Information Server or to an Azure App Service.
<space><space>Locally: https://docs.microsoft.com/de-de/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.1
<space><space>Cloud: https://docs.microsoft.com/de-de/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.1

**Important note:** SSL/TLS is highly recommended

### User Deploy

To import the feed into Outlook, your users must first obtain a token (see step 2 in Setup section) and then navigate to: webcal://{INSERT_WEBSERIVCE_URL_HERE}/my-tasks/?access_token={INSERT_ACCESS_TOKEN_HERE}

## Local Server (InLoox PM Enterprise Server)

To connect to a local InLoox service, update the endpoint to InLoox [endPoint](https://github.com/inloox-dev/inLoox-calendar-webservice/blob/05b33462cd4d77e85d8398dfaebc3bdb2bb77ae5/InLooxCalendarWebservice/Startup.cs#L12) to reflect your InLoox PM Enterprise Server url