@ExampleServices
Feature: ExampleServices

Scenario: Get something
	Given I want to GET resource /resourceDeal
	When I execute the rest request
	Then the API response code is OK
	Then the API response contains 'something specific'
	And the response schema matches 'ServicesSchema.ExampleSchema.json'
