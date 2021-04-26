
# EmailAPI

Coding challenge for Clarity Ventures


## API Reference

#### Send E-Mail

```http
  POST /api/mail/send
```

| form-data | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `Recipient` | `text/plain` | **Required**. The "TO" email |
| `Subject` | `text/plain` | **Required**. The subject line |
| `Body` | `text/plain` | **Required**. Body of the E-mail |
| `Attachments` | `application/octet-stream` | Any files you wish to send |


  
## Installation 

Install my-project with npm

```bash 
  npm install my-project
  cd my-project
```

### Windows:

1.) Clone repo locally
```bash 
    git pull https://github.com/BlueLovin/EmailAPI.git
```
2.) Open EmailAPI.sln in Visual Studio 2019

3.) Configure your appsettings.json under the "EmailAPI" project

4.) Build the solution by going to Build -> Build Solution

5.) Ctrl+F5 to start without debugging

6.) Check web server output to see what port the API server is listening on

### Linux/Mac:

1.) Clone repo locally
```bash 
    git pull https://github.com/BlueLovin/EmailAPI.git
```

2.) Use microsoft's script to install dotnet core 3.1.0 runtime
```bash 
./dotnet-install.sh -Runtime aspnetcore --install-dir /usr/share/dotnet -Version 3.1.0
```
3.) Navigate into EmailAPI/
```bash 
cd EmailAPI
```
4.) Configure appsettings.json

5.) Build the solution and run the API server
```bash 
dotnet build
```

```bash 
dotnet run
```

    
## Run React App

Go to the app directory

```bash
  cd frontend
```

Install dependencies

```bash
  npm install
```
```bash
  npm update --force
```

### Change REACT_APP_EMAILAPI_PORT in the ".env" file to the port that API server is listening on. Default: 52413


Start the server

```bash
  npm start
```

  
## Running Tests

To run tests from the command line, run the following command

```bash
  dotnet test
```

  