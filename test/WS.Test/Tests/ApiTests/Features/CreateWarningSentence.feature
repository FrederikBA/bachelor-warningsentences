Feature: CreateWarningSentence
	Feature to create warning sentences into the chemicals database

@CreateWarningSentence
Scenario: Create a new warning sentence
	Given an authenticated user is logged into the system
	When the user creates a new warning sentence
	Then the system should store the warning sentence in the database