# Project context
This projet is a technical test for TrueStory. 
It suppose to be a simple RESTful Web API using C# that integrates with the mock API provided at https://restful-api.dev.

It needed to have the following requirements:

- Framework .NET 8.0
- API Methods: Implement at least three API methods:
  - To retrieve data from the mock API with the ability to filter by name(substring) and paging
  - To add data to the mock API
  - To remove data via the mock API
- Validation:
  - Implement proper validation for the model.
  - Ensure all required fields are provided and valid when creating or updating a product.
- Error Handling:
  - Implement proper error handling.

## What this project is about

I decided to create a project based on clean architecture and CQRS because I love those patterns. 
Since I had to call another API, I decided not to use a database engine; all the data are stored in the Mock API. 

As you can see, I use MediatR for the CQRS part, and I have changed my test Framework from XUnit to NUnit 
because I feel more comfortable with it, especially in ordering some of my integration tests, 
it helped to reduce the code base by using an attribute that already exists instead of creating new attribute for it. 

I created this project from scratch, 
but it is based on the same logic and architecture as [Jason Tyler's clean architecture template](https://github.com/jasontaylordev/CleanArchitecture).

I have tried to have good code coverage, which helped me with IA to create some tests. 
However, it may have been a mistake since I don't think those tests are coherent. 
I haven't found the time to create a proper CI pipeline with Github action to add a Sonar and Stryker analysis to ensure good coverage and adequate test quality. 

Of course, it uses Docker to run the project, but this is the base Dockerfile; I haven't touched it yet.

***Another critical point is that I intentionally introduced coupling between my layers for this test.***
I wanted to have a smaller code base, even if it meant having coupling. 
We would have added more abstraction, model mapping, and transformers in an actual project.

## What this project could have been

As I explained, I made those choices because I like this architecture, but I could have done a basic CRUD with minimal API and no dependency injection. 

Or I could have gone way further: adding a caching database to avoid spamming the Mock API, adding authentication,
adding a Postman collection, adding a Docker compose, adding a CI/CD pipeline...
But I think this is not what we are looking for, so I keep it simple. 
