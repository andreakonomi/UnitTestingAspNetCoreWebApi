To Test private methods first advice would be to not do it at all directly.
You should test it as scenario of a bigger public method that calls this one.

Making it public just to test would break encapsulation of the class which is not worth it.

A slightly less bad approach is to use [InternalsVisible] so that is visible by the tests assembly

## Setting up tests and Sharing Test Context
- On constructor, and its is created and disposed on Dispose method for every test for that class
- Class Fixture, create test context once for class and share across all tests
  - Be aware the dependencies should be stateless and not be affected from test to test
- Collection Fixture have one test context across all test classes