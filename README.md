This is a scheduled tasks project written in .Net core

In order to run this project, please follow the following instructions:

1) Clone the repo locally.
2) Mke sure you have .NET core 6.0 installed and dotnet cli. in case not, follow the instructions here: https://docs.microsoft.com/en-us/dotnet/core/install/macos
3) Open it via visual studio and hit the play button or run ```dotnet run --project .\WebApplication1\``` from the root of the project.
4) If you use visual studio - the Swagger ui page will be open so you can use it out of the box. In case you use the other option from the previous line, open postman (or any other tool you're using) and try to use the post and get methods
 
 Example - using Get ```http://localhost:5164/Timers/2```
 
 Example - using Post ```http://localhost:5164/Timers``` -> {
                                                "hours": 4,
                                                "minutes": 3,
                                                "seconds": 1,
                                                "url": "www.some-server.com"
                                               }
                                               
5) Run tests -via command line using the dontnet cli ```dotnet test .\Tests\```
