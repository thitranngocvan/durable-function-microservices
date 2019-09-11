export const appConfig = {
  baseUrl: 'https://carsbookingservices.azurewebsites.net',
  poolingEndpoint: 'runtime/webhooks/durabletask/instances/{{cars_instance_id}}?taskHub=DurableFunctionsHub&connection=Storage&code={{auth_code}}',
  authCode: 'b9yeNc6x2DUcYPWxLzaI0dFIlXPLfARo0I39qNu1jmj4ZsNJhJGiyw==',
};
