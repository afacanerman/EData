# EData

Lightweight querybuilder from querystring like Odata - for dotnet core)

How To Use:
 
Chaining Filters:
http://apiUrl.com/api/someactions?filter=equals("{propertyName}", "{text}") and equals("{propertyName}", "{text}")
Take:
http://apiUrl.com/api/someactions?Take=1
Skip:
http://apiUrl.com/api/someactions?Skip=1
Filter:
Contains:
http://apiUrl.com/api/someactions?filter=contains("{propertyName}", "{text}")
StartsWith:
http://apiUrl.com/api/someactions?filter=startswith("{propertyName}", "{text}")
Equals:
http://apiUrl.com/api/someactions?filter=equals("{propertyName}", "{text}")
Filter on Nested properties:
http://apiUrl.com/api/someactions?filter=equals("{Conference.Organiser.Name}", "{text}")
Gt (greater Than):
http://apiUrl.com/api/someactions?filter=gt("{propertyName}", "{value}")
Lt (less Than):
http://apiUrl.com/api/someactions?filter=lt("{propertyName}", "{value}")
Ge (greater than or equal):
http://apiUrl.com/api/someactions?filter=ge("{propertyName}", "{value}")
Le (less than or equal):
http://apiUrl.com/api/someactions?filter=le("{propertyName}", "{value}")
Merging more than one filter with "and"
 http://apiUrl.com/api/someactions?filter=equals("{propertyName}", "{value}") and ge("{propertyName}", "{value}")
Sort / Order by:
http://apiUrl.com/api/someactions?SortBy={propertyName}
http://apiUrl.com/api/someactions?SortBy={propertyName},{anotherPropertyName},{andAnother}...
http://apiUrl.com/api/someactions?SortBy={propertyName}&Desc=true
 

