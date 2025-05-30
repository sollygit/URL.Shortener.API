## [URL Shortener API](https://url-shortener-api.azurewebsites.net)
## [URL Shortener APP](https://url-shortener-app.azurewebsites.net)
## [URL Shortener Angular Demo](https://mockme-app.azurewebsites.net/shortly)

![image](https://github.com/user-attachments/assets/78d74bec-6676-4c37-b09a-77866e7f5a08)

### Technologies:
I used C# .NET8 for the API backend and Angular 8 for the frontend. 
I implemented Entity Framework as my ORM and chose MS SQL Server as relational db engine.
Connection string could be found in appsettings.Development.json
Development Environment: Visual Studio 2022 for back-end and VSCode for the front-end.

### Testing:
Function include at least one successful and one failing unit test.
URL.Shortener.UnitTest project added with some few unit-tests scenarios using Moq package.

### Documentation:
Swaager API description and code-base markdown added.
README.MD file in current repo.
inline code comments added where necessary.

Some basic EF commands used locally to create and seed the DB
```
dotnet ef migrations add Init_DB
dotnet ef database update
```
### Challenge 1 
Describe your approach to designing and implementing a URL shortener service, focusing on key considerations and design choices. 
* I have created 3 endpoints:
    1. Shorten a long URL and store it in DB - POST request.
    2. Get a list of shortened URL from DB - GET request.
    3. Get a shortened URL by unique code from DB - GET request. Return 404 if not found.
* Client A will use the POST endpoint for creating new shortened URL.
* Client B will use the GetByCode endpoint to get the shortened URL.
* ShortenedUrlService is responsible to generate a unique code for each URL and to store it in DB.
* Generate a 7-char alphanumeric code per url. Code length is configured in URLShortenerSettings.cs

### Challenge 2 
Create a UML Sequence Diagram showing how short links will be created by ClientA and accessed by ClientB.
* UML Diagram
![image](https://github.com/user-attachments/assets/609fe8f7-237b-411d-84d7-b69e166d1278)

### Challenge 3 
Develop a class diagram representing the structure of your URL Shortener implementation. 
![image](https://github.com/user-attachments/assets/135ef8af-435f-4983-a0e1-1eda1cc4df1c)

### Challenge 4 
Create an ERD for the database schema supporting the URL shortener service.
* ERD can be found in ShortenedUrl DB
![image](https://github.com/user-attachments/assets/002b57a0-fc6c-4062-9915-be2c86be3f34)
![image](https://github.com/user-attachments/assets/be349241-dd42-43f9-b526-52b90d7333fb)

### Challenge 5 
Code the URL shortener service based on your design in C#. Ensure that it is functional and can be demonstrated on your local device. 
* ShortenedUrlService.cs

### Challenge 6 
Develop a user interface in Angular that displays a list of shortened links, demonstrating your frontend development skills.
* Angular SPA created to display list of URL items from DB

### Challenge 7 
(Bonus) Deployment: For extra credit, create a Kubernetes or Docker Compose file to deploy your solution, showcasing your knowledge of containerization and deployment.
* Both back-end API and Angular app have been deployed to my Azure instance.
* The shortener url list have been integrated into my existing MockMe angular app and can be accessed from the main menu.
* https://mockme.azurewebsites.net/shortly
