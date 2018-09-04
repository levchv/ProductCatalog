# ProductCatalog

#Database deployment

open ProductCatalog.Database.sln solution
deploy ProductCatalog.Database.Sql.sqlproj project to your SQL database or execute Products.sql script directly on it


#WebApi Configuration

in appsettings.json file add (if not exists) or edit section "Database":

  "Database": {
	  "ConnectionString": "<connection string to your database>"
  }

please remember to replace "<connection string to your database>" with your SQL database connetion string
Example: "Password=Password;User ID=sa;Initial Catalog=ProductCatalog;Data Source=localhost"

to see swagger API Documentation open link: <web api hostname>/swagger
Example: https://localhost:44380/swagger


#WebClient Configuration

in appsettings.json file add (if not exists) or edit section "WebApiClient":
  "WebApiClient": {
	  "Url": "<url to web api (incuding version)>"
  }

please remember to replace "<url to web api (incuding version)>" with your Web Api url
Example: "https://localhost:44380/api/v1/"