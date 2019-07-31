# Customer Management  

```
mutation q($input:CustomerInput!){
  upsertCustomer(input: $input){
    id
    name
    address
    city
    emailAddress
    postalCode
    telephoneNumber
  }
}
```
```
{
  "input": {
    "id": "a901ec6c-f5af-4521-a712-a13a418f354c",
    "name": "Bugs Bunny",
    "address": "address 123",
    "postalCode": "postalCode 123",
    "city": "city 123",
    "telephoneNumber": "telephoneNumber 123",
    "emailAddress": "emailAddress 123"
    }
}
```
```
query{
  customer(id: "a901ec6c-f5af-4521-a712-a13a418f354c"){
    address
    city
    emailAddress
    id
    name
    postalCode
    telephoneNumber
  }
}
```
