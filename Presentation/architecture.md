# Onion Architeture

The project follows the Onion Architecture, which emphasizes the separation of concerns and promotes a clear dependency rule: code can depend on layers inward but not outward. The main layers are:

1. Domain Layer: Contains the core business logic and domain entities. This layer is independent of any other layers.
2. Application Layer: Contains the application services and interfaces. This layer depends on the Domain Layer.
3. Infrastructure Layer: Contains the implementation of interfaces and external dependencies such as databases and APIs. This layer depends on the Application Layer.
4. Presentation Layer: Contains the user interface and API controllers. This layer depends on the Application Layer.

## Implementation in PollZ

1. Domain Layer: The domain entities such as User, Poll, Question, and Answer are defined in the Model directory.
2. Application Layer: The application services such as UserService, PollService, QuestionService, and AnswerService are defined in the Service directory.
3. Infrastructure Layer: The data context and database migrations are defined in the Data directory. The implementation of repository interfaces is also part of this layer.
4. Presentation Layer: The API controllers such as UserRouter, PollRouter, QuestionRouter, and AnswerRouter are defined in the Router directory. The frontend components and pages are defined in the Frontend directory.
