
![image](https://user-images.githubusercontent.com/66834055/130873494-3af1a296-cb60-4364-9a4c-f806db226954.png)

Simple Pokedex REST API, built using ASP .NET Core and AWS Lambda!

### Description

The PokeFun API contains two endpoints:

- `pokemon/{name}` : The endpoint will send a GET request to the [PokeApi](https://pokeapi.co/), which will respond with standard information regarding the pokemon `{name}`. Since we are not interested in most of the resources it returns, we expose a simple Model that contains only four fields: The `Name` of the Pokemon, the `Habitat` where it lives, a `Description` of the Pokemon, and finally if `Is_Legendary` or not. 

- `pokemon/translated/{name}`: This endpoints returns the same fields as the previous endpoint, but with the description "translated" - If it's a legendary pokemon, or its habitat is "cave", then it performs a Yoda translation. Otherwise, it will perform a Shakespeare translation. This is done using the [yoda translator](https://funtranslations.com/api/yoda) and the [shakespeare translator](https://funtranslations.com/api/shakespeare). 

> :warning: **WARNING**: To maintain the service level, the funtranslation API ratelimits the number of API calls to only 60 API calls a day with distribution of 5 calls an hour. Therefore, if you exceeded the number of API calls, you don't get a translation at all - it simply returns the standard description.


### How to use

Easier than ever - simply use the following URL and add the API endpoints defined above!:

https://kaioq8froi.execute-api.eu-west-2.amazonaws.com/Prod

(I know - the URL is a bit ugly, maybe we could pay for a domain in Route53, but that means ... 💸💸💸). 

**_Example Endpoint I_**:

Request:

https://kaioq8froi.execute-api.eu-west-2.amazonaws.com/Prod/pokemon/mewtwo

Response body:

```json
{ 
  "habitat": "rare", 
  "is_legendary": true, 
  "name": "mewtwo", 
  "description": "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments." 
}
```

**_Example Endpoint II_: Yoda Translation**:

Request:

https://kaioq8froi.execute-api.eu-west-2.amazonaws.com/Prod/pokemon/translated/mewtwo

Response body:

```json
{
  "habitat": "rare",
  "is_legendary": true,
  "name": "mewtwo",
  "description": "Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was."
}
```

**_Example Endpoint II_: Shakespeare Translation**:

Request:

https://kaioq8froi.execute-api.eu-west-2.amazonaws.com/Prod/pokemon/translated/geodude

Response body:

```json
{
  "habitat": "mountain",
  "is_legendary": false,
  "name": "geodude",
  "description": "Did find in fields and mountains. Mistaking those folk for boulders, people oft grise or trippeth on those folk."
}
```

### How to build (Developers)

The 

### Future Improvements

Obviously, the project can be improved quite a lot in different areas. And that's cool, because it means room for improvement!

I wrote main ideas inside the (Project)[] window in Github, but I'll list them here too:


