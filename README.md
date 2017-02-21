# ExeAttacher

## Synopsis

This project is a sample project to test different patterns and good practises in C# like:

- Dependency injection
- Async/await
- C# live analyzers
- MVVM (for wpf version)
- Unit test 
  - mocking
  - code coverage report

There are two different applications offering the same funtionality, a console application and a desktop wpf.

## Motivation

The real functionality of the project is to modify exe files in order to allow them to be attached in an email. 
To achieve this the program does two things:

- change the file extension.
- Remove the **MZ** ["magic number"](https://en.wikipedia.org/wiki/DOS_MZ_executable) from the exe file.

## Tests

Unit test projects can be run from visual studio.
We use [xUnit.net](https://xunit.github.io/) and [Fluent Assertions](http://www.fluentassertions.com/) libraries.
To generate the code coverage results run the batch script code coverage.bat which uses [Open Cover](https://github.com/OpenCover/opencover) + [Report Generator](https://github.com/danielpalme/ReportGenerator).


## Contributors

todo...

## License

todo...