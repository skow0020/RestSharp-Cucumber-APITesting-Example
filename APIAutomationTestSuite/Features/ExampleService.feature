@DataServices
Feature: DataServices

Scenario: Get Claims
	Given I want to GET resource /Claims/201903101735
	When I execute the rest request
	Then the API response code is OK
	Then the API response contains '201903101735'
	And the response schema matches 'DataServicesSchema.GetClaimsSchema.json'

Scenario: GetMemberKits
	Given I want to GET resource /members/00091632500/memberkits
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '00091632500'
	And the response schema matches 'DataServicesSchema.GetMemberKitsSchema.json'

Scenario: GetMemberKitsByStartDate
	Given I want to GET resource /members/304059300/memberkits
	When I add parameters
	| name      | value    |
	| StartDate | 20181215 |
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '304059300'
	And the response schema matches 'DataServicesSchema.GetMemberKitsSchema.json'

Scenario: GetMemberKits_ByEndDate
	Given I want to GET resource /members/304059300/memberkits
	When I add parameters
	| name    | value    |
	| EndDate | 20181218 |
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '304059300'
	And the response schema matches 'DataServicesSchema.GetMemberKitsSchema.json'

Scenario: GetMemberKits_Valid_StartDate_EndDate_MemberNbr_Blank
	Given I want to GET resource /members//memberkits
	When I add parameters
	| name      | value    |
	| StartDate | 20181215 |
	| EndDate   | 20181227 |
	When I execute the rest request
	Then the API response code is InternalServerError

Scenario: GetMemberKits_InValid_StartDate_EndDate
	Given I want to GET resource /members/304059300/memberkits
	When I add parameters
	| name      | value   |
	| StartDate | 2018121 |
	| EndDate   | 2018122 |
	When I execute the rest request
	Then the API response code is InternalServerError

Scenario: GetMemberKits_Valid_StartDate_EndDate
	Given I want to GET resource /members/304059300/memberkits
	When I add parameters
	| name      | value    |
	| StartDate | 20181215 |
	| EndDate   | 20190606 |
	When I execute the rest request
	Then the API response code is OK
	And the response schema matches 'DataServicesSchema.GetMemberKitsSchema.json'

Scenario: GetMemberKits_Same_StartDate_EndDate
	Given I want to GET resource /members/304059300/memberkits
	When I add parameters
	| name      | value    |
	| StartDate | 20181215 |
	| EndDate   | 20181215 |
	When I execute the rest request
	Then the API response code is OK
	And the response schema matches 'DataServicesSchema.GetMemberKitsSchema.json'

Scenario: CreateMemberKit
	Given I want to POST resource /members/memberkits
	When I provide the JSON request object
	"""
	{
	  "MemberNbr":"304568300",
	  "KitRequestor":"created by Fuse",
	  "KitStatus":"N",
	  "KitType":"B", 
	  "KitExpediteFlag":"N", 
	  "ReasonCode":"10",
	 }
	"""
	And I execute the rest request
	Then the API response code is OK
	And the API response contains '304568300'
	And the response schema matches 'DataServicesSchema.CreateMemberKitSchema.json'
	And the CreateMemberKit response can be deserialized

Scenario: FindAssociatedMembers_CaseManagers
	Given I want to GET resource /members/00010115000/CaseManagers
	When I add parameters
	| name      | value       |
	| SortBy    | FirstName:A |
	And I execute the rest request
	Then the API response code is OK
	And the API response contains 'CaseDetail'
	And the response schema matches 'DataServicesSchema.GetCaseManagersSchema.json'

Scenario: FindMemberAuthorizations
	Given I want to GET resource /members/00091632500/Authorizations
	When I execute the rest request
	Then the API response code is OK
	And the API response contains 'Authorization'
	And the response schema matches 'DataServicesSchema.GetAuthorizationsSchema.json'

Scenario: FindMemberBrokers
	Given I want to GET resource /members/00091632500/Brokers
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '00091632500'
	And the response schema matches 'DataServicesSchema.GetBrokersSchema.json'

Scenario: FindMemberClaims
	Given I want to GET resource /members/00091632500/Claims
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '00091632500'
	And the response schema matches 'DataServicesSchema.GetMemberClaimsSchema.json'

Scenario: FindMemberPolicies
	Given I want to GET resource /members/00091632500/policies
	When I execute the rest request
	Then the API response contains '000916325'
	And the response schema matches 'DataServicesSchema.GetPoliciesSchema.json'

Scenario: FindMemberPolicies_InvalidMemberId
	Given I want to GET resource /members/3039911/policies
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0007'

Scenario: FindMemberPolicies_Invalid_AlphaNumeric_MemberID
	Given I want to GET resource /members/303991100b/policies
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0007'

Scenario: FindMemberPolicies_Invalid_Using_InvalidMember_Exact_9_Digits
	Given I want to GET resource /members/345554567/policies
	When I execute the rest request
	Then the API response code is OK
	And the response contains json warning code '404-0004'

Scenario: FindMemberPolicies_Invalid_Using_Blank_MemberNbr
	Given I want to GET resource /members//policies
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0004'

Scenario: Findmember
	Given I want to GET resource /members
	When I add parameters
	| name      | value       |
	| SortBy    | FirstName:A |
	| FirstName | mark        |
	| LastName  | Oakland     |
	And I execute the rest request
	Then the API response code is OK
	And the API response contains 'Mark'
	And the response schema matches 'DataServicesSchema.GetMembersSchema.json'

Scenario: GetPolicies_By_Id
	Given I want to GET resource /policies/3039915
	When I execute the rest request
	Then the API response code is OK
	 And the API response contains '3039915'
	And the response schema matches 'DataServicesSchema.GetPoliciesByIdSchema.json'

Scenario: GetPolicies_By_Id_PolicyNumLessThan7
	Given I want to GET resource /policies/303990
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0002'

Scenario: GetPolicies_By_Id_PolicyNumGreaterThan7
	Given I want to GET resource /policies/30399000
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0002'

Scenario: GetPolicies_By_Id_PolicyNumAlphaNumeric
	Given I want to GET resource /policies/427883ab
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0002'

Scenario: GetPolicies_By_Id_StatusFutureActive
	Given I want to GET resource /policies/3045752
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '3045752'
	And the response schema matches 'DataServicesSchema.GetPoliciesByIdSchema.json'

Scenario: GetPolicies_By_Id_Different values for BenefitCode_PlanCode_DivisionCode
	Given I want to GET resource /policies/3028846
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '3028846'
	And the response schema matches 'DataServicesSchema.GetPoliciesByIdSchema.json'

Scenario: GetPolicies_By_Id_StatusInactive
	Given I want to GET resource /policies/3028846
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '3028846'
	And the response schema matches 'DataServicesSchema.GetPoliciesByIdSchema.json'

Scenario: GetPolicies_By_Id_InvalidPolicyNum
	Given I want to GET resource /policies/3345545
	When I execute the rest request
	Then the API response code is OK
	And the response contains json warning code '404-0004'

Scenario: GetPolicies_By_Id_BlankPolicyNum
	Given I want to GET resource /policies/
	When I execute the rest request
	Then the API response code is InternalServerError

Scenario: GetMember_By_Id
	Given I want to GET resource /members/303991100
	When I execute the rest request
	Then the API response code is OK
	And the API response contains '303991100'
	And the response schema matches 'DataServicesSchema.GetMemberByIdSchema.json'

Scenario: GetMember_By_Id_InValidMemberNbr_Exactly_9_Digits
	Given I want to GET resource /members/302793420
	When I execute the rest request
	Then the API response code is OK
	And the response contains json warning code '404-0004'

Scenario: GetMember_By_Id_MemberNbr_Empty
	Given I want to GET resource /members/
	When I execute the rest request
	Then the API response code is InternalServerError

Scenario: GetMember_By_Id_InValidMemberNbr_LessThan_9_Digits
	Given I want to GET resource /members/30279342
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0004'

Scenario: GetMember_By_Id_InValidMemberNbr_MoreThan_9_Digits
	Given I want to GET resource /members/3027934223
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0004'

Scenario: FindAssociatedMembers
	Given I want to GET resource /members/302794800/associated
	When I execute the rest request
	Then the API response code is OK
	And the response schema matches 'DataServicesSchema.GetMemberAssociatedSchema.json'

Scenario: FindAssociatedMembers_Invalid_Using_InvalidMember_Exact_9_Digits
	Given I want to GET resource /members/302793456/associated
	When I execute the rest request
	Then the API response code is NOTFOUND
	And the response contains json error code 'E-404-0007'